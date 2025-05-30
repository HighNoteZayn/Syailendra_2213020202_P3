using System;
using Npgsql;

namespace Personal.Services
{
    public static class DatabaseConnection
    {
        private static string Host => "localhost";
        private static string Username => "postgres";
        private static string Password => "270703";
        private static string DatabaseName => "personal";

        private static string ConnectionString =>
            $"Host={Host};Username={Username};Password={Password};Database={DatabaseName}";

        public static NpgsqlConnection GetOpenConnection()
        {
            var conn = new NpgsqlConnection(ConnectionString);
            try
            {
                conn.Open(); // Akan lempar exception jika gagal
            }
            catch (NpgsqlException ex)
            {
                // Log error atau tangani exception
                Console.WriteLine($"Koneksi gagal: {ex.Message}");
                throw; // Lempar exception agar bisa ditangani lebih lanjut
            }
            return conn;
        }

        public static bool IsConnected()
        {
            try
            {
                using var conn = GetOpenConnection();
                return conn.State == System.Data.ConnectionState.Open;
            }
            catch (Exception ex)
            {
                // Tangani jika koneksi gagal
                Console.WriteLine($"Koneksi gagal: {ex.Message}");
                return false;
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            using var conn = GetOpenConnection();
            using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = @username AND password = @password", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password); // plain text untuk awal (nanti kita hash)

            var count = (long)cmd.ExecuteScalar();
            return count > 0;
        }

    }
}
