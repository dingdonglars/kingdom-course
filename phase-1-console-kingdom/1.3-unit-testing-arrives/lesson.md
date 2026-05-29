# Module 1.3 — Unit Testing Arrives

Today you write tests. Real ones, with xUnit and Shouldly, run with `dotnet test` from the terminal. By the end of the lesson the engine has eleven passing tests. The split you did in 1.2 is what makes today possible — the test project will reference the engine the same way the console does, and never touch the console at all.

A unit test is a small piece of code that checks one thing your code does. Why write tests now, while the engine is still simple? Think of a smoke alarm. You put it up before there's a fire, not after. You wrote `Building.Upgrade()` last week. You're sure it works. Six weeks from now you'll change `Building` so each level costs a different amount. Did you break anything? Without tests, you only find out when something else crashes — maybe minutes later, maybe months later. With tests, `dotnet test` tells you in 0.3 seconds.

Tests are also a kind of documentation that stays correct. A test called `Upgrade_IncreasesLevelByOne` *says what `Upgrade` is supposed to do*. The test runs every time you build, so this description can never quietly stop matching the real code.

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

The test project references `Kingdom.Engine` directly, the same way `Kingdom.Console` does. It does not reference the console at all. Tests check the engine. They don't need to know there's a console.

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

The names follow the rule from `STANDARDS.md`: `Method_Scenario_ExpectedBehaviour`. A name like that reads almost like a sentence. Each test is built the same way — *arrange* (set things up), *act* (do the thing), *assert* (check the result). The comments are in the first test to show you the three steps. The other tests leave them out, because the pattern is easy to see once you know it.

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

Create `tests/Kingdom.Engine.Tests/ResourceLedgerTests.cs`. The full file is in `starter/tests/Kingdom.Engine.Tests/ResourceLedgerTests.cs`. It has eight tests. They check the starting state of an empty ledger, `Add`, `Spend` (both when it works and when it fails), the error thrown for a negative amount, and a `[Theory]` with three `[InlineData]` cases that run the same test with different inputs.

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

Three `[InlineData]` lines become three runs of the same test, each with different inputs. That saves you from writing the same test three times. Use `[Theory]` whenever you would otherwise copy a test and change one number.

## Tinker

Make a test fail on purpose. Change one `.ShouldBe(50)` to `.ShouldBe(51)` and run `dotnet test`. Read the failure message — Shouldly tells you exactly what it expected and what it actually got, and it points at the line. Compare that to xUnit's own check (`Assert.Equal(50, ledger.Get(Resource.Wood))`) — same result, but the message helps you less. That's why the project uses Shouldly.

Add a test for `Citizen` — call it `Citizen_New_StartsIdle` and check that `c.Job` is `"Idle"` right after you create the citizen. One file, three lines. You'll write a hundred of these this year.

Try `Should.NotThrow(() => ...)` to check that some code *doesn't* throw an error. This is useful when "it doesn't crash" is the thing you care about.

## What you just did

The engine is protected by tests now. Eleven small tests that run in under a second and prove everything you've said about `Building`, `Upgrade`, and `ResourceLedger`. You met the three pieces every test framework gives you — a way to mark a method as a test (`[Fact]`), a way to run a test with several inputs (`[Theory]` + `[InlineData]`), and a way to say what you expect (`ShouldBe`). You also met the *arrange / act / assert* layout, which is the same in every testing framework you'll ever use, in any language. From here on, every new feature comes with the tests that prove it works — and the tests stay green, because if anything breaks them, you'll see it before you commit.

**Key concepts you can now name:**

- **unit test** — small automated check, one behaviour
- **`[Fact]` vs `[Theory]`** — single test vs parameterised
- **arrange / act / assert** — three-part layout for a test
- **`ShouldBe`** — Shouldly's readable assertion form
- **`Method_Scenario_Expected`** — test naming convention from STANDARDS

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the one big idea stuck: the shape of a test. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

In your test project, add one new test from memory. Name it `Upgrade_CalledTwice_LevelIsThree`. Inside it: *arrange* — make a new `Building`. *Act* — call `Upgrade()` twice. *Assert* — check the level is `3` with `ShouldBe`. Don't forget the `[Fact]` line above the method. Then run `dotnet test` and watch the count go up by one.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
[Fact]
public void Upgrade_CalledTwice_LevelIsThree()
{
    // arrange
    var b = new Building("Farm");
    // act
    b.Upgrade();
    b.Upgrade();
    // assert
    b.Level.ShouldBe(3);
}
```

`dotnet test` should now report one more passing test than before. If you forget the `[Fact]` line, the runner never sees the method and the count doesn't change — that's the most common slip.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 1.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 1.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 1.4 adds the **game loop** — the tick that makes the kingdom move forward in time. The tests you wrote today will catch anything that breaks there, and we'll add five more for the new behaviour. Eleven becomes sixteen.
