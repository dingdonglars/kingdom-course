# Phase 0 reflection — refactor one toy

Open one of your Phase 0 / Spark Week toys. Read it slowly. Notice everything you'd do differently now.

Don't add features. Just clean up:

- Variables that should be `const`
- Methods that should be split into smaller methods
- Names that don't pull weight (`thing`, `data`, `temp`)
- Comments that have rotted (the code did one thing; now it does another)
- Magic numbers (`if (score > 100)` — why 100?)
- The thing you'd structure completely differently

Commit your refactor as one or more commits with the prefix `[refactor]`. Each commit small and reviewable.

When you're done, write a 2-sentence reflection in your wins entry: *"What changed in my judgement between Spark Week and now?"*
