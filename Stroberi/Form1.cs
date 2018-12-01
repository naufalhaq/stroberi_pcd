using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stroberi
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        double totalRed,totalGreen,totalBlue;
        public Form1()
        {
            InitializeComponent();
            labelKeterangan.Visible = false;
        }


        private static int truncate(int x)
        {
            if (x > 255) x = 255;
            else if (x < 0) x = 0;
            return x;
        }
        private static Double truncate(Double x)
        {
            if (x > 255) x = 255;
            else if (x < 0) x = 0;
            return x;
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog bukaFile = new OpenFileDialog();
            bukaFile.Filter = "Image File (*.bmp, *.jpg, *.jpeg)|*.bmp;*.jpg;*.jpeg";
            if (DialogResult.OK == bukaFile.ShowDialog())
            {
                bmp = new Bitmap(bukaFile.FileName);
                this.pictureBox1.Image = bmp;
                toolStripStatusLabel1.Text = bukaFile.FileName + " |  Resolusi " + pictureBox1.Image.Width + " X " + pictureBox1.Image.Height;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void inputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                MessageBox.Show("Tidak ada citra yang akan diolah");
            else
            {
                double[] HistoR = new double[256];
                double[] HistoG = new double[256];
                double[] HistoB = new double[256];
                Bitmap b = new Bitmap((Bitmap)this.pictureBox1.Image);
                RGB_Form frm5 = new RGB_Form();
                for (int i = 0; i < 255; i++)
                {
                    HistoR[i] = 0;
                    HistoG[i] = 0;
                    HistoB[i] = 0;
                }
                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 255; j++)
                    {
                        Color c1 = b.GetPixel(i, j);
                        int merah = c1.R;
                        int hijau = c1.G;
                        int biru = c1.B;
                        HistoR[merah]++;
                        HistoG[hijau]++;
                        HistoB[biru]++;
                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;

                frm5.chart1.Series["Series1"].Color = Color.Red;
                frm5.chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                frm5.chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;

                foreach (Double HstR in HistoR)

                {
                    for (int i = 0; i <= 255; i++)
                    {
                        frm5.chart1.Series["Series1"].Points.AddXY(i, HistoR[i]);
                    }
                }

                frm5.chart2.Series["Series1"].Color = Color.Green;
                frm5.chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                frm5.chart2.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;

                foreach (Double HstG in HistoG)

                {
                    for (int i = 0; i <= 255; i++)
                    {
                        frm5.chart2.Series["Series1"].Points.AddXY(i, HistoG[i]);
                    }
                }

                frm5.chart3.Series["Series1"].Color = Color.Blue;
                frm5.chart3.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                frm5.chart3.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;

                foreach (Double HstB in HistoB)

                {
                    for (int i = 0; i <= 255; i++)
                    {
                        frm5.chart3.Series["Series1"].Points.AddXY(i, HistoB[i]);
                    }
                }
                frm5.ShowDialog();
            }
        }

        private void proses_Click(object sender, EventArgs e)
        {
            int wid = bmp.Width;
            int hei = bmp.Height;

            //double valuess = Convert.ToDouble(val);
            //double f = (259 * (valuess + 255)) / (255 * (259 - valuess));
            Color p;
            progressBar1.Maximum = hei;
            progressBar1.Visible = true;
            for (int y = 0; y < hei; y++)
            {
                for (int x = 0; x < wid; x++)
                {
                    p = bmp.GetPixel(x, y);
                    //double a = p.A*Math.Log10(1 + valuess);
                    if (p.R != 255 && p.G != 255 && p.B != 255)
                    {
                        totalRed += p.R;
                        totalBlue += p.B;
                        totalGreen += p.G;

                    }

                    //double r = valuess * (p.R - 128) + 128;
                    //double g = valuess * (p.G - 128) + 128;
                    //double b = valuess * (p.B - 128) + 128;
                    //bmpbuffer.SetPixel(x, y, Color.FromArgb(trunc(Convert.ToInt16(r)), trunc(Convert.ToInt16(g)), trunc(Convert.ToInt16(b))));
                }
                progressBar1.Value = y;
            }
            redLabel.Text = "RED = "+totalRed.ToString();
            blueLabel.Text = "BLUE = " + totalBlue.ToString();
            greenLabel.Text = "GREEN = " + totalGreen.ToString();
            if ((totalRed - totalGreen) < 0 && (totalGreen - totalBlue) > 110000)
            {
                labelKeterangan.Text = "MENTAH";
            }
            else
            {
                labelKeterangan.Text = "MATANG";
            }
            labelKeterangan.Visible = true;
            totalGreen = 0;totalRed = 0;totalBlue = 0;
            
            //pictureBox2.Image = bmpbuffer;
            progressBar1.Visible = false;
        }
    }
}
