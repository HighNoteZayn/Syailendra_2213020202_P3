using System;
using System.Windows.Forms;
using Npgsql;
using Personal.Services;

namespace Personal.Pages
{
    public partial class KalenderPage : UserControl
    {
        private readonly NpgsqlConnection _conn;
        private MonthCalendar monthCalendar;
        private ListView listViewTasks;

        public KalenderPage(NpgsqlConnection conn)
        {
            _conn = conn;
            InitializeComponent();
            LoadTasksForToday();
        }

        private void InitializeComponent()
        {
            // Inisialisasi MonthCalendar
            monthCalendar = new MonthCalendar
            {
                Location = new System.Drawing.Point(20, 20),
                MaxSelectionCount = 1,
                TodayDate = DateTime.Today
            };
            monthCalendar.DateChanged += MonthCalendar_DateChanged;

            // Inisialisasi ListView
            listViewTasks = new ListView
            {
                Location = new System.Drawing.Point(20, 250),
                Size = new System.Drawing.Size(400, 200),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            listViewTasks.Columns.Add("ID", 50);
            listViewTasks.Columns.Add("Judul", 200);
            listViewTasks.Columns.Add("Deadline", 100);
            listViewTasks.Columns.Add("Selesai", 70);

            this.Controls.Add(monthCalendar);
            this.Controls.Add(listViewTasks);
        }

        private void MonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            LoadTasks(e.Start); // Muat tugas berdasarkan tanggal yang dipilih
        }

        private void LoadTasks(DateTime date)
        {
            listViewTasks.Items.Clear();

            try
            {
                string query = @"
                    SELECT id, title, due_date, is_completed 
                    FROM tasks 
                    WHERE due_date = @selectedDate 
                    ORDER BY due_date";

                using var cmd = new NpgsqlCommand(query, _conn);
                cmd.Parameters.AddWithValue("selectedDate", date.Date);

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
                    var emptyItem = new ListViewItem("Tidak ada tugas di tanggal ini.");
                    emptyItem.ForeColor = System.Drawing.Color.Gray;
                    listViewTasks.Items.Add(emptyItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat tugas: " + ex.Message);
            }
        }

        private void LoadTasksForToday()
        {
            LoadTasks(DateTime.Today);
        }
    }
}