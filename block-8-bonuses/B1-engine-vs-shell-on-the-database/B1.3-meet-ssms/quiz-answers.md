# Quiz answers — B1.3

## 1. b
SSMS is the standard SQL Server GUI. Free, full-featured, used by every working DBA. Object Explorer + query windows + execution plans + activity monitor — the toolkit.

## 2. b
Azure Data Studio is Microsoft's cross-platform alternative. Same core (browse, query, plan), modern UI, runs on macOS/Linux. Pick whichever fits your OS.

## 3. a
The execution plan shows how the query will be processed: which index is used, in what order, how rows flow between operators. The first thing to read when "this query is slow" lands. Often you'll see a missing index suggestion.

## 4. a
The work is doable without SSMS — `sqlcmd`, EF logs, programmatic queries. But the GUI compresses minutes into seconds. Knowing one professional tool well = years of compounding productivity.

## 5. b
Generate Scripts exports the database (schema, optionally data) as one `.sql` file. Use it for backups, sharing with teammates, sanity-check diffs in version control.