# Quiz — Module 1.5

## 1. What does `: Building` in `class Farm : Building` mean?

a. `Farm` *contains* a `Building`
b. `Farm` *inherits from* `Building` — it gets all of `Building`'s fields/methods, can override any `virtual` ones, can add more
c. `Farm` is the same as `Building`
d. `Farm` replaces `Building`

## 2. What does `: base(name)` do in the constructor `public Farm(string name) : base(name) { }`?

a. Calls `Building`'s constructor with the name argument
b. Renames the farm
c. Creates a base class
d. Nothing — it's optional

## 3. Why is `override` required (and not optional) when replacing a `virtual` method?

a. It isn't required
b. So you can't override a method by accident — typing `override` is a deliberate signal
c. To make the code longer
d. It's a bug in the compiler

## 4. What does `b.GetType().Name` return for `b = new Farm("Main Farm")`?

a. `"Main Farm"`
b. `"Building"`
c. `"Farm"`
d. `"object"`

## 5. The lesson says *"prefer composition over inheritance."* Why?

a. Composition is faster
b. Inheritance is forbidden in modern C#
c. Deep inheritance trees become rigid and hard to refactor; composition (a class *contains* another) is more flexible. For one level deep (like `Building → Farm`), inheritance is fine.
d. Composition uses less memory

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
