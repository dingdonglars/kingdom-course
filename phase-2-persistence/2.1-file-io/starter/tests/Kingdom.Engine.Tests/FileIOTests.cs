using Shouldly;

namespace Kingdom.Engine.Tests;

public class FileIOTests
{
    [Fact]
    public void WriteAllText_ThenReadAllText_RoundtripsExactly()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kingdom-test-{Guid.NewGuid():N}.txt");
        var original = "Line one\nLine two\nLine three";
        try
        {
            File.WriteAllText(path, original);
            var roundtripped = File.ReadAllText(path);
            roundtripped.ShouldBe(original);
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Path_Combine_HandlesTrailingSeparators()
    {
        Path.Combine("a", "b").ShouldBe("a" + Path.DirectorySeparatorChar + "b");
        Path.Combine("a/", "b").ShouldBe("a/b");
    }

    [Fact]
    public void Directory_CreateDirectory_IsIdempotent()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"kingdom-test-{Guid.NewGuid():N}");
        try
        {
            Directory.CreateDirectory(dir);
            Directory.CreateDirectory(dir);
            Directory.Exists(dir).ShouldBeTrue();
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir);
        }
    }
}
