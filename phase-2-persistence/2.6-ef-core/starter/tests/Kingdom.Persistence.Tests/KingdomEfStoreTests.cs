using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence.EfCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class KingdomEfStoreTests
{
    [Fact]
    public void Save_ThenLoad_PreservesNameAndResources()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ef-{Guid.NewGuid():N}.db");
        try
        {
            var k = new global::Kingdom.Engine.Kingdom("EFTest");
            k.Resources.Add(Resource.Gold, 500);
            k.AddBuilding(new Farm("MyFarm"));

            var store = new KingdomEfStore(path);
            var id = store.Save(k);

            var loaded = store.Load(id, new SystemRandom(0), new SystemClock());
            loaded.Name.ShouldBe("EFTest");
            loaded.Resources.Get(Resource.Gold).ShouldBe(600);
            loaded.Buildings.Count.ShouldBe(1);
            loaded.Buildings.OfType<Farm>().Single().Name.ShouldBe("MyFarm");
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Save_TwoKingdoms_BothShowInListAll()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ef-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.Save(new global::Kingdom.Engine.Kingdom("Alpha"));
            store.Save(new global::Kingdom.Engine.Kingdom("Beta"));

            var all = store.ListAll();
            all.Count.ShouldBe(2);
            all.Select(e => e.Name).ShouldBe(new[] { "Alpha", "Beta" });
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Load_NonexistentId_Throws()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ef-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.EnsureCreated();
            Should.Throw<InvalidOperationException>(() =>
                store.Load(999, new SystemRandom(0), new SystemClock()));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
