# Módulo 2.4 — SQL Primer (com SQLite)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Arquivos JSON são ótimos para *um* save. Mas e se você quiser uma *lista* de todos os seus reinos salvos? E se quiser perguntar *"qual reino tem mais ouro?"* Com arquivos, você tem que carregar tudo só para encontrar uma coisa. Hoje a gente conhece **o banco de dados**, e a linguagem que bancos de dados falam: **SQL**. A gente usa o **SQLite** *(es-queue-el-ite)*, um banco de dados que vive em um único arquivo `.db` — sem servidor para rodar, nada para instalar.

> **Words to watch**
>
> - **database** — um armazém estruturado de dados que você pode consultar
> - **table** — uma grade de linhas e colunas; a unidade de armazenamento num banco de dados relacional
> - **column** — um campo com tipo numa tabela (ex.: `Name TEXT`, `Day INTEGER`)
> - **row** — um registro numa tabela
> - **SQL** *(see-quel)* — *Structured Query Language*. A linguagem para falar com bancos de dados relacionais.
> - **SQLite** — um motor de banco de dados SQL embutido; uma biblioteca, zero servidores, o banco de dados é um arquivo
> - **CREATE / INSERT / SELECT / UPDATE / DELETE** — os cinco comandos SQL mais usados

---

## Por que um banco de dados

Um arquivo guarda *uma* coisa bem. Um banco de dados guarda *muitas* coisas e deixa você fazer perguntas sobre elas.

Digamos que você salva 100 reinos ao longo de um ano. Com arquivos, você tem 100 arquivos JSON ordenados por nome. Para *"encontrar o que tem mais ouro"*, você tem que abrir todos os 100, ler cada um, e compará-los. Com um banco de dados, é uma linha: `SELECT name FROM kingdoms ORDER BY gold DESC LIMIT 1;`. Termina em alguns milissegundos.

É por isso que quase todo programa real usa um banco de dados. A pergunta mais difícil é *qual*. Para um reino que vive no seu laptop, o SQLite é um ótimo encaixe: sem servidor para rodar, sem porta para abrir, e o banco de dados inteiro é um único arquivo que você pode copiar para qualquer lugar.

## SQLite, em um parágrafo

SQLite é uma **biblioteca**, não um servidor. Você adiciona ela ao seu projeto, diz onde está o arquivo, e ela cuida do resto. É usada em todo lugar — seu celular, seu navegador, todo Mac, sua TV. Pode ser o software mais instalado já feito. Ela aguenta cargas reais e grandes (o Stack Overflow usa ela para algumas tarefas), mas também é um ótimo encaixe para um jogo de um jogador ou um projeto de aprendizado.

## Como uma tabela parece

Um banco de dados guarda dados em **tables** — grades de linhas e colunas. Uma tabela para seus reinos salvos pode parecer assim:

```text
   uma tabela chamada  kingdoms
   +----+-----------+-----+------+
   | id | name      | day | gold |   <- columns: cada um tem um tipo
   +----+-----------+-----+------+
   |  1 | Eldoria   |  12 |  340 |   <- uma row: um reino salvo
   |  2 | Stormhold |   4 |   90 |   <- outra row
   +----+-----------+-----+------+
```

Cada **row** é um reino salvo. Cada **column** é um fato sobre ele, com um tipo fixo (`name` é texto, `gold` é um número inteiro). SQL — os cinco comandos abaixo — é como você adiciona rows, muda elas, e faz perguntas sobre toda a grade sem carregá-la toda você mesmo.

## Os cinco comandos SQL

```sql
-- Criar uma tabela (define columns e seus tipos)
CREATE TABLE kingdoms (
    id    INTEGER PRIMARY KEY AUTOINCREMENT,
    name  TEXT NOT NULL,
    day   INTEGER NOT NULL,
    gold  INTEGER NOT NULL
);

-- Inserir uma row
INSERT INTO kingdoms (name, day, gold) VALUES ('Eldoria', 11, 250);

-- Ler todas as rows
SELECT id, name, day, gold FROM kingdoms;

-- Ler com um filtro
SELECT * FROM kingdoms WHERE gold > 100;

-- Atualizar rows
UPDATE kingdoms SET gold = gold + 50 WHERE name = 'Eldoria';

-- Apagar rows
DELETE FROM kingdoms WHERE gold = 0;
```

Isso é a maior parte do SQL. O resto são JOINs (Módulo 2.5), agregados (`COUNT`, `SUM`), e índices (mais tarde).

## Delta starter

- **MODIFICADO:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (adiciona o pacote `Microsoft.Data.Sqlite`)
- **NOVO:** `Kingdom.Persistence/SqliteDemo.cs` (classe pequena demonstrando connect + CREATE + INSERT + SELECT)
- **MODIFICADO:** `Kingdom.Console/Program.cs` (chama a demo)
- **NOVO:** `tests/Kingdom.Persistence.Tests/SqliteDemoTests.cs`

Código do engine sem mudanças. Código JSON sem mudanças. SQLite só adiciona algo novo — um segundo jeito de salvar, bem ao lado do JSON.

## Passo 0 — instale o pacote

```powershell
cd Kingdom.Persistence
dotnet add package Microsoft.Data.Sqlite
```

Isso adiciona `SqliteConnection` e as classes relacionadas ao seu projeto.

## Passo 1 — `SqliteDemo`

`Kingdom.Persistence/SqliteDemo.cs`:

```csharp
using Microsoft.Data.Sqlite;

namespace Kingdom.Persistence;

public static class SqliteDemo
{
    /// <summary>Roda uma sequência pequena de ponta a ponta: abrir, criar, inserir, consultar.</summary>
    public static IReadOnlyList<(int Id, string Name, int Day, int Gold)> RunDemo(string dbPath)
    {
        // A connection string diz "o arquivo está em <path>".
        // Pooling=False para o handle de arquivo do OS ser liberado no Dispose.
        // (Pooling é ótimo em produção mas mantém o arquivo bloqueado, o que atrapalha em testes.)
        var connStr = $"Data Source={dbPath};Pooling=False";

        using var conn = new SqliteConnection(connStr);
        conn.Open();

        // CREATE TABLE — IF NOT EXISTS torna idempotente
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS kingdoms (
                    id   INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    day  INTEGER NOT NULL,
                    gold INTEGER NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();
        }

        // INSERT — note: parâmetros, não concatenação de string (segurança contra SQL injection!)
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "INSERT INTO kingdoms (name, day, gold) VALUES ($name, $day, $gold)";
            cmd.Parameters.AddWithValue("$name", "Eldoria");
            cmd.Parameters.AddWithValue("$day", 11);
            cmd.Parameters.AddWithValue("$gold", 250);
            cmd.ExecuteNonQuery();
        }

        // SELECT — itera as rows
        var results = new List<(int, string, int, int)>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT id, name, day, gold FROM kingdoms ORDER BY id";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add((
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3)));
            }
        }

        return results;
    }
}
```

Três coisas para ler com atenção:

1. **`using var conn = new SqliteConnection(...)`.** A palavra-chave `using` garante que `Dispose` rode sozinho, o que fecha a conexão. Sempre use `using` para conexões, comandos e readers. Se não usar, o programa mantém handles de arquivo abertos que deveria ter liberado.
2. **Parâmetros `$name`, `$day`, `$gold`.** *Nunca* cole entrada de usuário direto numa string SQL. Isso te expõe a SQL injection (mais sobre isso abaixo). Use parâmetros. O SQLite os escapa para você, de forma segura.
3. **`reader.GetInt32(0)`.** Lê a primeira coluna como int. O reader te entrega uma row por vez, em ordem.

## Passo 2 — chame pelo console

`Kingdom.Console/Program.cs` — adicione no final:

```csharp
// Demo SQLite (Módulo 2.4)
var dbPath = Path.Combine(saveFolder, "kingdoms.db");
if (File.Exists(dbPath)) File.Delete(dbPath);   // começa do zero a cada rodada
var rows = SqliteDemo.RunDemo(dbPath);

Console.WriteLine();
Console.WriteLine($"=== SQLite demo — {rows.Count} row(s) ===");
foreach (var (id, name, day, gold) in rows)
    Console.WriteLine($"  #{id}  {name}, day {day}, gold {gold}");
```

Rode:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Você vai ver a row voltar do banco de dados. Encontre o arquivo aqui:

```
bin/Debug/net10.0/saves/kingdoms.db
```

Você pode abri-lo em qualquer navegador de SQLite (DB Browser for SQLite é gratuito). Um arquivo, e você pode fazer perguntas a ele a qualquer hora.

## Passo 3 — testes

`tests/Kingdom.Persistence.Tests/SqliteDemoTests.cs`:

```csharp
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SqliteDemoTests
{
    [Fact]
    public void RunDemo_FirstRun_ReturnsOneRow()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kdb-{Guid.NewGuid():N}.db");
        try
        {
            var rows = SqliteDemo.RunDemo(path);
            rows.Count.ShouldBe(1);
            rows[0].Name.ShouldBe("Eldoria");
            rows[0].Day.ShouldBe(11);
            rows[0].Gold.ShouldBe(250);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void RunDemo_TwoRuns_AccumulatesRows()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kdb-{Guid.NewGuid():N}.db");
        try
        {
            SqliteDemo.RunDemo(path);
            var rows = SqliteDemo.RunDemo(path);   // CREATE IF NOT EXISTS, segundo INSERT
            rows.Count.ShouldBe(2);
            rows[0].Id.ShouldBe(1);
            rows[1].Id.ShouldBe(2);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void DatabaseFile_IsCreated_OnFirstRun()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kdb-{Guid.NewGuid():N}.db");
        try
        {
            File.Exists(path).ShouldBeFalse();
            SqliteDemo.RunDemo(path);
            File.Exists(path).ShouldBeTrue();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Rode:

```powershell
dotnet test
```

Espere `Passed: 54` (51 + 3).

## Mexa um pouco

Adicione uma coluna: `wood INTEGER NOT NULL DEFAULT 0`. `ALTER TABLE` funciona — as rows que já existem recebem o valor padrão. (A gente vai cobrir migrações direito no Módulo 2.7.)

Rode uma query direto da linha de comando `sqlite3`: `sqlite3 saves/kingdoms.db "SELECT * FROM kingdoms"`. Mesmos dados, uma ferramenta diferente.

Tente colar entrada de usuário na string SQL: `cmd.CommandText = "INSERT INTO kingdoms (name) VALUES ('" + userInput + "')";`. Se `userInput` for `'); DROP TABLE kingdoms; --`, sua tabela inteira é apagada. Isso é SQL injection — e exatamente por isso sempre usamos parâmetros.

Insira 10.000 rows num loop. Veja como o SQLite é rápido com pequenas quantidades de dados. (Para grandes lotes de inserções, coloque dentro de `BEGIN TRANSACTION ... COMMIT` — isso torna cerca de 100 vezes mais rápido.)

## O que você acabou de fazer

O seu reino agora tem um terceiro lugar para viver: um arquivo de banco de dados SQLite. Você escreveu o seu primeiro `CREATE TABLE`, o seu primeiro `INSERT` com parâmetros, e o seu primeiro loop de reader com `SELECT` — mais três testes para provar a ida e volta (54 passando no total). Pelo caminho você conheceu os cinco comandos SQL (`CREATE`, `INSERT`, `SELECT`, `UPDATE`, `DELETE`), que juntos cobrem a maior parte do que você vai escrever pelo resto do ano. Você também conheceu o bug de segurança mais comum do mundo — *SQL injection* — e viu por que usar parâmetros, nunca colar entrada na string, é a única resposta segura. O engine e o código JSON não mudaram hoje. Este é o terceiro shell sobre o mesmo engine.

**Conceitos que você já sabe nomear:**

- **SQL** — a linguagem que bancos de dados relacionais falam
- **SQLite** — uma biblioteca + um banco de dados de arquivo único
- **CREATE / INSERT / SELECT / UPDATE / DELETE** — os cinco comandos centrais
- **parâmetros (`$name`)** — o único jeito seguro de colocar entrada de usuário em SQL
- **`using var conn`** — garante que a conexão feche quando o escopo termina

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: escrever SQL que cria uma tabela, coloca uma row nela e lê de volta. Ninguém corrige isso — o motor do banco de dados faz, que é o ponto. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra o prompt `sqlite3` num arquivo novo: `sqlite3 scratch.db`. Sem olhar, escreva SQL que:

1. Cria uma tabela chamada `heroes` — um `id`, um `name`, e um `level`.
2. Insere um herói.
3. Seleciona cada row de volta.

Você deve ver sua row voltar.

<details><summary>Travou? Abra aqui para conferir.</summary>

```sql
CREATE TABLE heroes (
    id    INTEGER PRIMARY KEY AUTOINCREMENT,
    name  TEXT NOT NULL,
    level INTEGER NOT NULL
);

INSERT INTO heroes (name, level) VALUES ('Lyra', 7);

SELECT id, name, level FROM heroes;
```

Se o `SELECT` mostrar seu herói, os três comandos funcionaram. O mesmo formato `CREATE` / `INSERT` / `SELECT` é a maior parte do SQL que você vai escrever o ano inteiro.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.4 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 2.5 apresenta os **JOINs** — o recurso do SQL que torna bancos de dados relacionais *relacionais*. A gente vai adicionar uma tabela `buildings` que se liga de volta a `kingdoms`, e aprender como consultar as duas tabelas ao mesmo tempo.
