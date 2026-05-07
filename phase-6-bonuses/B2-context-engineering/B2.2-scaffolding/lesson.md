# Bonus B2.2 — Scaffolding (Persistent Context)

Scaffolding is the *files* and *snippets* that make every AI interaction better, automatically. You set them up once, and they pay back every time you talk to the AI from that day on. You already have a decent scaffold — you've been adding to it since `ARCHITECTURE.md` landed at Module 4.0. Today's lesson is about auditing what you have, tightening it, and adding a couple of new pieces that earn their cost.

The mental model is simple. Each scaffold file is bytes the AI reads on every session. The cheaper the bytes (short, current, accurate), the better the output. The padding hurts twice — you pay tokens for it, and the AI has to read past it to get to the useful bits.

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

That's already a real scaffold. The audit isn't *"do you have the files?"* — you do. The audit is *"are they earning their cost?"*

For each file, ask three questions:

1. **Does it answer the AI's most-likely first question?** (*"What conventions does this project use?"* → STANDARDS. *"How are the projects connected?"* → ARCHITECTURE.) If a file doesn't answer a real first-question, it's filler.
2. **Is it short enough to be cheap context?** Each line is read every session. ARCHITECTURE.md should be 30–60 lines; STANDARDS.md around 100. Anything more is bloat.
3. **Is it current?** A scaffold file that lies wastes tokens *and* sends the AI confidently to the wrong answer. Update after every phase.

## Step 2 — add example files

A pattern that's worth adopting: keep small, hand-curated example files the AI can reference.

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

When you ask the AI to write a *similar* method, point it at this file: *"match the style of `examples/01-store-method.md`."* The output snaps to your pattern instantly. You spent ten minutes curating one example; you'll re-use it dozens of times.

Three to five examples cover most of what you'll ask for. Don't try to be exhaustive — pick the patterns you find yourself re-explaining.

## Step 3 — keep type files in one place

Where data shapes live, the AI can find them and stop guessing. In TypeScript projects, a single `types.ts` does this for you. In C#, the equivalent is keeping DTOs in one folder (`Dtos/`) with no other code in it. The AI reads the folder, knows your wire formats, and stops inventing fields.

This isn't a new file you write — it's a discipline about *where* you put things. Once the DTOs cluster in one place, the AI uses them right.

## Step 4 — add a "you are here" header to long files

Some teams put a short comment at the top of long files:

```csharp
// File: KingdomEfStore.cs
// Role: EF Core implementation of the kingdom store; CRUD plus slot listing
// Conventions: every method takes string ownerSub first; returns IReadOnly* for read methods
// See also: KingdomEntity, KingdomDbContext, ARCHITECTURE.md
```

Five lines, written once, doing context work for anyone — human or AI — who opens the file mid-task. Without it, the reader has to scroll and infer. With it, the reader knows the role before they read the first method.

This is optional, but useful for files over 200 lines or that mix several concerns.

## Tinker

Audit your `ARCHITECTURE.md` right now. Open it. Is it current? If anything in there has drifted since Module 4.0, update one line. *"Stale scaffolds are worse than no scaffolds"* — this is the discipline that prevents that.

Add a `.claude/examples/` folder with two or three hand-picked snippets. Pick patterns you've explained more than once.

Try a real prompt with and without the example file. Compare outputs. The difference is the value of the example.

## What you just did

You audited your scaffold (the persistent context the AI reads on every session), tightened any stale or padded files, and added two new pieces: an `examples/` folder with hand-picked snippets, and (optionally) a top-of-file orientation header on longer files. Each piece pays back forever — once it's in the repo, every future AI interaction benefits without you doing anything per-prompt. Roughly 20 minutes of curation; pays back across the lifetime of the project.

**Key concepts you can now name:**

- **scaffold file** — project doc the AI reads at session start
- **example file** — hand-curated snippet showing your pattern
- **type file** — single source of truth for data shapes
- **"you are here" header** — top-of-file orientation comment
- **stale scaffolds are worse than none** — audit and update each phase

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

B2.3 covers **scoping** — per-task framing. The other half of context engineering: what you say *for this specific request* on top of the scaffolding.
