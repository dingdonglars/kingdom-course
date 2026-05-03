# Module 1.1 — OOP Basics

> **Hook:** today the Kingdom begins. You'll create your first *classes* — `Building`, `Resource`, `Citizen`, `Kingdom` — and instantiate a tiny medieval realm in memory.

> **Words to watch**
> - **class** — a blueprint for creating objects
> - **object** (or *instance*) — a thing created from a class with `new`
> - **property** — a named value on an object (with `get` and optional `set`)
> - **constructor** — the special method that runs when an object is created (`new Building(...)`)
> - **encapsulation** — hiding internal data behind methods and properties
> - **enum** — a named set of allowed values (e.g., `Resource.Gold`, `Resource.Wood`)
> - **`new`** — the keyword that calls a constructor and gives you a fresh object

---

## Do it — your first Kingdom

Create a new project for the Kingdom:

```powershell
cd <your-repo-root>
dotnet new console -n KingdomConsole
cd KingdomConsole
```

You'll create five files. Each holds one class (or one enum). Filenames match types — that's the convention.

### `Resource.cs` — the resource enum

```csharp
namespace KingdomConsole;

public enum Resource
{
    Gold,
    Wood,
    Stone,
    Food
}
```

### `Building.cs`

```csharp
namespace KingdomConsole;

public class Building
{
    public string Name { get; }
    public int Level { get; private set; } = 1;

    public Building(string name)
    {
        Name = name;
    }

    public void Upgrade()
    {
        Level++;
    }
}
```

### `Citizen.cs`

```csharp
namespace KingdomConsole;

public class Citizen
{
    public string Name { get; }
    public string Job { get; set; } = "Idle";

    public Citizen(string name)
    {
        Name = name;
    }
}
```

### `Kingdom.cs`

```csharp
namespace KingdomConsole;

public class Kingdom
{
    public string Name { get; }
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public Dictionary<Resource, int> Resources { get; } = new();

    public Kingdom(string name)
    {
        Name = name;
        Resources[Resource.Gold] = 100;
        Resources[Resource.Wood] = 50;
        Resources[Resource.Stone] = 20;
        Resources[Resource.Food] = 30;
    }

    public void AddBuilding(Building b) => Buildings.Add(b);
    public void AddCitizen(Citizen c) => Citizens.Add(c);
}
```

### `Program.cs` — the entry point

```csharp
using KingdomConsole;

var kingdom = new Kingdom("Eldoria");
kingdom.AddBuilding(new Building("Main Farm"));
kingdom.AddBuilding(new Building("Old Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));

Console.WriteLine($"== {kingdom.Name} ==");
Console.WriteLine($"Buildings ({kingdom.Buildings.Count}):");
foreach (var b in kingdom.Buildings)
    Console.WriteLine($"  - {b.Name} (level {b.Level})");

Console.WriteLine($"Citizens ({kingdom.Citizens.Count}):");
foreach (var c in kingdom.Citizens)
    Console.WriteLine($"  - {c.Name}: {c.Job}");

Console.WriteLine("Resources:");
foreach (var (resource, count) in kingdom.Resources)
    Console.WriteLine($"  {resource}: {count}");
```

Run:

```powershell
dotnet run
```

You should see your kingdom printed. **Eldoria has buildings, citizens, and a treasury — entirely in memory.**

## Tinker

- Add a third building to `Program.cs`. Run again.
- Call `kingdom.Buildings[0].Upgrade()` before printing. The level should go up.
- Try `kingdom.Buildings[0].Name = "New Name"`. **It won't compile** — the property has no setter. *Why is that good?*
- Add a `void HireCitizen(string name)` method on `Kingdom` that creates a new `Citizen` and adds it. Use it from `Program.cs`.

## Name it

- **Class.** A blueprint. `Building` is the class; `new Building("Farm")` makes an instance.
- **Property.** A named value with a `get` and optionally a `set`. `Building.Name` has only a `get` (read-only); `Building.Level` has `get` + `private set` (read-only from outside, writable from inside the class).
- **Constructor.** Special method named for the class. `new Building("Farm")` calls `public Building(string name)`. Constructors initialise the object.
- **Encapsulation.** The class controls what the outside world can do with it. `Level++` only happens via `Upgrade()` — you can't accidentally set it to 999 from outside. *That's the value.*
- **Enum.** A named set of fixed values. `Resource.Gold` is more meaningful than `0`, and the compiler stops you from passing a bogus value.
- **Namespace.** `namespace KingdomConsole;` puts everything in this file under that name. Other files in the same namespace see each other automatically. (Module 1.9 goes deep on namespaces.)

## Quiz / challenge

Open `quiz.md`.

## Connect

Right now everything's in one project (`KingdomConsole`). That's fine for one lesson. Module 1.2 splits it: the Kingdom *engine* (the data + logic — Building, Citizen, Resource, Kingdom) moves to its own *class library*, and the *console* (Program.cs and the printing) becomes a thin shell on top. **That refactor is the lesson the rest of the course is named after.**