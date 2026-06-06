# Módulo 2.7 — Migrations

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

`EnsureCreated()` funciona para um banco de dados vazio. Mas o que acontece quando você lança a versão 1, os jogadores salvam dados com ela, e a versão 2 precisa de uma nova coluna? Você não pode só apagar o banco de dados e recriá-lo — perderia cada save. Hoje a gente conhece as **migrations** — o jeito certo de mudar a estrutura do banco de dados passo a passo, com cada passo registrado.

Uma *migration* (no sentido de banco de dados) é uma pequena mudança de estrutura registrada. Ela tem um passo "Up" que faz a mudança e um passo "Down" que desfaz. A gente está usando a palavra pela primeira vez hoje. Você vai vê-la de novo na Fase 3 e mais tarde. A ideia é a mesma em qualquer lugar que um banco de dados é usado.

> **Words to watch**
>
> - **migration** — uma mudança de schema com versão. Cada uma tem um "Up" (aplicar) e "Down" (reverter).
> - **`dotnet ef`** — a ferramenta CLI do EF Core para gerar e aplicar migrations.
> - **`add`** — gera uma nova migration a partir do modelo de entity atual.
> - **`update`** — aplica migrations pendentes ao banco de dados.
> - **drift** — quando o schema do banco de dados não bate com o modelo. A coisa que migrations existem para evitar.

---

## Por que migrations

Imagine que a versão 1 do Kingdom está lançada. Os jogadores têm 50 reinos salvos. A versão 2 adiciona uma tabela `Citizens`. Você não pode:

- Apagar o banco de dados (você perderia os dados do jogador)
- Editar as tabelas existentes na mão (cada jogador teria que fazer isso)
- Torcer para o EF resolver (`EnsureCreated` não vai, e não deveria)

O que você precisa é **uma lista registrada de mudanças de estrutura**, aplicadas em ordem, sempre batendo com o código. Isso é uma migration.

```
00_InitialCreate     → CREATE TABLE kingdoms (...);
01_AddBuildings      → CREATE TABLE buildings (...);
02_AddCitizens       → CREATE TABLE citizens (...);
03_AddSavedAt        → ALTER TABLE kingdoms ADD COLUMN saved_at TEXT;
```

Cada migration tem um timestamp, um método "Up" (faz a mudança), e um método "Down" (desfaz). O EF mantém a lista das aplicadas numa tabela especial `__EFMigrationsHistory`. Quando você lança a v2, o EF vê que `00`, `01` e `02` já estão aplicadas e `03` não — então roda só `03`. Isso torna seguro mudar a estrutura entre lançamentos.

## Delta starter

Este módulo é mais sobre os passos que você roda do que sobre código novo. O starter:

- **MODIFICADO:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (adiciona o pacote `Microsoft.EntityFrameworkCore.Design`)
- **NOVO:** `Kingdom.Persistence/Migrations/` (pasta com os arquivos de migration gerados — criada pelo `dotnet ef migrations add`)
- **MODIFICADO:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — usa `Database.Migrate()` em vez de `EnsureCreated()`
- **NOVO:** `tests/Kingdom.Persistence.Tests/MigrationsTests.cs` — verifica que o caminho de aplicação de migration funciona

## Passo 0 — instale o pacote de design e a ferramenta

```powershell
cd Kingdom.Persistence
dotnet add package Microsoft.EntityFrameworkCore.Design

# instala a CLI dotnet-ef globalmente (uma vez só)
dotnet tool install --global dotnet-ef
# ou atualiza se já instalado: dotnet tool update --global dotnet-ef
```

`dotnet-ef` é uma ferramenta de linha de comando, não parte do seu programa. Ela escreve código enquanto você constrói, não enquanto o programa roda. Um app lançado não precisa dela.

## Passo 1 — gere a migration inicial

Da raiz do repositório:

```powershell
dotnet ef migrations add InitialCreate --project Kingdom.Persistence --startup-project Kingdom.Console
```

O que você vai ver:

- Uma nova pasta `Kingdom.Persistence/Migrations/` com arquivos como:
  - `20260503120000_InitialCreate.cs` — os métodos Up/Down
  - `20260503120000_InitialCreate.Designer.cs` — snapshot do modelo nesta migration
  - `KingdomDbContextModelSnapshot.cs` — o snapshot do modelo *atual*

O método `Up` tem chamadas `migrationBuilder.CreateTable(...)` — C# simples que você pode ler, que o EF transforma em SQL quando a migration é aplicada.

> **Project + startup-project.** A ferramenta de migration precisa *carregar o seu DbContext*, e para isso ela tem que iniciar um projeto. A gente usa `Kingdom.Console` porque é onde o programa começa. O código da migration em si vai em `Kingdom.Persistence`, onde `KingdomDbContext` vive.

## Passo 2 — aplique a migration

Dois jeitos:

**A. CLI:**

```powershell
dotnet ef database update --project Kingdom.Persistence --startup-project Kingdom.Console
```

Isso atualiza o banco de dados no caminho que o seu `OnConfiguring` está usando. Você vai ver `Applying migration '20260503120000_InitialCreate'`.

**B. Do código (mais útil num app de verdade):**

Em `Kingdom.Persistence/EfCore/KingdomEfStore.cs`, substitua `EnsureCreated()` por:

```csharp
public void EnsureCreated()
{
    using var ctx = new KingdomDbContext(_dbPath);
    ctx.Database.Migrate();   // aplica todas as migrations pendentes
}
```

Agora cada chamada de Save traz o banco de dados para o dia com o modelo atual. O jogador não precisa fazer nada.

`EnsureCreated` e `Migrate` *não funcionam juntos* — uma vez que um banco de dados é feito com `EnsureCreated`, o EF não aceita migrations nele, e o inverso também. Escolha um e fique com ele. **Migrate** é a resposta certa para um app de verdade.

## Passo 3 — mude o modelo e adicione uma segunda migration

Suponha que queremos rastrear quando cada save aconteceu. Adicione uma propriedade a `KingdomEntity`:

```csharp
public DateTime SavedAt { get; set; }
```

Gere uma nova migration:

```powershell
dotnet ef migrations add AddSavedAt --project Kingdom.Persistence --startup-project Kingdom.Console
```

O EF compara o modelo *atual* com o snapshot *último* e escreve só o que mudou:

```csharp
// Dentro do 20260503130000_AddSavedAt.cs gerado
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<DateTime>(
        name: "SavedAt",
        table: "Kingdoms",
        type: "TEXT",
        nullable: false,
        defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
}
```

Aplique: `dotnet ef database update --project Kingdom.Persistence --startup-project Kingdom.Console`. As rows que já existem recebem `SavedAt = 0001-01-01` (o padrão). Novas rows recebem o que você escrever.

(Para o starter a gente vai incluir só o `InitialCreate` — adicionar `SavedAt` fica para o Mexa um pouco.)

## Passo 4 — configure `SavedAt` em `Save`

Em `KingdomEfStore.Save`, adicione:

```csharp
SavedAt = DateTime.UtcNow,
```

no inicializador da entity. Agora cada save registra a hora em que aconteceu.

## Passo 5 — testes

`tests/Kingdom.Persistence.Tests/MigrationsTests.cs`:

```csharp
using Kingdom.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class MigrationsTests
{
    [Fact]
    public void Migrate_OnFreshDatabase_CreatesSchema()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mig-{Guid.NewGuid():N}.db");
        try
        {
            using (var ctx = new KingdomDbContext(path))
                ctx.Database.Migrate();

            using (var ctx = new KingdomDbContext(path))
                ctx.Kingdoms.Count().ShouldBe(0);   // tabela existe, vazia
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Migrate_OnAlreadyMigratedDatabase_IsIdempotent()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mig-{Guid.NewGuid():N}.db");
        try
        {
            using (var ctx = new KingdomDbContext(path)) ctx.Database.Migrate();
            using (var ctx = new KingdomDbContext(path)) ctx.Database.Migrate();   // sem efeito na segunda vez
            using (var ctx = new KingdomDbContext(path)) ctx.Kingdoms.Count().ShouldBe(0);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void GetPendingMigrations_AfterMigrate_IsEmpty()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mig-{Guid.NewGuid():N}.db");
        try
        {
            using var ctx = new KingdomDbContext(path);
            ctx.Database.Migrate();
            ctx.Database.GetPendingMigrations().ShouldBeEmpty();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Rode:

```powershell
dotnet test
```

Espere `Passed: 63` (60 + 3).

## Mexa um pouco

Adicione a migration `SavedAt` como descrito no Passo 3. Veja o SQL que ela escreve: `ALTER TABLE Kingdoms ADD ...`. Aplique num banco de dados que já tem rows. As rows antigas recebem o valor padrão, e nada é perdido. Esse é o ponto inteiro.

Rode `dotnet ef migrations remove --project Kingdom.Persistence --startup-project Kingdom.Console` para desfazer a *última migration que ainda não foi aplicada*. (Você não pode `remove` uma que já foi aplicada — para isso você usa `update <MigrationAnterior>`.)

Rode `dotnet ef migrations script --project Kingdom.Persistence --startup-project Kingdom.Console`. Ele imprime o SQL que as migrations rodariam. Num produto real, isso é útil quando um administrador de banco de dados quer revisar a mudança primeiro.

Abra o banco de dados no DB Browser. Procure a tabela `__EFMigrationsHistory`. É assim que o EF sabe quais migrations já aplicou.

## O que você acabou de fazer

Você foi de *"crie a estrutura se não existir"* para *"mude a estrutura conforme o código muda."* Você fez a sua primeira migration com `dotnet ef migrations add InitialCreate`, aplicou ela tanto pela linha de comando quanto pelo código, e provou que é seguro rodar esse passo de aplicação mais de uma vez (três novos testes, 63 passando no total). Você também conheceu `__EFMigrationsHistory` — a pequena tabela de registros que deixa uma lista de migrations mudar um banco de dados ativo com segurança. A partir daqui, uma mudança de estrutura é algo que você revisa em código, não algo que te assusta no dia do lançamento.

**Conceitos que você já sabe nomear:**

- **migration** — uma mudança de schema com versão, reversível
- **`dotnet ef migrations add`** — gera a partir do modelo
- **`Database.Migrate()`** — aplica migrations pendentes no código
- **`__EFMigrationsHistory`** — registro do EF de migrations aplicadas
- **schema drift** — banco de dados e modelo divergindo; o que migrations previnem

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: uma migration é uma mudança de estrutura registrada que você gera, depois aplica. Ninguém corrige isso — é só para você. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

De memória:

1. Rode o comando que gera uma nova migration a partir do modelo.
2. Rode o comando que a aplica ao banco de dados.
3. Depois diga em voz alta — ou anote — o que cada um fez, e onde os novos arquivos foram parar.

<details><summary>Travou? Abra aqui para conferir.</summary>

Os dois comandos:

```powershell
dotnet ef migrations add InitialCreate --project Kingdom.Persistence --startup-project Kingdom.Console
dotnet ef database update --project Kingdom.Persistence --startup-project Kingdom.Console
```

O que você deveria ter visto:

- `migrations add` criou uma pasta `Migrations/` com um arquivo `..._InitialCreate.cs` (os métodos `Up`/`Down`) mais um arquivo de snapshot do modelo.
- `database update` imprimiu `Applying migration '..._InitialCreate'` e criou ou atualizou o arquivo `.db`.
- O comando `add` escreve código; o `update` muda o banco de dados. Dois passos separados.
- Rode `update` uma segunda vez e nada acontece — a migration já foi aplicada. Esse é o comportamento seguro-de-rerrodar que você testou.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.7 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

Módulo 2.8 — **ferramentas de banco de dados** — cobre as *ferramentas* em torno dos bancos de dados. DB Browser for SQLite, a linha de comando `sqlite3`, e a extensão SQLTools do VS Code. Conhecer as ferramentas torna todo o trabalho mais fácil.
