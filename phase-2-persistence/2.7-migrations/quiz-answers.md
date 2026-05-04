# Quiz answers — Module 2.7

## 1. b
`EnsureCreated` checks "does the database exist? if not, create it from the current model." It doesn't *evolve* an existing schema. Once players have saved data, you can't drop and recreate without losing it. Migrations apply schema changes incrementally without touching existing rows.

## 2. b
The migration files are C# code: an `Up` method that describes the new schema using `migrationBuilder.CreateTable(...)` etc., a `Down` for reverting, and a model snapshot. EF translates them to SQL at apply time. The whole pipeline is in version control, reviewable, repeatable.

## 3. b
EF stores a row per applied migration in `__EFMigrationsHistory`. When `Migrate()` runs, it checks this table to decide which migrations are new — those run; previously-applied ones are skipped. That's how migrations stay idempotent.

## 4. b
They're two different strategies — `EnsureCreated` writes the schema directly (no migration history), `Migrate` writes via the migration pipeline (with history). Mixing them confuses EF: an `EnsureCreated` DB has no migration history, so `Migrate` thinks every migration is new and tries to re-create existing tables. Pick one strategy per project — production = migrations.

## 5. b
Schema drift is when the database's actual schema doesn't match what the code's entity model expects. Causes: manual SQL edits, `EnsureCreated` after model changes, deploys that skip migrations. Migrations + a no-`EnsureCreated` rule eliminate it.