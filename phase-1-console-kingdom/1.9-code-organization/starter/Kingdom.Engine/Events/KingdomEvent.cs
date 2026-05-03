namespace Kingdom.Engine.Events;

public abstract record KingdomEvent(int Day, string Description);

public record TraderArrived(int Day, int GoldAmount)
    : KingdomEvent(Day, $"A trader arrived with {GoldAmount} gold.");

public record CitizenIll(int Day, string CitizenName)
    : KingdomEvent(Day, $"{CitizenName} fell ill.");

public record BuildingBurned(int Day, string BuildingName)
    : KingdomEvent(Day, $"{BuildingName} burned to the ground.");
