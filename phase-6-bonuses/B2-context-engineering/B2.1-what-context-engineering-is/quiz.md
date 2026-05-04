# Quiz — Bonus B2.1

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. The four-step frame for an AI interaction is...

- **a.** Prompt → context → output → eval
- **b.** Question → answer → review → done
- **c.** Code → test → ship → forget
- **d.** Plan → write → review → ship

## 2. Which three failure modes account for almost every bad AI output?

- **a.** Generic-tutorial output, invented APIs, drift from style
- **b.** Slow generation, expensive tokens, biased training data
- **c.** Wrong language, wrong file path, wrong syntax version
- **d.** None — modern AI is reliable enough that you can accept defaults

## 3. The lesson says "you don't control the AI; you control its context." What does that mean in practice?

- **a.** The actual generation step is opaque, but the prompt, the scaffolding, the scoping, and your eval are all yours — so spend effort there
- **b.** AI output is fundamentally broken and should be treated with suspicion at all times
- **c.** Performance is the main concern; context engineering optimises for speed
- **d.** Tradition — older developers prefer it this way

## 4. What's the difference between scaffolding and scoping?

- **a.** They're the same thing under two different names
- **b.** Scaffolding is persistent background (ARCHITECTURE, STANDARDS, examples). Scoping is per-task framing (goal, traps, style pointer for *this* request).
- **c.** Scoping is faster to set up than scaffolding
- **d.** Scaffolding is required by Claude; scoping is optional

## 5. What's the eval step?

- **a.** Your judgement on whether the output is right *for your project* — the AI doesn't know your standards, you do
- **b.** A performance benchmark run after every AI response
- **c.** A test framework that ships with most AI tools
- **d.** The AI's own self-assessment of its output

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
