// Roast-O-Matic — the canonical demo for Module 0.0.8
//
// Replace the roasts in this list with your own. Run it. Watch it pick one
// at random. That's the whole program. We'll add things to it in Module 0.1.

string[] roasts = {
    "Your password is 'password' and we both know it.",
    "Your favorite Roblox game called. It wants its lag back.",
    "I'd insult your code, but you haven't written any yet.",
};

var random = new Random();
var roast = roasts[random.Next(roasts.Length)];
Console.WriteLine(roast);
