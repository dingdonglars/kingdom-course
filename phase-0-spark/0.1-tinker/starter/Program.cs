// Roast-O-Matic v2 — Module 0.1 starter
//
// Asks for a name, then prints a randomly-picked roast that uses the name.

Console.Write("Who do you want to roast? ");
var name = Console.ReadLine();

string[] roasts = {
    "Your password is 'password' and we both know it.",
    "Your favorite Roblox game called. It wants its lag back.",
    "I'd insult your code, but you haven't written any yet.",
};

var random = new Random();
var roast = roasts[random.Next(roasts.Length)];

Console.WriteLine($"Hey {name?.ToUpper()} — {roast}");
