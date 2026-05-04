using Kingdom.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class MigrationsTests
{
    [Fact]
    public void Migrate_OnFreshDatabase_CreatesSchema()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mig-{Guid.NewGuid():N}.db");
        try
        {
            using (var ctx = new KingdomDbContext(path))
                ctx.Database.Migrate();

            using (var ctx = new KingdomDbContext(path))
                ctx.Kingdoms.Count().ShouldBe(0);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Migrate_OnAlreadyMigratedDatabase_IsIdempotent()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mig-{Guid.NewGuid():N}.db");
        try
        {
            using (var ctx = new KingdomDbContext(path)) ctx.Database.Migrate();
            using (var ctx = new KingdomDbContext(path)) ctx.Database.Migrate();
            using (var ctx = new KingdomDbContext(path)) ctx.Kingdoms.Count().ShouldBe(0);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void GetPendingMigrations_AfterMigrate_IsEmpty()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mig-{Guid.NewGuid():N}.db");
        try
        {
            using var ctx = new KingdomDbContext(path);
            ctx.Database.Migrate();
            ctx.Database.GetPendingMigrations().ShouldBeEmpty();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
