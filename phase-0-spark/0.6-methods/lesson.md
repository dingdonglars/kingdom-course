# Module 0.6 — Methods Deep Dive

Back in Tiny Adventure you wrote three methods: `Hallway`, `Kitchen`, and `Library`. You called them and they worked. But we never talked about how a method is built — the values you put in, and the answer it gives back. Today we will.

Think of a method as a button with a name on it. Once the button exists, you press it and it does its job. You don't rebuild it every time. In a game, "Jump" is a button. You press it, your character jumps. You don't think about *how* the jump works each time you press it. A method is that same idea, in code.

Here's what a method is made of:

- it can take *parameters* — values you hand it
- it can give back a *return value* — an answer it hands back to you
- it lives on a class

We'll also meet *overloading* (two methods with the same name) and the word `static`, which has been there inside every `Console.WriteLine` you've typed. Don't worry — we'll go through them one at a time.

> **Words to watch**
>
> - **method** — a named piece of code that does one job
> - **parameter** — a value passed *into* a method, named inside the brackets `( )`
> - **argument** — the *actual value* you pass when calling (parameter is the *name*; argument is the *value*)
> - **return value** — the value a method gives back; you write its type just before the method name
> - **`void`** — the special return type meaning "this method gives nothing back"
> - **overload** — two methods with the same name but different parameter types
> - **`static`** — a method belonging to the *type* itself, not to a specific *instance*

---

## Step 1 — make a methods demo project

```powershell
cd <your-repo-root>
dotnet new console -n MethodsDemo
cd MethodsDemo
```

Replace `Program.cs`:

```csharp
// A method with NO parameters and NO return value
Helpers.SayHello();

// A method with ONE parameter and NO return value
Helpers.Greet("Eldoria");

// A method with TWO parameters and a RETURN value (int)
int total = Helpers.AddGold(120, 35);
Console.WriteLine($"Total gold: {total}");

// A method with overloading: same name, different parameter types
Console.WriteLine(Helpers.Square(5));        // int version
Console.WriteLine(Helpers.Square(2.5));      // double version

// --- Method definitions live on a static class.
// Static classes can hold overloaded methods (top-level local functions can't).

static class Helpers
{
    public static void SayHello()
    {
        Console.WriteLine("Hello from Eldoria.");
    }

    public static void Greet(string kingdom)
    {
        Console.WriteLine($"Greetings, {kingdom}.");
    }

    public static int AddGold(int a, int b)
    {
        return a + b;
    }

    public static int Square(int x) => x * x;
    public static double Square(double x) => x * x;
}
```

Why put them inside a `static class`? Here's the reason.

In a small console program, any method you write at the *top* of the file is called a *local function*. C# has a rule for local functions: you can't give two of them the same name.

But we *want* two methods both called `Square` — one for whole numbers, one for decimals. So we put them inside a `static class`.

A `static class` is just a named box for methods. You never create it with `new`. You call its methods straight away, like `Helpers.Square(5)`. Methods inside it are *static methods*, and static methods are allowed to have the same name.

You've already used this without noticing. `Math.Max`, `Console.WriteLine`, and `File.ReadAllText` are all static methods that live on classes.

Run it:

```powershell
dotnet run
```

You should see:

```
Hello from Eldoria.
Greetings, Eldoria.
Total gold: 155
25
6.25
```

## Tinker

Now it's your turn. Try these one at a time.

Add a method `int Multiply(int a, int b)` and use it. The pattern is exactly like `AddGold`.

Add a third overload of `Square` for `long`: `public static long Square(long x) => x * x;`. Then call `Helpers.Square(1_000_000_000L)` and look at the result.

Try writing `int Square(int y)` next to `int Square(int x)` — same type, only the name is different. You'll get an error, and the code won't run. Overloads need different parameter *types*, not just different names. Why? When you call the method, C# only sees the *types* of the values you pass. It can't see the difference between the two methods just from the parameter name.

Give `Greet` a *second* parameter, a word to shout first: `Greet("Eldoria", "Hail!")` should print `"Hail! Greetings, Eldoria."` That's another overload — same name, but two parameters instead of one.

## Name it

You just used these ideas. Now let's give them names, with an example for each.

**Parameter vs argument.** These two words get mixed up all the time, so let's make them clear.

A *parameter* is the name you write when you create the method. In `void Greet(string kingdom)`, the parameter is `kingdom`. It's an empty slot, waiting for a value.

An *argument* is the real value you drop into that slot when you call the method. In `Greet("Eldoria")`, the argument is `"Eldoria"`.

Picture a vending machine. The keypad slot that says "enter a code" is the parameter. The `B4` you actually type in is the argument. Same machine, different snack, depending on what you type.

**Return value.** This is the answer a method hands back to you.

You write the type of that answer just before the method name. `int AddGold(...)` hands back an `int`. That's why `int total = Helpers.AddGold(120, 35);` can store the answer in `total`.

Some methods don't hand anything back. `void SayHello()` just prints and stops. `void` is the special word for "I give nothing back." A light switch is like a `void` method: it turns the light on, but it doesn't hand you anything.

**Overload.** Two methods can have the same name, as long as their parameters are different. That's an overload.

C# decides which one you meant by looking at what you pass. `Square(5)` runs the `int` version. `Square(2.5)` runs the `double` version.

We do the same in everyday English. "Play football", "play a song", "play a video game" — same word *play*, and you know which meaning fits from what comes after it. C# does this with the types of your arguments.

One rule: you can't make two overloads that take the same parameters and differ only in what they hand back. C# would have no way to see the difference between them.

**Expression-bodied methods.** `int Square(int x) => x * x;` is a shorter way to write this:

```csharp
int Square(int x) { return x * x; }
```

The `=>` arrow is called an *expression body*. It works well when the whole method is one short line. Use the `{ ... }` form when you need more than one line.

**`static`.** You haven't typed this word yourself yet, and that's normal.

At the top of the file — inside a small console app's `Program.cs` — C# sets that up for you, and `static` is already in place. You'll start writing it yourself in Phase 1, when you build your own classes.

Short version: a `static` method belongs to the *type* itself. You can call it without building anything first — like walking up to a help desk that's always open. `Math.Max(3, 7)` just works.

A non-static method (also called an *instance* method) belongs to one specific object you made with `new`. You need the object first — the same way you need an actual game character before you can tell *that* character to jump. More on this in Phase 1.

## Why methods exist

Three reasons. You'll see all three of them again and again this year.

**Naming.** A method puts a name on a piece of code. `CalculateTax(income)` tells you what it does right away. Twenty lines of math with no name tell you nothing until you read every line. It's the difference between a recipe titled "Make pancakes" and the same steps with no title at the top.

**Reuse.** Write it once, call it as many times as you like. `Greet("Eldoria")` and `Greet("Westmark")` both run the same method. No copy-paste. And if you fix a bug inside the method, every call gets the fix.

**Hiding detail.** When you call `Console.WriteLine`, you don't think about fonts, pixels, or the screen. You just say "print this." The method holds all that detail so you don't have to. It's like the brake pedal in a car: you press it, the car slows, and you never think about how it works inside. A lot of programming is choosing what to hide behind a good method name.

## What you just did

Nice work — you gave names to things you were already doing.

You took the methods you wrote in Module 0.3 and learned what each piece is called. A method has parameters (the names in the brackets). It receives arguments (the real values you pass). It may hand back a value (the type before the method name), or hand back nothing (`void`).

You saw that two methods can have the same name when their parameters are different — that's overloading. And you saw why our methods sit inside a `static class` instead of being local functions.

Five new ideas show up here. Every method you write from now on uses some of them, so they'll soon feel normal.

**Key concepts you can now name:**

- **parameter vs argument** — the name vs the value passed
- **return value** — what comes back; `void` means nothing
- **overload** — same name, different parameter types
- **expression body** — `=> expr` shorthand for `{ return expr; }`
- **`static`** — belongs to the type, not an instance

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 0.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage both files, commit message `Module 0.6 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 0.7 is collections — `List<T>`, arrays, `Dictionary<TKey, TValue>`. These are the boxes you put many things into. You already met one in Module 0.3. Next we name the rest and see when to use each one.
