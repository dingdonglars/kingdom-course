# Module 3.9 — M4 Close + the AI Unlock

> **Hook:** the kingdom is on the internet. Friends sign in with Google, save, play. **Block 5 is done.** This module is two things at once: the **M4 milestone close** (wins log, before/after, the per-milestone ritual) — and the **AI Unlock**, the moment the rules around AI-assisted code change. From now on, you can ask the AI for *implementation* — provided you can explain every line you merge.

> **Words to watch**
> - **AI Unlock** — the curriculum's named transition: AI from "friction-only" to "real collaborator"
> - **mode flag** — a single line in `ai-context/CLAUDE.md` that the AI reads to decide its behavior
> - **viva** — a 1:1 oral defense of your code; mentor (Lars) asks "explain this line" — at random
> - **AI-assistance section** — required block in every post-unlock PR description: which lines did the AI write, and which did you?

---

## What's done

You've shipped:

- A console kingdom (Block 3, M2)
- Persistence with EF Core, save slots, an interactive UI (Block 4, M3)
- A live HTTP API on the internet, with Google sign-in, multi-user, integration tests, and auto-deploy (Block 5, M4)

By the count of repos: 4 milestones reached, ~12 weeks-of-curriculum under your belt, ~80+ tests passing across engine/persistence/API. **You are no longer a beginner.**

## The M4 milestone ritual

Same shape as M2, M3 — but louder, because M4 is the *gate*:

1. Open `journal/wins.md`.
2. Write the M4 entry:

   ```markdown
   ## M4 — Block 5 — Live API

   - Live URL: `https://kingdom-api-yourname.azurewebsites.net`
   - Friends can sign in via Google and play
   - 80+ tests pass; CI/CD redeploys on every push to `main`
   - Real production hygiene: secrets out of repo, HTTPS-only, structured logs

   **Before:** the kingdom died when the program closed
   **After:**  the kingdom lives on the internet at a URL you can text

   Posted to `#wins` on YYYY-MM-DD.
   ```

3. Take a screenshot of your live URL + Scalar UI. Post in `#wins`.
4. Tag locally: `git tag m4-block-5-complete && git push origin m4-block-5-complete`
5. Open the milestone PR (your code → mentor for the M4 review). Per the post-unlock template (next section), include the AI-assistance section even though it's empty for this PR.

## The AI Unlock

Until now, the AI was deliberately limited. The rules in `ai-context/CLAUDE.md`:

- **🟥 Don't**: write course exercise code · solve quizzes · refactor for you
- **🟨 OK when asked (limited)**: friction (git messes, env setup, error messages you've tried) · concept explanations *after* you've tried · routine boilerplate
- **🟩 Always fine**: syntax lookups · naming suggestions · "is this good practice?"

The AI reads its `Current mode: pre-unlock` and pushes back when you ask for implementation. **From M4 onwards, the mode flips.** The new picture:

- 🟥 still off-limits — but the bar is *"don't lose the lesson"* not *"don't write code"*
- 🟨 expanded — implementation help is allowed, with a strict rule
- 🟩 unchanged

### The post-unlock hard rule

> **You must be able to explain every line of AI-generated code before you merge it.**

Concretely, when you ask the AI for code, the AI's job changes too. Its responses end with:

> *"Before you merge this, walk me through what each line does. If you can't explain a line, ask me about it instead of merging it."*

This is the discipline that prevents *AI-rot* — the failure mode where you ship code that works but you don't understand. Code you don't understand can't be debugged, refactored, or extended. **Explanation is the merge gate.**

### The PR template grows a section

Every post-unlock PR description includes an AI-assistance section:

```markdown
## AI assistance

- Bot: Claude / Copilot / Cursor / etc.
- Lines I wrote myself: [files / line ranges]
- Lines AI wrote (and I understand): [files / line ranges]
- Anything I'm unsure about: [be honest — flag for the mentor]
```

`/milestone-review` reads this section and seeds the viva. **You'll be asked to explain a random AI-written line.** If you can't, the merge gets pushed back — not as punishment but as a "you're missing something; let's close the gap before it compounds."

## Flipping the mode flag — manual step

In **three** repos, find `ai-context/CLAUDE.md` and change line 7:

```diff
-**Current mode: `pre-unlock`.**
+**Current mode: `post-unlock`.**
```

The three repos:
- Workshop: `D:\Athos\kingdom-curriculum\ai-context\CLAUDE.md`
- Course: `D:\Athos\kingdom-course\ai-context\CLAUDE.md`
- Starter-template: `D:\Athos\kingdom-course\starter-template\ai-context\CLAUDE.md`
- (Reference repo has no `ai-context/`)

Commit each:

```powershell
git commit -am "[M4] AI Unlock — flip mode flag pre-unlock → post-unlock"
```

The next time you (or any teammate, or AI agent) opens any of these projects, the AI reads the new mode and behaves accordingly. **No code change required in the AI's prompt — the file flag is the contract.**

## What stays the same

- **Hard rules unchanged**: no first names in learner artefacts, English only, no school/family inference.
- **Three buckets unchanged**: just the boundary between yellow and red expanded.
- **Mentor protocol unchanged**: 20-minute rule, weekly sync, milestone PR review.
- **Per-milestone ritual unchanged**: wins log, Discord post, before/after.

## Tinker

- Compare the same prompt in pre-unlock vs post-unlock: ask Claude *"write me a function that converts a Kingdom to a JSON string."* Pre-gate: it pushes back. Post-gate: it writes the function and asks you to explain each line.
- Read `ai-context/CLAUDE.md` end-to-end one more time. **You'll be back to it many times.**
- Browse some "AI helped me" PRs in well-run open source projects. Notice the pattern: small commits, careful per-file review, the human author still owns the mental model.

## Name it

- **AI Unlock** — the named transition. M4. *Today.*
- **Mode flag** — single line in `CLAUDE.md` that controls AI behavior. Editable, gitable.
- **Viva** — 1:1 oral defense at milestones. Random-line explanation tests.
- **AI-assistance PR section** — required post-unlock; "what I wrote vs what AI wrote vs what I'm unsure about."
- **Explanation as merge gate** — you can't ship code you can't explain.

## The rule of the through-line

> **The AI is a power tool, not an autopilot.** Power tools require respect. The discipline you've built across Blocks 1-5 — engine vs shell, deterministic tests, reads-before-writes, names that earn their keep — *that's* what lets you wield the AI without losing yourself.

## Quiz / challenge

Open `quiz.md`.

## Connect

**Phase 4 begins.** Block 6 is the **browser kingdom** — your engine ported to a JavaScript/TypeScript shell, served alongside your API. With AI now unlocked, the iteration loop accelerates substantially.