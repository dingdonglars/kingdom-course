# Módulo 2.9 — Save Slots (Múltiplos Reinos)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Um save por si só não é muito. *Muitos* saves é uma funcionalidade de verdade. Hoje o store EF cresce além de `Save` e `Load` para adicionar `Update` e `Delete` (e `ListAll`, que já existe) — o grupo completo de **CRUD**. Com um banco de dados de verdade por baixo, *"me dê os meus últimos 10 reinos"* é uma query de uma linha. O reino agora tem *save slots*, como todo jogo que você já jogou.

> **Words to watch**
>
> - **CRUD** *(crud)* — Create / Read / Update / Delete — as quatro operações sobre rows
> - **slot** — um save na lista (termo de design de jogos)
> - **`Update`** — modifica uma row existente (vs `Add`, que insere uma nova)
> - **transaction** — um grupo de operações que todas têm sucesso ou todas falham juntas

---

## Por que slots importam

Save slots mudaram os jogos. Antes deles, você salvava por cima do seu único save e torcia para não ter salvado num momento ruim. Com slots, você pode tentar um plano arriscado, salvar antes, e voltar para o save anterior se der errado. Slots tornam seguro experimentar.

Aqui está como funciona em código: cada slot é uma row em `kingdoms`. Listar os slots é `SELECT *`. Carregar um é `SELECT WHERE id =`. Salvar por cima de um slot que já existe é `UPDATE`. Criar um novo slot é `INSERT`. Remover um é `DELETE`. A funcionalidade inteira são cinco queries.

## Delta starter

- **MODIFICADO:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — adiciona `Update(int id, kingdom)` e `Delete(int id)`
- **NOVO:** `Kingdom.Persistence/EfCore/KingdomSlotInfo.cs` — DTO para a lista de slots (para o console não precisar conhecer as entities EF)
- **MODIFICADO:** `Kingdom.Console/Program.cs` — demo de múltiplos slots (cria 3 saves, lista eles, carrega o slot 2, modifica, atualiza, lista de novo)
- **NOVO:** `tests/Kingdom.Persistence.Tests/SlotCrudTests.cs`

## Passo 1 — DTO `KingdomSlotInfo`

O console não precisa ver `KingdomEntity`, e não deveria. Dê a ele um DTO pequeno:

```csharp
namespace Kingdom.Persistence.EfCore;

public record KingdomSlotInfo(int Id, string Name, int Day);
```

Três campos — exatamente o que um seletor de slots precisa mostrar.

## Passo 2 — `Update` e `Delete`

Em `KingdomEfStore.cs`, adicione:

```csharp
public void Update(int id, Kingdom.Engine.Kingdom kingdom)
{
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = ctx.Kingdoms
        .Include(k => k.Buildings)
        .Single(k => k.Id == id);

    entity.Name  = kingdom.Name;
    entity.Day   = kingdom.Day;
    entity.Gold  = kingdom.Resources.Get(Resource.Gold);
    entity.Wood  = kingdom.Resources.Get(Resource.Wood);
    entity.Stone = kingdom.Resources.Get(Resource.Stone);
    entity.Food  = kingdom.Resources.Get(Resource.Food);

    // Substitui os prédios — a estratégia correta mais simples. Remove os desconectados, adiciona os novos.
    entity.Buildings.Clear();
    entity.Buildings.AddRange(kingdom.Buildings.Select(b =>
        new BuildingEntity { Kind = b.GetType().Name, Name = b.Name, Level = b.Level }));

    ctx.SaveChanges();
}
```

> **Um detalhe nesse código.** `b.GetType().Name` retorna o nome da classe como string — `"Farm"`, `"Lumberyard"`, `"Mine"`. Todo objeto C# conhece o seu próprio tipo enquanto o programa roda. `GetType()` te dá o tipo, e `.Name` lê o seu nome curto. É um jeito prático de salvar "que tipo de coisa é essa?" numa coluna do banco de dados.

```csharp

public void Delete(int id)
{
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = ctx.Kingdoms.Find(id);
    if (entity is null) return;
    ctx.Kingdoms.Remove(entity);
    ctx.SaveChanges();
}

public IReadOnlyList<KingdomSlotInfo> ListSlots()
{
    using var ctx = new KingdomDbContext(_dbPath);
    return ctx.Kingdoms
        .AsNoTracking()
        .OrderBy(k => k.Id)
        .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
        .ToList();
}
```

Algumas notas:

- **`Find(id)` vs `Single(...)`** — `Find` retorna `null` quando a row está faltando (bom para "apague se estiver lá"). `Single` joga um erro em vez disso.
- **`.Select(k => new KingdomSlotInfo(...))`** é *projeção* — o EF escreve SQL que pega só essas três colunas, não a row inteira. Isso é mais rápido, usa menos memória, e pula o rastreamento que você não precisa.
- **`Clear()` + `AddRange()`** para a lista de prédios — o EF lida com os deletes e inserts juntos em uma transação. (Uma *transaction* é um grupo de mudanças no banco de dados que todas têm sucesso juntas ou todas falham juntas.) Para uma lista pequena está ótimo. Para uma lista de 10000, você compararia os dois e atualizaria só o que mudou.
- **Cascade delete** — por padrão, o EF apaga as rows filhas (os prédios) quando você apaga o pai (o reino). Se não quiser isso, você pode mudar em `OnModelCreating`.

## Passo 3 — demo de múltiplos slots pelo console

Em `Program.cs`, adicione:

```csharp
// Demo de save slots (Módulo 2.9)
Console.WriteLine();
Console.WriteLine("=== Demo de save slots ===");

// Cria três saves
var slotsStore = new KingdomEfStore(efDb);
var idA = slotsStore.Save(new global::Kingdom.Engine.Kingdom("Alpha"));
var idB = slotsStore.Save(new global::Kingdom.Engine.Kingdom("Beta"));
var idC = slotsStore.Save(new global::Kingdom.Engine.Kingdom("Gamma"));

// Lista os slots
ListSlots(slotsStore);

// Carrega o slot B, avança 5 dias, atualiza
var beta = slotsStore.Load(idB, new SystemRandom(0), new SystemClock());
for (int i = 0; i < 5; i++) beta.AdvanceDay();
slotsStore.Update(idB, beta);
Console.WriteLine($"Updated slot {idB} (Beta) — now at day {beta.Day}");

// Apaga o slot A
slotsStore.Delete(idA);
Console.WriteLine($"Deleted slot {idA} (Alpha)");

ListSlots(slotsStore);

void ListSlots(KingdomEfStore store)
{
    Console.WriteLine($"All slots:");
    foreach (var s in store.ListSlots())
        Console.WriteLine($"  #{s.Id}  {s.Name,-10} day {s.Day}");
}
```

Rode. A saída parece uma tela de save slots:

```
All slots:
  #2  Alpha      day 1
  #3  Beta       day 1
  #4  Gamma      day 1
Updated slot 3 (Beta) — now at day 6
Deleted slot 2 (Alpha)
All slots:
  #3  Beta       day 6
  #4  Gamma      day 1
```

## Passo 4 — testes

`tests/Kingdom.Persistence.Tests/SlotCrudTests.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence.EfCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SlotCrudTests
{
    [Fact]
    public void ListSlots_ReturnsLightweightDtos()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.Save(new global::Kingdom.Engine.Kingdom("Alpha"));
            store.Save(new global::Kingdom.Engine.Kingdom("Beta"));
            var slots = store.ListSlots();
            slots.Count.ShouldBe(2);
            slots[0].Name.ShouldBe("Alpha");
            slots[0].Day.ShouldBe(1);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Update_ChangesExistingRow_NotInsertNew()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            var k = new global::Kingdom.Engine.Kingdom("X");
            var id = store.Save(k);
            for (int i = 0; i < 10; i++) k.AdvanceDay();
            store.Update(id, k);

            store.ListSlots().Count.ShouldBe(1);
            var loaded = store.Load(id, new SystemRandom(0), new SystemClock());
            loaded.Resources.Get(Resource.Food).ShouldBeLessThan(30);   // um pouco foi consumido
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Update_ReplacesBuildingList()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            var k = new global::Kingdom.Engine.Kingdom("X");
            k.AddBuilding(new Farm("F1"));
            var id = store.Save(k);

            // Agora substitui os prédios por completo
            var k2 = new global::Kingdom.Engine.Kingdom("X");
            k2.AddBuilding(new Mine("M1"));
            k2.AddBuilding(new Lumberyard("L1"));
            store.Update(id, k2);

            var loaded = store.Load(id, new SystemRandom(0), new SystemClock());
            loaded.Buildings.Count.ShouldBe(2);
            loaded.Buildings.OfType<Farm>().ShouldBeEmpty();
            loaded.Buildings.OfType<Mine>().Count().ShouldBe(1);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Delete_RemovesRow_AndChildren()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            var k = new global::Kingdom.Engine.Kingdom("Doomed");
            k.AddBuilding(new Farm("F"));
            var id = store.Save(k);

            store.Delete(id);
            store.ListSlots().ShouldBeEmpty();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Delete_NonexistentId_DoesNotThrow()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.EnsureCreated();
            Should.NotThrow(() => store.Delete(999));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Rode:

```powershell
dotnet test
```

Espere `Passed: 68` (63 + 5).

## Mexa um pouco

Adicione um sexto slot, depois `ListSlots().OrderByDescending(s => s.Day).First()` — o save que você mais jogou. Uma linha de LINQ, e o EF escreve o SQL de `ORDER BY` + `LIMIT 1`.

Adicione um campo `LastSavedAt DateTime` em `KingdomEntity` (e uma migration). Ordene por ele em vez disso. Agora o seu seletor de slots pode mostrar *"jogado pela última vez há 3 horas."*

E se dois saves tiverem o mesmo nome? Isso é permitido — o `Id` é o que torna cada um único. O app decide como mostrá-los de forma diferente (por exemplo, adicionando a data).

Tente `store.Update(999, kingdom)`. Ele joga um erro por causa do `Single(...)`. Apps de verdade pegam isso e mostram uma mensagem como *"esse slot de save sumiu — sua lista estava desatualizada."*

## O que você acabou de fazer

Você completou as quatro operações CRUD — Create, Read, Update, Delete — na tabela kingdoms. Cinco novos testes provam que cada uma faz o que deve, incluindo apagar-depois-listar e atualizar-substitui-prédios (68 passando no total). Você também conheceu dois movimentos EF pequenos mas úteis: `.Select(k => new KingdomSlotInfo(...))` para pegar só as colunas que você precisa (menos dados, sem rastreamento), e `Find(id)` vs `Single(id)` para *"faltando está bem"* vs *"faltando é um erro."* O reino agora funciona como a tela de save de um jogo: liste tudo, escolha um, carregue.

**Conceitos que você já sabe nomear:**

- **CRUD** — Create / Read / Update / Delete
- **save slot** — uma row, um reino jogável
- **projeção** — `.Select(...)` para só as colunas que você precisa
- **`Find` vs `Single`** — null-em-falta vs erro-em-falta
- **cascade delete** — pai sumiu, filhos sumiram também

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: os quatro movimentos CRUD, e como `Delete` funciona no EF — encontre a row, remova-a, salve. Ninguém corrige isso — o executor de testes faz, que é o ponto. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Primeiro, nomeie as quatro operações CRUD em voz alta, com o movimento EF para cada (Create → `Add`, Read → uma query, Update → mude e depois `SaveChanges`, Delete → `Remove`). Depois, sem olhar, escreva o corpo de um método `Delete(int id)`:

1. Abra um contexto.
2. Encontre a row.
3. Retorne se estiver faltando.
4. Remova-a.
5. Salve.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
public void Delete(int id)
{
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = ctx.Kingdoms.Find(id);
    if (entity is null) return;       // faltando está bem — nada a fazer
    ctx.Kingdoms.Remove(entity);
    ctx.SaveChanges();
}
```

`Find(id)` retorna `null` quando a row não está lá, então o `if` torna "apagar algo que já sumiu" seguro em vez de erro. `Remove` e depois `SaveChanges` faz o delete. O EF também remove as rows de prédios abaixo — esse é o cascade delete.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.9 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.9 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 2.10 constrói a **UI de save slots** no console — um loop real de escolher e carregar que você pode usar, construído sobre as operações CRUD de hoje.
