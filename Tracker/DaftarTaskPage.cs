using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Npgsql;

namespace Personal.Pages
{
    public partial class DaftarTaskPage : UserControl
    {
        private readonly NpgsqlConnection _conn;
        private ListView listViewTasks;
        private ListBox listBox1;

        public DaftarTaskPage(NpgsqlConnection conn)
        {
            _conn = conn;
            InitializeComponent();
            LoadTasks();
        }

        private void InitializeComponent()
        {
            listViewTasks = new ListView();
            listBox1 = new ListBox();
            SuspendLayout();
            // 
            // listViewTasks
            // 
            listViewTasks.Location = new Point(0, 0);
            listViewTasks.Name = "listViewTasks";
            listViewTasks.Size = new Size(121, 97);
            listViewTasks.TabIndex = 0;
            listViewTasks.UseCompatibleStateImageBehavior = false;
            listViewTasks.SelectedIndexChanged += listViewTasks_SelectedIndexChanged;
            // 
            // listBox1
            // 
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(0, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(120, 79);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // DaftarTaskPage
            // 
            Controls.Add(listBox1);
            Controls.Add(listViewTasks);
            Name = "DaftarTaskPage";
            ResumeLayout(false);
        }


        private void LoadTasks()
        {
            try
            {
                using var cmd = new NpgsqlCommand("SELECT id, title, due_date, is_completed FROM tasks ORDER BY due_date", _conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var item = new ListViewItem(reader.GetInt32(0).ToString());
                    item.SubItems.Add(reader.GetString(1));
                    item.SubItems.Add(reader.GetDateTime(2).ToString("yyyy-MM-dd"));
                    item.SubItems.Add(reader.GetBoolean(3) ? "✓" : "✗");
                    listViewTasks.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat tugas: " + ex.Message);
            }
        }

        private void listViewTasks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void DaftarTaskPage_Load(object sender, EventArgs e)
        {
            var service = new HttpClientService();
            var tasks = await service.GetTasksAsync();

            foreach (var task in tasks)
            {
                // Tampilkan ke ListView, DataGrid, dll
                listBox1.Items.Add($"{task.Tanggal.ToShortDateString()} - {task.Judul}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}