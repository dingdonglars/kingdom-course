# Module 0.2 — Number Guess

> **Hook:** the computer picks a number between 1 and 100. You guess. It tells you if you're too high, too low, or right — and it's *snarky* about it.

> **Words to watch**
> - **loop** — a chunk of code that runs repeatedly until you tell it to stop
> - **`while`** — the C# keyword that starts a "keep doing this until something is false" loop
> - **conditional** — `if` / `else` — a fork in your code based on whether something is true or false
> - **`int.Parse(...)`** — the method that turns a string of digits into a real `int`

---

## Do it

Make a new folder *next to* `RoastOMatic` (in your repo root). In PowerShell:

```powershell
cd ..
dotnet new console -n NumberGuess
cd NumberGuess
```

Open `Program.cs` in VS Code. Replace its content with:

```csharp
var random = new Random();
var secret = random.Next(1, 101);  // 1..100 inclusive
var guesses = 0;

Console.WriteLine("I'm thinking of a number between 1 and 100. Guess.");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    var guess = int.Parse(input ?? "0");
    guesses++;

    if (guess < secret)
    {
        Console.WriteLine("Too low. And weak.");
    }
    else if (guess > secret)
    {
        Console.WriteLine("Too high. Calm down.");
    }
    else
    {
        Console.WriteLine($"Got it in {guesses}. Bare minimum effort, but I'll allow it.");
        break;
    }
}
```

Run it:

```powershell
dotnet run
```

Guess until you get it right.

## Tinker

- Make the messages even snarkier.
- Track the number of guesses and rate the player at the end (*"1 guess: are you cheating? 5 guesses: acceptable. 50 guesses: have you considered a different hobby?"*).
- Add a "give up" option — if the user types `quit`, reveal the number and exit.
- Make the range a variable (e.g., 1..1000). Hint: pull the `1` and `101` into named variables at the top.

## Name it

- **Loop.** The `while (true) { ... }` block runs over and over, forever, until something inside it tells it to stop. The thing that tells it to stop is `break;`.
- **Conditional.** `if (...) { } else if (...) { } else { }` is a fork in the road. Only one branch runs, depending on which condition is true.
- **`Random`.** The `Random` *class* knows how to make pseudo-random numbers. You created one with `new Random()`. Then you asked it for a number with `random.Next(1, 101)` — a method call. (Yes, `Next(1, 101)` includes 1 but excludes 101 — Microsoft's choice. We'll call out that quirk again later.)

## Quiz / challenge

Open `quiz.md`.

## Connect

Your Kingdom is going to be full of random events — bandit raids, lucky harvests, festival days. The same `Random` you used here will pick which event happens. The same `if/else` you used here will decide what the event does to your gold pile.
</content>
</invoke>