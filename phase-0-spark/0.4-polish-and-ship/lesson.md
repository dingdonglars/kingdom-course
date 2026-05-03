# Module 0.4 — Polish + Ship + README Anatomy

> **Hook:** today you make your toys *look* good — ASCII art, colored text, a saved high-scores file. And you write the README that ties M0 together.

> **Words to watch**
> - **ASCII art** — pictures made out of text characters (still cool in 2026, fight me)
> - **`Console.ForegroundColor`** — the property that controls what color text gets written in
> - **`File.WriteAllText`** — the method that writes a string to a file (creates the file if it doesn't exist)
> - **`File.ReadAllText`** — the matching method that reads a file's contents back as a string
> - **README** — the doc at the top of every repo that says what's here

---

## Do it — Polish

Make a new project to host your polish toy. (Or apply this polish to one of your existing toys — your call.)

```powershell
cd ..
dotnet new console -n Polish
cd Polish
```

Open `Program.cs` in VS Code. Replace with:

```csharp
// 1. ASCII art header
var art = @"
   _  _____ _   _  ____ ____   ___  __  __
  | |/ /_ _| \ | |/ ___|  _ \ / _ \|  \/  |
  | ' / | ||  \| | |  _| | | | | | | |\/| |
  | . \ | || |\  | |_| | |_| | |_| | |  | |
  |_|\_\___|_| \_|\____|____/ \___/|_|  |_|
";
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine(art);
Console.ResetColor();

// 2. Greet the player by name (with color)
Console.Write("Your name, hero: ");
var name = Console.ReadLine();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine($"Welcome to your kingdom, {name?.ToUpper()}.");
Console.ResetColor();

// 3. Save the player's name to a file (so we remember them next time)
File.WriteAllText("hero.txt", name ?? "");

// 4. On next run, read the file and greet the returning hero
if (File.Exists("hero.txt"))
{
    var saved = File.ReadAllText("hero.txt");
    Console.WriteLine();
    Console.WriteLine($"(File saved. Next time the program runs, '{saved}' will be remembered.)");
}
```

Run twice. The file `hero.txt` appears in the project folder. **You wrote to disk.**

## Tinker

- Make the welcome message a different color the *second* time the program runs (when the file already exists).
- Save the player's *score* (from Number Guess) — make Polish save the score and read it back.
- Add **multiple** ASCII art banners and pick one randomly each run.
- Use `Console.BackgroundColor` to make a banner with a colored background.

## Name it

- **Files (preview).** `File.WriteAllText("path", "content")` writes a string to a file. `File.ReadAllText("path")` reads a file back. `File.Exists("path")` checks if it's there. The path can be a name (current folder) or a full path. Phase 2 goes much deeper into files; today is just the preview.
- **`Console` API.** `Console.WriteLine`, `Console.Write`, `Console.ReadLine`, `Console.ForegroundColor`, `Console.BackgroundColor`, `Console.ResetColor`, `Console.Clear` — all *methods* and *properties* on the `Console` *class*. The Microsoft docs page for `System.Console` lists everything it can do.
- **README craft.** The README is the doc someone reads when they first open your repo. Anatomy below.

## README anatomy — the four sections that matter

Every good project README has these four sections. Memorise the shape:

1. **What** — one sentence. *"A small console game where you play a kingdom tycoon, written in C#."* If a stranger reads only this, they should know what this is.
2. **How to run** — the actual commands. *"Clone, then `dotnet run` in the project folder."* Don't make people guess.
3. **What I learned** — your README, your voice. What surprised you? What was hard? *(This section is yours; not every README has it, but for a learning project it's gold.)*
4. **What's next** — what you'd do if you kept going. *"Add more rooms. Save high scores. Write a v2 in JavaScript."* Even if "next" is "nothing" — say so.

That's it. Four sections. About 30–50 lines total. **Most READMEs aren't this short. They should be.**

## M0 milestone — *The Joke Toolbox*

You now have:
- `RoastOMatic/` (and v2 from 0.1)
- `NumberGuess/`
- `TinyAdventure/`
- `Polish/`

In your **repo root** (next to all four folders), create a `README.md` and write it yourself using the four-section anatomy above. Each toy gets two sentences: one *what*, one *how to run*. Then a "what I learned" section across all four (one paragraph). Then "what's next" — what would *you* add next?

Commit. Push.

```powershell
git add .
git commit -m "M0: The Joke Toolbox - four toys + README"
git push
```

**Per the milestone ritual** (see `STYLE.md`):

1. Open `journal/wins.md` in your repo and write one paragraph about M0.
2. Post in Slack `#wins` — link to your repo + a screenshot of one toy running.
3. Drop a one-liner: *"Four weeks ago I'd never written code. Today I shipped four games."*

Then take the rest of the day off. You earned it.

## Quiz / challenge

Open `quiz.md`.

## Connect

Foundations starts next week. We finally name the things you've been using all month — types, methods, collections, errors. After Foundations, you'll know enough C# to start building the Kingdom proper.
