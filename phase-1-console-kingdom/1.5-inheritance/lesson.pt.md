# Módulo 1.5 — Inheritance: Specialised Buildings

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

> **Aquecimento — 30 segundos, de cabeça.** Antes de hoje, traga de volta o que você construiu no Módulo 1.4:
>
> 1. O que é um *tick*, e o que o roda de novo e de novo?
> 2. `AdvanceDay` muda o reino em vez de retornar um valor. Como chamamos um método assim?
>
> Inseguro em alguma? Releia primeiro o **Módulo 1.4** — a aula de hoje fica bem em cima dele. Leve qualquer coisa que ficou frágil para o sync semanal.

Ontem o `Tick()` não fazia nada. Hoje criamos três tipos específicos de prédio — `Farm` produz comida, `Lumberyard` produz madeira, `Mine` produz pedra. Cada um *é* um prédio (mesmo nome, mesmo nível, mesmo upgrade), mas cada um dá tick de forma diferente. O recurso de OOP que deixa você dizer *"uma fazenda é um tipo de prédio"* é chamado de **herança (inheritance)**, e essa é a aula de hoje.

A outra opção seria três classes separadas — `Farm`, `Lumberyard`, `Mine` — cada uma com seu próprio `Name`, `Level`, `Upgrade` e `Tick`. Então toda mudança em uma teria que ser feita três vezes. A herança deixa você escrever `Building` uma vez, e depois dizer *"Farm é um Building, e aqui está a pequena parte especial das fazendas."* Farm ganha tudo o mais de graça.

É um pouco como animais. Um cachorro é um tipo de animal. Ele já tem tudo que um animal tem — ele respira, come, se move. Você só precisa descrever a parte que faz um cachorro ser um cachorro: ele late. Você não redescreve a respiração.

Desenhado, os três prédios de hoje ficam assim:

```text
                     Building                  the parent
                     Name, Level, Upgrade()
                  /      |       \
                 /       |        \
             Farm    Lumberyard    Mine        the children: each one IS
              |          |          |          a Building (it gets all of
            food       wood       stone        the above for free)...
                                               ...and only adds its own Tick()
```

Cada filho escreve *uma* coisa pequena — o que produz a cada tick — e recebe `Name`, `Level` e `Upgrade()` do pai sem copiar.

> **Words to watch**
>
> - **inheritance** — quando uma classe (a *filho*) recebe todos os campos e métodos de outra (o *pai*) e pode adicionar ou substituir os seus próprios
> - **subclass** (ou *child class*) — `Farm`, `Lumberyard`, `Mine`
> - **base class** (ou *parent class*) — `Building`
> - **`override`** — a palavra-chave que diz *"estou substituindo o método `virtual` do pai"*
> - **`base(...)`** — chama o constructor do pai de dentro do filho

---

## Por que herança, e quando ter cuidado

Herança é uma das ideias mais antigas na programação orientada a objetos, e é fácil usar demais. Um conselho moderno comum é *"prefira composição em vez de herança."* Isso significa: quando uma coisa quer usar outra, muitas vezes é mais limpo ela *conter* a outra em vez de *herdar* dela. Cadeias longas de herança (cinco ou seis níveis) ficam difíceis de mudar. Uma mudança perto do topo alcança cada classe abaixo dela, e isso é fácil de quebrar.

Para um nível de profundidade, porém — `Building` → `Farm`, `Lumberyard`, `Mine` — herança é a ferramenta certa. Três classes, três coisas que elas realmente compartilham, e nenhuma cadeia longa. Vamos voltar à composição mais tarde, quando houver uma razão clara para isso.

## Passo 1 — três subclasses

O `starter/` deste módulo adiciona três arquivos novos e atualiza `Program.cs`:

- **NOVO:** `Kingdom.Engine/Farm.cs`
- **NOVO:** `Kingdom.Engine/Lumberyard.cs`
- **NOVO:** `Kingdom.Engine/Mine.cs`
- **MODIFICADO:** `Kingdom.Console/Program.cs` (usa as novas subclasses)
- **NOVO:** `tests/Kingdom.Engine.Tests/SubclassTests.cs`

`Building.cs`, `Kingdom.cs` e os testes existentes estão sem mudança a partir do 1.4.

`Farm.cs`:

```csharp
namespace Kingdom.Engine;

public class Farm : Building
{
    public Farm(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        // Each level produces 5 food per day
        ledger.Add(Resource.Food, 5 * Level);
    }
}
```

Três coisas para ler com cuidado. O `: Building` depois de `class Farm` significa *"herde de Building."* `Farm` agora tem `Name`, `Level`, `Upgrade` e `Tick` de graça. O `: base(name)` no constructor passa o parâmetro `name` para cima para o constructor de `Building` — a classe pai ainda faz o trabalho de definir `Name`, e o filho só adiciona o que é novo (que aqui não é nada). E `override void Tick(...)` substitui o padrão vazio do pai. O compilador te obriga a escrever `override` quando você substitui um método `virtual`, para que você nunca possa substituir um por acidente.

`Lumberyard.cs`:

```csharp
namespace Kingdom.Engine;

public class Lumberyard : Building
{
    public Lumberyard(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Wood, 3 * Level);
    }
}
```

`Mine.cs`:

```csharp
namespace Kingdom.Engine;

public class Mine : Building
{
    public Mine(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Stone, 2 * Level);
    }
}
```

Três classes que parecem quase iguais. Isso parece repetição, e é um pouco. A pergunta que vale fazer é *"cada uma dessas mudaria de forma diferente com o tempo?"* Um `Farm` futuro pode adicionar um enum `Crop`. Um `Mine` futuro pode rastrear um `OreVein`. Sim, provavelmente sim. Então ter classes separadas vale a pena. Se duas classes realmente nunca vão se separar, coloque-as juntas como uma só — não crie uma nova classe para cada palavra no design.

## Passo 2 — use as subclasses no `Program.cs`

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));

PrintKingdom(kingdom);

for (int day = 0; day < 5; day++)
{
    kingdom.AdvanceDay();
    PrintKingdom(kingdom);
}

void PrintKingdom(Kingdom.Engine.Kingdom k)
{
    Console.WriteLine();
    Console.WriteLine($"== Day {k.Day} — {k.Name} ==");
    Console.WriteLine($"Buildings ({k.Buildings.Count}):");
    foreach (var b in k.Buildings)
        Console.WriteLine($"  - {b.GetType().Name} '{b.Name}' (level {b.Level})");
    Console.WriteLine($"Citizens:  {k.Citizens.Count}");
    Console.Write("Resources: ");
    foreach (var (r, n) in k.Resources.Snapshot())
        Console.Write($"{r}={n}  ");
    Console.WriteLine();
}
```

Repare em `b.GetType().Name`. **`GetType()`** é um método que todo objeto tem — ele devolve o tipo que o objeto realmente é enquanto o programa roda. `.Name` dá o nome curto do tipo como string (`"Farm"`, `"Lumberyard"`, `"Mine"`). É assim que o mesmo loop pode mostrar diferentes tipos de prédio sem que a gente escreva um `switch`. A lista guarda referências de `Building`, mas cada item *executa* seu próprio `Tick`, porque cada item é realmente um `Farm` ou um `Lumberyard` ou um `Mine`. O nome para isso é **polimorfismo** — todos parecem o mesmo tipo por fora, mas cada um se comporta diferente por dentro.

Build e rode:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Você deve ver do Dia 1 ao Dia 6, com comida subindo (5 da fazenda, menos 2 para os cidadãos, então +3 por dia), madeira +3 por dia, pedra +2 por dia. Os recursos realmente mudam agora.

## Passo 3 — teste as subclasses

`tests/Kingdom.Engine.Tests/SubclassTests.cs`:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class SubclassTests
{
    [Fact]
    public void Farm_Tick_AddsFoodEqualToFiveTimesLevel()
    {
        var ledger = new ResourceLedger();
        var farm = new Farm("F");
        farm.Tick(ledger);
        ledger.Get(Resource.Food).ShouldBe(5);
    }

    [Fact]
    public void Lumberyard_Tick_AddsWoodEqualToThreeTimesLevel()
    {
        var ledger = new ResourceLedger();
        var ly = new Lumberyard("L");
        ly.Tick(ledger);
        ledger.Get(Resource.Wood).ShouldBe(3);
    }

    [Fact]
    public void Mine_Tick_AddsStoneEqualToTwoTimesLevel()
    {
        var ledger = new ResourceLedger();
        var m = new Mine("M");
        m.Tick(ledger);
        ledger.Get(Resource.Stone).ShouldBe(2);
    }

    [Fact]
    public void Farm_Upgraded_ProducesMore()
    {
        var ledger = new ResourceLedger();
        var farm = new Farm("F");
        farm.Upgrade();   // level 2 now
        farm.Tick(ledger);
        ledger.Get(Resource.Food).ShouldBe(10);
    }

    [Fact]
    public void Subclass_InheritsName()
    {
        var farm = new Farm("Main Farm");
        farm.Name.ShouldBe("Main Farm");
        farm.Level.ShouldBe(1);
    }

    [Fact]
    public void Kingdom_AdvanceDay_RunsAllSubclassTicks()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Lumberyard("L"));
        k.AddBuilding(new Mine("M"));

        var foodBefore = k.Resources.Get(Resource.Food);
        var woodBefore = k.Resources.Get(Resource.Wood);
        var stoneBefore = k.Resources.Get(Resource.Stone);

        k.AdvanceDay();

        k.Resources.Get(Resource.Food).ShouldBe(foodBefore + 5);  // no citizens this test
        k.Resources.Get(Resource.Wood).ShouldBe(woodBefore + 3);
        k.Resources.Get(Resource.Stone).ShouldBe(stoneBefore + 2);
    }
}
```

Rode:

```powershell
dotnet test
```

Você deve ver `Passed: 22` — dezesseis de módulos anteriores, seis novos.

## Mexa um pouco

Adicione um `Quarry` (mármore, para prédios elegantes mais tarde). Copie `Mine.cs`, mude o nome da classe e o recurso que ele produz. Adicione um teste para ele. Cerca de cinco minutos de trabalho.

Tente tornar `Building` `abstract` (`public abstract class Building`). Agora o compilador vai recusar `new Building("Generic")` em qualquer lugar — uma classe abstrata só pode ser herdada, não criada por conta própria. Isso quebra os testes do Módulo 1.3? Quebra — `BuildingTests` usa `new Building("Farm")`. Tire de volta. `abstract` é um passo que *poderíamos* dar a seguir, e vamos chegar a isso quando precisarmos.

Mude o constructor para se chamar: escreva `Building(string name) : this("default") { }`. Rode o programa. Você vai receber um erro de stack overflow — o constructor fica se chamando sem fim. Vale ver uma vez, para você se lembrar.

Adicione um método em `Building` chamado `Describe()` que retorna `$"{GetType().Name} {Name} (level {Level})"`. Chame-o de `Program.cs` em vez de construir a string na mão. Mesmo resultado, menos digitação, e agora o engine possui a string de formato.

## O que você acabou de fazer

Três subclasses constroem sobre `Building` — `Farm`, `Lumberyard`, `Mine` — e cada uma substitui o `Tick` vazio com sua própria regra de produção. O mesmo loop `foreach` no `Kingdom.AdvanceDay` toca as três corretamente, porque cada item na lista *executa como o tipo que realmente é*, não como o tipo que a lista diz que guarda. Isso é polimorfismo, em um parágrafo. Você também conheceu `: base(...)` para chamar o constructor do pai, e viu o compilador exigir `override` para que você nunca possa substituir um método `virtual` por acidente. Seis novos testes passaram; vinte e dois no total agora.

**Conceitos que você já sabe nomear:**

- **inheritance** — a classe filho recebe os métodos e campos do pai
- **`override`** — substituindo claramente um método `virtual`
- **`: base(...)`** — chama o constructor do pai de dentro do filho
- **`GetType().Name`** — o nome do tipo real enquanto o programa roda
- **polimorfismo** — mesmo tipo por fora, comportamento diferente por subclasse

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a única grande ideia pegou: fazer uma subclasse. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Escreva um prédio novo de memória: um `Quarry` que produz mármore. (Primeiro adicione `Marble` ao seu enum `Resource`, ou produza `Stone` se preferir não mexer no enum.)

1. `Quarry` herda de `Building`.
2. Passe o nome para cima para o pai com `: base(name)`.
3. Faça `override` no método `Tick` para que ele adicione `4 * Level` do seu recurso ao ledger.
4. Adicione-o ao reino em `Program.cs` e rode.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
public class Quarry : Building
{
    public Quarry(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Stone, 4 * Level);
    }
}
```

Três coisas tinham que estar certas: `: Building` para herdar, `: base(name)` para passar o nome para o constructor do pai, e `override` no `Tick`. Se você esquecer `override`, o compilador reclama — e isso é de propósito, para que você nunca possa substituir um método `virtual` por acidente.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.5 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

## Checkpoint da metade — mostre ao Lars

Você está na metade da Fase 1. Antes de começar o Módulo 1.6, mostre ao Lars o que você construiu — na próxima vez que vocês dois conversarem, ou em uma ligação curta se não houver uma marcada. Cerca de dez minutos, uma conversa amigável, não uma prova. É a mesma ideia dos checkpoints no fim da Fase 0: dizer uma ideia em voz alta é o jeito mais rápido de achar a única parte que ainda está confusa, enquanto ela é pequena e fácil de consertar.

Tenha seu projeto aberto e rodando. O Lars vai pedir para você:

1. **Rodar o reino** e explicar o que um dia — `AdvanceDay` — faz, do início ao fim.
2. **Mostrar a divisão engine/shell.** Abra um arquivo do engine e um do console, e diga qual deles tem permissão de usar o outro, e por quê.
3. **Explicar uma subclasse.** Escolha `Farm`, `Lumberyard` ou `Mine`, e diga o que ela faz com o `Tick` do `Building`, e por que o mesmo loop `foreach` consegue rodar as três corretamente.

Não há nada para preparar e nada para entregar. Se uma pergunta te pegar de surpresa, esse é o ponto — você e o Lars acham isso juntos e escolhem o único módulo para dar uma olhada de volta antes da Fase 1 seguir. Depois siga para o Módulo 1.6.

## Próximo

O Módulo 1.6 apresenta o **LINQ** — os métodos de consulta (`Where`, `Select`, `Sum`, `OrderBy`) que transformam loops `for` manuais em perguntas de uma linha sobre as listas do reino. Você vai usá-los todo dia pelo resto do curso.
