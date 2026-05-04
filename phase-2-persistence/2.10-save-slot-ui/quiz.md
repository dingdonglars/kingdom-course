# Quiz — Module 2.10

## 1. The "menu loop" pattern is...

a. `while (true) { print menu; read input; dispatch; }` — the heart of every interactive shell
b. A C# library
c. A LINQ query
d. Nothing standard

## 2. Why use `int.TryParse(raw, out var id)` instead of `int.Parse(raw)`?

a. They're identical
b. `TryParse` returns `bool` for success without throwing — perfect for unvalidated user input. `Parse` throws on bad input.
c. `TryParse` is faster
d. Required by .NET

## 3. What does `Console.ReadLine()` return when the input stream is exhausted (EOF)?

a. An empty string
b. `null`
c. Throws an exception
d. The previous line

## 4. Why does the test redirect `Console.In` and `Console.Out`?

a. To hide test output from the user
b. To script user input and capture output, so an interactive UI can be tested without a human at the keyboard
c. To speed up the tests
d. Required by xUnit

## 5. The lesson says `Program.cs` is now ~5 lines, while engine + store are ~500. Why is that ratio healthy?

a. The shell does interaction; the engine does logic; the store does persistence. Each layer is thin and focused. A shell that grows past ~50 lines is doing too much.
b. Smaller `Program.cs` runs faster
c. Tradition
d. Required by .NET