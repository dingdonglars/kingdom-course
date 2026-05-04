# SQLite tour — pick a tool

## DB Browser for SQLite (GUI)

- Install: <https://sqlitebrowser.org/>
- File → Open Database → `bin/Debug/net10.0/saves/kingdoms-ef.db`
- "Browse Data" tab → see every row
- "Execute SQL" tab → run any query

## `sqlite3` CLI

- Already on macOS. Windows: <https://sqlite.org/download.html> → `sqlite-tools`
- Open: `sqlite3 saves/kingdoms-ef.db`
- Useful commands:
  - `.tables` — list tables
  - `.schema kingdoms` — show CREATE TABLE
  - `.headers on` + `.mode column` — pretty output
  - `SELECT * FROM kingdoms;` — query
  - `.quit`

## VS Code SQLTools

- Install: `SQLTools` and `SQLTools SQLite` extensions
- Command Palette → "SQLTools: Add new connection" → SQLite → pick the `.db`
- Connections sidebar → right-click table → "Open Table"
- In `.sql` files: Ctrl+E twice to execute

## EF migration script (preview)

```powershell
dotnet ef migrations script --project Kingdom.Persistence --startup-project Kingdom.Console
```

Outputs the SQL the migrations would run. No execution. Safe.

For idempotent SQL (safe to apply multiple times):

```powershell
dotnet ef migrations script -i --project Kingdom.Persistence --startup-project Kingdom.Console
```
