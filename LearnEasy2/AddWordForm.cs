using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;

namespace LearnEasy2
{
    public partial class AddWordForm : Form
    {
        public string Word1 => txtWord1.Text.Trim();
        public string Word2 => txtWord2.Text.Trim();
        public string Lang1 => cmbLang1.SelectedItem?.ToString();
        public string Lang2 => cmbLang2.SelectedItem?.ToString();
        public string Group => cmbGroup.SelectedItem?.ToString();

        private TextBox txtWord1;
        private TextBox txtWord2;
        private ComboBox cmbLang1;
        private ComboBox cmbLang2;
        private ComboBox cmbGroup;

        public AddWordForm(List<string> groups)
        {
            InitializeComponent();
            InitializeFields(groups);
        }

        private void InitializeFields(List<string> groups)
        {
            this.Text = "Add Word";
            this.Size = new Size(500, 240);
            this.BackColor = Color.FromArgb(204, 255, 204);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 10);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.SizeGripStyle = SizeGripStyle.Hide;

            // === Word 1 ===
            var lblWord1 = new Label
            {
                Text = "Enter word:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtWord1 = new TextBox
            {
                Location = new Point(140, 20),
                Size = new Size(200, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            cmbLang1 = new ComboBox
            {
                Location = new Point(350, 20),
                Size = new Size(100, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbLang1.Items.AddRange(new[] { "ru", "eng", "es", "fr" });
            cmbLang1.SelectedIndex = 0;

            // === Word 2 ===
            var lblWord2 = new Label
            {
                Text = "Enter word translation:",
                Location = new Point(20, 60),
                AutoSize = true
            };

            txtWord2 = new TextBox
            {
                Location = new Point(190, 60),
                Size = new Size(150, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            cmbLang2 = new ComboBox
            {
                Location = new Point(350, 60),
                Size = new Size(100, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbLang2.Items.AddRange(new[] { "ru", "eng", "es", "fr" });
            cmbLang2.SelectedIndex = 1;

            // === Group ===
            var lblGroup = new Label
            {
                Text = "Choose word group:",
                Location = new Point(20, 100),
                AutoSize = true
            };

            cmbGroup = new ComboBox
            {
                Location = new Point(190, 100),
                Size = new Size(260, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbGroup.Items.AddRange(groups.ToArray());
            cmbGroup.SelectedIndex = 0;

            // === Buttons ===
            var btnAdd = new Button
            {
                Text = "Add",
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 35),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Location = new Point(80, this.ClientSize.Height - 60)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.DialogResult = DialogResult.OK;
            btnAdd.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtWord1.Text) || string.IsNullOrWhiteSpace(txtWord2.Text))
                {
                    MessageBox.Show("Both word fields must be filled in.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cmbGroup.SelectedItem == null)
                {
                    MessageBox.Show("Please choose a group.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Сохраняем в SQLite
                using (var conn = new SQLiteConnection("Data Source=words.db;Version=3;"))
                {
                    var Allgroups = SqliteFunc.GetAllGroups();
                    int groupid = -1;
                    for(int i = 0; i < Allgroups.Count; i++)
					{
                        if(Allgroups[i].Name == cmbGroup.SelectedItem.ToString())
						{
                            groupid = Allgroups[i].Id;
                        }
					}
                    if(groupid == -1)
					{
                        SqliteFunc.AddGroup(cmbGroup.SelectedItem.ToString());
					}
                    int w1id = SqliteFunc.InsertWordAndReturnId(txtWord1.Text.Trim(), cmbLang1.SelectedItem.ToString());
                    int w2id = SqliteFunc.InsertWordAndReturnId(txtWord2.Text.Trim(), cmbLang2.SelectedItem.ToString());
                    SqliteFunc.InsertWordPair(w1id, w2id, groupid);
                    MessageBox.Show("Word added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            };


            var btnCancel = new Button
            {
                Text = "Cancel",
                BackColor = Color.FromArgb(120, 150, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 35),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 180, this.ClientSize.Height - 60)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.DialogResult = DialogResult.Cancel;

            // Обновлять позиции кнопок при изменении размера формы
            this.Resize += (s, e) =>
            {
                btnAdd.Location = new Point(80, this.ClientSize.Height - 60);
                btnCancel.Location = new Point(this.ClientSize.Width - 180, this.ClientSize.Height - 60);
            };

            this.Controls.AddRange(new Control[]
            {
        lblWord1, txtWord1, cmbLang1,
        lblWord2, txtWord2, cmbLang2,
        lblGroup, cmbGroup,
        btnAdd, btnCancel
            });
        }


        private void AddWordForm_Load(object sender, EventArgs e)
        {

        }
    }
}
