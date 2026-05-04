namespace Kingdom.Persistence;

public record KingdomSummary(
    string Name,
    int Day,
    int BuildingCount,
    int CitizenCount,
    int Gold,
    int Wood,
    int Stone,
    int Food
);
