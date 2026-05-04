# Quiz answers — Module 5.3

## 1. a
A metatable is a table-of-behavior. Lua looks up special hooks (`__index`, `__add`, `__tostring`, etc.) on the metatable when operating on the parent table. Sounds magical; it's just structured fallback rules.

## 2. b
The `__index` hook says "if a key doesn't exist on this table, look in `__index`." Setting `Building.__index = Building` makes any instance with `Building` as its metatable fall through to `Building` for method lookup. That's the whole class trick.

## 3. b
The colon is sugar: `obj:method(args)` desugars to `obj.method(obj, args)`. Without the colon, `self` is missing. Forgetting the colon is a one-of-the-most-common Lua bugs.

## 4. b
ModuleScripts are libraries. They don't run on their own; `require(script.Parent.Building)` loads + caches them. Engine code, shared types, helpers — all live in ModuleScripts (typically in `ReplicatedStorage`).

## 5. a
OOP is *grouping*: data + methods together, with inheritance. Lua does it without a `class` keyword. The recipe is verbose; the meaning is identical to C#'s `class Building`. Once you've seen the pattern three times, it's mechanical.