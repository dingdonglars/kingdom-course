using System.Diagnostics;
using Xunit;

namespace M4.Tests;

public class ApiChallengeTests
{
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    private static readonly string ApiProject = Path.Combine(RepoRoot, "Kingdom.Api", "Kingdom.Api.csproj");

    [Fact]
    public void ApiProject_Exists()
    {
        Assert.True(File.Exists(ApiProject), $"Expected Kingdom.Api.csproj at {ApiProject}.");
    }

    [Fact]
    public void ApiProject_Builds()
    {
        if (!File.Exists(ApiProject)) return;
        var psi = new ProcessStartInfo("dotnet", $"build \"{ApiProject}\" -c Debug --nologo --verbosity minimal")
        {
            RedirectStandardOutput = true, RedirectStandardError = true,
            UseShellExecute = false, CreateNoWindow = true
        };
        using var p = Process.Start(psi)!;
        p.WaitForExit(120_000);
        var output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
        Assert.True(p.ExitCode == 0, $"`dotnet build` for Kingdom.Api failed:\n{output}");
    }

    [Fact]
    public void Api_Has_OpenApi_Wired()
    {
        if (!File.Exists(ApiProject)) return;
        var programCs = Path.Combine(Path.GetDirectoryName(ApiProject)!, "Program.cs");
        if (!File.Exists(programCs)) Assert.Fail($"Expected {programCs}");
        var content = File.ReadAllText(programCs);
        Assert.True(content.Contains("AddOpenApi") || content.Contains("AddSwaggerGen"),
            "Expected OpenAPI/Swagger registration in Program.cs.");
    }

    [Fact]
    public void Api_Has_RequireAuthorization_Or_Authorize()
    {
        if (!File.Exists(ApiProject)) return;
        var programCs = Path.Combine(Path.GetDirectoryName(ApiProject)!, "Program.cs");
        var content = File.ReadAllText(programCs);
        Assert.True(content.Contains("RequireAuthorization") || content.Contains("[Authorize"),
            "Expected at least one endpoint to be auth-required.");
    }

    [Fact]
    public void GitHub_Workflow_Exists()
    {
        var workflowDir = Path.Combine(RepoRoot, ".github", "workflows");
        Assert.True(Directory.Exists(workflowDir),
            $"Expected .github/workflows/ at {workflowDir} (CI/CD setup).");
        var ymls = Directory.GetFiles(workflowDir, "*.yml");
        Assert.NotEmpty(ymls);
    }

    [Fact]
    public void Journal_HasM4WinsEntry()
    {
        var winsPath = Path.Combine(RepoRoot, "journal", "wins.md");
        Assert.True(File.Exists(winsPath), $"Expected journal/wins.md.");
        var content = File.ReadAllText(winsPath);
        Assert.True(content.Length > 100, "wins.md too short for M4.");
        Assert.True(content.Contains("M4") || content.Contains("Block 5") || content.Contains("Live API"),
            "wins.md should mention M4 / Block 5 / Live API.");
    }
}
