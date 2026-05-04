# Quiz — Module 5.3

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What is a metatable?

- **a.** A table that defines extra behaviour for *another* table — the heart of Lua's OOP-via-tables recipe
- **b.** A type of database table indexed by metadata
- **c.** A larger table used to store other tables
- **d.** A reserved Lua keyword for declaring objects

## 2. What does `__index = Building` do on a metatable?

- **a.** Tags the table with a comment for the editor
- **b.** Tells the metatable: when a key isn't found on the instance, look it up on `Building` — this is how methods get found
- **c.** Builds an index of the table's keys for faster lookup
- **d.** A piece of syntax Roblox requires on every ModuleScript

## 3. Why use the colon syntax `farm:upgrade()` instead of the dot `farm.upgrade()`?

- **a.** They behave identically — the colon is a stylistic preference
- **b.** The colon implicitly passes `self` as the first argument; the dot doesn't, so `farm.upgrade()` is missing `self` and throws
- **c.** The colon is the older syntax and will be removed in a future Luau version
- **d.** The colon is required when calling a function from inside a class

## 4. What is a ModuleScript in Roblox?

- **a.** A script that runs automatically when the place starts
- **b.** A script that defines a module — it doesn't run on its own; other scripts call `require` on it. Used for engine code, libraries, shared types.
- **c.** A LocalScript with a different name in the Explorer
- **d.** A folder for grouping related scripts together

## 5. The lesson says "OOP isn't `class` keywords." What does that mean here?

- **a.** OOP is grouping data with methods plus inheritance. Lua does it with tables and metatables instead of a `class` keyword. The pattern is the value, not the syntax.
- **b.** C# does object-orientation incorrectly compared to Lua's approach
- **c.** Lua doesn't really have object-orientation, only tables
- **d.** Whether or not to use OOP is a personal style preference

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
