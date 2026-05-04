# Quiz answers — B1.1

## 1. b
LocalDB is the developer edition of SQL Server: same engine, no daemon, no admin needed. Each user gets their own instance (`(localdb)\MSSQLLocalDB`). Free; ships with the SDK.

## 2. a
The lesson is a *prediction validation*. We claim "the engine doesn't care which DB." If true, swapping should be trivial. The trivialness IS the lesson — proof that the discipline paid off. Anticlimax = success.

## 3. a
LocalDB instances use the syntax `(localdb)\<InstanceName>`. The default instance is `MSSQLLocalDB`. SQL Server proper uses `localhost`, IP, or hostname; same connection string family.

## 4. a
LocalDB is Windows-only. On macOS/Linux, run the SQL Server image (`mcr.microsoft.com/mssql/server`) in Docker. Same SQL Server engine, different packaging.

## 5. b
`sqllocaldb info` lists the LocalDB instances on this machine. Useful for "is the default instance there?" Pair with `sqllocaldb start MSSQLLocalDB` to ensure it's running.