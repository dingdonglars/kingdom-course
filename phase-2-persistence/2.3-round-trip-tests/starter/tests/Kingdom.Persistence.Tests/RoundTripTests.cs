using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class RoundTripTests
{
    [Fact]
    public void Empty_Kingdom_Roundtrips()
    {
        var k = new global::Kingdom.Engine.Kingdom("Empty");
        Roundtrip(k).Name.ShouldBe("Empty");
    }

    [Fact]
    public void NameAndDay_Survive_Roundtrip()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test", new SystemRandom(7), new SystemClock());
        for (int i = 0; i < 25; i++) k.AdvanceDay();
        var loaded = Roundtrip(k);
        loaded.Name.ShouldBe("Test");
        loaded.Day.ShouldBe(26);
    }

    [Fact]
    public void Buildings_AndLevels_Survive_Roundtrip()
    {
        var k = new global::Kingdom.Engine.Kingdom("X");
        var f = new Farm("F"); f.Upgrade(); f.Upgrade();
        k.AddBuilding(f);
        k.AddBuilding(new Mine("M"));
        k.AddBuilding(new Lumberyard("L"));

        var loaded = Roundtrip(k);

        loaded.Buildings.Count.ShouldBe(3);
        loaded.Buildings.OfType<Farm>().Single().Level.ShouldBe(3);
        loaded.Buildings.OfType<Mine>().Single().Name.ShouldBe("M");
    }

    [Fact]
    public void Resources_Survive_Roundtrip()
    {
        var k = new global::Kingdom.Engine.Kingdom("X");
        k.Resources.Add(Resource.Gold, 999);

        var loaded = Roundtrip(k);

        loaded.Resources.Get(Resource.Gold).ShouldBe(1099);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(200)]
    public void AnyKingdom_Roundtrips(int days)
    {
        var k = new global::Kingdom.Engine.Kingdom("Sweep", new SystemRandom(42), new SystemClock());
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Lumberyard("L"));
        k.AddCitizen(new Citizen("A"));
        for (int i = 0; i < days; i++) k.AdvanceDay();

        var loaded = Roundtrip(k);

        loaded.Day.ShouldBe(k.Day);
        loaded.Buildings.Count.ShouldBe(k.Buildings.Count);
        foreach (var resource in Enum.GetValues<Resource>())
            loaded.Resources.Get(resource).ShouldBe(k.Resources.Get(resource));
    }

    private static Kingdom.Engine.Kingdom Roundtrip(Kingdom.Engine.Kingdom k)
    {
        var path = Path.Combine(Path.GetTempPath(), $"rt-{Guid.NewGuid():N}.json");
        try
        {
            var store = new KingdomJsonStore();
            store.SaveFull(k, path);
            return store.LoadFull(path, new SystemRandom(0), new SystemClock());
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
