# Quiz — Module 1.6

## 1. What does `.Where(b => b.Level > 1)` return?

a. A `bool`
b. A *new* sequence containing only the items whose `Level > 1`
c. The first item with `Level > 1`
d. `null`

## 2. What's the difference between `.First(...)` and `.FirstOrDefault(...)`?

a. They're identical
b. `First` throws if no item matches; `FirstOrDefault` returns `default(T)` (e.g., `null` for reference types, `0` for `int`)
c. `First` is faster
d. `FirstOrDefault` doesn't accept a predicate

## 3. What is a *predicate*?

a. A class
b. A function that returns `bool`
c. A keyword
d. A namespace

## 4. The `b => b.Level > 1` syntax is called a...

a. Lambda expression — a throwaway function written inline
b. Generic constraint
c. Anonymous class
d. Async method

## 5. Why is `.OfType<Farm>()` cleaner than `.Where(b => b is Farm)`?

a. It's not — they're equivalent in behavior, but `OfType<Farm>` *also* returns the items typed as `Farm` (not `Building`), so you can call Farm-specific methods without a cast
b. It's faster
c. `Where` is deprecated
d. They produce different results