using System;
using System.Collections.Generic;
using Npgsql;

class Program
{
    static string connString = "Host=localhost;Username=postgres;Password=270703;Database=habit_tracker";

    static void Main()
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        InitializeDatabase(conn);

        while (true)
        {
            Console.WriteLine("\nMenu Utama");
            Console.WriteLine("1. Tambah Habit");
            Console.WriteLine("2. Tandai Habit Selesai");
            Console.WriteLine("3. Lihat Habit");
            Console.WriteLine("4. Hapus Habit");
            Console.WriteLine("5. Tambah Task");
            Console.WriteLine("6. Tandai Task Selesai");
            Console.WriteLine("7. Lihat Task");
            Console.WriteLine("8. Hapus Task");
            Console.WriteLine("9. Keluar");
            Console.Write("Pilih opsi: ");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1": TambahKebiasaan(conn); break;
                case "2": TandaiSelesaiHabit(conn); break;
                case "3": LihatKebiasaan(conn); break;
                case "4": HapusKebiasaan(conn); break;
                case "5": TambahTask(conn); break;
                case "6": TandaiSelesaiTask(conn); break;
                case "7": LihatTask(conn); break;
                case "8": HapusTask(conn); break;
                case "9": return;
                default: Console.WriteLine("Opsi tidak valid"); break;
            }
        }
    }

    static void InitializeDatabase(NpgsqlConnection conn)
    {
        using var cmd = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS habits (
                id SERIAL PRIMARY KEY,
                name TEXT NOT NULL,
                description TEXT,
                completed BOOLEAN DEFAULT FALSE,
                created_at TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS tasks (
                id SERIAL PRIMARY KEY,
                title TEXT NOT NULL,
                category_id INTEGER,
                due_date DATE,
                is_completed BOOLEAN DEFAULT FALSE,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );
        ", conn);
        cmd.ExecuteNonQuery();
    }

    // ---------- HABIT ----------
    static void TambahKebiasaan(NpgsqlConnection conn)
    {
        Console.Write("Nama Habit: ");
        string name = Console.ReadLine();
        Console.Write("Deskripsi: ");
        string description = Console.ReadLine();

        using var cmd = new NpgsqlCommand("INSERT INTO habits (name, description) VALUES (@name, @desc)", conn);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@desc", description);
        cmd.ExecuteNonQuery();

        Console.WriteLine("Habit berhasil ditambahkan!");
    }

    static void TandaiSelesaiHabit(NpgsqlConnection conn)
    {
        Console.Write("ID habit yang selesai: ");
        int id = int.Parse(Console.ReadLine());

        using var cmd = new NpgsqlCommand("UPDATE habits SET completed = TRUE WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();

        Console.WriteLine("Habit ditandai selesai!");
    }

    static void LihatKebiasaan(NpgsqlConnection conn)
    {
        using var cmd = new NpgsqlCommand("SELECT id, name, description, completed FROM habits", conn);
        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\nDaftar Habit:");
        while (reader.Read())
        {
            Console.WriteLine($"{reader[0]} - {reader[1]} ({reader[2]}) - Selesai: {reader[3]}");
        }
    }

    static void HapusKebiasaan(NpgsqlConnection conn)
    {
        Console.Write("ID habit yang ingin dihapus: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID tidak valid!");
            return;
        }

        using var cmd = new NpgsqlCommand("DELETE FROM habits WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine(rows > 0 ? "Habit dihapus." : "ID tidak ditemukan.");
    }

    // ---------- TASK ----------
    static void TambahTask(NpgsqlConnection conn)
    {
        Console.Write("Judul Task: ");
        string title = Console.ReadLine();
        Console.Write("Category ID (angka, atau kosong): ");
        string catInput = Console.ReadLine();
        int? categoryId = string.IsNullOrWhiteSpace(catInput) ? (int?)null : int.Parse(catInput);

        Console.Write("Tanggal deadline (yyyy-mm-dd): ");
        DateTime dueDate = DateTime.Parse(Console.ReadLine());

        using var cmd = new NpgsqlCommand("INSERT INTO tasks (title, category_id, due_date) VALUES (@title, @cat, @date)", conn);
        cmd.Parameters.AddWithValue("@title", title);
        if (categoryId.HasValue)
            cmd.Parameters.AddWithValue("@cat", categoryId.Value);
        else
            cmd.Parameters.AddWithValue("@cat", DBNull.Value);
        cmd.Parameters.AddWithValue("@date", dueDate);

        cmd.ExecuteNonQuery();
        Console.WriteLine("Task berhasil ditambahkan!");
    }

    static void TandaiSelesaiTask(NpgsqlConnection conn)
    {
        Console.Write("ID task yang selesai: ");
        int id = int.Parse(Console.ReadLine());

        using var cmd = new NpgsqlCommand("UPDATE tasks SET is_completed = TRUE WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();

        Console.WriteLine("Task ditandai selesai!");
    }

    static void LihatTask(NpgsqlConnection conn)
    {
        using var cmd = new NpgsqlCommand("SELECT id, title, category_id, due_date, is_completed FROM tasks ORDER BY due_date", conn);
        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\nDaftar Task:");
        while (reader.Read())
        {
            Console.WriteLine($"{reader[0]} - {reader[1]} (Kategori ID: {reader[2]}) - Due: {reader[3]:yyyy-MM-dd} - Selesai: {reader[4]}");
        }
    }

    static void HapusTask(NpgsqlConnection conn)
    {
        Console.Write("ID task yang ingin dihapus: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID tidak valid!");
            return;
        }

        using var cmd = new NpgsqlCommand("DELETE FROM tasks WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine(rows > 0 ? "Task dihapus." : "ID tidak ditemukan.");
    }
}
