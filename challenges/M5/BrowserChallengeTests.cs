using System.Diagnostics;
using System.Text.Json;
using Xunit;

namespace M5.Tests;

public class BrowserChallengeTests
{
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));

    private static string FrontendDir
    {
        get
        {
            var viteDir = Path.Combine(RepoRoot, "web-vite");
            if (Directory.Exists(viteDir)) return viteDir;
            var webDir = Path.Combine(RepoRoot, "web");
            return webDir;
        }
    }

    [Fact]
    public void FrontendProject_Exists()
    {
        Assert.True(Directory.Exists(FrontendDir), $"Expected web-vite/ or web/ at repo root. Tried {FrontendDir}.");
    }

    [Fact]
    public void Frontend_HasPackageJson_WithViteAndVitest()
    {
        var pkgPath = Path.Combine(FrontendDir, "package.json");
        if (!File.Exists(pkgPath)) return;

        using var doc = JsonDocument.Parse(File.ReadAllText(pkgPath));
        var root = doc.RootElement;
        var hasVite = HasDep(root, "vite");
        var hasVitest = HasDep(root, "vitest");
        Assert.True(hasVite, "Expected vite in dependencies/devDependencies.");
        Assert.True(hasVitest, "Expected vitest in dependencies/devDependencies.");
    }

    [Fact]
    public void Frontend_HasAtLeastOneTestFile()
    {
        if (!Directory.Exists(FrontendDir)) return;
        var srcDir = Path.Combine(FrontendDir, "src");
        if (!Directory.Exists(srcDir)) return;
        var tests = Directory.GetFiles(srcDir, "*.test.ts", SearchOption.AllDirectories);
        Assert.NotEmpty(tests);
    }

    [Fact]
    public void StaticWebApps_Workflow_Exists()
    {
        var workflowDir = Path.Combine(RepoRoot, ".github", "workflows");
        if (!Directory.Exists(workflowDir)) Assert.Fail($"No .github/workflows/ at {workflowDir}");
        var swa = Directory.GetFiles(workflowDir, "azure-static-web-apps-*.yml");
        Assert.NotEmpty(swa);
    }

    [Fact]
    public void Journal_HasM5WinsEntry()
    {
        var winsPath = Path.Combine(RepoRoot, "journal", "wins.md");
        Assert.True(File.Exists(winsPath), $"Expected journal/wins.md.");
        var content = File.ReadAllText(winsPath);
        Assert.True(content.Length > 100, "wins.md too short for M5.");
        Assert.True(content.Contains("M5") || content.Contains("Block 6") || content.Contains("Browser"),
            "wins.md should mention M5 / Block 6 / Browser.");
    }

    private static bool HasDep(JsonElement root, string name)
    {
        foreach (var section in new[] { "dependencies", "devDependencies" })
        {
            if (root.TryGetProperty(section, out var deps) && deps.TryGetProperty(name, out _))
                return true;
        }
        return false;
    }
}
