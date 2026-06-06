# Module 0.5 — Types Deep Dive + Naming Conventions

You've been writing code for about a month now — strings, numbers, booleans — without us ever stopping to give them names. Today we slow down and name the basic building blocks. Six types you've already used, one new one (`DateTime`), and a small new feature called *nullable*. The compiler may have started warning you about nullable around Module 0.1, if you noticed the squiggles. This is the lesson where those squiggles get explained.

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

Six types in five lines. **Each one tells the compiler what kind of value to expect**, and the compiler refuses anything else. Try `int gold = 3.5;` — it won't compile. Try `bool isAtWar = "false";` — it won't compile either. *That's the whole point of types.* The compiler catches the mistake before the program ever runs.

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

The `(int)` is the cast — *"treat this `double` as an `int` instead."* Two things to remember about C# math, and both of them trip up beginners:

- `int / int` is *integer division* — the decimal part is dropped. `5 / 2` gives you `2`, not `2.5`. If you want `2.5`, at least one side must be a `double`: `5 / 2.0` or `5.0 / 2`.
- Casting from `double` to `int` *truncates* — `(int)3.99` is `3`, not `4`. It drops the decimal part; it does not round.

Both of these will catch you out at some point. Knowing them now means you'll recognise the bug when it happens.

## Step 3 — nullable, and turning the warning system on

This is the new part. Open `TypesDemo.csproj` in your editor. Near the top you'll see:

```xml
<Nullable>enable</Nullable>
```

That one line turns on a feature called **nullable reference types**. With it on, the C# compiler keeps track of which values are *allowed* to be `null` (no value at all) and which aren't. It then warns you when you mix the two up.

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

Now the question is: **once a value might be `null`, what do you do with it?** Three operators help here.

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

Read each one slowly. You'll use these again and again through the year:

- **`??`** — *"give me this backup value when it's `null`."* Most useful when you read user input or load a file.
- **`?.`** — *"don't crash if it's `null`; just give me `null` back."* This lets you call one method after another without checking for `null` at every step.
- **`!`** — *"I know better than the compiler; this isn't actually `null`."* Use this rarely. You're overruling the compiler, and if you're wrong, the program crashes.

### Why warnings only show up *now*

Your earlier programs (Roast-O-Matic, the number guess game, the tiny adventure) didn't show these warnings. That's because their `.csproj` files say `<Nullable>disable</Nullable>` instead of `enable`. Until today, this idea was too much to add on top of everything else, so we left the warnings off. From `TypesDemo` on, they're on. **From this module forward, all your projects have nullable turned on.** The starter projects for the rest of the course already do.

If you're curious, open `RoastOMatic/RoastOMatic.csproj` and change `disable` to `enable`. Save. Open `Program.cs`. Yellow squiggles appear. None of them are bugs in your code; they're places where the compiler isn't sure whether a value could be `null`. Change it back to `disable` if you want; it's up to you. The point is just to *see* what the rule does.

## Tinker

Try `int gold = 3.5;` and read the error.

Try printing `founded` without the format part: `Console.WriteLine($"founded: {founded}");`. How does it look by default?

Try `string name; Console.WriteLine(name);` — the variable is created but never given a value. The compiler refuses it. *Why?*

Add a method `int? FindGold(string kingdomName)` that returns `100` if the name is `"Eldoria"` and `null` otherwise. Call it twice — once with `"Eldoria"`, once with `"Nowhere"`. Use `??` to print `"none"` when the answer is `null`.

## Naming conventions

Now the second half of today's lesson. C# has strict naming conventions, and every C# developer follows them. The reason is that they make code easy to predict. If you see `Kingdom`, you know it's a type. If you see `kingdom`, you know it's a variable. Whether the first letter is capital or not tells you something.

| What | Style | Example |
|---|---|---|
| Type / class / record | **PascalCase** | `Building`, `Kingdom`, `MyClass` |
| Method / property | **PascalCase** | `Console.WriteLine`, `kingdom.Name` |
| Local variable / parameter | **camelCase** | `goldCount`, `firstName` |
| Private field | **`_camelCase`** | `_health`, `_random` (with one underscore) |
| Interface | **`I` + PascalCase** | `IBuilding`, `IRandom` |
| Constant | **PascalCase** | `const int MaxBuildings = 50;` |
| Async method | adds `Async` suffix | `SaveAsync`, `LoadAsync` |

The full set is in `STANDARDS.md` at the top of the curriculum. VS Code (with the C# Dev Kit extension) marks any break from these rules as you type — a yellow squiggle, with the suggestion in the hover box. **Use code suggestions and the rename tool (press `F2` on a name to rename it everywhere at once). Go along with the conventions, don't push against them.**

The two you'll use most this year:

- **Type names — `PascalCase`.** First letter capital, no underscores, no spaces. `Building`, `ResourceLedger`, `KingdomDbContext`.
- **Variables — `camelCase`.** First letter lowercase, then capitals at word starts. `goldCount`, `firstName`, `currentDay`.

Copy the style you see in the code around you. When you're not sure, your **IDE** (*Integrated Development Environment* — the editor you write code in; for this course, that's VS Code) tells you with a red squiggle under a name that has the wrong capital letters.

## What you just did

You learned the six types you'll use for the rest of the year — `int`, `double`, `bool`, `string`, `DateTime`, plus the *nullable* form like `string?`. You met two C# math rules that catch out beginners (integer division and truncating casts). You turned on the nullable warnings in your project, saw what the squiggles mean, and learned three operators (`??`, `?.`, `!`) for working with values that might be missing. And you met the naming conventions every C# developer follows — `PascalCase` for types, `camelCase` for variables. Six new words, one new compiler feature, and a set of naming rules that shape every line of C# you'll write from here on.

**Key concepts you can now name:**

- the five everyday types — `int`, `double`, `bool`, `string`, `DateTime`
- *cast* — change a value from one type to another with `(type)value`
- *nullable* — `string?` allows `null`; `??`, `?.`, `!` work with it
- *PascalCase* and *camelCase* — type names vs variable names

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open a new empty file. Without looking:

1. Make a variable that is allowed to hold `null` — a nullable string — and set it to `null`.
2. Print it, but use `??` to print `"unknown"` when it's `null`. Run it; it should print `unknown`.
3. Change the value to a real word and run again — now it should print the word.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
string? title = null;
Console.WriteLine(title ?? "unknown");

title = "the Brave";
Console.WriteLine(title ?? "unknown");
```

- The `?` in `string?` makes the type nullable — it may hold a string, or `null`.
- `??` means "use this backup value when the left side is `null`." So the first line prints `unknown`, the second prints `the Brave`.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 0.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage both files, commit message `Module 0.5 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 0.6 introduces **methods** — named pieces of code you call by name. You've been calling methods since day 1 (`Console.WriteLine` is a method). Now you'll write your own.
