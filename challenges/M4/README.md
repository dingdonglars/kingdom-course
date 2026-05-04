# Challenge — M4 — *Live API*

Wraps **Phase 3 — Web API**.

M4 is the milestone where the kingdom leaves your machine. The challenge can't visit your live URL — that would need real auth tokens — so it checks the local signals instead: the API project builds, OpenAPI is wired, protected endpoints actually reject unauthenticated calls, and a deploy workflow exists.

## What this verifies

| Check | Looks for |
| --- | --- |
| API project | `Kingdom.Api/` exists and builds |
| OpenAPI | `Program.cs` registers OpenAPI; `/openapi/v1.json` is served |
| Auth required | Auth-required endpoints return `401` when called without a session |
| 404 on unknowns | Unknown paths return `404` |
| Deploy workflow | `.github/workflows/*.yml` exists — proof that CI/CD is set up |
| Wins entry | `journal/wins.md` with an M4 entry mentioning M4 / Phase 3 / Live API |

## What this skips

- Whether the deploy actually worked — impossible to verify locally. The workflow file's presence is the proof of intent; the live URL is the brag-worthy artefact you show a friend.
- The exact set of endpoint paths — your API can have whatever routes make sense.
- That OAuth is wired end-to-end — that would require a real Google flow. The auth check just confirms protected routes return `401` when unauthenticated.

## How to run

From your repo root:

```powershell
dotnet test path\to\challenges\M4\M4.Tests.csproj
```

Green = M4 met. Then the milestone ritual.

## AI Unlock

M4 also fires the **AI Unlock**. After the challenge passes, flip `Current mode: pre-unlock` → `post-unlock` in `CLAUDE.md`. The rules around AI-assisted code change at this point — read `ai-tools.md` to see what's different.
