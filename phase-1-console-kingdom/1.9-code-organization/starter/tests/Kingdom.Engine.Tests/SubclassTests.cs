using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Resources;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class SubclassTests
{
    [Fact]
    public void Farm_Tick_AddsFoodEqualToFiveTimesLevel()
    {
        var ledger = new ResourceLedger();
        var farm = new Farm("F");
        farm.Tick(ledger);
        ledger.Get(Resource.Food).ShouldBe(5);
    }

    [Fact]
    public void Lumberyard_Tick_AddsWoodEqualToThreeTimesLevel()
    {
        var ledger = new ResourceLedger();
        var ly = new Lumberyard("L");
        ly.Tick(ledger);
        ledger.Get(Resource.Wood).ShouldBe(3);
    }

    [Fact]
    public void Mine_Tick_AddsStoneEqualToTwoTimesLevel()
    {
        var ledger = new ResourceLedger();
        var m = new Mine("M");
        m.Tick(ledger);
        ledger.Get(Resource.Stone).ShouldBe(2);
    }

    [Fact]
    public void Farm_Upgraded_ProducesMore()
    {
        var ledger = new ResourceLedger();
        var farm = new Farm("F");
        farm.Upgrade();
        farm.Tick(ledger);
        ledger.Get(Resource.Food).ShouldBe(10);
    }

    [Fact]
    public void Subclass_InheritsName()
    {
        var farm = new Farm("Main Farm");
        farm.Name.ShouldBe("Main Farm");
        farm.Level.ShouldBe(1);
    }

    [Fact]
    public void Kingdom_AdvanceDay_RunsAllSubclassTicks()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Lumberyard("L"));
        k.AddBuilding(new Mine("M"));

        var foodBefore = k.Resources.Get(Resource.Food);
        var woodBefore = k.Resources.Get(Resource.Wood);
        var stoneBefore = k.Resources.Get(Resource.Stone);

        k.AdvanceDay();

        k.Resources.Get(Resource.Food).ShouldBe(foodBefore + 5);
        k.Resources.Get(Resource.Wood).ShouldBe(woodBefore + 3);
        k.Resources.Get(Resource.Stone).ShouldBe(stoneBefore + 2);
    }
}
