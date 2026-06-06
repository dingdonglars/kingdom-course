# Módulo 2.5 — JOINs

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

No Módulo 2.4 a gente tinha uma tabela. Apps de verdade têm dez ou cinquenta. Hoje a gente adiciona uma tabela `buildings` que *pertence à* tabela `kingdoms`, e aprende como perguntar *"me mostra cada reino junto com todos os seus prédios"* em uma única query. É isso que torna um banco de dados *relacional* — ele pode guardar coisas que se relacionam entre si.

> **Words to watch**
>
> - **foreign key** — uma coluna cujo valor é o `id` de uma row em *outra* tabela
> - **JOIN** — combinar rows de duas tabelas numa condição de correspondência
> - **`INNER JOIN`** — só rows que existem nos *dois* lados
> - **`LEFT JOIN`** — toda row da tabela da esquerda; correspondência da direita (ou `NULL`)
> - **`GROUP BY`** + agregados (`COUNT`, `SUM`, `AVG`) — colapsa muitas rows numa por grupo

---

## Por que duas tabelas

Guardar prédios dentro da tabela `kingdoms` é complicado. Como você colocaria uma lista inteira de prédios numa única célula? Você teria que transformá-los em JSON — o que funciona, mas aí você não pode consultar *dentro* desse JSON facilmente. O jeito melhor: cada tipo de coisa tem a sua própria tabela, e você liga as tabelas com foreign keys.

```
kingdoms                 buildings
========                 =========
id  name      day        id  kingdom_id  kind     name      level
1   Eldoria   11         1   1           Farm     Main      3
2   Briar     7          2   1           Mine     Old       1
                         3   2           Farm     East      2
```

O `buildings.kingdom_id` *aponta de volta* para `kingdoms.id`. Essa é a foreign key. Agora você pode perguntar: *"todos os prédios do reino 1"* (`WHERE kingdom_id = 1`), *"o reino do prédio 3"* (`JOIN ... ON kingdom_id = id`), e *"cada reino com o número de prédios que tem"* (`JOIN ... GROUP BY ...`).

## Os três tipos de JOIN que você vai usar quase sempre

```sql
-- INNER JOIN: rows que existem nos dois lados
SELECT k.name, b.name FROM kingdoms k
INNER JOIN buildings b ON b.kingdom_id = k.id;

-- LEFT JOIN: todo reino, mesmo os sem prédios (b.* será NULL)
SELECT k.name, b.name FROM kingdoms k
LEFT JOIN buildings b ON b.kingdom_id = k.id;

-- Agregados com GROUP BY: uma row por reino, com contagem de prédios
SELECT k.name, COUNT(b.id) AS building_count FROM kingdoms k
LEFT JOIN buildings b ON b.kingdom_id = k.id
GROUP BY k.id;
```

`k` e `b` são *aliases* — apelidos curtos para as tabelas dentro da query. Sem eles, você teria que escrever `kingdoms.name` e `buildings.name` em todo lugar.

## Delta starter

- **NOVO:** `Kingdom.Persistence/SqliteJoinsDemo.cs` — configura duas tabelas, insere dados, roda três queries
- **MODIFICADO:** `Kingdom.Console/Program.cs` (chama a nova demo)
- **NOVO:** `tests/Kingdom.Persistence.Tests/SqliteJoinsDemoTests.cs`

Engine e JSON sem mudanças.

## Passo 1 — `SqliteJoinsDemo`

`Kingdom.Persistence/SqliteJoinsDemo.cs`:

```csharp
using Microsoft.Data.Sqlite;

namespace Kingdom.Persistence;

public static class SqliteJoinsDemo
{
    public record KingdomRow(int Id, string Name);
    public record BuildingRow(int Id, int KingdomId, string Kind, string Name, int Level);
    public record KingdomCount(string Name, int BuildingCount);

    public static (IReadOnlyList<KingdomRow> Kingdoms,
                   IReadOnlyList<BuildingRow> Buildings,
                   IReadOnlyList<KingdomCount> Counts)
        RunDemo(string dbPath)
    {
        var connStr = $"Data Source={dbPath};Pooling=False";
        using var conn = new SqliteConnection(connStr);
        conn.Open();

        // --- schema ---
        Exec(conn, @"
            CREATE TABLE kingdoms (
                id   INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL
            );
            CREATE TABLE buildings (
                id          INTEGER PRIMARY KEY AUTOINCREMENT,
                kingdom_id  INTEGER NOT NULL REFERENCES kingdoms(id),
                kind        TEXT NOT NULL,
                name        TEXT NOT NULL,
                level       INTEGER NOT NULL DEFAULT 1
            );
        ");

        // --- dados iniciais ---
        var eldoria = InsertKingdom(conn, "Eldoria");
        var briar   = InsertKingdom(conn, "Briarholm");
        var empty   = InsertKingdom(conn, "Stoneholt");        // sem prédios — para demo LEFT JOIN
        InsertBuilding(conn, eldoria, "Farm", "Main",      3);
        InsertBuilding(conn, eldoria, "Mine", "Old Vein",  1);
        InsertBuilding(conn, briar,   "Farm", "East Farm", 2);

        // --- queries ---
        var kingdoms = Read(conn, "SELECT id, name FROM kingdoms ORDER BY id",
            r => new KingdomRow(r.GetInt32(0), r.GetString(1)));

        var inner = Read(conn, @"
                SELECT b.id, b.kingdom_id, b.kind, b.name, b.level
                FROM buildings b
                INNER JOIN kingdoms k ON k.id = b.kingdom_id
                ORDER BY b.id",
            r => new BuildingRow(r.GetInt32(0), r.GetInt32(1), r.GetString(2), r.GetString(3), r.GetInt32(4)));

        var counts = Read(conn, @"
                SELECT k.name, COUNT(b.id) AS building_count
                FROM kingdoms k
                LEFT JOIN buildings b ON b.kingdom_id = k.id
                GROUP BY k.id
                ORDER BY k.id",
            r => new KingdomCount(r.GetString(0), r.GetInt32(1)));

        return (kingdoms, inner, counts);
    }

    private static int InsertKingdom(SqliteConnection conn, string name)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO kingdoms (name) VALUES ($name); SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$name", name);
        return (int)(long)cmd.ExecuteScalar()!;
    }

    private static void InsertBuilding(SqliteConnection conn, int kingdomId, string kind, string name, int level)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO buildings (kingdom_id, kind, name, level)
            VALUES ($kid, $kind, $name, $level)";
        cmd.Parameters.AddWithValue("$kid", kingdomId);
        cmd.Parameters.AddWithValue("$kind", kind);
        cmd.Parameters.AddWithValue("$name", name);
        cmd.Parameters.AddWithValue("$level", level);
        cmd.ExecuteNonQuery();
    }

    private static void Exec(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    private static List<T> Read<T>(SqliteConnection conn, string sql, Func<SqliteDataReader, T> map)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        using var r = cmd.ExecuteReader();
        var list = new List<T>();
        while (r.Read()) list.Add(map(r));
        return list;
    }
}
```

Note os métodos helper (`Exec`, `Read<T>`, `InsertKingdom`, `InsertBuilding`). Não tem nada de inteligente neles — eles só evitam que você repita o mesmo código de configuração várias vezes. Quando você já escreveu a mesma configuração SQL três vezes, coloque num helper.

> **Não se preocupe com os colchetes angulares ainda.** `Read<T>` e `Func<SqliteDataReader, T>` usam um recurso chamado *generics* — um jeito de escrever um helper que funciona para qualquer tipo `T`. A gente vai cobrir generics direito mais tarde. Por agora, leia `T` como *"qualquer tipo que quem chama pedir"*, e o helper preenche. Basta funcionar.

`last_insert_rowid()` é uma função do SQLite que retorna o id da row que você acabou de inserir. É o jeito padrão de responder *"acabei de inserir uma row — qual id ela recebeu?"*

## Passo 2 — chame pelo console

Em `Program.cs`, adicione:

```csharp
// Demo de JOINs (Módulo 2.5)
var joinsDb = Path.Combine(saveFolder, "kingdoms-joins.db");
if (File.Exists(joinsDb)) File.Delete(joinsDb);
var (kingdomRows, joinedBuildings, counts) = SqliteJoinsDemo.RunDemo(joinsDb);

Console.WriteLine();
Console.WriteLine($"=== Demo de JOINs ({joinsDb}) ===");
Console.WriteLine($"Reinos ({kingdomRows.Count}):");
foreach (var k in kingdomRows)
    Console.WriteLine($"  #{k.Id} {k.Name}");
Console.WriteLine($"Prédios (INNER JOIN, {joinedBuildings.Count}):");
foreach (var b in joinedBuildings)
    Console.WriteLine($"  #{b.Id}  k{b.KingdomId}  {b.Kind} '{b.Name}' (lvl {b.Level})");
Console.WriteLine($"Contagens (LEFT JOIN + GROUP BY):");
foreach (var c in counts)
    Console.WriteLine($"  {c.Name}: {c.BuildingCount} prédio(s)");
```

`Stoneholt` aparece com `0` porque o `LEFT JOIN` o mantém. Um `INNER JOIN` o teria deixado de fora.

Rode e veja a saída.

## Passo 3 — testes

`tests/Kingdom.Persistence.Tests/SqliteJoinsDemoTests.cs`:

```csharp
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SqliteJoinsDemoTests
{
    [Fact]
    public void RunDemo_HasThreeKingdomsAndThreeBuildings()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kjoin-{Guid.NewGuid():N}.db");
        try
        {
            var (kingdoms, buildings, counts) = SqliteJoinsDemo.RunDemo(path);
            kingdoms.Count.ShouldBe(3);
            buildings.Count.ShouldBe(3);   // INNER JOIN: só prédios com correspondência
            counts.Count.ShouldBe(3);      // LEFT JOIN: todo reino listado
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Counts_ShowZero_ForKingdomWithNoBuildings()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kjoin-{Guid.NewGuid():N}.db");
        try
        {
            var (_, _, counts) = SqliteJoinsDemo.RunDemo(path);
            counts.Single(c => c.Name == "Stoneholt").BuildingCount.ShouldBe(0);
            counts.Single(c => c.Name == "Eldoria").BuildingCount.ShouldBe(2);
            counts.Single(c => c.Name == "Briarholm").BuildingCount.ShouldBe(1);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void InnerJoin_OnlyReturnsBuildingsThatHaveAKingdom()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kjoin-{Guid.NewGuid():N}.db");
        try
        {
            var (_, buildings, _) = SqliteJoinsDemo.RunDemo(path);
            buildings.All(b => b.KingdomId > 0).ShouldBeTrue();
            buildings.Select(b => b.KingdomId).ShouldContain(1);   // Eldoria
            buildings.Select(b => b.KingdomId).ShouldContain(2);   // Briarholm
            buildings.Select(b => b.KingdomId).ShouldNotContain(3); // Stoneholt não tem prédios
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Rode:

```powershell
dotnet test
```

Espere `Passed: 57` (54 + 3).

## Mexa um pouco

Troque o LEFT JOIN por INNER JOIN na query de contagens. Stoneholt desaparece. Agora você pode ver a diferença entre os dois.

Adicione uma cláusula `WHERE k.name LIKE 'B%'`. Só Briarholm aparece. Em SQL, `LIKE` junto com `%` deixa você combinar parte de uma palavra.

Tente `SELECT k.name, b.kind, AVG(b.level) FROM kingdoms k JOIN buildings b ON b.kingdom_id = k.id GROUP BY k.id, b.kind`. Isso agrupa por *duas* coisas ao mesmo tempo: reino e tipo de prédio.

Remova a ligação de foreign key: escreva `kingdom_id INTEGER NOT NULL` sem o `REFERENCES kingdoms(id)`. Insira um prédio com `kingdom_id = 999`. Funciona — por padrão, o SQLite não verifica foreign keys (uma coisa a saber sobre ele). Rode `PRAGMA foreign_keys = ON;` primeiro para fazê-lo verificar.

## O que você acabou de fazer

Você foi de uma tabela para duas, e de uma query para quatro. Os prédios agora pertencem a reinos por uma foreign key, e você pode fazer perguntas reais ao banco de dados sobre os dois ao mesmo tempo: cada prédio com seu reino (`INNER JOIN`), cada reino *mesmo que não tenha prédios* (`LEFT JOIN`), e o número de prédios por reino (`GROUP BY` + `COUNT`). Três novos testes provam que as queries retornam o esperado — 57 passando no total. Você também conheceu `last_insert_rowid()`, o jeito padrão do SQLite de obter o id da row que você acabou de inserir.

**Conceitos que você já sabe nomear:**

- **foreign key** — uma coluna apontando para o id de outra tabela
- **`INNER JOIN`** — só rows que batem nos dois lados
- **`LEFT JOIN`** — toda row da esquerda, mesmo sem correspondência
- **`GROUP BY` + agregado** — `COUNT`/`SUM`/`AVG`, uma row por grupo
- **alias de tabela** — `k` para `kingdoms`; mais curto de ler

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: um `JOIN` lê de duas tabelas ao mesmo tempo, combinando rows em um id compartilhado. Ninguém corrige isso — o motor do banco de dados faz, que é o ponto. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra `sqlite3` num arquivo novo. Sem olhar:

1. Crie duas tabelas — `kingdoms` (`id`, `name`) e `buildings` (`id`, `kingdom_id`, `name`).
2. Coloque um reino e dois de seus prédios.
3. Escreva um `SELECT` que usa um `INNER JOIN` para mostrar o nome de cada prédio ao lado do nome do seu reino.

Você deve receber duas rows de volta, ambas mostrando o nome do reino.

<details><summary>Travou? Abra aqui para conferir.</summary>

```sql
SELECT k.name, b.name
FROM kingdoms k
INNER JOIN buildings b ON b.kingdom_id = k.id;
```

A condição do join é a parte a acertar: `b.kingdom_id = k.id` é como o banco de dados sabe qual prédio pertence a qual reino. O `k` e o `b` são só apelidos curtos para as tabelas.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.5 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 2.6 apresenta o **EF Core** — o ORM (mapeador objeto-relacional) do .NET que combina `class Kingdom { }` com uma row numa tabela, e deixa você escrever `dbContext.Kingdoms.Add(kingdom)` em vez de SQL bruto. O mesmo banco de dados por baixo, com muito menos código para escrever em cima.
