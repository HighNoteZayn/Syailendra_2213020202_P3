using System;
using System.Windows.Forms;
using Npgsql;
using Personal.Services;


namespace Personal.Pages
{
    public partial class TaskMendatangPage : UserControl
    {
        private readonly NpgsqlConnection _conn;
        private ListView listViewTasks;

        public TaskMendatangPage(NpgsqlConnection conn)
        {
            _conn = conn;
            InitializeComponent();
            this.Dock = DockStyle.Fill; // ⬅️ Pastikan UserControl memenuhi panel
            LoadTasks();
        }

        private void InitializeComponent()
        {
            listViewTasks = new ListView();
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
            // TaskMendatangPage
            // 
            Controls.Add(listViewTasks);
            Name = "TaskMendatangPage";
            Load += TaskMendatangPage_Load_1;
            ResumeLayout(false);
        }

        private void LoadTasks()
        {
            try
            {
                using var cmd = new NpgsqlCommand(@"
                    SELECT id, title, due_date, is_completed 
                    FROM tasks 
                    WHERE due_date > CURRENT_DATE
                    ORDER BY due_date", _conn);

                using var reader = cmd.ExecuteReader();

                bool adaData = false;

                while (reader.Read())
                {
                    adaData = true;
                    var item = new ListViewItem(reader.GetInt32(0).ToString());
                    item.SubItems.Add(reader.GetString(1));
                    item.SubItems.Add(reader.GetDateTime(2).ToString("yyyy-MM-dd"));
                    item.SubItems.Add(reader.GetBoolean(3) ? "✓" : "✗");
                    listViewTasks.Items.Add(item);
                }

                if (!adaData)
                {
                    var emptyItem = new ListViewItem("Tidak ada tugas mendatang.");
                    emptyItem.ForeColor = System.Drawing.Color.Gray;
                    listViewTasks.Items.Add(emptyItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat tugas mendatang: " + ex.Message);
            }
        }

        private void TaskMendatangPage_Load(object sender, EventArgs e)
        {
            // Hapus jika tidak dipakai
        }

        private void TaskMendatangPage_Load_1(object sender, EventArgs e)
        {

        }

        private void listViewTasks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}