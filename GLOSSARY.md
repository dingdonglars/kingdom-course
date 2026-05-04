# GLOSSARY.md

> Living alphabetical glossary of every term introduced in any lesson. Each entry is one line: a definition the learner can read in plain English, and the lesson where the term first appears.

> **How this list grows:** every lesson with a "Words to watch" sidebar adds its terms here at authoring time. `/lesson-review` and the final consistency audit catch missing entries.

## A

- **`A.CallTo(...)`** — FakeItEasy syntax to define what a fake should return when a method is called. *Module 1.8.*
- **`A.Fake<T>()`** — FakeItEasy syntax to make a fake implementing interface `T`. *Module 1.8.*
- **aggregate root** — the top-level class that owns and ties everything else together (`Kingdom`). Stays at the root of the namespace tree. *Module 1.9.*
- **alias** — a short name for a table in a SQL query (`SELECT k.name FROM kingdoms k`). *Module 2.5.*
- **argument** — the actual value you pass when calling a method (the *parameter* is the name in the definition; the *argument* is the value). *Module 0.6.*
- **arrange / act / assert** — the conventional 3-section structure of a unit test. *Module 1.3.*
- **`AsNoTracking`** — EF Core method for read-only queries; skips change-tracking, faster + safer. *Module 2.6.*
- **ASCII art** — pictures made out of text characters. *Module 0.4.*
- **`abstract`** — a class or method with no implementation; subclasses must provide it. *Module 1.5 (mention); Module 1.10 (tinker).*

## B

- **`base(...)`** — call the parent class's constructor from the child's. *Module 1.5.*
- **base class** (or *parent class*) — the class a child inherits from. *Module 1.5.*
- **`bool`** — `true` or `false`, nothing else. *Module 0.5.*
- **breakpoint** — a marked line where the debugger pauses execution. *Module 0.8.*

## C

- **cascade delete** — when deleting a parent row also deletes its child rows. EF default for required relationships. *Module 2.9.*
- **call stack** — the chain of methods that called each other to reach the current point of execution. *Module 0.8.*
- **camelCase** — convention for local variables and parameters (`goldCount`, `firstName`). *Module 0.5.*
- **`_camelCase`** — convention for private fields inside classes (one underscore + camelCase). *Module 0.5.*
- **cast** — convert a value from one type to another (`(int)3.99` is `3`). *Module 0.5.*
- **class library** — a project that compiles to a `.dll`, not an executable; no `Main`. Used by other projects. *Module 1.2.*
- **code-first** — defining the schema in C# code; the ORM generates SQL. Opposite: database-first. *Module 2.6.*
- **collection** — any data structure that holds multiple values (list, dictionary, array). *Module 0.7.*
- **composition** — a class *contains* another (vs. *inheriting* from). Often a more flexible alternative to inheritance. *Module 1.5.*
- **`Console.ReadLine`** — reads a line of user input (string), returns `null` at EOF. *Module 0.1; Module 2.10 (with EOF handling).*
- **`Console.SetIn` / `SetOut`** — redirect the console streams; lets tests script user input and capture output. *Module 2.10.*
- **CRUD** — Create / Read / Update / Delete; the four operations on rows. *Module 2.9.*
- **conditional** — `if` / `else` — a fork in the code. *Module 0.2.*
- **`Console.ForegroundColor`** — the property that controls the color of text written to the console from now on. *Module 0.4.*
- **`Console.ReadLine()`** — the method that pauses, waits for input, returns the line as a string. *Module 0.1.*

## D

- **database** — a structured store of data, queryable. *Module 2.4.*
- **`DateTime`** — a date *and* a time, together. *Module 0.5.*
- **DB Browser for SQLite** — free GUI tool for opening + querying SQLite files. *Module 2.8.*
- **`DbContext`** — EF Core's gateway to the database; one per connection lifetime. *Module 2.6.*
- **`DbSet<T>`** — EF Core property representing a table; supports LINQ. *Module 2.6.*
- **debugger** — a tool that pauses your program at chosen points so you can see what's happening. *Module 0.8.*
- **deferred execution** — LINQ doesn't run when you call `.Where(...)`; it builds a recipe. The work happens when you iterate / call `.ToList()` / `.Count()`. *Module 1.6.*
- **dependency injection (DI)** — passing a class's collaborators in via the constructor instead of newing them up inside. *Module 1.8.*
- **deterministic** — same inputs always produce the same outputs. The trait that makes engines testable. *Module 1.7 (introduced as the rule we broke); Module 1.8 (fixed).*
- **`Dictionary<K, V>`** — a lookup table from `K` (key) to `V` (value); each key appears at most once. *Module 0.7.*
- **discard (`_`)** — a name for "I don't care about this value." Also used in switch patterns as the catch-all "anything else." *Module 1.7.*
- **`dotnet ef`** — EF Core CLI tool for generating + applying migrations. *Module 2.7.*
- **`dotnet ef migrations script`** — outputs the SQL the migrations would run (no execution). *Module 2.8.*
- **`double`** — a number that can have a decimal point. *Module 0.5.*
- **DTO** — Data Transfer Object — a small data-only record purpose-built for crossing a boundary (disk, network, API). *Module 2.2.*

## E

- **EF Core** — Entity Framework Core; .NET's standard ORM. *Module 2.6.*
- **`EnsureCreated`** — EF method that creates the database file + tables from the current model. Doesn't use migrations; only suitable for fresh DBs. *Module 2.6.*
- **`__EFMigrationsHistory`** — EF's bookkeeping table tracking which migrations have been applied. *Module 2.7.*
- **encapsulation** — keeping a class's internals private so callers can't poke at them; only the public methods can change state. *Module 1.2 (preview), Module 1.3 (named).*
- **encoding** — how text becomes bytes. UTF-8 is the answer for ~everything written this decade. *Module 2.1.*
- **engine** — the part of code about the *domain* (the kingdom, its rules). Doesn't know about IO, networks, UI. *Module 1.2.*
- **engine vs shell** — the discipline that separates domain logic (engine) from input/output (shell). The through-line of the entire course. *Module 1.2.*
- **entity** — a class EF maps to a table. *Module 2.6.*
- **event** — a thing that happened in the kingdom on a specific day. Stored as a small record. *Module 1.7.*
- **event log** — the in-order list of all events the kingdom has accumulated. *Module 1.7.*
- **EventEngine** — the class that rolls dice each tick to decide whether (and which) event happens. *Module 1.7; rewritten Module 1.8.*
- **exception** — a runtime error that the program couldn't handle and threw. *Module 0.8.*
- **extension method** — a static method whose first parameter has `this`; looks like an instance method at the call site. LINQ is built on these. *Module 1.6.*

## F

- **`[Fact]`** — xUnit attribute marking a single unit test method. *Module 1.3.*
- **factory method** — a static method returning an instance, used in place of a constructor (`Kingdom.LoadFrom(snap, ...)`). *Module 2.3.*
- **fake** (a.k.a. *mock*, *stub*) — a test-time stand-in for a real collaborator. *Module 1.8.*
- **FakeItEasy** — the .NET library we use for one-line fakes (`A.Fake<IRandom>()`). *Module 1.8.*
- **`File.ReadAllText`** — reads a file's entire contents as a string. *Module 0.4; Module 2.1 (with paths/encoding).*
- **`File.WriteAllText`** — writes a string to a file (creates if missing; overwrites if present). *Module 0.4; Module 2.1.*
- **`Find` (EF)** — returns the entity by primary key, `null` if missing (vs. `Single` which throws). *Module 2.9.*
- **foreign key** — a column whose value matches an `id` in another table. *Module 2.5.*
- **`for`** — counter-based loop (`for (int i = 0; i < n; i++)`). *Module 0.7.*
- **`foreach`** — loops through every item in a collection, one at a time. *Module 0.7.*

## G

- **game loop** — the loop that calls `Tick` over and over. Heart of every game. *Module 1.4.*
- **`GetType()`** — a method on every object that returns its runtime type; `.Name` gives the short string name. *Module 1.5.*
- **`git cherry-pick`** — apply a specific commit (by hash) to the current branch. *Module 1.10.*
- **`git reset --hard`** — destructive: discard uncommitted changes and rewind the branch pointer. *Module 1.10.*
- **`git stash`** — set aside uncommitted changes (recoverable with `git stash pop`). *Module 1.10.*
- **global using** — a `using` directive in `GlobalUsings.cs` that applies to every file in the project. *Module 1.9.*
- **`GROUP BY`** — SQL clause that collapses rows into one per distinct grouping value. Used with aggregates (`COUNT`, `SUM`, `AVG`). *Module 2.5.*

## H

## I

- **`IClock`** — interface for "what time is it." Production: `SystemClock`. Tests: a fake. *Module 1.8.*
- **`IInterface`** — convention for interface names (`IBuilding`, `IRandom`). *Module 0.5.*
- **`Include`** — EF Core method to eager-load a related navigation property (`db.Kingdoms.Include(k => k.Buildings)`). *Module 2.6.*
- **inheritance** — a class declares it inherits from another with `:`; gets all of the parent's fields/methods, can add more, can override `virtual` ones. *Module 1.5.*
- **`InlineData`** — xUnit attribute supplying one set of inputs to a `[Theory]`. *Module 1.3.*
- **`INNER JOIN`** — only rows that match on both sides of the JOIN. *Module 2.5.*
- **`int`** — a whole number (no decimal point); range about ±2 billion. *Module 0.5.*
- **`int.Parse(...)`** — turns a string of digits into an `int`; throws if not a valid integer. *Module 0.2.*
- **`int.TryParse(...)`** — *try* to parse, return `bool` for success, write the value via `out`. The standard pattern for unvalidated user input. *Module 2.10.*
- **interface** — a contract — method/property *shapes*, no bodies. Many classes can implement the same interface. *Module 1.8.*
- **`internal`** — visibility modifier; type/member visible inside the same assembly only. *Module 1.9.*
- **`IRandom`** — the engine's interface for random numbers. Production: `SystemRandom`. Tests: a fake. *Module 1.8.*

## J

- **JOIN** — combine rows from two tables on a matching condition. *Module 2.5.*
- **JSON** — JavaScript Object Notation; the universal text data format. *Module 2.2.*
- **`JsonSerializer`** — `System.Text.Json`'s API. `Serialize` (object → string), `Deserialize<T>` (string → T). *Module 2.2.*

## K

## L

- **lambda** — the `x => expr` syntax. A throwaway function written inline. *Module 1.6.*
- **`LEFT JOIN`** — every row from the left table; matching from the right or `NULL`. *Module 2.5.*
- **LINQ** — Language-Integrated Query: methods like `.Where`, `.Select`, `.OrderBy`, `.Sum` that work on any collection. *Module 0.7 (mention); Module 1.6 (depth).*
- **list** — an ordered collection of things you can add to, remove from, and loop through. *Module 0.3.*
- **`List<string>`** — a list specifically of strings. The `<string>` says "this is a list of strings"; you'll see `<int>`, `<Building>` later. *Module 0.3.*
- **`List<T>`** — generic ordered list of `T`s. *Module 0.7.*
- **loop** — a chunk of code that runs repeatedly until told to stop. *Module 0.2.*

## M

- **menu loop** — `while (true) { print menu; read input; dispatch; }` — the heart of every interactive shell. *Module 2.10.*
- **method** — a named chunk of code that does one thing. *Module 0.1 (called); Module 0.3 (defined); Module 0.6 (deep dive).*
- **migration** — a versioned, reversible schema change. Generated by `dotnet ef migrations add`. *Module 2.7.*
- **`Migrate()`** — apply all pending migrations to the DB (in code, equivalent of `dotnet ef database update`). *Module 2.7.*

## N

- **navigation property** — an entity property that points at a related entity or list of them; EF auto-loads via `Include`. *Module 2.6.*
- **noise word** — generic words like `Manager`, `Helper`, `Util`, `Data`, `Info` that don't tell you what the thing actually does. *Module 2.11.*
- **nullable** — a type that's also allowed to hold `null` (no value at all); written with a trailing `?`. *Module 0.5.*

## O

- **`OfType<T>()`** — LINQ method: filter a collection to items whose runtime type is `T`. *Module 1.6.*
- **organisation** — splitting code into multiple methods so each does one thing. *Module 0.3.*
- **`OrderBy` / `OrderByDescending`** — LINQ sort methods. *Module 1.6.*
- **ORM** — Object-Relational Mapper; translates between objects in memory and rows in a relational DB. EF Core, Dapper, etc. *Module 2.6.*
- **overload** — multiple methods with the same name but different parameter types. *Module 0.6.*
- **`override`** — keyword for replacing a `virtual` method in a subclass. The compiler insists on it (so you can't override by accident). *Module 1.5.*

## P

- **parameter** — the name of an input declared in a method definition (`string kingdom` in `Greet(string kingdom)`). *Module 0.6.*
- **parameters (SQL)** — `$name` / `@name` placeholders that send values separately from the query — defeats SQL injection. *Module 2.4.*
- **PascalCase** — convention for type names, method names, and properties (`Building`, `Console.WriteLine`). *Module 0.5.*
- **path** — a file's address on disk. Build with `Path.Combine(...)` for portability. *Module 2.1.*
- **`Path.Combine`** — joins path segments using the OS's separator. Always use this, not `+ "/"`. *Module 2.1.*
- **`PRIMARY KEY AUTOINCREMENT`** — SQL idiom for "give every row a unique id automatically." *Module 2.4.*
- **predicate** — a function returning `bool` (e.g., `b => b.Level > 1`). LINQ uses these everywhere. *Module 1.6.*
- **projection** — selecting only the columns you need (`.Select(k => new KingdomSlotInfo(...))`); EF generates SQL with only those columns. *Module 2.9.*
- **project reference** — a `.csproj` line saying "I depend on this other project." *Module 1.2.*
- **property-based testing** — assert that a property holds across many inputs, not just one specific case. `[Theory]+[InlineData]` is a manual seed. *Module 2.3.*

## Q

## R

- **`Random`** — the standard library class for random numbers. Powerful and dangerous — never use directly inside an engine. *Module 1.7.*
- **`record`** — C# keyword for a small immutable data class with auto value-equality, ToString, and deconstruction. Perfect for events. *Module 1.7.*
- **README** — the doc at the top of every repo. Four sections that matter: what / how to run / what you learned / what's next. *Module 0.4 (intro); Module 1.10 (depth).*
- **rename party** — a focused session that does only renames, nothing else. Atomic, reviewable, safe via IDE. *Module 2.11.*
- **rescue rule (git)** — read state (`git status`, `git log --oneline -10`) before you act on it. *Module 1.10.*
- **`ResourceLedger`** — the Kingdom engine's class that owns the `Dictionary<Resource, int>` and exposes `Get`/`Add`/`Spend`/`Snapshot`/`SetTo`. *Module 1.2; SetTo added Module 2.3.*
- **return value** — the value a method gives back; declared by the type before the method name. *Module 0.6.*
- **round-trip test** — save then load; assert the loaded model equals the original. *Module 2.3.*

## S

- **`save slot`** — one row in the kingdoms table; one save the player can pick from a list. *Module 2.9.*
- **schema drift** — when the database schema diverges from the code's model. Migrations exist to prevent this. *Module 2.7.*
- **seed** — a number passed to `Random` so two `Random(seed)` instances produce the same sequence. Used for reproducible runs. *Module 1.8.*
- **`SELECT` / `INSERT` / `UPDATE` / `DELETE`** — the four "do" SQL statements. *Module 2.4.*
- **serialise / deserialise** — turn an object into a string (or bytes) and back. *Module 2.2.*
- **shell** — the part of code that talks to the outside world (console, files, network, UI). Many shells, one engine. *Module 1.2.*
- **`Shouldly`** — a fluent assertions library. `value.ShouldBe(expected)` instead of `Assert.Equal(expected, value)`. *Module 1.3.*
- **`ShouldBe(...)`** — Shouldly's main assertion: throws with a clear message if the value doesn't match. *Module 1.3.*
- **side effect** — when a method changes state somewhere instead of (or in addition to) returning a value. `AdvanceDay()` is one big side effect. *Module 1.4.*
- **`Single` (LINQ)** — return the only element matching a predicate; throws if zero or many. *Module 2.6.*
- **snapshot** — a complete data picture of the kingdom at a moment in time (a record DTO). *Module 2.3.*
- **solution (`.slnx`)** — a file that groups related projects so they can be built together. *Module 1.2.*
- **SQL** — Structured Query Language; the language of relational databases. *Module 2.4.*
- **SQL injection** — what happens when you concatenate user input into SQL. Defeated by parameters. *Module 2.4.*
- **SQLite** — a self-contained SQL DB engine; one library, no server, the database is a file. *Module 2.4.*
- **`SqliteConnection`** — handle to a SQLite database file. Always wrap in `using`. *Module 2.4.*
- **`static`** — a method or field belonging to the type itself, not to any specific instance. *Module 0.6 (preview); Module 1.1 (depth).*
- **`string`** — a piece of text in code, in `"double quotes"`. *Module 0.1; Module 0.5 (formal).*
- **string interpolation** — the `$"..."` syntax that lets you stick variables inside a string with `{curly braces}`. *Module 0.1.*
- **subclass** (or *child class*) — a class that inherits from a base/parent class. *Module 1.5.*
- **sub-namespace** — `Kingdom.Engine.Buildings` is a sub-namespace of `Kingdom.Engine`. Same project, different bucket. *Module 1.9.*
- **`switch` expression** — modern pattern: `value switch { pattern => result, ... }`. Cleaner than if/else if ladders. *Module 1.7.*
- **`System.Text.Json`** — the modern .NET JSON library. *Module 2.2.*
- **`SystemClock`** — production implementation of `IClock`; returns `DateTime.UtcNow`. *Module 1.8.*
- **`SystemRandom`** — production implementation of `IRandom`; wraps `System.Random`. *Module 1.8.*

## T

- **table** — a grid of rows and columns; the unit of storage in a relational DB. *Module 2.4.*
- **`[Theory]` + `[InlineData]`** — xUnit attributes for parameterised tests: same logic, different inputs. *Module 1.3.*
- **`this` parameter** — the keyword on the first parameter of a static method that turns it into an extension method. *Module 1.6.*
- **`throw`** — the keyword for raising an exception. *Module 0.8.*
- **tick** — one step of game time. In our kingdom, one tick = one day. *Module 1.4.*
- **transaction** — a group of DB operations that all succeed or all fail together. *Module 2.9.*
- **`try / catch`** — wrap risky code in `try`; if it throws, the matching `catch` block runs. *Module 0.8.*
- **`try / finally`** — guarantees the `finally` block runs (e.g., test cleanup) even if the `try` throws. *Module 2.1.*
- **type** — what kind of value something is (number, text, true/false, date). *Module 0.5.*

## U

- **unit test** — a small piece of code that verifies one specific behavior of one specific method. *Module 1.3.*
- **`using` (resource)** — `using var x = new SqliteConnection(...)` — guarantees `Dispose` runs at end of scope. *Module 2.4.*
- **UTF-8** — text encoding; the default for ~everything written this decade. *Module 2.1.*

## V

- **variable** — a named place to store a piece of data so you can use it later. *Module 0.1.*
- **`virtual`** — a method that subclasses are *allowed* to override. The base class provides a default. *Module 1.4.*
- **`void`** — the special return type meaning "this method gives nothing back". *Module 0.6.*

## W

- **`Where`** — LINQ filter: keeps items the predicate likes. *Module 1.6.*
- **`when` (pattern)** — adds a predicate to a switch arm: `1 when k.Citizens.Count > 0 => ...`. *Module 1.7.*
- **`while`** — keyword starting a "keep doing this while the condition is true" loop. *Module 0.2.*
- **`WriteIndented`** — `JsonSerializerOptions` flag for human-readable (multi-line + indented) JSON. *Module 2.2.*

## X

- **XML doc comment** — `///` comment above a public type or method; the IDE shows it as a tooltip / IntelliSense entry. *Module 1.10.*

## Y

## Z
