using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Buildings
{
    public partial class MainForm : Form
    {
        private readonly Random random = new Random();
        private readonly float eps = 5.0f;

        private readonly List<PointF> points = new List<PointF>();
        private readonly List<PointF> polygon = new List<PointF>();

        private readonly float stepSize = 10.0f;

        private readonly float pointBigRadius = 3.0f;
        private readonly float pointSmallRadius = 1.0f;

        private bool showPolygon = true;

        private PolygonBuilder polygonBuilder = null;

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
            if (e.KeyData == Keys.F1)
            {
                this.showPolygon = !this.showPolygon;
                this.Invalidate();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (this.showPolygon)
            {
                if (this.polygon.Count > 2)
                {
                    e.Graphics.DrawPolygon(Pens.Silver, this.polygon.ToArray());
                }
                foreach (PointF point in this.polygon)
                {
                    e.Graphics.FillEllipse(Brushes.Blue, point.X - this.pointBigRadius, point.Y - this.pointBigRadius, 2 * this.pointBigRadius, 2 * this.pointBigRadius);
                }
            }

            foreach (PointF point in this.points)
            {
                e.Graphics.FillEllipse(Brushes.Black, point.X - this.pointSmallRadius, point.Y - this.pointSmallRadius, 2 * this.pointSmallRadius, 2 * this.pointSmallRadius);
            }

            if (this.polygonBuilder != null)
            {
                List<PointF> points = this.polygonBuilder.contour.ConvertAll(pointIndex =>
                {
                    LocatorZ locatorZ = this.polygonBuilder.points[pointIndex];
                    return new PointF((float)locatorZ.X, (float)locatorZ.Y);
                });
                e.Graphics.DrawPolygon(Pens.Pink, points.ToArray());
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.points.Count > 0)
                {
                    this.polygon.Clear();
                    this.polygonBuilder = null;
                }
                this.points.Clear();
                this.polygon.Add(e.Location);
            }
            if (e.Button == MouseButtons.Right)
            {
                this.points.Clear();
                for (float x = 0; x < this.ClientSize.Width; x += this.stepSize)
                {
                    for (int i = 0; i < this.polygon.Count; ++i)
                    {
                        int j = (i + 1) % this.polygon.Count;
                        PointF pi = this.polygon[i];
                        PointF pj = this.polygon[j];
                        float a = (x - pi.X) / (pj.X - pi.X);
                        if (0 <= a && a < 1)
                        {
                            float y = a * pj.Y + (1 - a) * pi.Y;
                            this.points.Add(new PointF(x + this.eps * ((float)this.random.NextDouble() - 0.5f), y + this.eps * ((float)this.random.NextDouble() - 0.5f)));
                        }
                    }
                }
                for (float y = 0; y < this.ClientSize.Height; y += this.stepSize)
                {
                    for (int i = 0; i < this.polygon.Count; ++i)
                    {
                        int j = (i + 1) % this.polygon.Count;
                        PointF pi = this.polygon[i];
                        PointF pj = this.polygon[j];
                        float a = (y - pi.Y) / (pj.Y - pi.Y);
                        if (0 <= a && a < 1)
                        {
                            float x = a * pj.X + (1 - a) * pi.X;
                            this.points.Add(new PointF(x + this.eps * ((float)this.random.NextDouble() - 0.5f), y + this.eps * ((float)this.random.NextDouble() - 0.5f)));
                        }
                    }
                }

                List<LocatorZ> locators = this.points.ConvertAll(point => new LocatorZ(point.X, point.Y, 0.0));
                this.polygonBuilder = new PolygonBuilder(locators);
                this.polygonBuilder.BuildContour(2 * this.stepSize);
            }
            this.Invalidate();
        }
    }
}
