# Quiz answers — Module 2.8

## 1. b
EF tells you what your code thinks is true. The GUI tells you what the database actually contains. When the two disagree (and they will), seeing the row directly is the fastest way to find the bug.

## 2. b
`.schema TABLE_NAME` shows the CREATE TABLE statement — column names, types, constraints, default values. Pair with `.tables` (list all tables) and `.indexes` (show indexes) for a quick orientation.

## 3. b
Generates the SQL that the migrations *would* run, but doesn't execute it. Use cases: preview before a production deploy; share with a teammate or DBA; read in code review.

## 4. b
`__EFMigrationsHistory` — a table EF auto-creates and maintains. Each row records one applied migration name. Next time `Migrate()` runs, it diffs this table against the migrations folder to decide what to apply.

## 5. a
The worst time to learn a new tool is during an incident. Install the GUI today; click around once. When the bug lands six months from now, you'll be productive in seconds instead of fumbling for an hour.