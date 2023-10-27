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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            comboBox1.Items.Add("HighestPriority");
            comboBox1.Items.Add("Random");
            comboBox1.Items.Add("LeastUtilization");
            comboBox2.Items.Add("NumberOfCustomers");
            comboBox2.Items.Add("SimulationEndTime");
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            int number,number2;
            string filePath = "..\\localManualTest.txt";
            if (textBox1.Text == null|| textBox1.Text=="")
            {
                MessageBox.Show("ERROR PLEASE MAKE SURE TO FILL NUMBER OF SERVERS");
                return;
            }
            else if (textBox2.Text == null || textBox2.Text == "")
            {
                MessageBox.Show("ERROR PLEASE MAKE SURE TO FILL STOPPING NUMBER");
                return;
            }
            else if (comboBox2.Text == null || comboBox2.Text == "")
            {
                MessageBox.Show("ERROR PLEASE MAKE SURE TO SELECT STOPPING CRITERIA");
                return;
            }
            else if (comboBox1.Text == null || comboBox1.Text == "")
            {
                MessageBox.Show("ERROR PLEASE MAKE SURE TO SELECT SELECTION METHOD");
                return;
            }
            try
            {
                 number = int.Parse(textBox1.Text);
                 number2 = int.Parse(textBox2.Text);
                // Use the 'number' variable, which now holds the integer value
                if (number <= 0||number2<=0)
                {
                    MessageBox.Show("Invalid input. Please enter a valid Positive integer.");
                    return;
                }
             
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid Positive integer.");
                return ;
            }
          
            // Text to append
            string numServ = "NumberOfServers\n"+textBox1.Text;
            string StoppingNumber = "\n\nStoppingNumber\n"+textBox2.Text;
            string StoppingCriteria = "\n\nStoppingCriteria\n"+ (comboBox1.SelectedIndex + 1);
            string SelectionMethod = "\n\nSelectionMethod\n"+(comboBox1.SelectedIndex+1);
            string allf = numServ + StoppingNumber + StoppingCriteria + SelectionMethod;

            try
            {
              
                // Create the file and write the initial text
                File.WriteAllText(filePath, allf);
                          
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("error "+ex);
            }
            this.Close();
            string InterarrivalDistribution = "InterarrivalDistribution";
            Form5 form5dintdist = new Form5(InterarrivalDistribution);
            form5dintdist.ShowDialog();
            for (int i = 0; i < number;i++)
            {
                string servDist = "ServiceDistribution_Server" + (i+1);
                Form5 form5 = new Form5(servDist);
                form5.ShowDialog();
            }
            
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
