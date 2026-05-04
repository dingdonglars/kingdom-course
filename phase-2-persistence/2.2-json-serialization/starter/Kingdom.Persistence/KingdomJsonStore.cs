using System.Text.Json;
using Kingdom.Engine.Resources;

namespace Kingdom.Persistence;

public class KingdomJsonStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public void Save(Kingdom.Engine.Kingdom kingdom, string path)
    {
        var summary = ToSummary(kingdom);
        var json = JsonSerializer.Serialize(summary, Options);
        File.WriteAllText(path, json);
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
}
