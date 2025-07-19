using System.Windows.Forms;
using System.Drawing;
namespace LearnEasy2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Panel sidePanel;
        private Button btnWords;
        private Button btnGames;
        private Button btnSettings;
        private Label lblTitle;
        private ComboBox cmbGroup;
        private ComboBox cmbDifficulty;
        private Label lblGroup;
        private Label lblDifficulty;
        private Button btnStart;
        private Panel panelWords;
        private Panel panelGames;
        private Panel panelGameMatches;
		private Panel panelGameCards;
		private Panel panelGameSpellCheck;
		private Panel panelSettings;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.sidePanel = new System.Windows.Forms.Panel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.btnWords = new System.Windows.Forms.Button();
			this.btnGames = new System.Windows.Forms.Button();
			this.btnSettings = new System.Windows.Forms.Button();
			this.cmbGroup = new System.Windows.Forms.ComboBox();
			this.cmbDifficulty = new System.Windows.Forms.ComboBox();
			this.lblGroup = new System.Windows.Forms.Label();
			this.lblDifficulty = new System.Windows.Forms.Label();
			this.btnStart = new System.Windows.Forms.Button();
			this.panelWords = new System.Windows.Forms.Panel();
			this.panelGames = new System.Windows.Forms.Panel();
			this.panelGameCards = new System.Windows.Forms.Panel();
			this.panelGameMatches = new System.Windows.Forms.Panel();
			this.panelGameSpellCheck = new System.Windows.Forms.Panel();
			this.panelSettings = new System.Windows.Forms.Panel();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.sidePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// sidePanel
			// 
			this.sidePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(83)))));
			this.sidePanel.Controls.Add(this.lblTitle);
			this.sidePanel.Controls.Add(this.btnWords);
			this.sidePanel.Controls.Add(this.btnGames);
			this.sidePanel.Controls.Add(this.btnSettings);
			this.sidePanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.sidePanel.Location = new System.Drawing.Point(0, 0);
			this.sidePanel.Name = "sidePanel";
			this.sidePanel.Size = new System.Drawing.Size(140, 550);
			this.sidePanel.TabIndex = 0;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(3, 20);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(101, 20);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "📘 LearnEasy";
			// 
			// btnWords
			// 
			this.btnWords.FlatAppearance.BorderSize = 0;
			this.btnWords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnWords.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnWords.ForeColor = System.Drawing.Color.White;
			this.btnWords.Location = new System.Drawing.Point(5, 100);
			this.btnWords.Name = "btnWords";
			this.btnWords.Size = new System.Drawing.Size(120, 30);
			this.btnWords.TabIndex = 1;
			this.btnWords.Text = "Words";
			this.btnWords.Click += new System.EventHandler(this.btnWords_Click);
			// 
			// btnGames
			// 
			this.btnGames.FlatAppearance.BorderSize = 0;
			this.btnGames.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnGames.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnGames.ForeColor = System.Drawing.Color.White;
			this.btnGames.Location = new System.Drawing.Point(5, 140);
			this.btnGames.Name = "btnGames";
			this.btnGames.Size = new System.Drawing.Size(120, 30);
			this.btnGames.TabIndex = 2;
			this.btnGames.Text = "Games";
			this.btnGames.Click += new System.EventHandler(this.btnGames_Click);
			// 
			// btnSettings
			// 
			this.btnSettings.FlatAppearance.BorderSize = 0;
			this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnSettings.ForeColor = System.Drawing.Color.White;
			this.btnSettings.Location = new System.Drawing.Point(5, 180);
			this.btnSettings.Name = "btnSettings";
			this.btnSettings.Size = new System.Drawing.Size(120, 30);
			this.btnSettings.TabIndex = 3;
			this.btnSettings.Text = "Settings";
			this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
			// 
			// cmbGroup
			// 
			this.cmbGroup.Location = new System.Drawing.Point(0, 0);
			this.cmbGroup.Name = "cmbGroup";
			this.cmbGroup.Size = new System.Drawing.Size(121, 21);
			this.cmbGroup.TabIndex = 0;
			// 
			// cmbDifficulty
			// 
			this.cmbDifficulty.Location = new System.Drawing.Point(0, 0);
			this.cmbDifficulty.Name = "cmbDifficulty";
			this.cmbDifficulty.Size = new System.Drawing.Size(121, 21);
			this.cmbDifficulty.TabIndex = 0;
			// 
			// lblGroup
			// 
			this.lblGroup.Location = new System.Drawing.Point(0, 0);
			this.lblGroup.Name = "lblGroup";
			this.lblGroup.Size = new System.Drawing.Size(100, 23);
			this.lblGroup.TabIndex = 0;
			// 
			// lblDifficulty
			// 
			this.lblDifficulty.Location = new System.Drawing.Point(0, 0);
			this.lblDifficulty.Name = "lblDifficulty";
			this.lblDifficulty.Size = new System.Drawing.Size(100, 23);
			this.lblDifficulty.TabIndex = 0;
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(0, 0);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 0;
			// 
			// panelWords
			// 
			this.panelWords.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			this.panelWords.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelWords.Location = new System.Drawing.Point(140, 0);
			this.panelWords.Name = "panelWords";
			this.panelWords.Size = new System.Drawing.Size(760, 550);
			this.panelWords.TabIndex = 0;
			this.panelWords.Visible = false;
			// 
			// panelGames
			// 
			this.panelGames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			this.panelGames.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelGames.Location = new System.Drawing.Point(140, 0);
			this.panelGames.Name = "panelGames";
			this.panelGames.Size = new System.Drawing.Size(760, 550);
			this.panelGames.TabIndex = 1;
			this.panelGames.Visible = false;
			// 
			// panelGameMatches
			// 
			this.panelGameMatches.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			this.panelGameMatches.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelGameMatches.Location = new System.Drawing.Point(140, 0);
			this.panelGameMatches.Name = "panelGameMatches";
			this.panelGameMatches.Size = new System.Drawing.Size(760, 550);
			this.panelGameMatches.TabIndex = 2;
			this.panelGameMatches.Visible = false;
			// 
			// panelGameCards
			// 
			this.panelGameCards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			this.panelGameCards.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelGameCards.Location = new System.Drawing.Point(140, 0);
			this.panelGameCards.Name = "panelGameCards";
			this.panelGameCards.Size = new System.Drawing.Size(760, 550);
			this.panelGameCards.TabIndex = 2;
			this.panelGameCards.Visible = false;
			//
			// panelGameSpellCheck
			// 
			this.panelGameSpellCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			this.panelGameSpellCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelGameSpellCheck.Location = new System.Drawing.Point(140, 0);
			this.panelGameSpellCheck.Name = "panelGameCards";
			this.panelGameSpellCheck.Size = new System.Drawing.Size(760, 550);
			this.panelGameSpellCheck.TabIndex = 2;
			this.panelGameSpellCheck.Visible = false;
			// panelSettings
			// 
			this.panelSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSettings.Location = new System.Drawing.Point(140, 0);
			this.panelSettings.Name = "panelSettings";
			this.panelSettings.Size = new System.Drawing.Size(760, 550);
			this.panelSettings.TabIndex = 3;
			this.panelSettings.Visible = false;
			// 
			// timer1
			// 
			this.timer1.Interval = 10;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(0, 0);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(100, 23);
			this.progressBar1.TabIndex = 4;
			this.progressBar1.Visible = false;
			// 
			// Form1
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			this.ClientSize = new System.Drawing.Size(900, 550);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.panelWords);
			this.Controls.Add(this.panelGames); 
			this.Controls.Add(this.panelGameCards);
			this.Controls.Add(this.panelGameSpellCheck);
			this.Controls.Add(this.panelGameMatches);
			this.Controls.Add(this.panelSettings);
			this.Controls.Add(this.sidePanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(900, 500);
			this.Name = "Form1";
			this.Text = "LearnEasy2";
			this.sidePanel.ResumeLayout(false);
			this.sidePanel.PerformLayout();
			this.ResumeLayout(false);

        }

		private Timer timer1;
		private ProgressBar progressBar1;
	}
}
