# Quiz — Module 0.2

## 1. What does `while (true) { ... }` do?

a. Runs the code inside once
b. Runs the code inside zero times (since `true` is suspicious)
c. Runs the code inside repeatedly, forever, until `break` is reached
d. Runs the code inside until the user presses Enter

## 2. What does `random.Next(1, 101)` give you?

a. A random number between 1 and 100 (inclusive)
b. A random number between 1 and 101 (inclusive)
c. A random number between 1 and 100 (random says hi)
d. The number 101

## 3. In an `if / else if / else` chain, how many branches run?

a. All of them
b. None of them
c. Exactly one
d. The first one that's true

## 4. What does `int.Parse("42")` give you?

a. The string `"42"`
b. The number `42`
c. An error (`"42"` isn't a number)
d. The number `42.0`

## 5. Why does the program use `int.Parse(input ?? "0")` instead of just `int.Parse(input)`?

a. Because `0` is funnier than the user's actual input
b. Because `Console.ReadLine()` returns `string?` (a string that *might* be `null`), and `int.Parse` can't handle `null` — `?? "0"` says "use the input if it's not null; otherwise use the string '0'"
c. To start counting from zero
d. There's no good reason; it's a habit
</content>
</invoke>