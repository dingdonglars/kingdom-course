# Quiz answers — Module 1.8

## 1. b
An interface is a *contract* — a list of method/property shapes with no implementation. Multiple classes can implement the same interface; the calling code only needs to know the interface, not which class is behind it.

## 2. b
Dependency injection (DI) means *"the dependencies come in from outside, not from inside."* `EventEngine` doesn't `new Random()` — it accepts an `IRandom` in its constructor. The shell decides which `IRandom` to pass (real, seeded, or a fake).

## 3. b
FakeItEasy syntax: *"when someone calls `NextDouble()` on the fake `rng`, return `0.1` instead of doing whatever the real implementation would do."* This is the entire point of fakes — surgical control over collaborators in tests.

## 4. a
Existing tests (`BuildingTests`, `ResourceLedgerTests`, `KingdomTickTests`, `LinqTests`, `EventLogTests`) all do `new Kingdom("Test")`. Removing the no-arg constructor would break every one. The chain `: this(name, new SystemRandom(), new SystemClock())` lets old call sites keep working.

## 5. b
This is the rule that makes the *same engine* portable across shells. Console runs it with `SystemRandom + SystemClock`. The browser will pass JS-backed implementations. Tests pass fakes. The engine is unchanged across all four hosts.