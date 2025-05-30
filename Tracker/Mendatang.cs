using System;
using System.Windows.Forms;
using Npgsql;
using Personal.Services;

namespace Personal
{
    public partial class Mendatang : UserControl
    {
        private ListView listViewTasks;

        public Mendatang()
        {
            InitializeComponent(); // Ini milik Designer
            InitListView();        // Custom layout buatan sendiri
            this.Load += Mendatang_Load;
        }

        private void InitListView()
        {
            listViewTasks = new ListView
            {
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(500, 300),
                View = View.Details,
                FullRowSelect = true
            };

            listViewTasks.Columns.Add("ID", 50);
            listViewTasks.Columns.Add("Judul", 200);
            listViewTasks.Columns.Add("Tanggal", 100);

            Controls.Add(listViewTasks);
            this.Size = new System.Drawing.Size(550, 350);
        }

        private void LoadTasks()
        {
            try
            {
                using var conn = DatabaseConnection.GetOpenConnection();

                // Cek apakah koneksi terbuka
                if (conn == null || conn.State != System.Data.ConnectionState.Open)
                {
                    MessageBox.Show("Koneksi database tidak terbuka.", "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Query untuk mengambil data
                using var cmd = new NpgsqlCommand(@"
                SELECT id, nama, tanggal 
                FROM task 
                WHERE tanggal > CURRENT_DATE 
                ORDER BY tanggal", conn);

                using var reader = cmd.ExecuteReader();

                listViewTasks.Items.Clear(); // Hapus item lama

                // Membaca data dari hasil query
                while (reader.Read())
                {
                    var item = new ListViewItem(reader.GetInt32(0).ToString());
                    item.SubItems.Add(reader.GetString(1));
                    item.SubItems.Add(reader.GetDateTime(2).ToString("yyyy-MM-dd"));
                    listViewTasks.Items.Add(item);
                }

                // Jika tidak ada data, tampilkan pesan
                if (listViewTasks.Items.Count == 0)
                {
                    var emptyItem = new ListViewItem("Tidak ada tugas mendatang.");
                    emptyItem.ForeColor = System.Drawing.Color.Gray;
                    listViewTasks.Items.Add(emptyItem);
                }
            }
            catch (NpgsqlException ex)
            {
                // Menangani error spesifik NpgsqlException
                MessageBox.Show($"Gagal memuat tugas mendatang: {ex.Message}", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Menangani error umum lainnya
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Mendatang_Load(object sender, EventArgs e)
        {
            // Verifikasi koneksi terlebih dahulu sebelum melanjutkan
            try
            {
                using (var conn = DatabaseConnection.GetOpenConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        MessageBox.Show("Koneksi berhasil!");
                        LoadTasks();
                    }
                    else
                    {
                        MessageBox.Show("Koneksi gagal", "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal terhubung ke database: {ex.Message}", "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
