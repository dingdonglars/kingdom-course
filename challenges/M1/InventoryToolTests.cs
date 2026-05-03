using System.Diagnostics;
using Xunit;

namespace M1.Tests;

public class InventoryToolTests
{
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    private static readonly string ToolFolder = Path.Combine(RepoRoot, "InventoryTool");

    [Fact]
    public void InventoryToolFolder_Exists()
    {
        Assert.True(Directory.Exists(ToolFolder), $"Expected InventoryTool folder at {ToolFolder} but it does not exist.");
    }

    [Fact]
    public void InventoryTool_HasCsproj()
    {
        if (!Directory.Exists(ToolFolder)) return;
        var csprojs = Directory.GetFiles(ToolFolder, "*.csproj");
        Assert.NotEmpty(csprojs);
    }

    [Fact]
    public void InventoryTool_Builds()
    {
        if (!Directory.Exists(ToolFolder)) return;

        var psi = new ProcessStartInfo("dotnet", $"build \"{ToolFolder}\" -c Debug --nologo --verbosity minimal")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var process = Process.Start(psi)!;
        process.WaitForExit(60_000);
        var output = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
        Assert.True(process.ExitCode == 0, $"`dotnet build` for InventoryTool failed:\n{output}");
    }

    [Fact]
    public void InventoryTool_RespondsToBasicCommands()
    {
        if (!Directory.Exists(ToolFolder)) return;

        var psi = new ProcessStartInfo("dotnet", $"run --project \"{ToolFolder}\" -c Debug --nologo --verbosity quiet")
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var process = Process.Start(psi)!;
        process.StandardInput.WriteLine("add apple");
        process.StandardInput.WriteLine("add banana");
        process.StandardInput.WriteLine("list");
        process.StandardInput.WriteLine("find apple");
        process.StandardInput.WriteLine("quit");
        process.StandardInput.Close();

        var stdout = process.StandardOutput.ReadToEnd();
        process.WaitForExit(60_000);

        Assert.Contains("apple", stdout, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("banana", stdout, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("found", stdout, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RepoRoot_HasReadme()
    {
        var readmePath = Path.Combine(RepoRoot, "README.md");
        Assert.True(File.Exists(readmePath), $"Expected your README at {readmePath}.");
    }

    [Fact]
    public void Journal_HasWinsEntry()
    {
        var winsPath = Path.Combine(RepoRoot, "journal", "wins.md");
        Assert.True(File.Exists(winsPath), $"Expected journal/wins.md.");
        var content = File.ReadAllText(winsPath);
        Assert.True(content.Length > 100, "wins.md exists but is empty or too short. Write at least one paragraph about M1.");
    }
}