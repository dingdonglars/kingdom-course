# Module 3.9 — M4 Close + the AI Unlock

The kingdom is on the internet. Friends sign in with Google, save their progress, and play. Phase 3 is done. This module is three things at once: the **M4 milestone close** (wins log, before/after, the per-milestone ritual), **Claude Code's arrival** (account + install), and the **AI Unlock** (the moment the rules around AI-assisted code change). Until today you've worked without an AI assistant — Lars has been the friction-helper via Slack `#help`, under the 20-minute rule. Today Claude arrives, and on the same day the rules expand: you can ask the AI for *implementation* — provided you can explain every line you merge.

This is the most important transition in the year. The discipline you've built across Phases 0 through 3 — the engine-vs-shell rule, deterministic tests, reading-before-writing, naming things on purpose — is what makes today safe. AI with implementation rights is a power tool that only works if the discipline is already there. You've earned this.

> **Words to watch**
>
> - **AI Unlock** — the curriculum's named transition: AI from "friction-only" to "real collaborator"
> - **mode flag** — a single line in `CLAUDE.md` that the AI reads to decide its behaviour
> - **viva** (vee-vah) — a one-on-one oral defense of your code; mentor (Lars) asks "explain this line" at random
> - **AI-assistance section** — required block in every post-unlock PR description: which lines did the AI write, which did you?

---

## What's done

You've shipped:

- A console kingdom (Phase 1, M2)
- Persistence with EF Core, save slots, an interactive UI (Phase 2, M3)
- A live HTTP API on the internet, with Google sign-in, multi-user, integration tests, and auto-deploy (Phase 3, M4)

By the count of repos: four milestones reached, roughly twelve weeks of curriculum behind you, eighty-plus tests passing across engine, persistence, and API. **You are no longer a beginner.**

## The M4 milestone ritual

Same pattern as M2 and M3 — but louder, because M4 is the milestone where the AI Unlock takes effect:

1. **Refresh the README** at the repo root — re-walk the four sections from M0.4. *How to run* now needs the live URL plus the `dotnet user-secrets` step; *What I learned* gets a Phase 3 paragraph (HTTP, OpenAPI, OAuth, multi-user persistence, App Service deploy). Every milestone close circles back to the README — the discipline that started in M2 close is the discipline now.
2. Open `journal/wins.md`.
3. Write the M4 entry:

   ```markdown
   ## M4 — Phase 3 — Live API

   - Live URL: `https://kingdom-api-yourname.azurewebsites.net`
   - Friends can sign in via Google and play
   - 80+ tests pass; CI/CD redeploys on every push to `main`
   - Real production hygiene: secrets out of repo, HTTPS-only, structured logs

   **Before:** the kingdom died when the program closed.
   **After:**  the kingdom lives on the internet at a URL you can text.

   Posted to `#wins` on YYYY-MM-DD.
   ```

4. Take a screenshot of your live URL plus the Scalar UI. Post in `#wins`.
5. **Tag the milestone.** This one's CLI-only — the panel doesn't have a button for tags: `git tag m4-phase-3-complete && git push origin m4-phase-3-complete`
6. **Open the M4 PR.** On github.com → your `kingdom` repo → banner *"phase-3 had recent pushes — Compare & pull request"* (or *Pull requests → New pull request*, base `main`, compare `phase-3`). Title: `M4 — Phase 3 — Live API`. Body: this milestone's `wins.md` bullets + `**Reviewer:** @dingdonglars` + the AI-assistance section per the post-unlock template (empty for this PR — Phase 3 was pre-unlock — but the section's presence marks that the new world starts now). Lars reviews → Approves → you Merge → delete the `phase-3` branch. Locally: `git switch main && git pull`. (Full walkthrough: Module 1.10.)

## The AI Unlock

Claude arrived in `pre-unlock` mode (its default — what it reads from `CLAUDE.md` when first installed). The pre-unlock rules:

- **Don't:** write course exercise code, solve quizzes, refactor for you
- **OK when asked (limited):** friction (git messes, env setup, error messages you've already tried), concept explanations *after* you've tried, routine boilerplate
- **Always fine:** syntax lookups, naming suggestions, *"is this good practice?"*

Claude reads `Current mode: pre-unlock` and pushes back when you ask for implementation. **Today, you flip the flag to `post-unlock`.** The new picture:

- *Don't* still applies — but the line is *"don't lose the lesson,"* not *"don't write code"*
- *OK when asked* expands — implementation help is allowed, with a strict rule attached
- *Always fine* unchanged

### The post-unlock hard rule

> **You must be able to explain every line of AI-generated code before you merge it.**

Concretely, when you ask the AI for code, the AI's job changes too. Its responses end with:

> *"Before you merge this, walk me through what each line does. If you can't explain a line, ask me about it instead of merging it."*

This is the discipline that prevents the failure mode where you ship code that works but you don't understand. Code you don't understand can't be debugged, refactored, or extended later. **Explanation is the merge rule.**

### The PR template grows a section

Every post-unlock PR description includes an AI-assistance section:

```markdown
## AI assistance

- Bot: Claude / Copilot / Cursor / etc.
- Lines I wrote myself: [files / line ranges]
- Lines AI wrote (and I understand): [files / line ranges]
- Anything I'm unsure about: [be honest — flag for the mentor]
```

`/milestone-review` reads this section and prepares the viva from it. **You'll be asked to explain a random AI-written line.** If you can't, the merge gets pushed back — not as punishment but as a *"you're missing something; let's close that gap before it builds up over time."*

## Set up Claude Code — install + account

Before flipping the mode flag, Claude Code has to exist on your machine and be signed in to an account. Lars sits with you for this part — he puts his card on the subscription, but the account is in **your name and your email**, with a password only you know.

### 1. Create your Anthropic account, together with Lars

In a browser, go to <https://console.anthropic.com>. Sign up with your email; pick a password (only you know it); complete the email confirmation. Lars enters the card details for the subscription tier he picked. You're now the owner of an Anthropic account that Lars pays for.

### 2. Install Claude Code

Claude Code is the CLI form — a chat that runs in your terminal, in the same folder as your repo, with your code in context. Install it:

```powershell
npm install -g @anthropic-ai/claude-code
```

Verify:

```powershell
claude --version
```

You should see a version number.

### 3. Sign in

In Windows Terminal:

```powershell
cd C:\code\kingdom
claude
```

The first launch pops a browser to sign in to Anthropic. Sign in with the account you just created. After sign-in, the browser closes, the terminal shows a prompt, and Claude is alive in your kingdom folder.

### 4. Try one slash command

Type `/` in the Claude prompt. You'll see a list including `/explain-this-concept`, `/code-review`, `/stuck-on-error`, `/walk-through-code`. These came in with the day-1 kit (in `.claude/commands/`) and have been waiting for Claude to arrive.

Pick one. *"explain what `git push` actually does, briefly"* — `/explain-this-concept` is the right shape for that. Read what comes back. Type `/exit` (or `Ctrl + C`) to leave.

The full rules — what to ask, what not to ask, the three buckets — are in `ai-tools.md`. Read it tonight; you'll come back to it through the rest of the year.

> **Why Claude and not Copilot or ChatGPT?** Claude is what Lars uses, so it's what Lars can guide you on. Copilot and Cursor and ChatGPT are fine tools too — once you know the patterns the course teaches with Claude, the patterns transfer. The full *why* is in `ai-tools.md`.

---

## Flipping the mode flag — manual step

Open `CLAUDE.md` at the root of your kingdom repo (`C:\code\kingdom\CLAUDE.md`). Find the line:

```diff
-**Current mode: `pre-unlock`.**
+**Current mode: `post-unlock`.**
```

Change it. Save.

The next time you (or any AI agent) opens this project, the AI reads the new mode and behaves accordingly. **No code change required in the AI's prompt — the file flag is the contract.**

**Commit it.** *"[M4] AI Unlock — flip mode flag pre-unlock → post-unlock"*. (Source Control panel → stage → commit → Sync. Or CLI: `git add . && git commit -m "[M4] AI Unlock — flip mode flag pre-unlock → post-unlock" && git push`.)

## What stays the same

- **Hard rules unchanged:** no first names in your visible work, English only, no school/family inference.
- **Three buckets unchanged:** the boundary between yellow and red moved; the buckets themselves did not.
- **Mentor protocol unchanged:** 20-minute rule, weekly sync, milestone PR review.
- **Per-milestone ritual unchanged:** wins log, Slack post, before/after.

## Tinker

Compare the same prompt before and after the flip. Ask Claude *"write me a function that converts a Kingdom to a JSON string."* Pre-unlock: it pushes back. Post-unlock: it writes the function and asks you to explain each line.

Read `CLAUDE.md` end-to-end one more time. You'll come back to it many times this year.

Browse some "AI helped me" PRs in well-run open source projects. Notice the pattern: small commits, careful per-file review, the human author still owns the mental model. The AI helps; the author still understands the code.

## The through-line

The AI is a power tool, not an autopilot. Power tools require respect. The discipline you've built across Phases 0 through 3 — engine vs shell, deterministic tests, read-before-write, names that earn their keep — is what lets you wield the AI without losing yourself. Without that discipline, AI-assisted code is a liability. With it, AI-assisted code multiplies your output without multiplying your bugs.

## What you just did

You closed M4 — the most important milestone of the year. Your kingdom is on the internet, friends can sign in and play, and CI/CD redeploys on every push. Eighty-plus tests pass across three projects. You also flipped the AI Unlock: the mode flag in `CLAUDE.md` moved from `pre-unlock` to `post-unlock` in three repos, which means every AI agent that opens those projects from now on operates under the new rules. The hard rule stands: you must be able to explain every line of AI-generated code before merging it. Phase 4 begins with the AI on as a real collaborator — and with the discipline you've spent six months building, ready to make that change safe.

**Key concepts you can now name:**

- **AI Unlock** — the named transition; M4; today
- **mode flag** — single line in `CLAUDE.md` that controls AI behaviour
- **viva** — one-on-one oral defense at milestones; random-line explanation tests
- **AI-assistance PR section** — required post-unlock; what you wrote vs what AI wrote
- **explanation as merge rule** — you can't ship code you can't explain

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

> *You just shipped M4. Time for the ritual: README refresh, `wins.md` entry, `#wins` post, before/after one-liner. Then take the rest of the day off.*

## Next

**Phase 4 begins.** Phase 4 is the **browser kingdom** — your engine ported to a JavaScript/TypeScript outer layer, served alongside your API. With the AI Unlock now in effect, the iteration loop speeds up considerably.
