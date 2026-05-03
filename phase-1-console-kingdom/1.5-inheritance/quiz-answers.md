# Quiz answers — Module 1.5

## 1. b
The colon in a class declaration means *"inherits from."* `Farm` gets `Name`, `Level`, `Upgrade()`, and `Tick()` automatically — it can override the `virtual` ones and add new methods of its own.

## 2. a
`: base(name)` calls the parent constructor — `Building(string name)` — passing along the `name`. Without it, you'd be writing `Name = name;` again in `Farm`'s constructor (and every subclass's). The base call is a delegation.

## 3. b
`override` makes the override explicit. Without the requirement, you might accidentally write a method with the same name and signature as a parent's `virtual` method, *thinking* you'd added a new method but actually replacing one. The keyword forces you to *intend* the override.

## 4. c
`GetType()` returns the *runtime type*. For `b = new Farm(...)`, even if `b`'s declared type is `Building`, `GetType()` is `Farm`. `.Name` gives the short string name `"Farm"`.

## 5. c
Inheritance is rigid: once `Farm` extends `Building`, every `Building` change ripples to `Farm`, `Lumberyard`, `Mine`. Deep chains (3+ levels) become brittle. Composition (a class *contains* another) avoids that. For a single level — `Building → Farm` — inheritance is the right tool.