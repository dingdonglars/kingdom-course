# Bonus B2.2 — Scaffolding (Persistent Context)

> **Hook:** scaffolding is the *files* and *snippets* that make every AI interaction better, automatically. You set them up once; they pay back forever. Today: audit and tighten yours.

> **Words to watch**
> - **scaffold file** — a project-level doc the AI reads on session start
> - **`CLAUDE.md` / `AGENTS.md`** — convention files for AI tools to read first
> - **example file** — a curated "look here for the pattern" snippet
> - **type file (`types.ts`)** — single source of truth for data shapes
> - **architecture doc** — one-pager describing the projects + data flow

---

## What you already have

Open your repo. You should see:

- `STANDARDS.md` — conventions
- `ai-context/CLAUDE.md` — AI rules (mode flag + behaviors)
- `ai-context/for-you.md` — learner-facing
- `ai-context/prompts/` — prompt templates
- `GLOSSARY.md` — project-specific vocabulary
- `ARCHITECTURE.md` — projects + data flow (added at M4.0)

**That's already a good scaffold.** B2.2 is about tightening it.

## The audit

For each scaffold file, ask:

1. **Does it answer the AI's most-likely first question?** ("What conventions does this project use?" → STANDARDS. "How are the projects connected?" → ARCHITECTURE.)
2. **Is it short enough to be cheap context?** Each file is bytes the AI reads. ARCHITECTURE.md should be 30-60 lines; STANDARDS.md ~100. Padding hurts.
3. **Is it current?** A scaffold doc that lies wastes tokens *and* misleads. Update it after every block.

## Add: example files

A new pattern: keep small, hand-curated example files the AI can reference.

`ai-context/examples/01-store-method.md`:

```markdown
# Example: a store method

Pattern used in `KingdomEfStore`:

```csharp
public IReadOnlyList<KingdomSlotInfo> ListSlots(string ownerSub)
{
    using var ctx = new KingdomDbContext(_dbPath);
    return ctx.Kingdoms.AsNoTracking()
        .Where(k => k.OwnerSub == ownerSub)
        .OrderBy(k => k.Id)
        .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
        .ToList();
}
```

Notes:
- `ownerSub` is *required* (security)
- `using var ctx` for disposal
- `AsNoTracking` for read-only
- Project to a small DTO with `.Select`
```

When you ask Claude to write a *similar* method, point it at this file: *"Match the style of `examples/01-store-method.md`."* The output snaps to your pattern instantly.

## Add: type files

For TypeScript projects, a single `types.ts` is doing context engineering for you. Same idea in C#: keep DTOs in one folder (`Dtos/`) with no other code. The AI reads them and knows your wire shapes.

## Add: a "you are here" header

Some teams put a comment at the top of long files:

```csharp
// File: KingdomEfStore.cs
// Role: EF Core implementation of the kingdom store; CRUD + slot listing
// Conventions: every method takes string ownerSub first; returns IReadOnly* for read methods
// See also: KingdomEntity, KingdomDbContext, ARCHITECTURE.md
```

5 lines; massive context hint to anyone (human or AI) opening the file mid-task.

## Tinker

- Audit your `ARCHITECTURE.md`. Is it current? Update one line.
- Add a `ai-context/examples/` folder with 2-3 hand-picked snippets.
- Try a real prompt with + without the example file. Compare outputs.

## Name it

- **Scaffold file** — project doc the AI reads.
- **Example file** — hand-curated snippet showing the pattern.
- **Type file** — single source of truth for data shapes.
- **"You are here" header** — top-of-file orientation comment.

## The rule of the through-line

> **Cheap context now. Expensive misunderstanding later.** The 20 minutes you spend curating scaffolding pay back in every AI interaction for the lifetime of the project.

## Quiz / challenge

Open `quiz.md`.

## Connect

B2.3 is **scoping** — per-task framing. The other half of context engineering: what you say *for this specific request* on top of the scaffolding.