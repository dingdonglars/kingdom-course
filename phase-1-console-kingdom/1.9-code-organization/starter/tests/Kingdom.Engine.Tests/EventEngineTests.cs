using FakeItEasy;
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Resources;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class EventEngineTests
{
    [Fact]
    public void RollOnce_HighRoll_ReturnsNull()
    {
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.9);
        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");

        engine.RollOnce(k).ShouldBeNull();
    }

    [Fact]
    public void RollOnce_LowRollPickZero_GivesTrader()
    {
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.1);
        A.CallTo(() => rng.Next(0, 3)).Returns(0);
        A.CallTo(() => rng.Next(10, 51)).Returns(50);
        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");

        var evt = engine.RollOnce(k);

        evt.ShouldBeOfType<TraderArrived>();
        ((TraderArrived)evt!).GoldAmount.ShouldBe(50);
    }

    [Fact]
    public void RollOnce_LowRollPickOne_NoCitizens_ReturnsNull()
    {
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.1);
        A.CallTo(() => rng.Next(0, 3)).Returns(1);
        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");

        engine.RollOnce(k).ShouldBeNull();
    }

    [Fact]
    public void RollOnce_LowRollPickOne_WithCitizen_GivesIllness()
    {
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.1);
        A.CallTo(() => rng.Next(0, 3)).Returns(1);
        A.CallTo(() => rng.Next(0, 1)).Returns(0);

        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("Lyra"));

        var evt = engine.RollOnce(k);
        evt.ShouldBeOfType<CitizenIll>();
        ((CitizenIll)evt!).CitizenName.ShouldBe("Lyra");
    }

    [Fact]
    public void Kingdom_WithFixedRandom_IsFullyDeterministic()
    {
        var k1 = new global::Kingdom.Engine.Kingdom("A", new SystemRandom(seed: 42), new SystemClock());
        var k2 = new global::Kingdom.Engine.Kingdom("B", new SystemRandom(seed: 42), new SystemClock());
        k1.AddCitizen(new Citizen("X")); k1.AddBuilding(new Farm("F"));
        k2.AddCitizen(new Citizen("X")); k2.AddBuilding(new Farm("F"));

        for (int i = 0; i < 30; i++) { k1.AdvanceDay(); k2.AdvanceDay(); }

        k1.EventLog.Count.ShouldBe(k2.EventLog.Count);
        for (int i = 0; i < k1.EventLog.Count; i++)
            k1.EventLog[i].Description.ShouldBe(k2.EventLog[i].Description);
    }
}
