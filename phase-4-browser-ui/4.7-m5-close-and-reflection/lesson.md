# Module 4.7 — M5 Close + Phase 0 Reflection

The kingdom is in browsers. **Phase 4 done. Five milestones, four shells, one engine.** Today closes the phase with the per-milestone ritual and a quietly powerful exercise: re-read the code you wrote in Phase 0 — Spark Week — with current eyes. Eight months of growth, visible in one comparison.

> **Words to watch**
>
> - **reflection** — looking back at past code with current eyes.
> - **before/after** — the small one-liner you write at every milestone.
> - **wins log** — your milestone history in `journal/wins.md`.

---

## The reflection exercise

Open your Phase 0 code. The first toys you wrote — `RoastOMatic`, `NumberGuess`, `TinyAdventure`. Read them.

Pick *one* of those toys (the worst, ideally) and spend thirty minutes refactoring with current eyes. Don't add features — clean up. Look for variables that should have been constants. Methods that should be split. Names that don't pull weight. Comments that have rotted. Magic numbers. The thing you'd do *completely differently* now.

Save the diff. Commit with `[refactor] phase-0 <toy> — applying lessons from phases 1-4`. The commit *is* the proof that you can see the gap.

Don't refactor *all* of Phase 0 — that's busywork. One toy is the experiment. The point is *seeing the gap*.

## M5 milestone ritual

Same pattern as M2, M3, and M4. The README refresh comes first — that's the discipline; everything else follows.

First, **refresh the README** at the repo root. Re-walk the four sections from M0.4. *How to run* now needs the `vite dev` step alongside the API; *What I learned* gets a Phase 4 paragraph (the browser as a runtime, vanilla TS, fetch + render). Don't skip this — every milestone close circles back here.

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

Third, take a screenshot of your live URL plus the kingdom rendering, and post to `#wins` in Slack.

Fourth, tag the milestone. This one's CLI-only — the panel doesn't have a button for tags:

```powershell
git tag m5-phase-4-complete
git push origin m5-phase-4-complete
```

Fifth, open the milestone PR (your branch → mentor review). The PR description includes the AI-assistance section per the post-Unlock template. Lars reads it before the weekly sync.

## Tinker

Compare your `KingdomCard.ts` from M4.4 to your Phase 0 `Program.cs`. One file is twenty lines of clean TypeScript; the other is hundreds of lines doing one thing. The growth is real and visible.

Re-read `STANDARDS.md`. Does anything in there now feel obvious that didn't last year? That's mastery.

Re-read `CLAUDE.md`. Notice the mode is `post-unlock`. Has your AI use changed since Phase 4 started? What works? What doesn't? Jot two sentences in `journal/wins.md` if anything stands out.

Take a screenshot of your kingdom-on-browser and use it as the first commit message of Phase 5 (Roblox). Phase 4 → Phase 5 transition.

## What you just did

Phase 4 closed. The kingdom now plays in any browser, you wrote a Phase 0 reflection refactor that proves you can see the gap between old code and new, and the M5 ritual is on disk — `wins.md` entry, `#wins` Slack post, milestone tag pushed, PR opened. Four shells, one engine, eight months of growth. The exercise of re-reading old code with new eyes is one of the cheapest mastery checks there is — generic dissatisfaction with code is anxiety; specific dissatisfaction (*"I'd extract this method, rename this variable, drop this comment"*) is mastery.

**Key concepts you can now name:**

- **reflection** — looking back at past code with current eyes
- **before/after** — one-liner per milestone; reads as a story over time
- **wins log** — `journal/wins.md`; your milestone history
- **specific vs generic dissatisfaction** — the second is mastery, the first is anxiety

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

**Phase 5 begins.** Phase 5 is the Roblox port — your engine moves into a different runtime and a different language (Luau). Same engine vs shell discipline; the proof that the discipline was never about C#.
