using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_ku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            getAvailablePorts();
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
        }

        private void getAvailablePorts()
        {
            toolStripComboBox1.Items.Clear();
            String[] ports = SerialPort.GetPortNames();
            toolStripComboBox1.Items.AddRange(ports);
            toolStripComboBox1.Items.Add("- Refresh -");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(toolStripComboBox1.Text == "- Refresh -" || toolStripComboBox1.Text == "")
            {
                MessageBox.Show("Silakan Pilih COM terlebih dahulu!");
                return;
            }

            if(toolStripButton1.Text == "Open")
            {
                try
                {
                    serialPort1.PortName = toolStripComboBox1.Text;
                    serialPort1.BaudRate = int.Parse(toolStripComboBox2.Text);
                    serialPort1.ReadTimeout = 1000;
                    serialPort1.WriteTimeout = 1000;
                    serialPort1.DataReceived += SerialPort1_DataReceived;
                    serialPort1.Open();
                    toolStripButton1.Text = "Close";
                }
                catch (Exception)
                {
                    MessageBox.Show("COM Tidak Dapat Digunakan!");
                }
            }
            else
            {
                serialPort1.Close();
                toolStripButton1.Text = "Open";
            }

        }

        String data = "";
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            data = sp.ReadLine();
            this.Invoke(new EventHandler(deCode));
        }

        private void deCode(object sender, EventArgs e)
        {
            char[] sparator = { '=', ',' };
            String[] dataSplit = data.Split(sparator);
            if (dataSplit[0] ==  "DATA")
            {
                DateTime time = DateTime.Now;
                float kelembapan = float.Parse(dataSplit[1]);
                float temperatur = float.Parse(dataSplit[2]);
                textBox1.Text = dataSplit[1];
                textBox2.Text = dataSplit[2];
                chart1.Series["Kelembapan"].Points.AddXY(time, kelembapan);
                chart1.Series["Temperatur"].Points.AddXY(time, temperatur);
            } else
            {
                textBox4.Text = data;
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text == "- Refresh -")
            {
                getAvailablePorts();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String dataKirim = textBox3.Text;
            if(serialPort1.IsOpen)
            {
                serialPort1.Write("S=");
                serialPort1.WriteLine(dataKirim);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String dataKirim = textBox5.Text;
            if (serialPort1.IsOpen)
            {
                serialPort1.Write("P=");
                serialPort1.WriteLine(dataKirim);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String dataKirim = textBox6.Text;
            if (serialPort1.IsOpen)
            {
                serialPort1.Write("I=");
                serialPort1.WriteLine(dataKirim);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String dataKirim = textBox7.Text;
            if (serialPort1.IsOpen)
            {
                serialPort1.Write("D=");
                serialPort1.WriteLine(dataKirim);
            }
        }
    }
}
