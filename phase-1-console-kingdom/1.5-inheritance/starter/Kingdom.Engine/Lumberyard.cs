namespace Kingdom.Engine;

public class Lumberyard : Building
{
    public Lumberyard(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Wood, 3 * Level);
    }
}
