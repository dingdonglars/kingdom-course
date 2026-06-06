# Módulo 0.3 — Tiny Adventure

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje você constrói uma aventura de texto com três quartos. *"Você está num corredor escuro. Tem uma porta ao norte e uma ao leste. >"* O mundo é seu para projetar; esta lição te dá a estrutura para construir. Duas ideias novas aparecem no caminho: uma *lista* (como o jogador carrega itens entre os quartos) e escrever seus próprios *methods* (um method por quarto, cada um chamando o próximo enquanto o jogador caminha).

> **Words to watch**
>
> - **list** — uma coleção ordenada de coisas que você pode adicionar, remover e percorrer em loop
> - **`List<string>`** — uma lista especificamente de strings (itens de texto)
> - **method (definido)** — até agora você só *chamou* methods (`Console.WriteLine(...)`); hoje você *escreve* um
> - **organisation** — dividir o seu código em vários methods para que cada um faça uma coisa

---

## Passo 1 — crie um novo projeto

```powershell
cd ..
dotnet new console -n TinyAdventure
cd TinyAdventure
```

Abra `Program.cs` e substitua o conteúdo:

```csharp
var inventory = new List<string>();

Hallway();

void Hallway()
{
    Console.WriteLine();
    Console.WriteLine("You stand in a dim hallway. There's a door north and a door east.");
    Console.Write("> ");
    var choice = Console.ReadLine().Trim().ToLower();

    if (choice == "north") Kitchen();
    else if (choice == "east") Library();
    else
    {
        Console.WriteLine("That's not a direction. Try 'north' or 'east'.");
        Hallway();
    }
}

void Kitchen()
{
    Console.WriteLine();
    Console.WriteLine("A kitchen, smells of stew. There's a knife on the counter.");
    Console.Write("> ");
    var choice = Console.ReadLine().Trim().ToLower();

    if (choice == "take knife")
    {
        inventory.Add("knife");
        Console.WriteLine("You pocket the knife. The cook is going to notice.");
        Hallway();
    }
    else if (choice == "back" || choice == "south")
    {
        Hallway();
    }
    else
    {
        Console.WriteLine("Try 'take knife' or 'back'.");
        Kitchen();
    }
}

void Library()
{
    Console.WriteLine();
    Console.WriteLine("A library, dust motes in the air. A book lies open on the table.");
    Console.Write("> ");
    var choice = Console.ReadLine().Trim().ToLower();

    if (choice == "read book")
    {
        if (inventory.Contains("knife"))
        {
            Console.WriteLine("The book reveals: 'The cook took the family's last gold. The knife is justice.'");
            Console.WriteLine("YOU WIN. Probably.");
        }
        else
        {
            Console.WriteLine("The book is in a language you don't know. You should look around first.");
            Library();
        }
    }
    else if (choice == "back" || choice == "west")
    {
        Hallway();
    }
    else
    {
        Console.WriteLine("Try 'read book' or 'back'.");
        Library();
    }
}
```

Há duas ideias novas nesse código que merecem uma olhada mais de perto. A primeira linha cria um `List<string>` — uma lista que guarda itens de texto e começa vazia. Cada method (`Hallway`, `Kitchen`, `Library`) é um quarto. Quando o jogador escolhe uma direção, o method do quarto atual chama o method do próximo quarto diretamente. Os methods se chamam uns aos outros, e é assim que o jogador se move.

Mais uma pequena coisa a notar: `Console.ReadLine().Trim().ToLower()` lê uma linha, remove espaços no começo e no fim (`Trim`) e a transforma em letras minúsculas (`ToLower`). Assim `"NORTH"`, `" north "` e `"North"` todos combinam com `"north"`.

Rode:

```powershell
dotnet run
```

Ande pelos quartos. Tente todos os comandos. Tente vencer o jogo (pegue a faca, depois leia o livro).

## Mexa um pouco

Adicione um quarto novo — um porão, um jardim, uma torre — e conecte-o a um dos quartos que você já tem.

Adicione um item que o jogador pode pegar no quarto novo. Talvez uma chave, uma vela ou uma carta antiga.

Adicione uma segunda forma de vencer que precisa do item novo. Agora o jogo tem dois jeitos de ganhar.

Faça os quartos listar o que está no `inventory` do jogador quando ele digitar `look` ou `inventory`. Você vai precisar de um pequeno loop para imprimir cada item.

## Dê nome às coisas

Uma **list** guarda coisas em ordem. `List<string>` é o tipo de lista do C# que pode crescer e encolher, e esta guarda strings. Você criou uma vazia com `new List<string>()`. Você adicionou a ela com `inventory.Add(...)`. Você verificou se algo estava nela com `inventory.Contains(...)`. A parte `<string>` diz *"esta é uma lista de strings."* Você vai ver `<int>`, `<Building>` e outros depois. Os colchetes angulares são a forma de dizer a esse tipo de lista qual tipo ela guarda.

Um **method (definido)** é um que você escreve. Você escreveu três hoje: `Hallway()`, `Kitchen()`, `Library()`. A palavra-chave `void` na frente diz que o method não devolve um valor — ele só *faz* algo. O corpo roda quando o method é chamado. Quando o corpo termina, o programa volta para a linha que o chamou.

**Organisation** é o que você obtém quando divide o código em methods. Imagine o mesmo jogo escrito como uma enorme cadeia de `if/else`: três quartos, três conjuntos de escolhas, tudo junto. Você não conseguiria ler. Com cada quarto como seu próprio method, as partes do código se alinham com as partes do jogo. Esse alinhamento é o objetivo.

## O que você acabou de fazer

Você escreveu um jogo de verdade que lembra coisas — um inventário que o jogador carrega entre os quartos — e você escreveu seus próprios methods pela primeira vez. Três quartos, cada um seu próprio method, chamando uns aos outros enquanto o jogador caminha. Você conheceu `List<string>` para guardar itens e viu como ficam os methods `void` quando *você* os escreve, não só os chama. A aventura inteira tem cerca de noventa linhas de código, divididas em quatro peças com nome que cada uma faz uma coisa.

**Conceitos que você já sabe nomear:**

- **`List<T>`** — coleção genérica ordenada
- **`Add` e `Contains`** — adicionar item, verificar se está na lista
- **definição de method** — `void Name() { ... }` que você escreveu
- **organisation** — dividir o código para que a estrutura espelhe o problema
- **retorno `void`** — method que age, não devolve nada

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a grande ideia pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar:

1. Escreva o seu próprio method chamado `Greet` que não recebe nada e não devolve nada (um method `void`).
2. Dentro dele, imprima uma mensagem curta.
3. Acima do method, chame-o duas vezes.
4. Rode e verifique que a mensagem é impressa duas vezes.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
Greet();
Greet();

void Greet()
{
    Console.WriteLine("Welcome, traveller.");
}
```

- `void` significa que o method não devolve nada — ele só faz algo.
- Você chama um method escrevendo o nome dele mais `()`.
- As duas chamadas no topo rodam antes do method abaixo delas. O C# deixa você escrever o method mais embaixo.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote as suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 0.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare os dois arquivos, mensagem do commit `Module 0.3 done`, Sync.
4. **Poste em `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI se você precisar relembrar. Traga as respostas do quiz das quais você estiver menos certo para o próximo sync semanal.

## Próximo

O Módulo 0.4 é um dia para deixar as coisas bonitas. Você escolhe o seu favorito dos três programas desta semana, melhora ele e o finaliza. Fim da Spark Week.
