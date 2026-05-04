using Xunit;

namespace M6.Tests;

public class RobloxArtefactTests
{
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));

    [Fact]
    public void RobloxKingdomFolder_Exists()
    {
        var path = Path.Combine(RepoRoot, "roblox-kingdom");
        Assert.True(Directory.Exists(path), $"Expected roblox-kingdom/ at {path} containing the Luau engine port.");
    }

    [Fact]
    public void EngineModuleScripts_AllPresent()
    {
        var enginePath = Path.Combine(RepoRoot, "roblox-kingdom", "Engine");
        if (!Directory.Exists(enginePath)) return;

        foreach (var name in new[] { "Building.lua", "Farm.lua", "Lumberyard.lua", "Mine.lua",
                                     "Kingdom.lua", "ResourceLedger.lua", "Citizen.lua" })
        {
            var p = Path.Combine(enginePath, name);
            Assert.True(File.Exists(p), $"Missing engine module: {name}");
        }
    }

    [Fact]
    public void Journal_HasM6WinsEntry()
    {
        var winsPath = Path.Combine(RepoRoot, "journal", "wins.md");
        Assert.True(File.Exists(winsPath), $"Expected journal/wins.md.");
        var content = File.ReadAllText(winsPath);
        Assert.True(content.Length > 100, "wins.md too short for M6.");
        Assert.True(content.Contains("M6") || content.Contains("Phase 5") || content.Contains("Roblox"),
            "wins.md should mention M6 / Phase 5 / Roblox.");
        Assert.Contains("roblox.com/games", content);
    }

    [Fact]
    public void Journal_HasCapstoneFile()
    {
        var capstonePath = Path.Combine(RepoRoot, "journal", "m6-looking-back.md");
        Assert.True(File.Exists(capstonePath), $"Expected journal/m6-looking-back.md filled in.");
        var content = File.ReadAllText(capstonePath);
        Assert.True(content.Length > 500, "m6-looking-back.md exists but is too short. The reflection is the artefact.");
    }
}
