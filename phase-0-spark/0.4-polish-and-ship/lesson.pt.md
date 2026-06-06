# Módulo 0.4 — Polish + Ship + README Anatomy

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje é o fim da Spark Week. Você deixa os seus programas *bonitos* — arte ASCII, texto colorido e um arquivo salvo que o programa lê de volta na próxima vez que rodar. Você escreve o README que reúne tudo o que foi feito em M0. E finaliza: commit, push, post em `#wins`. No final de hoje, você tem quatro programas funcionando no seu repositório com um README de verdade, e está pronto para Foundations.

> **Words to watch**
>
> - **ASCII art** — imagens feitas de caracteres de texto (ainda cool em 2026)
> - **`Console.ForegroundColor`** — a propriedade que controla a cor do texto impresso em seguida
> - **`File.WriteAllText`** — escreve uma string em um arquivo, criando-o se não existir
> - **`File.ReadAllText`** — lê o conteúdo de um arquivo de volta como string
> - **README** — o documento no topo de todo repositório que diz o que tem lá

---

## Passo 1 — crie um projeto de polish

Crie um novo projeto de console para isso. (Você também pode adicionar esse polish a um dos programas que já tem, se preferir.)

```powershell
cd ..
dotnet new console -n Polish
cd Polish
```

Abra `Program.cs` no VS Code. Substitua o conteúdo:

```csharp
// 1. ASCII art header
var art = @"
   _  _____ _   _  ____ ____   ___  __  __
  | |/ /_ _| \ | |/ ___|  _ \ / _ \|  \/  |
  | ' / | ||  \| | |  _| | | | | | | |\/| |
  | . \ | || |\  | |_| | |_| | |_| | |  | |
  |_|\_\___|_| \_|\____|____/ \___/|_|  |_|
";
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine(art);
Console.ResetColor();

// 2. Greet the player by name (with colour)
Console.Write("Your name, hero: ");
var name = Console.ReadLine();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine($"Welcome to your kingdom, {name.ToUpper()}.");
Console.ResetColor();

// 3. Save the player's name to a file (so we remember them next time)
File.WriteAllText("hero.txt", name);

// 4. On next run, read the file back and confirm
if (File.Exists("hero.txt"))
{
    var saved = File.ReadAllText("hero.txt");
    Console.WriteLine();
    Console.WriteLine($"(File saved. Next time the program runs, '{saved}' will be remembered.)");
}
```

A forma `@"..."` é uma **verbatim string**. Quebras de linha e barras invertidas são mantidas exatamente como você escreveu — que é exatamente o que você quer para arte ASCII. `Console.ForegroundColor` é uma *propriedade* que define a cor do texto impresso em seguida. `Console.ResetColor()` volta para a cor normal. Sempre redefina. Um programa que muda a cor e depois trava deixa o terminal do usuário preso nessa cor.

As duas linhas de `File` são a primeira vez que você escreve no disco. `File.WriteAllText("hero.txt", name)` cria um arquivo na pasta atual e escreve o que estiver em `name`. `File.ReadAllText("hero.txt")` o lê de volta. Vamos cobrir arquivos muito mais na Fase 2. Hoje é só uma primeira olhada.

Rode duas vezes:

```powershell
dotnet run
dotnet run
```

O arquivo `hero.txt` aparece na pasta do projeto. **Você escreveu no disco.**

## Ouça o editor

O editor te diz coisas enquanto você digita. Quando ele desenha uma linha colorida embaixo do seu código, ele está apontando para um ponto exato. Veja como ler isso.

Em `Program.cs`, encontre a linha `var name = Console.ReadLine();`. Mude `ReadLine` para `readLine` (r minúsculo — só essa pequena mudança, nada mais). Salve o arquivo.

Três coisas acontecem:

1. **Um sublinhado vermelho aparece em `readLine`.** Passe o mouse por cima. Uma caixa aparece: *"'Console' does not contain a definition for 'readLine'"*. O editor está dizendo que esse method não existe. O C# se importa com letras maiúsculas — `ReadLine` e `readLine` são dois nomes diferentes.

2. **Sem sugestões de código depois de `name.`** Vá para a linha seguinte e coloque o cursor logo depois de `name.` em `{name.ToUpper()}`. Normalmente um pequeno menu aparece com `.ToUpper()`, `.ToLower()`, `.Trim()` e mais. Agora ele sumiu ou está errado. Como a linha acima está quebrada, o editor não sabe o que é `name`, então não pode te ajudar na próxima linha.

3. **O build está quebrado.** Rode `dotnet run`. Ele se recusa. O mesmo erro da caixa de hover aparece no terminal.

Mude `readLine` de volta para `ReadLine`. Salve. O sublinhado desaparece. As sugestões de código voltam. `dotnet run` funciona.

**Vermelho significa: isso não vai compilar. Seu programa está quebrado até você consertar.** O compilador não vai rodar código com sublinhado vermelho. Nunca.

Agora para uma cor diferente. Adicione uma linha perto do topo de `Program.cs`, logo abaixo do bloco `var art = @"..."`:

```csharp
var debug = 5;
```

Salve. Um sublinhado **amarelo** aparece em `debug`. Passe o mouse: *"The variable 'debug' is assigned but its value is never used."* Isso é um aviso. O código compila. `dotnet run` funciona. Mas o compilador está apontando uma linha que não faz nada, o que provavelmente é um erro. Delete a linha e o amarelo vai embora.

**Amarelo significa: isso compila, mas acho que você cometeu um erro.** Sublinhados amarelos não impedem o programa de rodar. A maioria deles vale a pena ler, porque muitas vezes apontam bugs que ainda não te pegaram. Outras coisas que você verá em amarelo: código que nunca pode rodar, um valor que o compilador acha que pode estar vazio ou uma importação que você não usa.

(Também há sugestões em azul claro ou cinza — pequenas dicas de estilo, a menor prioridade. Pule-as por enquanto.)

**O que você acabou de aprender.**

O editor não espera nada de você. Ele não se importa se você está com pressa, se o seu código *parece* com o exemplo ou se você gostaria de ter terminado. Ele só sabe o que funciona e o que não funciona — e te diz, de graça, no momento em que você digita.

Quando as sugestões de código param de aparecer — quando você digita um ponto depois de uma variável e nada útil aparece — esse é o mesmo aviso, só em outra forma. O editor perdeu o controle do seu código. Algo antes está errado.

Três regras daqui para frente:

- **Sublinhado vermelho = pare e leia a caixa de hover.** Não continue digitando além dele. Ele não vai embora sozinho.
- **Sublinhado amarelo = leia antes de terminar.** Provavelmente não vai te bloquear, mas geralmente vale saber.
- **Sugestões de código sumindo onde você esperava = algo antes no arquivo está quebrado.** Role para cima, encontre o sublinhado, conserte-o.

O compilador é a única coisa neste curso inteiro que não espera nada de você. Ele só tem regras. Confie nele mais do que em você mesmo.

## Mexa um pouco

Faça a mensagem de boas-vindas ter uma cor diferente na *segunda* vez que o programa rodar (quando o arquivo já existe). Você vai precisar verificar `File.Exists("hero.txt")` *antes* de imprimir qualquer coisa.

Salve a pontuação do jogador do Number Guess. Faça o Polish salvar a pontuação e lê-la de volta na próxima execução, para que o programa se lembre da melhor pontuação até agora.

Adicione várias banners de arte ASCII e escolha uma aleatoriamente a cada execução. Você já sabe como: `Random.Next` e um array de strings.

Use `Console.BackgroundColor` para fazer uma banner com fundo colorido. Redefina ambas as cores quando terminar.

## Dê nome às coisas

**Arquivos (uma primeira olhada).** `File.WriteAllText("path", "content")` escreve uma string em um arquivo. `File.ReadAllText("path")` lê um arquivo de volta. `File.Exists("path")` verifica se um arquivo está lá. O path pode ser só um nome (na pasta atual) ou um path completo. A Fase 2 cobre arquivos adequadamente. Hoje é só o suficiente para salvar e recarregar uma coisa pequena.

**A classe `Console`.** `Console.WriteLine`, `Console.Write`, `Console.ReadLine`, `Console.ForegroundColor`, `Console.BackgroundColor`, `Console.ResetColor` e `Console.Clear` são todos methods e propriedades da classe `Console`. A página de documentação da Microsoft para `System.Console` lista tudo que ela pode fazer.

**Escrever um bom README.** O README é o arquivo que alguém lê quando abre o seu repositório pela primeira vez. As quatro seções que importam vêm a seguir.

## Anatomia do README — quatro seções que importam

Todo bom README de projeto tem essas quatro seções. Aprenda o formato de cor.

O **O que é** é uma frase. *"Um jogo de console pequeno onde você joga como um senhor de um reino, escrito em C#."* Se um estranho ler só essa linha, ele deve saber do que se trata.

O **Como rodar** são os comandos de verdade. *"Clone, depois `dotnet run` na pasta do projeto."* Não faça as pessoas adivinhar.

O **O que aprendi** é a parte onde você escreve com as suas palavras. O que te surpreendeu? O que foi difícil? Nem todo README tem isso, mas para um projeto de aprendizado é a parte mais valiosa.

O **O que vem depois** é o que você faria se continuasse. *"Adicionar mais quartos. Salvar pontuações máximas. Escrever uma v2 em JavaScript."* Mesmo que "depois" seja "nada", diga isso.

Só isso. Quatro seções. Cerca de 30 a 50 linhas no total. A maioria dos READMEs não é tão curta. Deveria ser.

## Marco M0 — *The Joke Toolbox*

Você agora tem:

- `RoastOMatic/` (e v2 do Módulo 0.1)
- `NumberGuess/`
- `TinyAdventure/`
- `Polish/`

No **topo do seu repositório** (ao lado de todas as quatro pastas), crie um `README.md` e escreva-o você mesmo usando as quatro seções acima. Dê a cada programa duas frases: uma *o que é*, uma *como rodar*. Depois adicione uma seção "o que aprendi" para os quatro juntos (um parágrafo). Depois "o que vem depois" — o que você adicionaria em seguida?

## Faça commit do seu trabalho

No painel Source Control do VS Code (`Ctrl + Shift + G G`):

1. Prepare as suas mudanças — passe o mouse em **Changes** e clique em `+`.
2. Mensagem do commit: *"M0: The Joke Toolbox - four toys + README"*.
3. Clique no **check azul** para fazer o commit.
4. Clique em **Sync Changes** para enviar para o GitHub.

> **Ou no terminal:**
>
> ```powershell
> git add .
> git commit -m "M0: The Joke Toolbox - four toys + README"
> git push
> ```

## O que você acabou de fazer

Você terminou a Spark Week. Quatro programas funcionando em um repositório, cada um pequeno mas completo — zoações aleatórias, um jogo de adivinhação, uma aventura de texto e um programa de polish que lê e escreve um arquivo. Cinco ideias novas em oito módulos: variáveis, loops, condicionais, listas e seus próprios methods. O repositório tem um README de verdade. Você tem algo para mostrar. A maioria das pessoas que diz *"vou aprender a programar este ano"* ainda está assistindo tutoriais no YouTube na terceira semana. Você tem quatro programas na internet.

**Conceitos que você já sabe nomear:**

- **`File.WriteAllText` / `File.ReadAllText`** — salvar e carregar texto
- **cores do `Console`** — `ForegroundColor`, `ResetColor`
- **verbatim string** — `@"..."` mantém as quebras de linha literais
- **anatomia do README** — o que é, como rodar, o que aprendi, o que vem depois
- **o kit da Spark Week** — variáveis, loops, condicionais, listas, methods

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a grande ideia pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar:

1. Escreva `"dragon"` em um arquivo chamado `note.txt`.
2. Leia esse arquivo de volta para uma variável.
3. Imprima o que você leu.
4. Rode, e verifique que `note.txt` aparece na pasta.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
File.WriteAllText("note.txt", "dragon");
var saved = File.ReadAllText("note.txt");
Console.WriteLine($"The file says: {saved}");
```

- `File.WriteAllText("note.txt", "dragon")` cria o arquivo (na pasta atual) e escreve o texto nele.
- `File.ReadAllText("note.txt")` lê o arquivo inteiro de volta como string.
- O arquivo fica no disco depois que o programa termina, então a próxima execução poderia lê-lo de novo.

</details>

## Fechamento do M0 — os passos do marco

Você acabou de terminar o M0. Aqui estão os passos para finalizá-lo.

1. **`journal/wins.md`** — abra-o no seu repositório e escreva um parágrafo sobre o M0 com as suas palavras. O que tem no The Joke Toolbox, o que foi mais difícil e o que te surpreendeu.
2. **Post no Slack `#wins`** — cole o link do seu repositório mais uma captura de tela de um programa rodando. Adicione uma legenda de uma linha como *"M0 done — The Joke Toolbox."*
3. **Linha de antes e depois** — *"Quatro semanas atrás eu nunca tinha escrito código. Hoje eu terminei quatro programas."* Diga em voz alta. E queira dizer.
4. **Marque o marco.** Este é só pelo terminal — o painel Source Control não tem botão para tags:

   ```powershell
   git tag m0-spark-week-complete
   git push origin m0-spark-week-complete
   ```

Depois descanse o resto do dia. Você mereceu.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote as suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 0.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare os dois arquivos, mensagem do commit `Module 0.4 done`, Sync.
4. **Poste em `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI se você precisar relembrar. Traga as respostas do quiz das quais você estiver menos certo para o próximo sync semanal.

## Próximo

Foundations começa na próxima semana. A gente finalmente dá nomes às coisas que você tem usado o mês todo — tipos, methods, coleções e erros. Depois de Foundations, você vai saber C# o suficiente para começar a construir o reino de verdade.
