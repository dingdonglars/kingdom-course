# Quiz — Module 0.7

## 1. What's the difference between `List<T>` and `Dictionary<K, V>`?

a. They're the same; just different syntax
b. `List<T>` is ordered and indexed by integer position; `Dictionary<K, V>` is indexed by key
c. `List<T>` is faster
d. `Dictionary<K, V>` can only hold strings

## 2. What does `inventory.GetValueOrDefault("apple", 0)` do?

a. Throws if `"apple"` is not a key
b. Returns the value for `"apple"` if it's a key, otherwise returns `0`
c. Sets `"apple"` to `0` and returns `0`
d. Always returns `0`

## 3. What does `foreach (var x in list)` do?

a. Loops through `list` in order, putting each item in turn into `x`
b. Loops backward through `list`
c. Sorts `list` and then loops
d. Modifies `list`

## 4. What does `inventory.OrderBy(kvp => kvp.Key)` give back?

a. The original dictionary, sorted in place
b. A new sequence of key-value pairs sorted by key
c. Just the keys
d. An error

## 5. Why use a `Dictionary<string, int>` for the Inventory Tool instead of `List<string>`?

a. They're equivalent
b. To track counts naturally — same item added twice becomes count 2, not two entries
c. Dictionaries are always faster
d. To avoid duplicates entirely

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
