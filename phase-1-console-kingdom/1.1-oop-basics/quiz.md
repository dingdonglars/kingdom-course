# Quiz — Module 1.1

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What's the difference between a *class* and an *object*?

- **a.** They're the same thing, two words for one idea
- **b.** A class is the blueprint; an object is one specific thing built from it with `new`
- **c.** A class is bigger than an object; objects are smaller pieces inside
- **d.** A class can have methods; an object can only hold values

## 2. What does `public int Level { get; private set; } = 1;` mean?

- **a.** `Level` is readable from outside; settable only from inside the class; defaults to 1
- **b.** `Level` is private; outside code can't read it
- **c.** `Level` is read-only and always stays at 1
- **d.** This is a syntax error and won't compile

## 3. What does the constructor `public Building(string name)` do?

- **a.** Defines a regular method that happens to share the class name
- **b.** Runs when you write `new Building("...")` and sets up the new object
- **c.** Returns a string named `name` to whoever called it
- **d.** Declares a property called `name` on every Building

## 4. Why is `Building.Name` read-only (no `set`)?

- **a.** Because Microsoft's style guide forbids settable strings
- **b.** Once a building is built, its name shouldn't drift — the class blocks accidental changes
- **c.** Because string properties don't support setters in C#
- **d.** It's a quirk of the .NET runtime; nothing meaningful

## 5. What's an enum used for?

- **a.** Counting how many of something you have in a list
- **b.** A fixed set of named values, so the compiler refuses anything outside the set
- **c.** Holding a list of strings that can grow at runtime
- **d.** Replacing classes when the data is too small for a class

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
