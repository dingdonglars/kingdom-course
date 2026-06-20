# Module 0.9c — The Arena

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

This is the last build-from-an-empty-file checkpoint before the Kingdom — and it's the most fun of the three. You built two small trackers (the Tavern Tab and the Quest Board). This one is a tiny game: a turn-by-turn fight in an arena. Monsters walk in, you swing at them, they swing back, and the round ends when either the arena is clear or you fall.

It uses the exact same Phase 0 tools you already have — a loop, a collection, a method that gives a number back, number-checking, and `try / catch`. Nothing new in the language. What's new is the *kind of program* you are making. The first two just stored and showed things. This one has a goal you can win or lose, and numbers that change every single turn. That's why it's the real test: you can't win it by remembering the order of the steps from last time. You have to actually understand the pieces.

There is one genuinely new trick in it too — marked clearly below: keeping two facts about each monster together. Everything else, you've done before.

Same deal as always: build it from an empty file, no lessons open, no copying. Then show Lars and talk it through. Go slow on this one — it rewards it.

## The rules

- **Lessons closed.** Don't scroll back through Phase 0 while you build. (Getting stuck and reading *one* lesson is allowed — see below.)
- **No copying.** Type this one fresh — not from your Tavern Tab, your Quest Board, or anything earlier.
- **Just you.** Every line your own.
- **Take your time.** Run it often.
- **Getting stuck is information, not failure.** Write down *where* you got stuck. That note tells you and Lars exactly what to practise.

## Set up your project — the standard way

Same habit as the last two. One program, one window. (Full version: `running-your-project.md`.)

1. Make the program inside your `kingdom` folder:

   ```powershell
   cd C:\code\kingdom
   dotnet new console -o Arena
   ```

2. Open **that** folder as the window: **File → Open Folder…** → `C:\code\kingdom\Arena`. The file tree on the left must say **Arena**, not kingdom.
3. Run with `dotnet run`. Debug with **F5**.

> **One program, one window.** Open the whole `kingdom` folder by mistake and *Run*/F5 won't know which program you mean (the *"more than one project"* error). The fix is opening the single program's folder — never your code. Check the title bar first.

## What you are building — the Arena

Picture a small fighting pit. You are the hero. You start with **30 HP** (HP means *health points* — how much damage you can take before you fall). Monsters come into the arena one at a time as you call them in. Each monster has two numbers of its own:

- its **HP** — how much health it has, and
- its **attack** — how hard it hits you back.

A turn works like this. You pick a monster and say how hard you swing. Your sword is strong: it **doubles** your swing. So a swing of 4 deals 8 damage. That damage comes off the monster's HP.

Then, if the monster is still alive, it hits *you* back for its attack number, and that comes off your 30 HP. When a monster's HP reaches zero, it falls and leaves the arena. When *your* HP reaches zero, you fall and the round is over.

You win when every monster you called in has fallen. You lose if your HP runs out first. That win-or-lose ending is the big difference from the Tavern Tab and the Quest Board — those just ran until you typed `quit`. This one has a finish line.

Here are the commands it must understand:

| Command | What it does |
|---|---|
| `spawn <name> <hp> <attack>` | Bring a monster into the arena: its name, its health, and how hard it hits back. |
| `hit <name> <swing>` | Swing at that monster. Your sword doubles your swing. If the monster lives, it hits you back. |
| `field` | Show every monster still standing — its HP and its attack. |
| `threat` | Show the total HP left across *all* monsters — how much fight is left. |
| `hero` | Show your own HP. |
| `help` | List the commands. |
| `quit` | Leave the arena early. |

An example run might look like this:

```text
The Arena. Type 'help' for commands.
> spawn goblin 10 3
A goblin enters the arena. 10 HP, hits for 3.
> spawn troll 18 6
A troll enters the arena. 18 HP, hits for 6.
> hero
You have 30 HP.
> hit goblin 4
You swing for 4. Your sword doubles it to 8. The goblin has 2 HP left.
The goblin hits back for 3. You have 27 HP.
> hit goblin 4
You swing for 4. Your sword doubles it to 8. The goblin falls!
> threat
18 HP of monsters still standing.
> hit troll 10
You swing for 10. Your sword doubles it to 20. The troll falls!
The arena is clear. You win!
```

You don't have to match that wording. Make it yours. The list of must-haves below is what matters.

## The one new idea — two dictionaries that share a key

Here's the new bit, and it's the reason this checkpoint is a step up from the last one.

Every monster has **two** numbers to remember: its HP *and* its attack. But a single `Dictionary<string, int>` only holds **one** number per name. So how do you store two numbers for the same monster?

The trick: keep **two dictionaries that share the same key** — the monster's name.

```csharp
var hp     = new Dictionary<string, int>();   // hp["goblin"]     = 10
var attack = new Dictionary<string, int>();   // attack["goblin"] = 3
```

Both use the monster's name as the key. `hp["goblin"]` tells you the goblin's health; `attack["goblin"]` tells you how hard the goblin hits. Read together, they describe one monster.

The only rule you must follow: **keep them in step.** Whenever a monster enters, write to *both*. Whenever a monster falls, remove it from *both*. If they ever fall out of step — a name in one but not the other — you'll get an error the moment you look it up. (That's exactly the kind of thing your `try / catch` will catch calmly instead of crashing.)

That's it. No new C# keyword — just a tidy way to use two of a tool you already know.

## A quick word on semicolons and brackets

Before you build, a short reminder about something that trips everyone up at first: *where the `;` goes, and where the `{ }` go.* Getting this right by yourself is half of writing code that runs. Read this slowly — it's worth it.

A **statement** is one single instruction. A statement ends with a semicolon. Three instructions means three semicolons:

```csharp
int score = 0;
score = score + 5;
Console.WriteLine(score);
```

But a line that **opens a block** is not a finished instruction — so it does *not* end with a semicolon. It ends with an opening brace `{`. The lines that open a block are things like `while (...)`, `if (...)`, `foreach (...)`, and the first line of a method:

```csharp
while (score < 10)
{
    score = score + 1;
}
```

Look closely at the three lines:

- `while (score < 10)` — **no semicolon.** It isn't finished. The `{ }` underneath it is its body. A semicolon here is one of the most common mistakes there is.
- `score = score + 1;` — a real, finished instruction, so it **ends with `;`**.
- The `{` on its own line has a matching `}` to close the block.

Two habits that catch almost every bracket and semicolon problem, and you can do both alone:

1. **Read each line and ask: "Is this one finished instruction, or is it opening a block?"** Finished instruction → end it with `;`. Opening a block → end it with `{`, no `;`.
2. **Every opener has a closer.** Every `(` needs its `)`. Every `{` needs its `}`. When you type a `{`, type the `}` straight away, then fill the middle. That way you never lose count.

If the program won't run and the error mentions `; expected` or `} expected`, come straight back to this list. It's almost always one of these two things — not the hard part of your code.

## The must-haves

Your program has to show all six of these, plus the two-dictionary idea above. This is what Lars will look for — the same six skills from the Tavern Tab and the Quest Board, in a fresh setting.

1. **A loop** that runs turn after turn. Unlike the last two, this loop can end three ways: you type `quit`, every monster falls (you win), or your HP hits zero (you lose). A `break` or a `return` is how you leave it.

2. **Two collections that share a key** — the two `Dictionary<string, int>` above. The monster's name is the key in both. `hp[name]` holds its health; `attack[name]` holds its hit. Add to both together, remove from both together.

3. **A method you wrote that returns a value** — not a `void` one. You've got two natural homes for this, and both do a little maths:
   - A `Damage` method that takes a swing and gives back the doubled number (`swing * 2`). You call it every time you hit.
   - A `threat` total: a method that loops every monster and adds up their HP, then *gives back* the sum.

   Pick at least one of these to be a real method that takes something in and hands a number back. (Doing both is even better.)

4. **Number checking with `int.TryParse`.** You now check numbers in two places. When a monster spawns you check **two** numbers — its HP *and* its attack. When you hit, you check the swing. If any of them isn't a number — say someone types `spawn goblin ten 3` — the program must not crash. It should say something polite and carry on.

5. **A `try / catch`** around the command handling, so an unexpected slip (like looking up a monster that isn't there) doesn't kill the whole program.

6. **Clear messages** built with string interpolation — for example `$"The {name} hits back for {power}. You have {heroHp} HP."`.

## Build it

Build it one piece at a time, and run it after *every* piece. That habit is what turns this from scary into easy. A good order:

1. **The loop and `quit` first.** Read a line, and if it's `quit`, stop. Print a welcome line above the loop. Run it. Make sure typing `quit` actually leaves.
2. **Add `help`.** Just print the list of commands. Run it.
3. **Add `spawn`.** Split the line into parts. Check **both** numbers with `TryParse`. Write the HP into one dictionary and the attack into the other, using the name as the key for both. Print a line saying the monster arrived. Run it — and on purpose, try a bad number in each slot to prove it doesn't crash.
4. **Add `hero` and `field`.** `hero` just prints your HP. `field` loops the monsters and prints each one's HP and attack. These are quick and let you *see* your data. Run after each.
5. **Add `hit`.** This is the heart of the game, so go slowly:
   - Check the monster's name is really there.
   - Check the swing is a number.
   - Work out the damage (your `Damage` method — swing doubled) and take it off that monster's HP.
   - If the monster's HP is now zero or less, it falls: remove it from **both** dictionaries and say so. Then check — was that the last monster? If so, you win.
   - Otherwise the monster hits back: take its attack off your HP, and say so. Then check — did *your* HP hit zero? If so, you lose.

   Run it after you get each part working. Test a hit that kills, a hit that doesn't, and a name that isn't there.
6. **Add `threat` last.** Write the method that loops every monster, adds up their HP, and returns the total. Have the command print it. Run it.
7. **Wrap the command handling in a `try / catch`.** Run it once more, and try to break it on purpose — a missing number, a monster that isn't there, a nonsense command. None of it should crash the program.

## If you get stuck

This is allowed — it's the useful part. Find the piece you're stuck on, go back and read *only that one lesson*, then come back and keep going. Note that you needed it.

| Stuck on… | Go back to |
|---|---|
| Reading input, printing, `$"..."` | Module 0.1 — Tinker |
| The `while` loop, `if`, `break` | Module 0.2 — Number Guess |
| A method that takes input and returns a value | Module 0.6 — Methods |
| `Dictionary` — storing and looking up by name | Module 0.7 — Collections |
| `int.TryParse`, `try / catch` | Module 0.8 — Errors and Debugging |

## Show Lars — the checkpoint

When your Arena runs and does all six must-haves, book some time with Lars. This is a normal, friendly part of the work — not a scary exam.

You will:

1. **Run it for him.** He'll try a few things, including a bad number in each slot and a monster that isn't there, to check it doesn't crash.
2. **Walk him through your code**, in your own words — especially how the two dictionaries stay in step, and how your method works out the damage or the total threat.
3. **Maybe make a small change on the spot** — he might ask you to add one little thing while he watches. That's the best way to show the ideas are really yours.

If something's shaky, that's not a fail — it's the exact thing to firm up before the Kingdom needs it. A little practice on that one piece, then show him again. That's a win.

## Wrap up

No quiz — building the Arena *is* the check.

1. **Progress** — one line in `journal/progress.md`: `Module 0.9c — The Arena — DATE — built the Arena from scratch.`
2. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage your work, commit message `Module 0.9c done`, Sync. (The terminal commit in `running-your-project.md` is the easy way from a one-program window.)
3. **Post in `#wins`** — one line about your Arena, plus the URL of the commit.

The real "done" is passing the checkpoint with Lars — the push just saves your work.

## Next

That's the last checkpoint. Next is **Phase 1 — the Kingdom begins.** In Module 1.1 you meet your first **classes** and build the first version of the Kingdom: buildings, citizens, resources. And remember how you kept two dictionaries in step for each monster? A class is the tidy answer to that — one thing that holds all of a monster's numbers together. Everything you've just shown you can do, you'll use from line one.
