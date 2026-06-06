# Module 2.9 — Save Slots (Multiple Kingdoms)

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

One save on its own isn't much. *Many* saves is a real feature. Today the EF store grows past `Save` and `Load` to add `Update` and `Delete` (and `ListAll`, which is already there) — the full **CRUD** group of four. With a real database underneath, *"give me my last 10 kingdoms"* is a one-line query. The kingdom now has *save slots*, like every game you've ever played.

> **Words to watch**
>
> - **CRUD** *(crud)* — Create / Read / Update / Delete — the four operations on rows
> - **slot** — one save in the list (game-design term)
> - **`Update`** — modify an existing row (vs `Add`, which inserts a new one)
> - **transaction** — a group of operations that all succeed or all fail together

---

## Why slots matter

Save slots changed games. Before them, you saved over your one save and hoped you hadn't saved at a bad moment. With slots, you can try a risky plan, save first, and go back to the earlier save if it goes wrong. Slots make it safe to experiment.

Here's how it works in code: each slot is one row in `kingdoms`. Listing the slots is `SELECT *`. Loading one is `SELECT WHERE id =`. Saving over a slot that already exists is `UPDATE`. Making a new slot is `INSERT`. Removing one is `DELETE`. The whole feature is five queries.

## Delta starter

- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — adds `Update(int id, kingdom)` and `Delete(int id)`
- **NEW:** `Kingdom.Persistence/EfCore/KingdomSlotInfo.cs` — DTO for the slot list (so the console doesn't need to know about EF entities)
- **MODIFIED:** `Kingdom.Console/Program.cs` — multi-slot demo (creates 3 saves, lists them, loads slot 2, modifies, updates, lists again)
- **NEW:** `tests/Kingdom.Persistence.Tests/SlotCrudTests.cs`

## Step 1 — `KingdomSlotInfo` DTO

The console doesn't need to see `KingdomEntity`, and it shouldn't. Give it a small DTO instead:

```csharp
namespace Kingdom.Persistence.EfCore;

public record KingdomSlotInfo(int Id, string Name, int Day);
```

Three fields — exactly what a slot picker needs to show.

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
```

> **One detail in that code.** `b.GetType().Name` returns the class name as a string — `"Farm"`, `"Lumberyard"`, `"Mine"`. Every C# object knows its own type while the program runs. `GetType()` gives you the type, and `.Name` reads its short name. It's a handy way to save "what kind of thing is this?" into a database column.

```csharp

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

- **`Find(id)` vs `Single(...)`** — `Find` returns `null` when the row is missing (good for "delete it if it's there"). `Single` throws an error instead.
- **`.Select(k => new KingdomSlotInfo(...))`** is *projection* — EF writes SQL that pulls only those three columns, not the whole row. That's faster, uses less memory, and skips tracking you don't need.
- **`Clear()` + `AddRange()`** for the building list — EF handles the deletes and inserts together in one transaction. (A *transaction* is a group of database changes that all succeed together or all fail together.) For a small list this is fine. For a list of 10000, you'd compare the two and update only what changed.
- **Cascade delete** — by default, EF deletes the child rows (the buildings) when you delete the parent (the kingdom). If you don't want that, you can change it in `OnModelCreating`.

## Step 3 — multi-slot demo from the console

In `Program.cs`, append:

```csharp
// Save slots demo (Module 2.9)
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

Run it. The output reads like a save-slot screen:

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

Add a sixth slot, then `ListSlots().OrderByDescending(s => s.Day).First()` — the save you've played the most. One LINQ line, and EF writes the `ORDER BY` + `LIMIT 1` SQL.

Add a `LastSavedAt DateTime` field on `KingdomEntity` (and a migration). Sort by it instead. Now your slot picker can show *"last played 3 hours ago."*

What if two saves have the same name? That's allowed — the `Id` is what makes each one unique. The app decides how to show them apart (for example, by adding the date).

Try `store.Update(999, kingdom)`. It throws an error because of `Single(...)`. Real apps catch this and show a message like *"that save slot is gone — your list was out of date."*

## What you just did

You completed the four CRUD operations — Create, Read, Update, Delete — on the kingdoms table. Five new tests prove each one does what it should, including delete-then-list and update-replaces-buildings (68 passing total). You also met two small but useful EF moves: `.Select(k => new KingdomSlotInfo(...))` to pull only the columns you need (less data, no tracking), and `Find(id)` versus `Single(id)` for *"missing is OK"* versus *"missing is an error."* The kingdom now works like a game's save screen: list everything, pick one, load it.

**Key concepts you can now name:**

- **CRUD** — Create / Read / Update / Delete
- **save slot** — one row, one playable kingdom
- **projection** — `.Select(...)` for only the columns you need
- **`Find` vs `Single`** — null-on-miss vs throw-on-miss
- **cascade delete** — parent gone, children gone too

## On your own

Time to put the book away. Don't scroll back up to the steps — show yourself, from your own head, that the one big idea stuck: the four CRUD moves, and how `Delete` works in EF — find the row, remove it, save. No one marks this — the test runner does, which is the point. It's the fastest way to spot what hasn't stuck yet, while it's still small to fix. Getting stuck here is completely fine — that's exactly what it's for.

First, name the four CRUD operations out loud, with the EF move for each (Create → `Add`, Read → a query, Update → change then `SaveChanges`, Delete → `Remove`). Then, without looking, write the body of a `Delete(int id)` method:

1. Open a context.
2. Find the row.
3. Return if it's missing.
4. Remove it.
5. Save.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
public void Delete(int id)
{
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = ctx.Kingdoms.Find(id);
    if (entity is null) return;       // missing is OK — nothing to do
    ctx.Kingdoms.Remove(entity);
    ctx.SaveChanges();
}
```

`Find(id)` returns `null` when the row isn't there, so the `if` makes "delete something that's already gone" safe instead of an error. `Remove` then `SaveChanges` does the delete. EF also removes the building rows under it — that's the cascade delete.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.9 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.9 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.10 builds the **save-slot UI** in the console — a real pick-and-load loop you can use, built on the CRUD operations from today.
