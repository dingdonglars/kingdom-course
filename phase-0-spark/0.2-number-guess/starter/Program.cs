// Number Guess — Module 0.2 starter

var random = new Random();
var secret = random.Next(1, 101);  // 1..100 inclusive
var guesses = 0;

Console.WriteLine("I'm thinking of a number between 1 and 100. Guess.");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    var guess = int.Parse(input);
    guesses++;

    if (guess < secret)
    {
        Console.WriteLine("Too low. And weak.");
    }
    else if (guess > secret)
    {
        Console.WriteLine("Too high. Calm down.");
    }
    else
    {
        Console.WriteLine($"Got it in {guesses}. Bare minimum effort, but I'll allow it.");
        break;
    }
}
