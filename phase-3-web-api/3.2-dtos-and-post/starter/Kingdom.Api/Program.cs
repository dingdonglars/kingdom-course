using Kingdom.Api.Dtos;
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddCitizen(new Citizen("Lyra"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

app.MapGet("/kingdom", () => KingdomJsonStore.ToSummary(kingdom));

app.MapPost("/kingdom/tick", (int? days) =>
{
    var n = Math.Clamp(days ?? 1, 1, 100);
    for (int i = 0; i < n; i++) kingdom.AdvanceDay();

    return Results.Ok(new TickResponse(
        DaysAdvanced: n,
        KingdomName: kingdom.Name,
        CurrentDay:  kingdom.Day,
        Gold:  kingdom.Resources.Get(Resource.Gold),
        Wood:  kingdom.Resources.Get(Resource.Wood),
        Stone: kingdom.Resources.Get(Resource.Stone),
        Food:  kingdom.Resources.Get(Resource.Food)));
});

app.Run();

public partial class Program { }
