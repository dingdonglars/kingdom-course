# Módulo 2.1 — File I/O

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Até agora, o seu reino some no momento em que você fecha o programa. Hoje a gente muda isso. No fim deste módulo, o seu reino vai se escrever num arquivo de texto no disco. Amanhã a gente transforma isso em JSON. Depois, SQLite. Mas começamos com a coisa mais simples: abrir um arquivo, escrever um texto, fechar.

Guardar o trabalho para ele durar depois que o programa termina se chama *persistência*. Na primeira vez que você vir um arquivo aparecer, fechar o programa e descobrir que o arquivo ainda está lá, persistência vai começar a fazer sentido.

Aqui está a ideia inteira desta fase em uma figura:

```text
   NA MEMÓRIA (enquanto roda)             NO DISCO (um arquivo)
   -------------------------             -------------------
   seu objeto Kingdom          salva      save.txt
   gold 100, wood 50, ...      ------->   "Eldoria 100 50 20 30"
                                          |
   some no momento em que       carrega   | ainda está lá depois que você fecha,
   o programa fecha           <-------     reinicia e volta amanhã
```

Tudo que você fez até agora vive só na coluna da esquerda — some quando o programa para. A Fase 2 é sobre as setas: escrever o reino *para fora* no disco e lê-lo de volta *para dentro* na próxima vez. O arquivo de hoje é a versão mais simples disso; JSON, depois SQLite, depois um banco de dados de verdade são as mesmas duas setas feitas de um jeito melhor.

Pelo caminho você vai aprender o jeito exato que o Windows lida com caminhos de arquivo e fins de linha. Depois de hoje você vai saber a diferença entre `C:\foo` e `C:/foo`.

> **Words to watch**
>
> - **path** — o endereço de um arquivo no disco: `C:\code\kingdom\save.txt`, ou escrito de forma portável com `Path.Combine(...)`
> - **absolute vs relative** — `C:\foo\bar.txt` é absolute; `bar.txt` é relative (relativo à pasta onde o programa está rodando)
> - **encoding** — como texto vira bytes. UTF-8 é a resposta para quase tudo escrito nesta década.
> - **`File.WriteAllText`** / **`File.ReadAllText`** — as duas formas mais simples de colocar uma string no disco e buscá-la de volta.

---

## Abertura da fase — branch `phase-2`

Antes de qualquer código (o porquê está no Módulo 1.1):

```powershell
cd C:\code\kingdom
git switch -c phase-2
```

Todo commit desta fase vai na branch `phase-2`. No Módulo 2.11 (o marco M3), você vai abrir um PR para mesclá-la de volta na `main`.

---

## Por que arquivo primeiro

Arquivos são o jeito mais simples de salvar: abrir, escrever uma string, fechar. Nada é mais rápido de adicionar e nada é mais fácil de conferir — você pode abrir o arquivo no Bloco de Notas e ler o que está dentro. JSON (Módulo 2.2) e SQLite (Módulo 2.4) constroem sobre essa mesma ideia básica: escrever alguns bytes no disco. Então a gente começa com essa ideia básica.

Este módulo *não muda o engine ainda.* Todo o trabalho com arquivo acontece em `Kingdom.Console/Program.cs`. O engine ainda não sabe que o disco existe. Amanhã a gente muda isso — mas só depois que você tiver visto como tudo se encaixa.

## Passo 1 — caminhos

No Windows, caminhos de arquivo usam barras invertidas. Mas em código C#, uma barra invertida dentro de uma string é um caractere especial. Então você tem duas opções: escrever *duas vezes*, ou colocar um `@` na frente da string (isso se chama string *verbatim*):

```csharp
string a = "C:\\code\\kingdom\\save.txt";       // duas barras
string b = @"C:\code\kingdom\save.txt";          // verbatim @"..." — o que você vai ver normalmente
```

Mas escrever o caminho completo na mão é arriscado — quebra fácil. Use `Path.Combine` para que seu código funcione em qualquer sistema operacional:

```csharp
using System.IO;

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
var savePath   = Path.Combine(saveFolder, "kingdom.txt");
```

`AppContext.BaseDirectory` é a pasta onde o programa está rodando. Combine isso com `"saves"` e você tem uma pasta bem do lado do programa. Funciona do mesmo jeito no Windows ou no Mac, e você sempre sabe onde ela está.

## Passo 2 — escreva, leia de volta

```csharp
using System.IO;
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;

IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddCitizen(new Citizen("Lyra"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(saveFolder);                       // não faz nada se já existe
var savePath = Path.Combine(saveFolder, "kingdom.txt");

// Monta um snapshot pequeno e legível por humanos
var lines = new List<string>
{
    $"Name: {kingdom.Name}",
    $"Day: {kingdom.Day}",
    $"Buildings: {kingdom.Buildings.Count}",
    $"Citizens: {kingdom.Citizens.Count}",
    $"Gold: {kingdom.Resources.Get(Resource.Gold)}"
};

File.WriteAllText(savePath, string.Join('\n', lines));
Console.WriteLine($"Saved to {savePath}");

// Agora lê de volta
var loaded = File.ReadAllText(savePath);
Console.WriteLine();
Console.WriteLine("=== File contents ===");
Console.WriteLine(loaded);
```

Rode:

```powershell
dotnet run --project Kingdom.Console
```

Abra `bin/Debug/net10.0/saves/kingdom.txt` no Bloco de Notas. Está lá — o seu texto no disco. Feche o programa. Abra de novo. O arquivo ainda está lá. É um passo pequeno, mas é uma ideia grande.

## Passo 3 — encoding (aprenda uma vez)

*Encoding* é como o texto vira bytes para ser guardado. `File.WriteAllText(path, text)` usa um encoding chamado UTF-8 *sem BOM* (byte-order mark) por padrão no .NET moderno. Essa é a escolha certa para quase tudo. Se você quiser dizer isso em voz alta no seu código:

```csharp
File.WriteAllText(savePath, contents, System.Text.Encoding.UTF8);
```

Se um arquivo abrir em outra ferramenta com um `` estranho no começo, o encoding é a causa — as duas ferramentas discordam sobre como ler os bytes. Sempre use UTF-8, a não ser que você esteja trabalhando com um sistema antigo que precisa de outra coisa.

## Passo 4 — faça a viagem de ida e volta do que você escreveu

Aqui está um teste real: escreva algo, leia de volta, e confira que os dois são iguais.

`tests/Kingdom.Engine.Tests/FileIOTests.cs` (novo):

```csharp
namespace Kingdom.Engine.Tests;

public class FileIOTests
{
    [Fact]
    public void WriteAllText_ThenReadAllText_RoundtripsExactly()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kingdom-test-{Guid.NewGuid():N}.txt");
        var original = "Line one\nLine two\nLine three";
        try
        {
            File.WriteAllText(path, original);
            var roundtripped = File.ReadAllText(path);
            roundtripped.ShouldBe(original);
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Path_Combine_HandlesTrailingSeparators()
    {
        // Path.Combine é tolerante com barras no final
        Path.Combine("a", "b").ShouldBe("a" + Path.DirectorySeparatorChar + "b");
        Path.Combine("a/", "b").ShouldBe("a/b");
    }

    [Fact]
    public void Directory_CreateDirectory_IsIdempotent()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"kingdom-test-{Guid.NewGuid():N}");
        try
        {
            Directory.CreateDirectory(dir);
            Directory.CreateDirectory(dir);   // a segunda chamada não joga erro
            Directory.Exists(dir).ShouldBeTrue();
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir);
        }
    }
}
```

Três testes. Os blocos `try / finally` apagam os arquivos temporários no final — um teste nunca deve deixar arquivos no disco. Rode:

```powershell
dotnet test
```

Espere `Passed: 38` (35 + 3).

## Mexa um pouco

Tente salvar um arquivo diferente a cada dia: `kingdom-day-{kingdom.Day}.txt`. Agora a sua pasta `saves/` vai enchendo de arquivos. Olhe para ela depois de trinta dias — você tem um pequeno registro de cada dia.

Tente `File.AppendAllText(path, text)`. Ele adiciona ao final de um arquivo em vez de substituir o que está lá. Use para escrever cada evento em `events.log` e veja o arquivo crescer.

Tente escrever um milhão de linhas: `File.WriteAllText(path, string.Join('\n', Enumerable.Range(1, 1_000_000)))`. Abra o arquivo no Bloco de Notas. O Bloco de Notas vai ser lento. É o tamanho de arquivo onde um banco de dados começa a fazer mais sentido do que um arquivo de texto.

Apague a linha `Directory.CreateDirectory(saveFolder)`, ou vire ela num comentário. Rode de novo. Você vai receber um erro `DirectoryNotFoundException`. É por isso que a gente cria a pasta antes, se ela ainda não existir.

## O que você acabou de fazer

O seu reino passou de um programa que só imprime para um programa que *salva*. Você escreveu um pequeno snapshot num arquivo de verdade no disco, leu de volta, e conferiu a viagem de ida e volta com três testes — o seu total agora é 38 passando. Você também aprendeu dois fatos sobre o Windows que vão te poupar horas mais tarde: caminhos usam barras invertidas (que precisam de dois caracteres ou de `@"..."`), e o .NET moderno escreve UTF-8 sem BOM, que é o que você quer toda vez. A parte interessante é o que você *não* mudou: o engine não tem nenhuma edição neste módulo. O disco é trabalho do shell; o engine não sabe disso.

**Conceitos que você já sabe nomear:**

- **path** — o endereço de um arquivo; construa com `Path.Combine`
- **absolute vs relative** — endereço completo vs *"relativo a onde estou rodando"*
- **`File.WriteAllText` / `ReadAllText`** — a persistência mais simples
- **`Directory.CreateDirectory`** — cria uma pasta, sem problema se já existe
- **engine vs shell** — o disco fica no shell; o engine continua limpo

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a única ideia grande pegou: escrever uma string num arquivo, depois lê-la de volta. Ninguém corrige isso — é só para você. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar:

1. Construa um caminho de salvamento com `Path.Combine`.
2. Crie a pasta.
3. Escreva o texto `"Hello kingdom"` num arquivo.
4. Leia o mesmo arquivo de volta e imprima o que você leu.
5. Rode — o texto que você imprime deve ser igual ao que você escreveu.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
using System.IO;

var folder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(folder);
var path = Path.Combine(folder, "hello.txt");

File.WriteAllText(path, "Hello kingdom");

var back = File.ReadAllText(path);
Console.WriteLine(back);          // Hello kingdom
```

Se o texto que você leu for igual ao que você escreveu, você entendeu. Se esqueceu o `Directory.CreateDirectory`, vai receber um `DirectoryNotFoundException` — essa é exatamente a lição da seção Mexa um pouco, aparecendo na hora certa.

</details>

## Movimento git da semana — `.gitignore`

Você começou a escrever arquivos no disco neste módulo. Alguns arquivos pertencem ao git (o seu código, o `.csproj`, os arquivos de teste). Outros não (os arquivos de build em `bin/` e `obj/`, chaves secretas, arquivos `.env`, e arquivos extras que o sistema operacional cria, como `.DS_Store`).

O seu repositório `kingdom` já tem um `.gitignore` do kit do dia 1 — abra ele na raiz do repositório. Cada linha é um padrão de arquivos que o git deve *ignorar*. Quando você cria um arquivo novo que bate com um desses padrões, o painel do Source Control pula ele. Ele nunca aparece em *Changes*.

Se você não conseguir entender por que o VS Code não está mostrando um arquivo que você esperava, confira o `.gitignore`. Na maioria das vezes, é isso.

> **Ou no terminal:** `git status --ignored` lista todo arquivo que o git está ignorando agora — útil quando você quer ter certeza.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.1 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 2.2 apresenta a **serialização JSON** — em vez de escrever cinco linhas de texto simples, a gente salva o reino inteiro como JSON e carrega de volta para um objeto `Kingdom` de verdade. É onde salvar se torna realmente útil.
