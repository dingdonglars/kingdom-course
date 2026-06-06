# Module 3.9 — M4 Close + the AI Unlock

The kingdom is on the internet. Friends sign in with Google, save their progress, and play. Phase 3 is done. This module is three things at once: the **M4 milestone close** (wins log, before/after, the per-milestone ritual), **Claude Code's arrival** (account + install), and the **AI Unlock** (the moment the rules around AI-assisted code change). Until today you've worked without an AI assistant — Lars has been the one who helps when you're stuck, through Slack `#help`, under the 20-minute rule. Today Claude arrives, and on the same day the rules open up: you can ask the AI to *write code for you* — as long as you can explain every line you merge.

This is the most important change in the whole year. The discipline you've built across Phases 0 through 3 — the engine-vs-shell rule, tests that give the same result every time, reading before writing, naming things on purpose — is what makes today safe. An AI that can write your code is a power tool, and it only works well if that discipline is already there. You've earned this.

> **Words to watch**
>
> - **AI Unlock** — the course's named change: AI goes from "only when you're stuck" to "real collaborator"
> - **mode flag** — a single line in `CLAUDE.md` that the AI reads to decide how to behave
> - **viva** (vee-vah) — a one-on-one spoken check of your code; the mentor (Lars) picks a line and asks you to explain it
> - **AI-assistance section** — a required block in every post-unlock PR description: which lines did the AI write, and which did you write?

---

## What's done

You've shipped:

- A console kingdom (Phase 1, M2)
- Persistence with EF Core, save slots, an interactive UI (Phase 2, M3)
- A live HTTP API on the internet, with Google sign-in, multi-user, integration tests, and auto-deploy (Phase 3, M4)

Counting it up: four milestones reached, about twelve weeks of the course behind you, eighty-plus tests passing across the engine, persistence, and API. **You are no longer a beginner.**

## The M4 milestone ritual

Same pattern as M2 and M3 — but this one matters more, because M4 is the milestone where the AI Unlock takes effect:

1. **Refresh the README** at the repo root — go back through the four sections from Module 0.4. *How to run* now needs the live URL plus the `dotnet user-secrets` step; *What I learned* gets a Phase 3 paragraph (HTTP, OpenAPI, OAuth, multi-user persistence, App Service deploy). Every milestone close comes back to the README — the same habit you started at the M2 close.
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
6. **Open the M4 PR.** On github.com → your `kingdom` repo → the banner *"phase-3 had recent pushes — Compare & pull request"* (or *Pull requests → New pull request*, base `main`, compare `phase-3`). Title: `M4 — Phase 3 — Live API`. Body: this milestone's `wins.md` bullets + `**Reviewer:** @dingdonglars` + the AI-assistance section from the post-unlock template. (Leave the AI-assistance section empty for this PR — Phase 3 was pre-unlock — but include it anyway, to mark that the new rules start now.) Lars reviews → Approves → you Merge → delete the `phase-3` branch. On your machine: `git switch main && git pull`. (Full walkthrough: Module 1.10.)

## The AI Unlock

Claude arrived in `pre-unlock` mode (its default — what it reads from `CLAUDE.md` when first installed). The pre-unlock rules:

- **Don't:** write course exercise code, answer quizzes, or refactor for you
- **OK when asked (limited):** help when you're stuck (git tangles, environment setup, error messages you've already tried), explain a concept *after* you've tried, write routine boilerplate
- **Always fine:** look up syntax, suggest names, answer *"is this good practice?"*

Claude reads `Current mode: pre-unlock` and says no when you ask it to write your code. **Today, you change the flag to `post-unlock`.** The new picture:

- *Don't* still applies — but now the line is *"don't skip the learning,"* not *"don't write code"*
- *OK when asked* gets bigger — the AI can write code for you now, with one strict rule attached
- *Always fine* stays the same

### The post-unlock hard rule

> **You must be able to explain every line of AI-generated code before you merge it.**

In practice, when you ask the AI for code, the AI's job changes too. Its answers end with:

> *"Before you merge this, walk me through what each line does. If you can't explain a line, ask me about it instead of merging it."*

This is the habit that stops you from shipping code that works but that you don't understand. Code you don't understand can't be debugged, refactored, or built on later. **Being able to explain it is the rule for merging it.**

### The PR template grows a section

Every post-unlock PR description includes an AI-assistance section:

```markdown
## AI assistance

- Bot: Claude / Copilot / Cursor / etc.
- Lines I wrote myself: [files / line ranges]
- Lines AI wrote (and I understand): [files / line ranges]
- Anything I'm unsure about: [be honest — flag for the mentor]
```

`/milestone-review` reads this section and uses it to set up the viva. **You'll be asked to explain a line the AI wrote, picked at random.** If you can't, the merge waits — not as a punishment, but as a *"there's a gap here; let's close it before it builds up over time."*

## Set up Claude Code — install + account

Before you change the mode flag, Claude Code has to be on your machine and signed in to an account. Lars sits with you for this part — he puts his card on the subscription, but the account is in **your name and your email**, with a password only you know.

### 1. Create your Anthropic account, together with Lars

In a browser, go to <https://console.anthropic.com>. Sign up with your email, pick a password (only you know it), and finish the email confirmation. Lars enters the card details for the subscription level he picked. You now own an Anthropic account that Lars pays for.

### 2. Install Claude Code

Claude Code is the command-line version — a chat that runs in your terminal, in the same folder as your repo, with your code right there for it to read. Install it:

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

The first launch opens a browser to sign in to Anthropic. Sign in with the account you just created. After sign-in, the browser closes, the terminal shows a prompt, and Claude is running in your kingdom folder.

### 4. Try one slash command

Type `/` in the Claude prompt. You'll see a list that includes `/explain-this-concept`, `/code-review`, `/stuck-on-error`, `/walk-through-code`. These came in with the day-1 kit (in `.claude/commands/`) and have been waiting for Claude to arrive.

Pick one. *"explain what `git push` actually does, briefly"* — `/explain-this-concept` is the right one for that. Read what comes back. Type `/exit` (or `Ctrl + C`) to leave.

The full rules — what to ask, what not to ask, the three buckets — are in `ai-tools.md`. Read it tonight; you'll come back to it through the rest of the year.

> **Why Claude and not Copilot or ChatGPT?** Claude is what Lars uses, so it's what Lars can help you with. Copilot, Cursor, and ChatGPT are fine tools too — once you know the patterns the course teaches with Claude, those patterns carry over to the others. The full *why* is in `ai-tools.md`.

---

## Flipping the mode flag — manual step

Open `CLAUDE.md` at the root of your kingdom repo (`C:\code\kingdom\CLAUDE.md`). Find the line:

```diff
-**Current mode: `pre-unlock`.**
+**Current mode: `post-unlock`.**
```

Change it. Save.

The next time you (or any AI agent) opens this project, the AI reads the new mode and behaves the new way. **You don't have to change anything in the AI's prompt — this one line in the file is what controls it.**

**Commit it.** *"[M4] AI Unlock — flip mode flag pre-unlock → post-unlock"*. (Source Control panel → stage → commit → Sync. Or in the terminal: `git add . && git commit -m "[M4] AI Unlock — flip mode flag pre-unlock → post-unlock" && git push`.)

## What stays the same

- **Hard rules unchanged:** no first names in your visible work, English only, no school/family inference.
- **Three buckets unchanged:** the boundary between yellow and red moved; the buckets themselves did not.
- **Mentor protocol unchanged:** 20-minute rule, weekly sync, milestone PR review.
- **Per-milestone ritual unchanged:** wins log, Slack post, before/after.

## Tinker

Compare the same prompt before and after the change. Ask Claude *"write me a function that converts a Kingdom to a JSON string."* Before the unlock: it says no. After the unlock: it writes the function and asks you to explain each line.

Read `CLAUDE.md` from start to finish one more time. You'll come back to it many times this year.

Look at some "AI helped me" PRs in well-run open source projects. Notice the pattern: small commits, careful review file by file, and the human author still understanding the whole thing. The AI helps; the author still understands the code.

## The main point

The AI is a power tool, not an autopilot. Power tools need respect. The discipline you've built across Phases 0 through 3 — engine vs shell, tests that give the same result every time, read before write, clear names that pull their weight — is what lets you use the AI well and stay in control. Without that discipline, AI-written code becomes a problem. With it, the AI gets you more done without giving you more bugs.

## What you just did

You closed M4 — the most important milestone of the year. Your kingdom is on the internet, friends can sign in and play, and CI/CD redeploys on every push. Eighty-plus tests pass across three projects. You also did the AI Unlock: the mode flag in `CLAUDE.md` moved from `pre-unlock` to `post-unlock` in three repos, which means every AI agent that opens those projects from now on follows the new rules. The strict rule stands: you must be able to explain every line of AI-written code before you merge it. Phase 4 begins with the AI on as a real collaborator — and with the discipline you've spent six months building, ready to make that change safe.

**Key concepts you can now name:**

- **AI Unlock** — the named change; M4; today
- **mode flag** — a single line in `CLAUDE.md` that controls how the AI behaves
- **viva** — one-on-one spoken check at milestones; you explain a line picked at random
- **AI-assistance PR section** — required after the unlock; what you wrote vs what the AI wrote
- **explanation as the merge rule** — you can't ship code you can't explain

## On your own

Time to put the book away. Don't scroll back up to the steps — explain the AI Unlock from your own head. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

In your own words, answer three things on paper:

1. What one line in `CLAUDE.md` did you change today, and from what to what?
2. What is the one strict rule for AI-written code now?
3. What stays exactly the same after the unlock?

<details><summary>Stuck? Open this to check yourself.</summary>

1. You changed **`Current mode: pre-unlock`** to **`Current mode: post-unlock`**. That one line is what every AI agent reads to decide how to behave — you don't touch the AI's prompt, just the flag in the file.
2. The hard rule: **you must be able to explain every line of AI-written code before you merge it.** If you can't explain a line, you ask about it instead of merging it. Code you can't explain can't be debugged or built on later.
3. Unchanged: the hard rules (no first names in your visible work, English only), the three buckets (only the line between yellow and red moved), the mentor protocol (20-minute rule, weekly sync, milestone PR review), and the per-milestone ritual.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.9 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.9 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

> *You just shipped M4. Time for the ritual: README refresh, `wins.md` entry, `#wins` post, before/after one-liner. Then take the rest of the day off.*

## Next

**Phase 4 begins.** Phase 4 is the **browser kingdom** — your engine moved to a JavaScript/TypeScript outer layer, served alongside your API. With the AI Unlock now in effect, you'll be able to work through changes a lot faster.
