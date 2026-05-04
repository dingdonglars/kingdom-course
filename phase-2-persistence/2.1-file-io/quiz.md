# Quiz — Module 2.1

## 1. Why use `Path.Combine("a", "b")` instead of `"a\\b"` or `"a/b"`?

a. It's faster
b. It picks the right separator for the OS at runtime — your code works on Windows, macOS, Linux
c. The compiler requires it
d. It encrypts the path

## 2. What does `Directory.CreateDirectory(folder)` do if the folder already exists?

a. Throws `IOException`
b. Nothing — it's idempotent (no-op when the folder exists)
c. Deletes and recreates
d. Returns `false`

## 3. What encoding does `File.WriteAllText(path, text)` use by default in modern .NET?

a. ASCII
b. UTF-8 without BOM
c. UTF-16
d. Whatever the OS prefers

## 4. Why use `Path.GetTempPath()` in tests instead of writing to the project folder?

a. It's faster
b. Temp files don't pollute the source tree, get cleaned up by the OS, and tests can run in parallel without colliding
c. It's a C# requirement
d. To hide the test files from git

## 5. The lesson says "the engine has zero changes this module." Why does that matter?

a. It proves the engine-vs-shell discipline works — disk is a shell concern
b. It saves typing
c. It's coincidence
d. Tests would otherwise break

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
