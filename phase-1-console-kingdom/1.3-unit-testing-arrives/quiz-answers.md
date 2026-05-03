# Quiz answers — Module 1.3

## 1. b

`[Fact]` marks a single test method that runs once. `[Theory]` is for *parameterised* tests — the same logic runs once for each `[InlineData(...)]`. Use `[Theory]` when you want to verify the same behavior across multiple inputs without duplicating code.

## 2. c

Shouldly's `ShouldBe` checks the value. If it doesn't match, it throws a `ShouldAssertException` with a clear message ("Expected `Level` to be 2 but was 5"), and xUnit reports the test as failed. The test framework then continues with the next test.

## 3. b

Arrange / Act / Assert: set up the state (arrange), do the operation (act), verify the outcome (assert). It's a discipline that keeps tests readable. Some test frameworks (Given/When/Then) phrase it differently but the structure is the same.

## 4. b

The engine is the *reusable* part. It will be used by the console shell (this phase), the web API shell (Phase 3), the browser shell (Phase 4), the Roblox shell (Phase 5). Testing the engine once verifies the rules across all of them. Testing the console shell would verify how it formats output — useful, but a smaller payoff for the test investment.

## 5. b

Method underscore Scenario underscore ExpectedBehavior. `Spend_WhenInsufficient_ReturnsFalse`: the method being tested is `Spend`, the scenario is "when the ledger has insufficient funds", and the expected behavior is "returns `false`". This convention (from `STANDARDS.md`) means test names read as documentation.