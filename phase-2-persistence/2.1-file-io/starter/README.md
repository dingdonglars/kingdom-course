# Module 2.1 starter — File I/O

Delta from Module 1.10. Files in this folder:

- **MODIFIED:** `Kingdom.Console/Program.cs` — saves a tiny human-readable snapshot to `saves/kingdom.txt`, then reads it back
- **NEW:** `tests/Kingdom.Engine.Tests/FileIOTests.cs` — three roundtrip / safety tests

Engine code unchanged. The whole point: the engine doesn't know about disk.

After applying:

- `dotnet build` — 0 errors
- `dotnet run --project Kingdom.Console` — output ends with the file contents printed back; check `bin/Debug/net10.0/saves/kingdom.txt` exists
- `dotnet test` — 38 passing (35 from 1.9 + 3 new)