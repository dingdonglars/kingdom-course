# Module 0.7 — Collections + Inventory Tool, v1

> **Hook:** today you start the **Inventory Tool** — your M1 milestone. A small CLI that adds, removes, finds, lists, and saves items. By the end of this lesson it works for the happy path. Module 0.8 hardens it.

> **Words to watch**
> - **collection** — any data structure that holds multiple values (list, dictionary, array)
> - **`List<T>`** — ordered, growable list of `T`s
> - **`Dictionary<K, V>`** — a lookup table from `K` (key) to `V` (value). Like a list, but indexed by key
> - **`foreach`** — loops through every item in a collection, one at a time
> - **`for`** — loops with a counter (`i = 0; i < n; i++`)
> - **LINQ** — Language-Integrated Query: methods like `.Where`, `.Select`, `.OrderBy`, `.Sum` that work on any collection

---

## Do it — collections quick tour

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

var startsWithW = resources.Where(r => r.StartsWith("w")).ToList();
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

Run, see the output. **Two new collection types in your toolbox.**

## Now — the Inventory Tool

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

Try:

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

Run again, then `load`. Your inventory comes back.

## Tinker

- Add a `count <item>` command that prints just the count for one item.
- Add an `total` command that prints the sum of all item counts (use LINQ: `inventory.Values.Sum()`).
- Make `save` automatic — every command that changes state also writes to disk.
- Add a `clear` command.

## Name it

- **`List<T>`.** Ordered, growable, allows duplicates. Indexed by integer position (`list[0]`).
- **`Dictionary<K, V>`.** Lookup by key. Each key appears at most once. Faster lookup than `List`.
- **`foreach (var x in collection)`.** Visits each element once. Don't modify the collection during the loop (will crash).
- **`for (int i = 0; i < n; i++)`.** Counter loop. Use when you need the index, or when looping a known number of times.
- **LINQ.** Methods like `.Where`, `.Select`, `.OrderBy`, `.Distinct`, `.Sum` that work on any collection. They take a function (often a lambda like `r => r.StartsWith("w")`) and produce a new collection. Compose them: `resources.Where(r => r.StartsWith("w")).Distinct().OrderBy(r => r)`.

## Quiz / challenge

Open `quiz.md`.

## Connect

The Inventory Tool you just built is the M1 milestone — but it has problems. What if the user types `remove` with no item? What if `inventory.txt` is corrupt? What if `add` is called with an empty string? Module 0.8 fixes all of that. Then you ship M1.