# Quiz answers — Module 1.6

## 1. b
`Where` is a *filter*. It returns a new `IEnumerable<T>` containing exactly the items the predicate matched. The original collection is unchanged. (And it's *lazy* — work only happens when you iterate or call `.ToList()` / `.Count()` etc.)

## 2. b
`First` throws `InvalidOperationException` if the sequence is empty (or no item matches the predicate). `FirstOrDefault` returns the default value of the type — `null` for reference types, `0` for `int`, `false` for `bool`. Use `FirstOrDefault` when "no match" is a normal case; use `First` when missing is a bug.

## 3. b
A predicate is just a function returning `bool`. `b => b.Level > 1` is a predicate that takes a building and returns whether its level exceeds 1. LINQ's `Where`, `Any`, `All`, `Count`, `First` all accept predicates.

## 4. a
`x => expr` is a *lambda expression* — a method written inline, no name needed. The compiler generates the actual method behind the scenes.

## 5. a
`Where(b => b is Farm)` returns `IEnumerable<Building>` — you can't call `Farm`-specific properties on the items without a cast. `OfType<Farm>()` returns `IEnumerable<Farm>` directly. Both are valid; `OfType` is usually more useful.