using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using OxyPlot.Axes;
using OxyPlot;

namespace LearnEasy2
{
    public partial class Form1 : Form
    {
        string Lanfrom = "ru";
        string Lanto = "eng";
        int curPoints = 0;
        int prTime = 0;
        int Gamenow = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnWords_Click(object sender, EventArgs e)
        {
            ShowPanel(panelWords);
            InitWordPanel();
        }

        private void btnGames_Click(object sender, EventArgs e)
        {
            ShowPanel(panelGames);
            InitGamesPanel();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowPanel(panelSettings);
            InitSettingsPanel();
        }
        private void btnMatches_Click(object sender, EventArgs e)
        {
            ShowPanel(panelGameMatches);
            InitGameMatchesPanel();
        }
        private void btnCards_Click(object sender, EventArgs e)
        {
            ShowPanel(panelGameCards);
            InitGameCardsPanel();
        }
        private void ShowPanel(Panel panelToShow)
        {
            panelWords.Visible = false;
            panelGames.Visible = false;
            panelSettings.Visible = false;
            panelGameMatches.Visible = false;

            panelToShow.Visible = true;
            panelToShow.BringToFront();
        }
        public void LoadWordsForGroup(string groupName, DataGridView dataGrid, ComboBox groupCombo)
        {
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();
            var grEnt = SqliteFunc.GetAllGroups();
            int geId = 0;
            foreach (var group in grEnt)
			{
                if(group.Name == groupName)
				{
                    geId = group.Id;
                    break;
				}
			}
            dataGrid.Columns.Add("Word1", "Word");
            dataGrid.Columns.Add("Word2", "Translation");
            List<WordEntry> wordsfrom = new List<WordEntry>();
            List<WordEntry> wordsto = new List<WordEntry>();
            var wordsFromGroups = SqliteFunc.GetAllGroupWords(geId, Lanfrom, Lanto);
            for(int i = 0; i < wordsFromGroups.Count; i ++)
            {
				dataGrid.Rows.Add(wordsFromGroups[i].Item1, wordsFromGroups[i].Item2);
            }
        }
        private void InitStatisticsPanel()
        {
            panelSettings.Controls.Clear();

            int spacing = 30;
            int btnWidth = 350;
            int btnHeight = 60;
            int startY = 80;

            string[] statButtons = { "Word Statistics", "Matches Statistics", "Cards Statistics", "SpellCheck Statistics" };

            for (int i = 0; i < statButtons.Length; i++)
            {
                var btn = new Button
                {
                    Text = statButtons[i],
                    Size = new Size(btnWidth, btnHeight),
                    Location = new Point((panelSettings.Width - btnWidth) / 2, startY + i * (btnHeight + spacing)),
                    BackColor = Color.FromArgb(33, 150, 83),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    Anchor = AnchorStyles.Top
                };
                btn.FlatAppearance.BorderSize = 0;

                // Пример обработчиков кликов (можно заменить по нужному функционалу)
                if (i == 0)
                {
                    btn.Click += (s, e) =>
                    {
                        InitWordStatisticsPanel();
                    };
                }
                if(i == 1)
				{
                    btn.Click += (s, e) =>
                    {
                        InitMatchesStatisticsPanel();
                    };
                }
                if (i == 2)
                {
                    btn.Click += (s, e) =>
                    {
                        InitCardsStatisticsPanel();
                    };
                }

                panelSettings.Controls.Add(btn);
            }
        }

        private void InitSettingsPanel()
        {
            panelSettings.Controls.Clear();

            int spacing = 30;
            int btnWidth = 350;
            int btnHeight = 70;
            int startY = 100;
            // === Кнопка Language ===
            var btnLanguage = new Button
            {
                Text = "Language",
                Size = new Size(btnWidth, btnHeight),
                Location = new Point((panelSettings.Width - btnWidth) / 2, startY),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Anchor = AnchorStyles.Top
            };
            btnLanguage.FlatAppearance.BorderSize = 0;
            btnLanguage.Click += (s, e) =>
            {
                GamesFunc.GenerateWordVariations("Hello", 10);
            };
            panelSettings.Controls.Add(btnLanguage);

            // === Кнопка Statistics ===
            var btnStats = new Button
            {
                Text = "Statistics",
                Size = new Size(btnWidth, btnHeight),
                Location = new Point((panelSettings.Width - btnWidth) / 2, startY + btnHeight + spacing),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Anchor = AnchorStyles.Top
            };
            btnStats.FlatAppearance.BorderSize = 0;
            btnStats.Click += (s, e) =>
            {
                InitStatisticsPanel();
            };

            panelSettings.Controls.Add(btnStats);

            // === Кнопка Learning language ===
            var btnLearnLang = new Button
            {
                Text = "Learning language",
                Size = new Size(btnWidth, btnHeight),
                Location = new Point((panelSettings.Width - btnWidth) / 2, startY + 2 * (btnHeight + spacing)),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Anchor = AnchorStyles.Top
            };
            btnLearnLang.FlatAppearance.BorderSize = 0;
            btnLearnLang.Click += (s, e) =>
            {
                var langForm = new SelectLearnLanForm(Lanfrom, Lanto);
                if (langForm.ShowDialog() == DialogResult.OK)
                {
                    Lanfrom = langForm.SelectedFrom;
                    Lanto = langForm.SelectedTo;
                    /*MessageBox.Show($"Язык изучения: {Lanfrom} → {Lanto}");*/
                }

            };
            panelSettings.Controls.Add(btnLearnLang);
        }
        private void InitWordPanel()
        {
            panelWords.Controls.Clear();
            int padding = 20;
            int spacing = 10;

            // === DataGridView ===
            var dataGrid = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(panelWords.Width - 40, panelWords.Height - 150),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                RowHeadersVisible = false,
                ColumnCount = 2,
                AllowUserToAddRows = false,
                ReadOnly = false
            };

            dataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dataGrid.Columns[0].Name = "Word";
            dataGrid.Columns[1].Name = "Translation";
            dataGrid.ReadOnly = true;   
            dataGrid.MultiSelect = false;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.RowHeadersVisible = false; 
            dataGrid.ClearSelection();

            dataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGrid.SelectionChanged += (s, e) => dataGrid.ClearSelection();

            panelWords.Controls.Add(dataGrid);

            int controlsTop = dataGrid.Bottom + spacing;
            // === ComboBox для выбора группы ===
            var groupCombo = new ComboBox
            {
                Location = new Point(padding, controlsTop),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            var groups = SqliteFunc.GetGroupNames();
            if (groups.Count == 0)
            {
                SqliteFunc.AddGroup("Group 1");
                groups.Add("Group 1");
            }
            groupCombo.Items.AddRange(groups.ToArray());
            groupCombo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            groupCombo.SelectedIndex = 0;
            groupCombo.SelectedIndexChanged += (s, e) =>
            {
                if (groupCombo.SelectedItem != null)
                {
                    string selectedGroup = groupCombo.SelectedItem.ToString();
                    LoadWordsForGroup(selectedGroup, dataGrid, groupCombo);
                }
            };
            LoadWordsForGroup(groupCombo.SelectedItem.ToString(), dataGrid, groupCombo);

            panelWords.Controls.Add(groupCombo);
            // === Кнопка Add Word ===
            var btnAddWord = new Button
            {
                Text = "Add Word",
                Location = new Point(groupCombo.Right + spacing, controlsTop),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAddWord.FlatAppearance.BorderSize = 0;
            btnAddWord.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            panelWords.Controls.Add(btnAddWord);
            btnAddWord.Click += (s, e) =>
            {
                
                groups = SqliteFunc.GetGroupNames();
                if(groups.Count == 0)
				{
                    groups.Add("Group 1");
				}
                var addForm = new AddWordForm(groups);

                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    string word1 = addForm.Word1;
                    string word2 = addForm.Word2;
                    string lang1 = addForm.Lang1;
                    string lang2 = addForm.Lang2;
                    string group = addForm.Group;

                }
                LoadWordsForGroup(groupCombo.SelectedItem.ToString(), dataGrid, groupCombo);
            };


            // === Кнопка Delete Word ===
            var btnDeleteWord = new Button
            {
                Text = "Delete Word",
                Location = new Point(btnAddWord.Right + spacing, controlsTop),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnDeleteWord.FlatAppearance.BorderSize = 0;
            btnDeleteWord.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteWord.FlatAppearance.BorderSize = 0;
            btnDeleteWord.Click += (s, e) =>
            {
                var deleteForm = new DeleteWordForm();

                if (deleteForm.ShowDialog() == DialogResult.OK)
                {
                    string word1 = deleteForm.Word1;

                }
                LoadWordsForGroup(groupCombo.SelectedItem.ToString(), dataGrid, groupCombo);
            };
            panelWords.Controls.Add(btnDeleteWord);

            // === Кнопка Add Group ===
            var btnAddGroup = new Button
            {
                Text = "Add Group",
                Location = new Point(btnDeleteWord.Right + spacing, controlsTop),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAddGroup.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btnAddGroup.FlatAppearance.BorderSize = 0;
            btnAddGroup.Click += (s, e) =>
            {
                var addForm = new AddGroupForm();

                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    string word1 = addForm.Word1;
                }
                LoadWordsForGroup(groupCombo.SelectedItem.ToString(), dataGrid, groupCombo);
                InitWordPanel();
            };
            panelWords.Controls.Add(btnAddGroup);
            // === Кнопка Delete Group ===
            var btnDeleteGroup = new Button
            {
                Text = "Delete Group",
                Location = new Point(btnAddGroup.Right + spacing, controlsTop),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnDeleteGroup.FlatAppearance.BorderSize = 0;
            btnDeleteGroup.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btnDeleteGroup.FlatAppearance.BorderSize = 0;
            btnDeleteGroup.Click += (s, e) =>
            {
                var groupEn = SqliteFunc.GetAllGroups();
                if (groups.Count == 0)
                {
                    groups.Add("Group 1");
                }
                var deleteForm = new DeleteGroupForm(groupEn);

                if (deleteForm.ShowDialog() == DialogResult.OK)
                {
                    //string word1 = deleteForm.Word1;

                }
                LoadWordsForGroup(groupCombo.SelectedItem.ToString(), dataGrid, groupCombo);
                InitWordPanel();
            };
            panelWords.Controls.Add(btnDeleteGroup);

            
        }
        private void InitGamesPanel()
        {
            panelGames.Controls.Clear();

            int spacing = 30;
            int btnWidth = 350;
            int btnHeight = 70;
            int startY = 100;
            var btnMatches = new Button
            {
                Text = "Matches",
                Size = new Size(btnWidth, btnHeight),
                Location = new Point((panelGames.Width - btnWidth) / 2, startY),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Anchor = AnchorStyles.Top
            };
            btnMatches.FlatAppearance.BorderSize = 0;
            btnMatches.Click += new System.EventHandler(this.btnMatches_Click);
            panelGames.Controls.Add(btnMatches);

            var btnCards = new Button
            {
                Text = "Cards",
                Size = new Size(btnWidth, btnHeight),
                Location = new Point((panelGames.Width - btnWidth) / 2, startY + btnHeight + spacing),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Anchor = AnchorStyles.Top
            };
            btnCards.FlatAppearance.BorderSize = 0;
            btnCards.Click += new System.EventHandler(this.btnCards_Click);
            panelGames.Controls.Add(btnCards);

            var btnSpellCheck= new Button
            {
                Text = "SpellCheck",
                Size = new Size(btnWidth, btnHeight),
                Location = new Point((panelGames.Width - btnWidth) / 2, startY + 2 * (btnHeight + spacing)),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Anchor = AnchorStyles.Top
            };
            btnSpellCheck.FlatAppearance.BorderSize = 0;
            btnSpellCheck.Click += (s, e) =>
            {
                var langForm = new SelectLearnLanForm(Lanfrom, Lanto);
                if (langForm.ShowDialog() == DialogResult.OK)
                {
                    Lanfrom = langForm.SelectedFrom;
                    Lanto = langForm.SelectedTo;
                    /*MessageBox.Show($"Язык изучения: {Lanfrom} → {Lanto}");*/
                }

            };
            panelGames.Controls.Add(btnSpellCheck);
        }
        private void InitGameMatchesPanel()
        {
            panelGameMatches.Controls.Clear();
            panelGameMatches.BackColor = Color.FromArgb(204, 255, 204); // светло-зелёный фон
            Label lblFrom = new Label
            {
                Text = "Group:",
                Location = new Point(100, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold)
            };
            panelGameMatches.Controls.Add(lblFrom);

            ComboBox cmbGroup = new ComboBox
            {
                Location = new Point(300, 180),
                Size = new Size(170, 40),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 13)
            };
            var groups = SqliteFunc.GetAllGroups();
            List<string> groupNamesForMatches = new List<string>();
            for (int i = 0; i < groups.Count; i++)
			{
                if(SqliteFunc.GetWordPairCountByGroup(groups[i].Id) > 2)
				{
                    groupNamesForMatches.Add(groups[i].Name);
				}
			}
            cmbGroup.Items.AddRange(groupNamesForMatches.ToArray());
            cmbGroup.SelectedIndex = 0;
            panelGameMatches.Controls.Add(cmbGroup);

            Label lblTo = new Label
            {
                Text = "Difficulty:",
                Location = new Point(100, 130),
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold)
            };
            panelGameMatches.Controls.Add(lblTo);

            ComboBox cmbDif = new ComboBox
            {
                Location = new Point(300, 130),
                Size = new Size(170, 40),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 13)
            };
            cmbDif.Items.AddRange(new[] { "Easy", "Medium", "Hard"});
            cmbDif.SelectedIndex = 0;
            panelGameMatches.Controls.Add(cmbDif);

            Button btnChange = new Button
            {
                Text = "Start",
                DialogResult = DialogResult.OK,
                Location = new Point(150, 300),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnChange.FlatAppearance.BorderSize = 0;
            btnChange.Click += (s, e) =>
            {
                var gr = SqliteFunc.GetAllGroups();
                int groupid = -1;
                for(int i = 0; i < gr.Count; i++)
				{
                    if(gr[i].Name == cmbGroup.SelectedItem.ToString())
					{
                        groupid = gr[i].Id;
                    }
				}
                StartGameMatches(cmbDif.SelectedIndex, groupid);
            };
            panelGameMatches.Controls.Add(btnChange);

            Button btnCancel = new Button
            {
                Text = "Exit",
                DialogResult = DialogResult.Cancel,
                Location = new Point(400, 300),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                ShowPanel(panelGames);
                InitGamesPanel();
            };
            panelGameMatches.Controls.Add(btnCancel);
        }
        private void InitGameCardsPanel()
        {
            panelGameCards.Controls.Clear();
            panelGameCards.BackColor = Color.FromArgb(204, 255, 204); // светло-зелёный фон
            Label lblFrom = new Label
            {
                Text = "Group:",
                Location = new Point(100, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold)
            };
            panelGameCards.Controls.Add(lblFrom);

            ComboBox cmbGroup = new ComboBox
            {
                Location = new Point(300, 180),
                Size = new Size(170, 40),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 13)
            };
            var groups = SqliteFunc.GetAllGroups();
            List<string> groupNamesForCards = new List<string>();
            for (int i = 0; i < groups.Count; i++)
            {
                if (SqliteFunc.GetWordPairCountByGroup(groups[i].Id) > 0)
                {
                    groupNamesForCards.Add(groups[i].Name);
                }
            }
            cmbGroup.Items.AddRange(groupNamesForCards.ToArray());
            cmbGroup.SelectedIndex = 0;
            panelGameCards.Controls.Add(cmbGroup);

            Label lblDif = new Label
            {
                Text = "Difficulty:",
                Location = new Point(100, 130),
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold)
            };
            panelGameCards.Controls.Add(lblDif);

            ComboBox cmbDif = new ComboBox
            {
                Location = new Point(300, 130),
                Size = new Size(170, 40),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 13)
            };
            cmbDif.Items.AddRange(new[] { "Easy", "Medium", "Hard" });
            cmbDif.SelectedIndex = 0;
            panelGameCards.Controls.Add(cmbDif);

            Button btnStart = new Button
            {
                Text = "Start",
                DialogResult = DialogResult.OK,
                Location = new Point(150, 300),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += (s, e) =>
            {
                var gr = SqliteFunc.GetAllGroups();
                int groupid = -1;
                for (int i = 0; i < gr.Count; i++)
                {
                    if (gr[i].Name == cmbGroup.SelectedItem.ToString())
                    {
                        groupid = gr[i].Id;
                    }
                }
                GameCardsRound(cmbDif.SelectedIndex, groupid);
            };
            panelGameCards.Controls.Add(btnStart);

            Button btnCancel = new Button
            {
                Text = "Exit",
                DialogResult = DialogResult.Cancel,
                Location = new Point(400, 300),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                ShowPanel(panelGames);
                InitGamesPanel();
            };
            panelGameCards.Controls.Add(btnCancel);
        }
        private void Make_Button_Clicked(Button TButton, int clicked = -1)
        {
            if (clicked != 0 && clicked != 1)
            {
                if (!Is_MatchButton_Clicked(TButton))
                {
                    TButton.BackColor = Color.FromArgb(45, 100, 105);
                }
                else
                {
                    TButton.BackColor = Color.FromArgb(33, 150, 83);
                }
            }
			else
			{
                if (clicked == 1)
                {
                    TButton.BackColor = Color.FromArgb(45, 100, 105);
                }
                else
                {
                    TButton.BackColor = Color.FromArgb(33, 150, 83);
                }
            }
        }
        private void CardsEndGame()
        {
            prTime = 0;
            Gamenow = -1;
            timer1.Enabled = false;
            SqliteFunc.InsertGameResult("Cards", curPoints);
            int padding = 20;
            int spacing = 30;
            int btnWidth = 350;
            int btnHeight = 70;
            int startY = 200;
            panelGameCards.Controls.Clear();
            panelGameCards.BackColor = Color.FromArgb(204, 255, 204); // светло-зелёный фон
            int maxScore = SqliteFunc.GetMaxGameScore("Cards");
            Label lblScore = new Label
            {
                Text = "Your score: " + curPoints.ToString() + "\nMax Score: " + maxScore.ToString(),
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblScore.Location = new Point((panelGameCards.Width - lblScore.Width - padding) / 2, 100);
            panelGameCards.Controls.Add(lblScore);
            var btnOk = new Button
            {
                Text = "Ok",
                Size = new Size(btnWidth, btnHeight),
                Location = new Point((panelGameCards.Width - btnWidth) / 2, startY + btnHeight + spacing),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Anchor = AnchorStyles.Top
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) =>
            {
                ShowPanel(panelGames);
                InitGamesPanel();
            };
            panelGameCards.Controls.Add(btnOk);
            curPoints = 0;
        }
        private void MatchesEndGame()
        {
            prTime = 0;
            Gamenow = -1;
            timer1.Enabled = false;
            SqliteFunc.InsertGameResult("Matches", curPoints);
            int padding = 20;
            int spacing = 30;
            int btnWidth = 350;
            int btnHeight = 70;
            int startY = 200;
            panelGameMatches.Controls.Clear();
            panelGameMatches.BackColor = Color.FromArgb(204, 255, 204); // светло-зелёный фон
            int maxScore = SqliteFunc.GetMaxGameScore("Matches");
            Label lblScore = new Label
            {
                Text = "Your score: " + curPoints.ToString() + "\nMax Score: " + maxScore.ToString(),
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblScore.Location = new Point((panelGameMatches.Width - lblScore.Width - padding) / 2, 100);
            panelGameMatches.Controls.Add(lblScore);
            var btnOk = new Button
            {
                Text = "Ok",
                Size = new Size(btnWidth, btnHeight),
                Location = new Point((panelGameMatches.Width - btnWidth) / 2, startY + btnHeight + spacing),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Anchor = AnchorStyles.Top
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) =>
            {
                ShowPanel(panelGames);
                InitGamesPanel();
            };
            panelGameMatches.Controls.Add(btnOk);
            curPoints = 0;
        }
        private bool Is_MatchButton_Clicked(Button TButton)
        {
            if (TButton.BackColor == Color.FromArgb(33, 150, 83) || !TButton.Enabled)
            {
                return false;
            }
            return true;
        }
        private bool IsEndOfTheMatchesRound(List<Button> TButtons)
        {
            for(int i = 0; i < TButtons.Count; i++)
			{
                if(TButtons[i].Enabled == true)
				{
                    return false;
				}
			}
            return true;
        }
        private void MatchButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            MatchesButtonContext context = clickedButton.Tag as MatchesButtonContext;

            var left = context.LeftButtons;
            var right = context.RightButtons;
            int clickedInd = -1;
            Make_Button_Clicked(clickedButton);
            if (context.MyIndex > 2)
            {
                foreach (var btn in right)
                {
                    if(btn.Text != clickedButton.Text)
                        Make_Button_Clicked(btn, 0);
                }
                for(int i = 0; i < left.Count; i ++)
                {
                    if (Is_MatchButton_Clicked(left[i]))
                        clickedInd = i;
                }
                if (clickedInd != -1)
                {
                    bool Isright = SqliteFunc.AreWordsPaired(left[clickedInd].Text, clickedButton.Text);
					if (Isright)
                    {
                        SqliteFunc.UpdatePairUsage(left[clickedInd].Text, true);
                        if (context.difficulty == 0) curPoints += 1;
                        if (context.difficulty == 1) curPoints += 5;
                        if (context.difficulty == 2) curPoints += 10;
                        context.lblPoints.Text = "Points: "+ curPoints + "\nMaximum Points: " + SqliteFunc.GetMaxGameScore("Matches");
                        left[clickedInd].Enabled = false;
                        clickedButton.Enabled = false;
                        left[clickedInd].Hide();
                        clickedButton.Hide();
						if (IsEndOfTheMatchesRound(left))
						{
                            CreateMatchesRound(context.difficulty, context.group, left, right, context.lblPoints, context.roundTimer);

                        }
                    }
					else
                    {
                        SqliteFunc.UpdatePairUsage(left[clickedInd].Text, false);
                        MatchesEndGame();
					}
                }
            }
			else
			{
                foreach (var btn in left)
                {
                    if (btn.Text != clickedButton.Text)
                        Make_Button_Clicked(btn, 0);
                }
                for (int i = 0; i < right.Count; i++)
                {
                    if (Is_MatchButton_Clicked(right[i]))
                        clickedInd = i;
                }
                if (clickedInd != -1)
                {
                    bool Isright = SqliteFunc.AreWordsPaired(right[clickedInd].Text, clickedButton.Text);
                    if (Isright)
                    {
                        SqliteFunc.UpdatePairUsage(right[clickedInd].Text, true);
                        if (context.difficulty == 0) curPoints += 1;
                        if (context.difficulty == 1) curPoints += 5;
                        if (context.difficulty == 2) curPoints += 10;
                        context.lblPoints.Text = "Points: " + curPoints + "\nMaximum Points: " + SqliteFunc.GetMaxGameScore("Matches");
                        right[clickedInd].Enabled = false;
                        clickedButton.Enabled = false;
                        right[clickedInd].Hide();
                        clickedButton.Hide();
                        if (IsEndOfTheMatchesRound(left))
                        {
                            CreateMatchesRound(context.difficulty, context.group, left, right, context.lblPoints, context.roundTimer);

                        }
                    }
					else
                    {
                        SqliteFunc.UpdatePairUsage(right[clickedInd].Text, false);
                        MatchesEndGame();
                    }
                }
            }
        }
        private void CreateMatchesRound(int difficulty, int group, List<Button> leftButtons, List<Button> rightButtons, Label lblPoints, Timer timer)
        {
            progressBar1.Location = new Point(100, 30);
            progressBar1.Size = new Size(panelGameMatches.Width - 200, 30);
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Visible = true;
            if (difficulty == 0)
			{
                progressBar1.Maximum = 2000;
            }
            if (difficulty == 1)
            {
                progressBar1.Maximum = 400;
            }
            if (difficulty == 2)
            {
                progressBar1.Maximum = 200;
            }
            progressBar1.Value = progressBar1.Maximum;
            timer.Enabled = true;
            progressBar1.Value = progressBar1.Maximum;
            panelGameMatches.Controls.Add(progressBar1);
            var words = SqliteFunc.GetAllGroupWords(group, Lanfrom, Lanto);
            int[] randomWordsId = GamesFunc.GenerateRandomIntVector(3, 0, words.Count);
            int[] randomWordsRasp = GamesFunc.GenerateRandomIntVector(2, 0, 20);
            List<string> wordsFrom = new List<string>();
            List<string> wordsTo = new List<string>();
            for(int i = 0; i < randomWordsId.Count(); i++)
			{
                wordsFrom.Add(words[randomWordsId[i]].Item1);
                wordsTo.Add(words[randomWordsId[i]].Item2);
            }
            for (int i = 0; i < randomWordsRasp.Count(); i++)
            {
                string temp;
                temp = wordsFrom[i];
                wordsFrom[i] = wordsFrom[randomWordsRasp[i]%3];
                wordsFrom[randomWordsRasp[i] % 3] = temp;
            }
            for (int i = 0; i < leftButtons.Count; i++)
            {
                leftButtons[i].Text = wordsFrom[i];
                leftButtons[i].Enabled = true;
                leftButtons[i].Show();
                Make_Button_Clicked(leftButtons[i], 0);
                rightButtons[i].Text = wordsTo[i];
                rightButtons[i].Enabled = true;
                rightButtons[i].Show();
                Make_Button_Clicked(rightButtons[i] , 0);
            }
        }
        private void StartGameMatches(int difficulty, int group)
        {
            panelGameMatches.Controls.Clear();
            panelGameMatches.BackColor = Color.FromArgb(204, 255, 204); // светло-зелёный фон

            

            Font btnFont = new Font("Segoe UI", 11F, FontStyle.Bold);
            Color btnBack = Color.FromArgb(33, 150, 83);
            Color btnFore = Color.White;

            int spacingX = 40;
            int spacingY = 20;
            int buttonWidth = (panelGameMatches.Width - 3 * spacingX) / 2;
            int buttonHeight = 60;

            List<Button> leftButtons = new List<Button>();
            List<Button> rightButtons = new List<Button>();

            // Левая колонка
            for (int i = 0; i < 3; i++)
            {
                Button btn = new Button
                {
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(spacingX, 100 + i * (buttonHeight + spacingY)),
                    BackColor = btnBack,
                    ForeColor = btnFore,
                    FlatStyle = FlatStyle.Flat,
                    Font = btnFont
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += new System.EventHandler(MatchButton_Click);

                panelGameMatches.Controls.Add(btn);
                leftButtons.Add(btn);
            }

            // Правая колонка
            for (int i = 0; i < 3; i++)
            {
                Button btn = new Button
                {
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(spacingX * 2 + buttonWidth, 100 + i * (buttonHeight + spacingY)),
                    BackColor = btnBack,
                    ForeColor = btnFore,
                    FlatStyle = FlatStyle.Flat,
                    Font = btnFont
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += new System.EventHandler(MatchButton_Click);

                panelGameMatches.Controls.Add(btn);
                rightButtons.Add(btn);
            }
            // Очки
            Label lblPoints = new Label
            {
                Text = "Points: " + curPoints + "\nMaximum Points: " + SqliteFunc.GetMaxGameScore("Matches"),
                Location = new Point((panelGames.Width - 200) / 2, panelGames.Height - 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelGameMatches.Controls.Add(lblPoints);

            int MyIndex = 0;
            foreach (var btn in leftButtons.Concat(rightButtons))
            {

                var context = new MatchesButtonContext { LeftButtons = leftButtons, RightButtons = rightButtons, MyIndex = MyIndex, lblPoints = lblPoints, difficulty = difficulty, group = group, roundTimer = timer1};
                btn.Tag = context;
                MyIndex++;
            }
            Gamenow = 1;
            CreateMatchesRound(difficulty, group, leftButtons, rightButtons, lblPoints, timer1);
        }
        private void GameCardsRound(int difficulty, int group)
        {
            Gamenow = 2;
            panelGameCards.Controls.Clear();
            panelGameCards.BackColor = Color.FromArgb(204, 255, 204); progressBar1.Location = new Point(100, 30);
            progressBar1.Size = new Size(panelGameMatches.Width - 200, 30);
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Visible = true;
            if (difficulty == 0)
            {
                progressBar1.Maximum = 1500;
            }
            if (difficulty == 1)
            {
                progressBar1.Maximum = 800;
            }
            if (difficulty == 2)
            {
                progressBar1.Maximum = 400;
            }
            progressBar1.Value = progressBar1.Maximum;
            timer1.Enabled = true;
            progressBar1.Value = progressBar1.Maximum;
            panelGameCards.Controls.Add(progressBar1);
            var words = SqliteFunc.GetAllGroupWords(group, Lanfrom, Lanto);
            var id = GamesFunc.GenerateRandomIntVector(1, 0, words.Count);
            var word = words[id[0]];

            // === Label со словом ===
            Label lblWord = new Label
            {
                Text = word.Item1,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(300, 80),
                Location = new Point((panelGameCards.Width - 300) / 2, 150),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(33, 150, 83)
            };
            panelGameCards.Controls.Add(lblWord);

            // === Текстовое поле ===
            TextBox txtInput = new TextBox
            {
                Font = new Font("Segoe UI", 16F),
                Size = new Size(300, 40),
                Location = new Point((panelGameCards.Width - 420) / 2, lblWord.Bottom + 70)
            };
            panelGameCards.Controls.Add(txtInput);

            // === Кнопка Confirm ===
            Button btnConfirm = new Button
            {
                Text = "Confirm",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(120, 40),
                Location = new Point(txtInput.Right + 10, txtInput.Top),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            panelGameCards.Controls.Add(btnConfirm);

            // === Очки ===
            Label lblPoints = new Label
            {
                Text = "Points: " + curPoints + "\nMaximum Points: " + SqliteFunc.GetMaxGameScore("Cards"),
                AutoSize = true,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Location = new Point((panelGameCards.Width - 200) / 2, txtInput.Bottom + 70),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelGameCards.Controls.Add(lblPoints);

            // TODO: Подключить обработку нажатия
            btnConfirm.Click += (s, e) =>
            {
                string userAnswer = txtInput.Text.Trim().ToLower();
                string correctAnswer = word.Item2.ToLower();

                if (userAnswer == correctAnswer)
                {
                    if (difficulty == 0) curPoints += 1;
                    if (difficulty == 1) curPoints += 5;
                    if (difficulty == 2) curPoints += 10;
                    GameCardsRound(difficulty, group);
                }
                else
                {
                    CardsEndGame();
                }
            };
        }

        private void InitWordStatisticsPanel()
        {
            panelSettings.Controls.Clear();

            int padding = 20;
            int spacing = 30;
            int labelHeight = 30;
            int startY = 60;
            int labelWidth = panelSettings.Width - 2 * padding;

            // Заголовок
            var lblTitle = new Label
            {
                Text = "Top 10 Problematic Words",
                Location = new Point(padding, startY),
                Size = new Size(labelWidth, labelHeight),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelSettings.Controls.Add(lblTitle);

            // Получение статистики слов
            var pairs = SqliteFunc.GetAllWordPairsWithStats(); // (WordA, WordB, correct, total)
            var topProblems = pairs
                .Where(p => p.Total > 0)
                .OrderBy(p => (double)p.Correct / p.Total)
                .Take(10)
                .ToList();

            int currentY = startY + labelHeight + spacing;

            foreach (var pair in topProblems)
            {
                var label = new Label
                {
                    Text = $"{pair.WordA} ↔ {pair.WordB}  —  {pair.Correct}/{pair.Total} correct",
                    Location = new Point(padding, currentY),
                    Size = new Size(labelWidth, labelHeight),
                    Font = new Font("Segoe UI", 13, FontStyle.Bold),
                    ForeColor = Color.DarkRed,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                panelSettings.Controls.Add(label);
                currentY += labelHeight + 5;
            }

            // Кнопка Назад
            var btnBack = new Button
            {
                Text = "Back",
                Size = new Size(120, 40),
                Location = new Point((panelSettings.Width - 120) / 2, currentY + 20),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, e) => InitStatisticsPanel();
            panelSettings.Controls.Add(btnBack);
        }
        private void InitMatchesStatisticsPanel()
        {
            panelSettings.Controls.Clear();
            panelSettings.BackColor = Color.FromArgb(204, 255, 204);

            // === Заголовок ===
            var title = new Label
            {
                Text = "Matches Statistics",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point((panelSettings.Width - 300) / 2, 20)
            };
            panelSettings.Controls.Add(title);

            // === Комбо выбора периода ===
            var cmbPeriod = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(50, 70),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPeriod.Items.AddRange(new string[] { "Last 7 Days", "Last 30 Days", "All Time" });
            cmbPeriod.SelectedIndex = 0;
            panelSettings.Controls.Add(cmbPeriod);

            // === Переключатель типа графика ===
            var chkViewMode = new CheckBox
            {
                Text = "Show Games per Day",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(50, 110),
                Checked = false
            };
            panelSettings.Controls.Add(chkViewMode);

            // === Кнопка Назад ===
            var btnBack = new Button
            {
                Text = "Back",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(100, 35),
                Location = new Point(50, panelSettings.Height - 60),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, e) =>
            {
                InitStatisticsPanel();
            };
            panelSettings.Controls.Add(btnBack);

            // === Контейнер под график ===
            var plotView = new OxyPlot.WindowsForms.PlotView
            {
                Location = new Point(300, 70),
                Size = new Size(panelSettings.Width - 350, panelSettings.Height - 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            panelSettings.Controls.Add(plotView);

            // === Событие обновления графика ===
            void RefreshChart()
            {
                string selection = cmbPeriod.SelectedItem.ToString();
                bool isGameCountMode = chkViewMode.Checked;

                DateTime fromDate = DateTime.MinValue;
                if (selection == "Last 7 Days")
                    fromDate = DateTime.UtcNow.AddDays(-7);
                else if (selection == "Last 30 Days")
                    fromDate = DateTime.UtcNow.AddDays(-30);

                var results = SqliteFunc.GetAllGameResults()
                    .Where(r => r.GameName == "Matches" &&
                                (selection == "All Time" || DateTime.Parse(r.DatePlayed) >= fromDate))
                    .OrderBy(r => DateTime.Parse(r.DatePlayed))
                    .ToList();

                var model = new OxyPlot.PlotModel
                {
                    Title = isGameCountMode ? "Games per Day" : "Performance Over Time"
                };

                var series = new OxyPlot.Series.LineSeries
                {
                    MarkerType = OxyPlot.MarkerType.Circle,
                    MarkerSize = 4,
                    MarkerStroke = OxyPlot.OxyColors.DarkGreen
                };

                if (isGameCountMode)
                {
                    // Группировка по дате
                    var grouped = results
                        .GroupBy(r => DateTime.Parse(r.DatePlayed).Date)
                        .Select(g => new { Date = g.Key, Count = g.Count() });

                    foreach (var g in grouped)
                    {
                        series.Points.Add(new OxyPlot.DataPoint(DateTimeAxis.ToDouble(g.Date), g.Count));
                    }
                }
                else
                {
                    foreach (var r in results)
                    {
                        DateTime date = DateTime.Parse(r.DatePlayed);
                        series.Points.Add(new OxyPlot.DataPoint(DateTimeAxis.ToDouble(date), r.Points));
                    }
                }

                model.Series.Add(series);
                model.Axes.Add(new DateTimeAxis
                {
                    Position = AxisPosition.Bottom,
                    StringFormat = "dd.MM",
                    Title = "Date",
                    MajorGridlineStyle = LineStyle.Solid
                });

                model.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = isGameCountMode ? "Games" : "Score",
                    Minimum = 0,
                    MajorGridlineStyle = LineStyle.Solid
                });

                plotView.Model = model;
            }

            // === События переключения ===
            cmbPeriod.SelectedIndexChanged += (s, e) => RefreshChart();
            chkViewMode.CheckedChanged += (s, e) => RefreshChart();

            // === Первичная инициализация ===
            RefreshChart();
        }
        private void InitCardsStatisticsPanel()
        {
            panelSettings.Controls.Clear();
            panelSettings.BackColor = Color.FromArgb(204, 255, 204);

            // === Заголовок ===
            var title = new Label
            {
                Text = "Cards Statistics",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point((panelSettings.Width - 300) / 2, 20)
            };
            panelSettings.Controls.Add(title);

            // === Комбо выбора периода ===
            var cmbPeriod = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(50, 70),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPeriod.Items.AddRange(new string[] { "Last 7 Days", "Last 30 Days", "All Time" });
            cmbPeriod.SelectedIndex = 0;
            panelSettings.Controls.Add(cmbPeriod);

            // === Переключатель типа графика ===
            var chkViewMode = new CheckBox
            {
                Text = "Show Games per Day",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(50, 110),
                Checked = false
            };
            panelSettings.Controls.Add(chkViewMode);

            // === Кнопка Назад ===
            var btnBack = new Button
            {
                Text = "Back",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(100, 35),
                Location = new Point(50, panelSettings.Height - 60),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, e) =>
            {
                InitStatisticsPanel();
            };
            panelSettings.Controls.Add(btnBack);

            // === Контейнер под график ===
            var plotView = new OxyPlot.WindowsForms.PlotView
            {
                Location = new Point(300, 70),
                Size = new Size(panelSettings.Width - 350, panelSettings.Height - 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            panelSettings.Controls.Add(plotView);

            // === Событие обновления графика ===
            void RefreshChart()
            {
                string selection = cmbPeriod.SelectedItem.ToString();
                bool isGameCountMode = chkViewMode.Checked;

                DateTime fromDate = DateTime.MinValue;
                if (selection == "Last 7 Days")
                    fromDate = DateTime.UtcNow.AddDays(-7);
                else if (selection == "Last 30 Days")
                    fromDate = DateTime.UtcNow.AddDays(-30);

                var results = SqliteFunc.GetAllGameResults()
                    .Where(r => r.GameName == "Cards" &&
                                (selection == "All Time" || DateTime.Parse(r.DatePlayed) >= fromDate))
                    .OrderBy(r => DateTime.Parse(r.DatePlayed))
                    .ToList();

                var model = new OxyPlot.PlotModel
                {
                    Title = isGameCountMode ? "Games per Day" : "Performance Over Time"
                };

                var series = new OxyPlot.Series.LineSeries
                {
                    MarkerType = OxyPlot.MarkerType.Circle,
                    MarkerSize = 4,
                    MarkerStroke = OxyPlot.OxyColors.DarkGreen
                };

                if (isGameCountMode)
                {
                    // Группировка по дате
                    var grouped = results
                        .GroupBy(r => DateTime.Parse(r.DatePlayed).Date)
                        .Select(g => new { Date = g.Key, Count = g.Count() });

                    foreach (var g in grouped)
                    {
                        series.Points.Add(new OxyPlot.DataPoint(DateTimeAxis.ToDouble(g.Date), g.Count));
                    }
                }
                else
                {
                    foreach (var r in results)
                    {
                        DateTime date = DateTime.Parse(r.DatePlayed);
                        series.Points.Add(new OxyPlot.DataPoint(DateTimeAxis.ToDouble(date), r.Points));
                    }
                }

                model.Series.Add(series);
                model.Axes.Add(new DateTimeAxis
                {
                    Position = AxisPosition.Bottom,
                    StringFormat = "dd.MM",
                    Title = "Date",
                    MajorGridlineStyle = LineStyle.Solid
                });

                model.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = isGameCountMode ? "Games" : "Score",
                    Minimum = 0,
                    MajorGridlineStyle = LineStyle.Solid
                });

                plotView.Model = model;
            }

            // === События переключения ===
            cmbPeriod.SelectedIndexChanged += (s, e) => RefreshChart();
            chkViewMode.CheckedChanged += (s, e) => RefreshChart();

            // === Первичная инициализация ===
            RefreshChart();
        }


        private void timer1_Tick(object sender, EventArgs e)
		{
            if(Gamenow == 1)
			{
                progressBar1.Value = Math.Max(0, progressBar1.Value - 1);
                if (progressBar1.Value <= 0 && prTime > 10)
                {
                    timer1.Enabled = false;
                    MatchesEndGame();
                }
                prTime++;
            }
            if (Gamenow == 2)
            {
                progressBar1.Value = Math.Max(0, progressBar1.Value - 1);
                if (progressBar1.Value <= 0 && prTime > 10)
                {
                    timer1.Enabled = false;
                    CardsEndGame();
                }
                prTime++;
            }
        }
	}
}
