# Challenge — M0 — *The Joke Toolbox*

Wraps **Block 1 (Spark Week)**.

## What this checks

That your repo by end of M0 has:

- Four toy folders at the repo root: `RoastOMatic/`, `NumberGuess/`, `TinyAdventure/`, `Polish/`.
- Each toy has a `.csproj` that builds (`dotnet build` succeeds).
- A `README.md` at the repo root (your M0 README).
- A `journal/wins.md` entry exists (per the milestone ritual).

## What this does NOT check

The toys themselves. We trust the lesson `dotnet run` checks. M0's challenge is *structure*, not behavior — *did you ship four toys with a README*.

## How to run

In your repo root:

```powershell
dotnet test path\to\challenges\M0\M0.Tests.csproj
```

Green = M0 met. Red = at least one toy folder is missing, won't build, or you forgot the README / wins entry.
</content>
</invoke>