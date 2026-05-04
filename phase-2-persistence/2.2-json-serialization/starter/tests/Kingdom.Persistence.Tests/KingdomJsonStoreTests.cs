using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class KingdomJsonStoreTests
{
    [Fact]
    public void Save_ThenLoad_RoundtripsName()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kingdom-{Guid.NewGuid():N}.json");
        try
        {
            var k = new global::Kingdom.Engine.Kingdom("Roundtripper", new SystemRandom(7), new SystemClock());
            var store = new KingdomJsonStore();
            store.Save(k, path);
            var loaded = store.Load(path);
            loaded.Name.ShouldBe("Roundtripper");
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Save_ProducesIndentedJson()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kingdom-{Guid.NewGuid():N}.json");
        try
        {
            var k = new global::Kingdom.Engine.Kingdom("X");
            new KingdomJsonStore().Save(k, path);
            var raw = File.ReadAllText(path);
            raw.ShouldContain("\n");
            raw.ShouldContain("\"Name\": \"X\"");
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void ToSummary_CapturesAllKnownFields()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddCitizen(new Citizen("A"));
        for (int i = 0; i < 5; i++) k.AdvanceDay();

        var s = KingdomJsonStore.ToSummary(k);
        s.Name.ShouldBe("Test");
        s.Day.ShouldBe(6);
        s.BuildingCount.ShouldBe(1);
        s.CitizenCount.ShouldBe(1);
    }

    [Fact]
    public void Load_MissingFile_Throws()
    {
        var store = new KingdomJsonStore();
        var path = Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.json");
        Should.Throw<FileNotFoundException>(() => store.Load(path));
    }

    [Fact]
    public void Load_InvalidJson_ThrowsJsonException()
    {
        var path = Path.Combine(Path.GetTempPath(), $"bad-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path, "{ this is not json");
            var store = new KingdomJsonStore();
            Should.Throw<System.Text.Json.JsonException>(() => store.Load(path));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
