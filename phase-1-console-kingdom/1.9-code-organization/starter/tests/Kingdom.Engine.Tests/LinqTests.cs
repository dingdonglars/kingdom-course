using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Resources;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class LinqTests
{
    [Fact]
    public void OfType_FiltersToFarmsOnly()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F1"));
        k.AddBuilding(new Lumberyard("L1"));
        k.AddBuilding(new Farm("F2"));

        k.Buildings.OfType<Farm>().Count().ShouldBe(2);
    }

    [Fact]
    public void Sum_OfBuildingLevels_AddsUp()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F1"));
        var f2 = new Farm("F2");
        f2.Upgrade(); f2.Upgrade();
        k.AddBuilding(f2);

        k.Buildings.Sum(b => b.Level).ShouldBe(4);
    }

    [Fact]
    public void OrderByDescending_TopBuilding_IsHighestLevel()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F1"));
        var top = new Mine("Big Mine");
        top.Upgrade(); top.Upgrade(); top.Upgrade();
        k.AddBuilding(top);
        k.AddBuilding(new Lumberyard("L1"));

        k.Buildings.OrderByDescending(b => b.Level).First().Name.ShouldBe("Big Mine");
    }

    [Fact]
    public void Any_NoFarms_ReturnsFalse()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Mine("M"));
        k.Buildings.Any(b => b is Farm).ShouldBeFalse();
    }

    [Fact]
    public void All_AllAtLevelOne_ReturnsTrue()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Mine("M"));
        k.Buildings.All(b => b.Level == 1).ShouldBeTrue();
    }
}
