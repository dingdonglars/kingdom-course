# Módulo 2.6 — EF Core (Code-First)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

SQL bruto está bem para uma query. Mas vinte queries — cada uma com o seu próprio bloco `using` para a conexão, o comando e o reader — cansa rápido. Hoje a gente conhece o **Entity Framework Core**, o *ORM* do .NET. Um ORM é uma biblioteca que combina classes com rows do banco de dados: você escreve classes em C#, e o EF as transforma em tabelas. Você escreve `db.Kingdoms.Add(k); db.SaveChanges();`, e o EF escreve o SQL por você.

> **Words to watch**
>
> - **ORM** *(oh-arr-em)* — *Object-Relational Mapper*. Uma biblioteca que mapeia classes para rows do banco de dados.
> - **Entity** — uma classe que o EF mapeia para uma tabela.
> - **DbContext** — sua porta de entrada para o banco de dados; uma classe com propriedades `DbSet<T>` (uma por tabela).
> - **`DbSet<T>`** — representa uma tabela; suporta queries LINQ (`db.Kingdoms.Where(...)`).
> - **Code-first** — define o schema em código C#; o EF gera o SQL.
> - **Database-first** — o oposto — gera C# a partir de um schema existente. A gente não faz isso.

---

## Por que um ORM

Depois de escrever `using var conn / cmd / reader` trinta vezes, você começa a ver os mesmos padrões de novo e de novo. Um ORM transforma esses padrões em código pronto:

| SQL bruto | EF Core |
|---|---|
| `INSERT INTO kingdoms (name) VALUES ($name)` | `db.Kingdoms.Add(k); db.SaveChanges();` |
| `SELECT * FROM kingdoms WHERE name = $n` | `db.Kingdoms.Single(k => k.Name == n)` |
| Mapeamento manual de rows → objetos | `Kingdom` já é o tipo |

EF é LINQ sobre um banco de dados. É o mesmo `Where`/`Select`/`OrderBy` que você aprendeu no Módulo 1.6 — o EF os transforma em SQL por baixo dos panos.

Quando você *não* deveria usar um ORM? Quando você precisa de uma query ajustada na mão para velocidade, ou quando o SQL bruto é genuinamente mais simples que a versão EF. No código normal, isso não acontece com frequência.

## Entity de persistência vs modelo do engine

A gente **não** combina `Kingdom.Engine.Kingdom` diretamente com uma tabela — essa classe tem interfaces, campos privados, e um construtor que precisa de `IRandom` e `IClock`. O EF não consegue ler isso de forma limpa.

Em vez disso, a gente adiciona **entity classes** em `Kingdom.Persistence` — classes C# planas com propriedades públicas, construídas para o EF. Transformar o modelo do engine em entities e vice-versa é trabalho da camada de persistência (assim como o DTO JSON no Módulo 2.2).

```
Kingdom.Engine.Kingdom   <─ToEntity / FromEntity─>   KingdomEntity (EF)   <─EF─>   kingdoms (tabela)
```

Três camadas, cada uma com um trabalho. O engine nunca importa `Microsoft.EntityFrameworkCore`.

(Uma *entity* aqui significa apenas uma classe que o EF mapeia para uma tabela.)

## Delta starter

- **MODIFICADO:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (adiciona o pacote EF Core SQLite)
- **NOVO:** `Kingdom.Persistence/EfCore/KingdomEntity.cs`, `BuildingEntity.cs`
- **NOVO:** `Kingdom.Persistence/EfCore/KingdomDbContext.cs`
- **NOVO:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — Save/Load usando EF
- **MODIFICADO:** `Kingdom.Console/Program.cs` (uma pequena demo EF Core no final)
- **NOVO:** `tests/Kingdom.Persistence.Tests/KingdomEfStoreTests.cs`

## Passo 0 — instale o EF Core

```powershell
cd Kingdom.Persistence
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

Dois pacotes: o ORM central, mais o driver SQLite. (Para outros bancos de dados você usaria `Microsoft.EntityFrameworkCore.SqlServer` ou `.PostgreSQL` — o mesmo código EF em cima, só um driver diferente por baixo.)

## Passo 1 — entities

`Kingdom.Persistence/EfCore/KingdomEntity.cs`:

```csharp
namespace Kingdom.Persistence.EfCore;

public class KingdomEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Day { get; set; }
    public int Gold { get; set; }
    public int Wood { get; set; }
    public int Stone { get; set; }
    public int Food { get; set; }

    public List<BuildingEntity> Buildings { get; set; } = new();
}
```

`Kingdom.Persistence/EfCore/BuildingEntity.cs`:

```csharp
namespace Kingdom.Persistence.EfCore;

public class BuildingEntity
{
    public int Id { get; set; }
    public string Kind { get; set; } = "";   // "Farm", "Lumberyard", "Mine"
    public string Name { get; set; } = "";
    public int Level { get; set; }

    public int KingdomId { get; set; }       // foreign key
    public KingdomEntity? Kingdom { get; set; }   // navegação
}
```

As regras que o EF segue por padrão:

- Uma propriedade chamada `Id` é automaticamente a chave primária.
- Se a entity A tem um `List<X>`, e a entity X tem um `int AId` mais um `A? A`, o EF lê isso como um relacionamento um-para-muitos (um A tem muitos X). Ele descobre isso sozinho.
- As propriedades precisam de `set;` para o EF poder preenchê-las ao ler uma row.

## Passo 2 — `DbContext`

`Kingdom.Persistence/EfCore/KingdomDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;

namespace Kingdom.Persistence.EfCore;

public class KingdomDbContext : DbContext
{
    private readonly string _path;

    public KingdomDbContext(string dbPath) { _path = dbPath; }

    public DbSet<KingdomEntity> Kingdoms => Set<KingdomEntity>();
    public DbSet<BuildingEntity> Buildings => Set<BuildingEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_path};Pooling=False");
}
```

`DbSet<KingdomEntity> Kingdoms` é a tabela `kingdoms`. Por padrão, o EF nomeia a tabela igual ao tipo, no plural. Ele descobre sozinho. Mesmo padrão: `DbSet<BuildingEntity> Buildings` é a tabela `buildings`.

`OnConfiguring` diz ao EF *"use SQLite neste caminho."* Apps de verdade geralmente configuram a connection string em outro lugar (num container de DI). Para aprender, escrever aqui mesmo está ótimo.

## Passo 3 — `KingdomEfStore` — salvar e carregar

`Kingdom.Persistence/EfCore/KingdomEfStore.cs`:

```csharp
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Microsoft.EntityFrameworkCore;

namespace Kingdom.Persistence.EfCore;

public class KingdomEfStore
{
    private readonly string _dbPath;
    public KingdomEfStore(string dbPath) { _dbPath = dbPath; }

    /// <summary>Garante que o arquivo do banco de dados + schema existam.</summary>
    public void EnsureCreated()
    {
        using var ctx = new KingdomDbContext(_dbPath);
        ctx.Database.EnsureCreated();
    }

    public int Save(Kingdom.Engine.Kingdom kingdom)
    {
        EnsureCreated();
        using var ctx = new KingdomDbContext(_dbPath);

        var entity = new KingdomEntity
        {
            Name = kingdom.Name,
            Day  = kingdom.Day,
            Gold = kingdom.Resources.Get(Resource.Gold),
            Wood = kingdom.Resources.Get(Resource.Wood),
            Stone = kingdom.Resources.Get(Resource.Stone),
            Food  = kingdom.Resources.Get(Resource.Food),
            Buildings = kingdom.Buildings
                .Select(b => new BuildingEntity { Kind = b.GetType().Name, Name = b.Name, Level = b.Level })
                .ToList()
        };

        ctx.Kingdoms.Add(entity);
        ctx.SaveChanges();          // <-- escreve tudo em uma transação
        return entity.Id;            // EF preencheu isso depois do SaveChanges
    }

    public Kingdom.Engine.Kingdom Load(int id, IRandom rng, IClock clock)
    {
        using var ctx = new KingdomDbContext(_dbPath);
        var entity = ctx.Kingdoms
            .Include(k => k.Buildings)   // <-- carrega os prédios relacionados de forma ansiosa
            .Single(k => k.Id == id);

        var k = new Kingdom.Engine.Kingdom(entity.Name, rng, clock);
        k.Resources.SetTo(Resource.Gold, entity.Gold);
        k.Resources.SetTo(Resource.Wood, entity.Wood);
        k.Resources.SetTo(Resource.Stone, entity.Stone);
        k.Resources.SetTo(Resource.Food, entity.Food);

        foreach (var b in entity.Buildings)
        {
            Building bld = b.Kind switch
            {
                "Farm"        => new Farm(b.Name, b.Level),
                "Lumberyard"  => new Lumberyard(b.Name, b.Level),
                "Mine"        => new Mine(b.Name, b.Level),
                _ => throw new InvalidOperationException($"Unknown building kind '{b.Kind}'.")
            };
            k.AddBuilding(bld);
        }
        // (Day e Citizens não modelados nesta entity mínima ainda — Módulo 2.7 expande.)

        return k;
    }

    public IReadOnlyList<KingdomEntity> ListAll()
    {
        using var ctx = new KingdomDbContext(_dbPath);
        return ctx.Kingdoms
            .AsNoTracking()
            .OrderBy(k => k.Id)
            .ToList();
    }
}
```

Coisas para ler com atenção:

- `ctx.Database.EnsureCreated()` — cria o arquivo do banco de dados e as tabelas se ainda não existirem. Útil para aprender. A gente vai mudar para *migrations* no Módulo 2.7 (o jeito certo).
- `ctx.Kingdoms.Add(entity); ctx.SaveChanges()` — duas linhas. O EF as transforma nas instruções INSERT (ele manda várias de uma vez). A lista `Buildings` é salva também, porque o EF acompanha o link entre elas.
- `Include(k => k.Buildings)` — por padrão, o EF não carrega os prédios relacionados. `Include` diz *"busque os prédios também."*
- `AsNoTracking()` — para queries somente leitura. É mais rápido e seguro quando você não planeja mudar os resultados.

## Passo 4 — chame pelo console

Em `Program.cs`, adicione:

```csharp
// Demo EF Core (Módulo 2.6)
var efDb = Path.Combine(saveFolder, "kingdoms-ef.db");
if (File.Exists(efDb)) File.Delete(efDb);

var efStore = new KingdomEfStore(efDb);
var savedId = efStore.Save(kingdom);
Console.WriteLine();
Console.WriteLine($"=== Demo EF Core ({efDb}) ===");
Console.WriteLine($"Saved kingdom #{savedId}");

var loaded = efStore.Load(savedId, new SystemRandom(0), new SystemClock());
Console.WriteLine($"Loaded: {loaded.Name} com {loaded.Buildings.Count} prédio(s), gold {loaded.Resources.Get(Resource.Gold)}");

var all = efStore.ListAll();
Console.WriteLine($"Todos os reinos salvos ({all.Count}):");
foreach (var e in all)
    Console.WriteLine($"  #{e.Id}  {e.Name}  day {e.Day}");
```

Compile e rode.

## Passo 5 — testes

`tests/Kingdom.Persistence.Tests/KingdomEfStoreTests.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence.EfCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class KingdomEfStoreTests
{
    [Fact]
    public void Save_ThenLoad_PreservesNameAndResources()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ef-{Guid.NewGuid():N}.db");
        try
        {
            var k = new global::Kingdom.Engine.Kingdom("EFTest");
            k.Resources.Add(Resource.Gold, 500);
            k.AddBuilding(new Farm("MyFarm"));

            var store = new KingdomEfStore(path);
            var id = store.Save(k);

            var loaded = store.Load(id, new SystemRandom(0), new SystemClock());
            loaded.Name.ShouldBe("EFTest");
            loaded.Resources.Get(Resource.Gold).ShouldBe(600);  // 100 padrão + 500 adicionados
            loaded.Buildings.Count.ShouldBe(1);
            loaded.Buildings.OfType<Farm>().Single().Name.ShouldBe("MyFarm");
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Save_TwoKingdoms_BothShowInListAll()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ef-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.Save(new global::Kingdom.Engine.Kingdom("Alpha"));
            store.Save(new global::Kingdom.Engine.Kingdom("Beta"));

            var all = store.ListAll();
            all.Count.ShouldBe(2);
            all.Select(e => e.Name).ShouldBe(new[] { "Alpha", "Beta" });
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Load_NonexistentId_Throws()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ef-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.EnsureCreated();    // arquivo existe, só sem rows
            Should.Throw<InvalidOperationException>(() =>
                store.Load(999, new SystemRandom(0), new SystemClock()));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Rode:

```powershell
dotnet test
```

Espere `Passed: 60` (57 + 3).

## Mexa um pouco

Abra `bin/Debug/net10.0/saves/kingdoms-ef.db` no DB Browser for SQLite. As tabelas que o EF criou são *exatamente* o que a gente teria escrito na mão. Nada escondido.

Adicione uma propriedade a `KingdomEntity`: `public DateTime SavedAt { get; set; }`. Rode de novo. Vai travar — o banco de dados não tem essa coluna ainda. Esse é o problema que as migrations resolvem (Módulo 2.7).

Faça o EF imprimir seu SQL: em `OnConfiguring`, adicione `.LogTo(Console.WriteLine, LogLevel.Information)`. Rode de novo. Você vai ver o SQL real que o EF está escrevendo. Lê-lo torna o ORM muito menos misterioso.

`db.Kingdoms.Where(k => k.Gold > 100).ToList()` — o mesmo LINQ que `List<T>.Where`, transformado em SQL. Tente `OrderByDescending`, `Count`, `Sum`. Tudo funciona.

## O que você acabou de fazer

Você substituiu SQL bruto por objetos C# e deixou o EF Core fazer a tradução. Duas entity classes, um `DbContext`, e um store pequeno te dão salvar, carregar e listar — com três novos testes provando que funciona (60 no total). Você também conheceu o padrão de três camadas que mantém o resto desta fase: o modelo do engine de um lado, o banco de dados do outro, e uma *entity* no meio para o engine nunca ter que importar `Microsoft.EntityFrameworkCore`. Manter esses separados parece trabalho extra num projeto pequeno — e é — mas é a diferença entre *"podemos trocar de banco de dados um dia"* e *"estamos presos neste para sempre."*

**Conceitos que você já sabe nomear:**

- **ORM** — biblioteca de tradução classe-para-row
- **`DbContext`** — sua porta de entrada para o banco de dados
- **`DbSet<T>`** — representa uma tabela; consultável via LINQ
- **`Add` + `SaveChanges`** — o padrão INSERT do EF
- **`Include`** — carrega ansiosamente uma coleção relacionada

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: com EF você adiciona um objeto e chama `SaveChanges`, e o EF escreve o SQL por você. Ninguém corrige isso — o executor de testes faz, que é o ponto. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem olhar:

1. Escreva as linhas que salvam uma nova `KingdomEntity` (dê só um `Name`) num `KingdomDbContext`.
2. Escreva uma query que lê todos os reinos de volta.
3. Rode — você deve receber seu único reino de volta.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
using var ctx = new KingdomDbContext(dbPath);
ctx.Database.EnsureCreated();

ctx.Kingdoms.Add(new KingdomEntity { Name = "Eldoria" });
ctx.SaveChanges();

foreach (var k in ctx.Kingdoms)
    Console.WriteLine($"#{k.Id} {k.Name}");   // #1 Eldoria
```

`Add` e depois `SaveChanges` é o salvamento todo. O EF deu um `Id` à row por você — você não escreveu uma linha de SQL.

</details>

## Movimento git da semana — leia um diff linha por linha

Você mudou muito hoje: novas entities EF, um DbContext, um store novo. Antes de mesclar ou compartilhar isso com alguém (ou com você-do-futuro), leia o diff *com cuidado*, linha por linha.

No painel do Source Control do VS Code: clique em qualquer arquivo em *Changes* para abrir o diff lado a lado. Percorra os *hunks* um por vez (um hunk é um bloco de linhas alteradas, com algumas linhas sem mudança ao redor para você saber onde está). Para cada um, pergunte *"por que essa linha?"* Os hunks que você não consegue explicar em inglês simples são os que você deve entender melhor ou desfazer.

Para ler o diff de outra pessoa (o PR de um colega, ou um dos seus commits antigos): no GitLens Commit Graph, clique em qualquer commit. O painel à direita mostra a mesma visão hunk por hunk.

> **Ou no terminal:** `git show <commit-hash>` mostra o diff de um único commit. `git log -p` mostra o diff de cada commit no seu histórico.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.6 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 2.7 apresenta as **migrations** — o jeito certo de mudar a estrutura do banco de dados após o primeiro lançamento. `EnsureCreated` funciona para um *banco de dados completamente novo*. Migrations funcionam quando o *banco de dados já tem dados e a estrutura precisa mudar*.
