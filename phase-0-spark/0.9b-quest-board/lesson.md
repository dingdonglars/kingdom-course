# Module 0.9b — Quest Board

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

You built the Tavern Tab from a blank file. Here is one more, the same kind of checkpoint: a small new program, made from scratch, using only what Phase 0 taught you. Then you show it to Lars and talk through it.

Why a second one? Because the skills that carry you through the whole Kingdom — a loop, a collection, a method that gives something back, checking numbers, catching errors — get stronger every time you reach for them from your own head instead of copying. The Tavern Tab proved you can do it once. This proves it wasn't a fluke. Take your time; there is no rush.

## The rules

Same as last time. They are what make the check mean something.

- **Lessons closed.** Don't scroll back through Phase 0 while you build. (If you get stuck, see "If you get stuck" below — that's allowed, and it's the point.)
- **No copying.** Don't paste from your Tavern Tab, your Roast-O-Matic, or your Inventory Tool. Type this one fresh.
- **Just you.** No asking a friend for the answer — every line should be your own.
- **Take your time.** Go slow. Run it often.
- **Getting stuck is information, not failure.** When you get stuck, write down *where*. That note tells you and Lars exactly what to practise.

## Set up your project — the standard way

Before you write a line, get the project open the right way. This is the habit that keeps *Run* and the debugger calm. The full version is in `running-your-project.md`; here's the short form.

1. Open a terminal and make the new program inside your `kingdom` folder:

   ```powershell
   cd C:\code\kingdom
   dotnet new console -o QuestBoard
   ```

2. Open **that** folder as the window: **File → Open Folder…** → `C:\code\kingdom\QuestBoard`. The file tree on the left must say **QuestBoard**, not kingdom.
3. Run with `dotnet run`. Debug with **F5**. Both work with no extra setup, because there's exactly one program in the window.

> **One program, one window.** If you ever open the whole `kingdom` folder instead, `dotnet run` and **F5** won't know which program you mean — that's the *"more than one project"* error. The fix is never your code; it's opening the single program's folder. Check the title bar first.

## What you are building — the Quest Board

A small command-line tool that tracks the quests on a notice board and the coins each one pays.

It runs in a loop, reading one command at a time, until the user types `quit`. The commands it must understand:

| Command | What it does |
|---|---|
| `post <name> <reward>` | Put a quest on the board for that many coins. (If the quest name is already there, the new reward replaces the old one.) |
| `done <name>` | The quest is finished — take it off the board. |
| `reward <name>` | Show what one quest pays. |
| `board` | Show every open quest and its reward. |
| `bounty` | Show the total coins promised across *all* open quests. |
| `help` | List the commands. |
| `quit` | Leave the program. |

An example run might look like this:

```text
Quest Board. Type 'help' for commands.
> post slay-dragon 100
Quest 'slay-dragon' posted for 100 coins.
> post fetch-herbs 20
Quest 'fetch-herbs' posted for 20 coins.
> bounty
Total bounty on the board: 120 coins.
> done fetch-herbs
Quest 'fetch-herbs' is done.
> board
slay-dragon — 100 coins.
> quit
Bye.
```

You don't have to match that wording. Make it yours. What matters is the list below.

## The must-haves

Your program has to show all six of these. This is what Lars will look for — the same six the Tavern Tab needed, in a fresh shape.

1. **A loop** that keeps asking for commands until the user types `quit`.
2. **A collection** that holds each quest's name and its reward. (A `Dictionary<string, int>` fits perfectly — the name is the key, the reward is the value.)
3. **A method you wrote yourself that returns a value** — not a `void` one. The `bounty` command is the natural place: a method that adds up every reward and *gives back* the number.
4. **Number checking with `int.TryParse`.** If someone types `post slay-dragon abc`, the program must not crash. It should say something polite and carry on.
5. **A `try / catch`** so an unexpected error doesn't kill the whole program.
6. **Clear messages** built with string interpolation (`$"Quest '{name}' posted for {reward} coins."`).

## Build it

Start small and add one piece at a time. A good order:

1. Write the command loop first — read a line, and if it's `quit`, stop. Run it.
2. Add `help`. Run it.
3. Add `post` — split the line into parts, check the reward with `TryParse`, store it in the dictionary. Run it. Test a bad number on purpose.
4. Add `reward`, `board`, and `done`. Run after each.
5. Write the `bounty` method last. Run it.
6. Wrap the command handling in a `try / catch`. Run it once more.

Run it after *every* small step. That habit is what makes this feel easy instead of scary.

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

When your Quest Board runs and does all six must-haves, book some time with Lars. This is a normal, friendly part of the work — not a scary exam.

You will:

1. **Run it for him.** He'll try a few things, including a bad number, to check it doesn't crash.
2. **Walk him through your code**, in your own words. He'll ask "why is this line here?" about a few spots. There are no trick questions; he just wants to hear *you* explain *your* code.
3. **Maybe make a small change on the spot** — he might ask you to add one little thing while he watches. That's the best way to show the ideas are really yours.

If there's something you're not sure about, that's not a fail. It means you found the exact thing to practise before it could cause trouble later. You'll spend a little time on that one piece, then show him again. That's a win.

## Wrap up

There's no quiz this time — building the Quest Board *is* the check.

1. **Progress** — one line in `journal/progress.md`: `Module 0.9b — Quest Board — DATE — built the Quest Board from scratch.`
2. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage your work, commit message `Module 0.9b done`, Sync. (If your window is the QuestBoard folder, the terminal commit shown in `running-your-project.md` is the easy way.)
3. **Post in `#wins`** — one line about your Quest Board, plus the URL of the commit.

The real "done" is passing the checkpoint with Lars — the push just saves your work.

## Next

One more checkpoint after this — Module 0.9c, the **Caravan Ledger** — and then Phase 1, the Kingdom, begins.
