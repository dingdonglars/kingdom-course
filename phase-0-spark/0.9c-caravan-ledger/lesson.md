# Module 0.9c — Caravan Ledger

This is the last build-from-scratch checkpoint before the Kingdom. It's a small step harder than the Quest Board, and there is one genuinely new idea in it — clearly marked below, so you'll know exactly what's new and why. Everything else is the Phase 0 toolkit you already own.

Same deal as before: build it from an empty file, no lessons open, no copying. Then show Lars and talk it through. Take your time — this one rewards going slowly.

## The rules

- **Lessons closed.** Don't scroll back through Phase 0 while you build. (Getting stuck and reading *one* lesson is allowed — see below.)
- **No copying.** Type this one fresh — not from your Quest Board or anything earlier.
- **Just you.** Every line your own.
- **Take your time.** Run it often.
- **Getting stuck is information, not failure.** Write down *where* you got stuck.

## Set up your project — the standard way

Same habit as the Quest Board. One program, one window. (Full version: `running-your-project.md`.)

1. Make the program inside your `kingdom` folder:

   ```powershell
   cd C:\code\kingdom
   dotnet new console -o CaravanLedger
   ```

2. Open **that** folder as the window: **File → Open Folder…** → `C:\code\kingdom\CaravanLedger`. The file tree must say **CaravanLedger**, not kingdom.
3. Run with `dotnet run`. Debug with **F5**.

> **One program, one window.** Open the whole `kingdom` folder by mistake and *Run*/F5 won't know which program you mean (the *"more than one project"* error). The fix is opening the single program's folder — never your code. Check the title bar first.

## What you are building — the Caravan Ledger

A command-line tool that tracks trading caravans on the road. Each caravan carries some number of **crates**, and each crate has a **price**. A caravan's worth is simply *crates × price*.

It runs in a loop until the user types `quit`. The commands:

| Command | What it does |
|---|---|
| `add <name> <crates> <price>` | Register a caravan: how many crates it carries, and the price of one crate. |
| `sell <name>` | The caravan reached market and sold — take it off the ledger. |
| `value <name>` | Show what one caravan is worth (crates × price). |
| `ledger` | List every caravan: its crates, its price, and its worth. |
| `wealth` | Show the total worth of *all* caravans on the road. |
| `help` | List the commands. |
| `quit` | Leave the program. |

An example run:

```text
Caravan Ledger. Type 'help' for commands.
> add silk 10 5
Caravan 'silk': 10 crates at 5 coins each.
> add spice 4 25
Caravan 'spice': 4 crates at 25 coins each.
> value silk
'silk' is worth 50 coins.
> wealth
Total wealth on the road: 150 coins.
> sell silk
Caravan 'silk' sold.
> wealth
Total wealth on the road: 100 coins.
> quit
Bye.
```

Make the wording yours. The list below is what matters.

## The one new idea — two dictionaries that share a key

Here's the new bit, and it's the whole reason this checkpoint is a step up.

Every caravan has **two** numbers to remember: how many crates, *and* the price per crate. A single `Dictionary<string, int>` only holds **one** number per name. So how do you store two?

The trick: keep **two dictionaries that share the same key** — the caravan's name.

```csharp
var crates = new Dictionary<string, int>();   // crates["silk"] = 10
var price  = new Dictionary<string, int>();   // price["silk"]  = 5
```

Both use the caravan's name as the key. `crates["silk"]` tells you how many crates the silk caravan has; `price["silk"]` tells you the price of one of its crates. Together they describe one caravan.

The only rule you must follow: **keep them in step.** Whenever you add a caravan, write to *both*. Whenever you remove one, remove it from *both*. If they ever fall out of step — a name in one but not the other — you'll get an error when you look it up. (That's exactly the kind of thing your `try / catch` will catch calmly instead of crashing.)

That's it. No new C# keyword — just a tidy way to use two of a tool you already know.

## The must-haves

All six, same as before, plus the two-dictionary idea above:

1. **A loop** until the user types `quit`.
2. **Collections** — the two `Dictionary<string, int>` above, sharing the caravan name as their key.
3. **A method you wrote that returns a value.** Here it does a little *maths*: a `Value` method that takes a caravan's name and returns `crates × price`. Your `wealth` total is a second value-returning method that adds up every caravan's value.
4. **Number checking with `int.TryParse`** — and now you check **two** numbers when adding a caravan (the crates *and* the price). If either isn't a number, say so and carry on; don't crash.
5. **A `try / catch`** around the command handling.
6. **Clear messages** with string interpolation (`$"'{name}' is worth {worth} coins."`).

## Build it

One piece at a time, running after each:

1. The command loop — read a line, stop on `quit`. Run it.
2. Add `help`. Run it.
3. Add `add` — split the line, check **both** numbers with `TryParse`, write to **both** dictionaries. Run it. Try a bad number in each spot on purpose.
4. Add `value` — write the `Value` method that returns crates × price, and have the command print it. Run it.
5. Add `ledger` and `sell` (remember: `sell` removes from *both* dictionaries). Run after each.
6. Write `wealth` last — a method that loops every caravan and adds up each one's value. Run it.
7. Wrap the command handling in `try / catch`. Run once more.

## If you get stuck

Find the piece, read *only that one lesson*, come back, and note that you needed it.

| Stuck on… | Go back to |
|---|---|
| Reading input, printing, `$"..."` | Module 0.1 — Tinker |
| The `while` loop, `if`, `break` | Module 0.2 — Number Guess |
| A method that takes input and returns a value | Module 0.6 — Methods |
| `Dictionary` — storing and looking up by name | Module 0.7 — Collections |
| `int.TryParse`, `try / catch` | Module 0.8 — Errors and Debugging |

## Show Lars — the checkpoint

When your Caravan Ledger runs and does all six must-haves, book some time with Lars.

You will:

1. **Run it for him.** He'll try a few things, including a bad number in each slot, to check it doesn't crash.
2. **Walk him through your code**, in your own words — especially how the two dictionaries stay in step, and how your `Value` method works out a caravan's worth.
3. **Maybe make a small change on the spot** while he watches.

If something's shaky, that's not a fail — it's the exact thing to firm up before the Kingdom needs it. A little practice on that one piece, then show him again. That's a win.

## Wrap up

No quiz — building the Caravan Ledger *is* the check.

1. **Progress** — one line in `journal/progress.md`: `Module 0.9c — Caravan Ledger — DATE — built the Caravan Ledger from scratch.`
2. **Commit and push** — check Source Control says `kingdom` (not `kingdom-course`!), stage your work, commit message `Module 0.9c done`, Sync. (The terminal commit in `running-your-project.md` is the easy way from a one-program window.)
3. **Post in `#wins`** — one line about your Caravan Ledger, plus the commit URL.

The real "done" is passing the checkpoint with Lars.

## Next

That's the last checkpoint. Next is **Phase 1 — the Kingdom begins.** In Module 1.1 you meet your first **classes** and build the first version of the Kingdom: buildings, citizens, resources. Everything you've just shown you can do, you'll use from line one.
