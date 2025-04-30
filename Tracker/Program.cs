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
            Console.WriteLine("\nHabit Tracker");
            Console.WriteLine("1. Tambah");
            Console.WriteLine("2. Tandai Selesai");
            Console.WriteLine("3. Lihat");
            Console.WriteLine("4. Keluar");
            Console.Write("Pilih opsi: ");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1": TambahKebiasaan(conn); break;
                case "2": TandaiSelesai(conn); break;
                case "3": LihatKebiasaan(conn); break;
                case "4": return;
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
            );", conn);
        cmd.ExecuteNonQuery();
    }

    static void TambahKebiasaan(NpgsqlConnection conn)
    {
        Console.Write("Nama: ");
        string name = Console.ReadLine();
        Console.Write("Deskripsi: ");
        string description = Console.ReadLine();

        using var cmd = new NpgsqlCommand("INSERT INTO habits (name, description) VALUES (@name, @desc)", conn);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@desc", description);
        cmd.ExecuteNonQuery();

        Console.WriteLine("Berhasil ditambahkan!");
    }

    static void TandaiSelesai(NpgsqlConnection conn)
    {
        Console.Write("ID kebiasaan yang selesai: ");
        int id = int.Parse(Console.ReadLine());

        using var cmd = new NpgsqlCommand("UPDATE habits SET completed = TRUE WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();

        Console.WriteLine("Ditandai sebagai selesai!");
    }

    static void LihatKebiasaan(NpgsqlConnection conn)
    {
        using var cmd = new NpgsqlCommand("SELECT id, name, description, completed FROM habits", conn);
        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\nDaftar:");
        while (reader.Read())
        {
            Console.WriteLine($"{reader[0]} - {reader[1]} ({reader[2]}) - Selesai: {reader[3]}");
        }
    }
    static void HapusKebiasaan(NpgsqlConnection conn)
    {
        Console.Write("ID kebiasaan yang ingin dihapus: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID tidak valid!");
            return;
        }

        using var cmd = new NpgsqlCommand("DELETE FROM habits WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine(rows > 0 ? "Berhasil dihapus." : "ID tidak ditemukan.");
    }

}
