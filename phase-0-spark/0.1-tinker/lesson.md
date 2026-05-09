# Module 0.1 — Tinker (Roast-O-Matic v2)

Last time, Roast-O-Matic shouted the same three roasts at nobody in particular. Today it asks for your friend's name and roasts them by name. *"Hey BOB — your password is 'password'."* Same code as last time, two new lines. The point of today is small: you're going to learn how to ask the user a question, store what they typed, and stitch it into a string.

> **Words to watch**
>
> - **variable** — a named place to store a piece of data so you can use it later
> - **string** — a piece of text in code, written in `"double quotes"`
> - **method** — a named chunk of code that does one thing; `Console.WriteLine` is a method
> - **`Console.ReadLine()`** — the method that asks the user to type something and gives you back what they typed
> - **string interpolation** — the `$"..."` syntax that lets you stick a variable inside a string

---

## Step 1 — ask for a name

Open your `RoastOMatic` folder in VS Code. Above the `string[] roasts = ...` line, add this:

```csharp
Console.Write("Who do you want to roast? ");
var name = Console.ReadLine();
```

`Console.Write` prints without moving to the next line — the cursor stays right after the question mark, so the answer types in next to the prompt. `Console.ReadLine()` waits for the user to type a line and press Enter, then gives you back whatever they typed as a string. We store that string in a variable called `name`.

## Step 2 — use the name in the roast

Change the last line so the roast uses the name:

```csharp
Console.WriteLine($"Hey {name.ToUpper()} — {roast}");
```

The `$` in front of the string is **string interpolation**. Anything inside `{curly braces}` is treated as real C# — it gets evaluated, turned into text, and dropped into the string. So `$"Hey {name.ToUpper()}"` becomes `Hey BOB` if `name` is `"bob"`. Beats jamming things together with `+`.

Run it:

```powershell
dotnet run
```

Type a name. Hit enter. Get a personalised roast.

## Tinker

Try two names per run — `Console.ReadLine()` can be called twice. Each call waits for its own line of input.

Make Roast-O-Matic ask for what *kind* of roast they want (mild, spicy, nuclear) and pick a roast list based on the answer.

Add a roast that uses two names — *"Hey BOB — at least you're not as bad as ALICE."* Use string interpolation with two placeholders: `$"... {name1} ... {name2} ..."`.

## Name it

You used four ideas today; they have names worth knowing.

A **variable** is a labelled spot in memory holding a value. The `var` keyword tells C# *"figure out the type from what I'm assigning."* So `var name = Console.ReadLine();` declares a variable called `name`, and C# works out from `ReadLine()` that the type is `string`.

A **string** is anything in `"double quotes"` — a piece of text. C# treats strings as a real type; later you'll see `string` written explicitly in front of variable names.

A **method** is a named chunk of code you call by writing its name plus `()`. Methods often take something inside the parens (the *argument*) and give back something (the *return value*). `ReadLine()` takes nothing and gives back a string. `WriteLine(...)` takes a string and gives back nothing.

**String interpolation** is the `$"Hey {name}"` syntax — a string with placeholders that get replaced at runtime. The cleanest way to build text from values you already have.

## What you just did

You made your program ask a question and listen for the answer. Two extra lines turned a one-shot script into a tiny conversation: prompt, read, use the answer. You met four named ideas — variable, string, method, string interpolation — that turn up in every program you'll write from here on. The roast lines are the same as yesterday; the program reads completely differently. That's what variables buy you.

**Key concepts you can now name:**

- **variable** — labelled storage for a value
- **string** — text in double quotes
- **method** — named code you call with `()`
- **string interpolation** — `$"..."` with `{placeholders}`
- **`Console.ReadLine`** — wait for input, return a string

## Wrap up

A short routine at the end of every lesson from now on. Three things, one commit, on your phase branch:

1. **Quiz** — open `quiz.md`. Jot your answers and a sentence of reasoning in `journal/quiz-notes.md`, same layout as the entries already in there. Bring whichever you're least sure about to the next weekly sync.

2. **One line of progress** — in `journal/progress.md`. (If the file isn't there yet, copy `kingdom-course/starter-template/journal/progress.md` into your `kingdom/journal/` folder first — that gives you the format and a worked example.) Add a new line below the example:

   ```
   Module 0.1 — Tinker — 2026-MM-DD — added input + interpolation. Learnt: variables let the same code mean two different things.
   ```

   The "Learnt" half is the bit worth thinking about. One sentence, your own words.

3. **Commit and push.**

   - First, **make sure you're in the right repo.** Open Source Control (`Ctrl + Shift + G G`) — the panel header must say **`kingdom`** (the folder is `C:\code\kingdom`), *not* `kingdom-course`. Two folders, two roles, remember from the primer — your work goes in `kingdom`. The `kingdom-course` folder is read-only; nothing you write goes there. **Get this right every single time** — committing into `kingdom-course` is the easiest mistake to make in the first weeks, and it's annoying to undo.
   - Stage `journal/quiz-notes.md` and `journal/progress.md` (click the `+` next to each).
   - Commit message: `Module 0.1 done`.
   - Click the checkmark, then Sync.

   > **Or in the terminal** — same folder rule. Run `pwd` first; it should print `C:\code\kingdom`. If it prints `C:\code\kingdom-course`, `cd ..\kingdom` before going further.
   > ```powershell
   > git add journal/quiz-notes.md journal/progress.md
   > git commit -m "Module 0.1 done"
   > git push
   > ```

Then post in `#wins` — one line about today, plus the URL of the commit you just pushed. (Open your `kingdom` repo on github.com, click *Commits*, click the latest one, copy the URL from the address bar.)

The point isn't paperwork. It's a tiny visible trace that you did the lesson — proof for you, signal for Lars.

## Next

Module 0.2 brings randomness on purpose — your first guessing game, where the program picks a number and you try to find it.
