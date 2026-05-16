# Quiz — Module 0.7

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. What's the practical difference between `List<T>` and `Dictionary<K, V>`?

- **a.** They're the same thing — different syntax, identical behaviour at runtime
- **b.** `List<T>` is ordered and indexed by integer position; `Dictionary<K, V>` is indexed by key
- **c.** `List<T>` only holds numbers; `Dictionary<K, V>` only holds strings
- **d.** `List<T>` runs faster for every operation; `Dictionary<K, V>` is for tiny sets only

## 2. What does `inventory.GetValueOrDefault("apple", 0)` do?

- **a.** Adds `"apple"` to the dictionary with value `0` and returns `0`
- **b.** Returns the value for `"apple"` if it's a key, otherwise returns the fallback `0`
- **c.** Throws an exception when `"apple"` is not already a key
- **d.** Returns `0` regardless of whether `"apple"` is in the dictionary

## 3. What does `foreach (var x in list)` do?

- **a.** Walks the list in order, putting each element into `x` one at a time
- **b.** Walks the list in reverse order, last element first
- **c.** Sorts the list alphabetically and then walks the sorted result
- **d.** Modifies the list, removing each item as it visits it

## 4. What does `inventory.OrderBy(kvp => kvp.Key)` give back?

- **a.** The dictionary itself, sorted in place by key
- **b.** A new sequence of key-value pairs sorted by key (original is untouched)
- **c.** A list of just the keys, with the values discarded
- **d.** A compile error — dictionaries can't be sorted with LINQ

## 5. Why use `Dictionary<string, int>` for the Inventory Tool instead of `List<string>`?

- **a.** Dictionaries always run faster than lists, so they're the default choice
- **b.** A dictionary tracks counts naturally — same item added twice becomes count 2
- **c.** A list refuses duplicates, so it can't represent multiple of the same item
- **d.** They're equivalent for this case; the choice is purely stylistic

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
