# Module 5.8 — Publish + M6 Capstone Close

> **Hook:** the last technical step is one button click — `File → Publish to Roblox`. Then a URL. Then friends play. **One year. Five shells. One engine. M6.**

> **Words to watch**
> - **Publish** — upload your local `.rbxl` to Roblox; produces a place ID
> - **Place ID** — the unique URL of your game (`roblox.com/games/<id>/<name>`)
> - **Capstone** — the final reflection; the year wraps
> - **M6** — the last numbered milestone; you're a programmer who shipped five different versions of one engine

---

## Publish

In Studio: `File → Publish to Roblox` → "Create New" → name your place → save.

You'll see the place's URL: `roblox.com/games/<id>/<your-place-name>`. Anyone can visit. **By default it's public + free to play.**

If you want to keep it private while you polish: `Game Settings → Permissions → Friends only` (or unlisted).

## Pre-publish checklist

- [ ] Server scripts work in Studio play test (no errors in Output)
- [ ] DataStore enabled in Game Settings → Security
- [ ] Test with "Test → Local Server → 2 players" — ensure multiplayer works
- [ ] Worth playing for at least 60 seconds without your friend giving up
- [ ] Visual: tiles render, building click works, day counter ticks
- [ ] Resources stay non-negative; no out-of-bounds bugs
- [ ] Save on player leave; rejoin restores state
- [ ] No personal info in scripts (Athos's family, school, etc — per the standards rule)

## Send the URL

Pick three friends. Send them the URL with one sentence: *"I built this. Try it."*

**Sit with whatever happens.** They'll find bugs. They'll suggest features. They'll stop playing after 5 minutes. They'll keep playing for an hour. Whatever happens, your code touched another human's day. **That's the bar all of this was for.**

## M6 ritual (the capstone)

Same shape as M2/M3/M4/M5 — bigger:

1. Open `journal/wins.md`. Write the M6 entry:

   ```markdown
   ## M6 — Block 7 — Roblox-Published Kingdom

   - **Public Roblox URL:** roblox.com/games/<id>/<name>
   - 5 shells, one engine: console / file+JSON+SQL / web API / browser / Roblox
   - Engine ported from C# to Luau
   - DataStore persistence; multiplayer-ready; friends played

   **Before:** I asked Lars what programming was
   **After:**  I built and shipped a multiplayer game

   Posted to `#wins` on YYYY-MM-DD.
   ```

2. Tag locally: `git tag m6-block-7-complete && git push origin m6-block-7-complete`
3. Open the M6 milestone PR. **AI-assistance section in the description.**
4. Final viva with Lars — random-line explanation across the engine *and* a "tell me the story of one year" walk-through.

## The capstone reflection (one sitting, ~1 hour)

Open `journal/capstone.md`. Answer freely:

1. **What's the one thing you'd want to teach someone else?** Pick the lesson that surprised you most. Write it as a 2-paragraph blog post you'd send a friend who's about to start.
2. **What's the engine, in one paragraph?** Explain the through-line — engine vs shell — to a smart person who's never coded. Not what you built; what you *learned*.
3. **What's a project you'd start next?** Now that you have the toolset, what calls you? It doesn't have to be related to Kingdom. List 3 ideas; pick one; sketch a Phase-0-style "smallest interesting version."
4. **Re-read your Spark Week code.** Read `journal/wins.md` from M0 to M6 in order. **Sit with it.** Notice what's different about the way you think.

## Tinker (optional — these are *for fun* now)

- Add a leaderboard via Roblox's built-in `leaderstats` — every player sees the top kingdoms.
- Add a chat command (`/build farm`) — terminal nostalgia inside Roblox.
- Add a simple progression: at 100 gold, unlock Lumberyard. At 500, unlock Mine.
- Convert one of the modules from M5.5 to use `RunService.Heartbeat` for faster ticking. Gameplay feels different.
- **Learn one new thing on your own.** Pick anything. The course gave you the tools to learn anything next.

## Name it

- **Publish** — `File → Publish to Roblox`; produces a place ID.
- **Capstone** — the final reflection.
- **M6** — last numbered milestone; one engine, five shells, one year.

## The rule of the through-line — one last time

> **The model is forever. The runtime is a detail.**
>
> You proved it five times: console, persistence, web API, browser, Roblox. The same `Building`, the same `ResourceLedger`, the same `Kingdom.AdvanceDay`. Different runtimes. Different shells. The shape carries.
>
> **You're a programmer.** Past tense; future tense; both.

## Quiz / challenge

Open `quiz.md`. The last one.

## Connect

There's no next mandatory module. **You finished.**

If you want bonus arcs:
- **Block 8 / B1** — DB engine internals (build a tiny DB from scratch). 10 weeks.
- **Block 8 / B2** — context engineering (deep practical guide to AI-assisted coding). 10 weeks.

If you want to start your *own* project: Phase 0 of anything works. Smallest interesting version → ship → iterate. You know the loop now.

Either way: well done.