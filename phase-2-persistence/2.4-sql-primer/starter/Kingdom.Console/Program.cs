using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;

IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(saveFolder);

// JSON save (M2.2/2.3)
var savePath = Path.Combine(saveFolder, "kingdom.json");
var store = new KingdomJsonStore();
store.SaveFull(kingdom, savePath);
Console.WriteLine($"Saved JSON to {savePath}");

// SQLite demo (M2.4)
var dbPath = Path.Combine(saveFolder, "kingdoms.db");
if (File.Exists(dbPath)) File.Delete(dbPath);
var rows = SqliteDemo.RunDemo(dbPath);

Console.WriteLine();
Console.WriteLine($"=== SQLite demo — {rows.Count} row(s) in {dbPath} ===");
foreach (var (id, name, day, gold) in rows)
    Console.WriteLine($"  #{id}  {name}, day {day}, gold {gold}");
