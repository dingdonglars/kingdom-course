# Quiz — Module 2.1

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. Why use `Path.Combine("a", "b")` instead of `"a\\b"` or `"a/b"`?

- **a.** Faster than string concatenation at runtime
- **b.** Picks the right separator for the OS — your code works on Windows, Mac, Linux
- **c.** The C# compiler refuses string paths
- **d.** It encrypts the path before writing it

## 2. What does `Directory.CreateDirectory(folder)` do if the folder already exists?

- **a.** Throws `IOException` because the folder is in use
- **b.** Nothing — it's idempotent (no-op when the folder is there)
- **c.** Deletes the folder and recreates it from scratch
- **d.** Returns `false` and leaves the folder alone

## 3. What encoding does `File.WriteAllText(path, text)` use by default in modern .NET?

- **a.** ASCII (one byte per character)
- **b.** UTF-8 without BOM (the modern default)
- **c.** UTF-16 (the older Windows default)
- **d.** Whatever the operating system prefers

## 4. Why use `Path.GetTempPath()` in tests instead of writing to the project folder?

- **a.** The temp folder has faster disk access
- **b.** Temp files don't pollute the source tree, the OS cleans them up, and parallel test runs don't collide
- **c.** It's a C# language requirement for tests
- **d.** To hide the test files from git history

## 5. The lesson says *"the engine has zero changes this module."* Why does that matter?

- **a.** It proves the engine-vs-shell separation works — disk is a shell concern
- **b.** It saves typing during the lesson
- **c.** Coincidence; nothing to read into
- **d.** The tests would otherwise break in unexpected ways

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
