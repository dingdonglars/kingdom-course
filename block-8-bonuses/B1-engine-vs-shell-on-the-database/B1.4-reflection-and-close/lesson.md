# Bonus B1.4 — Reflection & Close

> **Hook:** B1 ends with a one-paragraph reflection. **The brevity is the lesson.** You swapped a database with three lines of config, met SSMS, and your tests stayed green. Write what that proves.

---

## Write the paragraph

Open `journal/B1-what-i-learned.md`. Three sentences:

1. **What did the swap take?**
2. **What does that prove?**
3. **What would you do differently if you started over today?**

Aim for ~150 words. **Don't pad.** The whole bonus is the answer to "did the discipline pay off"; your paragraph is the verdict.

A model answer (yours will differ):

> Swapping SQLite for SQL Server took three lines: a NuGet package change, the `UseSqlServer(...)` provider call, and the new connection string format. Then I regenerated migrations and all 71 tests passed unchanged. That proves the engine-vs-shell discipline isn't a slogan — it's a load-bearing pattern that survives a runtime swap. If I started over, I'd write the SQL Server provider check earlier (maybe at M3 instead of as a bonus), so the discipline gets a stress test sooner — but the discipline would have held either way.

## Tag the bonus

```powershell
git tag b1-engine-vs-shell-on-the-database-complete
git push origin b1-engine-vs-shell-on-the-database-complete
```

No milestone PR; this is a self-contained bonus. **The reflection is the artefact.**

## What's optional next

- Try **Azure SQL Database** (the cloud-hosted SQL Server). Same provider; different connection string. Same swap discipline, now over the network.
- Try **PostgreSQL** for the trifecta. Different vendor entirely.
- Read one chapter of *Designing Data-Intensive Applications* by Martin Kleppmann. Best book on this layer.

## Connect

You're done with B1. **B2 (Context Engineering) is the deeper bonus** — bigger payoff in your day-to-day AI use.