# Block 5 — Web API on the Internet

> **Friends play your game from anywhere.**

**What you'll have at the end:** Your Kingdom lives on a real public URL on the internet. Friends sign in with their Google accounts. There's a leaderboard. The deploy happens automatically every time you push to `main`.

**Estimated effort:** 9 weeks at 4–6 hrs/week  ·  **Wraps:** M4 *Kingdom v3 — Live API*

---

## Why this block?

This is the block that converts your Kingdom from *a thing on your computer* to *a thing on the internet*. Same engine, new shell — now the shell is HTTP. Other people can hit your URL.

This block also opens with **Module 3.0 — Reading Code Before Writing It** (a small read-someone-else's-API-code lesson before you write your own), and closes with the **AI Unlock** at the milestone PR — after M4, the rules around AI-assisted implementation expand.

## What you'll learn (named)

- Reading code as a senior skill — start the habit before writing
- HTTP basics — request, response, status codes
- ASP.NET Core minimal APIs
- DTOs vs domain — *yet another shell-vs-engine lesson*
- OpenAPI / Swagger
- Logging
- OAuth via Google — real auth, no DIY passwords
- Multi-user persistence
- Integration testing with `WebApplicationFactory<T>`
- Deploy to Azure App Service Free F1
- CI/CD lite — GitHub Actions on every push
- **Production hygiene** — secrets out of repos, parameterised queries, cookie flags

## What you'll build (the climb)

- **Week 23:** Read someone else's API code first. Then HTTP basics + first endpoint.
- **Weeks 24–26:** API features — DTOs, routing, OpenAPI, logging, OAuth setup + wiring.
- **Weeks 27–29:** multi-user kingdoms, integration tests.
- **Weeks 30–31:** deploy to Azure (with security primer), then CI/CD.

## Brag-worthy outcome

A URL you can text to a friend. They click it. They sign in with their Google account. They see your leaderboard. Their score appears on it.

---

[**→ Start Module 3.0 — Reading Code Before Writing It**](./3.0-reading-code-before-writing-it/lesson.md)
