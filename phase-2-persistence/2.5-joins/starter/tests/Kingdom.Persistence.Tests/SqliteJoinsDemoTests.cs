using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SqliteJoinsDemoTests
{
    [Fact]
    public void RunDemo_HasThreeKingdomsAndThreeBuildings()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kjoin-{Guid.NewGuid():N}.db");
        try
        {
            var (kingdoms, buildings, counts) = SqliteJoinsDemo.RunDemo(path);
            kingdoms.Count.ShouldBe(3);
            buildings.Count.ShouldBe(3);
            counts.Count.ShouldBe(3);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Counts_ShowZero_ForKingdomWithNoBuildings()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kjoin-{Guid.NewGuid():N}.db");
        try
        {
            var (_, _, counts) = SqliteJoinsDemo.RunDemo(path);
            counts.Single(c => c.Name == "Stoneholt").BuildingCount.ShouldBe(0);
            counts.Single(c => c.Name == "Eldoria").BuildingCount.ShouldBe(2);
            counts.Single(c => c.Name == "Briarholm").BuildingCount.ShouldBe(1);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void InnerJoin_OnlyReturnsBuildingsThatHaveAKingdom()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kjoin-{Guid.NewGuid():N}.db");
        try
        {
            var (_, buildings, _) = SqliteJoinsDemo.RunDemo(path);
            buildings.All(b => b.KingdomId > 0).ShouldBeTrue();
            buildings.Select(b => b.KingdomId).ShouldContain(1);
            buildings.Select(b => b.KingdomId).ShouldContain(2);
            buildings.Select(b => b.KingdomId).ShouldNotContain(3);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
