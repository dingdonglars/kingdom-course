# Módulo 0.2 — Number Guess

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O computador escolhe um número entre 1 e 100. Você chuta. Ele diz se você foi alto demais, baixo demais ou acertou — e é um pouco grosseiro com isso. Hoje é o seu primeiro programa com um *loop*: código que roda de novo e de novo até algo mandar parar. No final você vai ter um joguinho de verdade, e vai conhecer três ideias novas — loops, condicionais e como transformar texto digitado em número.

> **Words to watch**
>
> - **loop** — um pedaço de código que roda repetidamente até você mandar parar
> - **`while`** — a palavra-chave do C# que inicia um loop do tipo "continue fazendo isso até algo mudar"
> - **conditional** — `if` / `else` — uma bifurcação no seu código baseada em se algo é verdadeiro ou falso
> - **`int.Parse(...)`** — o method que transforma uma string de dígitos em um `int` de verdade
> - **`break`** — a palavra-chave do C# que salta direto para fora do loop em que você está

---

## Passo 1 — crie um novo projeto

Crie uma nova pasta ao lado do `RoastOMatic`, no topo do seu repositório. No PowerShell:

```powershell
cd ..
dotnet new console -n NumberGuess
cd NumberGuess
```

Agora você tem dois projetos de console sentados lado a lado no mesmo repositório. Eles não sabem um do outro. São dois programinhas separados.

## Passo 2 — escreva o jogo

Abra `Program.cs` no VS Code. Substitua o conteúdo por:

```csharp
var random = new Random();
var secret = random.Next(1, 101);  // 1..100 inclusive
var guesses = 0;

Console.WriteLine("I'm thinking of a number between 1 and 100. Guess.");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    var guess = int.Parse(input);
    guesses++;

    if (guess < secret)
    {
        Console.WriteLine("Too low. And weak.");
    }
    else if (guess > secret)
    {
        Console.WriteLine("Too high. Calm down.");
    }
    else
    {
        Console.WriteLine($"Got it in {guesses}. Bare minimum effort, but I'll allow it.");
        break;
    }
}
```

Muita coisa está acontecendo nessas trinta linhas. As três primeiras linhas montam o jogo: uma coisa que faz números aleatórios, o número secreto que ela escolheu e um contador de quantos chutes o jogador usou.

O bloco `while (true) { ... }` é o seu primeiro **loop**. Ele roda o código dentro de novo e de novo, para sempre, até algo mandar parar. Dentro do loop, o programa mostra um prompt, lê o que o jogador digitou, transforma esse texto em número inteiro com `int.Parse` e soma um ao contador.

A cadeia `if / else if / else` é um **conditional**. Pense nisso como uma bifurcação no caminho. Só um dos três galhos roda a cada volta do loop, dependendo de qual condição é verdadeira. Quando o jogador finalmente acerta, o galho `else` imprime a mensagem de vitória e a palavra-chave `break` salta para fora do loop, encerrando o programa.

Rode:

```powershell
dotnet run
```

Chute até acertar.

## Mexa um pouco

Deixe as mensagens ainda mais grosseiras. O programa é seu, então se divirta.

Acompanhe o número de chutes e avalie o jogador no final. *"1 chute: você está trapaçando? 5 chutes: aceitável. 50 chutes: já pensou em outro hobby?"*

Adicione uma opção de desistir. Se o usuário digitar `quit`, mostre o número e saia. Você vai precisar de um `if` extra antes da linha do `int.Parse`, porque `quit` não é número.

Transforme o intervalo em variável. Mova `1` e `101` para números inteiros com nome no topo, para que você possa mudar a dificuldade editando um lugar só em vez de dois.

## Dê nome às coisas

Um **loop** é código que roda de novo e de novo. A forma `while (condition) { ... }` continua rodando enquanto a condição for verdadeira. `while (true)` roda para sempre, até um `break;` dentro do corpo saltar para fora.

Um **conditional** é a forma `if (...) { } else if (...) { } else { }`. O C# verifica as condições de cima para baixo. A primeira que for verdadeira roda o seu bloco, e o restante é pulado.

A **classe `Random`** sabe como fazer números *pseudo-aleatórios*. São números que *parecem* aleatórios, mas são calculados por uma fórmula matemática fixa. Parecem aleatórios o suficiente para um jogo, e você pode obter a mesma sequência de novo se quiser. Você criou um com `new Random()`, que te dá um objeto `Random`, guardado na variável `random`. Depois pediu um número com `random.Next(1, 101)`, que é uma chamada de method nesse objeto. O número vai de 1 a 100. O número menor (1) está incluído, mas o maior (101) não está. Essa é uma escolha que a Microsoft fez, e surpreende quase todo mundo na primeira vez.

## O que você acabou de fazer

Você escreveu um jogo de verdade. O programa escolhe um número, você chuta, ele diz como foi, você chuta de novo. Esse loop está no centro de todo programa que faz mais do que imprimir uma vez e parar. Você conheceu três ideias com nome: loops (`while`), condicionais (`if`/`else if`/`else`) e transformar texto em número (`int.Parse`). O jogo inteiro tem cerca de trinta linhas de código, e uma delas é um comentário.

**Conceitos que você já sabe nomear:**

- **loop `while`** — repita até algo mudar
- **`break`** — salta para fora do loop atual
- **conditional** — bifurcação `if`/`else if`/`else`
- **`int.Parse`** — transforma uma string de dígitos em `int`
- **`Random.Next(min, max)`** — número aleatório, limite superior excluído

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a grande ideia pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar, escreva um pequeno programa com um loop `while (true)` que conta:

1. Comece um número em `1`.
2. A cada volta do loop, imprima-o e adicione `1`.
3. Quando chegar em `5`, use `break` para saltar fora do loop.

Deve imprimir `1 2 3 4 5` e depois parar.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
var n = 1;
while (true)
{
    Console.WriteLine(n);
    if (n == 5)
    {
        break;
    }
    n++;
}
```

- `while (true)` roda para sempre, até um `break` dentro saltar para fora.
- O `if` verifica uma condição a cada volta. Quando `n` é `5`, `break` encerra o loop.
- `n++` adiciona `1` a `n`.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote as suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 0.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare os dois arquivos, mensagem do commit `Module 0.2 done`, Sync.
4. **Poste em `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI se você precisar relembrar. Traga as respostas do quiz das quais você estiver menos certo para o próximo sync semanal.

## Próximo

O Módulo 0.3 transforma isso em uma pequena aventura — vários quartos, escolhas que mudam o que acontece, o seu primeiro jogo de texto. Os mesmos loops e condicionais, só com uma história maior.
