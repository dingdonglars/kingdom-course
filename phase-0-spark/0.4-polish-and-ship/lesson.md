# Module 0.4 — Polish + Ship + README Anatomy

Today is the end of Spark Week. You make your programs *look* good — ASCII art, coloured text, and a saved file the program reads back the next time it runs. You write the README that pulls all of M0 together. And you finish it off: commit, push, post in `#wins`. By the end of today you have four working programs in your repo with a real README, and you're ready for Foundations.

> **Words to watch**
>
> - **ASCII art** — pictures made out of text characters (still cool in 2026)
> - **`Console.ForegroundColor`** — the property that controls the colour of text written next
> - **`File.WriteAllText`** — writes a string to a file, creating it if needed
> - **`File.ReadAllText`** — reads a file's contents back as a string
> - **README** — the doc at the top of every repo that says what's here

---

## Step 1 — make a polish project

Make a new console project for this. (You can also add this polish to one of the programs you already have, if you prefer.)

```powershell
cd ..
dotnet new console -n Polish
cd Polish
```

Open `Program.cs` in VS Code. Replace the contents:

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

// 2. Greet the player by name (with colour)
Console.Write("Your name, hero: ");
var name = Console.ReadLine();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine($"Welcome to your kingdom, {name.ToUpper()}.");
Console.ResetColor();

// 3. Save the player's name to a file (so we remember them next time)
File.WriteAllText("hero.txt", name);

// 4. On next run, read the file back and confirm
if (File.Exists("hero.txt"))
{
    var saved = File.ReadAllText("hero.txt");
    Console.WriteLine();
    Console.WriteLine($"(File saved. Next time the program runs, '{saved}' will be remembered.)");
}
```

The `@"..."` form is a **verbatim string**. Line breaks and backslashes are kept exactly as you wrote them, which is just what you want for ASCII art. `Console.ForegroundColor` is a *property* that sets the colour of the text printed next. `Console.ResetColor()` puts it back to the normal colour. Always reset it. A program that changes the colour and then crashes leaves the user's terminal stuck in that colour.

The two `File` lines are the first time you write to the disk. `File.WriteAllText("hero.txt", name)` creates a file in the current folder and writes whatever is in `name`. `File.ReadAllText("hero.txt")` reads it back. We'll cover files much more in Phase 2. Today is just a first look.

Run it twice:

```powershell
dotnet run
dotnet run
```

The file `hero.txt` appears in the project folder. **You wrote to disk.**

## Listen to the editor

Your editor tells you things while you type. When it draws a coloured line under your code, it's pointing at one exact spot. Here's how to read it.

In `Program.cs`, find the line `var name = Console.ReadLine();`. Change `ReadLine` to `readLine` (lowercase r — that one small change, nothing else). Save the file.

Three things happen:

1. **A red squiggle appears under `readLine`.** Hover your mouse over it. A box pops up: *"'Console' does not contain a definition for 'readLine'"*. The editor is telling you that this method does not exist. C# cares about capital letters — `ReadLine` and `readLine` are two different names.

2. **No code suggestions after `name.`** Go to the next line and put your cursor right after `name.` in `{name.ToUpper()}`. Normally a small menu appears with `.ToUpper()`, `.ToLower()`, `.Trim()`, and more. Now it's gone or wrong. Because the line above is broken, the editor doesn't know what `name` is, so it can't help you on the next line either.

3. **The build is broken.** Run `dotnet run`. It refuses. The same error from the hover box shows up in the terminal.

Change `readLine` back to `ReadLine`. Save. The squiggle disappears. The code suggestions come back. `dotnet run` works.

**Red means: this will not compile. Your program is broken until you fix it.** The compiler will not run code with a red squiggle. Ever.

Now for a different colour. Add a line near the top of `Program.cs`, just under the `var art = @"..."` block:

```csharp
var debug = 5;
```

Save. A **yellow** squiggle appears under `debug`. Hover it: *"The variable 'debug' is assigned but its value is never used."* That's a warning. The code compiles. `dotnet run` works. But the compiler is pointing out a line that does nothing, which is probably a mistake. Delete the line and the yellow goes away.

**Yellow means: this compiles, but I think you made a mistake.** Yellow squiggles don't stop the program from running. Most of them are worth reading, because they often point at bugs that haven't bitten you yet. Other things you'll see yellow for: code that can never run, a value the compiler thinks might be empty, or an import you don't use.

(There are also pale blue or grey "suggestions" — small style tips, the lowest priority. Skip them for now.)

**What you just learned.**

The editor expects nothing from you. It doesn't care whether you're in a hurry, whether your code *looks* like the example, or whether you'd like to be finished. It only knows what works and what doesn't — and it tells you, for free, the moment you type it.

When the code suggestions stop appearing — when you type a dot after a variable and nothing useful comes up — that's the same warning, just in a different form. The editor has lost track of your code. Something earlier is wrong.

Three rules from here on:

- **Red squiggle = stop and read the hover box.** Don't keep typing past it. It won't go away on its own.
- **Yellow squiggle = read it before you finish.** It probably won't block you, but it's usually worth knowing.
- **Code suggestions missing where you expected them = something earlier in the file is broken.** Scroll up, find the squiggle, fix it.

The compiler is the one thing in this whole course that expects nothing from you. It only has rules. Trust it more than you trust yourself.

## Tinker

Make the welcome message a different colour the *second* time the program runs (when the file already exists). You'll need to check `File.Exists("hero.txt")` *before* you write anything.

Save the player's score from Number Guess. Make Polish save the score and read it back on the next run, so the program remembers the best score so far.

Add several ASCII art banners and pick one at random each run. You already know how: `Random.Next` and an array of strings.

Use `Console.BackgroundColor` to make a banner with a coloured background. Reset both colours when you're done.

## Name it

**Files (a first look).** `File.WriteAllText("path", "content")` writes a string to a file. `File.ReadAllText("path")` reads a file back. `File.Exists("path")` checks whether a file is there. The path can be just a name (in the current folder) or a full path. Phase 2 covers files properly. Today is just enough to save and reload one small thing.

**The `Console` class.** `Console.WriteLine`, `Console.Write`, `Console.ReadLine`, `Console.ForegroundColor`, `Console.BackgroundColor`, `Console.ResetColor`, and `Console.Clear` are all methods and properties on the `Console` class. The Microsoft docs page for `System.Console` lists everything it can do.

**Writing a good README.** The README is the file someone reads when they first open your repo. The four sections that matter come next.

## README anatomy — four sections that matter

Every good project README has these four sections. Learn the layout by heart.

The **What** is one sentence. *"A small console game where you play a kingdom tycoon, written in C#."* If a stranger reads only that line, they should know what this is.

The **How to run** is the actual commands. *"Clone, then `dotnet run` in the project folder."* Don't make people guess.

The **What I learned** is the part where you write in your own words. What surprised you? What was hard? Not every README has this, but for a learning project it's the most valuable part.

The **What's next** is what you'd do if you kept going. *"Add more rooms. Save high scores. Write a v2 in JavaScript."* Even if "next" is "nothing", say so.

That's it. Four sections. About 30 to 50 lines in total. Most READMEs aren't this short. They should be.

## M0 milestone — *The Joke Toolbox*

You now have:

- `RoastOMatic/` (and v2 from Module 0.1)
- `NumberGuess/`
- `TinyAdventure/`
- `Polish/`

At the **top of your repo** (next to all four folders), create a `README.md` and write it yourself using the four sections above. Give each program two sentences: one *what*, one *how to run*. Then add a "what I learned" section for all four together (one paragraph). Then "what's next" — what would you add next?

## Commit your work

In VS Code's Source Control panel (`Ctrl + Shift + G G`):

1. Stage your changes — hover **Changes** and click `+`.
2. Commit message: *"M0: The Joke Toolbox - four toys + README"*.
3. Click the blue **checkmark** to commit.
4. Click **Sync Changes** to push to GitHub.

> **Or in the terminal:**
>
> ```powershell
> git add .
> git commit -m "M0: The Joke Toolbox - four toys + README"
> git push
> ```

## What you just did

You finished Spark Week. Four working programs in one repo, each one small but complete — random roasts, a guessing game, a text adventure, and a polish program that reads and writes a file. Five new ideas in eight modules: variables, loops, conditionals, lists, and your own methods. The repo has a real README. You have something to show. Most people who say *"I'm going to learn to code this year"* are still watching YouTube tutorials in week three. You have four programs on the internet.

**Key concepts you can now name:**

- **`File.WriteAllText` / `File.ReadAllText`** — save and load text
- **`Console` colours** — `ForegroundColor`, `ResetColor`
- **verbatim string** — `@"..."` keeps newlines literal
- **README anatomy** — what, how to run, what I learned, what's next
- **the Spark Week toolkit** — variables, loops, conditionals, lists, methods

## M0 close — the milestone steps

You just finished M0. Here are the steps to close it out.

1. **`journal/wins.md`** — open it in your repo and write one paragraph about M0 in your own words. What's in The Joke Toolbox, what was hardest, and what surprised you.
2. **`#wins` Slack post** — paste the link to your repo plus a screenshot of one program running. Add a one-line caption like *"M0 done — The Joke Toolbox."*
3. **Before-and-after line** — *"Four weeks ago I'd never written code. Today I finished four programs."* Say it out loud. Mean it.
4. **Tag the milestone.** This one is terminal-only — the Source Control panel doesn't have a button for tags:

   ```powershell
   git tag m0-spark-week-complete
   git push origin m0-spark-week-complete
   ```

Then take the rest of the day off. You've earned it.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 0.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage both files, commit message `Module 0.4 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Foundations starts next week. We finally give names to the things you've been using all month — types, methods, collections, and errors. After Foundations, you'll know enough C# to start building the real kingdom.
