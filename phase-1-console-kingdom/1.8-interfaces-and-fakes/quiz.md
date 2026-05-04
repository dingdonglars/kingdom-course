# Quiz — Module 1.8

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What's an interface?

- **a.** A graphical layer used to draw the game's user interface
- **b.** A contract — method and property shapes with no bodies. Many classes can implement it.
- **c.** The opposite of a class — interfaces hold data, classes hold behaviour
- **d.** A Java-only feature that doesn't exist in modern C#

## 2. What does *dependency injection* mean in this lesson?

- **a.** Inserting code into a running program after compile time
- **b.** Passing collaborators (`IRandom`, `IClock`) in via the constructor instead of newing them up inside
- **c.** Using a third-party DI framework like Microsoft.Extensions.DependencyInjection
- **d.** Hot-loading a class assembly while the program runs

## 3. What does `A.CallTo(() => rng.NextDouble()).Returns(0.1)` do?

- **a.** Calls `NextDouble()` once on a real `Random` instance
- **b.** Tells the fake `rng` that whenever any code calls `rng.NextDouble()`, return `0.1`
- **c.** Throws an exception if `NextDouble()` is called more than once
- **d.** Records the call without changing what `rng` returns

## 4. Why does `Kingdom` keep a no-arg `Kingdom(string name)` constructor that chains to the new one?

- **a.** Backward compatibility — older tests use `new Kingdom("Test")` and would otherwise break
- **b.** To save typing in `Program.cs`
- **c.** Because C# requires every class to have a no-arg constructor
- **d.** It was added by accident and could be removed without consequence

## 5. The lesson says *"every external dependency comes in through an interface."* What does that buy you?

- **a.** Faster code at runtime, because interfaces dispatch is cheaper than direct calls
- **b.** The same engine runs across console, web, browser, Roblox — each shell wires its own implementations. Tests swap fakes for real ones.
- **c.** Smaller compiled binaries because interfaces avoid duplication
- **d.** Nothing concrete — it's a stylistic preference with no real benefit

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
