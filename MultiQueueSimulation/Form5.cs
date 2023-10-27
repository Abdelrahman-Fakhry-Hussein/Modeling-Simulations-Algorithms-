using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiQueueSimulation
{
    public partial class Form5 : Form
    {
        string stringInfo;
        public Form5(string varinfo)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = varinfo;
            if (varinfo == "InterarrivalDistribution")

                this.dataGridView1.Columns[0].HeaderText = "Interarrival Time";

            else 
                this.dataGridView1.Columns[0].HeaderText = "Service Time";
            try
            {
                string text = "\n\n"+varinfo+ "\n";
                File.AppendAllText("..\\localManualTest.txt", text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error :" + ex);
            }
            stringInfo =varinfo;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> column1Values = new List<int>();
            List<float> column2Values = new List<float>();
            int rownumber = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the row is not a new row (new row for adding data)
                if (!row.IsNewRow)
                {
                    // Check if the cell values are not null
                    if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                    {


                        if (float.TryParse(row.Cells[1].Value.ToString(), out float floatValue) && int.TryParse(row.Cells[0].Value.ToString(), out int intValue))
                        {
                            if (intValue >= 0 && floatValue > 0)
                            {
                                column1Values.Add(intValue);
                                column2Values.Add(floatValue);
                                rownumber++;
                            }
                            else
                            {
                                MessageBox.Show("negative values");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("error wrong input");
                            return;
                        }
        
                    }
            
                }
            }
        /*    float suml = column2Values.Sum(); 
            if (suml != 1) 
            {
                MessageBox.Show("sum of probability is not equal 1");
                return;


            }*/
            for (int i = 0; i < rownumber;i++)
            {
                try {

                    string text = column1Values[i] + ", "+(float)column2Values[i]+"\n";
                    // Append the text to the file
                    File.AppendAllText("..\\localManualTest.txt", text); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error :"+ex);
                }
            }
            this.Close();

        }
    }
}
