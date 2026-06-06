# Module 0.3 — Tiny Adventure

Today you build a three-room text adventure. *"You stand in a dim hallway. There's a door north and a door east. >"* The world is yours to design; this lesson gives you the frame to build on. Two new ideas come up along the way: a *list* (how the player carries items between rooms) and writing your own *methods* (one method per room, each calling the next as the player walks around).

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

There are two new ideas in this code worth a closer look. The first line creates a `List<string>` — a list that holds text items, and it starts empty. Each method (`Hallway`, `Kitchen`, `Library`) is one room. When the player chooses a direction, the current room's method calls the next room's method directly. The methods call each other, and that's how the player walks around.

One more small thing to notice: `Console.ReadLine().Trim().ToLower()` reads a line, removes any spaces at the start and end (`Trim`), and changes it to lowercase (`ToLower`). That way `"NORTH"`, `" north "`, and `"North"` all match `"north"`.

Run it:

```powershell
dotnet run
```

Walk around. Try every command. Try to win the game (take the knife, then read the book).

## Tinker

Add a fourth room — a basement, a garden, a tower — and connect it to one of the rooms you already have.

Add an item the player can pick up in your new room. Maybe a key, a candle, or an old letter.

Add a second way to win that needs the new item. Now the game has two ways to win.

Make the rooms list what's in the player's `inventory` when they type `look` or `inventory`. You'll need a small loop to print each item.

## Name it

A **list** keeps things in order. `List<string>` is C#'s list type that can grow and shrink, and this one holds strings. You created an empty one with `new List<string>()`. You added to it with `inventory.Add(...)`. You checked whether something was in it with `inventory.Contains(...)`. The `<string>` part says *"this is a list of strings."* You'll see `<int>`, `<Building>`, and others later. The angle brackets are how you tell this kind of list which type it holds.

A **method (defined)** is one you write yourself. You wrote three today: `Hallway()`, `Kitchen()`, `Library()`. The `void` keyword in front says the method doesn't give back a value — it just *does* something. The body runs when the method is called. When the body finishes, the program goes back to the line that called it.

**Organisation** is what you get when you split code into methods. Imagine the same game written as one giant `if/else` chain: three rooms, three sets of choices, all crammed together. You couldn't read it. With each room as its own method, the parts of the code line up with the parts of the game. That match is the whole point.

## What you just did

You wrote a real game that remembers things — an inventory the player carries between rooms — and you wrote your own methods for the first time. Three rooms, each its own method, calling each other as the player walks around. You met `List<string>` for storing items, and you saw what `void` methods look like when *you* write them, not just call them. The whole adventure is about ninety lines of code, split into four named pieces that each do one thing.

**Key concepts you can now name:**

- **`List<T>`** — generic ordered collection
- **`Add` and `Contains`** — append item, check membership
- **method definition** — `void Name() { ... }` you wrote
- **organisation** — split code so structure mirrors problem
- **`void` return** — method that does, doesn't give back

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open a new empty file. Without looking:

1. Write your own method called `Greet` that takes nothing and gives nothing back (a `void` method).
2. Inside it, print a short message.
3. Above the method, call it twice.
4. Run it and check the message prints two times.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
Greet();
Greet();

void Greet()
{
    Console.WriteLine("Welcome, traveller.");
}
```

- `void` means the method gives nothing back — it just does something.
- You call a method by writing its name plus `()`.
- The two calls at the top run before the method below them. C# lets you write the method underneath.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 0.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage both files, commit message `Module 0.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 0.4 is a day for making things look nice. You pick your favourite of the three programs from this week, improve it, and finish it. End of Spark Week.
