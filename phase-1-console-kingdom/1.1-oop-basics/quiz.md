# Quiz — Module 1.1

## 1. What's the difference between a *class* and an *object*?

a. They're the same thing
b. A class is the blueprint; an object is an instance created from the blueprint with `new`
c. A class is bigger; an object is smaller
d. A class can have methods; an object can't

## 2. What does `public int Level { get; private set; } = 1;` mean?

a. `Level` is readable from outside; settable only from inside the class; defaults to 1
b. `Level` is private (not readable from outside)
c. `Level` is read-only and always 1
d. There's an error in this line

## 3. What does the constructor `public Building(string name)` do?

a. Defines a new method called `Building`
b. Runs when you create a new `Building` with `new Building("...")` — initialises the object
c. Returns a string named `name`
d. Declares a property called `name`

## 4. Why is `Building.Name` read-only (no `set`)?

a. Because Microsoft requires it
b. Because once a building is built, its name shouldn't change — encapsulation prevents accidental mutation
c. Because strings can't be set
d. There's no good reason

## 5. What's an enum used for?

a. Counting things
b. A named set of fixed allowed values, so the compiler stops you from passing junk
c. A list of strings
d. Replacing classes

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
