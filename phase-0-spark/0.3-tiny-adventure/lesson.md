# Module 0.3 — Tiny Adventure

> **Hook:** today you build a 3-room text adventure. *"You stand in a dim hallway. There's a door north and a door east. > "* The world is yours; we just give you the bones.

> **Words to watch**
> - **list** — an ordered collection of things you can add to, remove from, and loop through
> - **`List<string>`** — a list specifically of strings (text items)
> - **method (defined)** — until now you've only *called* methods (`Console.WriteLine(...)`); today you *write* one
> - **organisation** — splitting your code into multiple methods so each does one thing

---

## Do it

```powershell
cd ..
dotnet new console -n TinyAdventure
cd TinyAdventure
```

Open `Program.cs`. Replace with:

```csharp
var inventory = new List<string>();

Hallway();

void Hallway()
{
    Console.WriteLine();
    Console.WriteLine("You stand in a dim hallway. There's a door north and a door east.");
    Console.Write("> ");
    var choice = Console.ReadLine()?.Trim().ToLower();

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
    var choice = Console.ReadLine()?.Trim().ToLower();

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
    var choice = Console.ReadLine()?.Trim().ToLower();

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

Run:

```powershell
dotnet run
```

Walk around. Try every command.

## Tinker

- Add a fourth room (a **basement**? a **garden**?) and connect it to one of the existing rooms.
- Add an item the player can pick up in your new room.
- Add a **second** win condition that needs the new item.
- Make the rooms talk to the player about what's in their `inventory` if they type `look` or `inventory`.

## Name it

- **List.** `List<string>` is C#'s flexible ordered collection. You created an empty one with `new List<string>()`. You added to it with `inventory.Add(...)`. You checked membership with `inventory.Contains(...)`. You can loop through it, sort it, find things in it. The `<string>` part says *"this is a list of strings"* — you'll see `<int>`, `<Building>`, etc. soon.
- **Methods (defined).** You just wrote three methods: `Hallway()`, `Kitchen()`, `Library()`. Each one is a named chunk of code. They *call each other* — `Hallway` calls `Kitchen` if the user goes north, `Kitchen` calls `Hallway` if the user goes back. The whole game is methods calling each other.
- **Organisation.** Splitting code into methods *organises* it. Imagine the same game written as one giant `if` chain — unreadable. With each room as a method, the structure of the game maps to the structure of the code.

## Quiz / challenge

Open `quiz.md`.

## Connect

Your Kingdom is going to have buildings, citizens, events, items — collections of things. `List<>` (and its cousin `Dictionary<>`) is how those collections live in code. And methods are how you'll structure the engine: `Building.Upgrade()`, `Citizen.AssignJob(...)`, `Kingdom.AdvanceTurn()`.
