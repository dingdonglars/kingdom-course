# Module 5.2 — Luau Basics

Luau is the language Roblox runs on. It's a variant of Lua with optional types added on top — smaller than C#, simpler than JavaScript, but built from the same parts: variables, functions, conditionals, loops, and one container called a *table*. Today we walk through the syntax differences. Tomorrow you use Luau to build classes.

> **Words to watch**
>
> - **Luau** — Roblox's variant of Lua. Adds optional types and a few performance tweaks.
> - **table** — Lua's universal container. Arrays, dictionaries, and objects are all tables.
> - **`local`** — declare a variable scoped to its block. Without `local`, the variable is global, which is almost never what you want.
> - **`function ... end`** — function syntax. `end` closes the body instead of `}`.
> - **type annotation** — the `: number` part of `local x: number = 10`. Optional, but recommended.

---

## C# to Luau, side by side

The fastest way to learn a second language is to read a translation table. The shapes are familiar; only the words have changed.

| C# | Luau | Note |
| --- | --- | --- |
| `int x = 10;` | `local x = 10` | `local` keeps the variable scoped. Without it, the variable becomes global. |
| `int x = 10;` (typed) | `local x: number = 10` | Type is optional but recommended. |
| `void Print(int x)` | `function Print(x: number)` | Function body ends with `end`. |
| `if (x > 0) { ... }` | `if x > 0 then ... end` | No braces. `then` opens, `end` closes. |
| `for (int i = 0; i < 10; i++)` | `for i = 1, 10 do ... end` | One-indexed. `do` opens, `end` closes. |
| `foreach (var item in list)` | `for _, item in ipairs(list) do ... end` | `ipairs` for arrays, `pairs` for dictionaries. |
| `// comment` | `-- comment` | Two dashes. |
| `/* block */` | `--[[ block ]]` | Double square brackets for block comments. |
| `string s = "x" + y;` | `local s = "x" .. y` | `..` for string concat. `+` would be a number error. |
| `null` | `nil` | The "no value" value. |
| `var d = new Dictionary<string,int>();` | `local d = {}` | Tables do double duty. |
| `var l = new List<string>();` | `local l = {}` | Same literal. Array vs dictionary is a usage distinction, not a type one. |

Luau is roughly half the syntax of C#. The runtime is much smaller too — no garbage-collection pauses, no JIT warmup, and a one-second tick is fast.

## Tables — the one container

Luau has exactly one built-in container, and it's called a table. The same value works as an array, as a dictionary, or as both at once.

```lua
-- Array-style
local resources = {"Gold", "Wood", "Stone"}
print(resources[1])   -- "Gold"  (one-indexed!)

-- Dictionary-style
local kingdom = { name = "Eldoria", day = 11 }
print(kingdom.name)            -- "Eldoria"
print(kingdom["name"])         -- same thing, different syntax

-- Mixed
local mixed = { "first", "second", name = "test" }
```

The biggest gotcha when you come from C# or JavaScript is that **arrays start at index 1**, not index 0. Muscle memory writes `for i = 0, n - 1` and gets nothing — every loop in Luau wants `for i = 1, n`. Re-train this once and the rest of the language gets out of your way.

## Functions

```lua
local function greet(name: string)
    print("Hello, " .. name)
end

greet("Athos")
```

`local` keeps the function name scoped to its file — without it, the function leaks into the global namespace, which Roblox places have one of, and which Lua programmers have collectively learned to dread. Always `local`.

Lua supports multiple return values with no special syntax — `function f() return 1, 2 end` returns two values, and `local a, b = f()` unpacks them. Anonymous functions look like `local f = function(x) return x * 2 end`.

## Conditionals

```lua
if score > 100 then
    print("High score!")
elseif score > 50 then
    print("OK")
else
    print("Try again")
end
```

`then` after the condition. `end` closes the whole `if`. No parentheses around the condition — they're allowed, just not needed.

## Loops

```lua
-- Numeric
for i = 1, 10 do
    print(i)
end

-- Step by 2
for i = 1, 10, 2 do
    print(i)         -- 1, 3, 5, 7, 9
end

-- Iterate an array
for index, value in ipairs(resources) do
    print(index, value)
end

-- Iterate a dictionary
for key, value in pairs(kingdom) do
    print(key, value)
end

-- While
while x < 10 do
    x = x + 1
end
```

Two iteration helpers worth memorising: `ipairs` walks an array in order and stops at the first nil; `pairs` walks every key in a dictionary in unspecified order. If you reach for the wrong one, you'll either skip entries or visit them in a strange order.

## The `print` debugging loop

The Output panel in Studio is your debugger ninety percent of the time. There is a real debugger — *Debug menu → Breakpoints* — and you'll meet it the day you really need it. For now, `print(x)` is enough.

## Type annotations — the Luau bit

Plain Lua has no types. Luau adds optional ones on top:

```lua
local function add(a: number, b: number): number
    return a + b
end

local kingdom: { name: string, day: number } = { name = "X", day = 1 }
```

Studio checks the types as you type — pass a string where a number was promised and the line gets a red squiggle. The check is similar to TypeScript's: it's a hint to the editor, not a runtime guard. Recommended for any function with two or more parameters.

## Tinker

Open Studio, insert a Script in `ServerScriptService`, and paste the small playground from the starter file `luau-basics.lua`. Hit Play and read the Output panel.

Try a typo on purpose: `local x: number = "hello"`. Studio underlines it. Hover the squiggle for the error.

`#resources` gives the length of a table-as-array. Print it.

`table.insert(resources, "Food")` adds an element to the end; `table.remove(resources, 1)` removes the first. The standard library has the array operations you'd expect, but they're functions on the `table` module, not methods on the array itself.

## What you just did

You walked through the syntax of a second language and noticed it's mostly familiar. Variables, functions, conditionals, loops — same ideas, different keywords. Luau's one container, the table, does the work that C# splits across `List`, `Dictionary`, and class-with-fields. The big gotcha you'll trip on for the first day or two is that arrays are one-indexed. Once that re-trains, you can read most Luau code on the page without slowing down. The syntax delta is roughly thirty minutes of reading; the rest comes from typing a few hundred lines of it over the next six modules.

**Key concepts you can now name:**

- *`local`* — declares a scoped variable; without it, you've made a global
- *table* — Luau's one container; array, dictionary, and object in one
- *one-indexed arrays* — the daily gotcha for C# and JavaScript brains
- *`..`* — string concat operator; `+` is numbers only
- *type annotations* — optional hints Studio checks while you type

## Words to add to the glossary

- **Luau** — Roblox's variant of Lua, with optional types.
- **table** — Lua's universal container; arrays, dictionaries, and objects are all tables.
- **`local`** — declares a variable scoped to its block.
- **`ipairs` / `pairs`** — array iteration vs dictionary iteration.
- **`nil`** — Lua's "no value" value.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 5.3 builds **OOP in Luau** — classes via tables and metatables. The same `Building` and `Farm` you wrote in Phase 1, in a smaller language with a different recipe.
