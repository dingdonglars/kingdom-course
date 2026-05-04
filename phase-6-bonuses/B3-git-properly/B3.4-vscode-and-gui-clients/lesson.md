# Bonus B3.4 — Tools — git inside VS Code (and other clients)

You've spent a year using VS Code's Source Control panel as your daily git surface. By now you've also met enough CLI to know when each is the right tool. Today's lesson goes deeper on the panel + GitLens (the extension you installed back at M1.6), and tours the dedicated GUI clients people reach for when they want even more.

The headline: keep what you've been doing. The Source Control panel for daily work, the CLI for the moves the panel doesn't expose (`reflog`, `cherry-pick`, `rebase -i`, scripts, CI, server work). Reach for a dedicated GUI (Fork, GitKraken, etc.) only if you find yourself wanting one; many seasoned developers never do.

> **Words to watch**
>
> - **Source Control panel** — VS Code's built-in git GUI; sidebar icon that looks like a branch
> - **stage** — mark a change as part of the next commit (the "staging area" or "index")
> - **hunk** — a contiguous block of changed lines in a diff
> - **GitLens** — a popular VS Code extension that adds inline blame and rich history views
> - **dedicated GUI client** — a standalone app whose only job is git (Fork, GitKraken, SourceTree, etc.)

---

## Step 1 — install (mostly nothing)

Git itself is already installed (you set it up on day 1, M0.0). VS Code already has a Source Control panel — there's nothing new to install for the basics.

The one extension that genuinely earns its place: **GitLens**. Install it now if you haven't.

1. In VS Code, open Extensions (`Ctrl+Shift+X`).
2. Search for *"GitLens — Git supercharged"* by GitKraken.
3. Click **Install**.

GitLens adds inline blame on every line ("last edited by you, two months ago"), rich commit hover-cards, and a graph view. The free tier is plenty for everything you'll do this year.

## Step 2 — opening the Source Control panel

The Source Control panel is the second-to-top icon in VS Code's left sidebar — looks like a branch with a fork. Click it. (Or press `Ctrl+Shift+G` then `G`.)

You'll see three sections:

- **Changes** — files modified since the last commit
- **Staged Changes** — files queued for the next commit (after `git add`)
- **Commit message box** — where you type the message and click the checkmark to commit

The mental model is exactly what you've been typing in the terminal:

| In the terminal | In the panel |
|---|---|
| `git status` | what the panel shows you, by default |
| `git add file.cs` | click the `+` next to a file in *Changes* |
| `git add -p` (interactive) | click a file → diff opens → click the `+` next to specific lines |
| `git commit -m "..."` | type message in the box, click the checkmark |
| `git push` | `...` menu → Push (or click the sync icon at the bottom) |

The killer feature is the third row: **stage individual lines**. In the diff view, every changed hunk has a `+` icon in the gutter; clicking it stages just that hunk. This is much faster than `git add -p` for picking apart a sloppy commit you accidentally made too big.

## Step 3 — reading diffs in the panel

Click any modified file in *Changes*. VS Code opens a side-by-side diff: old version on the left, new on the right, changed lines highlighted. This is far easier to read than `git diff` for anything more than a few lines.

The same view works on commits in history: with GitLens installed, opening the *Source Control* panel's history view and clicking a commit shows the diff for that commit, file by file. Useful for *"what did I actually change in last Tuesday's commit?"*

For a graphical view of the whole DAG, GitLens adds a **Commit Graph** view (Ctrl+Shift+P → *"GitLens: Show Commit Graph"*). It's the closest thing to the picture you drew in B3.1 — branches, commits, parent links, all visual. When you find yourself running `git log --graph --all` more than twice in a session, switch to the graph view.

## Step 4 — when to fall back to the CLI

The panel handles ~90% of daily moves. The CLI is faster for:

- **Anything scripted** — git commands in PowerShell scripts, CI pipelines, etc.
- **Operations the panel hides** — `git rebase -i`, `git cherry-pick`, `git reflog`, `git fsck`. The panel doesn't surface every command; the CLI exposes everything.
- **Working over SSH or in WSL** — you may not have VS Code there.
- **When something is confusing** — sometimes the panel's abstraction hides what's actually happening; dropping to the CLI and reading `git status` is faster than guessing what the GUI is doing.

The discipline is: *use both, lean toward whichever is faster for the move at hand*. You don't have to pick.

## Step 5 — dedicated GUI clients (the brief tour)

Some developers prefer a single dedicated app for git. The two most popular:

### Fork

[**Fork**](https://git-fork.com/) — fast, native, free for personal use ($50 one-time for commercial use). Clean visual graph, easy interactive rebase, side-by-side diff, integrated terminal. Loved by people who want a focused tool that "just does git." Recommended if you find yourself wanting more than the VS Code panel offers.

### GitKraken

[**GitKraken**](https://www.gitkraken.com/) — graphical-first; the marquee feature is its commit graph. Free for non-commercial use; subscription for full features. Polished, opinionated. Some find it too heavy; others love it.

Other names you'll see: **SourceTree** (free, Atlassian, ageing), **GitHub Desktop** (free, GitHub-specific, deliberately limited), **Sublime Merge** (paid, fast, by the Sublime Text people). Each has its loyalists.

**The honest take:** *most working developers use the CLI plus their editor's git panel and never touch a dedicated client.* They're nice to know exist; you don't need one. Try one if you're curious; switch back to VS Code if it doesn't add value. The course doesn't ship with any of them; if you want to try Fork, install it and see how it feels for a week.

## Tinker

Open the GitLens Commit Graph view in your kingdom repo. Click random commits; read what changed. This is the "read code more than you write" rule applied to your own history.

In the Source Control panel, deliberately make a sloppy commit: edit two unrelated things in one file, then stage them as two *separate* commits using the line-by-line `+` buttons. This is the move that makes commits readable when you've been making sloppy ones.

Install Fork (free for personal use). Open your kingdom repo in it. Try one operation in Fork that you'd normally do in the CLI — say, an interactive rebase. See whether it feels faster or slower for you. Uninstall if you don't end up loving it.

## What you just did

You closed B3 by mapping the model in your head onto the visual tools that show it. The VS Code Source Control panel handles ~90% of daily git moves and is already installed; GitLens adds inline blame and a graph view that mirrors the DAG you drew in B3.1. You learned when to drop back to the CLI (scripts, hidden commands, debugging the GUI itself) and met the dedicated clients (Fork, GitKraken, others) as *optional* tools — most working developers never reach for them. You also met the bonus's closing rule: lean toward whichever surface is faster for the move at hand. CLI and GUI aren't a choice; they're a pair.

**Key concepts you can now name:**

- **VS Code Source Control panel** — your main daily git GUI, no install needed
- **GitLens** — the one VS Code extension that earns its place for git
- **stage individual lines** — the panel's killer feature for unsloppy commits
- **the CLI fallback** — scripts, hidden commands, when the GUI confuses
- **dedicated GUI clients** — Fork, GitKraken, etc.; optional, often skippable

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

---

## Bonus B3 — wrap-up

B3 is done. You went from typing git commands you'd half-memorised to reasoning about git as a *DAG of commits with movable pointers*. You named the trade-off between merge and rebase. You met the safety net — `reflog` and the commits that were never actually lost. You hooked the model into the visual tools that show it. The commands you've been typing all year haven't changed; what's changed is the picture they paint in your head when you type them.

Tag the bonus complete:

```powershell
git tag b3-git-properly-complete
git push origin b3-git-properly-complete
```

There's no PR, no review — B3 is a self-contained bonus. The reflection lives in your habits from now on: *read state before acting; the DAG is just commits and pointers; reflog is the safety net; the GUI is a view of the model, not the model itself.*
