﻿using System;
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
        public Form1()
        {
            InitializeComponent();
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
                this.pictureBox1.Image = new Bitmap(bukaFile.FileName);
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
    }
}