# Bonus B3.1 — Git as a DAG (the model)

You have been typing `git add`, `git commit`, `git push` for a year. The commands work; you have written a year of code with them. What you probably haven't done is look at *what git actually stores* underneath. Today's lesson is the model — the picture that turns the commands into something you can reason about instead of memorise.

Why this matters: most git confusion comes from using git without a model. People type commands they have seen others type, get a result they didn't expect, panic, and add `--force` to make the panic go away. With the model in your head, git stops being a mystery. Most "I broke git" moments turn into *"oh, I see what happened — and here is the move that fixes it."*

> **Words to watch**
>
> - **commit** — a snapshot of your whole project at one point in time, plus a short message
> - **DAG** — *Directed Acyclic Graph*. A bunch of nodes with one-way arrows; no cycles. Git's history is a DAG of commits.
> - **HEAD** — git's pointer to "where you are right now"
> - **branch** — a named pointer to a specific commit; moves forward as you commit
> - **parent** — the commit a new commit was based on (most commits have one; merges have two)

---

## Step 1 — what a commit really stores

A commit isn't a diff. It is a *snapshot* of your whole project — every file, every folder, exactly as they were when you typed `git commit`. Git stores it in a smart way (it doesn't actually copy files that didn't change), but you can think of each commit as the *whole project* at that moment.

Each commit also carries:

- A **message** — your short note about what changed
- An **author** + a **timestamp**
- A pointer to its **parent commit** — the commit it was based on
- A unique **SHA hash** — git's name for that specific snapshot

The hash is the part that surprises people. When git says `commit 7a3f5b2…`, that long string isn't a random ID — it is a fingerprint worked out from the commit's contents. Change one character in one file and the hash changes too. Git's whole storage model is built on these fingerprints. That is why two repos can compare history without having to trust each other.

## Step 2 — branches as pointers

A branch is one short, simple thing: a *pointer* to a commit. That's all. `main` is a name pointing at one specific commit. `feature/farms` is a name pointing at another. The pointer moves forward when you commit on that branch.

This is the single most useful thing to understand about git. The branch *isn't* a series of commits; it is a label that *currently points at one*. The commits behind it are reachable through the parent chain, but the branch itself is just the label on the tip.

That means creating a branch is free — you are just writing down a new name pointing at the same commit. Deleting a branch is almost free — you erase the label, and the commits stay around (for a while; we'll meet `git gc` later).

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

The arrows point from each commit back to its parent. That direction is the whole reason git is a "directed" graph — every commit knows the one it came from.

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

`feature/farms` moved forward to `C4`. `main` stayed on `C3`. The two branches now differ by one commit.

This is the picture of every branch in every repo, ever. Once it is clear in your head, most git commands stop being a mystery.

## Step 3 — HEAD

`HEAD` is git's name for *where you are right now*. Usually it points at a branch (which points at a commit). When you `git commit`, here is what actually happens:

1. Git makes a new commit, using the current HEAD's commit as the parent
2. Git moves the branch that HEAD points at forward to the new commit
3. HEAD itself stays pointing at that branch

So the label moves forward for you, on its own.

If HEAD points straight at a commit instead of a branch, you are in *detached HEAD state* — git's polite way of saying *"you are floating; no branch is following you, so anything you commit here will be hard to find later."* It is not broken; it is just a state. The fix is `git switch -c <new-branch-name>` to make a branch from where you are.

## Step 4 — looking at your own DAG

Open your kingdom repo. The view you want is the GitLens **Commit Graph** — `Ctrl + Shift + P` → *"GitLens: Show Commit Graph"*. Every commit you have made appears as a node, with parent links drawn between them and branches shown as coloured lanes. That picture, on screen, is your real DAG. If GitLens isn't installed yet, install it (Extensions sidebar → search *GitLens*).

Pick a commit in the graph and click it — the right-side panel shows the message, the parent, the author, and the diff against the parent. Notice the diff is *not* the whole snapshot — it is the *change*, worked out from this commit's snapshot and the parent's. You are looking at the model.

> **Or in the terminal:**
>
> ```powershell
> git log --oneline --graph --decorate --all   # the graph
> git show <hash>                               # one commit's diff
> ```
>
> Same picture, same data; the panel just draws it more clearly. Many B3 readers prefer the CLI for a close look — `git log --graph` is fast, works in scripts, and works over SSH. Use both.

## Tinker

Run `git cat-file -p HEAD`. This is the raw stored commit object — author, parent SHA, tree SHA, and message. Underneath, git is *just text files*.

Run `git cat-file -p <tree-SHA>` (using the tree SHA from above). Trees list the files and folders in the snapshot. Go one step further with `git cat-file -p <file-blob-SHA>` and you'll see the file content itself. The whole model is just commits → trees → blobs, each one named by its SHA.

## What you just did

You looked at git not as a set of commands but as a *data structure* — a graph of commits, with branches as labels you can move on top. You met the four building blocks (commit, parent, branch, HEAD) and the rule that a commit is a *snapshot, not a diff*. You ran `git log --graph` against your own year of work and saw your real DAG. That is the model the rest of B3 builds on.

**Key concepts you can now name:**

- **DAG** — git's graph: commits with one-way parent links, no cycles
- **commit** — a labelled snapshot of the whole project
- **branch** — a pointer to a commit; the pointer, not the line of history
- **HEAD** — git's *"you are here"* pointer
- **detached HEAD** — HEAD pointing at a commit directly, not at a branch

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this one — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without scrolling back up, follow this story in your head and answer at the end. You start with `main` pointing at commit `C3`. You make a branch `feature/farms` (it points at `C3` too). You make one commit on `feature/farms`. Now: where does `main` point? Where does `feature/farms` point? How many commits got copied when you made the branch?

<details><summary>Stuck? Open this to check yourself.</summary>

- `main` still points at **`C3`** — making a branch doesn't move `main`, and committing on a *different* branch doesn't move it either.
- `feature/farms` points at the **new commit** (call it `C4`), whose parent is `C3`. Committing moved that branch's pointer forward.
- **Zero** commits got copied. A branch is just a new name (a pointer) — making one is free.

If you said `main` moved, remember: a commit only moves the branch that HEAD is currently on.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B3.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B3.1 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module B3.2 takes the model and applies it to the two operations that confuse people most: `merge` and `rebase`. With the DAG in your head, both of them finally make sense.
