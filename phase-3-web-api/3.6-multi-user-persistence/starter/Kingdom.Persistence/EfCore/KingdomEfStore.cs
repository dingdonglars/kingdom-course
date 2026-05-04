using Kingdom.Engine.Buildings;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Microsoft.EntityFrameworkCore;

namespace Kingdom.Persistence.EfCore;

public class KingdomEfStore
{
    private readonly string _dbPath;
    public KingdomEfStore(string dbPath) { _dbPath = dbPath; }

    public void EnsureCreated()
    {
        using var ctx = new KingdomDbContext(_dbPath);
        ctx.Database.Migrate();
    }

    public int Save(string ownerSub, Kingdom.Engine.Kingdom kingdom)
    {
        EnsureCreated();
        using var ctx = new KingdomDbContext(_dbPath);

        var entity = new KingdomEntity
        {
            OwnerSub = ownerSub,
            Name = kingdom.Name,
            Day  = kingdom.Day,
            Gold = kingdom.Resources.Get(Resource.Gold),
            Wood = kingdom.Resources.Get(Resource.Wood),
            Stone = kingdom.Resources.Get(Resource.Stone),
            Food  = kingdom.Resources.Get(Resource.Food),
            Buildings = kingdom.Buildings
                .Select(b => new BuildingEntity { Kind = b.GetType().Name, Name = b.Name, Level = b.Level })
                .ToList()
        };

        ctx.Kingdoms.Add(entity);
        ctx.SaveChanges();
        return entity.Id;
    }

    public Kingdom.Engine.Kingdom Load(string ownerSub, int id, IRandom rng, IClock clock)
    {
        using var ctx = new KingdomDbContext(_dbPath);
        var entity = ctx.Kingdoms
            .Include(k => k.Buildings)
            .Single(k => k.Id == id && k.OwnerSub == ownerSub);

        var k = new Kingdom.Engine.Kingdom(entity.Name, rng, clock);
        k.Resources.SetTo(Resource.Gold, entity.Gold);
        k.Resources.SetTo(Resource.Wood, entity.Wood);
        k.Resources.SetTo(Resource.Stone, entity.Stone);
        k.Resources.SetTo(Resource.Food, entity.Food);

        foreach (var b in entity.Buildings)
        {
            Building bld = b.Kind switch
            {
                "Farm"        => new Farm(b.Name, b.Level),
                "Lumberyard"  => new Lumberyard(b.Name, b.Level),
                "Mine"        => new Mine(b.Name, b.Level),
                _ => throw new InvalidOperationException($"Unknown building kind '{b.Kind}'.")
            };
            k.AddBuilding(bld);
        }

        return k;
    }

    public void Update(string ownerSub, int id, Kingdom.Engine.Kingdom kingdom)
    {
        using var ctx = new KingdomDbContext(_dbPath);
        var entity = ctx.Kingdoms
            .Include(k => k.Buildings)
            .Single(k => k.Id == id && k.OwnerSub == ownerSub);

        entity.Name  = kingdom.Name;
        entity.Day   = kingdom.Day;
        entity.Gold  = kingdom.Resources.Get(Resource.Gold);
        entity.Wood  = kingdom.Resources.Get(Resource.Wood);
        entity.Stone = kingdom.Resources.Get(Resource.Stone);
        entity.Food  = kingdom.Resources.Get(Resource.Food);

        entity.Buildings.Clear();
        entity.Buildings.AddRange(kingdom.Buildings.Select(b =>
            new BuildingEntity { Kind = b.GetType().Name, Name = b.Name, Level = b.Level }));

        ctx.SaveChanges();
    }

    public void Delete(string ownerSub, int id)
    {
        using var ctx = new KingdomDbContext(_dbPath);
        var entity = ctx.Kingdoms.SingleOrDefault(k => k.Id == id && k.OwnerSub == ownerSub);
        if (entity is null) return;
        ctx.Kingdoms.Remove(entity);
        ctx.SaveChanges();
    }

    public IReadOnlyList<KingdomSlotInfo> ListSlots(string ownerSub)
    {
        using var ctx = new KingdomDbContext(_dbPath);
        return ctx.Kingdoms.AsNoTracking()
            .Where(k => k.OwnerSub == ownerSub)
            .OrderBy(k => k.Id)
            .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
            .ToList();
    }
}
