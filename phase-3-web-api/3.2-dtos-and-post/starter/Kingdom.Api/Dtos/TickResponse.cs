namespace Kingdom.Api.Dtos;

public record TickResponse(
    int DaysAdvanced,
    string KingdomName,
    int CurrentDay,
    int Gold, int Wood, int Stone, int Food
);
