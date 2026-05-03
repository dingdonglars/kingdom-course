# Quiz — Module 1.7

## 1. What's a `record` in C#?

a. A music recording
b. A small data class with auto-generated value equality, ToString, and immutability — perfect for events
c. A keyword for database tables
d. A type of comment

## 2. Why is `EventEngine` newing up `Random` directly *bad*?

a. `Random` is slow
b. The engine is now non-deterministic and untestable — you can't write "given dice X, expect event Y"
c. `Random` is deprecated
d. It uses too much memory

## 3. What does the `_` underscore in `_ => null` (the switch) mean?

a. A discard variable
b. The "anything else" case — matches whatever didn't match above
c. Both a and b
d. A typo

## 4. What does `1 when k.Citizens.Count > 0` mean in a switch?

a. The pattern `1`, but only if `k.Citizens.Count > 0`. Otherwise the next pattern is checked.
b. Always match `1`
c. A C# 12 feature only
d. A bug

## 5. Why are the tests in `EventLogTests.cs` so vague ("some events happen")?

a. They were lazy
b. The engine is non-deterministic — we can't make precise assertions, only loose ones. Module 1.8 introduces `IRandom` to fix this.
c. The test framework is limited
d. xUnit doesn't support specific assertions