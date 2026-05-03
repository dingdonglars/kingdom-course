# Quiz answers — Module 0.6

## 1. b

`void` is the special return type that means *"this method returns nothing"*. You can call a `void` method, but you can't assign its result to anything (because there is no result).

## 2. c

`Square(7)` calls `Square(int x)` with `x = 7`. The body is `x * x`, which evaluates to `49`. The `=>` syntax means *"return the value of this expression"*.

## 3. b

Method overloading is allowed when the *parameter types* differ. `int Square(int x)` and `double Square(double x)` are valid overloads. Same parameter types but different *return types* (option c) is **not** allowed — C# can't tell them apart at the call site. Different parameter *names* (option d) doesn't make them different overloads — only the types matter.

(Also note: this only works inside a class. Top-level *local* functions can't be overloaded — that's why the lesson uses a `static class Helpers`.)

## 4. b

*Parameter* is the formal name in the method definition: `string kingdom` in `void Greet(string kingdom)`. *Argument* is the actual value you pass: `"Eldoria"` in `Greet("Eldoria")`. The terms are often used interchangeably in casual conversation; the precise distinction matters when reading API documentation.

## 5. d (both a and c)

Both expressions are correct. The `=>` syntax is called *expression-bodied* and is shorthand for *block-bodied* `{ return ...; }`. They produce identical compiled code and behave identically. Use `=>` when the body is a single expression; use the `{ ... }` form when you need multiple statements.