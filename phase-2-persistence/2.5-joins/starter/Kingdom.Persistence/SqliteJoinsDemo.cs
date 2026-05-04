using Microsoft.Data.Sqlite;

namespace Kingdom.Persistence;

public static class SqliteJoinsDemo
{
    public record KingdomRow(int Id, string Name);
    public record BuildingRow(int Id, int KingdomId, string Kind, string Name, int Level);
    public record KingdomCount(string Name, int BuildingCount);

    public static (IReadOnlyList<KingdomRow> Kingdoms,
                   IReadOnlyList<BuildingRow> Buildings,
                   IReadOnlyList<KingdomCount> Counts)
        RunDemo(string dbPath)
    {
        var connStr = $"Data Source={dbPath};Pooling=False";
        using var conn = new SqliteConnection(connStr);
        conn.Open();

        Exec(conn, @"
            CREATE TABLE kingdoms (
                id   INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL
            );
            CREATE TABLE buildings (
                id          INTEGER PRIMARY KEY AUTOINCREMENT,
                kingdom_id  INTEGER NOT NULL REFERENCES kingdoms(id),
                kind        TEXT NOT NULL,
                name        TEXT NOT NULL,
                level       INTEGER NOT NULL DEFAULT 1
            );
        ");

        var eldoria = InsertKingdom(conn, "Eldoria");
        var briar   = InsertKingdom(conn, "Briarholm");
        var empty   = InsertKingdom(conn, "Stoneholt");
        InsertBuilding(conn, eldoria, "Farm", "Main",      3);
        InsertBuilding(conn, eldoria, "Mine", "Old Vein",  1);
        InsertBuilding(conn, briar,   "Farm", "East Farm", 2);

        var kingdoms = Read(conn, "SELECT id, name FROM kingdoms ORDER BY id",
            r => new KingdomRow(r.GetInt32(0), r.GetString(1)));

        var inner = Read(conn, @"
                SELECT b.id, b.kingdom_id, b.kind, b.name, b.level
                FROM buildings b
                INNER JOIN kingdoms k ON k.id = b.kingdom_id
                ORDER BY b.id",
            r => new BuildingRow(r.GetInt32(0), r.GetInt32(1), r.GetString(2), r.GetString(3), r.GetInt32(4)));

        var counts = Read(conn, @"
                SELECT k.name, COUNT(b.id) AS building_count
                FROM kingdoms k
                LEFT JOIN buildings b ON b.kingdom_id = k.id
                GROUP BY k.id
                ORDER BY k.id",
            r => new KingdomCount(r.GetString(0), r.GetInt32(1)));

        return (kingdoms, inner, counts);
    }

    private static int InsertKingdom(SqliteConnection conn, string name)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO kingdoms (name) VALUES ($name); SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$name", name);
        return (int)(long)cmd.ExecuteScalar()!;
    }

    private static void InsertBuilding(SqliteConnection conn, int kingdomId, string kind, string name, int level)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO buildings (kingdom_id, kind, name, level)
            VALUES ($kid, $kind, $name, $level)";
        cmd.Parameters.AddWithValue("$kid", kingdomId);
        cmd.Parameters.AddWithValue("$kind", kind);
        cmd.Parameters.AddWithValue("$name", name);
        cmd.Parameters.AddWithValue("$level", level);
        cmd.ExecuteNonQuery();
    }

    private static void Exec(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    private static List<T> Read<T>(SqliteConnection conn, string sql, Func<SqliteDataReader, T> map)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        using var r = cmd.ExecuteReader();
        var list = new List<T>();
        while (r.Read()) list.Add(map(r));
        return list;
    }
}
