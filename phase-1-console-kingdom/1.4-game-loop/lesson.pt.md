# Módulo 1.4 — The Game Loop

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

> **Aquecimento — 30 segundos, de cabeça.** Antes de hoje, traga de volta o que você construiu no Módulo 1.3:
>
> 1. Quais são as três partes de um teste de unidade?
> 2. Quando você usaria `[Theory]` em vez de `[Fact]`?
>
> Inseguro em alguma? Releia primeiro o **Módulo 1.3** — a aula de hoje fica bem em cima dele. Leve qualquer coisa que ficou frágil para o sync semanal.

Agora o seu reino é como uma fotografia. Você o constrói, o imprime, o programa termina, e nada mudou. Hoje o reino aprende a *se mover*. Cada dia, toda fazenda produz um pouco de comida, toda mina produz um pouco de pedra, e os cidadãos comem. Números sobem. Números descem. No fim deste módulo o programa roda por cinco dias e te mostra o que aconteceu.

Precisamos de uma palavra nova para hoje. **Tick.** Um tick é um passo do tempo de jogo. Em um jogo rápido como um shooter, um tick pode acontecer sessenta vezes por segundo. Em um jogo lento como o nosso — um reino que você administra por muitos dias — um tick é uma vez por *dia*. O comprimento exato não importa. O que importa é que o engine tem um método que significa *"avance o tempo em um passo."* Esse método vai se chamar `AdvanceDay`. Uma vez que existe, qualquer coisa que queira avançar o tempo chama o mesmo método — o console hoje, um botão em uma página web em alguns meses, um jogo Roblox mais tarde no ano. Mesmo engine, muitas shells.

> **Words to watch**
>
> - **tick** — um passo do tempo de jogo. No nosso reino, um tick é um dia.
> - **game loop** — o loop que chama `AdvanceDay` de novo e de novo e mostra o resultado a cada vez.
> - **side effect** — quando um método muda algo em vez de (ou além de) retornar um valor.
> - **subclass** — um tipo de classe mais específico construído sobre outro (vamos conhecer esses de verdade no Módulo 1.5).

---

## O que você está construindo

Três pequenas mudanças no engine, mais um loop no console e testes:

| Arquivo | Mudança |
| --- | --- |
| `Kingdom.Engine/Building.cs` | Ganha um método `Tick(ResourceLedger)`. Vazio por enquanto. |
| `Kingdom.Engine/Kingdom.cs` | Ganha uma property `Day` e um método `AdvanceDay()`. |
| `Kingdom.Console/Program.cs` | Roda o reino por cinco dias e imprime cada dia. |
| `tests/Kingdom.Engine.Tests/KingdomTickTests.cs` | Arquivo de testes novo. |

O starter deste módulo tem apenas esses quatro arquivos. Copie-os por cima da sua pasta de trabalho do Módulo 1.3.

## Passo 1 — dê ao `Building` um método `Tick`

Abra `Building.cs`. Agora ele só guarda `Name` e `Level`. Hoje adicionamos um método que diz *"um tick do tempo de jogo aconteceu — faça o que você faz."*

```csharp
namespace Kingdom.Engine;

public class Building
{
    public string Name { get; }
    public int Level { get; private set; } = 1;

    public Building(string name) { Name = name; }

    public void Upgrade() => Level++;

    // New today. The default does nothing.
    // Specific kinds of building (Farm, Mine, ...) will fill this in tomorrow.
    public virtual void Tick(ResourceLedger ledger) { }
}
```

A nova palavra-chave é **`virtual`**. Ela marca um método como um que pode ser *substituído* — o `Farm` e o `Mine` de amanhã (as *subclasses*) vão escrever sua própria versão de `Tick`, e o engine vai usar essas em vez deste padrão vazio. Hoje o padrão é vazio de propósito. Assim o programa todo pode rodar do começo ao fim antes que os tipos específicos de prédio existam. Adicionamos o *esboço* hoje e o *comportamento* amanhã.

## Passo 2 — dê ao `Kingdom` um `Day` e um `AdvanceDay`

Abra `Kingdom.cs`. Duas coisas vão entrar:

```csharp
namespace Kingdom.Engine;

public class Kingdom
{
    public string Name { get; }
    public int Day { get; private set; } = 1;
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public ResourceLedger Resources { get; } = new();

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
        // 1. Every building ticks.
        foreach (var b in Buildings)
            b.Tick(Resources);

        // 2. Every citizen eats one food. If there's none left, Spend
        //    just returns false and the day continues — nobody starves yet.
        foreach (var _ in Citizens)
            Resources.Spend(Resource.Food, 1);

        // 3. Time moves forward.
        Day++;
    }
}
```

Três coisas acontecem em um tick: prédios produzem, cidadãos comem, o contador de dias sobe. **A ordem importa.** Os prédios têm que produzir *antes* de os cidadãos comerem. Caso contrário, no Dia 1, os cidadãos estariam comendo comida que as fazendas ainda não plantaram. É uma coisa pequena no Dia 1, mas é um bug confuso no Dia 50. Acerte a ordem agora.

`AdvanceDay()` retorna `void` — ele não *devolve* um valor. Em vez disso, ele muda o ledger, o contador de dias, e (em breve) os prédios. Mudar coisas assim, em vez de retornar um valor, é chamado de **side effect**. Engines usam side effects muito. A outra opção seria retornar uma cópia nova do reino inteiro a cada tick, o que significaria copiar tudo toda vez. Então esta forma é mais poderosa, mas um pouco mais difícil de seguir. É uma troca justa.

## Passo 3 — rode do console

Abra `Program.cs`. O shell do console agora faz duas coisas: construir um reino, depois rodá-lo por cinco dias.

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Building("Main Farm"));
kingdom.AddBuilding(new Building("Old Mine"));
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
    Console.WriteLine($"Buildings: {k.Buildings.Count}");
    Console.WriteLine($"Citizens:  {k.Citizens.Count}");
    Console.Write("Resources: ");
    foreach (var (r, n) in k.Resources.Snapshot())
        Console.Write($"{r}={n}  ");
    Console.WriteLine();
}
```

Build e rode:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Você deve ver do Dia 1 ao Dia 6 impresso, e a comida caindo 2 por dia — dois cidadãos, uma mordida cada. Os outros recursos ainda não mudam. Esse é o trabalho de amanhã. O trabalho de hoje era só fazer o mundo dar um tick.

## Veja um tick acontecer — no depurador

Você viu cinco dias passarem na saída. Agora desacelere *um* dia bem devagar e veja ele acontecer linha por linha, com o mesmo depurador que você conheceu no Módulo 0.8 — desta vez no seu próprio engine.

1. Abra o `Kingdom.cs` e clique na faixa logo à esquerda da primeira linha dentro de `AdvanceDay` — a linha `foreach (var b in Buildings)`. Um ponto de parada (**breakpoint**) vermelho aparece.
2. Aperte **F5**. (Sua janela é a `kingdom-game`, e o `Kingdom.Console` é a única coisa que pode rodar, então o F5 inicia sem perguntar nada.) O programa roda a primeira impressão e então **pausa no seu breakpoint** — bem no começo do primeiro tick.
3. Abra o painel **Variables** à esquerda e ache `Resources` e `Day`. Note o valor da comida, e que `Day` é `1`.
4. Aperte **F10** (*step over*) para rodar uma linha de cada vez. Passe pelo loop dos prédios, depois pelo loop dos cidadãos — **veja o valor da comida cair um a cada vez que um cidadão come**. Siga até `Day++` e veja `Day` virar `2`.
5. Aperte **F11** (*step into*) em `b.Tick(Resources)` para entrar *dentro* do `Tick` de um prédio. Hoje ele está vazio, então você sai direto — mas lembre deste ponto: no Módulo 1.5 as fazendas e minas preenchem esse método, e entrar nele vai mostrar produção de verdade.
6. Aperte **F5** para deixar os dias restantes rodarem, ou o quadrado vermelho **Stop** para encerrar.

É para isso que serve um depurador: em vez de adivinhar o que o `AdvanceDay` faz pela saída impressa, você *vê cada linha mudar o reino*. Toda vez que um número parecer errado mais tarde no ano, é assim que você descobre o porquê.

## Passo 4 — escreva os testes

> **Um detalhe pequeno do C# que você vai encontrar aqui.** O arquivo de testes está no namespace `Kingdom.Engine.Tests`. Dentro desse namespace, se você escrever só a palavra `Kingdom` sozinha, o compilador C# não consegue dizer se você quer dizer o *namespace* `Kingdom` ou a *classe* `Kingdom`. Então ele escolhe o namespace, e o teste não vai compilar. A correção é escrever `global::Kingdom.Engine.Kingdom`. O prefixo `global::` diz ao compilador *"comece bem do topo, depois desça."* Você vai ver isso uma ou duas vezes nos próximos módulos, e só isso.

`tests/Kingdom.Engine.Tests/KingdomTickTests.cs`:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class KingdomTickTests
{
    [Fact]
    public void NewKingdom_StartsAtDay1()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.Day.ShouldBe(1);
    }

    [Fact]
    public void AdvanceDay_IncrementsDayCounter()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AdvanceDay();
        k.Day.ShouldBe(2);
    }

    [Fact]
    public void AdvanceDay_CitizensConsumeFood()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        k.AddCitizen(new Citizen("B"));
        var foodBefore = k.Resources.Get(Resource.Food);
        k.AdvanceDay();
        k.Resources.Get(Resource.Food).ShouldBe(foodBefore - 2);
    }

    [Fact]
    public void AdvanceDay_NoFood_DoesNotCrash()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        // Spend all the food first.
        k.Resources.Spend(Resource.Food, k.Resources.Get(Resource.Food));
        Should.NotThrow(() => k.AdvanceDay());
    }

    [Fact]
    public void AdvanceDay_TenDays_CountsCorrectly()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        for (int i = 0; i < 10; i++) k.AdvanceDay();
        k.Day.ShouldBe(11);
    }
}
```

```powershell
dotnet test
```

Você deve ver `Passed: 16` — onze do Módulo 1.3 mais cinco novos.

## Mexa um pouco

Tente mudar o comprimento do loop de 5 para 100 dias. A comida vai abaixo de zero? Não deveria — `Spend` recusa pegar mais do que o ledger tem e retorna `false`. Verifique lendo `Get(Resource.Food)` depois do loop e vendo se está em zero ou abaixo.

Adicione um terceiro cidadão. A comida agora cai 3 por dia em vez de 2.

Chame `AdvanceDay()` mil vezes seguidas. Quanto tempo leva? Se o seu engine for bem construído, é quase instantâneo — engines devem ser rápidos.

Mova a linha `Day++` para o *topo* de `AdvanceDay` em vez do fundo. O programa ainda roda, mas o significado muda: a primeira chamada agora toca os prédios no "Dia 2." Isso é pequeno e fácil de ignorar. É exatamente o tipo de bug que entra no seu código e depois te confunde uma semana depois.

## A linha-guia

A *linha-guia* deste curso é uma regra que continuamos voltando a ela: o engine nunca decide *quando* dá um tick — esse é o trabalho da shell. Hoje o console roda cinco ticks em um loop `for`. Mais tarde este ano o mesmo engine vai dar um tick por clique em um browser, uma vez por passo em Roblox, ou sempre que um jogador pedir a um site que faça algo. O engine só oferece `AdvanceDay()` e espera ser chamado.

## O que você acabou de fazer

O reino passou de uma fotografia quieta para uma coisa que *se move*. Você adicionou um método no `Building` chamado `Tick` — vazio por enquanto, mas pronto para as fazendas e minas de amanhã preencherem. Você adicionou `Day` e `AdvanceDay` ao `Kingdom`, e o console rodou o mundo por cinco dias em um loop `for`. Ao longo do caminho você conheceu duas ideias que valem guardar: um **side effect** (um método que muda estado em vez de retornar um valor), e **`virtual`** (um método que classes mais específicas têm permissão de substituir). Você também viu a linha-guia do curso em um exemplo real: o engine sabe *como* dar um tick, e a shell decide *quando*. Cinco novos testes passaram; dezesseis no total agora.

**Conceitos que você já sabe nomear:**

- um *tick* e um *game loop* — um passo do tempo de jogo, rodado de novo e de novo
- *side effects* — métodos que mudam estado em vez de retornar valores
- *`virtual`* — um método que classes mais específicas têm permissão de substituir
- a linha-guia — o engine sabe *como*; a shell decide *quando*

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a única grande ideia pegou: fazer o reino avançar um passo. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra `Kingdom.cs`. Sem olhar, escreva o método `AdvanceDay()` da sua própria cabeça. Ele deve fazer três coisas, **nesta ordem**:

1. Todo prédio dá um tick (`b.Tick(Resources)`).
2. Todo cidadão come uma comida.
3. O contador de dias sobe em um.

Depois faça build e rode o console por cinco dias, e veja o número do dia subir e a comida cair.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
public void AdvanceDay()
{
    foreach (var b in Buildings)
        b.Tick(Resources);

    foreach (var _ in Citizens)
        Resources.Spend(Resource.Food, 1);

    Day++;
}
```

A ordem importa: os prédios produzem **antes** de os cidadãos comerem. `AdvanceDay` não retorna nada (`void`) — ele muda o contador de dias e o ledger em vez de devolver um valor. Esse tipo de mudança é um **side effect**.

</details>

## Movimento git da semana — leia seu diff

Você escreveu código de verdade hoje: um override de `Tick` no `Building`, um `AdvanceDay` no `Kingdom`, testes novos. Antes de fazer commit, *leia o que você está prestes a commitar.*

No painel Source Control do VS Code, clique em qualquer arquivo modificado em *Changes* — o diff lado a lado abre. A versão antiga fica à esquerda, a nova à direita, e cada linha mudada está destacada. Leia uma vez antes de preparar (stage).

Esse hábito captura erros — um `Console.WriteLine` que você esqueceu de remover, um comentário pela metade, um arquivo que você não quis mudar. Dois segundos de leitura te poupam do tipo de bug pequeno que leva dez minutos para achar depois.

> **Ou no terminal:** `git diff` (mudanças sem stage), ou `git diff --staged` (o que está na fila para o próximo commit).

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.4 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 1.5 apresenta os *tipos mais específicos* — `Farm`, `Lumberyard`, `Mine`. Cada um vai preencher `Tick` para que produza um recurso diferente. Depois de amanhã, os quatro recursos se movem todo dia, não só a comida.
