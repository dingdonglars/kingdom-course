# Module 4.7 — M5 Close + Phase 0 Reflection

> **Hook:** the kingdom is in browsers. **Block 6 done. Five milestones, four shells, one engine.** Today closes the block with the per-milestone ritual — and a uniquely high-leverage exercise: **re-read the code you wrote in Phase 0 (Spark Week)**. Eight months of growth in one comparison.

> **Words to watch**
> - **reflection** — looking back at past code with current eyes
> - **before/after** — the small ritual of writing one for every milestone
> - **wins log** — your milestone history in `journal/wins.md`

---

## The reflection exercise

Open your Phase 0 / Spark Week code. The first toy you wrote — `RoastOMatic`, `NumberGuess`, `TinyAdventure`. Read it.

For *one* of those toys (pick the worst), spend 30 minutes refactoring with current eyes. Don't add features — clean up. Look for:

- Variables that should have been constants
- Methods that should be split
- Names that don't pull weight
- Comments that have rotted
- Magic numbers
- The thing you'd do *completely differently* now

**Save the diff.** Commit with `[refactor] phase-0 <toy> — applying lessons from Blocks 1-6`. The commit IS the artefact — proof that you can see the gap.

You don't need to refactor *all* of Phase 0 — that's busywork. One toy is the experiment. The lesson is *seeing the gap.*

## M5 milestone ritual

Same shape as M2/M3/M4:

1. Open `journal/wins.md`. Write the M5 entry:

   ```markdown
   ## M5 — Block 6 — Browser-Playable Kingdom

   - Live frontend URL: `https://kingdom-______.azurestaticapps.net`
   - Vite + TypeScript + componentised vanilla TS
   - Frontend tests in Vitest
   - 5 shells now: console, persistence, web API, browser
   - Phase 0 reflection: refactored `<toy>`; commit `<hash>`

   **Before:** Phase 5 didn't exist; the kingdom was a thing in my terminal
   **After:**  Friends play in their browser. I read my old Spark Week code and saw how far I've come.

   Posted to `#wins` on YYYY-MM-DD.
   ```

2. Take a screenshot of your live URL + the kingdom rendering.
3. Post to `#wins`.
4. Tag locally: `git tag m5-block-6-complete && git push origin m5-block-6-complete`
5. Open the milestone PR (your branch → mentor review). The PR description includes the AI-assistance section per post-gate template.

## Stretch — the optional Svelte taster

If you have time/energy left in the block (the curriculum allots Week 42 here), spend 4-6 hours porting `KingdomCard` to a **Svelte component**. You'll see:

- Templating with reactive `{name}` interpolation
- `<script>` block for component logic
- Single-file components (`.svelte`) that bundle HTML/CSS/JS together
- Compiler-driven reactivity (no virtual DOM)

It's a one-page mental model shift. Most learners decide they like Svelte; some prefer React (richer ecosystem); some go back to vanilla. **The taster is to give you the data point — not to switch frameworks.**

## Tinker

- Compare your `KingdomCard.ts` (M4.4) to your Phase 0 `Program.cs`. **One file is 20 lines of clean TS; the other is hundreds of lines doing one thing**. The improvement is real.
- Re-read `STANDARDS.md`. Does anything in there *now feel obvious*? That's mastery.
- Re-read `ai-context/CLAUDE.md`. Notice you're now on `post-gate`. Has your AI use changed? What works? What doesn't?
- Take a screenshot of your kingdom-on-browser + post it as your first commit message of Phase 5 (Roblox). Block 6 → Block 7 transition.

## Name it

- **Reflection** — looking back at past code with current eyes; finding the growth.
- **Before/after** — one-liner per milestone; reads as a story over time.
- **Wins log** — `journal/wins.md`; your milestone history.

## The rule of the through-line

> **Mastery is realising your old code is bad in *specific* ways.** Generic dissatisfaction is anxiety. Specific dissatisfaction — "I'd extract this method, rename this variable, drop this comment" — is mastery.

## M5 closes Block 6. Quiz / challenge

Open `quiz.md`.

## Connect

**Phase 5 begins.** Block 7 is the *bonus arc* — porting your engine to **Roblox + Luau**. A different runtime, a different language, **same model**. The proof that "engine vs shell" was never about C#.