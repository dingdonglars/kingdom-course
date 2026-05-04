# Milestone Challenges

Each milestone (M0–M6) ships with a `dotnet test`-runnable suite. Green means the milestone is met. There's no partial credit, but no penalty for taking the time you need to get there.

These suites do a quiet second job. From Phase 0 onward you *run* tests to verify your work — well before you write a test of your own. By the time Phase 1 explicitly teaches unit testing, the test runner is already familiar.

The challenges are smoke tests, not graders. They check structure, names, and that things build and run. They don't judge whether your code is *good* — that's the mentor's job at milestone review. Think of green as "the wiring is in place," and the conversation with Lars as "the work is worth shipping."

## Per-milestone folder

Each `M<n>/` subfolder contains:

- `README.md` — what the milestone tests cover, what they skip, how to run them.
- The test suite itself — a small `.csproj` + a few test files.

## How to run any milestone

From your repo root:

```powershell
dotnet test path\to\challenges\M<n>\M<n>.Tests.csproj
```

Replace `<n>` with the milestone number. Green = met. Red = something the README spells out is missing or broken.
