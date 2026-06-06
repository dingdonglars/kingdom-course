# Bonus B1.4 — Reflection and Close

B1 ends with a paragraph. You changed a database in three lines, met SSMS, and your tests stayed green the whole time. Today's job is to write down what that proves — in your own words, in your own journal — and close the bonus.

The point of writing it down is not nice writing. It is that putting what happened into your own words turns it into something you remember. A change you did and forgot about fades. A change you described in your own words stays with you.

---

## Step 1 — write the paragraph

Open `journal/B1-what-i-learned.md`. There's a template waiting for you with three prompts:

1. **What did the swap take?**
2. **What does that prove?**
3. **What would you do differently if you started over today?**

Aim for about 150 words in total. Don't pad. The whole bonus is one big answer to the question *"did the engine-vs-shell rule pay off?"* and your paragraph is your answer.

A model answer (yours will say something different, in your own voice):

> Changing SQLite for SQL Server took three lines: a NuGet package change, the `UseSqlServer(...)` provider call, and the new connection string format. Then I regenerated migrations and all 71 tests passed unchanged. That proves the engine-vs-shell rule isn't just a nice phrase — it's a real pattern that survives a database swap. If I started over, I'd add a SQL Server provider check earlier, maybe somewhere in Phase 3, so the rule gets a real test sooner. But it would have held either way.

The model answer is about 110 words. That is roughly the size you are aiming for — clear, not padded.

## Step 2 — tag the bonus

Once your paragraph is written and committed to your journal, tag the bonus complete. This one's CLI-only — the panel doesn't have a button for tags:

```powershell
git tag b1-engine-vs-shell-on-the-database-complete
git push origin b1-engine-vs-shell-on-the-database-complete
```

There's no milestone PR for B1 — it is a bonus that stands on its own. The reflection paragraph is what you keep, and the tag is the marker.

## What's optional next

If B1 felt good, here are some natural next steps (none required):

- Try **Azure SQL Database** — the cloud version of SQL Server. Same provider call, different connection string. Same swap, now over the network instead of on your own machine.
- Try **PostgreSQL** to make it three databases. A different company, same three-line pattern.
- Read one chapter of *Designing Data-Intensive Applications* by Martin Kleppmann. The best book on this whole layer.

## What you just did

You finished a small, focused bonus. The whole arc was: install LocalDB, change three lines of config, watch every test pass, install SSMS to look at the result, and write down what it meant. Each step was small on purpose, because the point of the bonus was to turn the engine-vs-shell rule into something you could see and touch — not just a thing said back in Phase 1. The reflection paragraph in your journal is what you keep.

**Key concepts you can now name:**

- **engine-vs-shell, proved** — the discipline survives a real database swap
- **provider swap** — three lines of config plus migration regen
- **migration regen** — provider change means regenerating migration SQL
- **SSMS as a lifelong tool** — the GUI that travels with you

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this one — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without scrolling back up, from memory:

1. Name the four steps the arc took, in order.
2. Then say, in one sentence, what the whole thing proved.

<details><summary>Stuck? Open this to check yourself.</summary>

The four steps, in order:

1. Install LocalDB (B1.1).
2. Change three lines of config and regenerate migrations (B1.2).
3. Watch every test pass unchanged (B1.2, Step 4).
4. Open SSMS to look at the rows your engine wrote (B1.3).

What it proved: the engine-vs-shell rule is real — the database is a part you can swap out in a few lines, and the engine and its tests never notice.

</details>

## Wrap up

No quiz this lesson — the reflection paragraph is the artefact. So:

1. **Progress** — one line in `journal/progress.md`: `Module B1.4 — Reflection and Close — DATE — closed B1: 3-line DB swap, all tests green. Learnt: one sentence.`
2. **Commit and push** — your reflection paragraph plus the progress line, commit message `Module B1.4 done`, Sync.
3. **Post in `#wins`** — *"B1 closed."* with the URL of the commit (the tag from Step 2 is separate; it doesn't need its own #wins post).

Module 0.1 covers the why and the panel/CLI steps if you need a refresher.

## Next

You are done with B1. **B2 (Context Engineering)** is the deeper bonus — it names and improves what you have been doing with AI tools since the AI Unlock at Module 4.0.
