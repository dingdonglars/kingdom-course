# Quiz — Module 2.10

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. The *menu loop* pattern is...

- **a.** `while (true) { print menu; read input; dispatch; }` — the heart of every interactive UI
- **b.** A C# library called `Menu.Loop`
- **c.** A specific kind of LINQ query
- **d.** Nothing standard; just a phrase used informally

## 2. Why use `int.TryParse(raw, out var id)` instead of `int.Parse(raw)`?

- **a.** They are functionally identical and either works
- **b.** `TryParse` returns `bool` for success; `Parse` throws `FormatException` on bad input
- **c.** `TryParse` runs faster on every input
- **d.** `int.Parse` was deprecated in .NET 8

## 3. What does `Console.ReadLine()` return when the input stream is exhausted (EOF)?

- **a.** An empty string ("")
- **b.** `null`
- **c.** Throws `EndOfStreamException`
- **d.** The previous line read (last value cached)

## 4. Why does the test redirect `Console.In` and `Console.Out`?

- **a.** To hide test output from the developer running the suite
- **b.** To script user input and capture output so an interactive UI can be tested without a human at the keyboard
- **c.** To run the test about three times faster
- **d.** Required by xUnit for any test that touches `Console`

## 5. The lesson notes that `Program.cs` is now about five lines while engine + store are around 500. Why is that ratio healthy?

- **a.** The runtime does interaction; the engine does logic; the store does saving — each layer is thin and focused
- **b.** Smaller `Program.cs` files run faster than larger ones
- **c.** Tradition that all .NET console projects follow
- **d.** .NET refuses to build a `Program.cs` larger than 500 lines

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
