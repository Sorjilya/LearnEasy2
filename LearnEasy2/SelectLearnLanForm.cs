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
	public partial class SelectLearnLanForm : Form
	{
            public string SelectedFrom { get; private set; }
            public string SelectedTo { get; private set; }

            private ComboBox cmbFrom;
            private ComboBox cmbTo;
            private Button btnChange;
            private Button btnCancel;

            public SelectLearnLanForm(string currentFrom, string currentTo)
            {
                this.Text = "Select Languages";
                this.Size = new Size(350, 200);
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackColor = Color.FromArgb(204, 255, 204);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                Label lblFrom = new Label
                {
                    Text = "Choose language From:",
                    Location = new Point(20, 20),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10)
                };
                Controls.Add(lblFrom);

                cmbFrom = new ComboBox
                {
                    Location = new Point(200, 18),
                    Size = new Size(100, 25),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 10)
                };
                cmbFrom.Items.AddRange(new[] { "ru", "eng", "es", "fr" });
                cmbFrom.SelectedItem = currentFrom;
                Controls.Add(cmbFrom);

                Label lblTo = new Label
                {
                    Text = "Choose language To:",
                    Location = new Point(20, 60),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10)
                };
                Controls.Add(lblTo);

                cmbTo = new ComboBox
                {
                    Location = new Point(200, 58),
                    Size = new Size(100, 25),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 10)
                };
                cmbTo.Items.AddRange(new[] { "ru", "eng", "es", "fr" });
                cmbTo.SelectedItem = currentTo;
                Controls.Add(cmbTo);

                btnChange = new Button
                {
                    Text = "Change",
                    DialogResult = DialogResult.OK,
                    Location = new Point(60, 110),
                    Size = new Size(100, 35),
                    BackColor = Color.FromArgb(33, 150, 83),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };
                btnChange.FlatAppearance.BorderSize = 0;
                btnChange.Click += (s, e) =>
                {
                    SelectedFrom = cmbFrom.SelectedItem.ToString();
                    SelectedTo = cmbTo.SelectedItem.ToString();
                };
                Controls.Add(btnChange);

                btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(180, 110),
                    Size = new Size(100, 35),
                    BackColor = Color.FromArgb(33, 150, 83),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };
                btnCancel.FlatAppearance.BorderSize = 0;
                Controls.Add(btnCancel);

                AcceptButton = btnChange;
                CancelButton = btnCancel;
            }
    }
}

