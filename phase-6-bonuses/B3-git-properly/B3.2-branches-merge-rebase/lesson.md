# Bonus B3.2 — Branches, merge, rebase

There are two ways to combine work from different branches. They produce different histories. Both have good uses. People online argue endlessly about which one to prefer. For now, you just want to know what each one *does* to the DAG, so you can pick the one that fits the situation.

The short version: **`merge` keeps the history as it happened**; **`rebase` rewrites the history.** Neither is wrong. The trade-off is between *showing how the work really happened* and *a clean, straight-line story.*

> **Words to watch**
>
> - **fast-forward** — a merge where the target branch's tip is an ancestor of the source; git just moves the pointer
> - **merge commit** — a new commit with two parents, joining two divergent lines
> - **rebase** — replay your commits onto a new base; rewrites their hashes
> - **force-push** — `git push --force` (or `--force-with-lease`); pushes a rewritten history over what's on the remote
> - **upstream** — the remote branch your local branch tracks

---

## The setup we'll reason about

You start on `main` at commit `C3`. You make a feature branch `feature/farms`, do two commits on it (`C4`, `C5`), and meanwhile someone else adds a commit to `main` (`C6`).

```
                            +-------+
                            |  C5   |   ← feature/farms
                            +-------+
                                |
                                v
                            +-------+
                            |  C4   |
                            +-------+        +-------+
                                |             |  C6   |   ← main
                                v             +-------+
                            +-------+--------+    |
                            |  C3   |             v
                            +-------+--------+ ← (was main)
```

You want to combine your `feature/farms` work with `main`. Two ways.

## Option A — merge

```powershell
git switch main
git merge feature/farms
```

Git creates a new commit, `M`, with *two parents* (`C6` from `main`, `C5` from `feature/farms`). The DAG now has a fork-and-join layout:

```
                            +-------+
                            |   M   |   ← main
                            +-------+
                              /     \
                             v       v
                        +-------+   +-------+
                        |  C5   |   |  C6   |
                        +-------+   +-------+
                            |           |
                            v           v
                        +-------+    +-------+
                        |  C4   |    |  C3   |
                        +-------+    +-------+
                            |           |
                            +-----+-----+
                                  v
                              (older)
```

`M` is a **merge commit**. It has two parents — the latest commit on each branch — and a message git writes for you (or that you write).

What this keeps: *the truth of how the work happened.* You worked on two branches at once, and the history shows it. The commits keep their original SHAs.

What it costs: the graph in `git log --graph` gets busier. If twenty people merge twenty branches a week, it gets hard to read.

### The fast-forward exception

If `main` *hadn't* moved (that is, no `C6`), git would do a **fast-forward** merge — no merge commit, it just slides the `main` pointer up to `C5`. No new commit, no two-parent fork. Straight-line history. Git does this on its own when it can; it is the simpler case.

## Option B — rebase

```powershell
git switch feature/farms
git rebase main
```

Git takes your commits (`C4`, `C5`), lifts them off, *replays* them on top of `main`'s current tip (`C6`), and throws away the originals. The replayed commits get **new SHAs** — they are new commits, even though the message and changes are the same.

```
                            +-------+
                            |  C5'  |   ← feature/farms
                            +-------+
                                |
                                v
                            +-------+
                            |  C4'  |
                            +-------+
                                |
                                v
                            +-------+
                            |  C6   |   ← main
                            +-------+
                                |
                                v
                            +-------+
                            |  C3   |
                            +-------+
```

The history is now a **straight line**. There is no fork-and-join. `git log --graph` reads top to bottom like a story.

What this gives you: a clean, straight-line history. Reviewers love it. Bisecting (finding which commit broke something) is simpler.

What it costs: the originals are gone. The new commits have new SHAs. If you had already pushed `C4` and `C5` to GitHub, and someone else pulled them, your rebase has now split away from theirs — and you can't see it. They will get conflicts the next time they pull. This is the danger.

## In the panel vs the terminal

Both moves exist in VS Code's Source Control panel: `...` menu → *Branch → Merge from* or *Branch → Rebase from*, then pick the source branch. The panel handles the common path. For advanced moves (interactive rebase, squash, fixup), drop to the terminal — that is where the full set of options lives. We'll spend the rest of the lesson at the CLI because it makes *what is actually happening* easier to see.

## The rule of thumb

- **Rebase your *own* branches that you haven't pushed yet** to tidy them up before merging. Safe; it only touches your local copy.
- **Merge** when work is going *into* a shared branch (`main`). The merge commit is honest about the join.
- **Never rebase shared branches.** If you have pushed, and others might have pulled, treat the history as *frozen*.

A common workflow: you work on `feature/farms`, rebase on your own machine to tidy up your commits before opening a PR, then `merge` into `main` when the PR is approved. Mostly straight-line history, and no surprises on shared branches.

## Force-push, the dangerous one

Sometimes you *do* need to overwrite the remote — for example, when you have rebased a branch you had already pushed. The command is `git push --force`.

The cost: any teammate who had pulled the old version of the branch now has commits in their local copy that don't exist on the remote anymore. Their next `git pull` will get strange. They will have to know to run `git reset --hard origin/<branch>`.

The safer version: `git push --force-with-lease`. It does the same thing, but git refuses to push if the remote moved since you last fetched — that is, if someone else pushed in the meantime. This is the version to learn.

> **Rule:** never force-push to `main` (or whatever branch is shared with the team). Force-push to your own feature branches sparingly and announce it (*"heads up, just rebased `feature/farms`"*) if anyone else has been pulling them.

## Hands-on

Make a sandbox repo:

```powershell
mkdir $HOME\git-sandbox
cd $HOME\git-sandbox
git init
"line 1" | Set-Content README.md
git add README.md
git commit -m "C1: initial"
"line 2" | Add-Content README.md
git commit -am "C2: add line 2"
git switch -c feature
"feature line" | Add-Content README.md
git commit -am "F1: feature work"
git switch main
"main line" | Add-Content README.md
git commit -am "C3: parallel main work"
```

Now try both:

**Merge attempt:**

```powershell
git merge feature
# resolve conflict if any (edit README.md, then git add + git commit)
git log --oneline --graph --all
```

You'll see the fork-and-join layout with a merge commit.

**Reset and try rebase:**

```powershell
git reset --hard <C3-hash>          # back to before the merge
git switch feature
git rebase main
git log --oneline --graph --all
```

Now linear. The feature commits have new SHAs.

Compare the two `git log --graph` outputs side by side. The first one keeps the two-branches-at-once layout; the second one makes it look like the work happened one step after another.

## Tinker

Try `git rebase -i HEAD~3`. Interactive rebase opens an editor where you can reorder, squash, or edit your last three commits. Squash is the move you'll use most — combine three small commits into one neat one before merging.

Read your kingdom repo's `git log --graph --all`. How many merge commits are there? How many places where two branches joined back together? That is the picture of how the year actually happened.

Try `git push --force-with-lease` in your sandbox after a rebase. Notice it works when no one else has touched the branch.

## What you just did

You met the two ways to combine git history — `merge` (keeps the two-branches-at-once layout with a merge commit) and `rebase` (replays your commits onto a new base, producing a straight-line history with new SHAs). You learned the rule of thumb: rebase your own branches you haven't pushed yet; merge into shared branches; never rebase shared history. You met `force-push` and the safer `--force-with-lease`. You ran a sandbox where you saw the same two-branch situation produce two different DAG layouts depending on which one you picked.

**Key concepts you can now name:**

- **fast-forward merge** — no merge commit needed; pointer slides forward
- **merge commit** — a commit with two parents that joins two branches
- **rebase** — replay commits onto a new base; new SHAs, linear history
- **force-push** — overwrite the remote; dangerous on shared branches
- **`--force-with-lease`** — safer force-push that refuses if the remote moved

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B3.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B3.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module B3.3 is the safety net: `reflog`, getting back work you thought you'd lost, and the rescue moves you reach for when something feels wrong. The rule is *read the state before you act*; B3.3 turns that into specific commands.
