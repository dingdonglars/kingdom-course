# Quiz — Module 0.3

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. What does `new List<string>()` create?

- **a.** A new variable named `"List"` that holds a single string
- **b.** An empty list ready to hold strings
- **c.** A list whose first item is the literal word `"string"`
- **d.** An error — lists can't be created with `new` in C#

## 2. What does `inventory.Add("knife")` do to the list?

- **a.** Replaces every item in the list with `"knife"`
- **b.** Appends `"knife"` to the end of the list, growing it by one
- **c.** Removes `"knife"` from the list if it's there
- **d.** Returns `true` if the list already contains `"knife"`

## 3. What does `inventory.Contains("knife")` return?

- **a.** The position of `"knife"` in the list (or `-1` if missing)
- **b.** A copy of the item if found, otherwise an empty string
- **c.** `true` if `"knife"` is in the list, `false` if it isn't
- **d.** The whole list, with `"knife"` highlighted

## 4. Why is the adventure split into three methods (`Hallway`, `Kitchen`, `Library`)?

- **a.** Because three is the smallest count C# allows for a working program
- **b.** To save memory — methods take less RAM than `if` chains
- **c.** Because the structure of the game maps to the structure of the code; each method does one room
- **d.** The compiler refuses to handle a single method longer than fifty lines

## 5. When `Hallway()` calls `Kitchen()`, where does the program continue after `Kitchen()` finishes?

- **a.** At the very start of the program, restarting it
- **b.** Inside `Hallway()`, on the line right after the call to `Kitchen()`
- **c.** Wherever called `Hallway()` in the first place, skipping the rest of `Hallway`
- **d.** It depends on what the user typed

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
