using System.Text.Json;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Engine.Snapshots;

namespace Kingdom.Persistence;

public class KingdomJsonStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    // ----- summary (M2.2) -----

    public void Save(Kingdom.Engine.Kingdom kingdom, string path)
    {
        var summary = ToSummary(kingdom);
        File.WriteAllText(path, JsonSerializer.Serialize(summary, Options));
    }

    public KingdomSummary Load(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<KingdomSummary>(json)
            ?? throw new InvalidOperationException("Could not deserialize kingdom.");
    }

    public static KingdomSummary ToSummary(Kingdom.Engine.Kingdom k) =>
        new(
            k.Name,
            k.Day,
            k.Buildings.Count,
            k.Citizens.Count,
            k.Resources.Get(Resource.Gold),
            k.Resources.Get(Resource.Wood),
            k.Resources.Get(Resource.Stone),
            k.Resources.Get(Resource.Food)
        );

    // ----- full snapshot (M2.3) -----

    public void SaveFull(Kingdom.Engine.Kingdom kingdom, string path)
    {
        var snap = kingdom.ToSnapshot();
        File.WriteAllText(path, JsonSerializer.Serialize(snap, Options));
    }

    public Kingdom.Engine.Kingdom LoadFull(string path, IRandom rng, IClock clock)
    {
        var snap = JsonSerializer.Deserialize<KingdomSnapshot>(File.ReadAllText(path))
            ?? throw new InvalidOperationException("Could not deserialize snapshot.");
        return Kingdom.Engine.Kingdom.LoadFrom(snap, rng, clock);
    }
}
