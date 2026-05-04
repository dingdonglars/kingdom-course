-- Five sample queries against your kingdoms DB.
-- Copy any into DB Browser, sqlite3, or SQLTools.

-- 1. Every kingdom + how many buildings it has
SELECT k.name, COUNT(b.id) AS building_count
FROM Kingdoms k
LEFT JOIN Buildings b ON b.KingdomId = k.Id
GROUP BY k.Id
ORDER BY building_count DESC;

-- 2. The richest kingdom (by gold)
SELECT name, gold FROM Kingdoms ORDER BY gold DESC LIMIT 1;

-- 3. Buildings of each kind, with total levels
SELECT kind, COUNT(*) AS n, SUM(level) AS total_levels
FROM Buildings GROUP BY kind;

-- 4. Kingdoms that have at least one Mine
SELECT DISTINCT k.name
FROM Kingdoms k
JOIN Buildings b ON b.KingdomId = k.Id
WHERE b.kind = 'Mine';

-- 5. The migration history (what EF has applied)
SELECT * FROM __EFMigrationsHistory;
