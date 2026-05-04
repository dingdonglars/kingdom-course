using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence.EfCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SlotCrudTests
{
    [Fact]
    public void ListSlots_ReturnsLightweightDtos()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.Save(new global::Kingdom.Engine.Kingdom("Alpha"));
            store.Save(new global::Kingdom.Engine.Kingdom("Beta"));
            var slots = store.ListSlots();
            slots.Count.ShouldBe(2);
            slots[0].Name.ShouldBe("Alpha");
            slots[0].Day.ShouldBe(1);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Update_ChangesExistingRow_NotInsertNew()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            var k = new global::Kingdom.Engine.Kingdom("X");
            k.AddCitizen(new Kingdom.Engine.Citizens.Citizen("A"));
            var id = store.Save(k);
            for (int i = 0; i < 10; i++) k.AdvanceDay();
            store.Update(id, k);

            store.ListSlots().Count.ShouldBe(1);
            var loaded = store.Load(id, new SystemRandom(0), new SystemClock());
            loaded.Resources.Get(Resource.Food).ShouldBeLessThan(30);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Update_ReplacesBuildingList()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            var k = new global::Kingdom.Engine.Kingdom("X");
            k.AddBuilding(new Farm("F1"));
            var id = store.Save(k);

            var k2 = new global::Kingdom.Engine.Kingdom("X");
            k2.AddBuilding(new Mine("M1"));
            k2.AddBuilding(new Lumberyard("L1"));
            store.Update(id, k2);

            var loaded = store.Load(id, new SystemRandom(0), new SystemClock());
            loaded.Buildings.Count.ShouldBe(2);
            loaded.Buildings.OfType<Farm>().ShouldBeEmpty();
            loaded.Buildings.OfType<Mine>().Count().ShouldBe(1);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Delete_RemovesRow_AndChildren()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            var k = new global::Kingdom.Engine.Kingdom("Doomed");
            k.AddBuilding(new Farm("F"));
            var id = store.Save(k);

            store.Delete(id);
            store.ListSlots().ShouldBeEmpty();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Delete_NonexistentId_DoesNotThrow()
    {
        var path = Path.Combine(Path.GetTempPath(), $"slot-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.EnsureCreated();
            Should.NotThrow(() => store.Delete(999));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
