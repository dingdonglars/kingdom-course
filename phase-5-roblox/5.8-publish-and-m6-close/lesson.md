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

Then sit with whatever happens. They might find a bug. They might suggest features. They might stop playing after five minutes. They might keep playing for an hour. Whatever happens, your code became part of another person's day. **That is what all of this was for.**

## The M6 ritual

Same steps as the milestones before — bigger, and the last one:

1. **Update the README at the repo root one last time.** Go through the four sections from Module 0.4 again. The repo now has five shells — *How to run* should cover all of them; *What I learned* is a paragraph about the whole year; *What's next* is honest about whether you're carrying on or stopping. This is the version people will read when they click through from the live game URL.
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
4. **Open the M6 PR.** On github.com → your `kingdom` repo → banner *"phase-5 had recent pushes — Compare & pull request"* (or *Pull requests → New pull request*, base `main`, compare `phase-5`). Title: `M6 — Phase 5 — Roblox-Published Kingdom`. Body: this milestone's `wins.md` bullets + `**Reviewer:** @dingdonglars` + the AI-assistance section from the post-Unlock template. This is the final PR. After Lars approves and you've done the viva (next step), you Merge and delete the `phase-5` branch. (Full walkthrough: Module 1.10.)
5. Final viva with Lars — he picks random lines from the engine and asks you to explain them, plus a "tell me the story of one year" conversation.

## The final reflection (one sitting, about an hour)

Open `journal/m6-looking-back.md`. Answer freely:

1. **What's the one thing you'd want to teach someone else?** Pick the lesson that surprised you most. Write it as a two-paragraph blog post you'd send a friend who's about to start.
2. **What's the engine, in one paragraph?** Explain the through-line — engine vs shell — to a smart person who's never coded. Not what you built; what you *learned*.
3. **What's a project you'd start next?** Now that you have the tools, what do you want to build? It doesn't have to be about Kingdom. List three ideas, pick one, and sketch a Phase-0-style "smallest interesting version."
4. **Re-read your Spark Week code.** Read `journal/wins.md` from M0 to M6 in order. Sit with it. Notice what's different about the way you think now.

## Tinker (these are *for fun* now)

- Add a leaderboard with Roblox's built-in `leaderstats` so every player sees the top kingdoms.
- Add a chat command (`/build farm`) — a little reminder of the terminal, now inside Roblox.
- Add a small progression: at 100 gold, unlock the Lumberyard; at 500, unlock the Mine.
- Change one of the loops from Module 5.5 to use `RunService.Heartbeat` for a faster tick. The game feels different.
- Learn one new thing on your own. Pick anything. The course gave you the tools to learn whatever comes next.

## What you just did

You took the engine you wrote in Phase 1 and published it as a public Roblox place. The pre-publish checklist showed the place was really playable. The publish click turned it into a URL anyone could open. The three messages turned that URL into something other people played. **One year. Five shells. One engine.** Console, file with JSON and SQLite, web API, browser, Roblox. Two languages, two places to run, one model that never changed. The whole point of the course became real in your hands when you sent that URL.

**Key concepts you can now name:**

- *publish* — `File → Publish to Roblox`; produces a place ID
- *place ID* — the URL friends click to play your game
- *the M6 ritual* — wins entry, tag, milestone PR, viva
- *final reflection* — the hour-long sit-down with `m6-looking-back.md`
- *the main idea, proven* — five shells, one engine, two languages

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the last big move stuck. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without looking, say the Studio menu path that turns your local place into a URL friends can open. Then say the one sentence the whole course was proving — the engine vs where it runs.

<details><summary>Stuck? Open this to check yourself.</summary>

The publish path: **File → Publish to Roblox** → *Create New* → name the place → save. After that you get a URL, `roblox.com/games/<id>/<name>`, that anyone can open.

The one idea: **the model lasts; where it runs is just a detail.** The same engine ran in five shells — console, file with JSON and SQLite, web API, browser, Roblox — and the `Building`, `ResourceLedger`, and `Kingdom` never changed.

</details>

## The main idea, one last time

> **The model lasts. Where it runs is just a detail.**
>
> You proved it five times — console, saving to a file, web API, browser, Roblox. The same `Building`, the same `ResourceLedger`, the same `Kingdom.AdvanceDay`. Different places to run. Different shells. The same model every time.
>
> **You're a programmer.** Past tense; future tense; both.

## Wrap up

1. **Quiz** — open `quiz.md` — the last one. Jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 5.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 5.8 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

There's no next mandatory module. **You finished.**

If you want bonus tracks:

- **Phase 6 / B1** — how databases work inside (build a tiny database from scratch). About ten weeks.
- **Phase 6 / B2** — context engineering (a deep, hands-on guide to AI-assisted coding). About ten weeks.

If you want to start your own project: Phase 0 of anything works. Build the smallest interesting version, publish it, then improve it. You know the loop now.

Either way: well done.
