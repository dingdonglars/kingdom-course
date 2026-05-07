using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence;
using Kingdom.Persistence.EfCore;

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
SqliteDemo.RunDemo(dbPath);
Console.WriteLine($"Ran single-table SQLite demo against {dbPath}");

// SQLite JOINs demo (Module 2.5)
var joinsDb = Path.Combine(saveFolder, "kingdoms-joins.db");
if (File.Exists(joinsDb)) File.Delete(joinsDb);
SqliteJoinsDemo.RunDemo(joinsDb);
Console.WriteLine($"Ran JOINs demo against {joinsDb}");

// EF Core demo (Module 2.6)
var efDb = Path.Combine(saveFolder, "kingdoms-ef.db");
if (File.Exists(efDb)) File.Delete(efDb);

var efStore = new KingdomEfStore(efDb);
var savedId = efStore.Save(kingdom);
Console.WriteLine();
Console.WriteLine($"=== EF Core demo ({efDb}) ===");
Console.WriteLine($"Saved kingdom #{savedId}");

var loaded = efStore.Load(savedId, new SystemRandom(0), new SystemClock());
Console.WriteLine($"Loaded: {loaded.Name} with {loaded.Buildings.Count} building(s), gold {loaded.Resources.Get(Resource.Gold)}");

var all = efStore.ListAll();
Console.WriteLine($"All saved kingdoms ({all.Count}):");
foreach (var e in all)
    Console.WriteLine($"  #{e.Id}  {e.Name}  day {e.Day}");
