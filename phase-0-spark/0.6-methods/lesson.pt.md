# Módulo 0.6 — Methods Deep Dive

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

De volta ao Tiny Adventure você escreveu três methods: `Hallway`, `Kitchen` e `Library`. Você os chamou e eles funcionaram. Mas a gente nunca falou sobre como um method é construído — os valores que você coloca nele e a resposta que ele devolve. Hoje vamos falar.

Pense em um method como um botão com um nome. Quando o botão existe, você o aperta e ele faz o trabalho. Você não o reconstrói toda vez. Em um jogo, "Jump" é um botão. Você aperta, o seu personagem pula. Você não pensa em *como* o pulo funciona cada vez que aperta. Um method é essa mesma ideia, em código.

Veja do que um method é feito:

- ele pode receber *parâmetros* — valores que você passa para ele
- ele pode devolver um *valor de retorno* — uma resposta que ele te devolve
- ele vive em uma classe

Também vamos conhecer o *overloading* (dois methods com o mesmo nome) e a palavra `static`, que está dentro de todo `Console.WriteLine` que você digitou. Não se preocupe — vamos ver um de cada vez.

> **Words to watch**
>
> - **method** — um pedaço de código com nome que faz um trabalho
> - **parameter** — um valor passado *para dentro* de um method, com nome dentro dos parênteses `( )`
> - **argument** — o *valor real* que você passa quando chama (parameter é o *nome*; argument é o *valor*)
> - **return value** — o valor que um method devolve; você escreve o tipo dele logo antes do nome do method
> - **`void`** — o tipo de retorno especial que significa "este method não devolve nada"
> - **overload** — dois methods com o mesmo nome mas tipos de parâmetros diferentes
> - **`static`** — um method que pertence ao *tipo* em si, não a uma *instância* específica

---

## Passo 1 — crie um projeto de demonstração de methods

```powershell
cd <your-repo-root>
dotnet new console -n MethodsDemo
cd MethodsDemo
```

Substitua `Program.cs`:

```csharp
// A method with NO parameters and NO return value
Helpers.SayHello();

// A method with ONE parameter and NO return value
Helpers.Greet("Eldoria");

// A method with TWO parameters and a RETURN value (int)
int total = Helpers.AddGold(120, 35);
Console.WriteLine($"Total gold: {total}");

// A method with overloading: same name, different parameter types
Console.WriteLine(Helpers.Square(5));        // int version
Console.WriteLine(Helpers.Square(2.5));      // double version

// --- Method definitions live on a static class.
// Static classes can hold overloaded methods (top-level local functions can't).

static class Helpers
{
    public static void SayHello()
    {
        Console.WriteLine("Hello from Eldoria.");
    }

    public static void Greet(string kingdom)
    {
        Console.WriteLine($"Greetings, {kingdom}.");
    }

    public static int AddGold(int a, int b)
    {
        return a + b;
    }

    public static int Square(int x) => x * x;
    public static double Square(double x) => x * x;
}
```

Por que colocá-los dentro de uma `static class`? Aqui está o motivo.

Em um programa de console pequeno, qualquer method que você escreve no *topo* do arquivo se chama *local function*. O C# tem uma regra para local functions: você não pode dar a duas delas o mesmo nome.

Mas a gente *quer* dois methods chamados `Square` — um para números inteiros, um para decimais. Então os colocamos dentro de uma `static class`.

Uma `static class` é apenas uma caixa com nome para methods. Você nunca a cria com `new`. Você chama os methods dela diretamente, como `Helpers.Square(5)`. Methods dentro dela são *static methods*, e static methods podem ter o mesmo nome.

Você já usou isso sem perceber. `Math.Max`, `Console.WriteLine` e `File.ReadAllText` são todos static methods que vivem em classes.

Rode:

```powershell
dotnet run
```

Você deve ver:

```
Hello from Eldoria.
Greetings, Eldoria.
Total gold: 155
25
6.25
```

## Mexa um pouco

Agora é a sua vez. Tente um de cada vez.

Adicione um method `int Multiply(int a, int b)` e use-o. O padrão é exatamente como `AddGold`.

Adicione um terceiro overload de `Square` para `long`: `public static long Square(long x) => x * x;`. Depois chame `Helpers.Square(1_000_000_000L)` e veja o resultado.

Tente escrever `int Square(int y)` ao lado de `int Square(int x)` — mesmo tipo, só o nome é diferente. Você vai ter um erro e o código não vai rodar. Overloads precisam de *tipos* de parâmetros diferentes, não só nomes diferentes. Por quê? Quando você chama o method, o C# só vê os *tipos* dos valores que você passa. Ele não consegue ver a diferença entre os dois methods só pelo nome do parâmetro.

Dê a `Greet` um *segundo* parâmetro, uma palavra para gritar primeiro: `Greet("Eldoria", "Hail!")` deve imprimir `"Hail! Greetings, Eldoria."` Isso é outro overload — mesmo nome, mas dois parâmetros em vez de um.

## Dê nome às coisas

Você acabou de usar essas ideias. Agora vamos dar nomes a elas, com um exemplo para cada.

**Parameter vs argument.** Essas duas palavras se confundem o tempo todo, então vamos deixar claro.

Um *parameter* é o nome que você escreve quando cria o method. Em `void Greet(string kingdom)`, o parameter é `kingdom`. É um slot vazio, esperando por um valor.

Um *argument* é o valor real que você coloca nesse slot quando chama o method. Em `Greet("Eldoria")`, o argument é `"Eldoria"`.

Imagine uma máquina de venda automática. O slot do teclado que diz "insira um código" é o parameter. O `B4` que você realmente digita é o argument. Mesma máquina, salgadinho diferente, dependendo do que você digita.

**Return value.** Esta é a resposta que um method te devolve.

Você escreve o tipo dessa resposta logo antes do nome do method. `int AddGold(...)` devolve um `int`. É por isso que `int total = Helpers.AddGold(120, 35);` pode armazenar a resposta em `total`.

Alguns methods não devolvem nada. `void SayHello()` só imprime e para. `void` é a palavra especial para "não devolvo nada." Um interruptor de luz é como um method `void`: ele liga a luz, mas não te entrega nada.

**Overload.** Dois methods podem ter o mesmo nome, desde que os parâmetros sejam diferentes. Isso é um overload.

O C# decide qual você quis dizer olhando o que você passou. `Square(5)` roda a versão `int`. `Square(2.5)` roda a versão `double`.

A gente faz o mesmo em português do dia a dia. "Jogar bola", "jogar um jogo", "jogar uma carta" — mesma palavra, e você sabe qual significado cabe pelo que vem depois. O C# faz isso com os tipos dos seus arguments.

Uma regra: você não pode fazer dois overloads que recebem os mesmos parâmetros e diferem só no que devolvem. O C# não teria como ver a diferença entre eles.

**Methods com corpo de expressão.** `int Square(int x) => x * x;` é um jeito mais curto de escrever isto:

```csharp
int Square(int x) { return x * x; }
```

A flecha `=>` se chama *expression body*. Funciona bem quando o method inteiro é uma linha curta. Use a forma `{ ... }` quando precisar de mais de uma linha.

**`static`.** Você ainda não escreveu essa palavra, e isso é normal.

No topo do arquivo — dentro do `Program.cs` de um pequeno aplicativo de console — o C# configura isso para você, e `static` já está no lugar. Você vai começar a escrevê-lo na Fase 1, quando construir as suas próprias classes.

Versão curta: um method `static` pertence ao *tipo* em si. Você pode chamá-lo sem construir nada primeiro — como ir a um balcão de ajuda que sempre está aberto. `Math.Max(3, 7)` simplesmente funciona.

Um method não-static (também chamado *instance* method) pertence a um objeto específico que você criou com `new`. Você precisa do objeto primeiro — da mesma forma que você precisa de um personagem do jogo de verdade antes de poder dizer àquele personagem para pular. Mais sobre isso na Fase 1.

## Por que methods existem

Três motivos. Você vai ver os três de novo e de novo este ano.

**Nomenclatura.** Um method coloca um nome em um pedaço de código. `CalculateTax(income)` te diz o que ele faz imediatamente. Vinte linhas de matemática sem nome não dizem nada até você ler cada linha. É a diferença entre uma receita intitulada "Fazer panquecas" e os mesmos passos sem título no topo.

**Reutilização.** Escreva uma vez, chame quantas quiser. `Greet("Eldoria")` e `Greet("Westmark")` rodam o mesmo method. Sem copiar e colar. E se você corrigir um bug dentro do method, todas as chamadas recebem o conserto.

**Esconder detalhes.** Quando você chama `Console.WriteLine`, você não pensa em fontes, pixels ou a tela. Você só diz "imprima isso." O method guarda todo esse detalhe para que você não precise. É como o pedal de freio no carro: você pisa, o carro freia, e você nunca pensa em como funciona por dentro. Muita programação é escolher o que esconder por trás de um bom nome de method.

## O que você acabou de fazer

Boa — você deu nomes ao que já estava fazendo.

Você pegou os methods que escreveu no Módulo 0.3 e aprendeu como se chama cada parte. Um method tem parameters (os nomes nos parênteses). Ele recebe arguments (os valores reais que você passa). Ele pode devolver um valor (o tipo antes do nome do method), ou não devolver nada (`void`).

Você viu que dois methods podem ter o mesmo nome quando os parâmetros são diferentes — isso é overloading. E você viu por que nossos methods ficam dentro de uma `static class` em vez de serem local functions.

Cinco ideias novas aparecem aqui. Todo method que você escrever daqui para frente usa algumas delas, então logo vão parecer normais.

**Conceitos que você já sabe nomear:**

- **parameter vs argument** — o nome vs o valor passado
- **return value** — o que vem de volta; `void` significa nada
- **overload** — mesmo nome, tipos de parâmetros diferentes
- **expression body** — atalho `=> expr` para `{ return expr; }`
- **`static`** — pertence ao tipo, não a uma instância

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a grande ideia pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar:

1. Escreva um method chamado `Add` que recebe dois parâmetros `int` e devolve a soma deles.
2. Chame-o, guarde a resposta em uma variável e imprima-a.
3. Rode e verifique se o número está certo.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
int total = Add(7, 5);
Console.WriteLine(total);

int Add(int a, int b)
{
    return a + b;
}
```

- Os dois nomes `int` nos parênteses — `a` e `b` — são os parameters (slots vazios).
- `7` e `5` são os arguments (os valores reais que você passa).
- O `int` antes de `Add` é o tipo de retorno. `return a + b;` devolve a resposta, então `total` guarda `12`.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote as suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 0.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare os dois arquivos, mensagem do commit `Module 0.6 done`, Sync.
4. **Poste em `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI se você precisar relembrar. Traga as respostas do quiz das quais você estiver menos certo para o próximo sync semanal.

## Próximo

O Módulo 0.7 é coleções — `List<T>`, arrays, `Dictionary<TKey, TValue>`. Essas são as caixas onde você coloca muitas coisas. Você já conheceu uma no Módulo 0.3. Na sequência vamos nomear as demais e ver quando usar cada uma.
