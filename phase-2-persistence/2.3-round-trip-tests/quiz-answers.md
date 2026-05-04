# Quiz answers — Module 2.3

## 1. b
A round-trip test is "save → load → assert equal." It catches subtle serialisation bugs that don't show up otherwise: levels reset, fields lost, ordering shifted. One round-trip test gives more coverage than a dozen field-by-field assertions.

## 2. b
To save state exactly, every meaningful piece of it has to be reachable from outside the class. Hidden state — `Day` had no setter; `Building.Level` had no constructor — is the kind of "looks fine until you try to load" trap. Persistence forces you to face the model honestly. The fixes you make for load almost always improve the model overall.

## 3. b
A factory method is a static method that returns an instance — sometimes via a non-trivial construction path. `Kingdom.LoadFrom(snap, rng, clock)` is a factory that takes a data record and rebuilds a full Kingdom. It's an alternative to constructors for "build from data" cases.

## 4. a
`protected` means "only this class and its subclasses can call this." Outside callers (the console, tests) shouldn't be constructing `Building`s with arbitrary levels — that's a load-only operation. The subclasses (`Farm`, `Lumberyard`, `Mine`) need it to forward `(name, level)` up to the base.

## 5. b
Property-based testing — instead of asserting one specific outcome ("for input X, output should be Y"), assert that a *property* holds across many inputs ("for any reasonable kingdom, save+load equals original"). Real property libraries (FsCheck) generate hundreds of inputs automatically; `[Theory] + [InlineData]` is the manual seed of the same idea.