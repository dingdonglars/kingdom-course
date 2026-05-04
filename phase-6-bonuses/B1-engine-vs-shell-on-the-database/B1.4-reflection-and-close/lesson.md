# Bonus B1.4 — Reflection and Close

B1 ends with a paragraph. You swapped a database in three lines, met SSMS, and your tests stayed green throughout. Today's job is to write down what that proves — in your own words, in your own journal — and close the bonus.

The whole point of writing it down is not literary polish. It's that *the act of naming what just happened* turns the experience into a thing you remember. A swap you did and shrugged off fades. A swap you described in your own words sticks.

---

## Step 1 — write the paragraph

Open `journal/B1-what-i-learned.md`. There's a template waiting for you with three prompts:

1. **What did the swap take?**
2. **What does that prove?**
3. **What would you do differently if you started over today?**

Aim for about 150 words total. Don't pad. The whole bonus is one big answer to *"did the engine-vs-shell discipline pay off?"* and your paragraph is the verdict.

A model answer (yours will say something different, in your own voice):

> Swapping SQLite for SQL Server took three lines: a NuGet package change, the `UseSqlServer(...)` provider call, and the new connection string format. Then I regenerated migrations and all 71 tests passed unchanged. That proves the engine-vs-shell discipline isn't a slogan — it's a real pattern that survives a runtime swap. If I started over, I'd write a SQL Server provider check earlier, maybe somewhere in Phase 3, so the discipline gets a real stress test sooner. But the discipline would have held either way.

The model answer is ~110 words. That's roughly the size you're aiming for — *named*, not padded.

## Step 2 — tag the bonus

Once your paragraph is written and committed to your journal, tag the bonus complete:

```powershell
git tag b1-engine-vs-shell-on-the-database-complete
git push origin b1-engine-vs-shell-on-the-database-complete
```

There's no milestone PR for B1 — it's a self-contained bonus. The reflection paragraph is what you keep, and the tag is the marker.

## What's optional next

If B1 felt good, here are some natural follow-ups (none required):

- Try **Azure SQL Database** — the cloud-hosted version of SQL Server. Same provider call; different connection string. Same swap discipline, now over the network instead of localhost.
- Try **PostgreSQL** for the trifecta. Different vendor entirely, same three-line pattern.
- Read one chapter of *Designing Data-Intensive Applications* by Martin Kleppmann. Best book on this whole layer.

## What you just did

You finished a small, sharp bonus. The whole arc was: install LocalDB, change three lines of config, watch every test pass, install SSMS to look at the result, write down what it meant. Each step was deliberately small, because the bonus's whole point was to make the engine-vs-shell rule cash out as something you could see and touch — not just a thing said in lectures back in Phase 1. The reflection paragraph in your journal is what you keep.

**Key concepts you can now name:**

- **engine-vs-shell, proved** — the discipline survives a real database swap
- **provider swap** — three lines of config plus migration regen
- **migration regen** — provider change means regenerating migration SQL
- **SSMS as a lifelong tool** — the GUI that travels with you

## Next

You're done with B1. **B2 (Context Engineering)** is the deeper bonus — it names and sharpens what you've been doing with AI tools since the AI Unlock at Module 4.0.
