using System.Security.Claims;
using Kingdom.Api.Dtos;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Kingdom.Persistence.EfCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // JSON API — return 401/403 instead of redirecting to a login page.
        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
if (app.Environment.IsDevelopment())
    app.MapScalarApiReference();

var dbPath = Path.Combine(AppContext.BaseDirectory, "kingdoms.db");
var store = new KingdomEfStore(dbPath);
store.EnsureCreated();

// SystemClock collision: Microsoft.AspNetCore.Authentication ships its own
// SystemClock, so the engine one must be fully qualified here.
IRandom rng = new SystemRandom();
IClock clock = new Kingdom.Engine.Infrastructure.SystemClock();

// Auth endpoints
app.MapGet("/login", () => Results.Challenge(
    new AuthenticationProperties { RedirectUri = "/" },
    [GoogleDefaults.AuthenticationScheme]));

app.MapPost("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok();
});

app.MapGet("/me", (HttpContext ctx) =>
{
    if (ctx.User.Identity?.IsAuthenticated != true)
        return Results.Unauthorized();
    return Results.Ok(new
    {
        Email = ctx.User.FindFirst("email")?.Value ?? ctx.User.FindFirst(ClaimTypes.Email)?.Value,
        Name  = ctx.User.FindFirst("name")?.Value  ?? ctx.User.FindFirst(ClaimTypes.Name)?.Value,
        Sub   = ctx.User.FindFirst("sub")?.Value   ?? ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
    });
});

// Kingdom endpoints — auth required, scoped to the signed-in user (Module 3.6 adds OwnerSub)
var group = app.MapGroup("/kingdoms").RequireAuthorization();

group.MapGet("/", () => store.ListSlots());

group.MapGet("/{id:int}", (int id) =>
{
    try { return Results.Ok(KingdomJsonStore.ToSummary(store.Load(id, rng, clock))); }
    catch (InvalidOperationException) { return Results.NotFound(new { error = $"No kingdom with id {id}." }); }
});

group.MapPost("/", (CreateKingdomRequest req, ILogger<Program> log) =>
{
    if (string.IsNullOrWhiteSpace(req.Name))
        return Results.BadRequest(new { error = "Name is required." });
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
    log.LogInformation("Ticked kingdom {KingdomId} by {Days} days", id, n);
    return Results.Ok(KingdomJsonStore.ToSummary(k));
});

group.MapDelete("/{id:int}", (int id) =>
{
    store.Delete(id);
    return Results.NoContent();
});

app.Run();

public partial class Program { }
