namespace Kingdom.Engine.Buildings;

public class Building
{
    public string Name { get; }
    public int Level { get; private set; } = 1;

    public Building(string name) { Name = name; }

    protected Building(string name, int level) { Name = name; Level = level; }

    public void Upgrade() => Level++;

    public virtual void Tick(ResourceLedger ledger) { }
}
