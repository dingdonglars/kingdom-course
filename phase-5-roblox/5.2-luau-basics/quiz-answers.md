# Quiz answers — Module 5.2

## 1. b
`local` scopes the variable to the enclosing function/block. Without `local`, Lua creates a *global* — usually a bug. Always `local` unless you specifically want global.

## 2. b
1-indexed. `arr[1]` is the first element. C# muscle memory says `arr[0]`; in Luau that's `nil`. The single biggest cross-language adjustment.

## 3. b
`..` for concat (`"hello " .. name`). `+` is strictly numeric in Luau and would throw a type error on strings. Different choice than C#/JS — Lua designers wanted operator unambiguity.

## 4. a
`ipairs(arr)` walks 1, 2, 3, ... stopping at the first nil. `pairs(dict)` walks every key in arbitrary order. For arrays use `ipairs`; for hashtable-style use `pairs`.

## 5. a
Same value as TypeScript: editor-time errors instead of runtime errors. Gradually adoptable — you can leave any line untyped and Luau infers what it can.