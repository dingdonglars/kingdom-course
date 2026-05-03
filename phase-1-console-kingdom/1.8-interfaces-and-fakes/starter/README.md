# Module 1.8 starter — interfaces, IRandom, IClock, FakeItEasy

Delta from Module 1.7. Files in this folder:

- **NEW:** `Kingdom.Engine/IRandom.cs`, `SystemRandom.cs`
- **NEW:** `Kingdom.Engine/IClock.cs`, `SystemClock.cs`
- **MODIFIED:** `Kingdom.Engine/EventEngine.cs` (takes `IRandom`)
- **MODIFIED:** `Kingdom.Engine/Kingdom.cs` (takes `IRandom + IClock`; convenience no-arg constructor preserved)
- **MODIFIED:** `Kingdom.Console/Program.cs` (constructs `SystemRandom(seed:42)` and `SystemClock`)
- **NEW:** `tests/Kingdom.Engine.Tests/EventEngineTests.cs`
- **MODIFIED:** `tests/Kingdom.Engine.Tests/Kingdom.Engine.Tests.csproj` (adds FakeItEasy package)

Run from the project root:

```powershell
cd tests/Kingdom.Engine.Tests
dotnet add package FakeItEasy
cd ../..

dotnet build
dotnet run --project Kingdom.Console
dotnet run --project Kingdom.Console   # same output (seeded)
dotnet test                            # 35 passing (30 from 1.7 + 5 new)
```