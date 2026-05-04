# Quiz — Bonus B2.2

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What's a scaffold file?

- **a.** A project-level doc the AI reads at session start (e.g. `ARCHITECTURE.md`, `STANDARDS.md`, `ai-context/CLAUDE.md`)
- **b.** A new C# project type used for AI-related work
- **c.** A test fixture file required by AI integration testing
- **d.** Any file that has the word "scaffold" in its name

## 2. Why is *short* a virtue for scaffold files?

- **a.** Each line is read every session — padding wastes tokens and slows down the parts the AI actually needs
- **b.** Files run faster when they're short, even on disk
- **c.** Required by Claude; longer files are rejected
- **d.** Personal style preference, not a real constraint

## 3. What does an example file in `ai-context/examples/` give you?

- **a.** A hand-picked, in-style snippet you can point the AI at — *"match the style of this example"*
- **b.** A set of test cases the AI can run against your code
- **c.** Documentation for human readers only; the AI doesn't read these
- **d.** Nothing useful; the AI ignores files outside its standard locations

## 4. Why is "stale scaffolds are worse than no scaffolds" a real warning?

- **a.** A wrong scaffold misleads the AI confidently — it follows it to the wrong answer rather than asking
- **b.** Stale scaffolds look unprofessional in a code review
- **c.** Required by the .NET runtime to be current
- **d.** Stale scaffolds run slower than current ones

## 5. What does the "you are here" file header do?

- **a.** A five-line top-of-file comment naming the file's role, conventions, and related files — orientation for any reader who opens it mid-task
- **b.** A required header demanded by Visual Studio for class files
- **c.** A search-engine optimisation tactic for code repositories
- **d.** Nothing special; it's just a comment

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
