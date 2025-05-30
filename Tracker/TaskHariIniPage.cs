using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;
using Personal.Services;

namespace Personal.Pages
{
    public partial class TaskHariIniPage : UserControl
    {
        private readonly NpgsqlConnection _conn;
        private ListView listViewTasks;
        private Button btnHapus;

        public TaskHariIniPage(NpgsqlConnection conn)
        {
            _conn = conn;
            InitializeComponent();
            LoadTasks();
        }

        private void InitializeComponent()
        {
            listViewTasks = new ListView();
            btnHapus = new Button();
            SuspendLayout();
            // 
            // listViewTasks
            // 
            listViewTasks.CheckBoxes = true;
            listViewTasks.FullRowSelect = true;
            listViewTasks.Location = new Point(10, 10);
            listViewTasks.Name = "listViewTasks";
            listViewTasks.Size = new Size(400, 300);
            listViewTasks.TabIndex = 0;
            listViewTasks.UseCompatibleStateImageBehavior = false;
            listViewTasks.View = View.Details;

            // ✅ Tambahkan kolom-kolom ini
            listViewTasks.Columns.Add("ID", 50);
            listViewTasks.Columns.Add("Nama", 150);
            listViewTasks.Columns.Add("Tanggal", 100);
            listViewTasks.Columns.Add("Selesai", 80);

            listViewTasks.ItemChecked += ListViewTasks_ItemChecked;
            listViewTasks.SelectedIndexChanged += listViewTasks_SelectedIndexChanged;
            // 
            // btnHapus
            // 
            btnHapus.Location = new Point(10, 320);
            btnHapus.Name = "btnHapus";
            btnHapus.Size = new Size(75, 37);
            btnHapus.TabIndex = 1;
            btnHapus.Text = "Hapus";
            btnHapus.Click += BtnHapus_Click;
            // 
            // TaskHariIniPage
            // 
            Controls.Add(listViewTasks);
            Controls.Add(btnHapus);
            Name = "TaskHariIniPage";
            Size = new Size(430, 360);
            ResumeLayout(false);
        }


        private void LoadTasks()
        {
            try
            {
                if (_conn.State != System.Data.ConnectionState.Open)
                    _conn.Open();

                listViewTasks.Items.Clear();

                using var cmd = new NpgsqlCommand(@"
                    SELECT id, nama, tanggal, selesai
                    FROM task
                    WHERE tanggal = CURRENT_DATE
                    ORDER BY tanggal", _conn);

                using var reader = cmd.ExecuteReader();

                bool adaData = false;

                while (reader.Read())
                {
                    adaData = true;
                    var id = reader.GetInt32(0).ToString();
                    var nama = reader.GetString(1);
                    var tanggal = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                    var selesai = reader.GetBoolean(3);

                    var item = new ListViewItem(id);
                    item.SubItems.Add(nama);
                    item.SubItems.Add(tanggal);
                    item.SubItems.Add(selesai ? "✓" : "✗");
                    item.Checked = selesai;
                    listViewTasks.Items.Add(item);
                }

                if (!adaData)
                {
                    var emptyItem = new ListViewItem("Tidak ada tugas hari ini.");
                    emptyItem.ForeColor = Color.Gray;
                    listViewTasks.Items.Add(emptyItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat tugas hari ini: " + ex.Message);
            }
        }

        private void ListViewTasks_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.SubItems.Count < 4) return;

            if (!int.TryParse(e.Item.Text, out int id)) return;

            bool selesai = e.Item.Checked;

            try
            {
                using var cmd = new NpgsqlCommand("UPDATE task SET selesai = @selesai WHERE id = @id", _conn);
                cmd.Parameters.AddWithValue("@selesai", selesai);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                // Update tampilan kolom "Selesai"
                e.Item.SubItems[3].Text = selesai ? "✓" : "✗";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mengupdate status selesai: " + ex.Message);
            }
        }

        private void BtnHapus_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Pilih tugas yang ingin dihapus.");
                return;
            }

            var confirm = MessageBox.Show("Yakin ingin menghapus tugas ini?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            foreach (ListViewItem item in listViewTasks.SelectedItems)
            {
                if (!int.TryParse(item.Text, out int id)) continue;

                try
                {
                    using var cmd = new NpgsqlCommand("DELETE FROM task WHERE id = @id", _conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    listViewTasks.Items.Remove(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus tugas: " + ex.Message);
                }
            }
        }

        private void listViewTasks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
