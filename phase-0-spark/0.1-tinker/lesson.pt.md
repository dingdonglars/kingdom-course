# Módulo 0.1 — Tinker (Roast-O-Matic v2)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Da última vez, o Roast-O-Matic imprimia as mesmas três zoações para ninguém em particular. Hoje ele pergunta o nome do seu amigo e zoa ele pelo nome. *"Hey BOB — your password is 'password'."* É o mesmo código de antes, mais duas linhas novas. A meta de hoje é pequena. Você vai aprender a fazer uma pergunta ao usuário, guardar o que ele digitou e colocar isso dentro de uma string.

> **Words to watch**
>
> - **variable** — um lugar com nome para guardar um dado e usar depois
> - **string** — um pedaço de texto no código, escrito entre `"aspas duplas"`
> - **method** — um trecho de código com nome que faz uma coisa; `Console.WriteLine` é um method
> - **`Console.ReadLine()`** — o method que pede para o usuário digitar algo e devolve o que ele digitou
> - **string interpolation** — a sintaxe `$"..."` que deixa você enfiar uma variável dentro de uma string

---

## Passo 1 — peça um nome

Abra a sua pasta `RoastOMatic` no VS Code — abra ela **como a própria janela** (*File → Open Folder…* → a pasta `RoastOMatic`), para que a árvore de arquivos à esquerda mostre `RoastOMatic`, e não o repositório `kingdom` inteiro. Um programa, uma janela — esse é o hábito que mantém o *Run* e o depurador simples o ano inteiro. (Tem um guia de uma página, `running-your-project.md`, se você quiser o quadro completo.) Acima da linha `string[] roasts = ...`, adicione isto:

```csharp
Console.Write("Who do you want to roast? ");
var name = Console.ReadLine();
```

`Console.Write` imprime sem pular para a próxima linha. O cursor fica logo depois do ponto de interrogação, então a resposta aparece ao lado da pergunta. `Console.ReadLine()` espera o usuário digitar uma linha e apertar Enter, e então devolve o que ele digitou, como uma string. A gente guarda essa string numa variável chamada `name`.

## Passo 2 — use o nome na zoação

Mude a última linha para a zoação usar o nome:

```csharp
Console.WriteLine($"Hey {name.ToUpper()} — {roast}");
```

O `$` na frente da string é **string interpolation**. Qualquer coisa dentro de `{chaves}` é tratada como C# de verdade. O C# calcula, transforma o resultado em texto e coloca esse texto dentro da string. Então `$"Hey {name.ToUpper()}"` vira `Hey BOB` se `name` for `"bob"`. Isso é bem mais limpo do que juntar pedaços com `+`.

Rode:

```powershell
dotnet run
```

Digite um nome. Aperte Enter. Você recebe uma zoação com o nome da pessoa nela.

## Mexa um pouco

Tente dois nomes por execução. Você pode chamar `Console.ReadLine()` duas vezes. Cada chamada espera a sua própria linha de entrada.

Faça o Roast-O-Matic perguntar que *tipo* de zoação a pessoa quer (leve, apimentada, nuclear) e escolher uma lista de zoações com base na resposta.

Adicione uma zoação que usa dois nomes — *"Hey BOB — at least you're not as bad as ALICE."* Use string interpolation com dois espaços reservados: `$"... {name1} ... {name2} ..."`.

## Dê nome às coisas

Você usou quatro ideias hoje. Cada uma tem um nome que vale a pena saber.

Uma **variable** é um lugar com nome que guarda um valor. A palavra-chave `var` diz ao C# *"descubra o tipo a partir do valor que estou te dando."* Então `var name = Console.ReadLine();` cria uma variável chamada `name`, e o C# vê, pelo `ReadLine()`, que o tipo é `string`.

Uma **string** é qualquer coisa entre `"aspas duplas"` — um pedaço de texto. O C# trata texto como um tipo de verdade. Mais para frente você vai ver a palavra `string` escrita na frente dos nomes de variáveis.

Um **method** é um pedaço de código com nome que você chama escrevendo o nome dele mais `()`. Methods muitas vezes recebem algo dentro dos parênteses (o *argument*) e devolvem algo (o *return value*). `ReadLine()` não recebe nada e devolve uma string. `WriteLine(...)` recebe uma string e não devolve nada.

**String interpolation** é o estilo `$"Hey {name}"` — uma string com espaços reservados que são preenchidos enquanto o programa roda. É o jeito mais limpo de montar texto a partir de valores que você já tem.

## O que você acabou de fazer

Você fez o seu programa fazer uma pergunta e esperar a resposta. Duas linhas a mais transformaram um programa que só imprimia numa pequena conversa: pergunta, lê, usa a resposta. Você conheceu quatro ideias com nome — variable, string, method, string interpolation — que aparecem em todo programa que você vai escrever daqui para frente. As linhas de zoação são as mesmas de ontem, mas o programa se lê de um jeito completamente diferente. É isso que as variáveis te dão.

**Conceitos que você já sabe nomear:**

- **variable** — armazenamento com rótulo para um valor
- **string** — texto entre aspas duplas
- **method** — código com nome que você chama com `()`
- **string interpolation** — `$"..."` com `{espaços reservados}`
- **`Console.ReadLine`** — espera a entrada, devolve uma string

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar:

1. Escreva um programinha que pergunta o nome do usuário e lê o que ele digitar.
2. Imprima `Hello, NAME!` usando o nome dele — com string interpolation (o estilo `$"..."` com `{chaves}`), não `+`.
3. Rode e digite o seu nome.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
Console.Write("What's your name? ");
var name = Console.ReadLine();
Console.WriteLine($"Hello, {name}!");
```

- `Console.ReadLine()` espera o usuário digitar uma linha e devolve o que ele digitou, como uma string.
- O `$` na frente da string é string interpolation. Qualquer coisa dentro de `{ }` é C# de verdade, calculado e colocado no texto.

</details>

## Fechamento

Uma rotina curta no fim de toda lição daqui para frente. Três coisas, um commit, na sua branch da fase:

1. **Quiz** — abra o `quiz.md`. Anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md`, no mesmo formato das anotações que já estão lá. Leve à próxima conversa semanal aquela de que você tiver menos certeza.

2. **Uma linha de progresso** — no `journal/progress.md`. (Se o arquivo ainda não existir, copie `kingdom-course/starter-template/journal/progress.md` para a sua pasta `kingdom/journal/` primeiro — isso te dá o formato e um exemplo pronto.) Adicione uma linha nova abaixo do exemplo:

   ```
   Module 0.1 — Tinker — 2026-MM-DD — added input + interpolation. Learnt: variables let the same code mean two different things.
   ```

   A metade do "Learnt" é a parte que vale a pena pensar. Uma frase, com as suas palavras.

3. **Faça commit e push.**

   - Primeiro, **confira que você está no repositório certo.** Abra o Source Control (`Ctrl + Shift + G G`) — o cabeçalho do painel tem que dizer **`kingdom`** (a pasta é `C:\code\kingdom`), *não* `kingdom-course`. Duas pastas, dois papéis, lembra do primer — o seu trabalho vai em `kingdom`. A pasta `kingdom-course` é só leitura; nada do que você escreve vai lá. **Acerte isso toda vez** — fazer commit dentro de `kingdom-course` é o erro mais fácil de cometer nas primeiras semanas, e é chato de desfazer.
   - Prepare (stage) `journal/quiz-notes.md` e `journal/progress.md` (clique no `+` ao lado de cada um).
   - Mensagem do commit: `Module 0.1 done`.
   - Clique no check e depois em Sync.

   > **Ou no terminal** — mesma regra de pasta. Rode `pwd` primeiro; deve aparecer `C:\code\kingdom`. Se aparecer `C:\code\kingdom-course`, rode `cd ..\kingdom` antes de continuar.
   > ```powershell
   > git add journal/quiz-notes.md journal/progress.md
   > git commit -m "Module 0.1 done"
   > git push
   > ```

Depois poste no `#wins` — uma linha sobre o dia de hoje, mais a URL do commit que você acabou de enviar. (Abra o seu repositório `kingdom` no github.com, clique em *Commits*, clique no mais recente, copie a URL da barra de endereço.)

O ponto não é burocracia. É um rastro pequeno e visível de que você fez a lição — prova para você, sinal para o Lars.

## Próximo

O Módulo 0.2 usa aleatoriedade de propósito — o seu primeiro jogo de adivinhação, em que o programa escolhe um número e você tenta achar.
