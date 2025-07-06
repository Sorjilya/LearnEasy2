using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LearnEasy2
{
	public partial class DeleteWordForm : Form
	{
        public string Word1 => txtWord1.Text.Trim();

        private TextBox txtWord1;

        public DeleteWordForm()
        {
            InitializeComponent();
            InitializeFields();
        }

        private void InitializeFields()
        {
            this.Text = "Delete Word";
            this.Size = new Size(400, 130);
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
                Location = new Point(160, 20),
                Size = new Size(200, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };


            // === Buttons ===
            var btnAdd = new Button
            {
                Text = "Delete",
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
                if (string.IsNullOrWhiteSpace(txtWord1.Text))
                {
                    MessageBox.Show("Field must be filled in.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var words = SqliteFunc.GetAllWords();
                foreach (var word in words)
				{
                    if(word.Word == txtWord1.Text)
                    {
                        SqliteFunc.DeleteWordById(word.Id);
                    }
				}
                MessageBox.Show("Group added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


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
        lblWord1, txtWord1,
        btnAdd, btnCancel
            });
        }

        private void DeleteWordForm_Load(object sender, EventArgs e)
		{

		}
	}
}
