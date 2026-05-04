# Bonus B2.5 — Comparing Tools, then the Close

Claude isn't the only AI assistant. Cursor, GitHub Copilot, Claude Code, Continue, Codeium — each one fits a different niche. Today we name the field, talk about when each shines, and then you write the one-page reflection that closes B2 and the curriculum at the same time.

The point of comparing tools isn't to convince you to use them all. The point is to recognise the *shapes* — chat-style, inline completion, agentic — so when a new tool launches next year (and one will), you know what category it falls into and whether to bother trying it.

> **Words to watch**
>
> - **chat-style** — you talk, the AI talks back; web UI or terminal
> - **inline completion** — ghost text appears as you type; Tab to accept (Copilot, Cursor, Codeium)
> - **agentic** — the AI writes, edits, and runs commands across multiple files (Claude Code, Cursor's Agent mode)
> - **diff-based** — the AI proposes a diff; you accept or reject hunks
> - **MCP** *(em-see-pee)* — Model Context Protocol; Anthropic's emerging standard for tools that AIs can talk to

---

## The map

| Tool | Style | Best for |
| --- | --- | --- |
| **Claude (web/desktop)** | Chat | Long-form reasoning, refactor planning, design conversations |
| **Claude Code (CLI)** | Agentic terminal | Multi-file edits, test-loop coding, project-aware work |
| **GitHub Copilot** | Inline plus chat | In-IDE completions; great at *"finish this line"* |
| **Cursor** | Editor (forked VS Code) plus Agent mode | Wholesale rewrites, repo-aware editing, strong agent loop |
| **Continue** | Open-source IDE plugin | Self-hosted models, enterprise control |
| **Codeium / Tabnine** | Inline completion | Free, lightweight, works in many editors |

For a learning project like yours, the recommended stack:

- **Claude Code** for non-trivial tasks (planning, multi-file edits, test loops)
- **GitHub Copilot** for in-IDE small completions
- **Claude desktop** for pure conversation and explanation

You don't need all of them. Pick one chat-style and one inline. Ship.

## When each shines

A few examples of which tool fits which task:

- *"Help me think through how to structure the OAuth flow."* → **Claude desktop / Claude (chat)**
- *"Add `LoadRichest` to the EF store, with a test."* → **Claude Code** (agent)
- *"Finish this method I'm typing."* → **Copilot** (inline)
- *"Refactor this 200-line file into four components."* → **Cursor agent** or **Claude Code**
- *"Translate this Spanish error message."* → **Claude desktop** (or any chat-style tool)

When in doubt: chat-style for thinking, inline for typing, agentic for doing.

## Step — write the reflection

Open `journal/B2-what-i-learned.md`. The template has five prompts. Pick the ones that resonate; you don't have to answer all five.

1. **What did the AI do well across the year?** Pick one task you used it for repeatedly. What made it work?
2. **What did it do badly?** Pick the worst output you accepted. What broke? What did you learn from the breakage?
3. **What workflow change did you make?** What's different about how you start a task today vs. month one?
4. **One concrete piece of advice for someone starting next year?** Three sentences max.
5. **Where will you use AI more next year? Where less?**

Aim for ~500 words total. The exercise isn't literary polish — it's thinking about how you think (which is what the M5.8 reflection touched on too). Naming what you've been doing is what turns a bunch of habits into a transferable skill.

## Step — tag the bonus

Once your reflection is written and committed:

```powershell
git tag b2-context-engineering-complete
git push origin b2-context-engineering-complete
```

## What's next (genuine end of curriculum)

You've finished M6 *and* both bonuses. The course is over.

Some directions if you want them, none required:

- **Start a new project.** Anything. Your toolset travels.
- **Contribute to an open-source project.** Read its README; pick a `good-first-issue`; learn how a real codebase is maintained by people who didn't write it from scratch.
- **Teach someone else.** Pick the lesson that helped you most and write it for a friend who's about to start. The act of explaining is its own next level.
- **Stay sharp.** Read code, write code, read about code. The skill keeps building over time as long as you keep showing up.

## What you just did

You closed B2 and the curriculum at the same time. The closing reflection is what you keep — five hundred words about what AI did well, what it did badly, how your workflow changed, what advice you'd give next year's beginner. Five modules of context engineering — frame, scaffolding, scoping, eval, tools — and you've named what you were already doing. The discipline is yours now; the tools come and go around it.

**Key concepts you can now name:**

- **chat-style** — talk, AI talks back
- **inline completion** — ghost text in the editor; Tab to accept
- **agentic** — multi-step file editing plus tool use
- **MCP** — Model Context Protocol; emerging tool-talking standard
- **your picked stack** — one chat-style tool, one inline tool, optionally one agent

## The through-line — for the last time

> **The discipline is yours. The tools come and go. The thing you take with you is the way you think.**
>
> One year. Five shells. One engine. Two languages. Two bonuses. **You're not someone who took a programming course any more — you're a programmer.**

## Quiz

Open `quiz.md`. The very last one. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
