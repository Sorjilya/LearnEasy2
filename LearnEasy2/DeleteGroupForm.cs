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
	public partial class DeleteGroupForm : Form
	{
        public string Group => cmbGroup.SelectedItem?.ToString();
        private ComboBox cmbGroup;
        public DeleteGroupForm(List<GroupEntry> groups)
        {
            InitializeComponent();
            InitializeFields(groups);
        }

        private void InitializeFields(List<GroupEntry> groups)
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

            cmbGroup = new ComboBox
            {
                Location = new Point(120, 15),
                Size = new Size(260, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            List<string> grNames = new List<string>();
            for(int i = 0; i < groups.Count; i++)
			{
                grNames.Add(groups[i].Name);
			}  
            cmbGroup.Items.AddRange(grNames.ToArray());
            cmbGroup.SelectedIndex = 0;


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
                int groupid = 0;
                foreach (var gr in groups)
                {
                    if (gr.Name == cmbGroup.Text)
                    {
                        groupid = gr.Id;
                    }
                }
                SqliteFunc.DeleteGroupById(groupid);
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
        lblWord1, cmbGroup,
        btnAdd, btnCancel
            });
        }
        private void DeleteGroupForm_Load(object sender, EventArgs e)
		{

		}
	}
}
