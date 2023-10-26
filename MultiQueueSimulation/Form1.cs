using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueModels;
using MultiQueueTesting;

namespace MultiQueueSimulation
{
    public partial class Form1 : Form
    {
        static string path;
        
        List<SimulationCase> simulationtable;
        static SimulationSystem system2;
        public Form1(string pathinp)
        {
            path = pathinp;
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //dataGridView1.DataSource = simulationtable;
            if (path == null)

            {
                button1.Enabled = true;
                
                button3.Enabled = false;

            }
            else 
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = true;
                setPath(path);
                int nOServ = system2.NumberOfServers;
                
                textBox1.Text = nOServ.ToString();
                for (int i = 0; i < nOServ; i++)
                {
                    dataGridView1.Rows.Add("Server " + (i + 1));


                }


            }
            
        }
        public static void setSystem(SimulationSystem sys)
        {
            system2 = sys;

            //dataGridView1.DataSource = simulationtable;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select File";
            fileDialog.Filter = "ALl files (*.*)|*.*|Text file (*.txt)|*.txt";
            fileDialog.FilterIndex = 1;
            fileDialog.ShowDialog();
            
            if (fileDialog.FileName == null || fileDialog.FileName == "")
            {

                return;

            }
            else
            {
               
                path = fileDialog.FileName;
                 this.Close();
                

            }

        }
        public static string getPath() 
        {
            return path;
        }
        public static void setPath(string pa)
        {
            path = pa;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3(system2);
            frm3.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button3.Enabled = true;
            path = "..\\localManualTest.txt";
            Form4 form4 = new Form4();
            form4.ShowDialog();
            
            this.Close();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
           
            simulationtable = system2.return_data_of_server(e.RowIndex+1);
            Form2 frm = new Form2(simulationtable);


            frm.ShowDialog();
            //dataGridView1.Rows[e.RowIndex]
        }
    }
}
