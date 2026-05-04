# Quiz — Module 1.2

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What's the difference between a class library and a console app project?

- **a.** Class libraries are smaller and meant for utilities only
- **b.** Console apps have a `Main` and produce an `.exe`; class libraries have no `Main` and produce a `.dll` that other projects use
- **c.** Class libraries can hold methods but not classes
- **d.** They're the same; only the file extension differs

## 2. Which depends on which?

- **a.** The engine depends on the console
- **b.** The console depends on the engine
- **c.** They both depend on each other equally
- **d.** Neither — they're built independently and linked at runtime

## 3. Why is `Console.WriteLine` allowed in `Kingdom.Console.Program.cs` but not in `Kingdom.Engine.Kingdom.cs`?

- **a.** It isn't allowed in either; both should use a logger
- **b.** The engine is meant to work from any runtime — `Console.WriteLine` would tie it to the console
- **c.** There's no real reason; it's just a style preference
- **d.** Performance — `Console.WriteLine` is too slow for engine code

## 4. What does `<ProjectReference Include="..\Kingdom.Engine\Kingdom.Engine.csproj" />` do?

- **a.** Copies the engine's source files into the console project at build time
- **b.** Tells the console project that it depends on the engine project
- **c.** Adds the engine to source control alongside the console
- **d.** Renames the engine project to match the console

## 5. The engine project has no `OutputType`. What does it default to?

- **a.** `Exe` — every project produces an executable by default
- **b.** `Library` — it produces a `.dll` that other projects use
- **c.** `WinExe` — a Windows-specific executable
- **d.** Nothing — the build fails until `OutputType` is set

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
