# Módulo 2.2 — Serialização JSON

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Ontem você escreveu cinco linhas de texto simples no disco. Hoje o reino aprende a se escrever como **JSON** — o formato `{ "Name": "Eldoria", "Day": 11, ... }` usado por quase todas as APIs na internet. Você também vai adicionar um projeto novo — `Kingdom.Persistence` — para manter o código JSON separado do engine e do console. O mesmo reino, um terceiro shell.

JSON é também onde você encontra um padrão pequeno mas importante: o **DTO**. As letras significam *Data Transfer Object*. É um record pequeno que guarda só dados, feito para um único trabalho: mover esses dados por uma fronteira, como escrevê-los no disco ou mandá-los pela rede. Você vai usar DTOs de novo em todas as fases a partir de agora. A gente os apresenta de verdade hoje.

> **Words to watch**
>
> - **JSON** *(jay-son)* — *JavaScript Object Notation*. Texto simples no formato `{ "key": value, ... }`. Universalmente legível.
> - **serialise** — transformar um objeto em uma string (ou bytes). *Deserialise* é o caminho inverso.
> - **`System.Text.Json`** — a biblioteca JSON moderna do .NET. Já vem incluída. Use esta em vez da antiga Newtonsoft.
> - **DTO** — *Data Transfer Object*. Um record pequeno de dados feito para cruzar uma fronteira (disco, rede etc.).

---

## Por que JSON, por que um novo projeto

A Fase 1 mostrou que *records imutáveis pequenos* são um bom jeito de modelar dados. JSON é o que records viram quando saem do programa — para ir para o disco, para a rede, ou até para outra linguagem. Um `record` em C# converte para JSON e volta limpo, com quase nenhum trabalho extra.

O novo projeto — `Kingdom.Persistence` — existe por um motivo: o engine não deve saber sobre JSON. Se colocarmos código JSON em `Kingdom.Engine`, todo shell construído em cima dele vai carregar o JSON junto, mesmo quando não precisar. (Roblox não usa JSON; o shell de banco de dados usa SQL, não JSON.) Ao dar ao JSON o seu próprio projeto, cada shell pode escolher se quer usá-lo.

Aqui está quais projetos dependem de quais:

```
Kingdom.Console  ──┐
                   ├──► Kingdom.Persistence  ──►  Kingdom.Engine
Kingdom.Persistence┤
                   └──► Kingdom.Engine
```

Console depende dos dois. Persistence depende do Engine. Engine não depende de nada.

## Delta starter

- **NOVO projeto:** `Kingdom.Persistence/` (classlib) com `KingdomSummary.cs` + `KingdomJsonStore.cs`
- **NOVO:** `Kingdom.Persistence/Kingdom.Persistence.csproj`
- **MODIFICADO:** `Kingdom.slnx` (adiciona o novo projeto)
- **MODIFICADO:** `Kingdom.Console/Program.cs` (usa o store)
- **MODIFICADO:** `Kingdom.Console/Kingdom.Console.csproj` (referência de projeto para Persistence)
- **NOVO:** `tests/Kingdom.Persistence.Tests/` projeto de testes (ou adicione testes ao projeto de testes do engine — veja o Passo 5)

## Passo 1 — configure o novo projeto

Da raiz do repositório:

```powershell
dotnet new classlib -n Kingdom.Persistence
dotnet add Kingdom.Persistence reference Kingdom.Engine
dotnet add Kingdom.Console reference Kingdom.Persistence
dotnet sln Kingdom.slnx add Kingdom.Persistence
```

Agora você tem um terceiro projeto ao lado de `Kingdom.Engine` e `Kingdom.Console`.

## Passo 2 — o record DTO

`Kingdom.Persistence/KingdomSummary.cs`:

```csharp
namespace Kingdom.Persistence;

/// <summary>
/// Um snapshot pequeno só de dados de um reino. Feito para o disco/rede —
/// não é o modelo do engine. Perde dados de propósito (elimina EventLog, estado interno).
/// </summary>
public record KingdomSummary(
    string Name,
    int Day,
    int BuildingCount,
    int CitizenCount,
    int Gold,
    int Wood,
    int Stone,
    int Food
);
```

Por que um record separado, em vez de salvar o `Kingdom` diretamente? A serialização JSON lê de propriedades. Mas `Kingdom` tem interfaces, campos privados, e um construtor que precisa de `IRandom` e `IClock`. O `JsonSerializer` não sabe o que fazer com nada disso. Um DTO feito para o trabalho é muito mais simples — ele guarda só o que nos interessa, na forma que queremos.

Este é o **padrão DTO.** Você vai vê-lo em todo lugar: APIs, filas de mensagens, formatos de arquivo. Sempre tem um record pequeno e separado na fronteira.

## Passo 3 — o store

`Kingdom.Persistence/KingdomJsonStore.cs`:

```csharp
using System.Text.Json;
using Kingdom.Engine;
using Kingdom.Engine.Resources;

namespace Kingdom.Persistence;

public class KingdomJsonStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true       // saída legível por humanos; mude para false para compactar
    };

    public void Save(Kingdom.Engine.Kingdom kingdom, string path)
    {
        var summary = ToSummary(kingdom);
        var json = JsonSerializer.Serialize(summary, Options);
        File.WriteAllText(path, json);
    }

    public KingdomSummary Load(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<KingdomSummary>(json)
            ?? throw new InvalidOperationException("Could not deserialize kingdom.");
    }

    public static KingdomSummary ToSummary(Kingdom.Engine.Kingdom k) =>
        new(
            k.Name,
            k.Day,
            k.Buildings.Count,
            k.Citizens.Count,
            k.Resources.Get(Resource.Gold),
            k.Resources.Get(Resource.Wood),
            k.Resources.Get(Resource.Stone),
            k.Resources.Get(Resource.Food)
        );
}
```

`JsonSerializer.Serialize(value, options)` transforma o record em JSON. `Deserialize<T>(json)` transforma JSON de volta em `T`. Para um `record`, é isso que você precisa — sem atributos, sem mapeamento na mão.

`WriteIndented = true` te dá JSON em várias linhas, fácil de ler. Coloque `false` quando não precisar mais ler o arquivo você mesmo.

## Passo 4 — use pelo console

`Kingdom.Console/Program.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;

IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(saveFolder);
var savePath = Path.Combine(saveFolder, "kingdom.json");

var store = new KingdomJsonStore();
store.Save(kingdom, savePath);
Console.WriteLine($"Saved to {savePath}");

var loaded = store.Load(savePath);
Console.WriteLine();
Console.WriteLine("=== Loaded summary ===");
Console.WriteLine($"  Name: {loaded.Name}");
Console.WriteLine($"  Day:  {loaded.Day}");
Console.WriteLine($"  Buildings: {loaded.BuildingCount}, Citizens: {loaded.CitizenCount}");
Console.WriteLine($"  Gold: {loaded.Gold}, Wood: {loaded.Wood}, Stone: {loaded.Stone}, Food: {loaded.Food}");
```

Compile e rode:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Abra `bin/Debug/net10.0/saves/kingdom.json` — JSON limpo e indentado. Você pode até editar ele na mão no Bloco de Notas e ele ainda vai carregar de volta. Isso é o lado bom dos formatos de texto simples.

## Passo 5 — testes

Uma escolha a fazer: os testes de persistência vão para o projeto `Kingdom.Engine.Tests` existente ou para um novo projeto `Kingdom.Persistence.Tests`?

- **Adicionar ao existente:** mais simples. Os testes referenciam tanto o Engine quanto o Persistence.
- **Novo projeto:** mais limpo — cada biblioteca tem seus próprios testes. É como codebases maiores fazem.

A gente vai com o **novo projeto** — ele ensina o padrão certo. Da raiz do repositório:

```powershell
dotnet new xunit -n Kingdom.Persistence.Tests -o tests/Kingdom.Persistence.Tests
dotnet add tests/Kingdom.Persistence.Tests reference Kingdom.Engine
dotnet add tests/Kingdom.Persistence.Tests reference Kingdom.Persistence
dotnet add tests/Kingdom.Persistence.Tests package Shouldly
dotnet sln Kingdom.slnx add tests/Kingdom.Persistence.Tests
```

`tests/Kingdom.Persistence.Tests/KingdomJsonStoreTests.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class KingdomJsonStoreTests
{
    [Fact]
    public void Save_ThenLoad_RoundtripsName()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kingdom-{Guid.NewGuid():N}.json");
        try
        {
            var k = new global::Kingdom.Engine.Kingdom("Roundtripper", new SystemRandom(7), new SystemClock());
            var store = new KingdomJsonStore();
            store.Save(k, path);
            var loaded = store.Load(path);
            loaded.Name.ShouldBe("Roundtripper");
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Save_ProducesIndentedJson()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kingdom-{Guid.NewGuid():N}.json");
        try
        {
            var k = new global::Kingdom.Engine.Kingdom("X");
            new KingdomJsonStore().Save(k, path);
            var raw = File.ReadAllText(path);
            raw.ShouldContain("\n");                // várias linhas
            raw.ShouldContain("\"Name\": \"X\"");   // forma indentada
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void ToSummary_CapturesAllKnownFields()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddCitizen(new Citizen("A"));
        for (int i = 0; i < 5; i++) k.AdvanceDay();

        var s = KingdomJsonStore.ToSummary(k);
        s.Name.ShouldBe("Test");
        s.Day.ShouldBe(6);
        s.BuildingCount.ShouldBe(1);
        s.CitizenCount.ShouldBe(1);
    }

    [Fact]
    public void Load_MissingFile_Throws()
    {
        var store = new KingdomJsonStore();
        var path = Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.json");
        Should.Throw<FileNotFoundException>(() => store.Load(path));
    }

    [Fact]
    public void Load_InvalidJson_ThrowsJsonException()
    {
        var path = Path.Combine(Path.GetTempPath(), $"bad-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path, "{ this is not json");
            var store = new KingdomJsonStore();
            Should.Throw<System.Text.Json.JsonException>(() => store.Load(path));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Rode todos os testes dos dois projetos:

```powershell
dotnet test
```

Espere `Passed: 43` (38 do Módulo 2.1 + 5 novos no projeto de testes de persistência).

## Mexa um pouco

Coloque `WriteIndented = false`. Salve um reino. O JSON agora é uma linha — os mesmos dados, muito menor. Essa é a forma que vai pela rede.

Adicione uma propriedade ao `KingdomSummary` — digamos, `int FarmCount`. Rode o teste que carrega um arquivo JSON antigo (sem `FarmCount`). Ele carrega bem, com `FarmCount = 0` (o valor padrão). JSON não reclama quando um campo falta.

Adicione `[JsonPropertyName("name")]` acima de `Name` no record. Agora a saída JSON usa `"name"` em minúscula. Isso é útil quando você precisa bater com uma API que já existe.

Tente salvar mil reinos num loop, cada um com um nome diferente. Salvar começa a parecer lento. Esse é o ponto onde um banco de dados faz mais sentido.

## O que você acabou de fazer

Você construiu o seu primeiro formato de salvamento de verdade. O mesmo reino de ontem, mas agora ele serializa para JSON e carrega de volta de forma limpa — cinco testes provam isso (43 passando no total). Pelo caminho você adicionou um terceiro projeto, `Kingdom.Persistence`, para o engine não carregar JSON para shells que não querem. Você também conheceu o padrão **DTO**, que você vai ver em toda fase a partir de agora: um record pequeno que guarda só dados, feito para cruzar uma fronteira, mantido separado do modelo do engine. O projeto do engine ainda não tem mudanças — dois módulos seguidos.

**Conceitos que você já sabe nomear:**

- **JSON** — formato universal de dados em texto simples
- **serialise / deserialise** — objeto-para-texto, texto-para-objeto
- **`JsonSerializer`** — `Serialize(value, opts)`, `Deserialize<T>(json)`
- **DTO** — record pequeno de dados numa fronteira
- **`WriteIndented`** — JSON legível vs compacto

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: transformar um record em JSON, depois transformar o JSON de volta num record. Ninguém corrige isso — é só para você. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Faça um `record Player(string Name, int Score)` pequeno. Depois, sem olhar:

1. Serialize um `Player` para uma string JSON com `JsonSerializer.Serialize(...)`.
2. Imprima a string para você poder vê-la.
3. Deserialize de volta para um `Player` com `JsonSerializer.Deserialize<Player>(...)`.
4. Rode — o nome e a pontuação que você receber de volta devem ser iguais aos que você colocou.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
using System.Text.Json;

var player = new Player("Lyra", 250);

var json = JsonSerializer.Serialize(player);
Console.WriteLine(json);                 // {"Name":"Lyra","Score":250}

var back = JsonSerializer.Deserialize<Player>(json);
Console.WriteLine($"{back!.Name} {back.Score}");   // Lyra 250

record Player(string Name, int Score);
```

Um `record` não precisa de nenhuma configuração extra — `Serialize` lê suas propriedades, `Deserialize<T>` preenche elas de volta. Essa é a ideia inteira.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.2 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

Módulo 2.3 — **testes de ida e volta** — leva o padrão de hoje mais longe: qualquer reino que você salvar deve bater consigo mesmo quando você carregar de volta. A gente vai escrever o snapshot *completo* (não só o resumo) e provar que o estado do engine pode ser reconstruído exatamente.
