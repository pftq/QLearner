using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QLearner.QStates;

namespace QLearner
{
    public partial class MazeGUI_Example : Form
    {
        private int currentSelfX = -1, currentSelfY = -1, currentGoalX = -1, currentGoalY = -1;

        public MazeGUI_Example(int x, int y, int selfx, int selfy, int goalx, int goaly)
        {
            InitializeComponent();
            Width = 60 + x * 25;
            Height = 60 + y * 25;
            for (int i = 0; i < x; i++)
            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "" + i;
                c.HeaderText = "" + i;
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = Math.Max(1, 100 / x);
                grid.Columns.Add(c);
            }
            for (int i = 0; i < y; i++)
            {
                grid.Rows.Add("");
                grid.Rows[i].HeaderCell.Value = "" + i;
            }

            SetSelf(selfx, selfy);
            SetGoal(goalx, goaly);

            KeyDown += (o, s) => { OK_Click(); };
            grid.KeyDown += (o, s) => { OK_Click(); };

        }

        private delegate void QuitD();
        public void Quit()
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new QuitD(Quit));
            }
            else
            {
                Close();
            }
        }

        private delegate void SetSelfD(int x, int y);
        public void SetSelf(int x, int y)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new SetSelfD(SetSelf), x, y);
            }
            else if (currentSelfX != x || currentSelfY != y)
            {
                if (currentSelfY >= 0 && grid.Rows[currentSelfY].Cells[currentSelfX].Value.ToString() == "X") grid.Rows[currentSelfY].Cells[currentSelfX].Value = "";
                grid.Rows[y].Cells[x].Value = "X";
                currentSelfX = x; currentSelfY = y;
            }
        }

        private delegate void SetGoalD(int x, int y);
        public void SetGoal(int x, int y)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new SetGoalD(SetGoal), x, y);
            }
            else if (currentGoalX != x || currentGoalY != y)
            {
                if (currentGoalY >= 0 && grid.Rows[currentGoalY].Cells[currentGoalX].Value.ToString() == "G") grid.Rows[currentGoalY].Cells[currentGoalX].Value = "";
                grid.Rows[y].Cells[x].Value = "G";
                currentGoalX = x; currentGoalY = y;
            }
        }

        private void OK_Click(object sender = null, EventArgs e = null)
        {
            Close();
        }

    }
}
