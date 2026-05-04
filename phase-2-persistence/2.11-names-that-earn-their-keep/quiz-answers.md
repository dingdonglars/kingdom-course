# Quiz answers — Module 2.11

## 1. b
*Manager* is one of the great noise words. It signals "a class exists" but not what it does. `OrderRepository` says: this loads/saves orders. `OrderManager` says: this... manages... orders. Pick the verb (`Repository`, `Validator`, `Compressor`) — never the abstraction (`Manager`).

## 2. b
Scope discipline. A name's job is to tell the reader, *given the surrounding code*, what the thing is. Inside `foreach (var b in buildings)`, `b` is fine — the loop tells you `b` is a building. As soon as the variable lives across 50 lines, it earns its full name (`building`).

## 3. b
A rename party gives you small, atomic, easy-to-review commits with no logic mixed in. It also lets you spot related renames — if you rename `KingdomData` to `KingdomEntity`, you'll naturally also rename `ToData()` to `ToEntity()`. Doing renames as you happen to touch each file scatters them across many PRs.

## 4. b
The IDE's Rename refactoring is *safe* — it updates every reference, including in tests, comments (XML doc), conditional compilation. Search-and-replace catches the obvious cases and misses the subtle ones (a comment that mentions the old name, a string literal, a type used by reflection).

## 5. b
Reader cost. Every name lives in the reader's head while they read the surrounding code. A weak name (`Manager`, `Helper`, `Util`) costs the same to remember but tells you nothing. A great name (`KingdomEfStore`, `ToSnapshot`) costs the same and tells you everything. Aim for the second.