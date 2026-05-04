# Quiz answers — B1.2

## 1. a
Three lines: the NuGet package reference, the `UseSqlServer(...)` call replacing `UseSqlite(...)`, and the connection string format. **Three.** That's the anti-climax.

## 2. a
Migrations are *generated* SQL. AUTOINCREMENT (SQLite) → IDENTITY (SQL Server). Different DECIMAL handling. Different default-value syntax. EF abstracts this when *writing* C# but bakes it into the migration files at generation time.

## 3. a
The lesson is the prediction validation. Engine vs shell predicts trivial swap. Trivial swap = discipline kept its promise. If the swap had been hard, the discipline was a lie all along. The boredom is the trophy.

## 4. b
EF Core supports many provider-specific packages. `UsePostgresql(...)`, `UseMySql(...)`, `UseCosmos(...)`, `UseInMemoryDatabase(...)`. Same three-line pattern works for any of them.

## 5. a
Tests are the contract. If they pass with no logic change, the engine still does the right thing. The DB is genuinely a swappable detail. **Tests pass = the discipline held.**