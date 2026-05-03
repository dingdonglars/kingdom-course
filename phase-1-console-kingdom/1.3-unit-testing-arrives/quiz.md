# Quiz — Module 1.3

## 1. What's the difference between `[Fact]` and `[Theory]`?

a. They're identical
b. `[Fact]` is a single test; `[Theory]` runs the same test logic with multiple input sets supplied via `[InlineData]`
c. `[Fact]` is for true things; `[Theory]` is for hypotheses
d. `[Theory]` doesn't actually run

## 2. What does `b.Level.ShouldBe(2)` do if `b.Level` is `5`?

a. Sets `b.Level` to `2`
b. Returns `false`
c. Throws an assertion exception, marking the test as failed
d. Logs a warning

## 3. What's the conventional structure of a unit test?

a. Setup / Teardown / Assert
b. Arrange / Act / Assert
c. Build / Test / Deploy
d. Try / Catch / Finally

## 4. Why test the engine and not the shell (Console)?

a. The shell is too small to test
b. Engine code has the business rules; testing it once verifies them across all future shells (web, browser, Roblox)
c. The shell can't be tested
d. There's no good reason

## 5. What does the test name `Spend_WhenInsufficient_ReturnsFalse` tell you?

a. Nothing — it's just a name
b. Method (`Spend`), scenario (insufficient funds), expected behavior (returns false). The convention from `STANDARDS.md`.
c. The test will spend something
d. The test was written by someone named "Spend"