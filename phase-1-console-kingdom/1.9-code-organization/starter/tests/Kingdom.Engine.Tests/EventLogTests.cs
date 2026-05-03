using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Resources;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class EventLogTests
{
    [Fact]
    public void NewKingdom_HasEmptyEventLog()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.EventLog.ShouldBeEmpty();
    }

    [Fact]
    public void After50Days_LogHasSomeEvents()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        k.AddBuilding(new Farm("F"));
        for (int i = 0; i < 50; i++) k.AdvanceDay();

        k.EventLog.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void EventDay_AlwaysReflectsKingdomDayWhenLogged()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        for (int i = 0; i < 30; i++) k.AdvanceDay();

        k.EventLog.All(e => e.Day >= 1 && e.Day < k.Day).ShouldBeTrue();
    }
}
