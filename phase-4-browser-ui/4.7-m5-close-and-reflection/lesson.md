# Module 4.7 — M5 Close + Phase 0 Reflection

The kingdom runs in browsers. **Phase 4 done. Five milestones, four shells, one engine.** Today closes the phase with the milestone ritual and one quiet but powerful exercise: read the code you wrote in Phase 0 — Spark Week — again, now that you know much more. Eight months of progress, easy to see in one comparison.

> **Words to watch**
>
> - **reflection** — reading your old code again, now that you know more.
> - **before/after** — the short one-line note you write at every milestone.
> - **wins log** — your list of milestones in `journal/wins.md`.

---

## The reflection exercise

Open your Phase 0 code. The first toys you wrote — `RoastOMatic`, `NumberGuess`, `TinyAdventure`. Read them.

Pick *one* of those toys (the worst one, ideally) and spend thirty minutes cleaning it up, now that you know more. Don't add features — just improve what's there. Look for variables that should have been constants. Methods that should be split into smaller ones. Names that don't say what they mean. Comments that are now out of date. Magic numbers. The thing you'd do *completely differently* now.

Save the diff. Commit with `[refactor] phase-0 <toy> — applying lessons from phases 1-4`. That commit *is* the proof that you can see the difference between your old code and your new code.

Don't clean up *all* of Phase 0 — that's busywork. One toy is the experiment. The point is to *see the difference*.

## M5 milestone ritual

Same pattern as M2, M3, and M4. The README update comes first — that's the rule; everything else follows.

First, **update the README** at the repo root. Go through the four sections from Module 0.4 again. *How to run* now needs the `vite dev` step next to the API; *What I learned* gets a Phase 4 paragraph (the browser as a runtime, vanilla TS, fetch + render). Don't skip this — every milestone close comes back to this step.

Second, open `journal/wins.md` and write the M5 entry:

```markdown
## M5 — Phase 4 — Browser-Playable Kingdom

- Live frontend URL: `https://kingdom-______.azurestaticapps.net`
- Vite + TypeScript + componentised vanilla TS
- Frontend tests in Vitest
- Four shells now: console, persistence, web API, browser
- Phase 0 reflection: refactored `<toy>`; commit `<hash>`

**Before:** the kingdom was a thing in my terminal.
**After:**  friends play in their browser. I read my old Spark Week code and saw how far I've come.

Posted to `#wins` on YYYY-MM-DD.
```

Third, take a screenshot of your live URL with the kingdom rendered on it, and post it to `#wins` in Slack.

Fourth, tag the milestone. This step is CLI-only — the panel doesn't have a button for tags:

```powershell
git tag m5-phase-4-complete
git push origin m5-phase-4-complete
```

Fifth, **open the M5 PR.** On github.com → your `kingdom` repo → banner *"phase-4 had recent pushes — Compare & pull request"* (or *Pull requests → New pull request*, base `main`, compare `phase-4`). Title: `M5 — Phase 4 — Browser-Playable Kingdom`. Body: this milestone's `wins.md` bullets + `**Reviewer:** @dingdonglars` + the AI-assistance section from the post-Unlock template. Lars reads it before the weekly sync and Approves; then you Merge and delete the `phase-4` branch. On your machine: `git switch main && git pull`. (Full walkthrough: Module 1.10.)

## Tinker

Compare your `KingdomCard.ts` from Module 4.4 to your Phase 0 `Program.cs`. One file is twenty lines of clean TypeScript; the other is hundreds of lines all doing one thing. The progress is real, and you can see it.

Read `STANDARDS.md` again. Does anything in there feel obvious now that didn't a year ago? That's a sign of how much you've learned.

Read `CLAUDE.md` again. Notice the mode is `post-unlock`. Has the way you use the AI changed since Phase 4 started? What works well? What doesn't? Write two sentences in `journal/wins.md` if anything stands out.

Take a screenshot of your kingdom running in the browser and use it as the first commit message of Phase 5 (Roblox). It marks the move from Phase 4 to Phase 5.

## What you just did

Phase 4 closed. The kingdom now plays in any browser. You cleaned up one Phase 0 toy, which proves you can see the difference between your old code and your new code. The M5 ritual is on disk — `wins.md` entry, `#wins` Slack post, milestone tag pushed, PR opened. Four shells, one engine, eight months of progress. Reading old code again, now that you know more, is one of the cheapest ways to check how much you've learned. If you only feel a vague unease about old code, that's anxiety; if you can say exactly what you'd change (*"I'd pull this out into a method, rename this variable, delete this comment"*), that's real skill.

**Key concepts you can now name:**

- **reflection** — reading your old code again, now that you know more
- **before/after** — one line per milestone; together they tell your story over time
- **wins log** — `journal/wins.md`; your list of milestones
- **specific vs vague dissatisfaction** — knowing exactly what to change is skill; vague unease is just anxiety

## On your own

Time to put the book away. Don't scroll back up — open your old Phase 0 code one more time and pick three lines you would change. For each one, say the exact change out loud or write it down: not "this feels off" but "I would rename this variable to `goldCount`", "I would pull these lines into a method called `RollDice`", "I would delete this comment, it is out of date". No one marks this — it's just for you. It's the easiest way to see whether the difference between old and new code is real skill or just a vague feeling. Getting stuck here is completely fine — that's exactly what it's for.

<details><summary>Stuck? Open this to check yourself.</summary>

There's no single right answer here — it's your code. A good answer names the *exact* change, like these:

- "This `int x = 5;` should be a named constant — `const int StartingGold = 5;`."
- "These ten lines that pick a random number belong in their own method, `RollDice()`."
- "This variable `temp` should be `roastLine` — the name should say what it holds."

If your three changes are that specific, that's the skill the lesson is about. If all you can say is "it just feels messy," that's the vague unease — and naming even one concrete fix is how you turn it into skill.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.7 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

**Phase 5 begins.** Phase 5 is the Roblox port — your engine moves into a different runtime and a different language (Luau). Same engine-vs-shell idea as before; it's the proof that the idea was never really about C#.
