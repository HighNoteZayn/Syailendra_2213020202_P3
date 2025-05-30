namespace Personal
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panelContent = new Panel();
            panelbar = new Panel();
            Keluar = new Button();
            Pengaturan = new Button();
            Tambah = new Button();
            Cuaca = new Button();
            Kalender = new Button();
            HariIni = new Button();
            Mendatang = new Button();
            Search = new TextBox();
            panelbar.SuspendLayout();
            SuspendLayout();
            // 
            // panelContent
            // 
            resources.ApplyResources(panelContent, "panelContent");
            panelContent.Name = "panelContent";
            panelContent.Paint += panelContent_Paint;
            // 
            // panelbar
            // 
            panelbar.BackColor = SystemColors.Info;
            panelbar.Controls.Add(Keluar);
            panelbar.Controls.Add(Pengaturan);
            panelbar.Controls.Add(Tambah);
            panelbar.Controls.Add(Cuaca);
            panelbar.Controls.Add(Kalender);
            panelbar.Controls.Add(HariIni);
            panelbar.Controls.Add(Mendatang);
            panelbar.Controls.Add(Search);
            resources.ApplyResources(panelbar, "panelbar");
            panelbar.Name = "panelbar";
            // 
            // Keluar
            // 
            resources.ApplyResources(Keluar, "Keluar");
            Keluar.Name = "Keluar";
            Keluar.UseVisualStyleBackColor = true;
            // 
            // Pengaturan
            // 
            resources.ApplyResources(Pengaturan, "Pengaturan");
            Pengaturan.Name = "Pengaturan";
            Pengaturan.UseVisualStyleBackColor = true;
            // 
            // Tambah
            // 
            resources.ApplyResources(Tambah, "Tambah");
            Tambah.Name = "Tambah";
            Tambah.UseVisualStyleBackColor = true;
            Tambah.Click += ButtonTambahTugas_Click;
            // 
            // Cuaca
            // 
            resources.ApplyResources(Cuaca, "Cuaca");
            Cuaca.Name = "Cuaca";
            Cuaca.UseVisualStyleBackColor = true;
            Cuaca.Click += ButtonCuaca_Click;
            // 
            // Kalender
            // 
            resources.ApplyResources(Kalender, "Kalender");
            Kalender.Name = "Kalender";
            Kalender.UseVisualStyleBackColor = true;
            Kalender.Click += ButtonKalender_Click;
            // 
            // HariIni
            // 
            resources.ApplyResources(HariIni, "HariIni");
            HariIni.Name = "HariIni";
            HariIni.UseVisualStyleBackColor = true;
            HariIni.Click += ButtonHariIni_Click;
            // 
            // Mendatang
            // 
            resources.ApplyResources(Mendatang, "Mendatang");
            Mendatang.Name = "Mendatang";
            Mendatang.UseVisualStyleBackColor = true;
            Mendatang.Click += ButtonMendatang_Click;
            // 
            // Search
            // 
            resources.ApplyResources(Search, "Search");
            Search.Name = "Search";
            Search.TextChanged += Search_TextChanged;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelbar);
            Controls.Add(panelContent);
            Name = "Form1";
            Load += Form1_Load;
            panelbar.ResumeLayout(false);
            panelbar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelContent;
        private Panel panelbar;
        private TextBox Search;
        private Button Mendatang;
        private Button Cuaca;
        private Button Kalender;
        private Button HariIni;
        private Button Pengaturan;
        private Button Tambah;
        private Button Keluar;
    }
}
