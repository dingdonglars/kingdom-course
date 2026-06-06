# Bonus B3.4 — Tools — git inside VS Code (and other clients)

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

You have spent a year using VS Code's Source Control panel as your daily git tool. By now you have also used enough CLI to know when each one is the right choice. Today's lesson goes deeper on the panel and GitLens (the extension you installed back at Module 1.6), and tours the dedicated GUI clients people reach for when they want even more.

The main point: keep what you have been doing. The Source Control panel for daily work, the CLI for the moves the panel doesn't have a button for (`reflog`, `cherry-pick`, `rebase -i`, scripts, CI, server work). Reach for a dedicated GUI (Fork, GitKraken, and so on) only if you find yourself wanting one; many experienced developers never do.

> **Words to watch**
>
> - **Source Control panel** — VS Code's built-in git GUI; sidebar icon that looks like a branch
> - **stage** — mark a change as part of the next commit (the "staging area" or "index")
> - **hunk** — a contiguous block of changed lines in a diff
> - **GitLens** — a popular VS Code extension that adds inline blame and rich history views
> - **dedicated GUI client** — a standalone app whose only job is git (Fork, GitKraken, SourceTree, etc.)

---

## Step 1 — install (mostly nothing)

Git itself is already installed (you set it up on day 1, Module 0.0). VS Code already has a Source Control panel — there is nothing new to install for the basics.

The one extension that is really worth having: **GitLens**. Install it now if you haven't.

1. In VS Code, open Extensions (`Ctrl + Shift + X`).
2. Search for *"GitLens — Git supercharged"* by GitKraken.
3. Click **Install**.

GitLens adds inline blame on every line ("last edited by you, two months ago"), detailed commit hover-cards, and a graph view. The free tier is plenty for everything you'll do this year.

## Step 2 — opening the Source Control panel

The Source Control panel is the second-to-top icon in VS Code's left sidebar — looks like a branch with a fork. Click it. (Or press `Ctrl + Shift + G G`.)

You'll see three sections:

- **Changes** — files modified since the last commit
- **Staged Changes** — files queued for the next commit (after `git add`)
- **Commit message box** — where you type the message and click the checkmark to commit

The idea is exactly what you have been typing in the terminal:

| In the terminal | In the panel |
|---|---|
| `git status` | what the panel shows you, by default |
| `git add file.cs` | click the `+` next to a file in *Changes* |
| `git add -p` (interactive) | click a file → diff opens → click the `+` next to specific lines |
| `git commit -m "..."` | type message in the box, click the checkmark |
| `git push` | `...` menu → Push (or click the sync icon at the bottom) |

The best feature is the third row: **stage individual lines**. In the diff view, every changed hunk has a `+` icon in the gutter; clicking it stages just that hunk. This is much faster than `git add -p` for splitting up a messy commit you accidentally made too big.

## Step 3 — reading diffs in the panel

Click any modified file in *Changes*. VS Code opens a side-by-side diff: old version on the left, new on the right, changed lines highlighted. This is far easier to read than `git diff` for anything more than a few lines.

The same view works on commits in history: with GitLens installed, open the *Source Control* panel's history view and click a commit. It shows the diff for that commit, file by file. Useful for *"what did I actually change in last Tuesday's commit?"*

For a picture of the whole DAG, GitLens adds a **Commit Graph** view (`Ctrl + Shift + P` → *"GitLens: Show Commit Graph"*). It is the closest thing to the picture you drew in B3.1 — branches, commits, and parent links, all on screen. When you find yourself running `git log --graph --all` more than twice in one session, switch to the graph view.

## Step 4 — when to fall back to the CLI

The panel handles about 90% of daily moves. The CLI is faster for:

- **Anything in a script** — git commands in PowerShell scripts, CI pipelines, and so on.
- **Commands the panel doesn't show** — `git rebase -i`, `git cherry-pick`, `git reflog`, `git fsck`. The panel doesn't have a button for every command; the CLI has them all.
- **Working over SSH or in WSL** — you may not have VS Code there.
- **When something is confusing** — sometimes the panel hides what is actually happening; dropping to the CLI and reading `git status` is faster than guessing what the GUI is doing.

The habit is: *use both, and lean toward whichever is faster for the move you are doing*. You don't have to pick one.

## Step 5 — dedicated GUI clients (the brief tour)

Some developers prefer a single dedicated app for git. The two most popular:

### Fork

[**Fork**](https://git-fork.com/) — fast, native, free for personal use ($50 one-time for commercial use). Clean visual graph, easy interactive rebase, side-by-side diff, built-in terminal. Loved by people who want a focused tool that "just does git." Worth a look if you find yourself wanting more than the VS Code panel offers.

### GitKraken

[**GitKraken**](https://www.gitkraken.com/) — graphics-first; its main feature is the commit graph. Free for non-commercial use; subscription for full features. Polished, with strong opinions about how you should work. Some find it too heavy; others love it.

Other names you'll see: **SourceTree** (free, Atlassian, getting old), **GitHub Desktop** (free, GitHub-specific, kept simple on purpose), **Sublime Merge** (paid, fast, by the Sublime Text people). Each has its fans.

**The honest take:** *most working developers use the CLI plus their editor's git panel and never touch a dedicated client.* They are nice to know about; you don't need one. Try one if you are curious; switch back to VS Code if it doesn't help. The course doesn't come with any of them; if you want to try Fork, install it and see how it feels for a week.

## Tinker

Open the GitLens Commit Graph view in your kingdom repo. Click random commits and read what changed. This is the "read code more than you write" rule applied to your own history.

In the Source Control panel, make a messy commit on purpose: edit two unrelated things in one file, then stage them as two *separate* commits using the line-by-line `+` buttons. This is the move that keeps commits easy to read when you have been making messy ones.

Install Fork (free for personal use). Open your kingdom repo in it. Try one thing in Fork that you would normally do in the CLI — say, an interactive rebase. See whether it feels faster or slower for you. Uninstall it if you don't end up liking it.

## What you just did

You closed B3 by connecting the model in your head to the on-screen tools that show it. The VS Code Source Control panel handles about 90% of daily git moves and is already installed; GitLens adds inline blame and a graph view that matches the DAG you drew in B3.1. You learned when to drop back to the CLI (scripts, hidden commands, debugging the GUI itself) and met the dedicated clients (Fork, GitKraken, and others) as *optional* tools — most working developers never reach for them. You also met the bonus's closing rule: lean toward whichever tool is faster for the move you are doing. CLI and GUI aren't a choice between two things; they are a pair you use together.

**Key concepts you can now name:**

- **VS Code Source Control panel** — your main daily git GUI, no install needed
- **GitLens** — the one VS Code extension that earns its place for git
- **stage individual lines** — the panel's best feature for tidy commits
- **the CLI fallback** — scripts, hidden commands, when the GUI confuses
- **dedicated GUI clients** — Fork, GitKraken, etc.; optional, often skippable

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this one — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without scrolling back up, from memory:

1. For each of these terminal commands, say what you click in the VS Code Source Control panel to do the same thing: `git status`, `git add file.cs`, `git commit -m "..."`, `git push`.
2. Then name the panel's best feature — the one that beats the terminal for tidying up a messy commit.

<details><summary>Stuck? Open this to check yourself.</summary>

| In the terminal | In the panel |
|---|---|
| `git status` | what the panel shows by default (the *Changes* list) |
| `git add file.cs` | click the `+` next to the file in *Changes* |
| `git commit -m "..."` | type the message in the box, click the checkmark |
| `git push` | `...` menu → Push (or the sync icon at the bottom) |

The best feature: **stage individual lines** — in the diff view, click the `+` next to a single changed block (hunk) to stage just that part. It splits a too-big commit faster than `git add -p`.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B3.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B3.4 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

---

## Bonus B3 — wrap-up

B3 is done. You went from typing git commands you had half-learned to thinking about git as a *DAG of commits with movable pointers*. You named the trade-off between merge and rebase. You met the safety net — `reflog` and the commits that were never actually lost. You connected the model to the on-screen tools that show it. The commands you have been typing all year haven't changed; what has changed is the picture they paint in your head when you type them.

Tag the bonus complete. **This one's CLI-only — the panel doesn't have a button for tags:**

```powershell
git tag b3-git-properly-complete
git push origin b3-git-properly-complete
```

There's no PR, no review — B3 is a bonus that stands on its own. The lesson lives in your habits from now on: *read the state before you act; the DAG is just commits and pointers; reflog is the safety net; the GUI is a view of the model, not the model itself.*
