namespace Kingdom.Engine;

public class Mine : Building
{
    public Mine(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Stone, 2 * Level);
    }
}
