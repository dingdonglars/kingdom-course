# Quiz answers — Module 2.4

## 1. c
SQLite is a *library* — you reference it from your code and the database is a single `.db` file on disk. No service to install, no port to open, no separate server process. Massively deployed (phones, browsers, OSes); also great for learning and small apps.

## 2. b
SQL injection: if you concatenate `userInput` into your SQL string, an input like `'); DROP TABLE kingdoms; --` runs as code. Parameters (`$name`, `@name`, etc.) are sent separately from the query text — the database driver quotes/escapes them safely. **Always parameters. No exceptions.**

## 3. c
`SELECT` reads rows. `INSERT` / `UPDATE` / `DELETE` modify rows but don't return them (use `ExecuteNonQuery`). `CREATE` defines structure. `SELECT` uses `ExecuteReader`; counts/single values use `ExecuteScalar`.

## 4. b
The `using` keyword guarantees `Dispose` runs at the end of the scope — even if an exception is thrown midway. Without it, you can leak file handles, which on Windows means you can't delete the database file from another process. **Connections, commands, and readers all need `using`.**

## 5. b
Same kingdom, third storage runtime — first plain text (M2.1), then JSON (M2.2/M2.3), now SQLite. The engine has not changed. That's the engine-vs-shell discipline keeping its promise.