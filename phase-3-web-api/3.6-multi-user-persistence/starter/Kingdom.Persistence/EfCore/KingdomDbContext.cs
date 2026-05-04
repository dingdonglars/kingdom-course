using Microsoft.EntityFrameworkCore;

namespace Kingdom.Persistence.EfCore;

public class KingdomDbContext : DbContext
{
    private readonly string _path;

    public KingdomDbContext(string dbPath) { _path = dbPath; }

    public DbSet<KingdomEntity> Kingdoms => Set<KingdomEntity>();
    public DbSet<BuildingEntity> Buildings => Set<BuildingEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_path};Pooling=False");

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<KingdomEntity>().HasIndex(k => k.OwnerSub);
    }
}
