# Quiz — Module 0.4

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does `Console.ForegroundColor = ConsoleColor.Yellow;` do?

- **a.** Repaints the entire console window background yellow until the program ends
- **b.** Sets the colour of all text written from now on to yellow until you change it again
- **c.** Highlights the cursor in yellow but leaves the text colour alone
- **d.** Throws an exception unless the terminal driver explicitly supports yellow

## 2. What does `Console.ResetColor()` do?

- **a.** Resets the entire console application to factory defaults, including font and size
- **b.** Clears the visible console output, like `Console.Clear()` would
- **c.** Restores the foreground and background colours to the terminal's defaults
- **d.** Resets the cursor position to the top-left of the console window

## 3. What does `File.WriteAllText("hero.txt", name)` do?

- **a.** Reads the contents of `hero.txt` into the variable `name`, replacing what was there
- **b.** Writes whatever is in `name` to a file called `hero.txt`, creating it or overwriting it
- **c.** Appends `name` to the end of `hero.txt`, leaving any existing content intact
- **d.** Throws an error unless `hero.txt` already exists in the current folder

## 4. Where does `hero.txt` end up on disk?

- **a.** Always in the user's Windows Documents folder, regardless of where the program runs
- **b.** In the program's current working directory (usually the project folder when you `dotnet run`)
- **c.** On the Windows desktop, ready for the user to find and double-click
- **d.** In a hidden system location only administrators can read

## 5. The README anatomy from this lesson has four sections. Which is NOT one of them?

- **a.** What — one sentence saying what the project is
- **b.** How to run — the actual commands a stranger would type
- **c.** Detailed API reference for every public method in the project
- **d.** What's next — what you'd add if you kept going

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
