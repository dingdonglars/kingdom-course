using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SqliteDemoTests
{
    [Fact]
    public void RunDemo_FirstRun_ReturnsOneRow()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kdb-{Guid.NewGuid():N}.db");
        try
        {
            var rows = SqliteDemo.RunDemo(path);
            rows.Count.ShouldBe(1);
            rows[0].Name.ShouldBe("Eldoria");
            rows[0].Day.ShouldBe(11);
            rows[0].Gold.ShouldBe(250);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void RunDemo_TwoRuns_AccumulatesRows()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kdb-{Guid.NewGuid():N}.db");
        try
        {
            SqliteDemo.RunDemo(path);
            var rows = SqliteDemo.RunDemo(path);
            rows.Count.ShouldBe(2);
            rows[0].Id.ShouldBe(1);
            rows[1].Id.ShouldBe(2);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void DatabaseFile_IsCreated_OnFirstRun()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kdb-{Guid.NewGuid():N}.db");
        try
        {
            File.Exists(path).ShouldBeFalse();
            SqliteDemo.RunDemo(path);
            File.Exists(path).ShouldBeTrue();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
