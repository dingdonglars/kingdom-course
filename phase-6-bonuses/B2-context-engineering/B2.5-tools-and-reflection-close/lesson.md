# Bonus B2.5 — Comparing Tools + Reflection Close

> **Hook:** Claude isn't the only AI assistant. **Cursor, GitHub Copilot, Claude Code, Continue, Codeium** — each fits a different niche. Today we name them, when each shines, and you write your own one-page reflection on the year of AI use.

> **Words to watch**
> - **chat-style** — you talk, the AI talks back; web UI or terminal
> - **inline-completion** — ghost text appears as you type; Tab to accept (Copilot, Cursor, Codeium)
> - **agentic** — the AI writes + edits + runs commands across multiple files (Claude Code, Cursor's Agent mode)
> - **diff-based** — the AI proposes a diff; you accept or reject hunks
> - **MCP** — Model Context Protocol; standard for tools-talking-to-AIs

---

## The map

| Tool | Style | Best for |
|---|---|---|
| **Claude (web/desktop)** | Chat | Long-form reasoning, refactor planning, design conversations |
| **Claude Code (CLI)** | Agentic terminal | Multi-file edits, testing-loop coding, project-aware |
| **GitHub Copilot** | Inline + chat | In-IDE; great at "finish this line" and small completions |
| **Cursor** | Editor (forked VS Code) + Agent mode | Wholesale rewrites, repo-aware editing, strong agent loop |
| **Continue** | Open-source IDE plugin | Self-hosted models, enterprise control |
| **Codeium / Tabnine** | Inline completion | Free + lightweight + works in many editors |

For a *learning* project (yours), the recommended stack:

- **Claude Code** for non-trivial tasks (planning, multi-file edits, test loops)
- **Copilot** for in-IDE small completions
- **Claude desktop** for pure conversation / explanation

You don't need all of them. **Pick one chat-style + one inline; ship.**

## When each shines

- "Help me think through how to structure the OAuth flow." → **Claude desktop / Claude (chat)**.
- "Add `LoadRichest` to the EF store, with a test." → **Claude Code** (agent).
- "Finish this method I'm typing." → **Copilot** (inline).
- "Refactor this 200-line file into 4 components." → **Cursor agent** or **Claude Code**.
- "Translate this Spanish error message." → **Claude desktop** (or any).

When in doubt: chat-style for thinking, inline for typing, agentic for doing.

## The reflection

Open `journal/B2-what-i-learned.md`. Three to five paragraphs.

Prompts (pick the ones that resonate):

1. **What did the AI do well across the year?** Pick one task you used it for repeatedly. What made it work?
2. **What did it do badly?** Pick the worst output you accepted. What broke? What did you learn from the breakage?
3. **What workflow change did you make?** What's different about how you start a task today vs. month 1?
4. **One concrete piece of advice for someone starting next year?** Three sentences max.
5. **Where will you use AI more next year? Where less?**

Aim for ~500 words total. **The exercise is meta-cognition** — naming what you've been doing — not literary polish.

## Tag the bonus

```powershell
git tag b2-context-engineering-complete
git push origin b2-context-engineering-complete
```

## What's next (genuine end of curriculum)

You've finished Block 7 (M6 capstone) AND both bonuses. The course is over.

Some directions if you want them:

- **A new project.** Anything. Your toolset travels.
- **Contribute to an open source project.** Read its README; do `good-first-issue` work; learn how a real codebase is maintained.
- **Teach someone else.** Pick the lesson that helped you most; write it for a friend who's about to start.
- **Stay sharp** — read code, write code, read about code. The compounding never stops.

## Name it

- **Chat-style** — you talk; AI talks back.
- **Inline-completion** — ghost text in the editor; Tab to accept.
- **Agentic** — multi-step file editing + tool use.
- **MCP** — Model Context Protocol; emerging standard.
- **Picked stack** — your chosen one chat + one inline + (optionally) one agent.

## The rule of the through-line — for the last time in this curriculum

> **The discipline is yours. The tools come and go. The thing you take with you is the way you think.**
>
> One year. Five shells. One engine. Two languages. Two bonuses. **You're not someone who took a programming course any more — you're a programmer.**

## Quiz / challenge

Open `quiz.md`. The very last one.