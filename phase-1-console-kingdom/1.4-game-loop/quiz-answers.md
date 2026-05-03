# Quiz answers — Module 1.4

## 1. b
`virtual` says "subclasses *may* override this." The compiler then requires the subclass to write `override` (so you can't override by accident).

## 2. b
`AdvanceDay()` mutates the kingdom in place. That's a *side effect* — by design here. The alternative (returning a brand-new `Kingdom` each tick) would mean copying the entire state, every tick. For a long-running game loop, mutation is the right trade.

## 3. c
Order matters. If citizens ate first, on Day 1 they'd be eating food that the day's farms haven't produced yet. The convention "produce, then consume" is what makes the math intuitive.

## 4. b
A *side effect* is when a method changes state somewhere — not just returns a value. `Spend` mutates the ledger. `AdvanceDay` mutates the kingdom. The opposite is a *pure function* (input → output, no state change).

## 5. a
The test verifies that `AdvanceDay` doesn't *crash* when there's no food. (`Spend` returns `false`; the engine just continues. Later, this is where you'd add a `CitizenStarved` event.)