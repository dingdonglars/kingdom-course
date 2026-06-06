# Módulo 0.7 — Collections + Inventory Tool, v1

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje você começa o **Inventory Tool** — o programa que você vai entregar no marco M1. É um pequeno programa de linha de comando que adiciona, remove, encontra, lista, salva e carrega itens. No final desta lição ele funciona quando tudo vai bem: cada comando faz o que você espera quando a entrada é digitada corretamente. O Módulo 0.8 o torna mais forte contra os casos estranhos. Ao longo do caminho, você conhece os dois tipos de coleção que vai mais usar pelo resto do curso — `List<T>` e `Dictionary<K, V>` — mais LINQ, uma forma de trabalhar com coleções que se lê quase como inglês simples.

> **Words to watch**
>
> - **collection** — qualquer estrutura de dados que guarda múltiplos valores (lista, dicionário, array)
> - **`List<T>`** — lista ordenada e crescente de `T`s
> - **`Dictionary<K, V>`** — uma tabela de busca de chave para valor; como uma lista, mas indexada por chave
> - **`foreach`** — percorre cada item de uma coleção, um de cada vez
> - **`for`** — loop com contador (`i = 0; i < n; i++`)
> - **LINQ** — Language-Integrated Query: methods como `.Where`, `.Select`, `.OrderBy`, `.Sum` que funcionam em qualquer coleção

---

## Passo 1 — tour rápido pelas coleções

```powershell
cd <your-repo-root>
dotnet new console -n CollectionsDemo
cd CollectionsDemo
```

Substitua `Program.cs`:

```csharp
// List<T> — ordered, items can repeat
var resources = new List<string> { "gold", "wood", "stone", "wood" };

Console.WriteLine($"Resources count: {resources.Count}");

// foreach — visit each item
foreach (var r in resources)
{
    Console.WriteLine($"  - {r}");
}

// LINQ — querying in style
var distinct = resources.Distinct().ToList();
Console.WriteLine($"Distinct: {string.Join(", ", distinct)}");

var startsWithW = resources
    .Where(r => r.StartsWith("w"))
    .ToList();
Console.WriteLine($"Starts with w: {string.Join(", ", startsWithW)}");

// Dictionary<K, V> — lookup by key
var stockpile = new Dictionary<string, int>
{
    ["gold"] = 100,
    ["wood"] = 30,
    ["stone"] = 12,
};

Console.WriteLine($"Gold: {stockpile["gold"]}");
stockpile["gold"] += 25;
Console.WriteLine($"After raid: {stockpile["gold"]}");

// for — when you need the index
for (int i = 0; i < resources.Count; i++)
{
    Console.WriteLine($"  [{i}] = {resources[i]}");
}
```

Dois tipos de coleção em uma demo. `List<string>` mantém seus itens em ordem, e você chega a cada um pelo número de posição — `resources[0]` é `"gold"`. Itens podem se repetir (`"wood"` aparece duas vezes no exemplo). `Dictionary<string, int>` busca coisas por uma *chave* em vez disso. Cada chave aparece só uma vez, e a busca é rápida não importa quantas entradas estejam no dicionário.

As duas formas de loop merecem uma olhada mais de perto. `foreach` passa por cada item em ordem, e a variável do loop `r` guarda um item de cada vez. `for` usa um contador, o que é útil quando você precisa do número de posição, ou quando quer fazer loop um número definido de vezes. Use aquele que se lê mais claramente para o trabalho.

LINQ é uma linguagem de consulta embutida no C#. Os methods `Distinct`, `Where`, `OrderBy`, `Sum` e muitos outros funcionam em qualquer coleção. Eles devolvem uma *nova sequência* e deixam o original intacto. O que você passa para `Where` é uma pequeníssima função de uma linha — `r => r.StartsWith("w")` — chamada *lambda*. Você vai conhecer lambdas corretamente na Fase 1. Por enquanto, leia esta como *"dado um `r`, me diga se `r` começa com `w`."*

Rode e veja a saída. Agora você tem dois novos tipos de coleção para usar.

## Passo 2 — o Inventory Tool

Crie um novo projeto para o programa do M1 que você vai entregar:

```powershell
cd <your-repo-root>
dotnet new console -n InventoryTool
cd InventoryTool
```

Substitua `Program.cs`:

```csharp
// Inventory Tool — v1 (Module 0.7)
//
// Commands: add <item>, remove <item>, find <item>, list, save, load, quit
// Storage: Dictionary<string, int> — item name → count

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
    var arg = parts.Length > 1 ? parts[1] : "";

    switch (cmd)
    {
        case "add":
            inventory[arg] = inventory.GetValueOrDefault(arg, 0) + 1;
            Console.WriteLine($"Added: {arg} (now have {inventory[arg]})");
            break;

        case "remove":
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
            var lines = inventory.Select(kvp => $"{kvp.Key}={kvp.Value}");
            File.WriteAllText(SaveFile, string.Join("\n", lines));
            Console.WriteLine($"Saved {inventory.Count} item(s) to {SaveFile}.");
            break;

        case "load":
            if (File.Exists(SaveFile))
            {
                inventory.Clear();
                foreach (var l in File.ReadAllLines(SaveFile))
                {
                    var kv = l.Split('=', 2);
                    if (kv.Length == 2 && int.TryParse(kv[1], out var n))
                        inventory[kv[0]] = n;
                }
                Console.WriteLine($"Loaded {inventory.Count} item(s) from {SaveFile}.");
            }
            else
            {
                Console.WriteLine($"No save file at {SaveFile}.");
            }
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
```

Rode:

```powershell
dotnet run
```

Tente uma sessão assim:

```
> add apple
Added: apple (now have 1)
> add apple
Added: apple (now have 2)
> add banana
Added: banana (now have 1)
> list
You have:
  - apple x2
  - banana x1
> save
Saved 2 item(s) to inventory.txt.
> quit
Bye.
```

Rode o programa de novo. Digite `load`. O seu inventário volta.

## Mexa um pouco

Adicione um comando `count <item>` que imprime só a contagem de um item — mesma ideia que `find`, com saída mais simples.

Adicione um comando `total` que imprime a soma de todas as contagens de itens. LINQ faz isso em uma linha: `inventory.Values.Sum()`.

Faça o `save` acontecer sozinho — todo comando que muda o inventário também escreve no disco. Assim você não perde nada se o programa travar.

Adicione um comando `clear`. É uma chamada de method: `inventory.Clear()`.

## Dê nome às coisas

**`List<T>`** mantém itens em ordem, pode crescer e encolher e deixa o mesmo item aparecer mais de uma vez. Você chega a cada item pelo número de posição com `list[0]`, `list[1]` e assim por diante.

**`Dictionary<K, V>`** busca coisas por uma chave. Cada chave aparece só uma vez. A busca continua rápida mesmo com milhares de entradas, por causa do jeito inteligente que o dicionário armazena as coisas por dentro (ele usa algo chamado hash table).

**`foreach (var x in collection)`** passa por cada item uma vez. Não adicione nem remova itens da coleção enquanto o loop estiver rodando — se fizer isso, o programa quebra com um erro.

**`for (int i = 0; i < n; i++)`** é o loop contador. Use-o quando precisar do número de posição, ou quando estiver fazendo loop um número definido de vezes que não vem de uma coleção.

**LINQ** é o grupo de methods (`.Where`, `.Select`, `.OrderBy`, `.Distinct`, `.Sum` e muitos mais) que funcionam em qualquer coleção. Eles recebem uma função — muitas vezes uma lambda como `r => r.StartsWith("w")` — e devolvem uma nova coleção. Você pode encadeá-los: uma cadeia de chamadas `.Where`, `.Distinct` e `.OrderBy` se lê quase como uma frase.

## O que você acabou de fazer

Você construiu o Inventory Tool, v1 — o seu primeiro programa útil. Ele recebe comandos, edita um `Dictionary<string, int>`, ordena a saída com LINQ, salva no disco e carrega de volta. Cerca de cem linhas de código. O formato de salvar é texto simples (`apple=2` em cada linha) — simples e fácil de ler em qualquer editor. Dois tipos de coleção e uma linguagem de consulta agora fazem parte do que você pode usar. Você vai precisar dos três em quase todo programa daqui para frente.

**Conceitos que você já sabe nomear:**

- **`List<T>`** — ordenada, indexada por posição
- **`Dictionary<K, V>`** — busca rápida por chave
- **`foreach` vs `for`** — visitar cada item, vs loop contador
- **LINQ** — `.Where` `.OrderBy` `.Distinct` `.Sum` em qualquer coleção
- **lambda** — função inline como `r => r.StartsWith("w")`

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a grande ideia pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar:

1. Crie um `Dictionary<string, int>` que mapeia um nome de item para uma contagem.
2. Coloque `"gold"` com o valor `100` nele.
3. Leia `"gold"` de volta e imprima.
4. Adicione `25` a ele e imprima de novo.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
var stock = new Dictionary<string, int>();
stock["gold"] = 100;
Console.WriteLine(stock["gold"]);

stock["gold"] += 25;
Console.WriteLine(stock["gold"]);
```

- Um `Dictionary<string, int>` busca coisas por uma chave. Aqui a chave é uma `string` (o nome do item) e o valor é um `int` (a contagem).
- `stock["gold"] = 100;` armazena o valor sob a chave `"gold"`.
- `stock["gold"]` o lê de volta. A segunda impressão mostra `125`.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote as suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 0.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare os dois arquivos, mensagem do commit `Module 0.7 done`, Sync.
4. **Poste em `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI se você precisar relembrar. Traga as respostas do quiz das quais você estiver menos certo para o próximo sync semanal.

## Próximo

O Módulo 0.8 torna o Inventory Tool mais forte contra entradas inválidas — argumentos vazios, um arquivo de salvamento quebrado, um comando desconhecido — e finaliza o M1. Mesmo código, mas ele lida melhor quando as coisas dão errado.
