using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RegionFilter
{
    public partial class MainForm : Form
    {
        private readonly List<List<PointF>> points = new List<List<PointF>>();
        private readonly Region region = new Region();

        private const int defaultPointCount = 10;
        private const int defaultRegionCount = 10;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyData == Keys.F2)
            {
                this.points.Clear();
                this.region.Update(this.points);
            }
            if (e.KeyData == Keys.F3)
            {
                Random random = new Random();
                this.points.Clear();
                for (int i = 0; i < defaultRegionCount; ++i)
                {
                    this.points.Add(new List<PointF>());
                    for (int j = 0; j < defaultPointCount; ++j)
                    {
                        this.points[i].Add(new PointF(random.Next(this.ClientSize.Width), random.Next(this.ClientSize.Height)));
                    }
                }
                this.region.Update(this.points);

                this.Invalidate();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int[] array = new int[this.ClientSize.Width * this.ClientSize.Height];
            int index = 0;
            for (int y = 0; y < this.ClientSize.Height; ++y)
            {
                for (int x = 0; x < this.ClientSize.Width; ++x)
                {
                    array[index] = (this.region.Contains(x, y) ? Color.Green : Color.Blue).ToArgb();
                    ++index;
                }
            }
            stopwatch.Stop();
            this.Text = stopwatch.ElapsedMilliseconds.ToString();

            using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
            {
                BitmapData imageData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                Marshal.Copy(array, 0, imageData.Scan0, array.Length);
                bitmap.UnlockBits(imageData);
                e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
            }

            foreach (List<PointF> points in this.points)
            {
                if (points.Count > 1)
                {
                    e.Graphics.DrawPolygon(Pens.Red, points.ToArray());
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || this.points.Count == 0)
            {
                this.points.Add(new List<PointF>());
            }
            this.points[this.points.Count - 1].Add(new PointF(e.X, e.Y));
            this.region.Update(this.points);

            this.Invalidate();
        }
    }
}

