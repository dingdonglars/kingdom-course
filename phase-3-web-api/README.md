# Phase 3 — Web API on the Internet

> **Friends play your kingdom from anywhere — at a real URL you can text them.**

**What you'll have at the end:** A live URL on the open internet. Friends sign in with their Google account and play your kingdom from their browser. Every push to `main` redeploys the site automatically. Same engine as the console version; new outer layer.

**Estimated effort:** 9 weeks at 4–6 hrs/week  ·  **Wraps:** **M4** *Kingdom v3 — Live API*

---

## Why this phase?

Until now your kingdom has lived on your computer. This phase moves it onto the internet. The engine doesn't change at all — it didn't change when we added persistence either. What changes is the *outer layer*: instead of a console reading your keystrokes, it's an HTTP server reading requests from any client in the world. A browser, a `curl` command, a friend's phone — all the same engine, all the same kingdom.

This phase also opens with something a little different. Module 3.0 isn't a coding module; it's a *reading* module. Before you write your first endpoint, you spend half an hour reading two real ones written by the people who built ASP.NET. Reading code is the senior skill that nobody teaches and everyone needs, and we start the habit early.

The phase closes with **M4 — the AI Unlock**. Up to this point, you've been working without an AI assistant — Lars is your friction-helper via Slack `#help`. M4 is the day Claude joins your terminal, and the rules unlock for *implementation* help: the AI can write code with you, under one strict rule — you must be able to explain every line you merge. The work you've put in across Phases 0 through 3 is what makes that change safe.

## What you'll learn (named)

- Reading code as a senior skill — start the habit before the writing
- HTTP basics — request, response, verbs, status codes
- ASP.NET Core minimal APIs
- DTOs at the API boundary — same lesson as JSON persistence, doubled
- OpenAPI / Swagger for documentation
- Structured logging with `ILogger<T>`
- OAuth via Google — real authentication, no DIY passwords
- Multi-user persistence — every kingdom owned by a real signed-in user
- Integration testing with `WebApplicationFactory<T>`
- Deploying to Azure App Service Free tier
- CI/CD with GitHub Actions — auto-deploy on every push to `main`
- Production hygiene — secrets out of repos, parameterised queries, cookie flags

## What you'll build (the climb)

- **Week 23:** Read someone else's API code first. Then HTTP basics and your first endpoint.
- **Weeks 24–26:** API features — DTOs, routing, OpenAPI, logging, OAuth setup and wiring.
- **Weeks 27–29:** Multi-user kingdoms, integration tests.
- **Weeks 30–31:** Deploy to Azure with a security primer, then CI/CD on every push.

## Brag-worthy outcome

A URL you can text to a friend. They click it. They sign in with their Google account. They see their own kingdom. Their score appears on the leaderboard alongside yours. The same engine you wrote in Phase 1 is now serving real strangers over the internet, and a `git push` redeploys the whole thing in three minutes.

---

[**→ Start Module 3.0 — Reading Code Before Writing It**](./3.0-reading-code-before-writing-it/lesson.md)
