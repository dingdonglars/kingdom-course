using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace M3.Tests;

public class PersistenceChallengeTests
{
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    private static readonly string PersistenceProject = Path.Combine(RepoRoot, "Kingdom.Persistence", "Kingdom.Persistence.csproj");
    private static readonly string EngineProject = Path.Combine(RepoRoot, "Kingdom.Engine", "Kingdom.Engine.csproj");

    [Fact]
    public void PersistenceProject_Exists()
    {
        Assert.True(File.Exists(PersistenceProject), $"Expected Kingdom.Persistence.csproj at {PersistenceProject}.");
    }

    [Fact]
    public void PersistenceProject_Builds()
    {
        if (!File.Exists(PersistenceProject)) return;
        var psi = new ProcessStartInfo("dotnet", $"build \"{PersistenceProject}\" -c Debug --nologo --verbosity minimal")
        {
            RedirectStandardOutput = true, RedirectStandardError = true,
            UseShellExecute = false, CreateNoWindow = true
        };
        using var p = Process.Start(psi)!;
        p.WaitForExit(120_000);
        var output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
        Assert.True(p.ExitCode == 0, $"`dotnet build` for Kingdom.Persistence failed:\n{output}");
    }

    private Assembly LoadPersistenceAssembly()
    {
        var dir = Path.GetDirectoryName(PersistenceProject)!;
        var dll = Directory
            .GetFiles(Path.Combine(dir, "bin"), "Kingdom.Persistence.dll", SearchOption.AllDirectories)
            .OrderByDescending(File.GetLastWriteTimeUtc).First();
        return Assembly.LoadFrom(dll);
    }

    [Fact]
    public void Persistence_HasStoreClass_WithSaveAndLoad()
    {
        if (!File.Exists(PersistenceProject)) return;
        var asm = LoadPersistenceAssembly();
        var stores = asm.GetTypes().Where(t => t.Name.EndsWith("Store") && !t.IsAbstract).ToList();
        Assert.NotEmpty(stores);
        var store = stores.First(t => t.GetMethods().Any(m => m.Name == "Save")
                                      && t.GetMethods().Any(m => m.Name == "Load"));
        Assert.NotNull(store);
    }

    [Fact]
    public void Journal_HasM3WinsEntry()
    {
        var winsPath = Path.Combine(RepoRoot, "journal", "wins.md");
        Assert.True(File.Exists(winsPath), $"Expected journal/wins.md.");
        var content = File.ReadAllText(winsPath);
        Assert.True(content.Length > 100, "wins.md exists but is too short. Write at least one paragraph for M3.");
        Assert.True(content.Contains("M3") || content.Contains("Phase 2") || content.Contains("Persistence"),
            "wins.md should mention M3 / Phase 2 / Persistence.");
    }
}
