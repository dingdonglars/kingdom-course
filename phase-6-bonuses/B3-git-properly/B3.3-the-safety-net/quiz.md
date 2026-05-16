# Quiz — Bonus B3.3

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. What does `git reflog` show?

- **a.** Every commit ever made to the repo, oldest first
- **b.** The remote-tracking branches and their tips
- **c.** Where HEAD has been recently — every checkout, commit, reset, rebase
- **d.** A list of files modified in the last commit

## 2. After `git reset --hard HEAD~3`, are the three commits gone?

- **a.** Yes — `reset --hard` deletes them permanently
- **b.** No — they're still in git's storage, recoverable via reflog until `gc` runs
- **c.** Only if you've already pushed; locally they remain
- **d.** Only if you have a backup branch pointing at them

## 3. Which of these can git's safety net NOT recover?

- **a.** A commit you made and then ran `reset --hard` past
- **b.** A branch you deleted yesterday
- **c.** Uncommitted changes wiped by `git restore .`
- **d.** Commits orphaned by a force-push, found within hours

## 4. What does `git fsck --lost-found` do?

- **a.** Repairs corrupted git internals
- **b.** Finds dangling commits and writes them to `.git/lost-found/`
- **c.** Pulls all branches from the remote
- **d.** Verifies that the working tree matches HEAD

## 5. What is "the rescue rule"?

- **a.** Always make a backup branch before any destructive command
- **b.** Read state (`git status`, `git log`) before running a command that changes history
- **c.** Never use `--force` unless you're alone on the repo
- **d.** Run `git gc` after every rescue to clean up

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
