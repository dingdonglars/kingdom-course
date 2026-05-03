namespace Kingdom.Engine;

// NOTE: This class uses System.Random directly. That's bad for testing.
// Module 1.8 will rewrite it to take an IRandom interface.

public class EventEngine
{
    private readonly Random _rng = new();

    public KingdomEvent? RollOnce(Kingdom k)
    {
        if (_rng.NextDouble() > 0.3) return null;

        var pick = _rng.Next(3);
        return pick switch
        {
            0 => new TraderArrived(k.Day, _rng.Next(10, 51)),
            1 when k.Citizens.Count > 0 =>
                new CitizenIll(k.Day, k.Citizens[_rng.Next(k.Citizens.Count)].Name),
            2 when k.Buildings.Count > 0 =>
                new BuildingBurned(k.Day, k.Buildings[_rng.Next(k.Buildings.Count)].Name),
            _ => null
        };
    }
}
