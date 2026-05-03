// Methods Demo — Module 0.6 starter

// A method with NO parameters and NO return value
Helpers.SayHello();

// A method with ONE parameter and NO return value
Helpers.Greet("Eldoria");

// A method with TWO parameters and a RETURN value (int)
int total = Helpers.AddGold(120, 35);
Console.WriteLine($"Total gold: {total}");

// A method with overloading: same name, different parameters
Console.WriteLine(Helpers.Square(5));        // int version
Console.WriteLine(Helpers.Square(2.5));      // double version

// --- Method definitions live on a static class.
// Static classes can hold overloaded methods (top-level local functions can't).

static class Helpers
{
    public static void SayHello()
    {
        Console.WriteLine("Hello from Eldoria.");
    }

    public static void Greet(string kingdom)
    {
        Console.WriteLine($"Greetings, {kingdom}.");
    }

    public static int AddGold(int a, int b)
    {
        return a + b;
    }

    public static int Square(int x) => x * x;
    public static double Square(double x) => x * x;
}