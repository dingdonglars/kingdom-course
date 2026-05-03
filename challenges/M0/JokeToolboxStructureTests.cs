using System.Diagnostics;
using Xunit;

namespace M0.Tests;

public class JokeToolboxStructureTests
{
    // Tests assume they are run from the learner's repo root.
    // The challenge file's relative path: challenges/M0/...
    // Going up two levels gets us to the learner's repo root.
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));

    [Theory]
    [InlineData("RoastOMatic")]
    [InlineData("NumberGuess")]
    [InlineData("TinyAdventure")]
    [InlineData("Polish")]
    public void ToyFolder_Exists(string toyName)
    {
        var path = Path.Combine(RepoRoot, toyName);
        Assert.True(Directory.Exists(path), $"Expected toy folder at {path} but it does not exist. Did you create the {toyName} project?");
    }

    [Theory]
    [InlineData("RoastOMatic")]
    [InlineData("NumberGuess")]
    [InlineData("TinyAdventure")]
    [InlineData("Polish")]
    public void ToyHasCsproj(string toyName)
    {
        var folder = Path.Combine(RepoRoot, toyName);
        if (!Directory.Exists(folder)) return;  // Covered by ToyFolder_Exists
        var csprojs = Directory.GetFiles(folder, "*.csproj");
        Assert.NotEmpty(csprojs);
    }

    [Theory]
    [InlineData("RoastOMatic")]
    [InlineData("NumberGuess")]
    [InlineData("TinyAdventure")]
    [InlineData("Polish")]
    public void ToyBuilds(string toyName)
    {
        var folder = Path.Combine(RepoRoot, toyName);
        if (!Directory.Exists(folder)) return;

        var psi = new ProcessStartInfo("dotnet", $"build \"{folder}\" -c Debug --nologo --verbosity minimal")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi)!;
        process.WaitForExit(60_000);  // 60s budget
        var output = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
        Assert.True(process.ExitCode == 0, $"`dotnet build` for {toyName} failed:\n{output}");
    }

    [Fact]
    public void RepoRoot_HasReadme()
    {
        var readmePath = Path.Combine(RepoRoot, "README.md");
        Assert.True(File.Exists(readmePath), $"Expected your M0 README at {readmePath} but it does not exist.");
    }

    [Fact]
    public void Journal_HasWinsEntry()
    {
        var winsPath = Path.Combine(RepoRoot, "journal", "wins.md");
        Assert.True(File.Exists(winsPath), $"Expected journal/wins.md (the milestone ritual). Have you written your M0 wins paragraph?");
        var content = File.ReadAllText(winsPath);
        Assert.True(content.Length > 100, "wins.md exists but is empty or too short. Write at least one paragraph about M0.");
    }
}
