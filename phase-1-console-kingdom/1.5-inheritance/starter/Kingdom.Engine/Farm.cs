namespace Kingdom.Engine;

public class Farm : Building
{
    public Farm(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Food, 5 * Level);
    }
}
