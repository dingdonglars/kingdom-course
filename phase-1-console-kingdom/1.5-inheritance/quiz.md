# Quiz — Module 1.5

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does `: Building` in `class Farm : Building` mean?

- **a.** `Farm` *contains* a `Building` as a field on itself
- **b.** `Farm` *inherits from* `Building` — gets its fields and methods, can override `virtual` ones, can add more
- **c.** `Farm` is exactly the same class as `Building`, with a different name
- **d.** `Farm` replaces `Building` everywhere; `Building` no longer exists

## 2. What does `: base(name)` do in `public Farm(string name) : base(name) { }`?

- **a.** Calls `Building`'s constructor with the `name` argument so the parent's setup runs
- **b.** Renames the farm to whatever `name` says, every time
- **c.** Marks the constructor as part of a base class
- **d.** Nothing meaningful — it's optional decoration

## 3. Why is `override` required (and not optional) when replacing a `virtual` method?

- **a.** It isn't required; the compiler is happy without it
- **b.** So you can't replace a method by accident — typing `override` is a deliberate signal
- **c.** To make the code longer and slow down typing
- **d.** It's a recent addition to the language and replaces `new`

## 4. What does `b.GetType().Name` return for `b = new Farm("Main Farm")`?

- **a.** `"Main Farm"`
- **b.** `"Building"`
- **c.** `"Farm"`
- **d.** `"object"`

## 5. The lesson says *"prefer composition over inheritance."* Why?

- **a.** Composition runs faster at runtime than inheritance does
- **b.** Inheritance is forbidden in modern C# and the compiler warns on it
- **c.** Long inheritance chains become rigid; composition (a class *contains* another) flexes more easily. One level deep is fine.
- **d.** Composition uses less memory than inheritance does

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
