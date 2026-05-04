# Quiz answers — Module 2.10

## 1. a
The pattern is universal: print options, read input, dispatch to a handler, repeat. Same shape in console UIs, REPLs, web servers (request → handler → response → repeat), event-driven UIs (event → handler → next event).

## 2. b
`Parse` throws `FormatException` on bad input — fine when you control the source, terrible for unvalidated user input. `TryParse` returns `false` and assigns the value via the `out` parameter. The standard pattern for "this *might* be a number; tell me if it isn't."

## 3. b
`null` — that's the EOF signal. Always handle it: `var line = Console.ReadLine(); if (line is null) return;`. Otherwise, in tests with redirected input that runs out, you'll loop forever (or get a NullReferenceException trying to call `.Trim()` on null).

## 4. b
Tests can't have a human typing answers. `Console.SetIn(new StringReader("1\nTest\n4\n"))` *scripts* the input — the program reads each line as if a user typed it. `Console.SetOut(new StringWriter())` captures everything the program wrote. You can then assert on the captured output.

## 5. a
The 5-line shell is the proof that engine/shell separation worked. The shell's only job is to broker between the user and the engine + store. If the shell grows past ~50 lines, something domain-flavored has leaked into it; refactor.