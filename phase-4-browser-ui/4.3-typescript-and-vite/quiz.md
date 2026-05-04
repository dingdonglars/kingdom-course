# Quiz — Module 4.3

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does TypeScript add to JavaScript?

- **a.** A faster runtime engine that ships with modern browsers
- **b.** Static type-checking — same JavaScript runs underneath, but the compiler refuses obvious type bugs
- **c.** Lower memory use during execution in modern browsers
- **d.** A full replacement for JavaScript with new keywords and syntax

## 2. What does Vite do?

- **a.** A type checker for JavaScript that runs in the editor
- **b.** Modern frontend dev server and bundler — fast HMR, TypeScript out of the box, near-zero config
- **c.** A testing framework for browser code, similar to Vitest
- **d.** A backend framework for serving APIs from JavaScript projects

## 3. What's HMR?

- **a.** Hard Memory Reset — clears the browser cache between runs
- **b.** Hot Module Replacement — edit, save, the browser updates without losing page state
- **c.** A debugging mode for tracking down hard-to-reproduce JavaScript bugs
- **d.** A required setting in HTTPS configurations on modern frontends

## 4. The `as KingdomSlot[]` cast on the parsed JSON — why is it needed?

- **a.** Required by JSON parsing in modern browsers and Node runtimes
- **b.** The compiler can't verify the runtime API layout; `as` is your assertion that the parsed object matches the interface
- **c.** It speeds up the parsing step measurably for large response bodies
- **d.** A stylistic preference; the code works the same way without it

## 5. Why is the rule "types at every boundary"?

- **a.** Boundaries are where layout mistakes happen — wire format, file format, API contract — and types make the contract explicit
- **b.** Required by the TypeScript compiler before it will produce output
- **c.** Search engines now reward sites with stricter type discipline at boundaries
- **d.** Faster execution at runtime when the boundary types are declared first

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
