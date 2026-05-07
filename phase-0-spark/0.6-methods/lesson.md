# Module 0.6 — Methods Deep Dive

You wrote three methods in Tiny Adventure — `Hallway`, `Kitchen`, `Library` — without really naming what was happening. Today you learn the parts. A method takes *parameters* (values you pass in), maybe gives back a *return value*, and lives on a class. We'll meet *overloading* (two methods with the same name, different parameter types) and the `static` keyword that's been hiding behind every `Console.WriteLine` you've called.

> **Words to watch**
>
> - **method** — a named chunk of code that does one thing
> - **parameter** — a value passed *into* a method, named in the parens
> - **argument** — the *actual value* you pass when calling (parameter is the *name*; argument is the *value*)
> - **return value** — the value a method gives back; declared by the type before the method name
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

Why a `static class`? Inside a small console program, the methods you write at the *top* of the file are *local functions*, and C# does not let you overload local functions (two methods with the same name). Wrapping them in a `static class` — a class you don't create with `new`; you just call its methods directly — makes them *static methods*, which can be overloaded. You'll see this pattern everywhere: `Math.Max`, `Console.WriteLine`, `File.ReadAllText` are all static methods on classes.

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

Add a method `int Multiply(int a, int b)` and use it. The pattern is exactly like `AddGold`.

Add a third overload of `Square` for `long`: `public static long Square(long x) => x * x;`. Then call `Helpers.Square(1_000_000_000L)` and watch the result.

Try defining `int Square(int y)` (different parameter *name*, same type) alongside `int Square(int x)`. Compile error. Overloads need different parameter *types*, not just different names. Why? Because where the method is called, C# only sees the types of the values you pass — it can't pick the right method by parameter name.

Make `Greet` take a *second* parameter for an exclamation: `Greet("Eldoria", "Hail!")` should print `"Hail! Greetings, Eldoria."` That's another overload — same name, two parameters instead of one.

## Name it

**Parameter vs argument.** *Parameter* is the name in the method definition (`string kingdom` in `void Greet(string kingdom)`). *Argument* is the actual value you pass at the call (`"Eldoria"` in `Greet("Eldoria")`). The two terms are often used interchangeably in casual conversation; the precise distinction matters when you read API documentation.

**Return value.** What the method gives back. Declared as the type before the method name. `int AddGold(...)` returns an `int`. `void SayHello()` returns nothing — `void` is the special "nothing" type.

**Overload.** Same method name, different parameter types or counts. C# picks which overload to call based on the types of your arguments. `Square(5)` calls the `int` version; `Square(2.5)` calls the `double` version. Overloads with the same parameter types but only different *return* types are not allowed — there's no way for C# to tell them apart.

**Expression-bodied methods.** `int Square(int x) => x * x;` is shorthand for `int Square(int x) { return x * x; }`. The `=>` arrow is an *expression body*. Use it when the method is a single expression; use the `{ ... }` form when you need multiple statements.

**`static`.** You haven't seen the keyword yet because at the top level (in a small console app's `Program.cs`), C# wraps everything for you and `static` is implied. When you start writing classes properly in Phase 1, `static` becomes explicit. The short version: a `static` method belongs to the type itself; a non-static (instance) method belongs to a specific object you've created with `new`.

## Why methods exist

Three reasons.

**Naming.** A method gives a chunk of code a name. `CalculateTax(income)` is far more readable than twenty lines of math without a name.

**Reuse.** Define once, call many times. `Greet("Eldoria")` and `Greet("Westmark")` share one method.

**Hiding detail.** When you call `Console.WriteLine`, you don't think about how it works. The method *encapsulates* a complexity you don't need to think about. Most of programming is choosing what to hide behind a method name.

## What you just did

You took the methods you wrote in Module 0.3 and gave the parts proper names. A method has parameters (the names in the parens), receives arguments (the values you pass), and may return a value (the type before the method name) or nothing (`void`). You learned that two methods can share a name as long as their parameter types differ — that's overloading — and you saw why our methods live on a `static class` rather than as local functions. Five named ideas land in this lesson; every method you write from here on uses some combination of them.

**Key concepts you can now name:**

- **parameter vs argument** — the name vs the value passed
- **return value** — what comes back; `void` means nothing
- **overload** — same name, different parameter types
- **expression body** — `=> expr` shorthand for `{ return expr; }`
- **`static`** — belongs to the type, not an instance

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 0.7 is collections — `List<T>`, arrays, `Dictionary<TKey, TValue>` — the boxes you put many things into. You met one already in Module 0.3; tomorrow we name the rest and see when to use which.
