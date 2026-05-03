# Code Review (Don't Write the Fix)

Use when: you want Claude to find issues in your code, but **not** write the fix. Yellow bucket — friction help, not learning replacement.

## Template

> Review the following `<language>` code. Point out issues (bugs, style, naming, structure, edge cases). For each issue, say *what's wrong* and *why* — but **do not write the corrected code**. I'll fix it myself; reviewing is the lesson.
>
> ```
> <paste code>
> ```

## Example

> Review the following C# code. Point out issues (bugs, style, naming, structure, edge cases). For each issue, say *what's wrong* and *why* — but **do not write the corrected code**. I'll fix it myself; reviewing is the lesson.
>
> ```csharp
> public class Building {
>   public string n;
>   public int g;
>   public bool Up() {
>     if (g < 100) return false;
>     g -= 100;
>     return true;
>   }
> }
> ```

## Why this prompt works

- The instruction "don't write the fix" prevents the yellow bucket from drifting into red.
- "What's wrong and why" forces Claude to teach, not type.
- You learn by *fixing*, not by *reading the fix*.

## What you do with the response

For each issue Claude raises:
1. Decide: do you understand it?
2. If yes — fix it.
3. If no — ask Claude *to explain the issue further*, still without writing the fix.
4. After fixing, re-run `code-review.md` and see if there's anything left.
