using Shouldly;

namespace Kingdom.Api.Tests;

public class Endpoints_Integration_Tests : IClassFixture<IntegrationFixture>
{
    private readonly HttpClient _client;
    public Endpoints_Integration_Tests(IntegrationFixture f) => _client = f.CreateClient();

    [Fact]
    public async Task OpenApi_Spec_IsServed()
    {
        var resp = await _client.GetAsync("/openapi/v1.json");
        resp.IsSuccessStatusCode.ShouldBeTrue();
        var json = await resp.Content.ReadAsStringAsync();
        json.ShouldContain("\"openapi\":");
        json.ShouldContain("/kingdoms");
    }

    [Fact]
    public async Task UnauthenticatedKingdomsList_Returns401()
    {
        var resp = await _client.GetAsync("/kingdoms");
        ((int)resp.StatusCode).ShouldBe(401);
    }

    [Fact]
    public async Task UnknownPath_Returns404()
    {
        var resp = await _client.GetAsync("/this/does/not/exist");
        ((int)resp.StatusCode).ShouldBe(404);
    }
}
