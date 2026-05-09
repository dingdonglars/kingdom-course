# Module 5.3 — OOP in Luau (Tables-as-Classes, Metatables)

Lua doesn't have classes. It has tables and one extra mechanism called *metatables*, and that turns out to be enough to build classes by hand. Every Lua codebase you'll ever read uses some flavour of the same recipe. Today we port the Phase 1 `Building` and `Farm` to Luau using that recipe — same idea, smaller language, a bit more typing.

> **Words to watch**
>
> - **metatable** — a table that defines extra behaviour for *another* table (operators, lookup, comparison).
> - **`__index`** — a metatable entry that says "if a key isn't found here, look in this other table." This is what turns a table into a class.
> - **method-call syntax `:`** — `obj:method()` is shorthand for `obj.method(obj)`. The colon is what passes `self`.
> - **`setmetatable(t, mt)`** — attach a metatable to a table.
> - **module pattern** — return a table from a `ModuleScript`; `require` returns it. Roblox's import system.

---

## The OOP-via-tables recipe

Lua has one container (the table) and one trick (metatables). Every Lua codebase uses some variant of this recipe to fake classes.

```lua
-- The "class" is just a table.
local Building = {}
Building.__index = Building          -- methods are looked up on Building

-- Constructor.
function Building.new(name: string)
    local self = setmetatable({}, Building)
    self.name = name
    self.level = 1
    return self
end

-- Method.
function Building:upgrade()
    self.level = self.level + 1
end

function Building:tick(ledger: any)
    -- default: nothing
end

return Building
```

Read it line by line:

- `local Building = {}` — the table that represents the class.
- `Building.__index = Building` — when you look up a key on an *instance* and don't find it, fall back to the `Building` table. **This is how methods get found.**
- `Building.new(name)` is the constructor. It creates a new table, attaches `Building` as its metatable, fills the fields, returns it.
- `function Building:upgrade()` — the colon means *implicit `self` parameter*. It's equivalent to writing `Building.upgrade = function(self) ... end`.
- `setmetatable(t, mt)` attaches `mt` as `t`'s metatable.

You'll write this same recipe for every "class" in your engine. It's a bit verbose, but mechanical — by the third one your fingers know it.

## Inheritance

A subclass is the same recipe with one extra step: set the parent class as the subclass's `__index`.

```lua
local Building = require(script.Parent.Building)

local Farm = setmetatable({}, { __index = Building })   -- inherit
Farm.__index = Farm

function Farm.new(name: string)
    local self = Building.new(name)                     -- call parent constructor
    setmetatable(self, Farm)                            -- re-parent the instance
    return self
end

function Farm:tick(ledger: any)
    ledger:add("Food", 5 * self.level)                  -- override the default tick
end

return Farm
```

`Farm` "is a" `Building` because the parent sits in `Farm`'s `__index`, and instances of `Farm` get re-parented in the constructor. Method dispatch then walks the chain: an instance method falls back to `Farm`, then to `Building`.

## Module pattern (Roblox-specific)

Scripts in Roblox come in three flavours:

- **Script** — runs on the server when the place starts.
- **LocalScript** — runs on a client (a player's machine) when they join.
- **ModuleScript** — defines a module. It doesn't run on its own; another script calls `require` on it.

Engine code lives in **ModuleScripts** under `ReplicatedStorage`, so both server and client scripts can import it. Every module file has the same layout: declare a table at the top, attach functions to it, return it at the bottom.

```lua
-- consumer
local Building = require(game.ReplicatedStorage.Engine.Building)

local farm = Building.new("Main Farm")
farm:upgrade()
print(farm.level)          -- 2
```

`require` caches the module — calling it a hundred times returns the same table, not a hundred copies. Modules are effectively singletons.

## Tinker

Add a `Mine.lua` that mirrors `Farm.lua` but adds to `"Stone"` instead of `"Food"`. The pattern is identical; you're rehearsing the recipe.

Try `farm.upgrade()` (with a dot instead of a colon). It throws an error — `self` is missing because the dot doesn't pass it. The colon vs the dot is one of the two or three Luau details you'll trip on early; let it bite you once so the rule sticks.

Print `getmetatable(farm)`. The output is the `Farm` class table — proof that the metatable *is* the class.

Add a `Building:describe()` method that returns a string. Override it in `Farm`. Call `farm:describe()` and watch dispatch find the `Farm` version first, falling back to `Building`'s if you delete the override.

## What you just did

You met Lua's idea of object-oriented programming, which is a recipe rather than a keyword. A class is a table; a method is a function attached to that table; an instance is a fresh table whose metatable points at the class; `__index` is the rule that makes method lookup work. Inheritance is the same recipe with one more step. The whole recipe is mechanical, and it's the same one every Lua codebase uses — once you've written `Building` and `Farm` you can read `Mine`, `Lumberyard`, and the next twenty classes without thinking. You also met Roblox's three script flavours: ordinary Scripts that run on the server, LocalScripts that run on a player's machine, and ModuleScripts that hold engine code that other scripts `require`.

**Key concepts you can now name:**

- *metatable* — table-of-behaviour attached to another table
- *`__index`* — the fallback-lookup rule that makes method dispatch work
- *colon vs dot* — `:` passes `self` automatically; `.` doesn't
- *ModuleScript* — Roblox's importable code unit, returned by `require`
- *module pattern* — declare a table, attach functions, return it

## Words to add to the glossary

- **metatable** — a table attached to another table that defines extra behaviour.
- **`__index`** — a metatable entry that says "if a key isn't found here, look there."
- **`setmetatable`** — the function that attaches a metatable.
- **ModuleScript** — Roblox's importable code unit; returned by `require`.
- **module pattern** — declare a table, attach functions to it, return it.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 5.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 5.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 5.4 introduces the **Roblox-specific concepts** — Workspace, RemoteEvents, server vs client. The runtime context the engine plugs into.
