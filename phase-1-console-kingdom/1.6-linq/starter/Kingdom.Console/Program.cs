using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Farm("River Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));
kingdom.AddCitizen(new Citizen("Mira"));

PrintReport(kingdom);

for (int day = 0; day < 5; day++)
    kingdom.AdvanceDay();

PrintReport(kingdom);

void PrintReport(Kingdom.Engine.Kingdom k)
{
    Console.WriteLine();
    Console.WriteLine($"== Day {k.Day} — {k.Name} ==");

    var farms = k.Buildings.OfType<Farm>().Count();
    var lumberyards = k.Buildings.OfType<Lumberyard>().Count();
    var mines = k.Buildings.OfType<Mine>().Count();
    var totalLevels = k.Buildings.Sum(b => b.Level);
    var topBuilding = k.Buildings.OrderByDescending(b => b.Level).First();

    Console.WriteLine($"Buildings: {k.Buildings.Count} ({farms} farm, {lumberyards} lumberyard, {mines} mine) — total levels {totalLevels}");
    Console.WriteLine($"Top building: {topBuilding.GetType().Name} '{topBuilding.Name}' (level {topBuilding.Level})");
    Console.WriteLine($"Citizens: {k.Citizens.Count}");

    var foodPerDay = k.Buildings.OfType<Farm>().Sum(f => 5 * f.Level) - k.Citizens.Count;
    Console.WriteLine($"Food net per day: {foodPerDay:+0;-0;0}");

    Console.Write("Resources: ");
    foreach (var (r, n) in k.Resources.Snapshot())
        Console.Write($"{r}={n}  ");
    Console.WriteLine();
}
