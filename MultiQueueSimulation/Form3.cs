using MultiQueueModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MultiQueueSimulation
{
    
    public partial class Form3 : Form
    {
        SimulationSystem system1;
        public Form3(SimulationSystem sys)
        {
            InitializeComponent();
            Controls.Remove(chart1);
            system1 = sys;
            this.StartPosition = FormStartPosition.CenterScreen;
            //=======================================================
            int numofServ = sys.NumberOfServers;//EDIT GET THE NUMBER OF SERVERS
            //=======================================================
            for (int i = 0; i < numofServ; i++) 
            {

                comboBox1.Items.Add("Server "+(i+1).ToString());
            
            }
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            label2.Visible=false;
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controls.Remove(chart1);
            int index = comboBox1.SelectedIndex;
            label4.Text = system1.Servers[index].Utilization.ToString();
            label5.Text = system1.Servers[index].idle.ToString();
            label6.Text = system1.Servers[index].AverageServiceTime.ToString();
            if (chart1.Series.Count>0)
            chart1.Series.Remove(chart1.Series[0]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (comboBox1.SelectedIndex == -1) return;
            label2.Visible = true;
            // Sample data representing light status (1 for on, 0 for off) over time in seconds
            List<int> timeInSeconds =new List<int>();
            List<int> busyStatus = new List<int>();
            int index = comboBox1.SelectedIndex+1;
            List<SimulationCase> simulationtable;
            simulationtable = system1.return_data_of_server(index);
            int endt = simulationtable.Count ;
            int x = 0;
        /*    if (endt <= 50)
            {*/
                if (simulationtable[0].StartTime > 0)
                {
                    for (int l = 0; l < simulationtable[0].StartTime; l++)
                    {
                        busyStatus.Add(0);
                        timeInSeconds.Add(simulationtable[0].StartTime + l);
                    }
                }
                for (int i = 0; i < endt; i++)
                {

                    x = simulationtable[i].EndTime - simulationtable[i].StartTime;


                    if (x > 0)
                    {
                        for (int l = 0; l < x; l++)
                        {
                            busyStatus.Add(1);
                            timeInSeconds.Add(simulationtable[i].StartTime + l);
                        }


                    }

                    if (i > 0)
                    {
                        x = simulationtable[i].EndTime - simulationtable[i - 1].StartTime;
                        if (x > 0)
                        {
                            for (int l = 0; l < x; l++)
                            {
                                busyStatus.Add(0);
                                timeInSeconds.Add(simulationtable[i].StartTime + l);
                            }

                        }
                    }

                }



            // Create a new series
            Series series = new Series("Server Busy");
            series.ChartType = SeriesChartType.Column; // Set chart type to Column

            // Set the color of the bars to blue
            series.Color = Color.FromKnownColor(KnownColor.HotTrack);

            // Set the width of the bars to make them look connected
            series["PointWidth"] = "1"; // You can adjust this value to change the width of the bars

            // Add data points to the series
            for (int i = 0; i < timeInSeconds.Count; i++)
            {
                // Add data points with X and Y values (X = timeInSeconds, Y = lightStatus)
                series.Points.AddXY(timeInSeconds[i]+0.5, busyStatus[i]);
            }

            // Add the series to the chart
            chart1.Series.Add(series);

            // Set chart properties
            chart1.ChartAreas[0].AxisX.Title = "Time (seconds)";
            chart1.ChartAreas[0].AxisY.Title = "Server Busy";
            chart1.ChartAreas[0].AxisX.Interval = 1; // Display integer values on the X-axis
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0";
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 1;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            chart1.MouseWheel += Chart_MouseWheel;

            // Add the chart to the form
            Controls.Add(chart1);

            label7.Text = system1.PerformanceMeasures.AverageWaitingTime.ToString();
            label8.Text = system1.PerformanceMeasures.MaxQueueLength.ToString();
            label9.Text = system1.PerformanceMeasures.WaitingProbability.ToString();

        }
        private void Chart_MouseWheel(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)sender;
            ChartArea chartArea = chart.ChartAreas[0];

            if (e.Delta < 0) // Zoom out
            {
                chartArea.AxisX.ScaleView.ZoomReset();
                chartArea.AxisY.ScaleView.ZoomReset();
            }
            else // Zoom in
            {
                double xMin = chartArea.AxisX.ScaleView.ViewMinimum;
                double xMax = chartArea.AxisX.ScaleView.ViewMaximum;
         
                double posXStart = chartArea.AxisX.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                double posXFinish = chartArea.AxisX.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;


                chartArea.AxisX.ScaleView.Zoom(posXStart, posXFinish);
             
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
