# Module 0.5 — Types Deep Dive + Naming Conventions

> **Hook:** you've been using strings, numbers, and booleans for a month without us calling them anything. Today we put names on them, learn what each is for, and meet the conventions every C# developer follows.

> **Words to watch**
> - **type** — what *kind* of value something is (number, text, true/false, date)
> - **`int`** — a whole number (no decimal point); range about ±2 billion
> - **`double`** — a number that *can* have a decimal point
> - **`bool`** — `true` or `false`, nothing else
> - **`string`** — text in `"double quotes"`
> - **`DateTime`** — a date *and* a time, together
> - **cast** — convert a value from one type to another (sometimes safely, sometimes not)
> - **nullable** — a type that's allowed to also hold *no value at all* (`null`); written with a trailing `?`
> - **PascalCase** — the convention for type names: `Building`, `Resource`, `MyClass`
> - **camelCase** — for local variables and parameters: `goldCount`, `firstName`
> - **`_camelCase`** — for private fields inside classes (one underscore, then camelCase): `_health`
> - **`IInterface`** — the convention for interface names: `IBuilding`, `IRandom`

---

## Do it — types

Make a new console project:

```powershell
cd <your-repo-root>
dotnet new console -n TypesDemo
cd TypesDemo
```

Replace `Program.cs`:

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

// Casting — explicit conversion
double exactlyHalfGold = gold / 2.0;        // 50.0 (note the .0)
int roundedToBank = (int)exactlyHalfGold;   // 50 (truncates)
Console.WriteLine($"  half: {exactlyHalfGold}, banked: {roundedToBank}");

// Nullable — a value that might be missing
string? nicknameUnknown = null;
string? nickname = "the Bold";
Console.WriteLine($"  unknown nickname: {nicknameUnknown ?? "(none)"}");
Console.WriteLine($"  known nickname: {nickname}");
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
  half: 50, banked: 50
  unknown nickname: (none)
  known nickname: the Bold
```

## Tinker

- Try `int gold = 3.5;` — it won't compile. *Why?* (Read the error.)
- Try `int gold = (int)3.99;` — what does `gold` end up as? *(Truncation, not rounding.)*
- Try printing `founded` without the format string: `Console.WriteLine($"founded: {founded}");`. What's the default?
- Try `bool isAtWar = "false";` — won't compile. *(Strings and bools are different types.)*

## Name it — types

You used six types today. Quick reference:

| Type | What it holds | Example |
|---|---|---|
| `int` | Whole number, ~±2 billion | `int gold = 100;` |
| `double` | Decimal number, big range, not exact for money | `double avg = 12.5;` |
| `bool` | `true` or `false` | `bool ok = true;` |
| `string` | Text | `string name = "Eldoria";` |
| `DateTime` | Date + time | `new DateTime(2024, 1, 15)` |
| `T?` | Nullable T | `string? nickname = null;` |

**The two big traps:**
- `int / int` does *integer division* in C# — it discards the decimal. `5 / 2` is `2`, not `2.5`. To get `2.5`, at least one side must be `double`: `5 / 2.0` or `5.0 / 2`.
- Casting to `int` *truncates* (drops the decimal), it doesn't round. `(int)3.99` is `3`, not `4`.

## Aside — naming conventions (mechanical)

C# has strict conventions. Every C# developer follows them. They exist for *predictability* — if I see `Kingdom`, I know it's a type; if I see `kingdom`, I know it's a variable.

| What | Style | Example |
|---|---|---|
| Type / class / record | **PascalCase** | `Building`, `Kingdom`, `MyClass` |
| Method / property | **PascalCase** | `Console.WriteLine`, `kingdom.Name` |
| Local variable / parameter | **camelCase** | `goldCount`, `firstName` |
| Private field | **`_camelCase`** | `_health`, `_random` (with one underscore) |
| Interface | **`I` + PascalCase** | `IBuilding`, `IRandom` |
| Constant | **PascalCase** | `const int MaxBuildings = 50;` |
| Async method | adds `Async` suffix | `SaveAsync`, `LoadAsync` |

These are codified in `STANDARDS.md` at the repo root. VS Code (with the C# Dev Kit) flags violations as you type. **Use auto-complete and rename refactor (`F2`); don't fight the conventions.**

## Quiz / challenge

Open `quiz.md`.

## Connect

These types and these conventions are *every line* of C# you'll see for the next year. By Module 1.1, you'll be writing your first `class Building` — PascalCase because it's a type, with private fields like `_health`. By Phase 2 you'll meet `DateTime` properly when you save your kingdom. The names you learned today are the words for everything that follows.