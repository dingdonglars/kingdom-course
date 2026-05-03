# Quiz — Module 1.9

## 1. The compiler doesn't care about folders. So why use them?

a. The compiler does too care
b. Folders are a *convention* — they help humans scan the codebase. By matching folders to namespaces, the structure is consistent in two places.
c. Folders speed up the build
d. To impress reviewers

## 2. What's a "global using"?

a. A `using` directive in `GlobalUsings.cs` that applies to every file in the project
b. A `using` for `System` only
c. A keyword that doesn't exist in C#
d. The same as a normal `using`

## 3. Why does the *aggregate root* (`Kingdom.cs`) stay at the top level instead of in a subfolder?

a. Convention — the root class lives at the root, not buried in a subfolder named after itself
b. C# requires it
c. It's faster
d. By accident

## 4. What's the rough threshold for splitting a flat folder into subfolders?

a. Exactly 5 files
b. ~7 files (a soft rule of thumb — anything that takes more than a glance to scan)
c. 100 files
d. Never — flat is best

## 5. The lesson cautions against pre-emptively sub-namespacing. Why?

a. Sub-namespaces have a runtime cost
b. They add `using` lines and discoverability friction. Worth it for mid/large projects, overkill for tiny ones.
c. The compiler emits warnings
d. Pre-emptive sub-namespacing is forbidden