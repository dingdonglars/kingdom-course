# Quiz — Module 1.6

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does `.Where(b => b.Level > 1)` return?

- **a.** A `bool` saying whether any item matched the predicate
- **b.** A new sequence holding only the items whose `Level > 1`
- **c.** The first item with `Level > 1`, or `null` if none
- **d.** `null` always; `Where` is a side-effect method

## 2. What's the difference between `.First(...)` and `.FirstOrDefault(...)`?

- **a.** They behave identically; the second name is just a deprecated alias
- **b.** `First` throws if no item matches; `FirstOrDefault` returns `default(T)` (e.g. `null`, `0`)
- **c.** `First` is faster on big lists; `FirstOrDefault` is slower
- **d.** `FirstOrDefault` doesn't accept a predicate; `First` does

## 3. What is a *predicate*?

- **a.** A class that holds rules about a collection
- **b.** A function that returns `bool` — the kind of function `Where` and `Any` accept
- **c.** A C# keyword reserved for query expressions
- **d.** A namespace that contains the LINQ methods

## 4. The `b => b.Level > 1` syntax is called a...

- **a.** Lambda expression — a function written inline, no name needed
- **b.** Generic constraint that limits which types can be passed
- **c.** Anonymous class with one method on it
- **d.** Async method declared with the arrow syntax

## 5. Why is `.OfType<Farm>()` often nicer than `.Where(b => b is Farm)`?

- **a.** They're equivalent in result, but `OfType<Farm>` returns the items typed as `Farm`, so you can use Farm-only members without a cast
- **b.** `OfType<Farm>` is faster at runtime; `Where(b is Farm)` walks the list twice
- **c.** `Where` is deprecated and should be avoided
- **d.** They produce different results — `OfType<Farm>` includes subclasses of Farm; `Where(b is Farm)` doesn't

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
