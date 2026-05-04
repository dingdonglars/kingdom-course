# Challenge — M0 — *Spark Week*

Wraps **Phase 0 — Spark Week** (modules 0.0–0.4).

M0 is about structure, not behavior. The four toys you wrote each week run on their own — you saw them work when you `dotnet run`'d them. What this challenge confirms is that the repo is laid out the way the rest of the course expects, and that you ran the milestone ritual.

## What this verifies

| Check | Looks for |
| --- | --- |
| Toy folders | `RoastOMatic/`, `NumberGuess/`, `TinyAdventure/`, `Polish/` at the repo root |
| Each toy builds | A `.csproj` per toy; `dotnet build` succeeds |
| Repo README | `README.md` at the repo root |
| Wins entry | `journal/wins.md` exists with an M0 entry |

## What this skips

The toys themselves — whether the roast generator picks adjectives well, whether the guessing game ranges feel right. Those are visual checks during the lesson and a chat at milestone review.

## How to run

From your repo root:

```powershell
dotnet test path\to\challenges\M0\M0.Tests.csproj
```

Green = M0 met. Red usually means a toy folder is missing, won't build, or you forgot the README or wins entry. Read the failure message — the test names tell you exactly which check tripped.
