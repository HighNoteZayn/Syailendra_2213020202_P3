using System;
using System.Drawing;
using System.Windows.Forms;
using Assistly;
using Npgsql;
using Personal.Services;

namespace Personal.Pages
{
    public partial class TambahTaskPage : UserControl
    {
        private TextBox txtJudul;
        private DateTimePicker dtpDeadline;
        private Label lblJudul;
        private Label lblDeadline;
        private Label label1;
        private Button btnSimpan;

        public TambahTaskPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lblJudul = new Label();
            txtJudul = new TextBox();
            lblDeadline = new Label();
            dtpDeadline = new DateTimePicker();
            btnSimpan = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // lblJudul
            // 
            lblJudul.Location = new Point(20, 60);
            lblJudul.Name = "lblJudul";
            lblJudul.Size = new Size(100, 23);
            lblJudul.TabIndex = 0;
            lblJudul.Text = "Judul";
            // 
            // txtJudul
            // 
            txtJudul.Location = new Point(126, 52);
            txtJudul.Name = "txtJudul";
            txtJudul.Size = new Size(200, 31);
            txtJudul.TabIndex = 1;
            // 
            // lblDeadline
            // 
            lblDeadline.Location = new Point(20, 113);
            lblDeadline.Name = "lblDeadline";
            lblDeadline.Size = new Size(100, 23);
            lblDeadline.TabIndex = 2;
            lblDeadline.Text = "Batas";
            // 
            // dtpDeadline
            // 
            dtpDeadline.Location = new Point(130, 105);
            dtpDeadline.Name = "dtpDeadline";
            dtpDeadline.Size = new Size(200, 31);
            dtpDeadline.TabIndex = 3;
            // 
            // btnSimpan
            // 
            btnSimpan.Location = new Point(130, 155);
            btnSimpan.Name = "btnSimpan";
            btnSimpan.Size = new Size(93, 37);
            btnSimpan.TabIndex = 4;
            btnSimpan.Text = "Simpan";
            btnSimpan.Click += BtnSimpan_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(0, 255, 238);
            label1.Font = new Font("Abadi MT Condensed Extra Bold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(482, 18);
            label1.Name = "label1";
            label1.Size = new Size(36, 23);
            label1.TabIndex = 5;
            label1.Text = "<<";
            label1.Click += label1_Click;
            // 
            // TambahTaskPage
            // 
            Controls.Add(label1);
            Controls.Add(lblJudul);
            Controls.Add(txtJudul);
            Controls.Add(lblDeadline);
            Controls.Add(dtpDeadline);
            Controls.Add(btnSimpan);
            Name = "TambahTaskPage";
            Size = new Size(551, 276);
            Load += TambahTaskPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void BtnSimpan_Click(object sender, EventArgs e)
        {
            string title = txtJudul.Text.Trim();
            DateTime deadline = dtpDeadline.Value;

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Judul tugas tidak boleh kosong.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var conn = DatabaseConnection.GetOpenConnection();
                using var cmd = new NpgsqlCommand("INSERT INTO task (nama, tanggal) VALUES (@nama, @tanggal)", conn);
                cmd.Parameters.AddWithValue("@nama", title);
                cmd.Parameters.AddWithValue("@tanggal", deadline);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Tugas berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtJudul.Clear();
                dtpDeadline.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan tugas:\n" + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form1 Form = new Form1(); // Ganti dengan nama form tujuan
            Form.Show(); // Menampilkan form baru
            this.Hide();
        }

        private void TambahTaskPage_Load(object sender, EventArgs e)
        {

        }
    }
}
