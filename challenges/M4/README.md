# Challenge — M4 — *Kingdom v3 (Live API)*

Wraps **Block 5 (Web API)**.

## What this checks

- `Kingdom.Api/` project exists and builds
- `Program.cs` registers OpenAPI (`/openapi/v1.json` is served)
- Auth-required endpoints return `401` when called without a session
- Unknown paths return `404`
- A `.github/workflows/*.yml` deploy workflow exists (signal of CI/CD setup)
- `journal/wins.md` has an M4 entry mentioning M4 / Block 5 / Live API

## What this does NOT check

- That the deploy actually worked (impossible to verify locally — but the workflow file's presence is the proof of intent)
- The exact set of endpoint paths (your API's shape can vary)
- That OAuth is wired (would require a real Google flow); the auth check just looks for the 401

## How to run

```powershell
dotnet test path\to\challenges\M4\M4.Tests.csproj
```

Green = M4 met. Run the per-milestone ritual: wins post + before/after + Discord.

**M4 also fires the AI Unlock** — flip `Current mode: pre-unlock` → `post-unlock` in `ai-context/CLAUDE.md` after the challenge passes.
