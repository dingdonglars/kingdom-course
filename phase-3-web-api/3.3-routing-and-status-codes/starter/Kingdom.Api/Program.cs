using Kingdom.Api.Dtos;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Kingdom.Persistence.EfCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var dbPath = Path.Combine(AppContext.BaseDirectory, "kingdoms.db");
var store = new KingdomEfStore(dbPath);
store.EnsureCreated();
IRandom rng = new SystemRandom();
IClock clock = new SystemClock();

var group = app.MapGroup("/kingdoms");

group.MapGet("/", () => store.ListSlots());

group.MapGet("/{id:int}", (int id) =>
{
    try
    {
        var k = store.Load(id, rng, clock);
        return Results.Ok(KingdomJsonStore.ToSummary(k));
    }
    catch (InvalidOperationException)
    {
        return Results.NotFound(new { error = $"No kingdom with id {id}." });
    }
});

group.MapPost("/", (CreateKingdomRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Name))
        return Results.BadRequest(new { error = "Name is required." });

    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(k);
    return Results.Created($"/kingdoms/{id}", new KingdomCreated(id, k.Name));
});

group.MapPost("/{id:int}/tick", (int id, int? days) =>
{
    var n = Math.Clamp(days ?? 1, 1, 100);
    Kingdom.Engine.Kingdom k;
    try { k = store.Load(id, rng, clock); }
    catch (InvalidOperationException) { return Results.NotFound(); }

    for (int i = 0; i < n; i++) k.AdvanceDay();
    store.Update(id, k);
    return Results.Ok(KingdomJsonStore.ToSummary(k));
});

group.MapDelete("/{id:int}", (int id) =>
{
    store.Delete(id);
    return Results.NoContent();
});

app.Run();

public partial class Program { }
