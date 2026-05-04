# Quiz answers — Module 5.5

## 1. a
Lua's only container is the table; there's no `enum` keyword. Strings as keys are the universal idiom — `"Gold"`, `"Wood"`, `"Stone"`. Slightly easier to typo than an enum (no compile-time check on the literal value); slightly easier to extend (just add a new string).

## 2. b
`task.wait` is Roblox's lightweight pause. Each script runs in its own coroutine; one waiting doesn't block the others. ~Equivalent to `await Task.Delay` in C# without the visible async keyword.

## 3. c
Console (Block 3), persistence file/JSON/SQLite (Block 4), web API (Block 5), browser (Block 6), Roblox (Block 7). Five runtimes; one engine. The lesson lands on contact.

## 4. b
Luau intentionally has less syntactic ceremony than C#. No namespaces (just module returns), no access modifiers, no XML docs, no explicit type annotations needed. The shape carries; the syntax compresses.

## 5. a
Server authority is the multiplayer rule. Game loop on the server: one canonical state, replicated to all clients. Game loop on each client: every player has a slightly different reality + cheating is trivial.