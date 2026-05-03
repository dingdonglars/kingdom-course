# Quiz answers — Module 1.7

## 1. b
A `record` is C#'s shorthand for a small immutable data class. The compiler auto-generates equality (two records with the same field values are equal), ToString, deconstruction, and a copy-constructor. Perfect for events: small, immutable, comparable.

## 2. b
Newing up `Random` inside the engine bakes in unpredictability *and* hides the dependency. Tests have no way to control the dice. Two players starting on the same day get different worlds. Module 1.8 fixes both by passing in `IRandom` via the constructor.

## 3. c
Both. The underscore is a *discard* (a value you don't care about) AND in switch patterns it's the catch-all "anything else." The double meaning is intentional — both come down to "I'm not naming this."

## 4. a
`when` adds a predicate to a pattern. The arm only matches when (i) the value matches the pattern AND (ii) the `when` clause is true. If either fails, the next pattern is tried.

## 5. b
The tests are weak because the engine is non-deterministic. We can only assert things like "at least one event in 50 days" — not "exactly this event on this day." Tomorrow's `IRandom` makes precise assertions possible.