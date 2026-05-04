# Module 5.2 — Luau Basics

> **Hook:** Luau is Roblox's language — Lua with optional types. Smaller than C#, simpler than JavaScript, but the same building blocks: variables, functions, conditionals, loops, tables. Today we cover the syntax delta — what's the same, what's different. Tomorrow (5.3) we use it to build classes.

> **Words to watch**
> - **Luau** — Roblox's variant of Lua; adds types, performance, sandboxing
> - **table** — Lua's universal data structure: arrays, dictionaries, objects all in one
> - **`local`** — declare a local variable (Lua's `var`/`let`)
> - **`function ... end`** — function declaration; `end` instead of `}`
> - **type annotation** — `x: number` — optional types Luau adds on top of Lua

---

## C# → Luau, side by side

| C# | Luau | Notes |
|---|---|---|
| `int x = 10;` | `local x = 10` | `local` = scoped (default is global) |
| `int x = 10;` (typed) | `local x: number = 10` | Type optional but recommended |
| `void Print(int x)` | `function Print(x: number)` | `end` closes the function |
| `if (x > 0) { ... }` | `if x > 0 then ... end` | No braces; uses `then`/`end` |
| `for (int i = 0; i < 10; i++)` | `for i = 1, 10 do ... end` | **1-indexed**; uses `do`/`end` |
| `foreach (var item in list)` | `for _, item in ipairs(list) do ... end` | `ipairs` for arrays; `pairs` for dicts |
| `// comment` | `-- comment` | Two dashes |
| `/* block */` | `--[[ block ]]` |  |
| `string s = "x" + y;` | `local s = "x" .. y` | `..` for concat (`+` would error) |
| `null` | `nil` |  |
| `var d = new Dictionary<>();` | `local d = {}` | One literal for everything |
| `var l = new List<>();` | `local l = {}` | Same. Array vs dict by usage. |

**Luau is ~half the syntax of C#. The runtime is much smaller too** — no garbage collection pauses, no JIT warmup; a 1-second tick is *fast*.

## Tables — the one data structure

Luau has *one* container type: the table. It's an array, a dictionary, an object, all at once:

```lua
-- Array-style
local resources = {"Gold", "Wood", "Stone"}
print(resources[1])   -- "Gold"  (1-indexed!)

-- Dictionary-style
local kingdom = { name = "Eldoria", day = 11 }
print(kingdom.name)            -- "Eldoria"
print(kingdom["name"])         -- same thing

-- Mixed
local mixed = { "first", "second", name = "test" }
```

**1-indexed arrays are the biggest gotcha.** Coming from C#/JS, you'll write `i = 0` muscle-memory and get nothing. Re-train: **`for i = 1, n do`**.

## Functions

```lua
local function greet(name: string)
    print("Hello, " .. name)
end

greet("Athos")
```

- `local` keeps it scoped (otherwise it leaks to global).
- Multi-return is built in: `function f() return 1, 2 end` then `local a, b = f()`.
- Anonymous functions: `local f = function(x) return x * 2 end`.

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

`then` after the condition; `end` closes. No parentheses needed (you can add them).

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

-- Iterate array
for index, value in ipairs(resources) do
    print(index, value)
end

-- Iterate dict
for key, value in pairs(kingdom) do
    print(key, value)
end

-- While
while x < 10 do
    x = x + 1
end
```

`ipairs` for arrays (stops at first nil), `pairs` for dictionaries (any keys).

## The `print` debugging loop

In Studio's Output panel, `print(x)` is your debugger. There's a real debugger too (Debug menu → Breakpoints) but `print` covers 90% of cases.

## Type annotations (the Luau bit)

Luau adds optional types on top of Lua:

```lua
local function add(a: number, b: number): number
    return a + b
end

local kingdom: { name: string, day: number } = { name = "X", day = 1 }
```

The types are *checked at edit time in Studio* — squiggle if you pass a string where a number is expected. Recommended for any function with 2+ parameters.

## Delta starter

- `roblox-kingdom/scripts/luau-basics.lua` — a small playground demonstrating each concept

(Roblox files live outside the curriculum repo; this is a reference snippet you'll paste into Studio.)

## Tinker

- Open Studio. Insert a Script in `ServerScriptService`. Paste the basics. Run.
- Make a typo: `local x: number = "hello"`. **Studio underlines it.**
- Try `print(#resources)` — `#` is the length operator for tables-as-arrays.
- Try `table.insert(resources, "Food")` and `table.remove(resources, 1)`. Standard array operations.

## Name it

- **Luau** — Roblox's Lua + types.
- **`local`** — declare scoped variable.
- **`function`/`end`** — function syntax.
- **`then`/`end`** — if syntax.
- **`do`/`end`** — for syntax.
- **table** — universal container.
- **`ipairs`/`pairs`** — array vs dict iteration.
- **1-indexed arrays** — the big gotcha.
- **`..`** — string concat.
- **`nil`** — null.

## The rule of the through-line

> **A new language is mostly the same ideas in different syntax.** Variables, functions, ifs, loops, structures. The 30 minutes you spend on the syntax-delta page lets you read 80% of any Luau code. The rest comes from doing.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 5.3 builds **OOP in Luau** — classes via tables and metatables. Same `Building` / `Farm` / `Kingdom` shape from Block 3, in a smaller language.