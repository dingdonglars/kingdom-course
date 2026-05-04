# STANDARDS.md

> Single source of truth for code, naming, file, branch, commit, PR, and prose conventions for this project. Master copy lives in the workshop (`kingdom-curriculum`); copies flow to the course repo and into the learner's repo via the starter template. **Do not edit the copies — edit here, sync down.**

## C# code

Microsoft's official conventions, adopted as-is:

- **PascalCase** for types, methods, properties, public fields, namespaces.
- **camelCase** for locals, parameters.
- **`_camelCase`** for private fields (underscore prefix).
- **`I` prefix** for interfaces (`IBuilding`, `IRandom`).
- **`Async` suffix** on async methods (`SaveAsync`).
- **Namespaces match folders.** `Kingdom.Engine/Buildings/Farm.cs` → `namespace Kingdom.Engine.Buildings;`.

### Fluent / LINQ chains

Break before the dot when a chain has 3+ method calls, OR when a single line goes past ~80 characters. One method per line, dot leading, 4-space hanging indent. Reads like a story; easy to scan; easy to comment out a single step while debugging.

```csharp
// fine — short
var first = items.First();

// fine — two calls, fits a line
var top = items.OrderBy(i => i.Score).First();

// break it — three+ calls
return ctx.Kingdoms
    .AsNoTracking()
    .Where(k => k.OwnerSub == ownerSub)
    .OrderBy(k => k.Id)
    .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
    .ToList();
```

Same rule applies to TypeScript Promise chains and other fluent APIs.

## Test names

Format: `Method_Scenario_ExpectedBehavior`.

Examples:
- `Upgrade_WhenInsufficientGold_ReturnsFailure`
- `Tick_WhenFarmHasFarmer_AddsFood`
- `Save_WhenFileLocked_ThrowsIOException`

## JS / TS

- **camelCase** for symbols.
- **PascalCase** for components, classes.
- **UPPER_SNAKE_CASE** for constants.
- **kebab-case** for file names.

## Files & folders

- **kebab-case** for course content paths (`phase-1-console-kingdom`, `0.3-tiny-adventure`).
- **PascalCase** for C# project files matching their type (`Kingdom.Engine.csproj`).

## Branches

`phase-N/<short-topic>` (e.g., `phase-1/inheritance`, `phase-3/oauth`).

## Commits

- Imperative mood (*"add building upgrade tree"*, not *"added"*).
- Conventional-style prefix optional (`feat:`, `fix:`, `docs:`, `chore:`, `journal:`).
- Workshop-only convention for plan execution: `[P###-T##]` for plan-task work (uppercase P), `[P###-label]` for inter-task work.

## PRs

- **Imperative title.**
- **Body answers *what* and *why*.**
- **Post-gate (after M4):** body also includes an AI-assistance section (see "Post-gate AI-assistance section" below).

### Post-gate AI-assistance section (PR template addition)

After the AI Unlock Gate (end of M4 / Block 5), every PR includes:

```markdown
## AI assistance

- AI-assisted in this PR? **yes / no**
- If yes, which files / chunks?
  - `<file path>`: <brief description of what the AI did>
- For each AI-assisted chunk: I (the author) can explain every line.
```

This is honor-system. `/milestone-review` reads this section to seed Lars's viva at milestone PR review.

## Slack channel naming

- Singular nouns for state channels (`#help`).
- Plural for collections (`#milestones`).

## Plain language standard (course prose)

Course-content prose targets approximately **CEFR B1–B2** — short sentences, present tense, second person, no academic register, no needless idioms. The standard exists so difficult *language* doesn't block difficult *concepts*. Authoring conventions:

- **Idiom callouts.** When an idiom carries weight (cultural literacy the learner will need — *rubber duck debugging*, *yak shaving*), mark it with a callout that explains it.
  - Example: `> 💬 **Idiom — "rubber duck debugging":** explain your code aloud to an inanimate object. The act of explaining surfaces the bug.`
- **Pronunciation footnotes.** First time an unguessable term appears, give the pronunciation: *OAuth (oh-auth)*, *Kubernetes (koo-ber-net-eez)*, *queue (kyoo)*. Once per term, never again.
- **"Words to watch" sidebar.** Each lesson that introduces new terms includes a sidebar at the top — three to eight items, one line each. Every entry sources into `course/GLOSSARY.md`.

## Vocabulary in course prose (evolving list)

Athos is fifteen, bright, and new to programming. He is not used to industry jargon or AI-product vocabulary. The list below evolves while we author the course — add to it whenever a word feels too fancy and you find a simpler one.

### Avoid (use a plainer word, or rewrite)

These words add cost without earning their keep. Don't use them in lesson prose.

- *substrate* — say "what it runs on" or "underneath"
- *brevity* — say "keeping it short" or rewrite
- *capstone* — say "final" or "the last one" or describe what it actually is
- *gate* (as a noun for a milestone or unlock) — say "milestone" or "checkpoint" or describe
- *call site* — say "where it's called from" or "where you use it"
- *hook* (as in "the hook of the lesson") — never name it. If a lesson opens well, it does the work without announcing itself.
- *auto value-equality* — say "two records with the same fields are equal automatically"
- *anti-climax* — say "boring on purpose" or just describe the feeling
- *engineerable* — say "you can shape" or "you can change"
- *meta-cognition* — say "thinking about how you think"
- *AI-rot* — say "code you don't understand piles up"
- *under the hood* — say "underneath" or "behind the scenes"
- *battle-tested* — say "well-tested" or "used by lots of projects"
- *in lockstep* — say "together" or "matching"
- *compounding* — say "builds up over time"
- *the payoff* — say "what you get" or "the result"
- *idiom* (when used to mean a coding pattern) — say "the common way"
- *leverage* — say "useful" or rewrite

### Explain on first use, then OK (principle, not a closed list)

Words from the developer profession are allowed — that's the vocabulary Athos is here to learn. **The rule: explain in one sentence the first time the word appears in lesson prose, not just in the sidebar.** After that, use freely.

This applies to *every* developer-profession term, not only the examples below. The list isn't exhaustive and never will be — when you reach for a term you haven't introduced yet, that's the explanation moment.

A few representative examples of terms the curriculum introduces:

- *shell* (as in engine vs shell) — first use in Module 1.2
- *DTO* — first use in Module 2.2 ("a small data-only record for moving data across a boundary")
- *ORM* — first use in Module 2.6 ("a library that maps classes to database rows")
- *migration* (as in DB schema) — first use in Module 2.7
- *CORS* — first use in Module 4.2
- *idempotent* — first use in Module 3.1
- *aggregate root* — first use in Module 1.9
- *factory method* — first use in Module 2.3
- *coroutine* — first use in Module 5.5
- *MCP* (Model Context Protocol) — first use in Bonus B2.5

(Many more — every block introduces ten to twenty such terms. The discipline is the explain-on-first-use; the list above is illustration.)

### Fine to use freely

Words that *look* fancy but are unambiguous and have been taught in earlier modules: *engine, runtime, framework, library, package, repository, branch, commit, module, class, method, property, parameter, argument, return value, scope* (when used in the precise compiler sense), and any concept name explicitly taught (*interface, inheritance, polymorphism*).

### Workshop / journal / `STANDARDS.md` itself

Internal docs (workshop, journal, this file) may use the full jargon. The discipline is for the **Athos-facing** files: course lessons, course quizzes, course glossary, course READMEs, starter READMEs.

## Mechanical enforcement

- **`.editorconfig`** at every repo root — VS Code surfaces violations as you type.
- **`dotnet format`** in CI for the reference repo and recommended for the learner's repo.
- **ESLint + Prettier** for JS/TS (Phase 4 onward).
- **`markdownlint`** for course-content consistency.
- **Plain-language compliance pass** at the consistency audit (see spec §21.2). Hemingway-style review or a Claude pass that flags overly complex sentences.

## Privacy two-tier rule (reproduced from spec §15.7)

- **The learner's first name** does not appear in any learner-visible artefact (course repo, reference repo, learner's repo, screenshots, commits to course/reference). Workshop is exempt.
- **Social, school, family, personal context** does not appear in *any* repo, including the workshop.

## Language rule

The course is **English only**. AI may *receive* a question in Portuguese; AI's *authored output* and the learner's *produced* artefacts (code, journal, commits, PRs) stay English.
