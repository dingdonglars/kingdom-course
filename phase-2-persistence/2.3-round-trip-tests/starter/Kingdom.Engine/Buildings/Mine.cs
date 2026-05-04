namespace Kingdom.Engine.Buildings;

public class Mine : Building
{
    public Mine(string name) : base(name) { }
    public Mine(string name, int level) : base(name, level) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Stone, 2 * Level);
    }
}
