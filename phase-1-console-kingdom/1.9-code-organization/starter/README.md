# Module 1.9 starter — code organisation (full snapshot)

> **Note:** unlike most modules, this starter is a **full snapshot** of the engine, not a delta. Too many files moved to make a clean delta. To use it, replace your `Kingdom.Engine/` and `Kingdom.Console/` folders entirely with the ones in `starter/`.

Folder structure:

```
Kingdom.Engine/
├─ Kingdom.cs                  ← root (aggregate)
├─ GlobalUsings.cs             ← brings sub-namespaces into scope
├─ Buildings/   {Building, Farm, Lumberyard, Mine}
├─ Citizens/    {Citizen}
├─ Resources/   {Resource, ResourceLedger}
├─ Events/      {KingdomEvent + records, EventEngine}
└─ Infrastructure/  {IRandom, SystemRandom, IClock, SystemClock}
```

After replacing:

- `dotnet build` — 0 errors
- `dotnet run --project Kingdom.Console` — same Eldoria output as 1.8
- `dotnet test` — 35 passing (no behavior change, no new tests)

If you'd rather do the move yourself (recommended for practice), use `starter/` as the *reference* — your IDE's "Move file" + "Update namespace" tooling handles most of the work.