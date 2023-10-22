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
        List<SimulationCase> simulationtable;
        public Form1(List<SimulationCase> x)
        {
            simulationtable = x;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            dataGridView1.DataSource = simulationtable;
            
        }
    }
}
