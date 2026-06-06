# Módulo 1.3 — Unit Testing Arrives

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

> **Aquecimento — 30 segundos, de cabeça.** Antes de hoje, traga de volta o que você construiu no Módulo 1.2:
>
> 1. Qual projeto usa o outro — o engine usa o console, ou o console usa o engine?
> 2. O engine tem uma regra que nunca pode quebrar. Qual é?
>
> Inseguro em alguma? Releia primeiro o **Módulo 1.2** — é a ideia que dá nome ao curso inteiro, e a aula de hoje fica bem em cima dela. Leve qualquer coisa que ficou frágil para o sync semanal.

Hoje você escreve testes. De verdade, com xUnit e Shouldly, rodados com `dotnet test` no terminal. No fim da aula o engine tem onze testes passando. A divisão que você fez no 1.2 é o que torna hoje possível — o projeto de testes vai referenciar o engine da mesma forma que o console faz, e nunca tocar o console de jeito nenhum.

Um unit test é um pequeno pedaço de código que verifica uma coisa que seu código faz. Por que escrever testes agora, enquanto o engine ainda é simples? Pense em um alarme de fumaça. Você o coloca antes de haver fogo, não depois. Você escreveu `Building.Upgrade()` na semana passada. Você tem certeza que funciona. Seis semanas de agora você vai mudar `Building` para que cada nível custe uma quantidade diferente. Você quebrou alguma coisa? Sem testes, você só descobre quando algo mais crasha — talvez minutos depois, talvez meses depois. Com testes, `dotnet test` te diz em 0,3 segundos.

Testes também são uma espécie de documentação que fica correta. Um teste chamado `Upgrade_IncreasesLevelByOne` *diz o que `Upgrade` deveria fazer*. O teste roda toda vez que você faz build, então essa descrição nunca pode parar silenciosamente de combinar com o código de verdade.

Uma coisa para saber antes de começar: esse não é um tópico de um dia que você marca como feito. De hoje em diante, os testes andam junto com o engine pelo resto do ano. Quase toda aula daqui adiciona um recurso *e* os testes que o guardam. Então vale a pena colocar o ritmo — organize, aja, verifique — nos seus dedos agora, enquanto as coisas que você está testando ainda são pequenas e fáceis de entender.

> **Words to watch**
>
> - **unit test** — uma verificação automática pequena que exercita um pedaço de comportamento
> - **xUnit** — o framework de testes (rodador de testes mais os atributos `[Fact]` e `[Theory]`)
> - **Shouldly** — uma biblioteca de asserções com mensagens de falha legíveis (`x.ShouldBe(5)`)
> - **`[Fact]`** — marca um método como um teste que não recebe parâmetros
> - **`[Theory]` + `[InlineData]`** — roda a mesma lógica de teste com múltiplas entradas
> - **arrange / act / assert** — o layout padrão de três passos para um teste

---

## Passo 1 — configure o projeto de testes

Na raiz do seu repositório (ao lado de `Kingdom.Engine/` e `Kingdom.Console/`):

```powershell
mkdir tests
cd tests
dotnet new xunit -n Kingdom.Engine.Tests
cd Kingdom.Engine.Tests
dotnet add package Shouldly
dotnet add reference ..\..\Kingdom.Engine\Kingdom.Engine.csproj
cd ..\..
dotnet sln add tests\Kingdom.Engine.Tests
```

Agora você tem:

```
your-repo/
├─ Kingdom.Engine/
├─ Kingdom.Console/
├─ tests/
│   └─ Kingdom.Engine.Tests/
└─ Kingdom.slnx
```

O projeto de testes referencia o `Kingdom.Engine` diretamente, da mesma forma que o `Kingdom.Console` faz. Não referencia o console de jeito nenhum. Os testes verificam o engine. Eles não precisam saber que existe um console.

## Passo 2 — seus primeiros três testes

Abra `tests/Kingdom.Engine.Tests/UnitTest1.cs`. Renomeie-o para `BuildingTests.cs` (no VS Code: renomeie no painel Explorer, ou clique com o botão direito e escolha *Rename*). Substitua o conteúdo:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class BuildingTests
{
    [Fact]
    public void NewBuilding_StartsAtLevelOne()
    {
        // arrange
        var b = new Building("Farm");
        // act — nothing extra
        // assert
        b.Level.ShouldBe(1);
    }

    [Fact]
    public void Upgrade_IncreasesLevelByOne()
    {
        var b = new Building("Farm");
        b.Upgrade();
        b.Level.ShouldBe(2);
    }

    [Fact]
    public void Upgrade_CalledThreeTimes_LevelIsFour()
    {
        var b = new Building("Farm");
        b.Upgrade();
        b.Upgrade();
        b.Upgrade();
        b.Level.ShouldBe(4);
    }
}
```

Os nomes seguem a regra do `STANDARDS.md`: `Method_Scenario_ExpectedBehaviour`. Um nome assim se lê quase como uma frase. Cada teste é construído da mesma forma — *organize* (configure as coisas), *aja* (faça a coisa), *verifique* (confira o resultado). Os comentários estão no primeiro teste para mostrar a você os três passos. Os outros testes os deixam de fora, porque o padrão é fácil de ver quando você já o conhece.

Rode da raiz do repositório:

```powershell
dotnet test
```

Você deve ver algo como:

```
Passed!  - Failed: 0, Passed: 3, Skipped: 0, Total: 3
```

Três marcas de ok verdes. O engine tem testes agora.

## Passo 3 — mais testes, para o `ResourceLedger`

Crie `tests/Kingdom.Engine.Tests/ResourceLedgerTests.cs`. O arquivo completo está em `starter/tests/Kingdom.Engine.Tests/ResourceLedgerTests.cs`. Ele tem oito testes. Verificam o estado inicial de um ledger vazio, `Add`, `Spend` (quando funciona e quando falha), o erro lançado para uma quantia negativa, e um `[Theory]` com três casos `[InlineData]` que rodam o mesmo teste com entradas diferentes.

Rode `dotnet test` de novo. Você deve ter agora 11 testes passando.

## Passo 4 — a diferença entre `[Fact]` e `[Theory]`

`[Fact]` é um único teste, sem parâmetros. `[Theory]` é a mesma ideia mas recebe parâmetros por meio de `[InlineData]`:

```csharp
[Theory]
[InlineData(Resource.Gold, 100)]
[InlineData(Resource.Wood, 50)]
[InlineData(Resource.Stone, 20)]
public void Add_StartingFromZero_GetReturnsAmount(Resource r, int amount)
{
    var ledger = new ResourceLedger();
    ledger.Add(r, amount);
    ledger.Get(r).ShouldBe(amount);
}
```

Três linhas `[InlineData]` viram três execuções do mesmo teste, cada uma com entradas diferentes. Isso te poupa de escrever o mesmo teste três vezes. Use `[Theory]` sempre que você escreveria um teste copiado mudando apenas um número.

## Depure um teste

No Módulo 0.8 você conheceu o depurador apertando **F5** em um programa. Os testes te dão um segundo jeito de entrar — e muitas vezes o mais prático. Você pode colocar um breakpoint no código do seu engine e **depurar direto para dentro dele a partir de um teste**. Não há console para rodar nem nada para escolher iniciar: o teste monta um cenário pequeno e exato, e você vê seu código lidar com ele, linha por linha.

**Onde ficam os botões.** Passe o mouse por cima de qualquer `[Fact]` e o VS Code mostra dois links pequenos logo acima dele: **Run | Debug**. (Há também a view **Testing** — o ícone de béquer na barra à esquerda — que lista cada teste com seus próprios botões de run e debug.) O *Run* só roda o teste; o *Debug* o roda com o depurador anexado, então seus breakpoints o param.

**Experimente no `Upgrade`:**

1. Abra `Kingdom.Engine/Building.cs` e coloque um breakpoint na linha `Level++;` dentro de `Upgrade()`.
2. Abra `BuildingTests.cs` e clique em **Debug** acima de `Upgrade_IncreasesLevelByOne`.
3. O teste começa e **pausa dentro de `Upgrade()`** — com o pequeno mundo do teste já montado: um `Building` chamado `"Farm"`, com `Level` em `1`. Confirme isso no painel **Variables** à esquerda.
4. Aperte **F10** para rodar a linha `Level++;`. Veja `Level` mudar de `1` para `2`.
5. Aperte **Shift+F11** (*step out*) para voltar para o teste — você cai em `b.Level.ShouldBe(2)`, a linha que checa o resultado.
6. Aperte **F5** para terminar. O teste passa (verde).

Você acabou de ver a exata linha que o teste checa, por dentro — não a saída, o *código de verdade rodando*.

**É assim que você investiga um teste que ficou vermelho.** Quando um teste falha e a mensagem ainda não te diz *por quê*, não adivinhe: coloque um breakpoint no método que o teste chama, clique em **Debug** no teste que falhou, e avance passo a passo até ver um valor dar errado. Experimente agora — quebre um teste de propósito (o **Mexa um pouco** abaixo mostra o jeito mais rápido), depois depure para dentro dele e ache a linha que está errada.

Uma coisa prática: depurar um teste inicia o **test runner**, não o seu console — então não importa qual projeto normalmente iniciaria. O teste te leva direto ao código que ele exercita.

> Você conheceu o depurador no **Módulo 0.8**. Para o conjunto completo de teclas, painéis e truques — incluindo este atalho de depurar testes — mantenha o **`using-the-debugger.md`** (na raiz do curso) aberto como referência.

## Mexa um pouco

Faça um teste falhar de propósito. Mude um `.ShouldBe(50)` para `.ShouldBe(51)` e rode `dotnet test`. Leia a mensagem de falha — o Shouldly diz exatamente o que esperava e o que realmente recebeu, e aponta para a linha. Compare com a verificação própria do xUnit (`Assert.Equal(50, ledger.Get(Resource.Wood))`) — mesmo resultado, mas a mensagem te ajuda menos. É por isso que o projeto usa Shouldly.

Adicione um teste para `Citizen` — chame-o de `Citizen_New_StartsIdle` e verifique que `c.Job` é `"Idle"` logo depois de criar o cidadão. Um arquivo, três linhas. Você vai escrever uma centena desses este ano.

Tente `Should.NotThrow(() => ...)` para verificar que algum código *não* lança um erro. Isso é útil quando "não crasha" é a coisa que você se preocupa.

## O que você acabou de fazer

O engine está protegido por testes agora. Onze pequenos testes que rodam em menos de um segundo e provam tudo que você disse sobre `Building`, `Upgrade` e `ResourceLedger`. Você conheceu as três peças que todo framework de testes te dá — um jeito de marcar um método como teste (`[Fact]`), um jeito de rodar um teste com várias entradas (`[Theory]` + `[InlineData]`), e um jeito de dizer o que você espera (`ShouldBe`). Você também conheceu o layout *organize / aja / verifique*, que é o mesmo em todo framework de testes que você vai usar, em qualquer linguagem. De agora em diante, todo recurso novo vem com os testes que provam que funciona — e os testes ficam verdes, porque se qualquer coisa os quebrar, você vai ver antes de fazer commit.

**Conceitos que você já sabe nomear:**

- **unit test** — verificação automática pequena, um comportamento
- **`[Fact]` vs `[Theory]`** — teste único vs parametrizado
- **arrange / act / assert** — layout de três partes para um teste
- **`ShouldBe`** — forma de asserção legível do Shouldly
- **`Method_Scenario_Expected`** — convenção de nome de teste dos STANDARDS

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a única grande ideia pegou: a forma de um teste. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

No seu projeto de testes, adicione um teste novo de memória, chamado `Upgrade_CalledTwice_LevelIsThree`:

1. **Organize** — crie um `Building` novo.
2. **Aja** — chame `Upgrade()` duas vezes.
3. **Verifique** — confira que o nível é `3` com `ShouldBe`.
4. Não esqueça a linha `[Fact]` acima do método.

Depois rode `dotnet test` e veja a contagem subir em um.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
[Fact]
public void Upgrade_CalledTwice_LevelIsThree()
{
    // arrange
    var b = new Building("Farm");
    // act
    b.Upgrade();
    b.Upgrade();
    // assert
    b.Level.ShouldBe(3);
}
```

`dotnet test` deve agora reportar um teste a mais passando do que antes. Se você esquecer a linha `[Fact]`, o rodador nunca vê o método e a contagem não muda — esse é o erro mais comum.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.3 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 1.4 adiciona o **game loop** — o tick que faz o reino avançar no tempo. Os testes que você escreveu hoje vão capturar qualquer coisa que quebrar lá, e vamos adicionar cinco a mais para o novo comportamento. Onze se tornam dezesseis.
