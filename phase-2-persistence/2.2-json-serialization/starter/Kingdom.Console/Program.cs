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
var savePath = Path.Combine(saveFolder, "kingdom.json");

var store = new KingdomJsonStore();
store.Save(kingdom, savePath);
Console.WriteLine($"Saved to {savePath}");

var loaded = store.Load(savePath);
Console.WriteLine();
Console.WriteLine("=== Loaded summary ===");
Console.WriteLine($"  Name: {loaded.Name}");
Console.WriteLine($"  Day:  {loaded.Day}");
Console.WriteLine($"  Buildings: {loaded.BuildingCount}, Citizens: {loaded.CitizenCount}");
Console.WriteLine($"  Gold: {loaded.Gold}, Wood: {loaded.Wood}, Stone: {loaded.Stone}, Food: {loaded.Food}");
