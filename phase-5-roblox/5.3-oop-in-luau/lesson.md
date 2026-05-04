# Module 5.3 — OOP in Luau (Tables-as-Classes, Metatables)

> **Hook:** Lua doesn't have classes. It has *tables* and *metatables* — and that's enough to build classes by hand. Every Lua codebase you'll read uses some variant of this pattern. Today we port `Building` and `Farm` from Block 3 — same shape, smaller language.

> **Words to watch**
> - **metatable** — a table that defines behavior (operators, lookup) for *another* table
> - **`__index`** — a metatable hook: "if a key isn't found here, look in this other table"
> - **method-call syntax `:`** — `obj:method()` is sugar for `obj.method(obj)`
> - **`setmetatable(t, mt)`** — attach a metatable to a table
> - **module pattern** — return a table from a `ModuleScript`; `require` returns it

---

## The OOP-via-tables recipe

Lua has one container (table) and one trick (metatables). The recipe everyone uses:

```lua
-- Define the "class"
local Building = {}
Building.__index = Building          -- methods looked up on Building

-- Constructor
function Building.new(name: string)
    local self = setmetatable({}, Building)
    self.name = name
    self.level = 1
    return self
end

-- Method
function Building:upgrade()
    self.level = self.level + 1
end

function Building:tick(ledger: any)
    -- default: nothing
end

return Building
```

Read line-by-line:

- `local Building = {}` — the table representing the class.
- `Building.__index = Building` — when looking up a key on an *instance*, fall back to `Building`. **This is how methods get found.**
- `Building.new(name)` — constructor. Creates a new table, sets its metatable to `Building`, fills fields, returns it.
- `function Building:upgrade()` — the colon `:` means "implicit `self` parameter." Equivalent to `Building.upgrade = function(self) ... end`.
- `setmetatable(t, mt)` — attach `mt` as `t`'s metatable.

You'll write this same recipe for every "class" in your engine. **It's verbose, but mechanical** — you can fingertips-type it after the third one.

## Inheritance

Subclass = same recipe + set the parent as the `__index`:

```lua
local Building = require(script.Parent.Building)

local Farm = setmetatable({}, { __index = Building })   -- inherit
Farm.__index = Farm

function Farm.new(name: string)
    local self = Building.new(name)                     -- call parent ctor
    setmetatable(self, Farm)                            -- re-parent the instance
    return self
end

function Farm:tick(ledger: any)
    ledger:add("Food", 5 * self.level)                  -- override
end

return Farm
```

The pattern is `Farm` "is a" `Building` (parent's `__index`), plus its own methods.

## Module pattern (Roblox-specific)

In Roblox, scripts come in three flavors:

- **Script** — runs on the server when the place starts.
- **LocalScript** — runs on a client (player's machine) when they join.
- **ModuleScript** — defines a module; doesn't auto-run; another script `require`s it.

Engine code goes in **ModuleScripts** in `ReplicatedStorage` (so server + client can both `require` it). Top of the file: `local Building = {}`. Bottom: `return Building`.

Consumer:

```lua
local Building = require(game.ReplicatedStorage.Engine.Building)

local farm = Building.new("Main Farm")
farm:upgrade()
print(farm.level)          -- 2
```

`require` caches — calling `require(game...Building)` 100 times returns the same table. Modules are effectively singletons.

## Delta starter

Mini-port of Block 3's engine:

- `roblox-kingdom/Engine/Building.lua` (ModuleScript)
- `roblox-kingdom/Engine/Farm.lua` (ModuleScript)
- `roblox-kingdom/Engine/Lumberyard.lua`, `Mine.lua`
- `roblox-kingdom/scripts/test-engine.lua` (Script that requires + uses them)

In Studio's Explorer:
- Insert a `Folder` in `ReplicatedStorage` named `Engine`.
- Inside it, insert four `ModuleScript`s named `Building`, `Farm`, `Lumberyard`, `Mine`.
- Paste each `.lua` source.
- Insert a `Script` in `ServerScriptService` and paste `test-engine.lua`.
- Hit Play. Output shows the smoke test.

## Tinker

- Add a `Mine.lua` mirroring `Farm.lua` but adding to `"Stone"`. Standard subclass pattern.
- Try `farm.upgrade()` (with `.` instead of `:`). **Errors** — `self` is missing. The colon matters.
- Print `getmetatable(farm)` — shows the Farm class table. **The metatable IS the class.**
- Add a `Building:describe()` method returning `string`. Override in `Farm`. Watch dispatch happen.

## Name it

- **Metatable** — table-of-behavior attached to another table.
- **`__index`** — fallback lookup hook; the heart of class behavior.
- **`setmetatable(t, mt)`** — attach.
- **Method syntax `:`** — implicit `self`.
- **ModuleScript** — Roblox's importable code unit.
- **`require(...)`** — import a ModuleScript; cached.

## The rule of the through-line

> **OOP isn't `class` keywords. It's *grouping data + methods + inheritance*. Lua does it with tables. Roblox accepts the recipe as standard.** The pattern is verbose; the value is the same.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 5.4 introduces **Roblox-specific concepts** — Workspace, RemoteEvents, server vs client. The runtime context the engine plugs into.