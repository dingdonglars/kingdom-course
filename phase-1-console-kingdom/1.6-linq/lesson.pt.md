# Módulo 1.6 — LINQ on the Kingdom

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje há quase nenhuma linha nova de código, mas uma mudança real em como você *pensa*. O reino tem listas — prédios, cidadãos. Até agora, toda vez que você queria saber algo sobre elas ("quantas fazendas?", "qual prédio tem o nível mais alto?"), você escrevia um loop `for`. Hoje você para. **LINQ** (pronuncia "link") deixa você descrever a resposta em vez de escrever o loop.

LINQ significa *Language Integrated Query*. É um conjunto de métodos auxiliares (`Where`, `Select`, `Count`, `Sum`, `Any`, `OrderBy`, `First`, e mais) que funcionam em toda coleção do C#. Está embutido na linguagem — nada para instalar, só `using System.Linq;` (muitas vezes já adicionado para você). Uma vez que você conhece uns vinte desses métodos, você quase nunca escreve um loop só para contar ou filtrar de novo.

Aqui está a mudança real, e é uma forma de pensar, não só código mais curto. Com um loop `for` você explica *como* obter a resposta — começa um contador, percorre cada item, adiciona um a cada vez. Com LINQ você descreve *o que* quer — "dos prédios, as fazendas, contadas" — e deixa o C# fazer o percurso. Uma vez que você começa a ver as perguntas do seu reino como descrições em vez de loops, muito do código fica mais curto e mais fácil de ler de relance.

> **Words to watch**
>
> - **LINQ** (Language Integrated Query) — um conjunto de métodos (`Where`, `Select`, `Sum`, `Count`, `Any`, `OrderBy`, `First`) que funcionam em qualquer coleção
> - **predicate** — uma função que retorna `bool`. `Where(b => b.Level > 1)` recebe um predicate.
> - **lambda** — a sintaxe `x => ...`. Uma função descartável escrita na hora.
> - **deferred execution** — o LINQ não roda imediatamente. Ele constrói uma receita; a receita roda quando você pede pelos resultados.
> - **extension method** — um método estático cujo primeiro parâmetro tem `this`, chamado como se fosse um método de instância

---

## Por que LINQ

Suponha que você pergunte: *"Quantas fazendas meu reino tem?"*

Sem LINQ:

```csharp
int farmCount = 0;
foreach (var b in kingdom.Buildings)
    if (b is Farm) farmCount++;
```

Três linhas. Fácil de escrever. Fácil de ler uma vez. Mas você vai escrever cinquenta desses em um reino deste tamanho, e depois de um tempo todos parecem iguais e são difíceis de distinguir.

Com LINQ:

```csharp
int farmCount = kingdom.Buildings.OfType<Farm>().Count();
```

Uma linha. Se lê como *"dos prédios, os que são fazendas — conte."*

## Passo 1 — seis métodos para conhecer

Digite esses em um arquivo avulso para experimentar.

```csharp
using System.Linq;

var nums = new List<int> { 1, 2, 3, 4, 5 };

// Where — keep only the items that match the predicate
nums.Where(n => n > 2);              // 3, 4, 5

// Select — transform each item
nums.Select(n => n * n);             // 1, 4, 9, 16, 25

// Count — how many (optionally with a predicate)
nums.Count();                        // 5
nums.Count(n => n > 2);              // 3

// Sum — add them up
nums.Sum();                          // 15
nums.Sum(n => n * 10);               // 150 (sums after transforming)

// Any / All — return bool
nums.Any(n => n > 4);                // true (at least one)
nums.All(n => n > 0);                // true (every one)

// First / FirstOrDefault — get one item
nums.First(n => n > 3);              // 4 (throws if no match)
nums.FirstOrDefault(n => n > 100);   // 0 (returns default if no match)

// OrderBy / OrderByDescending — sort
nums.OrderByDescending(n => n);      // 5, 4, 3, 2, 1
```

Isso é cerca de 90% do LINQ que você vai precisar algum dia.

## Passo 2 — aplique ao reino

Este módulo muda só `Program.cs` e adiciona um arquivo de testes. As classes do engine não mudam.

- **MODIFICADO:** `Kingdom.Console/Program.cs` (usa LINQ para o relatório)
- **NOVO:** `tests/Kingdom.Engine.Tests/LinqTests.cs`

Atualize `Program.cs` para imprimir um resumo mais completo usando LINQ:

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Farm("River Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));
kingdom.AddCitizen(new Citizen("Mira"));

PrintReport(kingdom);

for (int day = 0; day < 5; day++)
    kingdom.AdvanceDay();

PrintReport(kingdom);

void PrintReport(Kingdom.Engine.Kingdom k)
{
    Console.WriteLine();
    Console.WriteLine($"== Day {k.Day} — {k.Name} ==");

    var farms = k.Buildings.OfType<Farm>().Count();
    var lumberyards = k.Buildings.OfType<Lumberyard>().Count();
    var mines = k.Buildings.OfType<Mine>().Count();
    var totalLevels = k.Buildings.Sum(b => b.Level);
    var topBuilding = k.Buildings
        .OrderByDescending(b => b.Level)
        .First();

    Console.WriteLine($"Buildings: {k.Buildings.Count} ({farms} farm, {lumberyards} lumberyard, {mines} mine) — total levels {totalLevels}");
    Console.WriteLine($"Top building: {topBuilding.GetType().Name} '{topBuilding.Name}' (level {topBuilding.Level})");
    Console.WriteLine($"Citizens: {k.Citizens.Count}");

    var foodPerDay = k.Buildings.OfType<Farm>().Sum(f => 5 * f.Level) - k.Citizens.Count;
    Console.WriteLine($"Food net per day: {foodPerDay:+0;-0;0}");

    Console.Write("Resources: ");
    foreach (var (r, n) in k.Resources.Snapshot())
        Console.Write($"{r}={n}  ");
    Console.WriteLine();
}
```

Leia cada linha LINQ devagar. `OfType<Farm>()` mantém os itens que são fazendas. `.Count()` diz quantos. `Sum(b => b.Level)` soma `b.Level` em cada prédio. A cadeia de `topBuilding` ordena os prédios do nível mais alto ao mais baixo e pega o primeiro. A linha de comida por dia diz *"some (level × 5) nas fazendas, depois subtraia o número de cidadãos."*

`{foodPerDay:+0;-0;0}` é uma string de formato para a impressão — números positivos ganham um `+`, os negativos mantêm o `-`, e zero fica `0`. Uma coisa pequena que torna o resultado mais fácil de ler.

Build e rode:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

No Dia 1 você vai ver algo como *"Food net per day: +7"* — duas fazendas vezes cinco dá dez, menos três cidadãos dá sete.

## Passo 3 — um auxiliar `KingdomStats` (opcional)

Se `Program.cs` está começando a parecer cheio, mova o LINQ para dentro do engine.

`Kingdom.Engine/KingdomStats.cs`:

```csharp
namespace Kingdom.Engine;

public static class KingdomStats
{
    public static int FarmCount(this Kingdom k) => k.Buildings.OfType<Farm>().Count();
    public static int LumberyardCount(this Kingdom k) => k.Buildings.OfType<Lumberyard>().Count();
    public static int MineCount(this Kingdom k) => k.Buildings.OfType<Mine>().Count();
    public static int TotalBuildingLevels(this Kingdom k) => k.Buildings.Sum(b => b.Level);
    public static Building? TopBuilding(this Kingdom k) =>
        k.Buildings
            .OrderByDescending(b => b.Level)
            .FirstOrDefault();
    public static int DailyFoodNet(this Kingdom k) =>
        k.Buildings.OfType<Farm>().Sum(f => 5 * f.Level) - k.Citizens.Count;
}
```

Repare no `this Kingdom k` no primeiro parâmetro. A palavra-chave `this` transforma cada um desses em um **extension method** — um método estático que *parece* um método no objeto quando você o chama:

```csharp
kingdom.FarmCount();   // really KingdomStats.FarmCount(kingdom)
```

Extension methods são como o próprio LINQ funciona. `Where`, `Select` e o restante são todos extension methods em `IEnumerable<T>` — é por isso que toda coleção em C# os tem automaticamente.

## Passo 4 — teste

`tests/Kingdom.Engine.Tests/LinqTests.cs`:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class LinqTests
{
    [Fact]
    public void OfType_FiltersToFarmsOnly()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F1"));
        k.AddBuilding(new Lumberyard("L1"));
        k.AddBuilding(new Farm("F2"));

        k.Buildings.OfType<Farm>().Count().ShouldBe(2);
    }

    [Fact]
    public void Sum_OfBuildingLevels_AddsUp()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F1"));
        var f2 = new Farm("F2");
        f2.Upgrade(); f2.Upgrade();      // level 3
        k.AddBuilding(f2);

        k.Buildings.Sum(b => b.Level).ShouldBe(4);   // 1 + 3
    }

    [Fact]
    public void OrderByDescending_TopBuilding_IsHighestLevel()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F1"));
        var top = new Mine("Big Mine");
        top.Upgrade(); top.Upgrade(); top.Upgrade();  // level 4
        k.AddBuilding(top);
        k.AddBuilding(new Lumberyard("L1"));

        k.Buildings
            .OrderByDescending(b => b.Level)
            .First()
            .Name
            .ShouldBe("Big Mine");
    }

    [Fact]
    public void Any_NoFarms_ReturnsFalse()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Mine("M"));
        k.Buildings.Any(b => b is Farm).ShouldBeFalse();
    }

    [Fact]
    public void All_AllAtLevelOne_ReturnsTrue()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Mine("M"));
        k.Buildings.All(b => b.Level == 1).ShouldBeTrue();
    }
}
```

Rode:

```powershell
dotnet test
```

Você deve ver `Passed: 27` (22 mais 5 novos).

## Mexa um pouco

Substitua `OfType<Farm>().Count()` por `Count(b => b is Farm)`. Os dois funcionam — o segundo usa a palavra-chave `is` direto na chamada. Escolha o que você achar mais fácil de ler.

Ordene os prédios do nível mais baixo ao mais alto com `OrderBy(b => b.Level)`. Imprima o resultado. A saída parece uma lista classificada.

Pegue o prédio com o segundo nível mais alto com `.OrderByDescending(b => b.Level).Skip(1).First()`. `Skip` é um dos métodos menos comuns, mas é útil quando você precisa.

Tente `kingdom.Buildings.Where(b => b.Level > 5).First()` quando nenhum prédio tem nível acima de 5. Ele lança um erro. Agora troque para `FirstOrDefault()`. Ele retorna `null` em vez disso. Essa é a diferença — `First` significa *"deveria ter um aqui"*, e `FirstOrDefault` significa *"se não tiver, tudo bem."*

## Uma nota sobre deferred execution

`Where(...)` não filtra de verdade a lista no momento em que você o chama. Ele devolve uma *receita* — uma consulta que sabe como filtrar, mas ainda não rodou. O trabalho real acontece quando você chama `.ToList()`, `.Count()`, `.First()`, ou percorre com `foreach`. Isso é chamado de **deferred execution** (deferred significa "deixado para depois"). É um detalhe pequeno, mas vale saber: se você guardar um resultado de `Where(...)` em uma variável, depois mudar a lista fonte, a próxima vez que você percorrer ela vai ter o resultado filtrado novo, não o antigo. Vamos trazer isso de volta se algum dia causar um problema. Por enquanto, só saiba que o LINQ espera até o último momento para fazer seu trabalho.

## A linha-guia

A linha-guia neste módulo: **descreva o resultado, não o loop**. Quando você quer *dizer o que quer* de uma coleção, use LINQ. Quando você quer *fazer algo com side effects* (imprimir, salvar, mudar), um `foreach` está bem. De agora em diante, todo `for` que você escrever neste código vale uma segunda olhada — o LINQ poderia dizer isso mais claramente?

## O que você acabou de fazer

Você conheceu o LINQ — uns dez métodos que cobrem quase todo "faça a lista uma pergunta" que você vai escrever este ano. Você os usou para imprimir um relatório mais completo do reino, e escreveu cinco testes que provam que as consultas fazem o que você disse. Você também conheceu *extension methods*, que é o que faz o LINQ parecer embutido (toda coleção tem esses métodos porque alguém os adicionou a `IEnumerable<T>` uma vez). Vinte e sete testes passando agora.

**Conceitos que você já sabe nomear:**

- **LINQ** — métodos de consulta que funcionam em qualquer coleção
- **predicate** — uma função que retorna verdadeiro ou falso, passada para `Where`/`Any`/`All`
- **lambda** — `x => expr`, uma função curta escrita direto onde é usada
- **extension method** — método estático com um primeiro parâmetro `this`, chamado como método no objeto
- **deferred execution** — o LINQ espera, e roda quando você pede pelos resultados

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a única grande ideia pegou: fazer uma pergunta a uma lista com LINQ em vez de escrever um loop. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Aqui está o jeito antigo com loop `for` para contar as minas:

```csharp
int mineCount = 0;
foreach (var b in kingdom.Buildings)
    if (b is Mine) mineCount++;
```

Sem olhar, faça duas coisas:

1. Reescreva essa contagem como uma única linha LINQ.
2. Escreva mais uma linha LINQ por conta própria: o **total de todos os níveis de prédio** somados.

Imprima os dois e rode.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
int mineCount = kingdom.Buildings.OfType<Mine>().Count();
int totalLevels = kingdom.Buildings.Sum(b => b.Level);
```

`OfType<Mine>()` mantém só as minas, depois `.Count()` diz quantas. `Sum(b => b.Level)` soma `b.Level` em cada prédio. Uma linha cada, e se leem quase como a pergunta que você fez.

</details>

## Movimento git da semana — veja seu histórico como um grafo

Seu log de commits agora está longo o suficiente para ser desenhado como uma imagem. Quer ver?

No VS Code, instale a extensão **GitLens** se você ainda não tiver (barra lateral de Extensions, busque por *"GitLens"*). Depois `Ctrl + Shift + P` → *"GitLens: Show Commit Graph"*. A visão de grafo abre — cada commit que você fez, com linhas desenhadas entre cada commit e o anterior, e branches mostradas como faixas coloridas. Ler seu próprio grafo de commits é o primeiro passo para entendê-lo.

> **Ou no terminal:** `git log --oneline --graph --decorate --all`.

Explicamos como esse grafo realmente funciona em B3.1, se você quiser fazer esse bônus.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.6 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 1.7 apresenta **eventos** — um cidadão adoece, um prédio pega fogo, um comerciante visita. Cada evento vira um registro guardado em uma lista, e o LINQ será exatamente a ferramenta certa para *"me mostre os eventos dos últimos sete dias."*
