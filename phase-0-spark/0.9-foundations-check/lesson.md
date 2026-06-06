# Module 0.9 — Foundations Check

Phase 0 is done. You have two finished programs on GitHub, and the basic parts of C# now have names. Before the Kingdom begins, there is one checkpoint: build a small new program from an empty file, using what you learned in Phase 0 — no lessons open, and no copying old code. Then show it to Lars and talk through it.

This is a checkpoint. Phase 1 — the Kingdom — starts once you and Lars are both happy that you really know the basics.

It is not a trick, and it is not a race. Take your time with this one small program — there is no rush. If there is something you are not sure about, this is the easiest and kindest place to find it — now, before the Kingdom needs it.

## Why we stop here

Phase 1 is different from Phase 0. The toys you made so far were each finished in a day. The Kingdom is one project that grows for six weeks. Each new lesson uses what the last one taught. Classes use methods. Tests use classes. The game loop uses all of it.

That is great when you really know the earlier parts. It is hard when you only half-know them, because a small thing you didn't quite get now turns into big confusion a few lessons later — and by then it is hard to even see where the confusion started.

So we check the basics first. One small program, one honest talk. That is the whole checkpoint.

## The rules

Read these before you start. They are what make the check mean something.

- **Lessons closed.** Do not scroll back through Phase 0 while you build. (If you get stuck, see "If you get stuck" below — that is allowed, and it is the point.)
- **No copying.** Don't paste from your Roast-O-Matic or your Inventory Tool. Type this one fresh.
- **Just you.** No asking a friend for the answer — every line should be your own. (You're not using AI on the course yet; that comes much later, so for now this one is easy.)
- **Take your time.** There is no rush on this one. Go slow. Run it often.
- **Getting stuck is information, not failure.** When you get stuck, write down *where*. That note is useful — it tells you and Lars exactly what to practise.

## What you are building — the Tavern Tab

A small command-line tool that tracks what each villager owes at the tavern.

It runs in a loop, reading one command at a time, until the user types `quit`. Here are the commands it must understand:

| Command | What it does |
|---|---|
| `order <name> <coins>` | Add that many coins to the villager's tab. If the villager is new, start their tab. |
| `paid <name>` | The villager paid up — clear their tab. |
| `tab <name>` | Show one villager's tab. |
| `all` | Show every villager and what they owe. |
| `total` | Show the total owed across *everyone*. |
| `help` | List the commands. |
| `quit` | Leave the program. |

An example run might look like this:

```text
Tavern Tab. Type 'help' for commands.
> order bob 3
Bob now owes 3 coins.
> order bob 2
Bob now owes 5 coins.
> order alice 4
Alice now owes 4 coins.
> total
The whole village owes 9 coins.
> paid bob
Bob has paid up.
> all
Alice owes 4 coins.
> quit
Bye.
```

You don't have to match that wording exactly. Make it yours. What matters is the list below.

## The must-haves

Your program has to show all six of these. This is what Lars will look for.

1. **A loop** that keeps asking for commands until the user types `quit`.
2. **A collection** that holds each villager's name and what they owe. (A `Dictionary<string, int>` fits this perfectly.)
3. **A method you wrote yourself that returns a value** — not a `void` one. The `total` command is the natural place: a method that adds up every tab and *gives back* the number.
4. **Number checking with `int.TryParse`.** If someone types `order bob abc`, the program must not crash. It should say something polite and carry on.
5. **A `try / catch`** so an unexpected error doesn't kill the whole program.
6. **Clear messages** built with string interpolation (`$"Bob now owes {amount} coins."`).

## Build it

Start small and add to it one piece at a time. A good order:

1. Write the command loop first — read a line, and if it's `quit`, stop. Run it.
2. Add `help`. Run it.
3. Add `order` — split the line into parts, check the number with `TryParse`, store it in the dictionary. Run it. Test a bad number on purpose.
4. Add `tab`, `all`, and `paid`. Run after each.
5. Write the `total` method last. Run it.
6. Wrap the command handling in a `try / catch`. Run it once more.

Run it after *every* small step. That is the habit that makes this feel easy instead of scary.

## If you get stuck

This is allowed — it's the useful part. Find which piece you're stuck on, go back and read *only that one lesson*, then come back and keep going. Note that you needed it.

| Stuck on… | Go back to |
|---|---|
| Reading input, printing, `$"..."` | Module 0.1 — Tinker |
| The `while` loop, `if`, `break` | Module 0.2 — Number Guess |
| Writing a method that returns a value | Module 0.6 — Methods |
| `Dictionary` — storing and looking up by name | Module 0.7 — Collections |
| `int.TryParse`, `try / catch` | Module 0.8 — Errors and Debugging |

## Show Lars — the checkpoint

When your Tavern Tab runs and does all six must-haves, book some time with Lars. This is the real checkpoint, and it is a normal, friendly part of the work — not a scary exam.

You will:

1. **Run it for him.** He'll try a few things, including a bad number, to check it doesn't crash.
2. **Walk him through your code**, in your own words. He'll ask "why is this line here?" about a few spots. There are no trick questions; he just wants to hear *you* explain *your* code.
3. **Maybe make a small change on the spot** — he might ask you to add one little thing while he watches. That's the best way to show the ideas are really yours.

When you and Lars are both happy, you've passed the checkpoint and the Kingdom begins.

If there is something you are not sure about, that is not a fail. It means you found the exact thing to practise before it could cause trouble later. You'll spend a little time on that one piece, then show him again. That is a win, not a setback.

## Wrap up

There's no quiz this time — building the Tavern Tab *is* the check.

1. **Progress** — one line in `journal/progress.md`: `Module 0.9 — Foundations Check — DATE — built the Tavern Tab from scratch.`
2. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage your work, commit message `Module 0.9 done`, Sync.
3. **Post in `#wins`** — one line about your Tavern Tab, plus the URL of the commit.

The real "done" for this module is passing the checkpoint with Lars — not the push. The push just saves your work.

## Next

Two more short checkpoints like this one come first — Module 0.9b (the **Quest Board**) and Module 0.9c (the **Caravan Ledger**). Each is another small program built from a blank file, so the Phase 0 skills are rock-solid before the Kingdom starts to lean on them.

Then Phase 1 — **the Kingdom begins.** In Module 1.1 you meet your first **classes** and build the first version of the Kingdom: buildings, citizens, resources. You'll use everything you just showed you can do.
