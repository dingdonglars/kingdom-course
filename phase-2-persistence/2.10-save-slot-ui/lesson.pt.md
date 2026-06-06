# Módulo 2.10 — UI de Save Slots (Console Interativo)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O `Program.cs` de demo faz as coisas certas, mas é só um script — *abra o programa, ele roda uma vez, acabou*. Jogos de verdade continuam num loop. Hoje a gente coloca as operações CRUD do Módulo 2.9 atrás de um menu interativo — *"1. Novo, 2. Carregar, 3. Apagar, 4. Sair"* — e finalmente tem algo que dá para jogar.

> **Words to watch**
>
> - **menu loop** — imprime opções, lê entrada, age, repete
> - **`Console.ReadLine`** — lê uma linha de entrada do usuário
> - **input validation** — lidar com o momento em que o usuário digita `"banana"` em vez de um número
> - **instrução `switch`** (vs expressão) — útil quando cada ramo *faz algo* em vez de retornar um valor

---

## Por que um loop de verdade importa

Um `Program.cs` de script é bom para testar o seu código. Mas um *usuário* — mesmo que esse usuário seja só você — precisa *interagir* com ele. O loop de hoje é o seu primeiro shell interativo. Você vai ver o mesmo padrão em todo nível:

- Console: `while (true) { print menu; read input; act; }`
- Web API: `while (true) { receive request; route; act; respond; }`
- Browser: `eventListener('click', () => act())` (movido por eventos, mas a mesma ideia)

A forma muda, mas a ideia central fica a mesma.

## Delta starter

- **NOVO:** `Kingdom.Console/SaveSlotUI.cs` — o menu + handlers
- **MODIFICADO:** `Kingdom.Console/Program.cs` — substitui a metade de baixo da demo por uma única chamada `SaveSlotUI.Run(...)`
- **NOVO:** `tests/Kingdom.Persistence.Tests/SaveSlotUITests.cs` — usa `Console.SetIn` / `Console.SetOut` para conduzir a UI num teste

## Passo 1 — `SaveSlotUI`

`Kingdom.Console/SaveSlotUI.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;

namespace Kingdom.Console;

public static class SaveSlotUI
{
    public static void Run(KingdomEfStore store, IRandom rng, IClock clock)
    {
        store.EnsureCreated();

        while (true)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("=== Kingdom — Save Slots ===");
            System.Console.WriteLine("  1. New kingdom");
            System.Console.WriteLine("  2. Load existing");
            System.Console.WriteLine("  3. Delete a slot");
            System.Console.WriteLine("  4. Quit");
            System.Console.Write("> ");

            var line = System.Console.ReadLine();
            if (line is null) return;   // EOF (entrada redirecionada e acabou)

            switch (line.Trim())
            {
                case "1":  NewKingdom(store, rng, clock); break;
                case "2":  LoadKingdom(store, rng, clock); break;
                case "3":  DeleteSlot(store); break;
                case "4":  return;
                default:   System.Console.WriteLine("Pick 1, 2, 3, or 4."); break;
            }
        }
    }

    private static void NewKingdom(KingdomEfStore store, IRandom rng, IClock clock)
    {
        System.Console.Write("Name: ");
        var name = (System.Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrEmpty(name)) { System.Console.WriteLine("Cancelled."); return; }

        var k = new global::Kingdom.Engine.Kingdom(name, rng, clock);
        k.AddBuilding(new Farm("Main Farm"));
        k.AddCitizen(new Citizen("Lyra"));
        var id = store.Save(k);
        System.Console.WriteLine($"Created '{name}' as slot #{id}.");
        PlayLoop(store, id, k);
    }

    private static void LoadKingdom(KingdomEfStore store, IRandom rng, IClock clock)
    {
        var slots = store.ListSlots();
        if (slots.Count == 0) { System.Console.WriteLine("No saves yet."); return; }
        ShowSlots(slots);

        System.Console.Write("Slot id (or blank to cancel): ");
        var raw = (System.Console.ReadLine() ?? "").Trim();
        if (!int.TryParse(raw, out var id)) { System.Console.WriteLine("Not a number."); return; }
        if (slots.All(s => s.Id != id)) { System.Console.WriteLine($"No slot with id {id}."); return; }

        var k = store.Load(id, rng, clock);
        System.Console.WriteLine($"Loaded #{id} '{k.Name}' at day {k.Day}.");
        PlayLoop(store, id, k);
    }

    private static void DeleteSlot(KingdomEfStore store)
    {
        var slots = store.ListSlots();
        if (slots.Count == 0) { System.Console.WriteLine("No saves to delete."); return; }
        ShowSlots(slots);

        System.Console.Write("Slot id to delete: ");
        var raw = (System.Console.ReadLine() ?? "").Trim();
        if (!int.TryParse(raw, out var id)) { System.Console.WriteLine("Not a number."); return; }
        store.Delete(id);
        System.Console.WriteLine($"Deleted #{id}.");
    }

    private static void PlayLoop(KingdomEfStore store, int id, Kingdom.Engine.Kingdom k)
    {
        while (true)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"--- {k.Name} day {k.Day} ---");
            System.Console.WriteLine("  a. Advance 1 day   d. Advance 10 days   s. Save & exit slot");
            System.Console.Write("> ");
            var c = (System.Console.ReadLine() ?? "").Trim();
            switch (c)
            {
                case "a": k.AdvanceDay(); break;
                case "d": for (int i = 0; i < 10; i++) k.AdvanceDay(); break;
                case "s": store.Update(id, k); System.Console.WriteLine($"Saved #{id} at day {k.Day}."); return;
                default:  System.Console.WriteLine("Pick a, d, or s."); break;
            }
        }
    }

    private static void ShowSlots(IReadOnlyList<KingdomSlotInfo> slots)
    {
        System.Console.WriteLine("Saved kingdoms:");
        foreach (var s in slots)
            System.Console.WriteLine($"  #{s.Id,-3} {s.Name,-20} day {s.Day}");
    }
}
```

Leia isso com atenção. Cada método faz uma coisa. O `switch` em `Run` é a parte que decide o que acontece a seguir, com base no que o usuário digitou. Cada handler então chama o store e volta.

Por que `System.Console.WriteLine` em vez de só `Console.WriteLine`? Nossa classe vive em `namespace Kingdom.Console`, e esse nome conflita com o tipo `System.Console`. Escrever o `System.Console` completo deixa claro qual deles a gente quer dizer.

## Passo 2 — `Program.cs` vira uma única linha

Substitua `Program.cs` por:

```csharp
using Kingdom.Console;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(saveFolder);
var dbPath = Path.Combine(saveFolder, "kingdoms-ef.db");

IRandom rng = new SystemRandom();
IClock clock = new SystemClock();

var store = new KingdomEfStore(dbPath);
SaveSlotUI.Run(store, rng, clock);
```

Esse é o shell todo. Lê entrada, decide o que fazer, salva. O engine e a camada de persistência fazem o trabalho real.

Compile e rode:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Agora você está no primeiro loop de jogo real do seu reino. Crie um reino, avance alguns dias, salve, saia, comece de novo e carregue. O reino lembra.

## Passo 3 — testando uma UI interativa

Para um teste, precisamos *fornecer* a entrada do usuário com antecedência. O .NET torna isso fácil: você redireciona `Console.In` e `Console.Out` para o teste ler de uma string e escrever para uma string.

`tests/Kingdom.Persistence.Tests/SaveSlotUITests.cs`:

```csharp
using Kingdom.Console;
using Kingdom.Engine;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SaveSlotUITests
{
    [Fact]
    public void Run_Quit_ExitsImmediately()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ui-{Guid.NewGuid():N}.db");
        try
        {
            using var input = new StringReader("4\n");           // sair
            using var output = new StringWriter();
            System.Console.SetIn(input);
            System.Console.SetOut(output);

            SaveSlotUI.Run(new KingdomEfStore(path), new SystemRandom(0), new SystemClock());

            output.ToString().ShouldContain("Quit");
        }
        finally
        {
            ResetConsole();
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Run_NewKingdom_ThenSaveExit_PersistsTheSlot()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ui-{Guid.NewGuid():N}.db");
        try
        {
            // 1 = novo, "Test" = nome, a = avançar, s = salvar+sair, 4 = sair
            using var input = new StringReader("1\nTest\na\ns\n4\n");
            using var output = new StringWriter();
            System.Console.SetIn(input);
            System.Console.SetOut(output);

            var store = new KingdomEfStore(path);
            SaveSlotUI.Run(store, new SystemRandom(0), new SystemClock());

            store.ListSlots().Count.ShouldBe(1);
            store.ListSlots()[0].Name.ShouldBe("Test");
            store.ListSlots()[0].Day.ShouldBe(2);   // começou no 1, avançou uma vez
        }
        finally
        {
            ResetConsole();
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Run_BadMenuPick_ShowsHelp_AndContinues()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ui-{Guid.NewGuid():N}.db");
        try
        {
            using var input = new StringReader("banana\n4\n");
            using var output = new StringWriter();
            System.Console.SetIn(input);
            System.Console.SetOut(output);

            SaveSlotUI.Run(new KingdomEfStore(path), new SystemRandom(0), new SystemClock());

            output.ToString().ShouldContain("Pick 1, 2, 3, or 4");
        }
        finally
        {
            ResetConsole();
            if (File.Exists(path)) File.Delete(path);
        }
    }

    private static void ResetConsole()
    {
        // Reconecta o console real — importante para outros testes não serem afetados
        var stdOut = new StreamWriter(System.Console.OpenStandardOutput()) { AutoFlush = true };
        System.Console.SetOut(stdOut);
        System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput()));
    }
}
```

Esses testes não tentam cobrir tudo — são verificações rápidas do básico. Três coisas que você realmente quer ter certeza:

- Sair funciona
- Um ciclo completo de criar → jogar → salvar → sair mantém os dados
- Entrada ruim não trava o loop

(O primeiro teste verifica que *"Quit"* aparece na saída. Isso funciona porque digitar `4` simplesmente retorna de `Run`. Se quiser uma verificação mais forte, certifique-se de que o menu não aparece *duas vezes*.)

Rode:

```powershell
dotnet test
```

Espere `Passed: 71` (68 + 3).

## Mexa um pouco

Adicione uma opção de menu `5. Quick stats` que imprime o número total de saves, mais o mais antigo e o mais rico. Uma query LINQ dentro do handler.

Adicione uma opção de renomear no loop de jogo. Leia um novo nome, depois `k.Name = newName`... espera, `Name` é somente leitura. Adicione um método `Rename(string)` ao engine. Salvar fez você mudar o modelo no Módulo 2.3, e agora a interface de usuário faz você mudar de novo.

Tente rodar o programa com entrada vindo de um arquivo: `dotnet run --project Kingdom.Console < script.txt`. O que estiver em `script.txt` vira a entrada. Qualquer UI de console pode ser rodada de um arquivo assim.

Substitua `IsNullOrEmpty` por `string.IsNullOrWhiteSpace(name)`. Agora um nome de `"   "` (só espaços) também falha na verificação. Sempre verifique também espaços, não só string vazia.

## O que você acabou de fazer

Você colocou o seu CRUD de save slots atrás de um menu interativo de verdade. O `Program.cs` encolheu de um longo script de demo para umas cinco linhas — configura o store, depois passa o controle para `SaveSlotUI.Run`. O loop da UI lê entrada e a manda para o handler certo, e cada handler faz exatamente o que você esperaria. Três novos testes alimentam a entrada através de `Console.SetIn`/`SetOut` (71 passando no total): sair funciona, um ciclo completo mantém os dados, e entrada ruim se recupera. A partir de agora, o reino é algo que você pode sentar e jogar por mais de uma sessão.

**Conceitos que você já sabe nomear:**

- **menu loop** — imprimir, ler, despachar, repetir
- **`Console.ReadLine`** — lê uma linha, retorna `null` no EOF
- **`int.TryParse`** — parse seguro de número sem exceções
- **`Console.SetIn` / `SetOut`** — script e captura em testes
- **tratamento de EOF** — retorna de forma limpa quando a entrada acaba

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: um menu loop imprime as opções, lê uma linha, decide o que fazer, e repete. Ninguém corrige isso — é só para você. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar, escreva um menu loop pequeno:

1. Imprima três opções — `1. Say hi`, `2. Count up`, `3. Quit`.
2. Leia uma linha, e use um `switch` para agir sobre ela.
3. A opção 1 imprime uma saudação; a opção 2 imprime um número que sobe a cada vez; a opção 3 retorna para fora do loop.
4. Qualquer outra coisa imprime uma linha curta "pick 1, 2, or 3".
5. Rode e clique pelo seu próprio menu.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
int count = 0;
while (true)
{
    Console.WriteLine("1. Say hi   2. Count up   3. Quit");
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line is null) return;          // entrada acabou

    switch (line.Trim())
    {
        case "1": Console.WriteLine("Hi!"); break;
        case "2": Console.WriteLine(++count); break;
        case "3": return;
        default:  Console.WriteLine("Pick 1, 2, or 3."); break;
    }
}
```

A forma é sempre a mesma: `while (true)` ao redor de imprimir → ler → `switch`, com um ramo que `return`a para sair. O guarda `if (line is null)` trata o momento em que a entrada acaba, para o loop nunca girar para sempre.

</details>

## Movimento git da semana — merge vs rebase (prévia)

Agora o seu `git log` está cheio de commits. Há dois jeitos de trazer trabalho de uma branch para outra, e eles deixam históricos diferentes:

- **Merge** mantém o quadro de duas branches rodando lado a lado, unidas por um *merge commit*. É honesto sobre como o trabalho realmente aconteceu.
- **Rebase** reproduz seus commits em cima de outra branch. Dá a eles novos SHAs (o SHA é a impressão digital única que o git usa para identificar cada commit) e uma linha única reta de histórico. É mais limpo de ler, mas você perde o quadro lado a lado.

No painel do Source Control do VS Code: menu `...` → *Branch → Merge from* (ou *Rebase from*). Escolha a branch de origem.

> **Ou no terminal:**
>
> ```powershell
> git switch main && git merge feature/save-slots     # merge
> git switch feature/save-slots && git rebase main    # rebase
> ```

Uma boa regra a seguir: **rebase suas próprias branches que você ainda não enviou** para arrumá-las antes de mesclar. **Merge** quando o trabalho vai para uma branch compartilhada como `main`. **Nunca rebase** uma branch que outras pessoas já podem ter puxado — seus commits recebem novos SHAs mas os delas não, e o próximo `git pull` fica confuso.

A gente cobre os dois movimentos direito em B3.2 se você fizer esse bônus.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.10 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.10 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 2.11 fecha a Fase 2: **Nomes que Valem o Espaço** — uma passagem cuidadosa pelos nomes de tudo que a gente construiu nesta fase. É o que transforma *"código bom"* em *"código bom que qualquer um pode ler seis meses depois."*
