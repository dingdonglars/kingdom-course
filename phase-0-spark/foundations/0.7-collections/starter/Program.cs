// Inventory Tool — Module 0.7 starter (v1, before error handling)

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