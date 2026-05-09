# Module 0.5 — Types Deep Dive + Naming Conventions

You've been writing code for about a month now — strings, numbers, booleans — without us ever stopping to put names on them. Today we slow down and name the building blocks. Six types you've already used; one new one (`DateTime`); and a small new feature called *nullable* that the compiler started warning you about somewhere around Module 0.1 if you were watching the squiggles. This is the lesson where the squiggles get explained.

> **Words to watch**
>
> - **type** — what *kind* of value something is (number, text, true/false, date)
> - **`int`** — a whole number (no decimal point); range about ±2 billion
> - **`double`** — a number that *can* have a decimal point
> - **`bool`** — `true` or `false`, nothing else
> - **`string`** — text in `"double quotes"`
> - **`DateTime`** — a date *and* a time, together
> - **cast** — convert a value from one type to another (sometimes safely, sometimes not)
> - **nullable** — a type that's allowed to also hold *no value at all* (`null`); written with a trailing `?`
> - **PascalCase / camelCase** — naming conventions; we'll meet both today

---

## Step 1 — make a project, use the types

Make a new console project:

```powershell
cd <your-repo-root>
dotnet new console -n TypesDemo
cd TypesDemo
```

Open the new folder in VS Code. Open `Program.cs`. Replace it with:

```csharp
// The five types you'll use most
int gold = 100;                       // whole number
double averagePopulation = 12.5;       // decimal
bool isAtWar = false;                  // true/false
string kingdomName = "Eldoria";        // text
DateTime founded = new DateTime(2024, 1, 15);

Console.WriteLine($"{kingdomName} (founded {founded:yyyy-MM-dd})");
Console.WriteLine($"  gold: {gold}");
Console.WriteLine($"  avg pop: {averagePopulation}");
Console.WriteLine($"  at war: {isAtWar}");
```

Run:

```powershell
dotnet run
```

You should see:

```
Eldoria (founded 2024-01-15)
  gold: 100
  avg pop: 12.5
  at war: False
```

Six types declared in five lines. **Each one tells the compiler what kind of value to expect**, and the compiler refuses anything else. Try `int gold = 3.5;` — won't compile. Try `bool isAtWar = "false";` — won't compile. *That's the point of types.* The compiler catches the mistake before the program ever runs.

| Type | What it holds | Example |
|---|---|---|
| `int` | Whole number, ~±2 billion | `int gold = 100;` |
| `double` | Decimal number, big range, not exact for money | `double avg = 12.5;` |
| `bool` | `true` or `false` | `bool ok = true;` |
| `string` | Text | `string name = "Eldoria";` |
| `DateTime` | Date + time | `new DateTime(2024, 1, 15)` |

## Step 2 — casting

Sometimes you have a value of one type and need it as another. That's a **cast**.

Add to your program:

```csharp
double exactlyHalfGold = gold / 2.0;        // 50.0 (note the .0)
int roundedToBank = (int)exactlyHalfGold;   // 50 (truncates)
Console.WriteLine($"  half: {exactlyHalfGold}, banked: {roundedToBank}");
```

The `(int)` is the cast — *"treat this `double` as an `int` instead."* Two things to remember about C# math, both of which catch beginners:

- `int / int` is *integer division* — the decimal is dropped. `5 / 2` gives you `2`, not `2.5`. If you want `2.5`, at least one side must be a `double`: `5 / 2.0` or `5.0 / 2`.
- Casting from `double` to `int` *truncates* — `(int)3.99` is `3`, not `4`. It drops the decimal; it doesn't round.

Both rules will bite you eventually. Knowing them now means you'll recognise the bug when it happens.

## Step 3 — nullable, and turning the warning system on

This is the new piece. Open `TypesDemo.csproj` in your editor. Near the top you'll see:

```xml
<Nullable>enable</Nullable>
```

That single line turns on a feature called **nullable reference types**. With it on, the C# compiler now keeps track of which values are *allowed* to be `null` (no value at all) and which aren't — and it warns you when you mix them up.

Add this to `Program.cs`:

```csharp
string nickname = null;
```

Save. The line gets a yellow squiggle. Hover the squiggle, or look at the *Problems* panel at the bottom of VS Code. You'll see:

> *Cannot convert null literal to non-nullable reference type.*

The compiler is saying: *"You said `string nickname` — that means a `string`, not `null`. If `nickname` might be `null`, declare it as `string?` instead."*

Fix it:

```csharp
string? nickname = null;
```

The `?` makes the type **nullable** — *"this might be a `string`, or it might be `null`."* The squiggle goes away.

Now the question is: **once a value might be `null`, what do you do with it?** Three operators help.

```csharp
string? nickname = null;

// 1. ?? — "use this if it's null"
string display = nickname ?? "(no nickname)";
Console.WriteLine($"Hello, {display}");

// 2. ?. — "only call .Length if it's not null"
int? length = nickname?.Length;     // null, because nickname is null
Console.WriteLine($"Nickname length: {length ?? -1}");

// 3. ! — "I promise it's not null; trust me"
nickname = "the Bold";
int trustedLength = nickname!.Length;   // works, because we just set it
Console.WriteLine($"Trusted length: {trustedLength}");
```

Read each one slowly. The patterns repeat through the rest of the year:

- **`??`** — *"give me a fallback when it's `null`."* Most useful when you read user input or load a file.
- **`?.`** — *"don't crash if it's `null`; just give me `null` back."* Lets you chain calls safely.
- **`!`** — *"I know better than the compiler; this isn't actually `null`."* Use sparingly — it's an override, and an override is a thing you have to defend later.

### Why warnings only show up *now*

Your earlier toys (Roast-O-Matic, the number guess game, the tiny adventure) didn't show these warnings. That's because their `.csproj` files have `<Nullable>disable</Nullable>` instead of `enable`. Until today, you weren't ready to meet this concept — so we turned the warnings off. As of `TypesDemo`, they're on. **From this module forward, all your projects will have nullable enabled.** The starter projects for the rest of the curriculum already do.

If you're curious, open `RoastOMatic/RoastOMatic.csproj` and change `disable` to `enable`. Save. Open `Program.cs`. Yellow squiggles will appear. None of them are bugs in your code; they're places where the compiler is uncertain. Change it back to `disable` if you want; it's optional. The point is to *see* what the rule does.

## Tinker

Try `int gold = 3.5;` — read the error.

Try printing `founded` without the format string: `Console.WriteLine($"founded: {founded}");`. What's the default layout?

Try `string name; Console.WriteLine(name);` — declared but never given a value. The compiler refuses it. *Why?*

Add a method `int? FindGold(string kingdomName)` that returns `100` if the name is `"Eldoria"` and `null` otherwise. Call it twice — once with `"Eldoria"`, once with `"Nowhere"`. Use `??` to print `"none"` when the answer is `null`.

## Naming conventions

Now the second half of today's lesson. C# has strict naming conventions. Every C# developer follows them. The reason is *predictability* — if you see `Kingdom`, you know it's a type; if you see `kingdom`, you know it's a variable. The pattern of capital-or-not carries information.

| What | Style | Example |
|---|---|---|
| Type / class / record | **PascalCase** | `Building`, `Kingdom`, `MyClass` |
| Method / property | **PascalCase** | `Console.WriteLine`, `kingdom.Name` |
| Local variable / parameter | **camelCase** | `goldCount`, `firstName` |
| Private field | **`_camelCase`** | `_health`, `_random` (with one underscore) |
| Interface | **`I` + PascalCase** | `IBuilding`, `IRandom` |
| Constant | **PascalCase** | `const int MaxBuildings = 50;` |
| Async method | adds `Async` suffix | `SaveAsync`, `LoadAsync` |

The full set is in `STANDARDS.md` at the root of the curriculum. VS Code (with the C# Dev Kit extension) flags any violation as you type — yellow squiggle, hover for the suggestion. **Use auto-complete and the rename refactor (press `F2` on a name to rename it everywhere it's used). Don't fight the conventions.**

The two you'll use most this year:

- **Type names — `PascalCase`.** First letter capital, no underscores, no spaces. `Building`, `ResourceLedger`, `KingdomDbContext`.
- **Variables — `camelCase`.** First letter lowercase, then capitals at word starts. `goldCount`, `firstName`, `currentDay`.

Match what you see in the code around you. When in doubt, your **IDE** (*Integrated Development Environment* — the editor you write code in; for this course, that's VS Code) will tell you with a red squiggle under the wrong-cased name.

## What you just did

You learned the six types you'll use for the rest of the year — `int`, `double`, `bool`, `string`, `DateTime`, plus the *nullable* form like `string?`. You met two C# math traps that bite beginners (integer division and truncating casts). You turned on the nullable-warning system in your project, saw what the squiggles mean, and learned three operators (`??`, `?.`, `!`) for working with values that might be missing. And you met the naming conventions every C# developer follows — `PascalCase` for types, `camelCase` for variables. Six new vocabulary items, one new compiler feature, and a stack of conventions that govern every line of C# you'll write from here on.

**Key concepts you can now name:**

- the five everyday types — `int`, `double`, `bool`, `string`, `DateTime`
- *cast* — change a value from one type to another with `(type)value`
- *nullable* — `string?` allows `null`; `??`, `?.`, `!` work with it
- *PascalCase* and *camelCase* — type names vs variable names

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 0.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage both files, commit message `Module 0.5 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 0.6 introduces **methods** — named chunks of code you call by name. You've been calling methods since day 1 (`Console.WriteLine` is a method); now you'll write your own.
