# Module 0.6 — Methods Deep Dive

> **Hook:** you wrote three methods in Tiny Adventure (`Hallway`, `Kitchen`, `Library`). Today you learn what was actually happening — parameters, return values, overloads, and the `static` keyword that's been on every method you've called.

> **Words to watch**
> - **method** — a named chunk of code that does one thing
> - **parameter** — a value passed *into* a method, named in the parens
> - **argument** — the *actual value* you pass when calling (the parameter is the *name*; the argument is the *value*)
> - **return value** — the value a method gives back; declared by the type before the method name
> - **`void`** — the special return type meaning "this method gives nothing back"
> - **overload** — multiple methods with the same name but different parameters
> - **`static`** — a method (or field) belonging to the *type* itself, not to any specific *instance*

---

## Do it — methods that take and return things

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

// A method with overloading: same name, different parameters
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

> **Why a `static class`?** Inside a small console program, the methods you write at the *top* of the file are *local functions* — and C# does NOT let you overload local functions (two methods with the same name). Wrapping them in a `static class` (a class you don't create with `new`; you just call its methods directly) makes them *static methods*, which DO allow overloading. You'll see this pattern everywhere: `Math.Max`, `Console.WriteLine`, `File.ReadAllText` are all static methods on classes.

Run:

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

- Add a method `int Multiply(int a, int b)` and use it.
- Add a third overload of `Square` for `long` (`long Square(long x) => x * x;`).
- Try defining `int Square(int y)` (different parameter *name*, same type) alongside the `int Square(int x)`. Compile error — overloads need different parameter *types*, not just different names. *Why?*
- Make `Greet` take a *second* parameter for an exclamation: `Greet("Eldoria", "Hail!")` → `"Hail! Greetings, Eldoria."`.

## Name it

- **Parameter vs argument.** *Parameter* is the name in the method definition (`string kingdom` in `void Greet(string kingdom)`). *Argument* is the actual value you pass (`"Eldoria"` in `Greet("Eldoria")`). Often used interchangeably; the strict difference helps when reading documentation.
- **Return value.** What the method *gives back*. Declared as the type before the method name. `int AddGold(...)` returns an `int`. `void SayHello()` returns nothing (`void` is the special "nothing" type).
- **Overload.** Same method name, different parameter types (or counts). C# decides which overload to call based on the types of your arguments. `Square(5)` calls the `int` version; `Square(2.5)` calls the `double` version. Overloads with the same parameter types but different names are *not* overloads — they're a compile error.
- **Expression-bodied methods.** `int Square(int x) => x * x;` is shorthand for `int Square(int x) { return x * x; }`. The `=>` (called the *fat arrow*) is an *expression body*. Use it for methods that fit on one line.
- **`static`.** You haven't seen the keyword yet because at *top-level* (in a small console app's `Program.cs`), C# wraps everything in implicit method scope and the `static` is implied. When you start writing classes (Phase 1), you'll see `static` explicitly. Brief preview: a `static` method belongs to the type itself; a non-static (instance) method belongs to a specific object you've created with `new`.

## Why methods exist

Three reasons:
1. **Naming.** A method gives a chunk of code a *name*. `CalculateTax(income)` is more readable than 20 lines of math without a name.
2. **Reuse.** Define once, call many times. `Greet("Eldoria")` and `Greet("Westmark")` share one method.
3. **Hiding detail.** When you call `Console.WriteLine`, you don't think about how it works. The method *encapsulates* a complexity you don't need to think about. **Most of programming is choosing what to hide behind a method name.**

## Quiz / challenge

Open `quiz.md`.

## Connect

Phase 1 is going to be classes full of methods — `Building.Upgrade()`, `Citizen.AssignJob()`, `Kingdom.AdvanceTurn()`. The patterns from today (parameters, return values, overloads) are exactly how those classes work. The only new word will be `static` (or rather, the absence of it).