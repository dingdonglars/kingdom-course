# Quiz answers — Module 0.7

## 1. b

`List<T>` is an ordered, indexed-by-integer collection. You access by position: `list[0]`. It allows duplicates.
`Dictionary<K, V>` is a key-value lookup. You access by key: `dict["gold"]`. Each key appears at most once.

Different tools for different jobs.

## 2. b

`GetValueOrDefault(key, fallback)` is the safe-lookup pattern: if `key` is in the dictionary, return its value; otherwise return the fallback. It does NOT add the key to the dictionary. Useful when you want a default for missing keys without writing an `if (dict.ContainsKey(...))` check.

## 3. a

`foreach (var x in list)` walks the collection in order, binding each element in turn to `x`. The body runs once per element. You don't get the index (use `for` for that). You can't modify the collection inside the loop (will throw).

## 4. b

LINQ's `OrderBy` returns a *new sequence* — it does not modify the original. The returned sequence is `IEnumerable<KeyValuePair<...>>`; you can `.ToList()` it or `foreach` over it directly. Dictionaries are themselves *unordered* (their iteration order is implementation-defined), so `OrderBy` is how you get a stable display.

## 5. b

A `List<string>` would record `["apple", "apple", "banana"]` — three entries, hard to count. A `Dictionary<string, int>` records `{"apple": 2, "banana": 1}` — counts naturally. The right collection makes the code shorter and the meaning clearer.