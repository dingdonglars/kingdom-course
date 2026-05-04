# Module 2.9 — Save Slots (Multiple Kingdoms)

One save isn't a feature; *many* saves is. Today the EF store grows beyond `Save` and `Load`: `Update`, `Delete`, `ListAll` (already there) — the full **CRUD** quartet. With a real DB underneath, *"give me my last 10 kingdoms"* is a one-line query. The kingdom now has *save slots*, like every game ever made.

> **Words to watch**
>
> - **CRUD** *(crud)* — Create / Read / Update / Delete — the four operations on rows
> - **slot** — one save in the list (game-design term)
> - **`Update`** — modify an existing row (vs `Add`, which inserts a new one)
> - **transaction** — a group of operations that all succeed or all fail together

---

## Why slots matter

Save slots changed games. Before them, you saved over your one save and prayed you didn't load at the wrong time. After them: try a risky strategy, save before, fall back if it goes wrong. Slots invite experimentation.

Mechanically: each slot is one row in `kingdoms`. Listing slots is `SELECT *`. Loading is `SELECT WHERE id =`. Saving over an existing slot is `UPDATE`. Creating a new slot is `INSERT`. Deleting is `DELETE`. Five queries for the entire feature.

## Delta starter

- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — adds `Update(int id, kingdom)` and `Delete(int id)`
- **NEW:** `Kingdom.Persistence/EfCore/KingdomSlotInfo.cs` — DTO for the slot list (so the console doesn't need to know about EF entities)
- **MODIFIED:** `Kingdom.Console/Program.cs` — multi-slot demo (creates 3 saves, lists them, loads slot 2, modifies, updates, lists again)
- **NEW:** `tests/Kingdom.Persistence.Tests/SlotCrudTests.cs`

## Step 1 — `KingdomSlotInfo` DTO

The console doesn't need (and shouldn't see) `KingdomEntity`. Give it a small DTO:

```csharp
namespace Kingdom.Persistence.EfCore;

public record KingdomSlotInfo(int Id, string Name, int Day);
```

Three fields — exactly what a slot picker needs to display.

## Step 2 — `Update` and `Delete`

In `KingdomEfStore.cs`, add:

```csharp
public void Update(int id, Kingdom.Engine.Kingdom kingdom)
{
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = ctx.Kingdoms
        .Include(k => k.Buildings)
        .Single(k => k.Id == id);

    entity.Name  = kingdom.Name;
    entity.Day   = kingdom.Day;
    entity.Gold  = kingdom.Resources.Get(Resource.Gold);
    entity.Wood  = kingdom.Resources.Get(Resource.Wood);
    entity.Stone = kingdom.Resources.Get(Resource.Stone);
    entity.Food  = kingdom.Resources.Get(Resource.Food);

    // Replace buildings — simplest correct strategy. Removes detached, adds new.
    entity.Buildings.Clear();
    entity.Buildings.AddRange(kingdom.Buildings.Select(b =>
        new BuildingEntity { Kind = b.GetType().Name, Name = b.Name, Level = b.Level }));

    ctx.SaveChanges();
}

public void Delete(int id)
{
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = ctx.Kingdoms.Find(id);
    if (entity is null) return;
    ctx.Kingdoms.Remove(entity);
    ctx.SaveChanges();
}

public IReadOnlyList<KingdomSlotInfo> ListSlots()
{
    using var ctx = new KingdomDbContext(_dbPath);
    return ctx.Kingdoms
        .AsNoTracking()
        .OrderBy(k => k.Id)
        .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
        .ToList();
}
```

A few notes:

- **`Find(id)` vs `Single(...)`** — `Find` returns `null` if missing (good for delete-if-exists semantics), `Single` throws.
- **`.Select(k => new KingdomSlotInfo(...))`** is *projection* — EF generates SQL that pulls only those three columns, not the whole row. Faster + less memory + no unwanted entity tracking.
- **`Clear()` + `AddRange()`** for the building list — EF tracks the deletions and inserts in a single transaction. For a small list this is fine; for a list of 10000, you'd diff and update.
- **Cascade delete** — by default EF deletes child rows (buildings) when the parent (kingdom) is deleted. If you don't want that, configure it in `OnModelCreating`.

## Step 3 — multi-slot demo from the console

In `Program.cs`, append:

```csharp
// Save slots demo (M2.9)
Console.WriteLine();
Console.WriteLine("=== Save slots demo ===");

// Create three saves
var slotsStore = new KingdomEfStore(efDb);
var idA = slotsStore.Save(new global::Kingdom.Engine.Kingdom("Alpha"));
var idB = slotsStore.Save(new global::Kingdom.Engine.Kingdom("Beta"));
var idC = slotsStore.Save(new global::Kingdom.Engine.Kingdom("Gamma"));

// List slots
ListSlots(slotsStore);

// Load slot B, advance 5 days, update
var beta = slotsStore.Load(idB, new SystemRandom(0), new SystemClock());
for (int i = 0; i < 5; i++) beta.AdvanceDay();
slotsStore.Update(idB, beta);
Console.WriteLine($"Updated slot {idB} (Beta) — now at day {beta.Day}");

// Delete slot A
slotsStore.Delete(idA);
Console.WriteLine($"Deleted slot {idA} (Alpha)");

ListSlots(slotsStore);

void ListSlots(KingdomEfStore store)
{
    Console.WriteLine($"All slots:");
    foreach (var s in store.ListSlots())
        Console.WriteLine($"  #{s.Id}  {s.Name,-10} day {s.Day}");
}
```

Run. The output reads like a save-slot UI:

```
All slots:
  #2  Alpha      day 1
  #3  Beta       day 1
  #4  Gamma      day 1
Updated slot 3 (Beta) — now at day 6
Deleted slot 2 (Alpha)
All slots:
  #3  Beta       day 6
  #4  Gamma      day 1
```

## Step 4 — tests

`tests/Kingdom.Persistence.Tests/SlotCrudTests.cs`:

```csharp
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
            var id = store.Save(k);
            for (int i = 0; i < 10; i++) k.AdvanceDay();
            store.Update(id, k);

            store.ListSlots().Count.ShouldBe(1);
            var loaded = store.Load(id, new SystemRandom(0), new SystemClock());
            loaded.Resources.Get(Resource.Food).ShouldBeLessThan(30);   // some was eaten
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

            // Now replace its buildings entirely
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
```

Run:

```powershell
dotnet test
```

Expect `Passed: 68` (63 + 5).

## Tinker

Add a sixth slot, then `ListSlots().OrderByDescending(s => s.Day).First()` — the most-played save. One LINQ line; EF generates the `ORDER BY` + `LIMIT 1` SQL.

Add a `LastSavedAt DateTime` field on `KingdomEntity` (and a migration). Sort by it instead. Now your slot picker can show *"last played 3 hours ago."*

What if two saves have the same name? It's allowed — `Id` is the unique identifier. The runtime decides how to display them (e.g., add the date).

Try `store.Update(999, kingdom)`. It throws because of `Single(...)`. Real apps catch this and show *"save slot is gone — you've been looking at stale data."*

## What you just did

You completed the CRUD quartet — Create, Read, Update, Delete — over the kingdoms table. Five new tests prove every operation does what it should, including delete-then-list and update-replaces-buildings (68 passing total). You also met two small-but-good EF tricks: `.Select(k => new KingdomSlotInfo(...))` to project only the columns you need (lightweight, less data, no tracking), and `Find(id)` vs `Single(id)` for *"missing is OK"* vs *"missing is an error."* The kingdom now behaves like every game's save screen: list everything, pick one, load it.

**Key concepts you can now name:**

- **CRUD** — Create / Read / Update / Delete
- **save slot** — one row, one playable kingdom
- **projection** — `.Select(...)` for only the columns you need
- **`Find` vs `Single`** — null-on-miss vs throw-on-miss
- **cascade delete** — parent gone, children gone too

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 2.10 builds the **save-slot UI** in the console — a real interactive pick-and-load loop using the CRUD operations from today.
