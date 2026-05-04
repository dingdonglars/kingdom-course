# Quiz — Module 0.3

## 1. What does `new List<string>()` create?

a. A new string named "List"
b. An empty list that holds strings
c. A list with one item: the word "string"
d. An error — you can't `new` a list

## 2. What does `inventory.Add("knife")` do?

a. Replaces everything in `inventory` with `"knife"`
b. Appends `"knife"` to the end of `inventory`
c. Removes `"knife"` from `inventory`
d. Checks if `"knife"` is in `inventory`

## 3. What does `inventory.Contains("knife")` give back?

a. The position of `"knife"` in the list
b. The word `"knife"`
c. `true` if `"knife"` is in the list, `false` otherwise
d. Nothing

## 4. Why is the game split into three methods (`Hallway`, `Kitchen`, `Library`) instead of one giant method?

a. Because three is a magic number
b. Because organisation — each method does one thing, and the structure of the game maps to the structure of the code
c. C# requires it
d. To save memory

## 5. When `Hallway()` calls `Kitchen()`, where does the program go after `Kitchen()` finishes?

a. Back to wherever called `Hallway()`
b. Back into `Hallway()` right after the call to `Kitchen()`
c. To the start of the program
d. It depends

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
