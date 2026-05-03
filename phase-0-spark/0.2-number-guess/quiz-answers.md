# Quiz answers — Module 0.2

## 1. c

`while (condition) { body }` runs the body repeatedly as long as the condition is true. `while (true)` is therefore an infinite loop — the only way out is `break;` inside the body.

## 2. a

`random.Next(min, max)` returns a number `>= min` and `< max`. So `Next(1, 101)` gives you a number from 1 to 100 inclusive — the upper bound is *exclusive*. Microsoft's API designers made this choice for compatibility with how loops are usually written; it's a common gotcha.

## 3. d (or c — both correct)

In an `if / else if / else` chain, branches are tested top-to-bottom. The *first* branch whose condition is true runs. The rest are skipped. Once one runs, you don't fall through to the next.

## 4. b

`int.Parse("42")` converts the string `"42"` into the integer `42`. If the string isn't a valid integer (`int.Parse("hello")` for instance), it throws an exception. Later we'll meet `int.TryParse` which returns success/failure instead of throwing.

## 5. b

`Console.ReadLine()` can return `null` (the question mark in its return type — `string?` — tells you that). `int.Parse` doesn't accept null. The `??` operator is *null coalescing* — `a ?? b` gives you `a` if it's not null, otherwise `b`. So `input ?? "0"` falls back to `"0"` if the input was somehow null. We're being defensive.
</content>
</invoke>