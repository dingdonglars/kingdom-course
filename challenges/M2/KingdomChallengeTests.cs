using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace M2.Tests;

public class KingdomChallengeTests
{
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    private static readonly string EngineProject = Path.Combine(RepoRoot, "Kingdom.Engine", "Kingdom.Engine.csproj");

    [Fact]
    public void EngineProject_Exists()
    {
        Assert.True(File.Exists(EngineProject), $"Expected Kingdom.Engine.csproj at {EngineProject}.");
    }

    [Fact]
    public void EngineProject_Builds()
    {
        if (!File.Exists(EngineProject)) return;
        var psi = new ProcessStartInfo("dotnet", $"build \"{EngineProject}\" -c Debug --nologo --verbosity minimal")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var p = Process.Start(psi)!;
        p.WaitForExit(120_000);
        var output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
        Assert.True(p.ExitCode == 0, $"`dotnet build` for Kingdom.Engine failed:\n{output}");
    }

    [Fact]
    public void Engine_DoesNotReferenceConsole_InSourceFiles()
    {
        if (!File.Exists(EngineProject)) return;
        var engineDir = Path.GetDirectoryName(EngineProject)!;
        var csFiles = Directory.GetFiles(engineDir, "*.cs", SearchOption.AllDirectories);
        var bad = csFiles.Where(f => File.ReadAllText(f).Contains("Console.")).ToList();
        Assert.True(bad.Count == 0,
            $"Engine should not reference Console. Found Console.* in:\n  - " + string.Join("\n  - ", bad));
    }

    private Assembly LoadEngineAssembly()
    {
        var engineDir = Path.GetDirectoryName(EngineProject)!;
        var dll = Directory
            .GetFiles(Path.Combine(engineDir, "bin"), "Kingdom.Engine.dll", SearchOption.AllDirectories)
            .OrderByDescending(File.GetLastWriteTimeUtc)
            .First();
        return Assembly.LoadFrom(dll);
    }

    [Fact]
    public void RequiredTypes_Exist()
    {
        if (!File.Exists(EngineProject)) return;
        var asm = LoadEngineAssembly();
        var typeNames = asm.GetTypes().Select(t => t.FullName ?? "").ToHashSet();

        Assert.Contains(typeNames, n => n.EndsWith(".Kingdom"));
        Assert.Contains(typeNames, n => n.EndsWith(".Building") || n.EndsWith(".Buildings.Building"));
        Assert.Contains(typeNames, n => n.EndsWith(".Farm") || n.EndsWith(".Buildings.Farm"));
        Assert.Contains(typeNames, n => n.EndsWith(".IRandom") || n.EndsWith(".Infrastructure.IRandom"));
    }

    [Fact]
    public void Kingdom_HasEventLogProperty()
    {
        if (!File.Exists(EngineProject)) return;
        var asm = LoadEngineAssembly();
        var kingdomType = asm.GetTypes().Single(t => t.Name == "Kingdom" && (t.Namespace?.EndsWith(".Engine") ?? false));
        var prop = kingdomType.GetProperty("EventLog");
        Assert.NotNull(prop);
    }

    [Fact]
    public void Kingdom_With50Days_AccumulatesEvents()
    {
        if (!File.Exists(EngineProject)) return;
        var asm = LoadEngineAssembly();
        var kingdomType = asm.GetTypes().Single(t => t.Name == "Kingdom" && (t.Namespace?.EndsWith(".Engine") ?? false));
        var instance = Activator.CreateInstance(kingdomType, new object[] { "ChallengeTest" });
        Assert.NotNull(instance);
        var advance = kingdomType.GetMethod("AdvanceDay");
        Assert.NotNull(advance);
        for (int i = 0; i < 50; i++) advance!.Invoke(instance, null);

        var log = (System.Collections.IEnumerable)kingdomType.GetProperty("EventLog")!.GetValue(instance)!;
        var count = log.Cast<object>().Count();
        Assert.True(count > 0, "After 50 days the EventLog should have at least one entry.");
    }

    [Fact]
    public void Journal_HasM2WinsEntry()
    {
        var winsPath = Path.Combine(RepoRoot, "journal", "wins.md");
        Assert.True(File.Exists(winsPath), $"Expected journal/wins.md.");
        var content = File.ReadAllText(winsPath);
        Assert.True(content.Length > 100, "wins.md exists but is too short. Write at least one paragraph for M2.");
        Assert.True(content.Contains("M2") || content.Contains("Phase 1") || content.Contains("Console Kingdom"),
            "wins.md should mention M2 / Phase 1 / Console Kingdom.");
    }
}
