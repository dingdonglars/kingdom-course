# Module 0.1 — Tinker (Roast-O-Matic v2)

> **Hook:** today Roast-O-Matic asks for your friend's name and roasts them by name. *"Hey BOB — your password is 'password'."* Same code as yesterday, two new lines.

> **Words to watch**
> - **variable** — a named place to store a piece of data (so you can use it later)
> - **string** — a piece of text in code, written in `"double quotes"`
> - **method** — a named chunk of code that does one thing; `Console.WriteLine` is a method
> - **`Console.ReadLine()`** — the method that asks the user to type something and gives you back what they typed
> - **string interpolation** — the `$"..."` syntax that lets you stick a variable inside a string

---

## Do it

Open your `RoastOMatic` folder in VS Code. Above the `string[] roasts = ...` line, add this:

```csharp
Console.Write("Who do you want to roast? ");
var name = Console.ReadLine();
```

And change the last line so the roast uses the name:

```csharp
Console.WriteLine($"Hey {name?.ToUpper()} — {roast}");
```

Run it:

```powershell
dotnet run
```

Type a name. Hit enter. Get a personalised roast.

## Tinker

- Try **two** names per run. Hint: `Console.ReadLine()` can be called twice.
- Make Roast-O-Matic ask for what *kind* of roast they want (mild / spicy / nuclear) and pick a roast list based on the answer.
- Add a roast that uses *two* names — *"Hey BOB — at least you're not as bad as ALICE."* Use string interpolation: `$"... {name1} ... {name2} ..."`.

## Name it

You used four things. Now we have names for them.

- **Variables.** `name` is a variable — a labelled spot in memory holding the user's input. The `var` keyword tells C# *"figure out the type from what I'm assigning."*
- **Strings.** Anything in `"double quotes"` is a *string* — a piece of text. C# treats strings as a real type; you'll see `string` written explicitly later.
- **Methods.** `Console.WriteLine`, `Console.Write`, `Console.ReadLine` — all *methods*. A method is a named chunk of code you call by writing its name + `()`. Methods often *take* something inside the parens (the *argument*) and *give back* something (the *return value*). `ReadLine()` takes nothing and gives back a string. `WriteLine(...)` takes a string and gives back nothing.
- **String interpolation.** `$"Hey {name}"` is *string interpolation* — a string with placeholders that get replaced with the values of variables. Beats jamming things together with `+`.

## Quiz / challenge

Open `quiz.md`.

## Connect

You just made your program *interactive*. Tomorrow we add randomness on purpose — your first guessing game.
