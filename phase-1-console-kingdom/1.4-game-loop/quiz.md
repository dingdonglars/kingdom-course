# Quiz — Module 1.4

## 1. What does the `virtual` keyword on `Building.Tick` do?

a. Marks it as fake / not-yet-implemented
b. Allows subclasses to override it
c. Makes it run on a virtual machine
d. Makes it asynchronous

## 2. Why is `AdvanceDay()` returning `void` instead of returning the new state?

a. Because returning state isn't allowed in C#
b. It mutates the kingdom in place — a *side effect*. Returning would mean copying the whole kingdom, which is unnecessary here.
c. The compiler refuses to return `Kingdom`
d. There's no good reason

## 3. In `AdvanceDay()`, why do buildings tick *before* citizens eat?

a. Alphabetical order
b. Performance
c. So today's food production is available for today's eating; flipping the order would mean eating tomorrow's food today
d. They tick simultaneously

## 4. What's a *side effect*?

a. Anything that prints to the console
b. A method that changes state somewhere instead of (or in addition to) returning a value
c. A bug
d. The opposite of a main effect

## 5. Why does `[Fact] Spend_NoFood_DoesNotCrash` exist?

a. To verify the code doesn't crash when `Spend` is asked to take more than the ledger has
b. Padding the test count
c. To test `Spend`'s return value
d. To check `Add`