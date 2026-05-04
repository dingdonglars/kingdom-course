namespace Kingdom.Engine.Snapshots;

public record KingdomSnapshot(
    string Name,
    int Day,
    int Gold, int Wood, int Stone, int Food,
    BuildingSnapshot[] Buildings,
    CitizenSnapshot[] Citizens
);

public record BuildingSnapshot(string Kind, string Name, int Level);

public record CitizenSnapshot(string Name);
