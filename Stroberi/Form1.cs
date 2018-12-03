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
            
        }

        private void inputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void histo() {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Tak ada Gambar");
            }
            else
            {
                Form1 histogForm = new Form1();
                //histogForm.histoRchart.Series["Gray"].Points.Clear();
                int[] histoR = new int[256];
                int[] histoG = new int[256];
                int[] histoB = new int[256];
                Bitmap bpms = new Bitmap(pictureBox1.Image);
                int wid = bpms.Width;
                int hei = bpms.Height;

                Color p;
                progressBar1.Maximum = hei;
                progressBar1.Visible = true;
                for (int y = 0; y < hei; y++)
                {
                    for (int x = 0; x < wid; x++)
                    {
                        p = bpms.GetPixel(x, y);

                        histoR[p.R] = histoR[p.R] + 1;
                        histoG[p.G] = histoG[p.G] + 1;
                        histoB[p.B] = histoB[p.B] + 1;

                    }
                    progressBar1.Value = y;
                }

                histoRchart.Series["Red"].Points.DataBindY(histoR);
                histoRchart.Series["Red"].Color = Color.Red;
                histoRchart.Series["Green"].Points.DataBindY(histoG);
                histoRchart.Series["Green"].Color = Color.Green;
                histoRchart.Series["Blue"].Points.DataBindY(histoB);
                histoRchart.Series["Blue"].Color = Color.Blue;

                progressBar1.Visible = false;
            }
        }

        private void proses_Click(object sender, EventArgs e)
        {
            int wid = bmp.Width;
            int hei = bmp.Height;
            double totalPix = wid * hei;

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
            redLabel.Text = "RED = "+(totalRed/totalPix).ToString();
            blueLabel.Text = "BLUE = " + (totalBlue / totalPix).ToString();
            greenLabel.Text = "GREEN = " + (totalGreen / totalPix).ToString();
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

            histo();
        }
    }
}
