# Quiz answers — Module 2.2

## 1. b
The engine is the kingdom's logic. Adding `System.Text.Json` to it would force every shell — including Roblox (which can't even use it) and the future database shell (which uses SQL, not JSON) — to drag the dependency. The separate `Kingdom.Persistence` project lets each shell choose. Engine stays clean.

## 2. a
Data Transfer Object — a small data-only record built for crossing a boundary (disk, HTTP, queue). Distinct from the domain model. The shape on the wire and the shape in memory are different concerns; let them differ.

## 3. b
`record` gives free value-equality, ToString, deconstruction, and immutability — all the qualities you want at a serialisation boundary. `JsonSerializer` handles `record` naturally; no attributes needed. Classes work too, but with more ceremony.

## 4. a
`WriteIndented = true` produces multi-line JSON with indentation — readable in any editor. Useful during development. For network/storage use, set it to `false` to save bytes.

## 5. b
The summary is *intentionally* lossy — it captures only what we need for this lesson. Module 2.3 (round-trip tests) and onward will build a fuller snapshot. **Always start with the smallest useful DTO; grow it as the use cases demand.**