# Quiz — Module 0.8

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does `try { A } catch (Exception ex) { B }` do?

- **a.** Always runs `B` first to set things up, then runs `A`
- **b.** Runs `A`; if `A` throws an exception, `B` runs instead and execution continues after
- **c.** Runs `B` regardless of whether `A` succeeded or threw
- **d.** Runs `A` and crashes if anything inside `A` throws an exception

## 2. Why catch specific exception types (`IOException`) before catching the generic `Exception`?

- **a.** The C# compiler refuses to build code that catches `Exception` first
- **b.** Specific catches run faster than generic ones at runtime
- **c.** Catches are tested top-to-bottom; specific first lets each problem get its own handler
- **d.** It's a stylistic preference with no actual impact on behaviour

## 3. What is a breakpoint in the debugger?

- **a.** A line where the program is guaranteed to crash at runtime
- **b.** A marked line where the debugger pauses execution so you can inspect state
- **c.** The end of every method, marking where the debugger naturally stops
- **d.** A `try`/`catch` boundary that the debugger respects automatically

## 4. In the VS Code debugger, what does `F10` (Step Over) do?

- **a.** Runs the current line and moves to the next line in the same method
- **b.** Dives into the method called on the current line, line by line
- **c.** Stops the debugger and ends the program immediately
- **d.** Restarts the program from the top with the same inputs

## 5. What is the call stack?

- **a.** A pile of unread error messages waiting to be shown to the user
- **b.** The chain of method calls that led from `Main` to the current line
- **c.** A list of every breakpoint set in the current debugging session
- **d.** A history of every key the user pressed since the program started

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
