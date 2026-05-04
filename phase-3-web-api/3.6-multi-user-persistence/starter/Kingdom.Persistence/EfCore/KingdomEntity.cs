namespace Kingdom.Persistence.EfCore;

public class KingdomEntity
{
    public int Id { get; set; }
    public string OwnerSub { get; set; } = "";
    public string Name { get; set; } = "";
    public int Day { get; set; }
    public int Gold { get; set; }
    public int Wood { get; set; }
    public int Stone { get; set; }
    public int Food { get; set; }

    public List<BuildingEntity> Buildings { get; set; } = new();
}
