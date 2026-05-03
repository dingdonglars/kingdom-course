# Module 1.3 — Unit Testing Arrives

> **Hook:** today you write tests. Real ones. With xUnit + Shouldly. Tests for the engine, run with `dotnet test`. Green check-marks. By the end of the lesson, the engine has the safety net it needs to grow into a real project.

> **Words to watch**
> - **unit test** — a small automated test that exercises one piece of behavior
> - **xUnit** — the testing framework. Runner + attributes (`[Fact]`, `[Theory]`)
> - **Shouldly** — assertion library with readable failure messages (`x.ShouldBe(5)`)
> - **`[Fact]`** — marks a method as a test (no parameters, runs once)
> - **`[Theory]` + `[InlineData]`** — runs the same test with multiple inputs
> - **arrange / act / assert** — the standard test shape

---

## Why test

You wrote `Building.Upgrade()`. You're confident it works. Six weeks from now you change `Building` to support different upgrade costs per level. Did you break anything? **Without tests: you find out when something downstream crashes.** With tests: `dotnet test` tells you in 0.3 seconds.

Tests are documentation that doesn't rot. A test called `Upgrade_IncreasesLevelByOne` *says what `Upgrade` is supposed to do*. The test runs every time you change anything, so the documentation is always true.

## Do it — set up the test project

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

## Do it — your first test

Open `tests/Kingdom.Engine.Tests/UnitTest1.cs`. Rename it to `BuildingTests.cs` (the rename refactor with `F2` works on filenames too via VS Code's *File: Rename* command, or just rename in Explorer). Replace its content:

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

Run from the repo root:

```powershell
dotnet test
```

You should see:

```
Passed!  - Failed: 0, Passed: 3, Skipped: 0, Total: 3
```

**Three green check-marks. The engine has tests now.**

## More tests — `ResourceLedger`

Create `tests/Kingdom.Engine.Tests/ResourceLedgerTests.cs`. See `starter/tests/Kingdom.Engine.Tests/ResourceLedgerTests.cs` for the full file (8 tests covering empty-ledger initial state, Add, Spend success/failure, negative-amount throw, plus a Theory with three InlineData cases).

`dotnet test` again. **You now have 11 tests. All green.**

## Tinker

- Make a test fail on purpose: change one `.ShouldBe(50)` to `.ShouldBe(51)`. Run. Read the failure message — Shouldly tells you exactly what was expected and what was actually got.
- Add a test for `Citizen` — `Citizen_NewCitizen_StartsIdle`.
- Try `Should.NotThrow(() => ...)` to assert that some code DOESN'T throw.

## Name it

- **Test framework (xUnit).** Provides the `[Fact]` and `[Theory]` attributes, the test runner, and the discovery mechanism. `dotnet test` finds and runs all `[Fact]` and `[Theory]` methods.
- **`[Fact]`** — a single test (no parameters).
- **`[Theory]` + `[InlineData]`** — the same test logic with multiple input sets. Each `[InlineData]` becomes its own test case.
- **Assertion (Shouldly).** `x.ShouldBe(5)` reads naturally and produces clear failure messages. Without Shouldly you'd write `Assert.Equal(5, x)` (xUnit's built-in) — works the same; Shouldly is just nicer.
- **Arrange / Act / Assert.** The convention for structuring a test: set up state, do the thing, check the result.
- **Test naming.** `Upgrade_IncreasesLevelByOne` — the `STANDARDS.md` convention is `Method_Scenario_ExpectedBehavior`. Reads as documentation.

## Quiz / challenge

Open `quiz.md`.

## Connect

The engine now has a safety net. Module 1.4 adds the **game loop** — the tick that makes resources accumulate per turn. Tests will catch any regression there. Module 1.5 introduces inheritance; tests will verify each Building subclass produces the right resources. Module 1.8 introduces FakeItEasy when randomness shows up — because random tests aren't deterministic without a fake.

**The engine is the bet, and the tests are how you sleep at night.**