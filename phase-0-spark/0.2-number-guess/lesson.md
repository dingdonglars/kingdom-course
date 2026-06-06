# Module 0.2 — Number Guess

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

The computer picks a number between 1 and 100. You guess. It tells you if you're too high, too low, or right — and it's a bit rude about it. Today is your first program with a *loop*: code that runs over and over until something tells it to stop. By the end you'll have a real little game, and you'll know three new ideas — loops, conditionals, and how to turn typed text into a number.

> **Words to watch**
>
> - **loop** — a chunk of code that runs repeatedly until you tell it to stop
> - **`while`** — the C# keyword that starts a "keep doing this until something changes" loop
> - **conditional** — `if` / `else` — a fork in your code based on whether something is true or false
> - **`int.Parse(...)`** — the method that turns a string of digits into a real `int`
> - **`break`** — the C# keyword that jumps straight out of the loop you're in

---

## Step 1 — make a new project

Make a new folder next to `RoastOMatic`, at the top of your repo. In PowerShell:

```powershell
cd ..
dotnet new console -n NumberGuess
cd NumberGuess
```

You now have two console projects sitting next to each other in the same repo. They don't know about each other. They are two separate little programs.

## Step 2 — write the game

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
    var guess = int.Parse(input);
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

A lot is happening in those thirty lines. The first three lines set up the game: a thing that makes random numbers, the secret number it picked, and a counter for how many guesses the player has used.

The `while (true) { ... }` block is your first **loop**. It runs the code inside over and over, forever, until something tells it to stop. Inside the loop, the program shows a prompt, reads what the player types, turns that text into a whole number with `int.Parse`, and adds one to the counter.

The `if / else if / else` chain is a **conditional**. Think of it as a split in the path. Only one of the three branches runs each time around the loop, depending on which condition is true. When the player finally guesses right, the `else` branch prints the win message, and the `break` keyword jumps out of the loop, which ends the program.

Run it:

```powershell
dotnet run
```

Guess until you get it right.

## Tinker

Make the messages even ruder. The program is yours, so have fun with it.

Track the number of guesses and rate the player at the end. *"1 guess: are you cheating? 5 guesses: acceptable. 50 guesses: have you considered a different hobby?"*

Add a give-up option. If the user types `quit`, show the number and exit. You'll need an extra `if` before the `int.Parse` line, because `quit` isn't a number.

Make the range a variable. Move `1` and `101` into named whole numbers at the top, so you can change the difficulty by editing one place instead of two.

## Name it

A **loop** is code that runs again and again. The `while (condition) { ... }` form keeps running while the condition is true. `while (true)` runs forever, until a `break;` inside the body jumps out.

A **conditional** is the `if (...) { } else if (...) { } else { }` form. C# checks the conditions from top to bottom. The first one that is true runs its block, and the rest are skipped.

The **`Random` class** knows how to make *pseudo-random* numbers. These are numbers that *look* random but are really worked out by a fixed math formula. They feel random enough for a game, and you can get the same sequence again if you ever want to. You created one with `new Random()`, which gives you a `Random` object, stored in the `random` variable. Then you asked it for a number with `random.Next(1, 101)`, which is a method call on that object. The number is from 1 to 100. The lower number (1) is included, but the upper number (101) is not. That's a choice Microsoft made, and it surprises almost everyone the first time.

## What you just did

You wrote a real game. The program picks a number, you guess, it tells you how you did, you guess again. That loop is at the centre of every program that does more than print once and stop. You met three named ideas: loops (`while`), conditionals (`if`/`else if`/`else`), and turning text into a number (`int.Parse`). The whole game is about thirty lines of code, and one of those is a comment.

**Key concepts you can now name:**

- **`while` loop** — repeat until something changes
- **`break`** — jump out of the current loop
- **conditional** — `if`/`else if`/`else` branching
- **`int.Parse`** — turn a string of digits into an `int`
- **`Random.Next(min, max)`** — random number, upper bound excluded

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open a new empty file. Without looking, write a small program with a `while (true)` loop that counts up:

1. Start a number at `1`.
2. Each time around the loop, print it and add `1`.
3. When it reaches `5`, use `break` to jump out of the loop.

It should print `1 2 3 4 5` and then stop.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
var n = 1;
while (true)
{
    Console.WriteLine(n);
    if (n == 5)
    {
        break;
    }
    n++;
}
```

- `while (true)` runs forever, until a `break` inside jumps out.
- The `if` checks one condition each time around. When `n` is `5`, `break` ends the loop.
- `n++` adds `1` to `n`.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 0.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage both files, commit message `Module 0.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 0.3 turns this into a tiny adventure — several rooms, choices that change what happens, your first text game. Same loops and conditionals, just a bigger story.
