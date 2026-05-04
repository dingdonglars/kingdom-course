using Microsoft.Data.Sqlite;

namespace Kingdom.Persistence;

public static class SqliteDemo
{
    public static IReadOnlyList<(int Id, string Name, int Day, int Gold)> RunDemo(string dbPath)
    {
        // Pooling=False so the OS file handle releases when the connection disposes.
        // Pooling is great in production but bites in tests (file stays locked after Dispose).
        var connStr = $"Data Source={dbPath};Pooling=False";

        using var conn = new SqliteConnection(connStr);
        conn.Open();

        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS kingdoms (
                    id   INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    day  INTEGER NOT NULL,
                    gold INTEGER NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();
        }

        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "INSERT INTO kingdoms (name, day, gold) VALUES ($name, $day, $gold)";
            cmd.Parameters.AddWithValue("$name", "Eldoria");
            cmd.Parameters.AddWithValue("$day", 11);
            cmd.Parameters.AddWithValue("$gold", 250);
            cmd.ExecuteNonQuery();
        }

        var results = new List<(int, string, int, int)>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT id, name, day, gold FROM kingdoms ORDER BY id";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add((
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3)));
            }
        }

        return results;
    }
}
