# Quiz answers — Module 0.3

## 1. b

`new List<string>()` calls the constructor of the `List<T>` class with `T = string`, creating an empty list ready to hold strings. The angle brackets `<...>` are how you tell a generic class what type it works with.

## 2. b

`Add` appends an item to the end of the list. `inventory.Add("knife")` makes the list grow by one element.

## 3. c

`Contains` is a *predicate* — it takes an item and asks *"is this in the list?"* — returning `true` or `false`. Behind the scenes it walks the list comparing each element to your argument.

## 4. b

Splitting code into methods is *organisation*. Each method has a clear single purpose. The structure of your code maps to the structure of the problem. Imagine the same adventure as a 60-line `if/else` chain — unreadable, hard to extend, hard to debug. With three methods it's three small problems instead of one big one.

## 5. b

When a method finishes, control returns to the line *after* the call. So if `Hallway` says `Kitchen();` and `Kitchen` returns, `Hallway` continues from the next line. (In our adventure, that "next line" is the *end* of the `if/else` chain in `Hallway`, which is the end of the `Hallway` method itself — so the program then returns to wherever called `Hallway`. In this case, the very first line of the program. So the program ends.)
</content>
</invoke>