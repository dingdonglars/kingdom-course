# Quiz answers — Module 2.1

## 1. b
`Path.Combine` picks the OS's directory separator (`\` on Windows, `/` elsewhere). Your code is portable. Hard-coded `\\` or `/` will eventually bite when the code runs somewhere unexpected.

## 2. b
`Directory.CreateDirectory` is *idempotent* — calling it on an existing folder is a no-op (no exception). The same is true for nested paths: it creates any missing parent folders along the way.

## 3. b
Modern .NET writes UTF-8 *without* a BOM. That's the right choice for almost everything written this decade. Pass an explicit `Encoding` only when talking to a legacy system that needs something else.

## 4. b
`Path.GetTempPath()` returns the OS's temp folder. Tests writing there don't litter the project, the OS cleans up old files, and parallel test runs don't fight over the same path (combined with `Guid.NewGuid()` as a suffix, collisions are vanishingly rare).

## 5. a
This is the engine-vs-shell discipline made concrete. Adding persistence didn't require touching `Kingdom.Engine/`. The same engine is now savable — and the same engine will be HTTP-able (Phase 3), browser-able (Phase 4), Roblox-able (Phase 5). The boundary holds.