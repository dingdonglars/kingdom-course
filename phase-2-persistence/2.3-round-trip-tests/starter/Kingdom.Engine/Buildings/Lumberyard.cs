namespace Kingdom.Engine.Buildings;

public class Lumberyard : Building
{
    public Lumberyard(string name) : base(name) { }
    public Lumberyard(string name, int level) : base(name, level) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Wood, 3 * Level);
    }
}
