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
