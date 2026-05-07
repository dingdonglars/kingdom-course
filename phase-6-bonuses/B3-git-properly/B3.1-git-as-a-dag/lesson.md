# Bonus B3.1 — Git as a DAG (the model)

You've been typing `git add`, `git commit`, `git push` for a year. The commands work; you've shipped a year of code with them. What you probably haven't done is look at *what git actually stores* underneath. Today's lesson is the model — the picture that turns commands into something you can reason about instead of memorise.

The reason this matters: most git confusion comes from operating on git without a model. People type commands they've seen others type, get a result they didn't expect, panic, and add `--force` to make the panic go away. With the model in your head, git stops being mystical. Most "I broke git" moments turn into *"oh, I see what happened — and here's the move that fixes it."*

> **Words to watch**
>
> - **commit** — a snapshot of your whole project at one point in time, plus a short message
> - **DAG** — *Directed Acyclic Graph*. A bunch of nodes with one-way arrows; no cycles. Git's history is a DAG of commits.
> - **HEAD** — git's pointer to "where you are right now"
> - **branch** — a named pointer to a specific commit; moves forward as you commit
> - **parent** — the commit a new commit was based on (most commits have one; merges have two)

---

## Step 1 — what a commit really stores

A commit isn't a diff. It's a *snapshot* of your whole project — every file, every folder, exactly as they were when you typed `git commit`. Git stores it efficiently (it doesn't actually duplicate files that didn't change), but conceptually each commit is the *whole world* at that moment.

Each commit also carries:

- A **message** — your short note about what changed
- An **author** + a **timestamp**
- A pointer to its **parent commit** — the commit it was based on
- A unique **SHA hash** — git's name for that specific snapshot

The hash is the part that surprises people. When git says `commit 7a3f5b2…`, that long string isn't a random ID — it's a fingerprint computed from the commit's contents. Change one character of one file and the hash changes too. Git's whole storage model is built on these fingerprints; that's why two repos can compare history without trusting each other.

## Step 2 — branches as pointers

A branch is one short, simple thing: a *pointer* to a commit. That's it. `main` is a name pointing at one specific commit. `feature/farms` is a name pointing at another. The pointer moves forward when you commit on that branch.

This is the single most useful realisation about git. The branch *isn't* a series of commits; it's a label that *currently points at one*. The commits behind it are reachable through the parent chain, but the branch itself is just the label on the tip.

That means creating a branch is free — you're just writing down a new name pointing at the same commit. Deleting a branch is almost free — you erase the label; the commits stay around (for a while; we'll meet `git gc` later).

```
                                    ↑ main
                            +-------+
                            |  C3   |
                            +-------+
                                |
                                v
                            +-------+
                            |  C2   |
                            +-------+
                                |
                                v
                            +-------+
                            |  C1   |
                            +-------+
```

`main` points at `C3`. `C3`'s parent is `C2`. `C2`'s parent is `C1`. The chain *is* the history.

Now make a feature branch:

```
                                    ↑ main
                            +-------+
                            |  C3   |   ← feature/farms
                            +-------+
```

Both `main` and `feature/farms` point at `C3`. No commits got duplicated. You added a label.

Now commit on `feature/farms`:

```
                            +-------+
                            |  C4   |   ← feature/farms
                            +-------+
                                |
                                v
                            +-------+
                            |  C3   |   ← main
                            +-------+
```

`feature/farms` advanced to `C4`. `main` stayed on `C3`. The two branches diverged by one commit.

This is the picture of every branch in every repo, ever. Internalise it and most git commands stop being mystical.

## Step 3 — HEAD

`HEAD` is git's name for *where you are right now*. Usually it points at a branch (which points at a commit). When you `git commit`, what actually happens is:

1. Git makes a new commit with the current HEAD's commit as parent
2. Git moves the branch HEAD points at to the new commit
3. HEAD itself stays pointing at that branch

So you advance the label without lifting a finger.

If HEAD points directly at a commit instead of a branch, you're in *detached HEAD state* — git's polite way of saying *"you're floating, no branch is following you, anything you commit here will be hard to find later."* It's not broken; it's just a state. The fix is `git switch -c <new-branch-name>` to create a branch from where you are.

## Step 4 — looking at your own DAG

Open your kingdom repo. The visual you want is the GitLens **Commit Graph** — `Ctrl + Shift + P` → *"GitLens: Show Commit Graph"*. Every commit you've made appears as a node, parent links drawn between them, branches as coloured lanes. *That picture, on screen, is your real DAG.* If GitLens isn't installed yet, install it (Extensions sidebar → search *GitLens*).

Pick a commit in the graph and click it — the right-side panel shows the message, parent, author, and the diff against the parent. Notice the diff is *not* the whole snapshot — it's the *change*, computed from this commit's snapshot and the parent's. You're looking at the model.

> **Or in the terminal:**
>
> ```powershell
> git log --oneline --graph --decorate --all   # the graph
> git show <hash>                               # one commit's diff
> ```
>
> Same picture, same data; the panel is just a clearer rendering. Many B3 readers prefer the CLI for raw inspection — `git log --graph` is fast, scriptable, and works over SSH. Use both.

## Tinker

Run `git cat-file -p HEAD`. This is the raw stored commit object — author, parent SHA, tree SHA, message. Underneath, git is *just text files*.

Run `git cat-file -p <tree-SHA>` (using the tree SHA from above). Trees list the files and folders in the snapshot. Drill in further with `git cat-file -p <file-blob-SHA>` and you'll see the file content itself. The whole model is just commits → trees → blobs, all keyed by SHA.

## What you just did

You looked at git not as a set of commands but as a *data structure* — a graph of commits, with branches as moveable labels on top. You met the four building blocks (commit, parent, branch, HEAD) and the rule that a commit is a *snapshot, not a diff*. You ran `git log --graph` against your own year of work and saw your real DAG. That's the model the rest of B3 builds on.

**Key concepts you can now name:**

- **DAG** — git's graph: commits with one-way parent links, no cycles
- **commit** — a labelled snapshot of the whole project
- **branch** — a pointer to a commit; the pointer, not the line of history
- **HEAD** — git's *"you are here"* pointer
- **detached HEAD** — HEAD pointing at a commit directly, not at a branch

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module B3.2 takes the model and applies it to the two operations that confuse people most: `merge` and `rebase`. With the DAG in your head, both finally make sense.
