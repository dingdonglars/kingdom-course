# Quiz — Module 2.3

## 1. What's a *round-trip test*?

a. A test that runs twice
b. A test that saves the model, loads it back, and asserts the loaded model equals the original
c. A test for travel apps
d. A test that calls a remote server

## 2. Why does the lesson say *"persistence is the most honest pressure on a model"*?

a. Saving to disk is slow
b. To save and load exactly, every meaningful piece of state must be addressable. Hidden or under-exposed state surfaces as a bug — exactly what persistence forces you to fix.
c. JSON is strict
d. Persistence requires more tests

## 3. What's a *factory method*?

a. An ASP.NET concept
b. A static method that returns an instance of a class — used in place of (or alongside) a constructor. `Kingdom.LoadFrom(snap, ...)` is the example here.
c. A constructor with a different name
d. Required for inheritance

## 4. Why is `Building`'s second constructor `protected` instead of `public`?

a. It only makes sense for subclasses (Farm/Lumberyard/Mine), not callers from outside the engine. `protected` lets the subclasses use it without exposing it broadly.
b. To make the code compile
c. Required by .NET
d. Performance

## 5. The lesson uses `[Theory] + [InlineData]` to run the same test with 4 different day counts. What's that pattern called (in its more general form)?

a. Multiple inheritance
b. Property-based testing — assert that some *property* (e.g., "round-trip preserves state") holds across many inputs, not just one
c. Mocking
d. Integration testing