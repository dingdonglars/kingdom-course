# Módulo 1.8 — Interfaces, IRandom, IClock and FakeItEasy

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

> **Aquecimento — 30 segundos, de cabeça.** Antes de hoje, traga de volta o que você construiu no Módulo 1.7:
>
> 1. O que é o *event log*, e por que ele é chamado de memória do reino?
> 2. Você adicionou aleatoriedade da última vez. Por que isso deixa o engine difícil de testar?
>
> Inseguro em alguma? Releia primeiro o **Módulo 1.7** — a aula de hoje conserta exatamente esse problema de teste, então fica bem em cima dela. Leve qualquer coisa que ficou frágil para o sync semanal.

Hoje é a aula que o Módulo 1.7 estava preparando. Tiramos duas interfaces do engine — `IRandom` (o dado) e `IClock` (o relógio). O console dá ao engine as versões reais. Os testes dão a ele **fakes**, construídos com **FakeItEasy**. Agora podemos escrever testes como *"se o dado rolar 0, o próximo evento é um TraderArrived com exatamente 50 de ouro"*, e eles são verdadeiros *toda vez*.

A correção aqui é um dos padrões mais comuns no código moderno: **faça uma interface, receba-a pelo constructor, e deixe quem chama escolher a versão**. O `EventEngine` de ontem tinha `private readonly Random _rng = new();` — uma dependência escondida que você não podia mudar. A versão de hoje tem `private readonly IRandom _rng;`, definida por um parâmetro do constructor — uma dependência que você pode ver e pode trocar. A mudança é pequena de ler, mas a diferença no que você pode testar é enorme.

Uma **interface** é um *contrato* — uma lista do que algo pode fazer, sem dizer como. Uma vez que `EventEngine` pede "qualquer coisa que cumpra o contrato `IRandom`", você escolhe qual versão ela recebe:

```text
            IRandom            the contract: "ask me for a number, I give one back"
           /        \
   SystemRandom      a fake
   (real dice —      (test dice — returns
    the console       exactly what a test
    hands this in)    tells it to)
```

Mesma tomada, dois plugues. O engine não sabe nem se importa qual recebeu — ele só chama o contrato. Esse é todo o truque que torna o engine testável.

> **Words to watch**
>
> - **interface** — um contrato: uma lista de formas de métodos sem corpos. Muitas classes podem implementar a mesma interface.
> - **dependency injection (DI)** — passar as outras classes que uma classe precisa pelo seu constructor, em vez de criá-las com `new` dentro
> - **fake / mock / stub** — um substituto para uma classe de verdade, usado em testes. Usamos **FakeItEasy** para criá-los em uma linha.
> - **deterministic** — as mesmas entradas sempre produzem as mesmas saídas. O traço que torna os engines testáveis.

---

## Por que isso importa

O `EventEngine` de ontem tinha três problemas. Era **difícil de testar** — os testes não podiam dizer "dado X, o resultado é Y", porque não havia como fazer o dado rolar X. Era **impossível de repetir** — dois jogadores começando no mesmo dia recebiam mundos diferentes. E tinha uma **dependência escondida** — ler o constructor não dizia que `EventEngine` precisava secretamente de uma fonte de números aleatórios. Bugs no comportamento de `Random` (existem na vida real) afetariam todo engine que o usasse, sem aviso.

A correção é a mesma nos três casos:

```csharp
// Before
public class EventEngine
{
    private readonly Random _rng = new();    // hidden, fixed
}

// After
public class EventEngine
{
    private readonly IRandom _rng;
    public EventEngine(IRandom rng) { _rng = rng; }   // visible, swappable
}
```

A shell decide. Para o programa de verdade, dê a ela um `SystemRandom`. Para os testes, dê um fake que retorna o que você quiser.

## Passo 0 — instale o FakeItEasy

No seu projeto de testes existente:

```powershell
cd tests/Kingdom.Engine.Tests
dotnet add package FakeItEasy
```

Isso adiciona uma linha ao `.csproj`:

```xml
<PackageReference Include="FakeItEasy" Version="..." />
```

Este módulo também adiciona:

- **NOVO:** `Kingdom.Engine/IRandom.cs` e `SystemRandom.cs`
- **NOVO:** `Kingdom.Engine/IClock.cs` e `SystemClock.cs`
- **MODIFICADO:** `Kingdom.Engine/EventEngine.cs` (recebe `IRandom`)
- **MODIFICADO:** `Kingdom.Engine/Kingdom.cs` (recebe `IRandom` + `IClock`, passa para `EventEngine`)
- **MODIFICADO:** `Kingdom.Console/Program.cs` (constrói as dependências e as passa para dentro)
- **NOVO:** `tests/Kingdom.Engine.Tests/EventEngineTests.cs` (usa FakeItEasy)

## Passo 1 — `IRandom` e `SystemRandom`

`Kingdom.Engine/IRandom.cs`:

```csharp
namespace Kingdom.Engine;

public interface IRandom
{
    /// <summary>Returns an integer in [minInclusive, maxExclusive).</summary>
    int Next(int minInclusive, int maxExclusive);

    /// <summary>Returns a double in [0.0, 1.0).</summary>
    double NextDouble();
}
```

Os dois métodos são exatamente as partes de `Random` que `EventEngine` estava usando. **Interfaces devem ser pequenas** — só o que o usuário dela precisa, nada mais. Se uma classe só usa dois métodos de `Random`, a interface deve ter esses dois e parar.

`Kingdom.Engine/SystemRandom.cs`:

```csharp
namespace Kingdom.Engine;

/// <summary>Production implementation. Wraps System.Random.</summary>
public class SystemRandom : IRandom
{
    private readonly Random _rng;
    public SystemRandom() { _rng = new Random(); }
    public SystemRandom(int seed) { _rng = new Random(seed); }    // for reproducible runs

    public int Next(int minInclusive, int maxExclusive) => _rng.Next(minInclusive, maxExclusive);
    public double NextDouble() => _rng.NextDouble();
}
```

Dois constructors — o sem argumentos significa *"me dê um mundo novo"*, e o com seed significa *"me dê o mesmo mundo que tive na última vez"*. A shell escolhe qual quer.

## Passo 2 — `IClock` e `SystemClock`

Mesmo padrão, mas para *"que horas são?"*

`Kingdom.Engine/IClock.cs`:

```csharp
namespace Kingdom.Engine;

public interface IClock
{
    DateTime Now { get; }
}
```

`Kingdom.Engine/SystemClock.cs`:

```csharp
namespace Kingdom.Engine;

public class SystemClock : IClock
{
    public DateTime Now => DateTime.UtcNow;
}
```

Não usamos `IClock` muito neste módulo, mas o engine vai começar a precisar dela na Fase 2 (salvamento) para os carimbos de *"salvo em"*. Configuramos hoje para que o código posterior esteja pronto.

## Passo 3 — mude o `EventEngine` para receber um `IRandom`

```csharp
namespace Kingdom.Engine;

public class EventEngine
{
    private readonly IRandom _rng;
    public EventEngine(IRandom rng) { _rng = rng; }

    public KingdomEvent? RollOnce(Kingdom k)
    {
        if (_rng.NextDouble() > 0.3) return null;

        var pick = _rng.Next(0, 3);
        return pick switch
        {
            0 => new TraderArrived(k.Day, _rng.Next(10, 51)),
            1 when k.Citizens.Count > 0 =>
                new CitizenIll(k.Day, k.Citizens[_rng.Next(0, k.Citizens.Count)].Name),
            2 when k.Buildings.Count > 0 =>
                new BuildingBurned(k.Day, k.Buildings[_rng.Next(0, k.Buildings.Count)].Name),
            _ => null
        };
    }
}
```

Três pequenas mudanças de ontem: o tipo do campo mudou de `Random` para `IRandom`, o constructor agora exige o dado, e `_rng.Next(3)` agora é `_rng.Next(0, 3)` porque o contrato de `IRandom` usa a forma de dois argumentos mais clara.

## Passo 4 — `Kingdom` recebe as dependências

```csharp
namespace Kingdom.Engine;

public class Kingdom
{
    public string Name { get; }
    public int Day { get; private set; } = 1;
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public ResourceLedger Resources { get; } = new();
    public List<KingdomEvent> EventLog { get; } = new();

    private readonly EventEngine _eventEngine;
    private readonly IClock _clock;

    public Kingdom(string name, IRandom rng, IClock clock)
    {
        Name = name;
        _eventEngine = new EventEngine(rng);
        _clock = clock;

        Resources.Add(Resource.Gold, 100);
        Resources.Add(Resource.Wood, 50);
        Resources.Add(Resource.Stone, 20);
        Resources.Add(Resource.Food, 30);
    }

    // Convenience constructor for older tests (1.3-1.7).
    // Uses real System implementations.
    public Kingdom(string name) : this(name, new SystemRandom(), new SystemClock()) { }

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

O constructor curto extra no fundo é o que mantém todo teste antigo funcionando. `new Kingdom("Test")` ainda cria um reino — mas agora ele constrói um `SystemRandom` e `SystemClock` de verdade para você por baixo dos panos. O padrão vale lembrar: quando uma classe passa de não precisar de dependências para precisar de várias, mantenha a forma sem argumentos encadeando para a mais longa (`: this(...)`). Os testes novos usam a forma completa com fakes, e os testes velhos ainda funcionam.

## Passo 5 — `Program.cs` constrói as dependências

```csharp
using Kingdom.Engine;

IRandom rng = new SystemRandom(seed: 42);   // remove the seed for unpredictable runs
IClock clock = new SystemClock();

var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, clock);
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));

for (int day = 0; day < 30; day++)
    kingdom.AdvanceDay();

Console.WriteLine($"== {kingdom.Name} after {kingdom.Day - 1} days ==");
Console.Write("Resources: ");
foreach (var (r, n) in kingdom.Resources.Snapshot())
    Console.Write($"{r}={n}  ");
Console.WriteLine();

Console.WriteLine();
Console.WriteLine($"=== Event log ({kingdom.EventLog.Count} entries) ===");
foreach (var e in kingdom.EventLog)
    Console.WriteLine($"  Day {e.Day,3}: {e.Description}");
```

Rode com o seed no lugar — o resultado é idêntico toda execução. Tire o seed — o resultado muda a cada execução. A shell decide; o engine só faz o que é mandado.

## Passo 6 — testando com FakeItEasy

`tests/Kingdom.Engine.Tests/EventEngineTests.cs`:

```csharp
using FakeItEasy;
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class EventEngineTests
{
    [Fact]
    public void RollOnce_HighRoll_ReturnsNull()
    {
        // Arrange — make the dice roll above 0.3 so the engine returns nothing
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.9);
        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");

        // Act
        var evt = engine.RollOnce(k);

        // Assert
        evt.ShouldBeNull();
    }

    [Fact]
    public void RollOnce_LowRollPickZero_GivesTrader()
    {
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.1);          // pass the threshold
        A.CallTo(() => rng.Next(0, 3)).Returns(0);              // pick 0 -> trader
        A.CallTo(() => rng.Next(10, 51)).Returns(50);           // exactly 50 gold
        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");

        var evt = engine.RollOnce(k);

        evt.ShouldBeOfType<TraderArrived>();
        ((TraderArrived)evt!).GoldAmount.ShouldBe(50);
    }

    [Fact]
    public void RollOnce_LowRollPickOne_NoCitizens_ReturnsNull()
    {
        // Pick 1 = CitizenIll, but no citizens -> fall through to default -> null
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.1);
        A.CallTo(() => rng.Next(0, 3)).Returns(1);
        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");
        // No citizens added.

        engine.RollOnce(k).ShouldBeNull();
    }

    [Fact]
    public void RollOnce_LowRollPickOne_WithCitizen_GivesIllness()
    {
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.1);
        A.CallTo(() => rng.Next(0, 3)).Returns(1);
        // The engine then calls rng.Next(0, k.Citizens.Count). One citizen -> 0.
        A.CallTo(() => rng.Next(0, 1)).Returns(0);

        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("Lyra"));

        var evt = engine.RollOnce(k);
        evt.ShouldBeOfType<CitizenIll>();
        ((CitizenIll)evt!).CitizenName.ShouldBe("Lyra");
    }

    [Fact]
    public void Kingdom_WithFixedRandom_IsFullyDeterministic()
    {
        // Two kingdoms with the same seed produce identical event logs
        var k1 = new global::Kingdom.Engine.Kingdom("A", new SystemRandom(seed: 42), new SystemClock());
        var k2 = new global::Kingdom.Engine.Kingdom("B", new SystemRandom(seed: 42), new SystemClock());
        k1.AddCitizen(new Citizen("X")); k1.AddBuilding(new Farm("F"));
        k2.AddCitizen(new Citizen("X")); k2.AddBuilding(new Farm("F"));

        for (int i = 0; i < 30; i++) { k1.AdvanceDay(); k2.AdvanceDay(); }

        k1.EventLog.Count.ShouldBe(k2.EventLog.Count);
        for (int i = 0; i < k1.EventLog.Count; i++)
            k1.EventLog[i].Description.ShouldBe(k2.EventLog[i].Description);
    }
}
```

Dois tipos de testes. Fakes (FakeItEasy) para os unit tests de `EventEngine` — controle exato do que cada chamada retorna. E um `SystemRandom` de verdade com seed para o teste maior, que prova que duas execuções completas com o mesmo seed produzem a mesma história. Os dois tipos são determinísticos.

Rode:

```powershell
dotnet test
```

Você deve ver `Passed: 35` (30 mais 5 novos).

## Mexa um pouco

Remova o constructor curto `Kingdom(string name)` que encadeia para o novo. Agora todo teste antigo quebra com *"no constructor takes 1 argument"*. É por isso que adicionamos o encadeado. Coloque-o de volta.

Tente mudar o seed em `Program.cs` de 42 para 7. Você tem uma história de reino diferente, mas a mesma toda execução.

Em um teste, configure o dado para rolar três comerciantes em três dias. É fácil com FakeItEasy: `A.CallTo(() => rng.NextDouble()).ReturnsNextFromSequence(0.1, 0.1, 0.1);` e `A.CallTo(() => rng.Next(0, 3)).ReturnsNextFromSequence(0, 0, 0);`.

Adicione uma property ao `IRandom`: `int Seed { get; }`. Os testes ainda compilam, porque `A.Fake<IRandom>()` constrói um fake que combina com qualquer interface. Mas `SystemRandom` ainda não tem essa property, então a build falha nessa classe. Adicione a property. A interface garantiu que os dois lados ficassem de acordo.

## A linha-guia

A linha-guia neste módulo: **toda dependência externa entra por uma interface**. Aleatório, relógio, sistema de arquivos, rede, banco de dados — nenhum deles é criado com `new` dentro do engine. A shell os fornece. Essa é a regra que faz o mesmo engine funcionar no console, em uma web API, em um browser, e em Roblox. Cada um troca para as versões que se encaixam nele. O engine não sabe nem se importa qual recebeu.

## O que você acabou de fazer

Você tirou `IRandom` e `IClock` do engine, passou-os pelo constructor de `EventEngine` e `Kingdom`, e usou **FakeItEasy** para construir fakes que deixam seus testes controlar o dado exatamente. Você escreveu cinco testes novos — quatro com fakes, um com um aleatório real com seed — e alcançou um nível de precisão nos testes que era impossível ontem. O constructor extra curto em `Kingdom` manteve todo teste antigo funcionando sem mudanças. Trinta e cinco testes passando agora, cada um determinístico.

**Conceitos que você já sabe nomear:**

- **interface** — um contrato de formas de métodos, sem corpos
- **dependency injection** — as classes que uma classe precisa chegam pelo seu constructor
- **fake** (FakeItEasy) — um substituto em tempo de teste para uma interface
- **`A.CallTo(...).Returns(...)`** — controle exato do que um fake retorna
- **deterministic** — as mesmas entradas sempre dão as mesmas saídas

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a única grande ideia pegou: receber uma dependência pelo constructor em vez de criá-la com `new` dentro. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Aqui está uma classe com uma dependência escondida:

```csharp
public class Greeter
{
    private readonly IClock _clock = new SystemClock();
    public string Greet() => $"Hello! It is now {_clock.Now}.";
}
```

Sem olhar:

1. Reescreva `Greeter` para que o `IClock` chegue pelo constructor em vez de ser criado dentro.
2. Em um teste, use FakeItEasy para criar um `IClock` fake e defina seu `Now` para uma data fixa.
3. Verifique que `Greet()` coloca essa data exata na mensagem.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
public class Greeter
{
    private readonly IClock _clock;
    public Greeter(IClock clock) { _clock = clock; }
    public string Greet() => $"Hello! It is now {_clock.Now}.";
}
```

```csharp
var clock = A.Fake<IClock>();
A.CallTo(() => clock.Now).Returns(new DateTime(2030, 1, 1));
var greeter = new Greeter(clock);
greeter.Greet().ShouldContain("2030");
```

Agora a dependência é **visível** no constructor e você pode **trocá-la** por um fake. Foi isso que tornou o dado testável nesta aula: a shell entrega ao engine sua versão de verdade, e o teste lhe entrega uma que ele controla.

</details>

## Movimento git da semana — branches

Até agora todo o seu trabalho estava no `main`. De agora em diante, qualquer coisa maior que uma mudança pequenininha merece sua própria branch — assim o seu `main` fica limpo para o trabalho terminado e revisado.

No VS Code: clique no nome da branch no canto inferior esquerdo da barra de status (diz `main`). Um menu abre com *"Create new branch"* no topo. Digite um nome — como `feature/event-engine` — e você está nela. O canto inferior esquerdo agora mostra a branch nova.

Para voltar: mesmo lugar, escolha `main`.

> **Ou no terminal:**
>
> ```powershell
> git switch -c feature/event-engine    # create + switch
> git switch main                        # switch back
> ```

Uma branch no git é só um marcador que aponta para um commit e se move conforme você adiciona mais; criar uma não custa nada. Explicamos como isso realmente funciona em B3.1.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.8 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 1.9 desacelera e olha para a **organização do código** — pastas, arquivos, namespaces, o que fica no `Engine` e o que vai para `Engine.Events` (um sub-namespace). Depois do 1.8, o engine tem tipos suficientes para que agrupá-los valha a pena.
