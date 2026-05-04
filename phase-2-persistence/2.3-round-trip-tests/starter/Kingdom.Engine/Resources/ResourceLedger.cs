namespace Kingdom.Engine.Resources;

public class ResourceLedger
{
    private readonly Dictionary<Resource, int> _amounts = new();

    public ResourceLedger()
    {
        foreach (Resource r in Enum.GetValues<Resource>())
            _amounts[r] = 0;
    }

    public int Get(Resource r) => _amounts[r];

    public void Add(Resource r, int amount)
    {
        if (amount < 0) throw new ArgumentException("Use Spend for negative amounts.");
        _amounts[r] += amount;
    }

    public bool Spend(Resource r, int amount)
    {
        if (amount < 0) throw new ArgumentException("Spend amount must be non-negative.");
        if (_amounts[r] < amount) return false;
        _amounts[r] -= amount;
        return true;
    }

    /// <summary>For load only — overwrites the amount. Don't use in game logic.</summary>
    public void SetTo(Resource r, int amount)
    {
        if (amount < 0) throw new ArgumentException("Amount must be non-negative.");
        _amounts[r] = amount;
    }

    public IReadOnlyDictionary<Resource, int> Snapshot() => _amounts;
}
