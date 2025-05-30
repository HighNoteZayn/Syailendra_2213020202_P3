using System;
using System.Windows.Forms;
using Personal.Services;

namespace Personal.Pages
{
    public partial class CuacaPage : UserControl
    {
        private Label labelCuaca;

        public CuacaPage()
        {
            InitializeComponent();
            LoadCuaca();
        }

        private void InitializeComponent()
        {
            labelCuaca = new Label();
            SuspendLayout();
            // 
            // labelCuaca
            // 
            labelCuaca.Location = new Point(0, 0);
            labelCuaca.Name = "labelCuaca";
            labelCuaca.Size = new Size(100, 23);
            labelCuaca.TabIndex = 0;
            // 
            // CuacaPage
            // 
            Controls.Add(labelCuaca);
            Name = "CuacaPage";
            Load += CuacaPage_Load;
            ResumeLayout(false);
        }

        private async void LoadCuaca()
        {
            labelCuaca.Text = "Memuat informasi cuaca...";
            try
            {
                var service = new CuacaService();
                var info = await service.GetCuacaAsync("Kediri");

                labelCuaca.Text = $@"
Kota         : {info.Kota}
Kondisi      : {info.Kondisi}
Suhu         : {info.Suhu}°C
Kelembapan   : {info.Kelembapan}%";
            }
            catch (Exception ex)
            {
                labelCuaca.Text = "Gagal memuat data cuaca.\n" + ex.Message;
            }
        }

        private void CuacaPage_Load(object sender, EventArgs e)
        {

        }
    }
}