# Módulo 1.7 — Events and Randomness

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

> **Aquecimento — 30 segundos, de cabeça.** Antes de hoje, traga de volta o que você construiu no Módulo 1.6:
>
> 1. O que um lambda como `b => b.Level > 1` entrega para um método como `Where`?
> 2. Cite um método LINQ que você usou no lugar de escrever um loop `for` na mão.
>
> Inseguro em alguma? Releia primeiro o **Módulo 1.6** — a aula de hoje fica bem em cima dele. Leve qualquer coisa que ficou frágil para o sync semanal.

O reino é previsível demais agora. As mesmas fazendas produzem a mesma comida, os mesmos cidadãos comem a mesma quantidade, todo dia para sempre. Hoje adicionamos sorte. A cada tick, há alguma chance de *algo acontecer*. Um comerciante chega com ouro. Um cidadão adoece. Um prédio pega fogo. Cada evento é salvo em uma lista, para que no Dia 50 o reino tenha uma história que você pode ler de volta.

Há duas razões para um log de eventos. A primeira é **a história** — um jogo de gestão sem eventos é só uma planilha. Visitantes aleatórios, incêndios e doenças são o que fazem o reino parecer vivo. A segunda é **a memória** — quando algo interessante acontece, você quer saber *quando* aconteceu. O log de eventos é a memória do reino. Com LINQ de ontem, você pode perguntar *"o que aconteceu nos últimos sete dias?"* em uma linha.

Tem um problema que vamos adicionar de propósito hoje e depois corrigir no Módulo 1.8. Os eventos vêm de `Random`, e usar `Random` diretamente dentro do engine torna o engine não-determinístico. Isso significa que o mesmo estado inicial produz eventos diferentes a cada execução, e você não consegue escrever um teste útil para algo que fica mudando. Você vai esbarrar nesse problema no Passo 5 desta aula, e a gente corrige amanhã.

> **Words to watch**
>
> - **event** — uma coisa que aconteceu no reino em um dia específico. Um objeto — título, dia, tipo.
> - **event log** — a lista de eventos que o reino acumulou, em ordem
> - **`record`** — uma palavra-chave do C# para uma pequena classe de dados que não pode ser mudada. Dois records com os mesmos campos são iguais automaticamente.
> - **`Random`** — a classe da biblioteca padrão para "me dê um número aleatório"
> - **deterministic** — as mesmas entradas sempre produzem as mesmas saídas

---

## Passo 1 — records `KingdomEvent`

O starter deste módulo:

- **NOVO:** `Kingdom.Engine/KingdomEvent.cs` (um record base + três records de subclasse)
- **NOVO:** `Kingdom.Engine/EventEngine.cs` (joga o dado a cada tick, retorna um evento ou null)
- **MODIFICADO:** `Kingdom.Engine/Kingdom.cs` (ganha `EventLog` e roda o event engine a cada tick)
- **MODIFICADO:** `Kingdom.Console/Program.cs` (imprime o log de eventos)
- **NOVO:** `tests/Kingdom.Engine.Tests/EventLogTests.cs`

Abra `KingdomEvent.cs`:

```csharp
namespace Kingdom.Engine;

public abstract record KingdomEvent(int Day, string Description);

public record TraderArrived(int Day, int GoldAmount)
    : KingdomEvent(Day, $"A trader arrived with {GoldAmount} gold.");

public record CitizenIll(int Day, string CitizenName)
    : KingdomEvent(Day, $"{CitizenName} fell ill.");

public record BuildingBurned(int Day, string BuildingName)
    : KingdomEvent(Day, $"{BuildingName} burned to the ground.");
```

A palavra-chave `record` é a forma curta do C# de escrever uma pequena classe de dados que não pode ser mudada depois de criada. A linha `public record TraderArrived(int Day, int GoldAmount)` faz quase o mesmo que escrever uma classe com duas propriedades só de leitura, um constructor que as define, uma verificação de igualdade que compara os campos, um `ToString` que os imprime, e um desconstrutor — e o C# escreve tudo isso para você. Dois records com os mesmos valores são iguais automaticamente, que é exatamente o que você quer para eventos.

O padrão aqui: um `abstract record KingdomEvent` com os campos compartilhados (`Day`, `Description`), depois três records de subclasse que adicionam seus próprios detalhes. Cada subclasse passa uma string de descrição clara para o record base. Essa descrição é o que fica impresso no log.

## Passo 2 — `EventEngine`

Abra `EventEngine.cs`:

```csharp
namespace Kingdom.Engine;

// NOTE: This class uses System.Random directly. That's bad for testing.
// Module 1.8 rewrites it to take an IRandom interface.
public class EventEngine
{
    private readonly Random _rng = new();

    public KingdomEvent? RollOnce(Kingdom k)
    {
        // 30% chance something happens this tick
        if (_rng.NextDouble() > 0.3) return null;

        // pick which event
        var pick = _rng.Next(3);
        return pick switch
        {
            0 => new TraderArrived(k.Day, _rng.Next(10, 51)),       // 10..50 gold
            1 when k.Citizens.Count > 0 =>
                new CitizenIll(k.Day, k.Citizens[_rng.Next(k.Citizens.Count)].Name),
            2 when k.Buildings.Count > 0 =>
                new BuildingBurned(k.Day, k.Buildings[_rng.Next(k.Buildings.Count)].Name),
            _ => null   // chosen event has no valid target — no event this day
        };
    }
}
```

Três coisas para notar. Primeiro, a **expressão switch** — um padrão moderno do C#: `pick switch { 0 => ..., 1 => ..., _ => padrão }`. O sublinhado significa *"qualquer outra coisa."* É mais limpo do que uma longa cadeia de `if`/`else if`. Segundo, **correspondência de padrão com `when`**: `1 when k.Citizens.Count > 0` diz *"caso 1, mas só se houver cidadãos."* Se não houver, o C# verifica o próximo padrão. Terceiro, o evento do comerciante não adiciona de verdade o ouro ao ledger aqui — por enquanto, os eventos só descrevem o que aconteceu. Vamos conectar isso direito na polish do Módulo 1.10.

## Passo 3 — ligue ao `Kingdom`

Abra `Kingdom.cs` e adicione os novos campos e o roll dentro de `AdvanceDay`:

```csharp
namespace Kingdom.Engine;

public class Kingdom
{
    public string Name { get; }
    public int Day { get; private set; } = 1;
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public ResourceLedger Resources { get; } = new();
    public List<KingdomEvent> EventLog { get; } = new();    // NEW

    private readonly EventEngine _eventEngine = new();      // NEW

    public Kingdom(string name)
    {
        Name = name;
        Resources.Add(Resource.Gold, 100);
        Resources.Add(Resource.Wood, 50);
        Resources.Add(Resource.Stone, 20);
        Resources.Add(Resource.Food, 30);
    }

    public void AddBuilding(Building b) => Buildings.Add(b);
    public void AddCitizen(Citizen c) => Citizens.Add(c);

    public void AdvanceDay()
    {
        foreach (var b in Buildings) b.Tick(Resources);
        foreach (var _ in Citizens) Resources.Spend(Resource.Food, 1);

        var evt = _eventEngine.RollOnce(this);
        if (evt is not null) EventLog.Add(evt);

        Day++;
    }
}
```

Dois novos campos (`EventLog`, `_eventEngine`) e duas novas linhas em `AdvanceDay`. Todo o resto fica igual.

## Passo 4 — imprima o log de eventos

Rode por 30 dias em vez de 5, e adicione uma seção de log de eventos:

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));

for (int day = 0; day < 30; day++)
    kingdom.AdvanceDay();

Console.WriteLine($"== {kingdom.Name} after {kingdom.Day - 1} days ==");
Console.WriteLine($"Buildings: {kingdom.Buildings.Count}");
Console.WriteLine($"Citizens:  {kingdom.Citizens.Count}");
Console.Write("Resources: ");
foreach (var (r, n) in kingdom.Resources.Snapshot())
    Console.Write($"{r}={n}  ");
Console.WriteLine();

Console.WriteLine();
Console.WriteLine($"=== Event log ({kingdom.EventLog.Count} entries) ===");
foreach (var e in kingdom.EventLog)
    Console.WriteLine($"  Day {e.Day,3}: {e.Description}");
```

`{e.Day,3}` é uma dica de formatação — ela completa o número do dia para ter três caracteres de largura. Isso alinha o log de forma ordenada.

Build e rode duas vezes:

```powershell
dotnet build
dotnet run --project Kingdom.Console
dotnet run --project Kingdom.Console
```

Eventos diferentes a cada vez. O reino não é mais determinístico. Lembre-se disso para o próximo passo.

## Passo 5 — teste o que pudermos (e veja o problema)

`tests/Kingdom.Engine.Tests/EventLogTests.cs`:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class EventLogTests
{
    [Fact]
    public void NewKingdom_HasEmptyEventLog()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.EventLog.ShouldBeEmpty();
    }

    [Fact]
    public void After50Days_LogHasSomeEvents()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        k.AddBuilding(new Farm("F"));
        for (int i = 0; i < 50; i++) k.AdvanceDay();

        // 30% per day x 50 days ~= 15 expected. We assert "some".
        k.EventLog.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void EventDay_AlwaysReflectsKingdomDayWhenLogged()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        for (int i = 0; i < 30; i++) k.AdvanceDay();

        // Every event's Day must be in [1, k.Day-1]
        k.EventLog.All(e => e.Day >= 1 && e.Day < k.Day).ShouldBeTrue();
    }
}
```

Três testes, e você já pode ver o problema. Não *podemos* testar coisas como *"quando o dado rola 0, um evento TraderArrived é criado"*, porque não há forma de controlar o dado. Não podemos verificar que *"o GoldAmount do TraderArrived está entre 10 e 50"* — bem, poderíamos rodar mil vezes e torcer. Não podemos verificar que *"este setup exato produz esta lista exata de eventos"* de jeito nenhum. Então os testes ficam vagos — *"algum evento acontece"*, *"dias estão no intervalo"*. Passam, mas verificam quase nada. Esse é o problema que o Módulo 1.8 corrige.

Rode:

```powershell
dotnet test
```

Você deve ver `Passed: 30` (27 mais 3 novos).

## Mexa um pouco

Rode `dotnet run --project Kingdom.Console` dez vezes seguidas. Resultado diferente toda vez. É o não-determinismo em ação.

Tente dar ao `Random` um número inicial fixo — mude `new Random()` para `new Random(42)`. Agora toda execução é idêntica. Você pode repetir ela exatamente. Mas esse número inicial agora está fixo dentro do engine, o que também é errado para um jogo de verdade (você quer um mundo diferente para cada jogador). O `IRandom` do Módulo 1.8 deixa a *shell* escolher em vez disso.

Aumente a chance de 30% para 90%. O console enche de eventos rápido.

Adicione uma quarta subclasse de evento — talvez `SecretFound`? — é basicamente os mesmos passos. Adicione-a ao `switch`. Repare que seus testes ainda passam sem nenhuma mudança. Os testes são frouxos demais para notar o novo comportamento — exatamente o problema que corrigiremos amanhã.

## A linha-guia

A linha-guia neste módulo: **engines devem ser determinísticos por padrão**. Qualquer coisa aleatória deve vir *de fora* por um parâmetro, nunca ser criada com `new Random()` lá dentro. Hoje quebramos essa regra de propósito, para que você possa ver por que importa. Amanhã a consertamos.

## O que você acabou de fazer

Você adicionou um sistema de verdade ao engine em cerca de quarenta linhas: um record base, três subclasses de evento, um pequeno pedaço de código que joga o dado, e uma lista para guardar os resultados. Você conheceu `record` (a forma curta do C# de escrever uma pequena classe de dados que não pode mudar, onde a igualdade compara os campos), a expressão `switch` moderna, e correspondência de padrão com `when`. Você também conheceu o limite dos testes quando a aleatoriedade está escondida dentro do engine — três testes vagos são o melhor que você pode fazer, que é exatamente onde começa a aula de amanhã. Trinta testes passando agora.

**Conceitos que você já sabe nomear:**

- **event** — um pequeno record que não pode mudar, descrevendo uma coisa que aconteceu
- **event log** — uma lista de eventos em ordem, a memória do reino
- **`record`** — uma classe curta de dados do C# onde a igualdade compara os campos
- **expressão `switch`** — um switch que retorna um valor, `_` para "qualquer outra coisa"
- **engine não-determinístico** — mesmas entradas, saídas diferentes, difícil de testar

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a única grande ideia pegou: escrever um `record`. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Adicione um quarto tipo de evento de memória, `SecretFound`:

1. É um `record` que herda de `KingdomEvent`.
2. Ele recebe um `Day` e uma `string Place`.
3. Ele passa uma descrição clara para o record base, como `$"A secret was found at {Place}."`.

Você não precisa adicioná-lo ao `switch` — só faça o record em si compilar.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
public record SecretFound(int Day, string Place)
    : KingdomEvent(Day, $"A secret was found at {Place}.");
```

Essa única linha te dá uma pequena classe de dados que não pode ser mudada depois de criada, com um constructor, igualdade de valor, e um `ToString` — tudo escrito para você. Dois records `SecretFound` com o mesmo `Day` e `Place` contam como iguais automaticamente.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.7 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 1.8 apresenta **`IRandom`**, **`IClock`**, e **FakeItEasy** — a correção para tudo que foi difícil neste módulo. Mesmo `EventEngine`, mas desta vez você pode testá-lo de verdade. Você vai ver a diferença nos primeiros três minutos.
