using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;

namespace Kingdom.Console;

public static class SaveSlotUI
{
    public static void Run(KingdomEfStore store, IRandom rng, IClock clock)
    {
        store.EnsureCreated();

        while (true)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("=== Kingdom — Save Slots ===");
            System.Console.WriteLine("  1. New kingdom");
            System.Console.WriteLine("  2. Load existing");
            System.Console.WriteLine("  3. Delete a slot");
            System.Console.WriteLine("  4. Quit");
            System.Console.Write("> ");

            var line = System.Console.ReadLine();
            if (line is null) return;

            switch (line.Trim())
            {
                case "1":  NewKingdom(store, rng, clock); break;
                case "2":  LoadKingdom(store, rng, clock); break;
                case "3":  DeleteSlot(store); break;
                case "4":  System.Console.WriteLine("Quit."); return;
                default:   System.Console.WriteLine("Pick 1, 2, 3, or 4."); break;
            }
        }
    }

    private static void NewKingdom(KingdomEfStore store, IRandom rng, IClock clock)
    {
        System.Console.Write("Name: ");
        var name = (System.Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrEmpty(name)) { System.Console.WriteLine("Cancelled."); return; }

        var k = new global::Kingdom.Engine.Kingdom(name, rng, clock);
        k.AddBuilding(new Farm("Main Farm"));
        k.AddCitizen(new Citizen("Lyra"));
        var id = store.Save(k);
        System.Console.WriteLine($"Created '{name}' as slot #{id}.");
        PlayLoop(store, id, k);
    }

    private static void LoadKingdom(KingdomEfStore store, IRandom rng, IClock clock)
    {
        var slots = store.ListSlots();
        if (slots.Count == 0) { System.Console.WriteLine("No saves yet."); return; }
        ShowSlots(slots);

        System.Console.Write("Slot id (or blank to cancel): ");
        var raw = (System.Console.ReadLine() ?? "").Trim();
        if (!int.TryParse(raw, out var id)) { System.Console.WriteLine("Not a number."); return; }
        if (slots.All(s => s.Id != id)) { System.Console.WriteLine($"No slot with id {id}."); return; }

        var k = store.Load(id, rng, clock);
        System.Console.WriteLine($"Loaded #{id} '{k.Name}' at day {k.Day}.");
        PlayLoop(store, id, k);
    }

    private static void DeleteSlot(KingdomEfStore store)
    {
        var slots = store.ListSlots();
        if (slots.Count == 0) { System.Console.WriteLine("No saves to delete."); return; }
        ShowSlots(slots);

        System.Console.Write("Slot id to delete: ");
        var raw = (System.Console.ReadLine() ?? "").Trim();
        if (!int.TryParse(raw, out var id)) { System.Console.WriteLine("Not a number."); return; }
        store.Delete(id);
        System.Console.WriteLine($"Deleted #{id}.");
    }

    private static void PlayLoop(KingdomEfStore store, int id, Kingdom.Engine.Kingdom k)
    {
        while (true)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"--- {k.Name} day {k.Day} ---");
            System.Console.WriteLine("  a. Advance 1 day   d. Advance 10 days   s. Save & exit slot");
            System.Console.Write("> ");
            var c = (System.Console.ReadLine() ?? "").Trim();
            switch (c)
            {
                case "a": k.AdvanceDay(); break;
                case "d": for (int i = 0; i < 10; i++) k.AdvanceDay(); break;
                case "s": store.Update(id, k); System.Console.WriteLine($"Saved #{id} at day {k.Day}."); return;
                default:  System.Console.WriteLine("Pick a, d, or s."); break;
            }
        }
    }

    private static void ShowSlots(IReadOnlyList<KingdomSlotInfo> slots)
    {
        System.Console.WriteLine("Saved kingdoms:");
        foreach (var s in slots)
            System.Console.WriteLine($"  #{s.Id,-3} {s.Name,-20} day {s.Day}");
    }
}
