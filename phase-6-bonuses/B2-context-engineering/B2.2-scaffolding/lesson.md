# Bonus B2.2 — Scaffolding (Persistent Context)

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

Scaffolding is the *files* and *snippets* that make every AI conversation better, on their own. You set them up once, and they pay back every time you talk to the AI from that day on. You already have a good scaffold — you have been adding to it since `ARCHITECTURE.md` appeared at Module 4.0. Today's lesson is about checking what you have, tightening it, and adding a couple of new pieces that are worth their cost.

The idea is simple. Each scaffold file is text the AI reads on every session. The cheaper the text (short, current, correct), the better the output. Padding hurts twice — you pay tokens for it, and the AI has to read past it to reach the useful parts.

> **Words to watch**
>
> - **scaffold file** — a project-level doc the AI reads at the start of a session
> - **`CLAUDE.md` / `AGENTS.md`** — convention files for AI tools to read first
> - **example file** — a curated *"look here for the pattern"* snippet
> - **type file** — a single source of truth for data shapes (DTOs, types, schemas)
> - **architecture doc** — a one-pager describing your projects and how data flows between them

---

## Step 1 — audit what you have

Open your repo. You should see most of these:

- `CLAUDE.md` (root) — auto-loaded by Claude Code; imports `CLAUDE.md`
- `STANDARDS.md` — code, naming, file conventions
- `CLAUDE.md` — AI-specific rules (mode flag plus behaviours)
- `ai-tools.md` — learner-facing notes on AI tooling
- `.claude/commands/` — slash commands you can type in Claude Code (`/explain-this-concept`, `/code-review`, `/stuck-on-error`, `/walk-through-code`, `/implementation-help`, `/lesson-review`, `/milestone-review`)
- `GLOSSARY.md` — project-specific vocabulary
- `ARCHITECTURE.md` — projects and data flow (added at Module 4.0)

That is already a real scaffold. The check isn't *"do you have the files?"* — you do. The check is *"are they worth their cost?"*

For each file, ask three questions:

1. **Does it answer the AI's most likely first question?** (*"What conventions does this project use?"* → STANDARDS. *"How are the projects connected?"* → ARCHITECTURE.) If a file doesn't answer a real first question, it is just filler.
2. **Is it short enough to be cheap context?** Each line is read every session. ARCHITECTURE.md should be 30–60 lines; STANDARDS.md around 100. Anything more is too much.
3. **Is it current?** A scaffold file that is wrong wastes tokens *and* sends the AI confidently to the wrong answer. Update it after every phase.

## Step 2 — add example files

A habit worth adopting: keep small, hand-picked example files the AI can look at.

Create `.claude/examples/01-store-method.md`:

````markdown
# Example: a store method

Pattern used in `KingdomEfStore`:

```csharp
public IReadOnlyList<KingdomSlotInfo> ListSlots(string ownerSub)
{
    using var ctx = new KingdomDbContext(_dbPath);
    return ctx.Kingdoms
        .AsNoTracking()
        .Where(k => k.OwnerSub == ownerSub)
        .OrderBy(k => k.Id)
        .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
        .ToList();
}
```

Notes:
- `ownerSub` is required for security
- `using var ctx` for disposal
- `AsNoTracking` for read-only queries
- Project to a small DTO with `.Select`
````

When you ask the AI to write a *similar* method, point it at this file: *"match the style of `examples/01-store-method.md`."* The output matches your pattern right away. You spent ten minutes making one example; you'll reuse it dozens of times.

Three to five examples cover most of what you'll ask for. Don't try to cover everything — pick the patterns you find yourself explaining again and again.

## Step 3 — keep type files in one place

When your data types live in one place, the AI can find them and stop guessing. In TypeScript projects, a single `types.ts` does this for you. In C#, the same idea is keeping DTOs in one folder (`Dtos/`) with no other code in it. The AI reads the folder, learns your data formats, and stops inventing fields.

This isn't a new file you write — it is a habit about *where* you put things. Once the DTOs are all in one place, the AI uses them correctly.

## Step 4 — add a "you are here" header to long files

Some teams put a short comment at the top of long files:

```csharp
// File: KingdomEfStore.cs
// Role: EF Core implementation of the kingdom store; CRUD plus slot listing
// Conventions: every method takes string ownerSub first; returns IReadOnly* for read methods
// See also: KingdomEntity, KingdomDbContext, ARCHITECTURE.md
```

Five lines, written once, helping anyone — human or AI — who opens the file in the middle of a task. Without it, the reader has to scroll and work it out. With it, the reader knows the role before they read the first method.

This is optional, but useful for files over 200 lines or files that do several different things.

## Tinker

Check your `ARCHITECTURE.md` right now. Open it. Is it current? If anything in there is out of date since Module 4.0, fix one line. *"A scaffold that is out of date is worse than no scaffold"* — this is the habit that prevents that.

Add a `.claude/examples/` folder with two or three hand-picked snippets. Pick patterns you have explained more than once.

Try a real prompt with and without the example file. Compare the outputs. The difference is the value of the example.

## What you just did

You checked your scaffold (the background context the AI reads on every session), tightened any files that were out of date or padded, and added two new pieces: an `examples/` folder with hand-picked snippets, and (optionally) a short header at the top of longer files. Each piece pays back forever — once it is in the repo, every future AI conversation benefits without you doing anything per-prompt. Roughly 20 minutes of work; it pays back across the whole life of the project.

**Key concepts you can now name:**

- **scaffold file** — project doc the AI reads at session start
- **example file** — hand-curated snippet showing your pattern
- **type file** — single source of truth for data shapes
- **"you are here" header** — top-of-file orientation comment
- **an out-of-date scaffold is worse than none** — check and update each phase

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this one — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without scrolling back up, run the audit on one real file. Pick one scaffold file in your repo (say `ARCHITECTURE.md` or `STANDARDS.md`), and from memory:

1. Ask it the three questions that decide whether a scaffold file is worth its cost.
2. Then answer them for that file.

<details><summary>Stuck? Open this to check yourself.</summary>

The three questions:

1. **Does it answer the AI's most likely first question?** If it doesn't answer a real first question, it is just filler.
2. **Is it short enough to be cheap context?** Every line is read every session, so keep it tight.
3. **Is it current?** A wrong scaffold file wastes tokens *and* sends the AI confidently to the wrong answer — an out-of-date scaffold is worse than none.

If your chosen file fails question 3, fix one line right now.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B2.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B2.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

B2.3 covers **scoping** — per-task framing. The other half of context engineering: what you say *for this one request* on top of the scaffolding.
