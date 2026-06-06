# Bônus B1.2 — Trocar o SQLite pelo SQL Server (Três Linhas)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Esta é a lição para a qual o bônus inteiro existe. Você vai mudar três linhas de configuração, regenerar as migrations e rodar os testes. Depois vai assistir cada teste passar sem tocar em uma única linha do código do engine. O ponto inteiro é o quão comum isso parece. A regra engine-vs-shell disse que a mudança seria pequena. Hoje é o dia que você prova isso.

Se isso parecer chato, o trabalho cuidadoso que você fez nas Fases 1 e 2 acabou de te pagar de volta. Chato de propósito. O engine tratou o banco de dados como uma peça que podia ser trocada, e agora a gente vai provar isso.

> **Words to watch**
>
> - **provider** — o adaptador do EF Core para um banco de dados específico (`UseSqlite(...)` vs `UseSqlServer(...)`)
> - **dialect** — pequenas diferenças de SQL entre bancos de dados (Identity vs AUTOINCREMENT, etc.). EF esconde essas diferenças.
> - **migration regen** — quando você muda de provider, você regenera os arquivos de migration porque a saída SQL é diferente

---

## Passo 1 — mudar o pacote

Abra `Kingdom.Persistence.csproj` e troque o pacote provider do EF Core:

```xml
<!-- Era -->
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.0" />

<!-- Agora -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
```

Essa é a linha um de três.

## Passo 2 — mudar a chamada do provider

Abra `KingdomDbContext.cs`. Encontre a linha onde você diz ao EF qual banco de dados usar:

```csharp
// Era
options.UseSqlite($"Data Source={_path};Pooling=False");

// Agora
options.UseSqlServer(
    $"Server=(localdb)\\MSSQLLocalDB;Database=Kingdom_{_path};" +
    "Trusted_Connection=True;TrustServerCertificate=true;");
```

O valor `_path` costumava ser o nome do arquivo SQLite. O SQL Server não trabalha com arquivos da mesma forma — ele tem bancos de dados com nome em um servidor. Então usamos `_path` como parte do nome do banco de dados em vez disso. Isso mantém cada slot de save separado, como era antes. (Escolha o nome que fizer sentido para você.)

Essas são as linhas dois e três. Três linhas no total — embora se você contar a mudança da connection string por conta própria, dá quatro. De qualquer jeito, é uma mudança de configuração, não uma reescrita.

## Passo 3 — regenerar os arquivos de migration

Arquivos de migration contêm SQL escrito para um provider. O SQLite diz `AUTOINCREMENT`; o SQL Server diz `IDENTITY(1,1)`. Mesmo código C# no lado do EF, SQL diferente no lado do banco de dados. Então quando você muda de provider, você tem que regenerar os arquivos de migration.

```powershell
# Apague as migrations antigas do SQLite
Remove-Item -Recurse Kingdom.Persistence/Migrations

# Gere novas para o SQL Server
dotnet ef migrations add InitialCreate `
    --project Kingdom.Persistence `
    --startup-project Kingdom.Console
```

> Um sistema real em produção nunca apaga suas migrations em um banco de dados ativo — isso jogaria fora o registro de como o schema mudou ao longo do tempo. Para uma mudança de prática em um banco de dados que não guarda nada importante, migrations novas são tranquilas.

## Passo 4 — rodar os testes

```powershell
dotnet build
dotnet test
```

Observe o contador. **Todos os testes passam.** Mesmo código do engine, mesma API de store, banco de dados diferente por baixo. O engine nunca percebeu a mudança.

Este é o momento para o qual o bônus existe. Pare e pense nisso por um segundo. Os testes que você escreveu contra o SQLite — os que verificam que reinos salvam, ledgers salvam e carregam de volta corretamente, e slots carregam por id — rodam sem mudança contra um banco de dados de uma empresa diferente. É isso que a regra engine-vs-shell estava prometendo o tempo todo.

## Passo 5 — confirmar com o sqlcmd

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT name FROM sys.databases"
```

Você deve ver entradas `Kingdom_*` na lista — uma por slot de save que os testes criaram. Escolha uma e olhe dentro:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -d Kingdom_test01 `
    -Q "SELECT TOP 5 * FROM Kingdoms"
```

Linhas reais, SQL Server real, escritas pelo seu código de engine que você não mudou. Esse é o resultado de três linhas de configuração.

## Mexa um pouco

Volte para o SQLite. A mesma mudança de três linhas, ao contrário. A mudança funciona da mesma forma nas duas direções — e essa é a prova de que o engine trata os dois bancos de dados da mesma forma.

Tente um terceiro provider. Adicione o pacote `Npgsql.EntityFrameworkCore.PostgreSQL`, troque `UseSqlServer(...)` por `UseNpgsql(...)`, aponte para uma instância Postgres (Docker está ótimo), regenere as migrations, rode os testes. Mesmo resultado. O mesmo resultado chato.

Ligue o logging do EF para ver o SQL que está sendo enviado de verdade:

```csharp
options.LogTo(Console.WriteLine, LogLevel.Information);
```

O C# acima dos seus métodos de store não mudou. Mas o SQL saindo pelo fundo é muito diferente entre providers. Essa é a camada de abstração fazendo seu trabalho.

## O que você acabou de fazer

Você provou que a regra engine-vs-shell não é só uma frase bonita — é um padrão real que sustenta o seu código. Três linhas de configuração (mais uma regeneração de migration) moveram o reino do SQLite para o SQL Server, e seus testes passaram sem mudança. O engine nunca ficou sabendo. Esse é o bônus inteiro, em um Passo 4. Todo o resto aqui — a instalação de ontem, o SSMS amanhã, a reflexão — está construído em torno desse único resultado.

**Conceitos que você já sabe nomear:**

- **provider** — o adaptador específico do banco de dados do EF Core
- **`UseSqlite` / `UseSqlServer` / `UseNpgsql`** — chamadas de configuração do provider, mesmo formato
- **migration regen** — muda o provider, regenera as migrations
- **dialect** — pequenas diferenças de SQL que o EF esconde para você
- **a regra engine-vs-shell paga de volta** — mesmo engine, storage substituível, testes provam isso

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, de memória:

1. Liste as três coisas que você edita para mover o EF Core do SQLite para o SQL Server.
2. Diga qual parte do seu código você deve regenerar depois da troca.
3. Diga a única coisa que *não* muda — a parte que prova o ponto inteiro do bônus.

<details><summary>Travou? Abra aqui para conferir.</summary>

As três coisas que você edita:

1. O pacote no `.csproj` — `Microsoft.EntityFrameworkCore.Sqlite` vira `Microsoft.EntityFrameworkCore.SqlServer`.
2. A chamada do provider — `options.UseSqlite(...)` vira `options.UseSqlServer(...)`.
3. A connection string dentro dessa chamada — o caminho do arquivo SQLite vira o formato do SQL Server (`Server=(localdb)\MSSQLLocalDB;...`).

Você deve regenerar os arquivos de migration — apague os antigos e rode `dotnet ef migrations add InitialCreate`, porque o SQL é diferente entre providers.

A parte que *não* muda: o seu código de engine e seus testes. Eles passam sem mudança. Essa é a prova.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B1.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B1.2 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

B1.3 apresenta o **SSMS** — SQL Server Management Studio — a GUI profissional para navegar e consultar o SQL Server. Cinco minutos para aprender o básico, e útil pelo resto da sua carreira.
