using Kingdom.Engine.Snapshots;

namespace Kingdom.Engine;

public class Kingdom
{
    public string Name { get; }
    private int _day = 1;
    public int Day => _day;

    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public ResourceLedger Resources { get; } = new();
    public List<KingdomEvent> EventLog { get; } = new();

    private readonly EventEngine _eventEngine;
    private readonly IClock _clock;

    public Kingdom(string name, IRandom rng, IClock clock)
    {
        Name = name;
        _eventEngine = new EventEngine(rng);
        _clock = clock;

        Resources.Add(Resource.Gold, 100);
        Resources.Add(Resource.Wood, 50);
        Resources.Add(Resource.Stone, 20);
        Resources.Add(Resource.Food, 30);
    }

    public Kingdom(string name) : this(name, new SystemRandom(), new SystemClock()) { }

    public void AddBuilding(Building b) => Buildings.Add(b);
    public void AddCitizen(Citizen c) => Citizens.Add(c);

    public void AdvanceDay()
    {
        foreach (var b in Buildings) b.Tick(Resources);
        foreach (var _ in Citizens) Resources.Spend(Resource.Food, 1);

        var evt = _eventEngine.RollOnce(this);
        if (evt is not null) EventLog.Add(evt);

        _day++;
    }

    public KingdomSnapshot ToSnapshot()
    {
        var buildings = Buildings
            .Select(b => new BuildingSnapshot(b.GetType().Name, b.Name, b.Level))
            .ToArray();
        var citizens = Citizens
            .Select(c => new CitizenSnapshot(c.Name))
            .ToArray();

        return new KingdomSnapshot(
            Name, Day,
            Resources.Get(Resource.Gold),
            Resources.Get(Resource.Wood),
            Resources.Get(Resource.Stone),
            Resources.Get(Resource.Food),
            buildings, citizens);
    }

    public static Kingdom LoadFrom(KingdomSnapshot snap, IRandom rng, IClock clock)
    {
        var k = new Kingdom(snap.Name, rng, clock);

        k.Resources.SetTo(Resource.Gold, snap.Gold);
        k.Resources.SetTo(Resource.Wood, snap.Wood);
        k.Resources.SetTo(Resource.Stone, snap.Stone);
        k.Resources.SetTo(Resource.Food, snap.Food);

        foreach (var bs in snap.Buildings)
        {
            Building b = bs.Kind switch
            {
                "Farm"        => new Farm(bs.Name, bs.Level),
                "Lumberyard"  => new Lumberyard(bs.Name, bs.Level),
                "Mine"        => new Mine(bs.Name, bs.Level),
                _ => throw new InvalidOperationException($"Unknown building kind '{bs.Kind}'.")
            };
            k.AddBuilding(b);
        }
        foreach (var cs in snap.Citizens)
            k.AddCitizen(new Citizen(cs.Name));

        k._day = snap.Day;
        return k;
    }
}
