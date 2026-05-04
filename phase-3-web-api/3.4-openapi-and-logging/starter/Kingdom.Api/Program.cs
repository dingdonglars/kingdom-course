using Kingdom.Api.Dtos;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Kingdom.Persistence.EfCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
if (app.Environment.IsDevelopment())
    app.MapScalarApiReference();

var dbPath = Path.Combine(AppContext.BaseDirectory, "kingdoms.db");
var store = new KingdomEfStore(dbPath);
store.EnsureCreated();
IRandom rng = new SystemRandom();
IClock clock = new SystemClock();

var group = app.MapGroup("/kingdoms");

group.MapGet("/", () => store.ListSlots());

group.MapGet("/{id:int}", (int id, ILogger<Program> log) =>
{
    try
    {
        var k = store.Load(id, rng, clock);
        return Results.Ok(KingdomJsonStore.ToSummary(k));
    }
    catch (InvalidOperationException)
    {
        log.LogInformation("Kingdom {KingdomId} not found", id);
        return Results.NotFound(new { error = $"No kingdom with id {id}." });
    }
});

group.MapPost("/", (CreateKingdomRequest req, ILogger<Program> log) =>
{
    if (string.IsNullOrWhiteSpace(req.Name))
    {
        log.LogWarning("CreateKingdom called with empty name");
        return Results.BadRequest(new { error = "Name is required." });
    }

    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(k);
    log.LogInformation("Created kingdom {KingdomId} '{KingdomName}'", id, k.Name);
    return Results.Created($"/kingdoms/{id}", new KingdomCreated(id, k.Name));
});

group.MapPost("/{id:int}/tick", (int id, int? days, ILogger<Program> log) =>
{
    var n = Math.Clamp(days ?? 1, 1, 100);
    Kingdom.Engine.Kingdom k;
    try { k = store.Load(id, rng, clock); }
    catch (InvalidOperationException) { return Results.NotFound(); }

    for (int i = 0; i < n; i++) k.AdvanceDay();
    store.Update(id, k);
    log.LogInformation("Ticked kingdom {KingdomId} by {Days} days (now day {Day})", id, n, k.Day);
    return Results.Ok(KingdomJsonStore.ToSummary(k));
});

group.MapDelete("/{id:int}", (int id, ILogger<Program> log) =>
{
    store.Delete(id);
    log.LogInformation("Deleted kingdom {KingdomId} (or no-op if it didn't exist)", id);
    return Results.NoContent();
});

app.Run();

public partial class Program { }
