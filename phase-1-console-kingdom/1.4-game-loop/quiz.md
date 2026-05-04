# Quiz — Module 1.4

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does the `virtual` keyword on `Building.Tick` do?

- **a.** Marks it as fake or not-yet-implemented
- **b.** Allows subclasses to replace it with their own version
- **c.** Makes it run on a virtual machine
- **d.** Makes it asynchronous

## 2. Why does `AdvanceDay()` return `void` instead of a new kingdom?

- **a.** C# forbids returning a custom class from a method
- **b.** It changes the kingdom in place — a side effect
- **c.** The compiler refuses `Kingdom` as a return type
- **d.** Returning anything from a tick method would be a syntax error

## 3. In `AdvanceDay()`, why do buildings tick *before* citizens eat?

- **a.** Alphabetical order — buildings come before citizens
- **b.** Performance — building loops are faster
- **c.** Order matters: today's farms produce before today's bites
- **d.** They tick simultaneously, so the order doesn't matter

## 4. What's a *side effect*?

- **a.** Anything that prints to the console
- **b.** A method that changes state instead of returning a value
- **c.** An unintended bug in your code
- **d.** A method with no return type

## 5. Why does `[Fact] Spend_NoFood_DoesNotCrash` exist?

- **a.** To verify the engine handles "no food, citizens still tick"
- **b.** To pad the test count
- **c.** To test `Spend`'s return value alone
- **d.** To verify `Add` works after a failed `Spend`

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
