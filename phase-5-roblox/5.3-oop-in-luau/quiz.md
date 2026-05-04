# Quiz — Module 5.3

## 1. What's a metatable?

a. A table that defines behaviors (operators, lookup) for *another* table; the heart of Lua's OOP-via-tables
b. A type of database
c. A larger table
d. A reserved word

## 2. What does `__index = Building` do?

a. Sets a comment
b. Tells the metatable: when a key isn't found on the instance, look it up on `Building` — that's how methods get found
c. Indexes the table
d. Required by Lua

## 3. Why use the colon syntax `farm:upgrade()` vs the dot `farm.upgrade()`?

a. They're identical
b. Colon implicitly passes `self` as the first argument; dot doesn't. `farm:upgrade()` = `farm.upgrade(farm)`
c. Style
d. Required by classes

## 4. What's a ModuleScript in Roblox?

a. A script that runs automatically
b. A script that defines a module — doesn't auto-run; another script `require`s it. Used for engine code, libraries, shared types.
c. A LocalScript variant
d. A folder

## 5. The lesson says "OOP isn't `class` keywords." What's the meaning?

a. OOP = grouping data + methods + inheritance. Lua delivers it via tables + metatables instead of a `class` keyword. The pattern is the value, not the syntax.
b. C# is wrong
c. Lua doesn't have OOP
d. Style preference