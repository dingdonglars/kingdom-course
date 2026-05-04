namespace Kingdom.Persistence.EfCore;

public class BuildingEntity
{
    public int Id { get; set; }
    public string Kind { get; set; } = "";
    public string Name { get; set; } = "";
    public int Level { get; set; }

    public int KingdomId { get; set; }
    public KingdomEntity? Kingdom { get; set; }
}
