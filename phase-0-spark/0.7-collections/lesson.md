# Module 0.7 — Collections + Inventory Tool, v1

Today you start the **Inventory Tool** — your M1 milestone deliverable. A small command-line program that adds, removes, finds, lists, saves, and loads items. By the end of this lesson it works for the happy path: every command does what you'd expect when the input is well-formed. Module 0.8 hardens it against the weird cases. Along the way you meet the two collection types you'll use most for the rest of the course — `List<T>` and `Dictionary<K, V>` — plus LINQ, the query syntax that makes working with collections feel like writing English.

> **Words to watch**
>
> - **collection** — any data structure that holds multiple values (list, dictionary, array)
> - **`List<T>`** — ordered, growable list of `T`s
> - **`Dictionary<K, V>`** — a lookup table from key to value; like a list, but indexed by key
> - **`foreach`** — loops through every item in a collection, one at a time
> - **`for`** — loops with a counter (`i = 0; i < n; i++`)
> - **LINQ** — Language-Integrated Query: methods like `.Where`, `.Select`, `.OrderBy`, `.Sum` that work on any collection

---

## Step 1 — collections quick tour

```powershell
cd <your-repo-root>
dotnet new console -n CollectionsDemo
cd CollectionsDemo
```

Replace `Program.cs`:

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

Two collection types in one demo. `List<string>` is ordered and indexed by integer position — `resources[0]` is `"gold"`. Items can repeat (`"wood"` appears twice in the example). `Dictionary<string, int>` is indexed by *key*; each key appears at most once, and the lookup is fast no matter how many entries are in the dictionary.

The two loop forms are worth pausing on. `foreach` walks each item in order; the loop variable `r` holds one item at a time. `for` uses a counter — handy when you need the index, or when you want to loop a fixed number of times. Use whichever reads more clearly for the task.

LINQ is the query language built into C#. The methods `Distinct`, `Where`, `OrderBy`, `Sum`, and many more work on any collection. They return a *new sequence*; the original is untouched. The argument to `Where` is a tiny inline function — `r => r.StartsWith("w")` — called a *lambda*. You'll meet lambdas properly in Phase 1; for now, read them as *"given an `r`, give back whether `r` starts with `w`."*

Run, see the output. Two new collection types are in your toolbox.

## Step 2 — the Inventory Tool

Make a new project for the M1 deliverable:

```powershell
cd <your-repo-root>
dotnet new console -n InventoryTool
cd InventoryTool
```

Replace `Program.cs`:

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

Run:

```powershell
dotnet run
```

Try a session like this:

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

Run the program again. Type `load`. Your inventory comes back.

## Tinker

Add a `count <item>` command that prints just the count for one item — same logic as `find`, simpler output.

Add a `total` command that prints the sum of all item counts. LINQ makes this one line: `inventory.Values.Sum()`.

Make `save` automatic — every command that changes state also writes to disk. You'll lose nothing if the program crashes.

Add a `clear` command. One method call: `inventory.Clear()`.

## Name it

**`List<T>`** is ordered, growable, and allows duplicates. Indexed by integer position with `list[0]`, `list[1]`, and so on.

**`Dictionary<K, V>`** is a lookup by key. Each key appears at most once. Lookup is fast — internally the dictionary uses a hash table — so it scales to thousands of entries without slowing down.

**`foreach (var x in collection)`** visits each element once. Don't modify the collection during the loop; doing so throws an exception.

**`for (int i = 0; i < n; i++)`** is the counter loop. Use when you need the index, or when looping a known number of times that doesn't come from a collection.

**LINQ** is the family of methods (`.Where`, `.Select`, `.OrderBy`, `.Distinct`, `.Sum`, and many more) that work on any collection. They take a function — often a lambda like `r => r.StartsWith("w")` — and produce a new collection. They compose: a chain of `.Where`, `.Distinct`, `.OrderBy` calls reads as a small story.

## What you just did

You built the Inventory Tool, v1 — your first useful program. It accepts commands, edits a `Dictionary<string, int>`, sorts output with LINQ, saves to disk and loads back. About a hundred lines of code. The save format is plain text (`apple=2` per line) — nothing fancy, fully readable in any editor. Two collection types and one query language are now part of your working vocabulary; you'll use all three in nearly every program from here on.

**Key concepts you can now name:**

- **`List<T>`** — ordered, indexed by position
- **`Dictionary<K, V>`** — fast lookup by key
- **`foreach` vs `for`** — visit each item, vs counter loop
- **LINQ** — `.Where` `.OrderBy` `.Distinct` `.Sum` on any collection
- **lambda** — inline function like `r => r.StartsWith("w")`

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 0.8 hardens the Inventory Tool against bad input — empty arguments, a corrupt save file, an unknown command — and ships M1. Same code; better defences.
