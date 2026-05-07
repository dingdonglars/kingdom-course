# Module 5.8 — Publish + M6 Close

The last technical step of the year is one button: *File → Publish to Roblox*. After that, a URL. After that, friends play. **One year. Five shells. One engine. M6.**

> **Words to watch**
>
> - **Publish** — upload your local `.rbxl` to Roblox. Produces a place ID.
> - **Place ID** — the unique URL of your game (`roblox.com/games/<id>/<name>`).
> - **Final reflection** — the sit-down you do with `journal/m6-looking-back.md`. About an hour. Quiet.
> - **M6** — the last numbered milestone of the year. You're a programmer who shipped five different versions of one engine.

---

## Step 1 — publish

In Studio: *File → Publish to Roblox* → *Create New* → name your place → save.

Once it's saved, you'll see the place's URL: `roblox.com/games/<id>/<your-place-name>`. Anyone with the link can visit. By default, the place is public and free to play.

If you want to keep it private while you polish: *Game Settings → Permissions → Friends only*, or set it to unlisted. You can flip it back to public when you're ready.

## Pre-publish checklist

Before you press the button, walk through this list once:

- [ ] Server scripts work in a Studio play test with no errors in Output.
- [ ] DataStore enabled in *Game Settings → Security*.
- [ ] Tested with *Test → Local Server → 2 players* — multiplayer works.
- [ ] Worth playing for at least sixty seconds without your friend giving up.
- [ ] Visual: tiles render, building click works, day counter ticks.
- [ ] Resources stay non-negative; no out-of-bounds bugs.
- [ ] Save on player leave; rejoin restores the saved state.
- [ ] No personal info in scripts (per the standards rule).

## Step 2 — send the URL

Pick three friends. Send them the URL with one sentence: *"I built this. Try it."*

Then sit with whatever happens. They might find a bug. They might suggest features. They might stop playing after five minutes. They might keep playing for an hour. Whatever happens, your code touched another human's day. **That's the standard all of this was for.**

## The M6 ritual

Same pattern as the previous milestones — bigger, and final:

1. **Refresh the README at the repo root one last time.** Re-walk the four sections from M0.4. The repo now spans five shells — *How to run* documents all of them; *What I learned* is a year-end paragraph; *What's next* is honest about whether you're continuing or stopping. This is the version someone clicks through to from the live game URL.
2. Open `journal/wins.md`. Write the M6 entry:

   ```markdown
   ## M6 — Phase 5 — Roblox-Published Kingdom

   - **Public Roblox URL:** roblox.com/games/<id>/<name>
   - 5 shells, one engine: console / file with JSON and SQLite / web API / browser / Roblox
   - Engine ported from C# to Luau
   - DataStore persistence; multiplayer-ready; friends played

   **Before:** I asked Lars what programming was
   **After:**  I built and shipped a multiplayer game

   Posted to `#wins` on YYYY-MM-DD.
   ```

3. **Tag the milestone.** This one's CLI-only — the panel doesn't have a button for tags: `git tag m6-phase-5-complete && git push origin m6-phase-5-complete`.
4. **Open the M6 PR.** On github.com → your `kingdom` repo → banner *"phase-5 had recent pushes — Compare & pull request"* (or *Pull requests → New pull request*, base `main`, compare `phase-5`). Title: `M6 — Phase 5 — Roblox-Published Kingdom`. Body: this milestone's `wins.md` bullets + `**Reviewer:** @dingdonglars` + the AI-assistance section per the post-Unlock template. This is the final PR; after Lars Approves and you've done the viva (next step), you Merge → delete the `phase-5` branch. (Full walkthrough: Module 1.10.)
5. Final viva with Lars — random-line walkthrough across the engine, plus a "tell me the story of one year" conversation.

## The final reflection (one sitting, about an hour)

Open `journal/m6-looking-back.md`. Answer freely:

1. **What's the one thing you'd want to teach someone else?** Pick the lesson that surprised you most. Write it as a two-paragraph blog post you'd send a friend who's about to start.
2. **What's the engine, in one paragraph?** Explain the through-line — engine vs shell — to a smart person who's never coded. Not what you built; what you *learned*.
3. **What's a project you'd start next?** Now that you have the toolset, what calls you? It doesn't have to be related to Kingdom. List three ideas; pick one; sketch a Phase-0-style "smallest interesting version."
4. **Re-read your Spark Week code.** Read `journal/wins.md` from M0 to M6 in order. Sit with it. Notice what's different about the way you think now.

## Tinker (these are *for fun* now)

- Add a leaderboard via Roblox's built-in `leaderstats` so every player sees the top kingdoms.
- Add a chat command (`/build farm`) — terminal nostalgia inside Roblox.
- Add a small progression: at 100 gold, unlock Lumberyard; at 500, unlock Mine.
- Convert one of the modules from M5.5 to use `RunService.Heartbeat` for a faster tick. Gameplay feels different.
- Learn one new thing on your own. Pick anything. The course gave you the tools to learn anything next.

## What you just did

You took the engine you wrote in Phase 1 and shipped it as a public Roblox place. The pre-publish checklist proved the place was actually playable; the publish click turned it into a URL anyone could click; the three messages turned the URL into a thing other people experienced. **One year. Five shells. One engine.** Console, file with JSON and SQLite, web API, browser, Roblox. Two languages, two runtimes, one model that didn't change. The point of the curriculum landed in your hands when you sent that URL.

**Key concepts you can now name:**

- *publish* — `File → Publish to Roblox`; produces a place ID
- *place ID* — the URL friends click to play your game
- *the M6 ritual* — wins entry, tag, milestone PR, viva
- *final reflection* — the hour-long sit-down with `m6-looking-back.md`
- *the through-line, lived* — five shells, one engine, two languages

## The through-line, one last time

> **The model is forever. The runtime is a detail.**
>
> You proved it five times — console, persistence, web API, browser, Roblox. The same `Building`, the same `ResourceLedger`, the same `Kingdom.AdvanceDay`. Different runtimes. Different shells. The pattern carries.
>
> **You're a programmer.** Past tense; future tense; both.

## Quiz

Open `quiz.md`. The last one. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

There's no next mandatory module. **You finished.**

If you want bonus arcs:

- **Phase 6 / B1** — DB engine internals (build a tiny database from scratch). Roughly ten weeks.
- **Phase 6 / B2** — context engineering (a deep, practical guide to AI-assisted coding). Roughly ten weeks.

If you want to start your own project: Phase 0 of anything works. Smallest interesting version, ship it, iterate. You know the loop now.

Either way: well done.
