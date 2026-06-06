# Módulo 0.8 — Errors, Debugging, and M1

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje você faz o Inventory Tool *não travar* quando as coisas dão errado, e conhece o depurador do VS Code — a ferramenta mais importante para entender código que não está fazendo o que você espera. Depois você finaliza o M1. Fim de Foundations, fim da Fase 0.

> **Words to watch**
>
> - **exception** — um erro em tempo de execução; um problema que o programa encontrou enquanto rodava e não conseguiu lidar
> - **`try / catch`** — o padrão do C# para "tente isso; se der erro, faça isso em vez disso"
> - **`throw`** — a palavra-chave para lançar sua própria exception
> - **debugger** — uma ferramenta que pausa o seu programa em pontos escolhidos para que você possa ver o que está acontecendo
> - **breakpoint** — uma linha marcada onde o depurador pausa
> - **call stack** — a cadeia de methods que se chamaram uns aos outros para chegar onde você está agora

---

## Passo 1 — o que pode dar errado

Abra o Inventory Tool que você escreveu no Módulo 0.7. Olhe o caso `load`. Como está agora, o que acontece se `inventory.txt` existir mas estiver quebrado? Ou se uma linha disser `apple=banana` em vez de `apple=2`? O `int.TryParse` que usamos lida com números inválidos tranquilamente — esse é o motivo todo do TryParse existir: te dizer se funcionou em vez de travar. Mas outras coisas ainda podem dar errado.

O arquivo pode existir mas estar bloqueado por outro programa — isso dá uma `IOException`. O arquivo pode estar em um drive onde você não tem permissão de escrita — isso dá uma `UnauthorizedAccessException`. O usuário pode digitar `add` sem nada depois — agora a gente silenciosamente adiciona um item com nome vazio (`""`), o que é um bug silencioso. Bugs silenciosos são piores do que os barulhentos, porque ninguém os nota.

## Passo 2 — reforce a ferramenta

Substitua o seu `Program.cs` por esta versão mais forte. As mudanças estão marcadas com `// NEW`:

```csharp
// Inventory Tool — v2 (Module 0.8)
//
// Hardened with try/catch and input validation.

var inventory = new Dictionary<string, int>();
const string SaveFile = "inventory.txt";

Console.WriteLine("Inventory Tool. Type 'help' for commands.");

while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line is null) break;
    line = line.Trim();
    if (line.Length == 0) continue;

    var parts = line.Split(' ', 2);
    var cmd = parts[0].ToLower();
    var arg = parts.Length > 1 ? parts[1].Trim() : "";

    try   // NEW: wrap everything so a bug in one command doesn't kill the program
    {
        switch (cmd)
        {
            case "add":
                if (string.IsNullOrEmpty(arg))   // NEW: validate
                {
                    Console.WriteLine("Usage: add <item>");
                    break;
                }
                inventory[arg] = inventory.GetValueOrDefault(arg, 0) + 1;
                Console.WriteLine($"Added: {arg} (now have {inventory[arg]})");
                break;

            case "remove":
                if (string.IsNullOrEmpty(arg))   // NEW
                {
                    Console.WriteLine("Usage: remove <item>");
                    break;
                }
                if (inventory.ContainsKey(arg) && inventory[arg] > 0)
                {
                    inventory[arg]--;
                    if (inventory[arg] == 0) inventory.Remove(arg);
                    Console.WriteLine($"Removed: {arg}");
                }
                else
                {
                    Console.WriteLine($"Not found: {arg}");
                }
                break;

            case "find":
                if (string.IsNullOrEmpty(arg))   // NEW
                {
                    Console.WriteLine("Usage: find <item>");
                    break;
                }
                if (inventory.ContainsKey(arg))
                    Console.WriteLine($"Found: {arg} (count: {inventory[arg]})");
                else
                    Console.WriteLine($"Not found: {arg}");
                break;

            case "list":
                if (inventory.Count == 0)
                {
                    Console.WriteLine("Empty.");
                }
                else
                {
                    Console.WriteLine($"You have:");
                    foreach (var (item, count) in inventory.OrderBy(kvp => kvp.Key))
                        Console.WriteLine($"  - {item} x{count}");
                }
                break;

            case "save":
                var savedLines = inventory.Select(kvp => $"{kvp.Key}={kvp.Value}");
                File.WriteAllText(SaveFile, string.Join("\n", savedLines));
                Console.WriteLine($"Saved {inventory.Count} item(s) to {SaveFile}.");
                break;

            case "load":
                if (!File.Exists(SaveFile))
                {
                    Console.WriteLine($"No save file at {SaveFile}.");
                    break;
                }
                inventory.Clear();
                int loaded = 0, skipped = 0;
                foreach (var l in File.ReadAllLines(SaveFile))
                {
                    var kv = l.Split('=', 2);
                    if (kv.Length == 2 && int.TryParse(kv[1], out var n) && n > 0)
                    {
                        inventory[kv[0]] = n;
                        loaded++;
                    }
                    else
                    {
                        skipped++;     // NEW: count bad lines instead of silently dropping
                    }
                }
                Console.WriteLine($"Loaded {loaded} item(s) from {SaveFile}." + (skipped > 0 ? $" Skipped {skipped} bad line(s)." : ""));
                break;

            case "help":
                Console.WriteLine("Commands: add <item>, remove <item>, find <item>, list, save, load, quit");
                break;

            case "quit":
            case "exit":
                Console.WriteLine("Bye.");
                return;

            default:
                Console.WriteLine($"Unknown command: {cmd}. Type 'help'.");
                break;
        }
    }
    catch (IOException ex)   // NEW: file is locked, drive full, etc.
    {
        Console.WriteLine($"File problem: {ex.Message}");
    }
    catch (UnauthorizedAccessException ex)   // NEW: permission denied
    {
        Console.WriteLine($"Permission problem: {ex.Message}");
    }
    catch (Exception ex)   // NEW: catch-all for anything we didn't anticipate
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}
```

O bloco `try` vai em volta de todo o switch. Se qualquer caso lançar um erro, o programa pula para o `catch` que combina com o tipo do erro. O C# verifica os catches de cima para baixo, e o primeiro que combinar é o que roda. A gente captura `IOException` e `UnauthorizedAccessException` pelo nome, porque podemos explicar esses problemas ao usuário em palavras simples. O `catch (Exception)` final é o para-tudo — ele trata qualquer coisa que não previmos, para que uma entrada ruim nunca quebre o programa inteiro.

Rode de novo. Tente `add` sem nada depois. Tente `remove` sem nada depois. Tente salvar em uma pasta para a qual você não tem permissão de escrita (mude `SaveFile` por um momento para `"C:\\Windows\\inventory.txt"` se quiser ver um catch rodar). O programa não trava mais.

## Passo 3 — o depurador

> **Deixe à mão:** o `using-the-debugger.md` na raiz do curso é uma referência de uma página para cada tecla, painel e truque do depurador — breakpoints, as teclas de step, Watch, Call Stack, e mudar um valor enquanto pausado. Esta seção te dá o começo; aquela página é a que você volta a consultar.

Abra `Program.cs` no VS Code. Clique na faixa estreita vazia logo à esquerda dos números de linha, ao lado da linha `inventory[arg] = inventory.GetValueOrDefault(arg, 0) + 1;`. Um ponto vermelho aparece. Isso é um **breakpoint**.

Primeiro, uma verificação rápida: a janela do VS Code deve estar aberta na pasta **`InventoryTool`** — a barra de título e a árvore de arquivos devem dizer `InventoryTool`, não o repositório `kingdom` inteiro. Se não estiverem, use *File → Open Folder…* → a pasta `InventoryTool`. (Um programa, uma janela — caso contrário, o F5 não saberá qual programa iniciar. O guia curto é `running-your-project.md`.)

Agora aperte `F5`, ou use *Run → Start Debugging*. O VS Code pode pedir para instalar o depurador do C# Dev Kit se você ainda não tiver; diga sim. O programa roda, depois pausa *no* seu breakpoint quando você digita `add apple` — bem antes dessa linha rodar.

O painel à esquerda mostra duas coisas que vale saber. **Variables** lista o valor atual de `arg` (`"apple"`), `inventory` (um dicionário vazio agora), `cmd` (`"add"`), e assim por diante. **Call stack** mostra quais methods chamaram quais para chegar a este ponto. Por enquanto é só `Program.<Main>$`, o lugar onde seu programa começa. Quando você estiver chamando seus próprios methods, você verá a cadeia completa aqui.

Passe o mouse sobre `arg` no código. O valor aparece. Aperte `F10` (*step over*) — a linha roda e você vai para a próxima. `inventory` agora contém `{"apple": 1}`. Você acabou de ver o dicionário mudar. Aperte `F5` para continuar; o programa imprime `Added: apple (now have 1)` e espera o próximo comando.

## Mexa com o depurador

Defina um breakpoint dentro do caso `load`. Rode o programa. Digite `load` e percorra o loop com `F10`, observando `kv`, `loaded` e `skipped` mudarem enquanto cada linha é lida.

Defina um breakpoint dentro do bloco `catch (IOException)`. Cause uma `IOException` de propósito mantendo `inventory.txt` aberto em outro programa (o Bloco de Notas funciona) enquanto você chama `save`.

Clique com o botão direito em uma variável no editor e escolha *Add to Watch*. A variável agora aparece no painel *Watch*, e o valor é atualizado enquanto você percorre o código.

## Dê nome às coisas

**Exception.** Um erro em tempo de execução. O C# armazena como um valor do tipo `Exception`, ou de um dos seus tipos mais específicos como `IOException`, `FileNotFoundException` ou `ArgumentException`.

**`try / catch`.** Coloque código arriscado em um bloco `try`. Se algo lançar um erro, um bloco `catch (TypeOfException)` roda em vez disso. O programa então continua depois do `catch`.

**A ordem dos catches importa.** O C# verifica os catches de cima para baixo, e o primeiro que combinar roda. Coloque os tipos específicos primeiro e `catch (Exception)` por último como para-tudo.

**`finally`** (não usamos hoje, mas você vai ver). Um bloco que sempre roda depois do `try`, seja com erro ou não. Útil para fechar arquivos, liberar travas e outras limpezas que têm que acontecer de qualquer jeito.

**Breakpoint.** Uma linha marcada onde o depurador pausa, *antes* de rodar essa linha.

**Step over (`F10`)** roda a linha atual e vai para a próxima linha no mesmo method. **Step into (`F11`)** entra dentro de uma chamada de method para que você possa percorrer esse method linha por linha.

**Call stack.** Quando um method chama outro, que chama outro, o depurador mostra essa cadeia. É útil para ver *como o programa chegou a esse ponto* — muitas vezes mais útil do que só saber onde o erro aconteceu.

## M1 — Inventory Tool, entregue

Agora você tem o programa para entregar no M1. Certifique-se de que o seu repositório tem:

- Uma pasta `InventoryTool/` no topo, com o `Program.cs` v2 e um `.csproj`
- Um `README.md` no topo descrevendo a ferramenta (use as quatro seções do Módulo 0.4)
- Uma entrada em `journal/wins.md` para o M1 (os passos do marco abaixo)

Rode o desafio M1:

```powershell
dotnet test path\to\challenges\M1\M1.Tests.csproj
```

Se os testes passarem (verde), o M1 está pronto.

## Faça commit do seu trabalho

No painel Source Control do VS Code (`Ctrl + Shift + G G`):

1. Prepare as suas mudanças — passe o mouse em **Changes** e clique em `+`.
2. Mensagem do commit: *"M1: Inventory Tool v2 — hardened + debugged"*.
3. Clique no **check azul** para fazer o commit.
4. Clique em **Sync Changes** para enviar para o GitHub.

> **Ou no terminal:**
>
> ```powershell
> git add .
> git commit -m "M1: Inventory Tool v2 — hardened + debugged"
> git push
> ```

## O que você acabou de fazer

Você tornou um programa de verdade mais forte. O Inventory Tool passou de "trava em entradas estranhas" para "diz ao usuário o que deu errado e continua rodando." Você conheceu `try/catch`, as três regras para a ordem dos catches e a família de tipos `Exception`. Você conheceu o depurador do VS Code — breakpoints, step over, o painel de variáveis, o call stack. O contador de "linhas ruins puladas" em `load` é uma coisa pequena, mas é a diferença entre uma ferramenta em que você confia e uma que não. Oito módulos da Fase 0 estão para trás. Você tem dois programas finalizados no seu GitHub (o kit M0 e o M1 Inventory Tool), e as partes básicas do C# agora têm nomes.

**Conceitos que você já sabe nomear:**

- **exception** — erro em tempo de execução que você pode capturar
- **`try / catch`** — bloco arriscado mais tratador
- **ordem dos catches** — específico primeiro, genérico por último
- **breakpoint** — o depurador pausa antes desta linha
- **step over vs step into** — pular uma chamada, vs segui-la por dentro
- **call stack** — cadeia de quem chamou quem

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que as duas grandes ideias pegaram. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *ainda não* pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

### 1. A forma do `try / catch`, de memória

Abra um arquivo novo e vazio. Sem olhar:

1. Escreva um bloco `try` com **dois** blocos `catch` depois — um capturando `IOException`, e um final capturando `Exception` simples.
2. Coloque uma linha dentro do `try` que lança de propósito — `throw new IOException("test");` serve.
3. Coloque um `Console.WriteLine` dentro de cada catch para que você possa ver qual roda. Rode.

Depois troque a ordem: coloque `catch (Exception)` *primeiro*. O que o compilador diz? Esse erro é o motivo todo de "específico primeiro, genérico por último."

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
try
{
    throw new IOException("test");
}
catch (IOException ex)
{
    Console.WriteLine($"IO: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Other: {ex.Message}");
}
```

Com `catch (Exception)` primeiro, o build falha: *"A previous catch clause already catches all exceptions."* O catch genérico engolhiria tudo antes do específico ter vez — então o C# não deixa você fazer isso.

</details>

### 2. Use o depurador de memória

Abra o seu Inventory Tool. Sem rolar de volta para o Passo 3, de memória:

1. Defina um breakpoint na linha dentro de `add` que muda o dicionário.
2. Inicie o debug e digite `add sword`.
3. Quando pausar, encontre o valor de `arg` no painel Variables.
4. Dê step over em uma linha e observe o `inventory` mudar. Depois deixe terminar.

<details><summary>Travou? Abra aqui para conferir.</summary>

- Breakpoint: clique na faixa estreita à esquerda do número de linha — um ponto vermelho aparece.
- Inicie o debug: `F5`.
- Pausa *antes* da linha do breakpoint rodar.
- Painel Variables (esquerda): `arg` é `"sword"`.
- Step over: `F10` — `inventory` agora mostra `{"sword": 1}`.
- Continue: `F5`.

</details>

## Fechamento do M1 — os passos do marco

Você acabou de terminar o M1. Aqui estão os passos para finalizá-lo.

1. **`journal/wins.md`** — abra-o no seu repositório e escreva um parágrafo sobre o M1 com as suas palavras. O que o Inventory Tool faz, o que foi mais difícil do que você esperava e o que você confia nele agora que não confiava na v1.
2. **Post no Slack `#wins`** — cole o link do seu repositório mais uma captura de tela da ferramenta rodando. Adicione uma legenda de uma linha como *"M1 done — Inventory Tool v2."*
3. **Linha de antes e depois** — *"Seis semanas atrás eu nunca tinha escrito uma linha de código. Hoje eu terminei uma ferramenta que vou usar de verdade."* Diga em voz alta.
4. **Marque o marco.** Este é só pelo terminal — o painel Source Control não tem botão para tags:

   ```powershell
   git tag m1-inventory-tool-complete
   git push origin m1-inventory-tool-complete
   ```

Depois descanse o resto do dia. Você mereceu.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote as suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 0.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare os dois arquivos, mensagem do commit `Module 0.8 done`, Sync.
4. **Poste em `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI se você precisar relembrar. Traga as respostas do quiz das quais você estiver menos certo para o próximo sync semanal.

## Próximo

A Fase 1 começa o projeto principal — o próprio Kingdom. Você vai conhecer as suas primeiras **classes** (Módulo 1.1), depois dividir o código em um *engine* e um *shell* (1.2 — a lição que dá nome ao resto do curso). Depois disso, você escreve os seus primeiros **testes**.
