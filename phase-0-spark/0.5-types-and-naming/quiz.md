# Quiz — Module 0.5

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does `int gold = 5 / 2;` set `gold` to?

- **a.** `2.5` (the actual division result)
- **b.** `2` (integer division drops the fractional part)
- **c.** `3` (the result rounded to the nearest int)
- **d.** A compile error — `5 / 2` isn't a valid int expression

## 2. What does `(int)3.99` give you?

- **a.** `3` — the cast truncates toward zero
- **b.** `4` — the cast rounds to the nearest int
- **c.** A compile error — doubles can't be cast to ints
- **d.** `3.99` — casting a double to itself does nothing

## 3. Which of these lines compiles?

- **a.** `int x = "5";` — assign the string `"5"` to an int
- **b.** `bool ready = "true";` — assign the string `"true"` to a bool
- **c.** `string name = 42;` — assign the int `42` to a string
- **d.** `string? nickname = null;` — declare a nullable string set to null

## 4. By convention, how should a public class be named?

- **a.** `myClass` — camelCase, lowercase initial
- **b.** `MyClass` — PascalCase, uppercase initial
- **c.** `_myClass` — leading underscore, lowercase initial
- **d.** `MY_CLASS` — uppercase with underscores between words

## 5. Why is `_camelCase` (with the leading underscore) used for private fields?

- **a.** It's a Microsoft tradition with no practical reason behind it
- **b.** It marks them apart from local variables and parameters at a glance
- **c.** It makes them deliberately harder to read so they aren't used casually
- **d.** It's a syntactic requirement — the compiler needs the underscore

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
