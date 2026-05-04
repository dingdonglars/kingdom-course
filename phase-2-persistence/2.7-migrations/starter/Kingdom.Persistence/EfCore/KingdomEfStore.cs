using Kingdom.Engine.Buildings;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Microsoft.EntityFrameworkCore;

namespace Kingdom.Persistence.EfCore;

public class KingdomEfStore
{
    private readonly string _dbPath;
    public KingdomEfStore(string dbPath) { _dbPath = dbPath; }

    /// <summary>Apply any pending migrations. Replaces M2.6's EnsureCreated().</summary>
    public void EnsureCreated()
    {
        using var ctx = new KingdomDbContext(_dbPath);
        ctx.Database.Migrate();
    }

    public int Save(Kingdom.Engine.Kingdom kingdom)
    {
        EnsureCreated();
        using var ctx = new KingdomDbContext(_dbPath);

        var entity = new KingdomEntity
        {
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

    public Kingdom.Engine.Kingdom Load(int id, IRandom rng, IClock clock)
    {
        using var ctx = new KingdomDbContext(_dbPath);
        var entity = ctx.Kingdoms
            .Include(k => k.Buildings)
            .Single(k => k.Id == id);

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

    public IReadOnlyList<KingdomEntity> ListAll()
    {
        using var ctx = new KingdomDbContext(_dbPath);
        return ctx.Kingdoms.AsNoTracking().OrderBy(k => k.Id).ToList();
    }
}
