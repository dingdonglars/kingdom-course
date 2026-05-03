// Types Demo — Module 0.5 starter

int gold = 100;
double averagePopulation = 12.5;
bool isAtWar = false;
string kingdomName = "Eldoria";
DateTime founded = new DateTime(2024, 1, 15);

Console.WriteLine($"{kingdomName} (founded {founded:yyyy-MM-dd})");
Console.WriteLine($"  gold: {gold}");
Console.WriteLine($"  avg pop: {averagePopulation}");
Console.WriteLine($"  at war: {isAtWar}");

double exactlyHalfGold = gold / 2.0;
int roundedToBank = (int)exactlyHalfGold;
Console.WriteLine($"  half: {exactlyHalfGold}, banked: {roundedToBank}");

string? nicknameUnknown = null;
string? nickname = "the Bold";
Console.WriteLine($"  unknown nickname: {nicknameUnknown ?? "(none)"}");
Console.WriteLine($"  known nickname: {nickname}");