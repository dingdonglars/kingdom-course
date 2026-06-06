# Módulo 3.3 — Roteamento, Status Codes e CRUD Multi-Reino

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Até agora a API tinha um reino em memória. Hoje a API lida com muitos reinos, salvos pelo EF store da Fase 2. `POST /kingdoms` cria um. `GET /kingdoms` lista todos. `GET /kingdoms/{id}` lê um específico. `DELETE /kingdoms/{id}` remove um. Isso é **CRUD via HTTP** — *criar, ler, atualizar, deletar* — o padrão por trás de toda API web já construída.

A outra metade de hoje é usar o *status code certo* para cada ação. 201 quando um `POST` cria algo. 404 quando a coisa não está lá. 204 quando um delete funciona. Os clientes tomam decisões com base no status code, então retornar o errado não é um detalhe pequeno. É um bug real que o cliente vai tropeçar.

> **Words to watch**
>
> - **route parameter** — `{id}` no caminho, combinado com um argumento do método
> - **`MapGroup`** — agrupa rotas que compartilham o início do caminho, como uma pasta para endpoints
> - **REST conventions** — regras acordadas que todo mundo segue sobre qual verbo, caminho, e status code ficam juntos
> - **`Created` (201)** — o status certo para um `POST` bem-sucedido que criou algo novo; inclui um cabeçalho `Location` com a URL da nova coisa

---

## Convenções REST, em uma tabela

| Ação | Verbo | Caminho | Sucesso | Falha |
|---|---|---|---|---|
| Listar | `GET` | `/kingdoms` | 200 + array | (raro) |
| Ler um | `GET` | `/kingdoms/{id}` | 200 + objeto | 404 |
| Criar | `POST` | `/kingdoms` | 201 + cabeçalho Location + novo objeto | 400 (entrada ruim) |
| Atualizar | `PUT` | `/kingdoms/{id}` | 200 + atualizado, ou 204 No Content | 404 / 400 |
| Deletar | `DELETE` | `/kingdoms/{id}` | 204 No Content | 404 |

Quando você segue essas convenções, qualquer desenvolvedor de cliente pode adivinhar a URL só pelo verbo e o nome da coisa. As convenções tornam uma API mais fácil de ler. A próxima equipe que usar sua API tem menos para aprender, porque já sabe o que esperar.

## O que vem no starter

Hoje a API muda de *um reino em memória* para *muitos reinos, salvos pelo EF store da Fase 2*.

- **NOVO:** `Kingdom.Api/Dtos/CreateKingdomRequest.cs` e `KingdomCreated.cs`
- **MODIFICADO:** `Kingdom.Api/Program.cs` — usa `KingdomEfStore` mais `MapGroup("/kingdoms")` e cinco endpoints
- **MODIFICADO:** `Kingdom.Api/Kingdom.Api.csproj` — já referencia Persistence (do Módulo 3.1)

## Passo 1 — DTOs de requisição e resposta

`Dtos/CreateKingdomRequest.cs`:

```csharp
namespace Kingdom.Api.Dtos;

public record CreateKingdomRequest(string Name);
```

`Dtos/KingdomCreated.cs`:

```csharp
namespace Kingdom.Api.Dtos;

public record KingdomCreated(int Id, string Name);
```

Os dois são pequenos e claros. A requisição diz exatamente o que o cliente envia. A resposta diz exatamente o que vem de volta.

## Passo 2 — conectar `KingdomEfStore` à API

```csharp
using Kingdom.Api.Dtos;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Kingdom.Persistence.EfCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Um arquivo DB por processo. (O Módulo 3.6 muda isso para um caminho configurável.)
var dbPath = Path.Combine(AppContext.BaseDirectory, "kingdoms.db");
var store = new KingdomEfStore(dbPath);
store.EnsureCreated();
IRandom rng = new SystemRandom();
IClock clock = new SystemClock();

// Todos os endpoints de reino em /kingdoms
var group = app.MapGroup("/kingdoms");

// LISTAR — GET /kingdoms
group.MapGet("/", () => store.ListSlots());

// LER UM — GET /kingdoms/{id}
group.MapGet("/{id:int}", (int id) =>
{
    try
    {
        var k = store.Load(id, rng, clock);
        return Results.Ok(KingdomJsonStore.ToSummary(k));
    }
    catch (InvalidOperationException)
    {
        return Results.NotFound(new { error = $"No kingdom with id {id}." });
    }
});

// CRIAR — POST /kingdoms  body: { "name": "..." }
group.MapPost("/", (CreateKingdomRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Name))
        return Results.BadRequest(new { error = "Name is required." });

    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(k);
    return Results.Created($"/kingdoms/{id}", new KingdomCreated(id, k.Name));
});

// TICK — POST /kingdoms/{id}/tick?days=N
group.MapPost("/{id:int}/tick", (int id, int? days) =>
{
    var n = Math.Clamp(days ?? 1, 1, 100);
    Kingdom.Engine.Kingdom k;
    try { k = store.Load(id, rng, clock); }
    catch (InvalidOperationException) { return Results.NotFound(); }

    for (int i = 0; i < n; i++) k.AdvanceDay();
    store.Update(id, k);
    return Results.Ok(KingdomJsonStore.ToSummary(k));
});

// DELETAR — DELETE /kingdoms/{id}
group.MapDelete("/{id:int}", (int id) =>
{
    store.Delete(id);
    return Results.NoContent();    // 204 — mesmo se nada existia; idempotente
});

app.Run();

public partial class Program { }
```

Cinco coisas para ler com cuidado:

1. **`MapGroup("/kingdoms")`** — todo endpoint adicionado ao `group` começa com `/kingdoms`. Isso mantém as strings de caminho curtas.
2. **`{id:int}`** — um parâmetro de rota que aceita só um número inteiro. `/kingdoms/abc` não vai combinar (não é um int); `/kingdoms/5` combina, com `id = 5`.
3. **`Results.Created(uri, value)`** — a resposta certa para um `POST` bem-sucedido. Define o status 201 *e* o cabeçalho `Location: /kingdoms/5`, para o cliente saber a URL da nova coisa.
4. **`Results.NoContent()`** — 204 — a ação funcionou, e não tem nada para enviar de volta. Essa é a resposta normal para `DELETE`.
5. **`try/catch (InvalidOperationException)`** — `store.Load` lança exceção se o registro não está lá, e a gente transforma isso em 404. Isso *não é ótimo* — estamos usando uma exceção para guiar o fluxo normal do programa. O Módulo 3.4 vai adicionar um `TryLoad` para poder lidar com um registro faltando sem exceção.

## Passo 3 — tente tudo

```powershell
dotnet run --project Kingdom.Api
# em outro terminal
curl http://localhost:5xxx/kingdoms                                                     # []
curl -X POST http://localhost:5xxx/kingdoms -H "Content-Type: application/json" -d '{"name":"Eldoria"}'
# Resposta: 201 Created, Location: /kingdoms/1, body { "id":1, "name":"Eldoria" }
curl http://localhost:5xxx/kingdoms                                                     # [ {id:1, ...} ]
curl http://localhost:5xxx/kingdoms/1                                                   # resumo completo
curl -X POST "http://localhost:5xxx/kingdoms/1/tick?days=5"
curl -X DELETE http://localhost:5xxx/kingdoms/1                                         # 204 No Content
curl -i http://localhost:5xxx/kingdoms/1                                                # 404 agora
```

Todo status code da tabela aparece.

## Mexa um pouco

Tente `POST /kingdoms` com `{"name": ""}`. Você recebe um 400 Bad Request da sua própria verificação. Sem body nenhum, você também recebe um 400 — esse vem do framework. Entre sua verificação e a do framework, os casos ruins óbvios estão cobertos.

Adicione `app.MapGet("/kingdoms/{id:int}/buildings", ...)` retornando os prédios de um reino.

Use `MapDelete` em `/kingdoms` (sem id) para deletar *todos* os reinos — cuidado! A maioria das APIs exige uma flag explícita `?confirm=yes` antes de deletar um grupo inteiro de coisas.

Rode dois `POST /kingdoms` um depois do outro. Veja os ids contando sozinhos nas respostas.

## O ponto principal

Status codes fazem parte da sua API. Retornar o errado não é um detalhe pequeno — é um bug real que o cliente vai tropeçar. Os clientes tomam decisões com base no status code. Um `200 OK` com `{ "error": "not found" }` confunde todo mundo que usa sua API. Use 201 para criações, 204 para deleções, 404 para coisas que não estão lá, 400 para entradas ruins. As convenções existem para que qualquer equipe de cliente possa adivinhar o comportamento certo sem ler o seu código.

## O que você acabou de fazer

Você moveu a API de um reino em memória para muitos reinos no banco de dados, com um conjunto completo de endpoints CRUD. Você usou `MapGroup("/kingdoms")` para cinco handlers compartilharem o início do caminho, constraints de rota (`{id:int}`) para o framework rejeitar URLs quebradas, e `Results.Created`, `Results.NoContent`, `Results.NotFound`, e `Results.BadRequest` para retornar o status code certo pelo motivo certo. Você também encontrou seu primeiro code smell — usar uma exceção para guiar o fluxo normal do programa quando um registro está faltando — e nomeou como algo que o Módulo 3.4 vai limpar.

**Conceitos que você já sabe nomear:**

- **route parameter** — um espaço reservado no caminho combinado com um argumento do handler, com constraint `:int` opcional
- **`MapGroup`** — compartilha o início de um caminho entre endpoints relacionados
- **REST conventions** — verbos e status codes que todo mundo concorda
- **201 Created** — POST bem-sucedido que criou algo novo, com um cabeçalho `Location`
- **204 No Content** — operação bem-sucedida sem nada para retornar; padrão para `DELETE`

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — escolha o status code certo para cada ação da sua própria cabeça. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

No papel, escreva o status code que combina com cada um destes. Para cada um, diga em poucas palavras por quê.

1. Um `POST /kingdoms` que cria um novo reino — sucesso.
2. Um `GET /kingdoms/{id}` para um id que não existe.
3. Um `DELETE /kingdoms/{id}` que funciona.
4. Um `POST /kingdoms` com um nome vazio.

<details><summary>Travou? Abra aqui para conferir.</summary>

1. **201 Created** — uma coisa nova foi criada; também envia um cabeçalho `Location` com a URL do novo reino.
2. **404 Not Found** — a coisa que o cliente pediu não está lá.
3. **204 No Content** — funcionou, e não tem nada para enviar de volta.
4. **400 Bad Request** — o cliente enviou dados ruins.

A grande divisão para lembrar: 2xx significa sucesso, 4xx é culpa do cliente, 5xx é culpa do servidor. Os clientes tomam decisões com base no status code, então o errado é um bug real, não um detalhe pequeno.

</details>

## Movimento git da semana — `git reflog`

Aqui está algo que ninguém te conta sobre o git: ele quase nunca perde de verdade o seu trabalho. Mesmo depois de `git reset --hard`, mesmo depois de um rebase que deu errado, os commits ainda estão salvos pelo git — o git só parou de apontar para eles.

`git reflog` (abreviação de *reference log*) é a rede de segurança. Mostra cada posição recente do HEAD (o ponteiro "você está aqui" do git), do mais novo para o mais antigo. Enquanto o SHA (a impressão digital única de um commit) que você quer ainda está no reflog — cerca de 30 dias por padrão — você pode recuperá-lo.

> **Esse é só via CLI — o painel não tem um botão para isso.**
>
> ```powershell
> git reflog                     # veja onde o HEAD esteve
> git reset --hard HEAD@{1}      # volte uma posição do HEAD
> ```

Este é o movimento: se você fez `reset --hard` no desespero e perdeu commits que queria manter, rode `git reflog`, encontre o SHA de antes do reset, depois rode `git reset --hard <esse-sha>` — e eles voltam. A gente vai fundo nessa rede de segurança no B3.3 se você fizer esse bônus.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.3 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

## Próximo

O Módulo 3.4 apresenta **OpenAPI (Swagger)** — documentação para a sua API gerada automaticamente — e **logging estruturado**. Os dois transformam uma API funcionando em uma que você pode passar para outro desenvolvedor sem ter que explicar tudo.
