# Module 1.3 — Unit Testing Arrives

Today you write tests. Real ones, with xUnit and Shouldly, run with `dotnet test` from the terminal. By the end of the lesson the engine has eleven passing tests and a safety net to grow on. The split you did in 1.2 is what makes today possible — the test project will reference the engine the same way the console does, and never touch the console at all.

The reason to test now, before the engine has any real complexity, is the same reason to learn the seatbelt before you drive. You wrote `Building.Upgrade()` last week. You're confident it works. Six weeks from now you'll change `Building` to support different upgrade costs per level. Did you break anything? Without tests, you find out when something downstream crashes — possibly minutes later, possibly months later. With tests, `dotnet test` tells you in 0.3 seconds.

Tests are also documentation that doesn't go stale. A test called `Upgrade_IncreasesLevelByOne` *says what `Upgrade` is supposed to do*. The test runs every time you build, so the documentation can never silently drift away from the truth.

> **Words to watch**
>
> - **unit test** — a small automated check that exercises one piece of behaviour
> - **xUnit** — the testing framework (test runner plus the `[Fact]` and `[Theory]` attributes)
> - **Shouldly** — an assertion library with readable failure messages (`x.ShouldBe(5)`)
> - **`[Fact]`** — marks a method as a test that takes no parameters
> - **`[Theory]` + `[InlineData]`** — runs the same test logic with multiple inputs
> - **arrange / act / assert** — the standard three-step layout for a test

---

## Step 1 — set up the test project

In your repo root (next to `Kingdom.Engine/` and `Kingdom.Console/`):

```powershell
mkdir tests
cd tests
dotnet new xunit -n Kingdom.Engine.Tests
cd Kingdom.Engine.Tests
dotnet add package Shouldly
dotnet add reference ..\..\Kingdom.Engine\Kingdom.Engine.csproj
cd ..\..
dotnet sln add tests\Kingdom.Engine.Tests
```

You now have:

```
your-repo/
├─ Kingdom.Engine/
├─ Kingdom.Console/
├─ tests/
│   └─ Kingdom.Engine.Tests/
└─ Kingdom.slnx
```

The test project references `Kingdom.Engine` directly, the same way `Kingdom.Console` does. It does not reference the console at all. Tests check the engine; they don't need to know there's a console.

## Step 2 — your first three tests

Open `tests/Kingdom.Engine.Tests/UnitTest1.cs`. Rename it to `BuildingTests.cs` (in VS Code: rename in the Explorer panel, or right-click and pick *Rename*). Replace its content:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class BuildingTests
{
    [Fact]
    public void NewBuilding_StartsAtLevelOne()
    {
        // arrange
        var b = new Building("Farm");
        // act — nothing extra
        // assert
        b.Level.ShouldBe(1);
    }

    [Fact]
    public void Upgrade_IncreasesLevelByOne()
    {
        var b = new Building("Farm");
        b.Upgrade();
        b.Level.ShouldBe(2);
    }

    [Fact]
    public void Upgrade_CalledThreeTimes_LevelIsFour()
    {
        var b = new Building("Farm");
        b.Upgrade();
        b.Upgrade();
        b.Upgrade();
        b.Level.ShouldBe(4);
    }
}
```

The names follow the convention from `STANDARDS.md`: `Method_Scenario_ExpectedBehaviour`. Reads like documentation. Each test is structured the same way — *arrange* (set up the state), *act* (do the thing), *assert* (check the result). The comments are there in the first test as a teaching prop; they fall off in the rest because the structure is obvious once you've seen it.

Run from the repo root:

```powershell
dotnet test
```

You should see something like:

```
Passed!  - Failed: 0, Passed: 3, Skipped: 0, Total: 3
```

Three green check-marks. The engine has tests now.

## Step 3 — more tests, for `ResourceLedger`

Create `tests/Kingdom.Engine.Tests/ResourceLedgerTests.cs`. The full file is in `starter/tests/Kingdom.Engine.Tests/ResourceLedgerTests.cs` — it contains eight tests that cover the empty-ledger initial state, `Add`, `Spend` (success and failure), the negative-amount throw, and a `[Theory]` with three `[InlineData]` cases for parameterised input.

Run `dotnet test` again. You should now have 11 passing tests.

## Step 4 — the difference between `[Fact]` and `[Theory]`

`[Fact]` is a single test, no parameters. `[Theory]` is the same idea but takes parameters via `[InlineData]`:

```csharp
[Theory]
[InlineData(Resource.Gold, 100)]
[InlineData(Resource.Wood, 50)]
[InlineData(Resource.Stone, 20)]
public void Add_StartingFromZero_GetReturnsAmount(Resource r, int amount)
{
    var ledger = new ResourceLedger();
    ledger.Add(r, amount);
    ledger.Get(r).ShouldBe(amount);
}
```

Three `[InlineData]` lines becomes three test runs of the same method body, each with different inputs. Saves writing the same test three times. Use `[Theory]` whenever you'd be tempted to copy-paste a test and change one number.

## Tinker

Make a test fail on purpose. Change one `.ShouldBe(50)` to `.ShouldBe(51)` and run `dotnet test`. Read the failure message — Shouldly tells you exactly what was expected and what it actually got, and it points at the line. Compare to xUnit's own assertion (`Assert.Equal(50, ledger.Get(Resource.Wood))`) — same effect, but the message is less helpful. That's why the project pulls Shouldly in.

Add a test for `Citizen` — call it `Citizen_New_StartsIdle` and check that `c.Job` is `"Idle"` right after construction. One file, three lines. You'll write a hundred of these this year.

Try `Should.NotThrow(() => ...)` to assert that some code *doesn't* throw. Useful when "doesn't crash" is the behaviour you care about.

## What you just did

The engine has a safety net now. Eleven small tests that run in under a second and prove every claim you've made about `Building`, `Upgrade`, and `ResourceLedger`. You met the three pieces every test framework gives you — a way to mark a method as a test (`[Fact]`), a way to parameterise a test (`[Theory]` + `[InlineData]`), and a way to express expectations (`ShouldBe`). You also met the *arrange / act / assert* layout, which is the same in every testing framework you'll ever use, in any language. From here on, every new feature ships with the tests that prove it works — and the tests stay green forever, because if anything breaks them, you'll see it before you commit.

**Key concepts you can now name:**

- **unit test** — small automated check, one behaviour
- **`[Fact]` vs `[Theory]`** — single test vs parameterised
- **arrange / act / assert** — three-part layout for a test
- **`ShouldBe`** — Shouldly's readable assertion form
- **`Method_Scenario_Expected`** — test naming convention from STANDARDS

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 1.4 adds the **game loop** — the tick that makes the kingdom move forward in time. The tests you wrote today will catch any regression there, and we'll add five more for the new behaviour. Eleven becomes sixteen.
