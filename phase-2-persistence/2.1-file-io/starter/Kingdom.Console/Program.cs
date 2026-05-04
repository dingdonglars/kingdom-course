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
Directory.CreateDirectory(saveFolder);
var savePath = Path.Combine(saveFolder, "kingdom.txt");

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

var loaded = File.ReadAllText(savePath);
Console.WriteLine();
Console.WriteLine("=== File contents ===");
Console.WriteLine(loaded);
