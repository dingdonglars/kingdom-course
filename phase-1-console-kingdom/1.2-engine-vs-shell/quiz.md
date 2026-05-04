# Quiz — Module 1.2

## 1. What's the difference between a class library and a console app project?

a. Class libraries are smaller
b. Console apps have a `Main` method and produce a runnable `.exe`; class libraries have no `Main` and produce a `.dll` that other projects use
c. Class libraries can't have classes
d. They're the same; just different file extensions

## 2. Which depends on which?

a. The engine depends on the console
b. The console depends on the engine
c. They both depend on each other
d. Neither depends on the other

## 3. Why is `Console.WriteLine` allowed in `Kingdom.Console.Program.cs` but not in `Kingdom.Engine.Kingdom.cs`?

a. It's not allowed in either
b. The engine is meant to be runtime-agnostic — `Console.WriteLine` would tie the engine to the console runtime
c. There's no real reason; it's just convention
d. Performance

## 4. What does `<ProjectReference Include="..\Kingdom.Engine\Kingdom.Engine.csproj" />` do?

a. Copies engine code into console project
b. Tells the console project that it depends on the engine project
c. Adds the engine to source control
d. Renames the engine project

## 5. The engine project has no `OutputType`. What does it default to?

a. `Exe`
b. `Library` (a .dll)
c. `WinExe`
d. The build fails

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
