# Module 0.3 — Tiny Adventure

Today you build a three-room text adventure. *"You stand in a dim hallway. There's a door north and a door east. >"* The world is yours; the bones come from this lesson. Two new ideas land along the way: a *list* (how the player carries items between rooms) and writing your own *methods* (a method per room, calling each other as the player walks around).

> **Words to watch**
>
> - **list** — an ordered collection of things you can add to, remove from, and loop through
> - **`List<string>`** — a list specifically of strings (text items)
> - **method (defined)** — until now you've only *called* methods (`Console.WriteLine(...)`); today you *write* one
> - **organisation** — splitting your code into multiple methods so each does one thing

---

## Step 1 — make a new project

```powershell
cd ..
dotnet new console -n TinyAdventure
cd TinyAdventure
```

Open `Program.cs` and replace the contents:

```csharp
var inventory = new List<string>();

Hallway();

void Hallway()
{
    Console.WriteLine();
    Console.WriteLine("You stand in a dim hallway. There's a door north and a door east.");
    Console.Write("> ");
    var choice = Console.ReadLine().Trim().ToLower();

    if (choice == "north") Kitchen();
    else if (choice == "east") Library();
    else
    {
        Console.WriteLine("That's not a direction. Try 'north' or 'east'.");
        Hallway();
    }
}

void Kitchen()
{
    Console.WriteLine();
    Console.WriteLine("A kitchen, smells of stew. There's a knife on the counter.");
    Console.Write("> ");
    var choice = Console.ReadLine().Trim().ToLower();

    if (choice == "take knife")
    {
        inventory.Add("knife");
        Console.WriteLine("You pocket the knife. The cook is going to notice.");
        Hallway();
    }
    else if (choice == "back" || choice == "south")
    {
        Hallway();
    }
    else
    {
        Console.WriteLine("Try 'take knife' or 'back'.");
        Kitchen();
    }
}

void Library()
{
    Console.WriteLine();
    Console.WriteLine("A library, dust motes in the air. A book lies open on the table.");
    Console.Write("> ");
    var choice = Console.ReadLine().Trim().ToLower();

    if (choice == "read book")
    {
        if (inventory.Contains("knife"))
        {
            Console.WriteLine("The book reveals: 'The cook took the family's last gold. The knife is justice.'");
            Console.WriteLine("YOU WIN. Probably.");
        }
        else
        {
            Console.WriteLine("The book is in a language you don't know. You should look around first.");
            Library();
        }
    }
    else if (choice == "back" || choice == "west")
    {
        Hallway();
    }
    else
    {
        Console.WriteLine("Try 'read book' or 'back'.");
        Library();
    }
}
```

There are two new ideas in this code worth pausing on. The first line creates a `List<string>` — a list that holds text items, starting empty. Each method (`Hallway`, `Kitchen`, `Library`) is one room. When the player chooses a direction, the current room's method calls the next room's method directly. The methods call each other; that's how the player walks around.

The other small thing to notice: `Console.ReadLine().Trim().ToLower()` reads a line, removes any spaces around the edges (`Trim`), and converts it to lowercase (`ToLower`). That way `"NORTH"`, `" north "`, and `"North"` all match `"north"`.

Run it:

```powershell
dotnet run
```

Walk around. Try every command. Try the win condition (take the knife, read the book).

## Tinker

Add a fourth room — a basement, a garden, a tower — and connect it to one of the existing rooms.

Add an item the player can pick up in your new room. Maybe a key, a candle, an old letter.

Add a second win condition that needs the new item. Now the game has two ways to win.

Make the rooms talk to the player about what's in their `inventory` if they type `look` or `inventory`. You'll need a small loop to print each item.

## Name it

A **list** is an ordered collection. `List<string>` is C#'s flexible list type, specifically holding strings. You created an empty one with `new List<string>()`. You added to it with `inventory.Add(...)`. You checked membership with `inventory.Contains(...)`. The `<string>` part says *"this is a list of strings"* — you'll see `<int>`, `<Building>`, and others later. The angle brackets are how you tell a generic class what type it works with.

A **method (defined)** is one you write yourself. You wrote three today: `Hallway()`, `Kitchen()`, `Library()`. The `void` keyword in front says the method doesn't give back a value — it just *does* something. The body runs when the method is called. When the body finishes, control returns to the line that called it.

**Organisation** is what splitting code into methods buys you. Imagine the same game written as one giant `if/else` chain — three rooms, three sets of choices, all jammed together. Unreadable. With each room as a method, the structure of the game maps to the structure of the code. That mapping is the whole point.

## What you just did

You wrote a real game with state — an inventory the player carries between rooms — and you wrote your own methods for the first time. Three rooms, each its own method, calling each other as the player walks around. You met `List<string>` for storing items, and you saw what `void` methods look like when *you* write them rather than just calling them. The whole adventure is about ninety lines of code, split into four named pieces that each do one thing.

**Key concepts you can now name:**

- **`List<T>`** — generic ordered collection
- **`Add` and `Contains`** — append item, check membership
- **method definition** — `void Name() { ... }` you wrote
- **organisation** — split code so structure mirrors problem
- **`void` return** — method that does, doesn't give back

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 0.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage both files, commit message `Module 0.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 0.4 is a polish day. You pick your favourite of the three programs from this week, dress it up, and ship it. End of Spark Week.
