using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Resources;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Shouldly;

namespace Kingdom.Engine.Tests;

// We use `global::Kingdom.Engine.Kingdom` because inside the namespace
// `Kingdom.Engine.Tests`, the unqualified name `Kingdom` is ambiguous —
// the compiler reads it as the outer namespace, not the class.
// The `global::` prefix says "start at the very top of the namespace tree."

public class KingdomTickTests
{
    [Fact]
    public void NewKingdom_StartsAtDay1()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.Day.ShouldBe(1);
    }

    [Fact]
    public void AdvanceDay_IncrementsDayCounter()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AdvanceDay();
        k.Day.ShouldBe(2);
    }

    [Fact]
    public void AdvanceDay_CitizensConsumeFood()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        k.AddCitizen(new Citizen("B"));
        var foodBefore = k.Resources.Get(Resource.Food);
        k.AdvanceDay();
        k.Resources.Get(Resource.Food).ShouldBe(foodBefore - 2);
    }

    [Fact]
    public void AdvanceDay_NoFood_DoesNotCrash()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        k.Resources.Spend(Resource.Food, k.Resources.Get(Resource.Food));
        Should.NotThrow(() => k.AdvanceDay());
    }

    [Fact]
    public void AdvanceDay_TenDays_CountsCorrectly()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        for (int i = 0; i < 10; i++) k.AdvanceDay();
        k.Day.ShouldBe(11);
    }
}
