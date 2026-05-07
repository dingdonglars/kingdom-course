# M3 rename-party checklist

Walk your engine, persistence, and console projects with the **five questions** open:

1. Does the name say what the thing **is**?
2. Could a fresh reader guess what it does?
3. Is the scope right (terse for short scope, long for big scope)?
4. Any noise words (Manager, Helper, Util, Data, Info)?
5. Does it match its neighbours' vocabulary?

## Engine

- [ ] `Building` / `Farm` / `Lumberyard` / `Mine` — already crisp; leave
- [ ] `Citizen` — leave
- [ ] `Resource` enum — leave
- [ ] `ResourceLedger` — pretty good; could it be `Treasury`? *Decide.*
- [ ] `KingdomEvent` + record subtypes (`TraderArrived`, `CitizenIll`, `BuildingBurned`) — already verb-past-tense or noun-state; great
- [ ] `EventEngine` — has the noise word `Engine`. Better: `EventRoller`? *Decide.*
- [ ] `IRandom` / `IClock` — small, intentional
- [ ] `SystemRandom` / `SystemClock` — convention — fine
- [ ] `KingdomSnapshot` / `BuildingSnapshot` / `CitizenSnapshot` — clear; leave

## Persistence

- [ ] `KingdomSummary` (M2.2) vs `KingdomSnapshot` (engine, M2.3) — *both have a reason to exist*. Make sure the comments/XML docs make the distinction clear.
- [ ] `KingdomJsonStore` — fine
- [ ] `SqliteDemo` / `SqliteJoinsDemo` — `*Demo` is a noise word in production code, but accurate for these (they exist to demonstrate). **Leave.**
- [ ] `KingdomEntity` / `BuildingEntity` — fine
- [ ] `KingdomDbContext` — convention; fine
- [ ] `KingdomEfStore` — fine
- [ ] `KingdomEfStore.EnsureCreated` — the body now calls `Migrate()`; the contract is "make sure the DB is ready." Name still earns. **Leave.**
- [ ] `KingdomEfStore.ListAll` — only used in tests, redundant with `ListSlots`. **Drop or rename.**
- [ ] `KingdomSlotInfo` — fine

## Console

- [ ] `SaveSlotUI` — fine
- [ ] Helpers inside `SaveSlotUI` (`NewKingdom`, `LoadKingdom`, `DeleteSlot`, `PlayLoop`, `ShowSlots`) — verb-noun; clear; leave

---

## After each rename
- [ ] `dotnet build` — 0 errors
- [ ] `dotnet test` — 71 passing
- [ ] Commit: `git commit -am "[M3-rename] <what + why>"`

## When done
- [ ] **Refresh the README at the repo root** — re-walk the four sections from M0.4 (*what / how to run / what I learned / what's next*). The kingdom now persists across four backends; the *How to run* and *What I learned* sections both need a paragraph that didn't exist at M2 close.
- [ ] Append to `wins.md`
- [ ] Post to `#wins`
- [ ] Tag: `git tag m3-block-4-complete`
- [ ] Open the M3 PR (`phase-2 → main`) on github.com — title `M3 — Phase 2 — Persistence`; body has wins bullets + `**Reviewer:** @dingdonglars`
- [ ] Lars Approves → you Merge → delete the `phase-2` branch
- [ ] Locally: `git switch main && git pull`
