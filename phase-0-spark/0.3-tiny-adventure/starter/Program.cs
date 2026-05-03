// Tiny Adventure — Module 0.3 starter
//
// 3-room text adventure: hallway, kitchen, library.
// Win condition: take the knife in the kitchen, then read the book in the library.

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
</content>
</invoke>