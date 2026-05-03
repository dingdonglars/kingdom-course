# Quiz answers — Module 1.9

## 1. b
Folders are pure organisation — the compiler reads namespaces, not folder paths. We use both, in lockstep, by convention. A reader scanning `Kingdom.Engine/Buildings/` immediately knows what lives there; the matching `namespace Kingdom.Engine.Buildings;` confirms it.

## 2. a
A `global using` directive applies to every file in the *project*. Put them in a `GlobalUsings.cs` file at the project root. They cut the noise of repetitive `using` blocks — but they hide where types come from, so use sparingly. Good for your own sub-namespaces; risky for third-party libraries.

## 3. a
The aggregate root is the class that ties everything together — `Kingdom` owns Buildings, Citizens, Resources, Events. By convention, the root lives at the root, not buried inside a `Kingdoms/` subfolder. (The same convention applies to `Program.cs` — it's the shell's entry point and stays at the top.)

## 4. b
The "~7 files" threshold is a soft rule of thumb. Once a folder has more than you can scan at a glance, it's time to consider splitting. Don't follow it religiously — sometimes 10 small related files in one folder is fine; sometimes 4 large files of mixed concerns are too many.

## 5. b
Pre-mature sub-namespacing adds cost (lots of `using` directives, "where did that type come from" friction) without benefit when there are few types. Wait until the flat structure starts to hurt — then organize.