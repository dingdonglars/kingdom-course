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

// JSON save (Module 2.2/2.3)
var savePath = Path.Combine(saveFolder, "kingdom.json");
new KingdomJsonStore().SaveFull(kingdom, savePath);
Console.WriteLine($"Saved JSON to {savePath}");

// SQLite single-table demo (Module 2.4)
var dbPath = Path.Combine(saveFolder, "kingdoms.db");
if (File.Exists(dbPath)) File.Delete(dbPath);
var rows = SqliteDemo.RunDemo(dbPath);
Console.WriteLine();
Console.WriteLine($"=== SQLite single-table demo — {rows.Count} row(s) in {dbPath} ===");
foreach (var (id, name, day, gold) in rows)
    Console.WriteLine($"  #{id}  {name}, day {day}, gold {gold}");

// SQLite JOINs demo (Module 2.5)
var joinsDb = Path.Combine(saveFolder, "kingdoms-joins.db");
if (File.Exists(joinsDb)) File.Delete(joinsDb);
var (kingdomRows, joinedBuildings, counts) = SqliteJoinsDemo.RunDemo(joinsDb);

Console.WriteLine();
Console.WriteLine($"=== JOINs demo ({joinsDb}) ===");
Console.WriteLine($"Kingdoms ({kingdomRows.Count}):");
foreach (var k in kingdomRows)
    Console.WriteLine($"  #{k.Id} {k.Name}");
Console.WriteLine($"Buildings (INNER JOIN, {joinedBuildings.Count}):");
foreach (var b in joinedBuildings)
    Console.WriteLine($"  #{b.Id}  k{b.KingdomId}  {b.Kind} '{b.Name}' (lvl {b.Level})");
Console.WriteLine($"Counts (LEFT JOIN + GROUP BY):");
foreach (var c in counts)
    Console.WriteLine($"  {c.Name}: {c.BuildingCount} building(s)");
