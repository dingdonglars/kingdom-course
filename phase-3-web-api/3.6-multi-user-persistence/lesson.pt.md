# Módulo 3.6 — Persistência Multi-Usuário

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje cada reino pertence a um *usuário*. O `sub` do usuário logado é salvo junto com cada reino. `GET /kingdoms` retorna *só os seus* reinos — nunca os de outra pessoa. Isso é o que transforma a API de uma pilha de dados compartilhada em um produto real com usuários separados.

A mudança é pequena em linhas de código. Ela é muito importante mesmo assim. Dados multi-usuário é a parte de qualquer app web onde os bugs se escondem com mais facilidade, e esses bugs são silenciosos: um usuário vê os dados de outro, ou os muda, e ninguém nota por semanas. O conserto é uma única cláusula `WHERE`, e o custo de esquecer é enorme. A gente vai construir esse hábito agora, enquanto a base de código ainda é pequena o suficiente para entender do começo ao fim.

> **Words to watch**
>
> - **owner** — o usuário que criou o recurso (usamos o claim `sub` do Google)
> - **authorisation** vs. **authentication** — auth*entication* é *quem é você*; auth*orisation* é *o que você pode fazer*
> - **scoped query** — cada cláusula `WHERE` inclui `OwnerSub = usuárioAtual`, para um usuário não poder ler ou mudar dados de outro
> - **migration with data preservation** — quando você adiciona uma coluna não-nula a uma tabela que já tem linhas, precisa dar um valor padrão

---

## Por que isso importa mais do que parece

Esse é o tipo de bug que vira notícia quando dá errado. A versão clássica: um usuário digita `/kingdoms/1234` na barra de endereço e recebe de volta o reino de outra pessoa. O conserto é uma cláusula `WHERE OwnerSub = ?`. Deixar essa cláusula de fora é exatamente o que deixa o bug acontecer. A gente constrói o hábito agora para que se torne automático.

## O que vem no starter

- **MODIFICADO:** `Kingdom.Persistence/EfCore/KingdomEntity.cs` — adiciona `string OwnerSub` (como chave estrangeira, indexada)
- **NOVA migration:** `dotnet ef migrations add AddOwnerSub` (você roda isso)
- **MODIFICADO:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — cada método recebe `ownerSub` e limita a consulta
- **MODIFICADO:** `Kingdom.Api/Program.cs` — lê `sub` do cookie de auth e passa para o store
- **NOVO:** `tests/Kingdom.Persistence.Tests/MultiUserTests.cs`

## Passo 1 — entidade recebe `OwnerSub`

```csharp
public class KingdomEntity
{
    public int Id { get; set; }
    public string OwnerSub { get; set; } = "";    // NOVO
    public string Name { get; set; } = "";
    // ... resto sem mudança
}
```

Em `KingdomDbContext.OnModelCreating(...)`, adicione um índice para a busca:

```csharp
protected override void OnModelCreating(ModelBuilder model)
{
    model.Entity<KingdomEntity>().HasIndex(k => k.OwnerSub);
}
```

Depois gere a migration:

```powershell
dotnet ef migrations add AddOwnerSub --project Kingdom.Persistence --startup-project Kingdom.Console
```

A migration gerada adiciona a coluna. Para linhas que já existem, o EF vai usar o padrão `""`, que significa que não terão dono. Uma migration de produção real preencheria a coluna para as linhas antigas ou recusaria rodar até você ter um plano para elas. Para o nosso DB de aprendizado, o padrão está bem.

## Passo 2 — métodos do store recebem `ownerSub`

Cada método público recebe um parâmetro `string ownerSub` e o adiciona à consulta. Note o padrão LINQ de quebrar a linha antes do ponto quando a cadeia tem três ou mais métodos:

```csharp
public int Save(string ownerSub, Kingdom.Engine.Kingdom kingdom)
{
    EnsureCreated();
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = new KingdomEntity
    {
        OwnerSub = ownerSub,            // <-- a única linha nova no Save
        Name = kingdom.Name,
        // ... resto sem mudança
    };
    ctx.Kingdoms.Add(entity);
    ctx.SaveChanges();
    return entity.Id;
}

public Kingdom.Engine.Kingdom Load(string ownerSub, int id, IRandom rng, IClock clock)
{
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = ctx.Kingdoms
        .Include(k => k.Buildings)
        .Single(k => k.Id == id && k.OwnerSub == ownerSub);   // <-- busca limitada
    // ... resto sem mudança
}

public IReadOnlyList<KingdomSlotInfo> ListSlots(string ownerSub)
{
    using var ctx = new KingdomDbContext(_dbPath);
    return ctx.Kingdoms
        .AsNoTracking()
        .Where(k => k.OwnerSub == ownerSub)
        .OrderBy(k => k.Id)
        .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
        .ToList();
}

// Mesmo padrão para Update e Delete: WHERE Id = id AND OwnerSub = ownerSub
```

O padrão que mantém os bugs fora: **`ownerSub` é um parâmetro obrigatório em cada método.** Um caller que esquecer recebe um erro de compilação, não um bug de segurança. Não faça opcional.

## Passo 3 — extrair `ownerSub` no `Program.cs`

```csharp
static string GetOwnerSub(HttpContext ctx)
{
    var sub = ctx.User.FindFirst("sub")?.Value
           ?? ctx.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    return sub ?? throw new InvalidOperationException(
        "No sub claim — request is unauthenticated.");
}

group.MapGet("/", (HttpContext ctx) => store.ListSlots(GetOwnerSub(ctx)));

group.MapPost("/", (CreateKingdomRequest req, HttpContext ctx, ILogger<Program> log) =>
{
    if (string.IsNullOrWhiteSpace(req.Name))
        return Results.BadRequest(new { error = "Name is required." });
    var ownerSub = GetOwnerSub(ctx);
    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(ownerSub, k);
    log.LogInformation("Created kingdom {KingdomId} '{KingdomName}' for {OwnerSub}", id, k.Name, ownerSub);
    return Results.Created($"/kingdoms/{id}", new KingdomCreated(id, k.Name));
});

// Mesmo padrão para Load/Tick/Delete: leia o sub, passe para o store. Sempre limitado.
```

## Passo 4 — testes multi-usuário

`tests/Kingdom.Persistence.Tests/MultiUserTests.cs`:

```csharp
[Fact]
public void Save_ScopedToOwner_OtherUserCannotSee()
{
    var path = Path.Combine(Path.GetTempPath(), $"mu-{Guid.NewGuid():N}.db");
    try
    {
        var store = new KingdomEfStore(path);
        store.Save("alice", new global::Kingdom.Engine.Kingdom("AliceVille"));
        store.Save("bob",   new global::Kingdom.Engine.Kingdom("BobsTown"));

        store.ListSlots("alice").Single().Name.ShouldBe("AliceVille");
        store.ListSlots("bob").Single().Name.ShouldBe("BobsTown");
        store.ListSlots("eve").ShouldBeEmpty();   // usuário desconhecido não vê nada
    }
    finally { if (File.Exists(path)) File.Delete(path); }
}

[Fact]
public void Load_OfOtherUsersKingdom_Throws()
{
    var path = Path.Combine(Path.GetTempPath(), $"mu-{Guid.NewGuid():N}.db");
    try
    {
        var store = new KingdomEfStore(path);
        var aliceId = store.Save("alice", new global::Kingdom.Engine.Kingdom("AliceVille"));

        Should.Throw<InvalidOperationException>(() =>
            store.Load("bob", aliceId, new SystemRandom(0), new SystemClock()));
    }
    finally { if (File.Exists(path)) File.Delete(path); }
}
```

O segundo teste é o que pegaria o bug. Sempre escreva um teste para o caso de usuário cruzado.

## Mexa um pouco

Faça login como Usuário A e crie um reino. Desconecte. Faça login como Usuário B (uma conta Google diferente). `GET /kingdoms` está vazio. Tente `GET /kingdoms/<id_reino_usuarioA>` — você recebe um 404. A busca limitada não encontra nada, o que é tratado como não-encontrado.

Remova a cláusula `&& k.OwnerSub == ownerSub` do `Load`. Rode os testes — o teste de usuário cruzado falha. Ponha a cláusula de volta. Isso é o teste fazendo seu trabalho.

Adicione um campo booleano `IsPublic` para que jogadores possam compartilhar reinos somente leitura. Agora a consulta limitada vira `OwnerSub == ownerSub OR IsPublic == true`. O mesmo hábito, com uma exceção cuidadosa.

## O ponto principal

A segurança multi-usuário pertence à camada de dados, não à UI. A UI pode esconder botões, mas se a API não limita as consultas, qualquer pessoa com `curl` pode ler os dados de qualquer um. A cláusula `WHERE OwnerSub = ?` é a proteção real.

## O que você acabou de fazer

Você transformou a API de *um grande banco de dados de reino compartilhado* para *cada usuário vê só seus próprios reinos*. A mudança foi uma nova coluna (`OwnerSub`), um novo índice, e uma cláusula `WHERE OwnerSub = ?` em cada leitura e escrita. O padrão que mantém os bugs fora: faça `ownerSub` um parâmetro obrigatório em cada método do store, para um caller que esquecer receber um erro de compilação em vez de um bug de segurança silencioso que ninguém nota por semanas. O teste de usuário cruzado que você escreveu — *Load do reino de outro usuário lança exceção* — é o que mais importa. É o teste que pegaria uma cláusula `WHERE` faltando antes de ir ao ar. Dois novos testes, oitenta e tantos passando no total.

**Conceitos que você já sabe nomear:**

- **owner** — o usuário que possui um recurso; usamos o claim `sub` do Google
- **scoped query** — cada leitura/escrita filtra pelo dono
- **authorisation** — o que você pode fazer (vs. authentication = quem você é)
- **`HasIndex(k => k.OwnerSub)`** — mantém a busca por dono rápida conforme os dados crescem
- **o teste de usuário cruzado** — o teste que prova seu valor quando alguém refatora

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — escreva uma busca limitada da sua própria cabeça. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Imagine um método `Load` no store que encontra um reino pelo `id`. Sem olhar:

1. Escreva a assinatura do método e a busca com `Single(...)` para que um usuário *nunca* possa carregar o reino de outro usuário.
2. Depois diga por que `ownerSub` é um parâmetro obrigatório, não um opcional.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
public Kingdom.Engine.Kingdom Load(string ownerSub, int id, IRandom rng, IClock clock)
{
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = ctx.Kingdoms
        .Include(k => k.Buildings)
        .Single(k => k.Id == id && k.OwnerSub == ownerSub);   // busca limitada
    // ... construir o reino a partir da entidade ...
}
```

A parte `&& k.OwnerSub == ownerSub` é a proteção real. Sem ela, qualquer pessoa poderia ler os dados de qualquer um só adivinhando um id. `ownerSub` é obrigatório, não opcional, para que um caller que esquecer receba um erro de compilação — não um bug de segurança silencioso que ninguém nota por semanas.

</details>

## Movimento git da semana — resolver um conflito de merge

Mais cedo ou mais tarde, duas branches mudam as mesmas linhas e o git não consegue fazer o merge sozinho. Você vai ver a mensagem: *"Conflicts must be resolved."*

No VS Code: abra o arquivo com o conflito. Cada *hunk* conflitante (um bloco de linhas alteradas próximas) mostra botões no editor — *Accept Current Change*, *Accept Incoming Change*, *Accept Both Changes*, *Compare Changes*. Clique o que é certo para cada hunk. Depois de lidar com cada conflito, o arquivo não está mais marcado como conflitante. Prepare-o, faça commit (a mensagem de commit já está preenchida com *"Merge branch ..."*), e dê push.

> **Ou no terminal:** conflitos aparecem como marcadores `<<<<<<<` / `=======` / `>>>>>>>` no arquivo. Edite o arquivo à mão para manter a versão que quer, delete os marcadores, rode `git add <arquivo>`, depois `git commit` (ou `git rebase --continue`).

O hábito para construir: **leia as duas versões antes de escolher uma.** Clicar em aceitar sem ler é como bugs silenciosos entram no merge.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.6 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

## Próximo

O Módulo 3.7 apresenta **testes de integração** com `WebApplicationFactory<Program>`. HTTP real, auth real, DB real — tudo escrito como testes e verificado automaticamente. Isso é o que deixa você refatorar com confiança, em vez de clicar por cada endpoint à mão.
