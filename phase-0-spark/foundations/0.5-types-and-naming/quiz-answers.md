# Quiz answers — Module 0.5

## 1. b

`5` and `2` are both `int`. `int / int` is *integer division* — the result is also an `int`, and the decimal part is dropped. So `5 / 2` is `2`. To get `2.5`, at least one side must be a `double`: `5 / 2.0` (which gives `2.5`).

## 2. a

Casting a `double` to an `int` **truncates** (drops everything after the decimal). It does *not* round. So `(int)3.99` is `3`, and `(int)-3.99` is `-3`. If you want to round, use `(int)Math.Round(3.99)`.

## 3. d

Only (d) compiles. (a), (b), and (c) try to assign a value of one type to a variable of a different type — C# is statically typed and won't allow it. (d) is fine because `string?` is the *nullable* version of `string`, which means it's allowed to hold `null`.

## 4. b

PascalCase (`MyClass`). Every C# developer follows this convention; the `STANDARDS.md` at the root of every project encodes it. The C# Dev Kit extension in VS Code flags violations.

## 5. b

The underscore is a *visual signal* — when you see `_health` inside a class, you know immediately it's a private field of *this* class, not a parameter or a local variable. It also sorts to the top in alphabetical lists. There's no compiler enforcement; it's a convention every C# codebase respects.