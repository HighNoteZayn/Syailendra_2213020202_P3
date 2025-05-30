using System;
using System.Windows.Forms;
using Npgsql;
using Personal.Pages;
using Personal.Services;

namespace Personal
{
    public partial class Form1 : Form
    {

        private ListBox listBox1;

        public Form1()
        {
            InitializeComponent();

            // Inisialisasi panel konten
            panelContent.Dock = DockStyle.Fill;
            this.Controls.Add(panelContent);

            // Atur teks tombol navigasi
            Mendatang.Text = "Mendatang";
            HariIni.Text = "Hari Ini";
            Kalender.Text = "Kalender";
            Cuaca.Text = "Cuaca";
            Tambah.Text = "Tambah Tugas";
            Pengaturan.Text = "Pengaturan";
            Keluar.Text = "Keluar";

            // Event handler untuk tombol
            Mendatang.Click += ButtonMendatang_Click;
            HariIni.Click += ButtonHariIni_Click;
            Kalender.Click += ButtonKalender_Click;
            Cuaca.Click += ButtonCuaca_Click;
            Tambah.Click += ButtonTambahTugas_Click;
            Pengaturan.Click += ButtonPengaturan_Click;
            Keluar.Click += ButtonKeluar_Click;

            Search.TextChanged += Search_TextChanged;

            this.Load += Form1_Load;
        }

        private void InitializeCustomComponents() // ? Benar
        {
            listBox1 = new ListBox
            {
                Location = new Point(10, 10),
                Size = new Size(400, 300)
            };
            this.Controls.Add(listBox1);
        }


        private void ButtonMendatang_Click(object sender, EventArgs e)
        {
            ShowContent(new TaskMendatangPage(GetDatabaseConnection()));
        }

        private void ButtonHariIni_Click(object sender, EventArgs e)
        {
            ShowContent(new TaskHariIniPage(GetDatabaseConnection()));
        }

        private void ButtonKalender_Click(object sender, EventArgs e)
        {
            ShowContent(new KalenderPage(GetDatabaseConnection()));
        }

        private void ButtonCuaca_Click(object sender, EventArgs e)
        {
            ShowContent(new CuacaPage());
        }

        private void ButtonTambahTugas_Click(object sender, EventArgs e)
        {
            ShowContent(new TambahTaskPage());
        }

        private void ButtonPengaturan_Click(object sender, EventArgs e)
        {
            // Implementasi halaman pengaturan
        }

        private void ButtonKeluar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            string keyword = Search.Text.Trim();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Implementasi halaman pengaturan
        }

        private NpgsqlConnection GetDatabaseConnection()
        {
            var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=270703;Database=personal");
            conn.Open(); // Tambahkan ini
            return conn;
        }

        private void ShowContent(UserControl content)
        {
            // Hapus kontrol lama jika ada
            panelContent.Controls.Clear();

            // Tambahkan kontrol baru
            if (content != null)
            {
                panelContent.Controls.Add(content);
                content.Dock = DockStyle.Fill;
                panelContent.BringToFront();
            }
            else
            {
                MessageBox.Show("Kontrol yang akan ditampilkan tidak valid.");
            }
        }

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}