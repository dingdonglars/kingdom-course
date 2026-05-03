# Quiz answers — Module 0.4

## 1. b

`Console.ForegroundColor` is a *property* (think: a remembered setting) that controls the color of text written by `Console.Write` / `Console.WriteLine`. Once set, it stays set until you change it again or call `Console.ResetColor()`. Older console programs forgot to reset and left the user's terminal in weird colors — don't be that program.

## 2. c

`ResetColor()` puts both the foreground and background colors back to their defaults (whatever they were when your program started). Use it after every color change so you don't leave the terminal in a stuck state if your program crashes.

## 3. b

`File.WriteAllText(path, content)` writes the entire `content` string to the file at `path`. If the file doesn't exist, it's created. If it exists, it's *overwritten* — no append, no warning. Be careful with this one in real apps; for a toy that's fine.

## 4. b

The path `"hero.txt"` (no folder prefix) is *relative* — it's resolved against the program's current working directory. When you `dotnet run` inside `Polish/`, that's the project's `bin/Debug/net10.0/` folder, where the program is actually executed. (Not always intuitive — Phase 2 covers paths properly.)

## 5. c

The four sections that matter: **What**, **How to run**, **What I learned** (or "Why this exists"), **What's next**. Detailed API documentation belongs in dedicated docs (or in the code itself), not in the README. READMEs are *introductions*; they should fit on one screen.
</content>
</invoke>