using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CsvHelper;

namespace GXT2_Viewer
{
    public partial class Form1 : Form
    {
        private GXT2 g;
        private string openedFile = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Columns[0].AutoSizeMode = dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // dataGridView1.Columns[dataGridView1.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.SortCompare += dataGridView1_SortCompare;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openedFile = openFileDialog1.FileName;
                Form1.ActiveForm.Text = "GXT2 Viewer - " + Path.GetFileName(openedFile);
                g = new GXT2(new FileStream(openedFile, FileMode.Open, FileAccess.Read));
                foreach (KeyValuePair<uint, byte[]> dataItem in g.DataItems)
                {
                    string c = Encoding.UTF8.GetString(dataItem.Value);
                    dataGridView1.Rows.Add(new string[]{ toHex(dataItem.Key), c });
                }
            }
        }

        private void saveAsCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openedFile != "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = File.CreateText(saveFileDialog1.FileName))
                    {
                        var csv = new CsvWriter(writer);
                        foreach (KeyValuePair<uint, byte[]> dataItem in g.DataItems)
                        {
                            string c = Encoding.UTF8.GetString(dataItem.Value);
                            csv.WriteField(toHex(dataItem.Key));
                            csv.WriteField(c);
                            csv.NextRecord();
                        }
                    }
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by theawesomecoder61\nPowered by CsvHelper\nIcon by Icons8", "GXT2 Viewer | version 1.1");
        }

        private void copyCellContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(dataGridView1.SelectedCells[0].Value.ToString());
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
            {
                e.Handled = true;
            }
        }

        private string toHex(uint o)
        {
            return "0x" + o.ToString("X");
        }
    }
}