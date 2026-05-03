using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class BuildingTests
{
    [Fact]
    public void NewBuilding_StartsAtLevelOne()
    {
        var b = new Building("Farm");
        b.Level.ShouldBe(1);
    }

    [Fact]
    public void Upgrade_IncreasesLevelByOne()
    {
        var b = new Building("Farm");
        b.Upgrade();
        b.Level.ShouldBe(2);
    }

    [Fact]
    public void Upgrade_CalledThreeTimes_LevelIsFour()
    {
        var b = new Building("Farm");
        b.Upgrade();
        b.Upgrade();
        b.Upgrade();
        b.Level.ShouldBe(4);
    }
}