using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labyrinth
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            disableControls(this.Controls);
            osToolStripMenuItem.Text = SystemInfo.getOS();
            lANIPToolStripMenuItem.Text = SystemInfo.getLANIP();
            wANIPToolStripMenuItem.Text = SystemInfo.getWANIP();
        }

        static public string path = "";
        public static int[,] map;
        public static int cnt = 1;
        public static int MapWidht = 10;
        public static int MapHeight = 10;

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                path = openFileDialog.FileName.ToString();
                textBox1.Text = path;

                String input = File.ReadAllText(@path);

                int i = 0, j = 0;

                map = new int[10, 10];

                foreach (var row in input.Split('\n'))
                {
                    j = 0;
                    foreach (var col in row.Trim().Split(' '))
                    {
                        map[i, j] = int.Parse(col.Trim());
                        j++;
                    }
                    i++;
                }

                MessageBox.Show(this, "File is opened!",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!" + "\r\n" + ex.ToString(),
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                string filename = saveFileDialog.FileName;
                System.IO.File.WriteAllText(filename, textBox1.Text);
                MessageBox.Show(this, "File is saved!",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!" + "\r\n" + ex.ToString(),
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        public void clearControls(Control.ControlCollection ctrlCollection)
        {
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is TextBoxBase)
                {
                    ctrl.Text = String.Empty;
                }
                else
                {
                    clearControls(ctrl.Controls);
                }
            }
        }

        public void enableControls(Control.ControlCollection ctrlCollection)
        {
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is TextBoxBase)
                {
                    ctrl.Enabled = true;
                }
                else if (ctrl is ButtonBase)
                {
                    ctrl.Enabled = true;
                }
                else
                {
                    enableControls(ctrl.Controls);
                }
            }
        }

        public void disableControls(Control.ControlCollection ctrlCollection)
        {
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is TextBoxBase)
                {
                    ctrl.Enabled = false;
                }
                else if (ctrl is ButtonBase)
                {
                    ctrl.Enabled = false;
                }
                else
                {
                    disableControls(ctrl.Controls);
                }
            }
        }

        private void unlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableControls(this.Controls);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearControls(this.Controls);
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Labyrinth Path-Finder\r\n"
                        + "Artur Zhadan\r\n"
                        + "2020",
                        "Message",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateToolStripMenuItem.Text = SystemInfo.getDate();
            timeToolStripMenuItem.Text = SystemInfo.getTime();
            stopwatchToolStripMenuItem.Text = SystemInfo.getStopwatch();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                int x1 = Convert.ToInt32(textBox2.Text);
                int y1 = Convert.ToInt32(textBox3.Text);
                int x2 = Convert.ToInt32(textBox4.Text);
                int y2 = Convert.ToInt32(textBox5.Text);

                dataGridView1.RowCount = MapWidht;
                dataGridView1.ColumnCount = MapHeight;

                for (int y = 0; y < MapHeight; y++)
                {
                    dataGridView1.Columns[y].Width = 25;
                    for (int x = 0; x < MapWidht; x++)
                    {
                        dataGridView1.Rows[x].Height = 25;
                        if (map[y, x] == 1)
                        {
                            dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Gray;
                        }
                    }
                }
               
                dataGridView1.Enabled = false;
                dataGridView1.ClearSelection();

                int min_dist = Int32.MaxValue;

                List<int> shortestPath = new List<int>();
                List<int> currentPath = new List<int>();
                bool[,] visited = new bool[10, 10];

                min_dist = Path.solve(map, visited, x1, y1, x2, y2, 0, min_dist, shortestPath, currentPath);
                visited = new bool[10, 10];

                textBox6.AppendText("Labyrinth: " + Convert.ToString(cnt) + "\r\n");
                if (min_dist != Int32.MaxValue)
                {
                    dataGridView1[y1, x1].Value = "A";
                    dataGridView1[y2, x2].Value = "B";
                    textBox6.AppendText("Path length: " + Convert.ToString(min_dist) + "\r\n");
                    textBox6.AppendText("Path: ");

                    for (int i = 0; i < shortestPath.Count; i += 2)
                    {
                        var x = shortestPath[i];
                        var y = shortestPath[i + 1];
                        textBox6.AppendText("(" + x + "; " + y + ")" + " ");
                    }

                    for (int i = 0; i < shortestPath.Count; i += 2)
                    {
                        var x = shortestPath[i];
                        var y = shortestPath[i + 1];
                        dataGridView1[y, x].Style.BackColor = Color.Red;
                        Thread.Sleep(100);
                        dataGridView1.Refresh();
                    }
                    textBox6.AppendText("\r\n");
                }
                else 
                {
                    textBox6.AppendText("Wrong path!" + "\r\n");
                }
                
                cnt++;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!" + "\r\n" + ex.ToString(),
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
