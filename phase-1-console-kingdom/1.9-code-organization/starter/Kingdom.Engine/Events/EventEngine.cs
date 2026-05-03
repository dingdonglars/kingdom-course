namespace Kingdom.Engine.Events;

public class EventEngine
{
    private readonly IRandom _rng;
    public EventEngine(IRandom rng) { _rng = rng; }

    public KingdomEvent? RollOnce(Kingdom k)
    {
        if (_rng.NextDouble() > 0.3) return null;

        var pick = _rng.Next(0, 3);
        return pick switch
        {
            0 => new TraderArrived(k.Day, _rng.Next(10, 51)),
            1 when k.Citizens.Count > 0 =>
                new CitizenIll(k.Day, k.Citizens[_rng.Next(0, k.Citizens.Count)].Name),
            2 when k.Buildings.Count > 0 =>
                new BuildingBurned(k.Day, k.Buildings[_rng.Next(0, k.Buildings.Count)].Name),
            _ => null
        };
    }
}
