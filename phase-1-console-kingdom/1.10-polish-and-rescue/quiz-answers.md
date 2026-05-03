# Quiz answers — Module 1.10

## 1. b
What / How to run / What you learned / What's next. Every README in this curriculum follows this shape. (License/changelog/contact are useful for libraries, not for personal/learning projects.)

## 2. b
`git stash` saves uncommitted changes to a stack and restores the working tree to a clean state. Recover with `git stash pop` (or `git stash apply` if you want to keep the stash entry). It's the *non-destructive* way to set work aside.

## 3. b
*Read state before acting.* `git status` shows what's changed; `git log --oneline -10` shows recent commits. Most git incidents are "I didn't know the state I was in, tried something, made it worse." The rescue is in the read.

## 4. b
`git reset --hard` discards uncommitted changes (and resets the branch pointer). If you reset back further than your last commit, work is lost unless it was committed. Use only when you're sure you want the destination state.

## 5. b
The before/after one-liner is what your future self reads in a year. The Discord post is the public commitment — it makes the win real, and the channel becomes a record of the journey.