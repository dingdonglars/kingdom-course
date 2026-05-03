# Quiz — Module 1.8

## 1. What's an interface?

a. A graphical user interface
b. A contract — method/property *shapes* with no bodies. Many classes can implement the same interface.
c. The opposite of a class
d. A Java-only feature

## 2. What does *dependency injection* mean in this lesson?

a. Injecting code at runtime
b. Passing a class's collaborators (e.g., `IRandom`, `IClock`) in via the constructor instead of newing them up inside
c. Using a DI framework
d. Hot-loading a module

## 3. What does `A.CallTo(() => rng.NextDouble()).Returns(0.1)` do?

a. Calls `NextDouble()` once
b. Sets up the fake `rng` so that whenever any code calls `rng.NextDouble()`, the fake returns `0.1`
c. Throws an exception
d. Records the call without changing behavior

## 4. Why does `Kingdom` keep a no-arg `Kingdom(string name)` constructor that chains to the new one?

a. Backward compatibility — existing tests (1.3 / 1.4) use `new Kingdom("Test")` and would break otherwise
b. To save typing
c. Because C# requires it
d. By accident

## 5. The lesson says "every external dependency comes in through an interface." What does that buy you?

a. Faster code
b. The same engine can run in console, web API, browser, Roblox — each shell wires its own implementations. Tests can swap fakes for real implementations. Easier to reason about.
c. Smaller binaries
d. It's a style preference with no real benefit