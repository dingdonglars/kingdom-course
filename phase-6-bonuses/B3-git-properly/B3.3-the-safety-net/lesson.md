# Bonus B3.3 — The safety net (reflog and recovery)

The thing nobody tells you about git: it almost never actually loses your work. Even when you've run `git reset --hard` and watched commits vanish from `git log`, they're still there — git just stopped pointing at them. The reason most "I broke git" panic ends without lost work is a single command: `git reflog`.

Today's lesson is the safety net. Specifically: how to find work that looks gone, how to recover from `reset --hard` mistakes, and how to develop the discipline that prevents most of these moments in the first place. The discipline is one sentence — *read state before acting*. The commands below are how you read state.

> **Words to watch**
>
> - **reflog** — git's local log of where every branch and HEAD has been
> - **dangling commit** — a commit no branch or tag points at, but still in git's storage
> - **garbage collection (`gc`)** — git eventually deletes truly unreachable objects (default ~30 days)
> - **`reset --hard`** — moves a branch to a different commit and wipes the working tree to match
> - **rescue** — recovering work that looks lost

---

## The model: nothing is gone (yet)

When you `git commit`, git stores the commit in its object database, keyed by SHA. *Nothing ever removes that object directly.* Branches, tags, HEAD — these are all just pointers. When you "delete" a commit by moving a branch off it, the commit still exists in git's storage. It's just not reachable through any pointer you normally see.

`git reflog` is the secret extra log of *where pointers used to be*. Every time HEAD moves — checkout, commit, reset, rebase, merge — git logs the move. Each entry has a SHA and a description like *"HEAD@{3}: rebase: feature/farms"*.

This is the safety net. As long as the reflog still has the SHA, the commit is recoverable.

## Step 1 — using reflog

This whole module is **CLI-only**. The Source Control panel doesn't have a button for `reflog`, `fsck`, or the rescue moves built on them — these are exactly the cases where the terminal is the right tool. (You've been using the panel all year; this is the lesson where you'll appreciate that the CLI is *also* still under your fingers.)

In any repo, run:

```powershell
git reflog
```

You'll see something like:

```
3a4f5b2 HEAD@{0}: commit: M2 close ritual
8c9d1e0 HEAD@{1}: commit: add Mine subclass
b2a3c4d HEAD@{2}: checkout: moving from main to feature/farms
...
```

This is your last hundred or so HEAD positions, in reverse order. The numbers in `HEAD@{N}` are how-many-moves-ago, not commit ages.

Any of those SHAs can be used like a commit reference:

```powershell
git show 8c9d1e0
git checkout 8c9d1e0
git branch rescue 8c9d1e0    # creates a branch pointing at that commit
```

## Step 2 — the canonical "I lost work" rescue

You ran `git reset --hard HEAD~3`. Three commits worth of work just vanished from `git log`. Panic.

The rescue:

```powershell
git reflog
# look for the entry just before the reset, e.g. HEAD@{1}
git reset --hard HEAD@{1}
```

That's it. You moved HEAD back to where it was before the reset. The "lost" commits are reachable again.

Equivalent forms:

```powershell
git reset --hard <SHA-from-reflog>
git switch -c rescue <SHA-from-reflog>     # if you'd rather create a branch
git cherry-pick <SHA-from-reflog>          # if you only want one of the commits back
```

The rescue works because the commits were never actually deleted — only *orphaned* (no branch or tag pointing at them anymore). `reflog` showed you the SHA; everything else is moving a pointer.

## Step 3 — when the safety net runs out

Git's garbage collection (`git gc`) eventually deletes truly unreachable objects. The default is 30-90 days. Until then, even commits you "lost" weeks ago are still in storage.

After `gc` runs, an unreachable commit is genuinely gone. If you suspect you've lost something old, try:

```powershell
git fsck --lost-found
```

This finds dangling commits and writes them to `.git/lost-found/`. It's a last-resort rescue; the contents are unlabelled, so you'll have to inspect each one.

## Step 4 — the discipline (the actual lesson)

Most rescues are unnecessary. They're caused by typing a command without reading what state you're in first.

Three states matter at any moment:

1. **Where is HEAD?** (`git status` answers this — the first line)
2. **What's modified, what's staged?** (`git status` again)
3. **Where do I want to end up?** (you, the human, decide)

If you can't name those three things, *don't run the next command yet*. Read first. Most "git ate my work" stories are *"I didn't actually know what state I was in, ran something, panicked, ran something else."*

The rescue rule: **read state before acting.** Specifically:

- Before `reset` of any kind: `git status` and `git log --oneline -10`. Make sure you understand which commits you're about to move past.
- Before `force-push`: `git log @{upstream}..HEAD` (commits you have that the remote doesn't) and `git log HEAD..@{upstream}` (commits the remote has that you'd overwrite).
- Before `rebase` of a pushed branch: think *"is anyone else pulling this?"* If yes, don't rebase.
- Before `clean -f` or `git restore .`: realise you're throwing away uncommitted changes. There is no reflog for those.

The two real ways to lose work are: uncommitted changes (no reflog covers them) and force-pushing over your own work (rare; you have to actively try). Everything else is recoverable.

## Hands-on

In your sandbox repo (or any repo where you don't mind experimenting):

1. Make three commits.
2. `git reset --hard HEAD~3`. Confirm with `git log --oneline` that you've gone back three commits.
3. `git reflog`. Confirm the three commits are still listed.
4. `git reset --hard HEAD@{1}`. Confirm with `git log --oneline` that the three commits are back.

Now try the cherry-pick variant:

5. `git reset --hard HEAD~3` again.
6. From `git reflog`, pick the SHA of one of the three commits (not the most recent).
7. `git cherry-pick <that-SHA>`. That single commit's changes are reapplied on top of HEAD.

The hands-on reps build the muscle of *"oh, that was just a pointer move; I can put it back."*

## Tinker

Run `git fsck --lost-found` in your kingdom repo. Probably nothing — but if there's anything in `.git/lost-found/`, that's an old dangling commit, recoverable until the next `gc`.

Run `git gc --prune=now --aggressive` in a *test* repo (not the kingdom). Notice that after `gc`, dangling commits are gone for real. This is why the safety net has a clock on it.

Set up a useful alias:

```powershell
git config --global alias.last "log -1 HEAD --stat"
git config --global alias.tree "log --oneline --graph --decorate --all"
```

Now `git last` and `git tree` are short forms for the long commands. Aliases save typing time across the year.

## What you just did

You met `git reflog` — the secret extra log of HEAD's history that's also git's safety net. You learned that "lost" commits aren't actually deleted until `gc` runs days or weeks later, so most rescues are pointer moves not data recovery. You worked through the canonical rescue: undo a `reset --hard` by reading the reflog and resetting again. You named the discipline that prevents most rescues: read state before acting — `git status` and `git log` before any move that changes history.

**Key concepts you can now name:**

- **reflog** — git's log of where HEAD has been; the safety net
- **dangling commit** — a commit no pointer reaches but git still stores
- **`gc`** — eventual deletion of truly unreachable objects (~30 days default)
- **the rescue rule** — read state (`git status`, `git log`) before acting
- **the two real ways to lose work** — uncommitted changes; force-pushing over your own work

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B3.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B3.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module B3.4 closes B3 with the *tools* around git — the VS Code Source Control panel (your main day-to-day GUI), plus a brief tour of the dedicated GUI clients (Fork, GitKraken). The model is in your head; the tools just show it to you visually.
