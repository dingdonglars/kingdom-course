# Quiz — Bonus B3.2

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does `git rebase` do to your commits?

- **a.** Combines them into a single commit with a new message
- **b.** Replays them onto a new base, giving them new SHAs
- **c.** Moves their parent pointer without touching the commit itself
- **d.** Permanently deletes them and creates a fresh commit

## 2. What's a *fast-forward* merge?

- **a.** A merge that runs faster because the branches haven't diverged
- **b.** A merge that skips creating a merge commit; the pointer just slides forward
- **c.** A merge that ignores conflicts and picks the source branch's version
- **d.** A merge that runs in the background while you keep working

## 3. Why is rebasing a *shared* branch dangerous?

- **a.** It can produce merge conflicts that are hard to resolve
- **b.** Other people who pulled the old version now have commits with different SHAs than the remote
- **c.** It overwrites the commit messages with default text
- **d.** It causes the GitHub web UI to display the wrong history

## 4. What's the difference between `--force` and `--force-with-lease`?

- **a.** `--force` is for the local repo, `--force-with-lease` is for the remote
- **b.** `--force-with-lease` adds a backup tag before force-pushing
- **c.** `--force-with-lease` refuses to push if the remote has moved since your last fetch
- **d.** They're aliases for the same command

## 5. When is rebase the right choice over merge?

- **a.** When you want a clean linear history on your *own* unpushed feature branch
- **b.** When you're combining work into a shared branch like `main`
- **c.** When you want the history to honestly show parallel work
- **d.** When you want to avoid changing any commit SHAs

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
