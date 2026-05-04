using Kingdom.Console;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SaveSlotUITests
{
    [Fact]
    public void Run_Quit_ExitsImmediately()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ui-{Guid.NewGuid():N}.db");
        try
        {
            using var input = new StringReader("4\n");
            using var output = new StringWriter();
            System.Console.SetIn(input);
            System.Console.SetOut(output);

            SaveSlotUI.Run(new KingdomEfStore(path), new SystemRandom(0), new SystemClock());

            output.ToString().ShouldContain("Quit");
        }
        finally
        {
            ResetConsole();
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Run_NewKingdom_ThenSaveExit_PersistsTheSlot()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ui-{Guid.NewGuid():N}.db");
        try
        {
            using var input = new StringReader("1\nTest\na\ns\n4\n");
            using var output = new StringWriter();
            System.Console.SetIn(input);
            System.Console.SetOut(output);

            var store = new KingdomEfStore(path);
            SaveSlotUI.Run(store, new SystemRandom(0), new SystemClock());

            store.ListSlots().Count.ShouldBe(1);
            store.ListSlots()[0].Name.ShouldBe("Test");
            store.ListSlots()[0].Day.ShouldBe(2);
        }
        finally
        {
            ResetConsole();
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Run_BadMenuPick_ShowsHelp_AndContinues()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ui-{Guid.NewGuid():N}.db");
        try
        {
            using var input = new StringReader("banana\n4\n");
            using var output = new StringWriter();
            System.Console.SetIn(input);
            System.Console.SetOut(output);

            SaveSlotUI.Run(new KingdomEfStore(path), new SystemRandom(0), new SystemClock());

            output.ToString().ShouldContain("Pick 1, 2, 3, or 4");
        }
        finally
        {
            ResetConsole();
            if (File.Exists(path)) File.Delete(path);
        }
    }

    private static void ResetConsole()
    {
        var stdOut = new StreamWriter(System.Console.OpenStandardOutput()) { AutoFlush = true };
        System.Console.SetOut(stdOut);
        System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput()));
    }
}
