using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace RegionFilter
{
    public partial class MainForm : Form
    {
        #region Private fields.

        private readonly Random random = new Random();

        private const int DefaultPointCount = 100;
        private const int DefaultRegionCount = 10;

        private readonly List<List<Point3D>> region = new List<List<Point3D>>();
        private readonly IRegionFilter regionFilter = new RegionFilter();

        private long updateRegionTime;
        private long updatePointsTime;

        private Bitmap bitmap = new Bitmap(1, 1);
        private readonly Color pointInRegionColor = Color.Green;
        private readonly Color pointOutRegionColor = Color.FromArgb(0);

        private bool isDrawContour = true;
        private readonly Color contourColor = Color.Red;
        private const int ContourPartAlpha = 100;
        private const float ContourPartWidth = 1.0f;
        private const int CurrentContourPartAlpha = 255;
        private const float CurrentContourPartWidth = 2.0f;
        private const float PointSize = 20.0f;

        private bool isDrawStrings = true;
        private readonly Brush stringsBrush = Brushes.Yellow;

        private int indexPart = -1;
        private int indexLine = -1;
        private int indexPoint = -1;

        private Point mousePoint;
        private bool updateOnMouseUp;

        #endregion Private fields.

        private void CreateRandomPoints()
        {
            for (var i = 0; i < DefaultRegionCount; ++i)
            {
                this.region.Add(new List<Point3D>());
                for (var j = 0; j < DefaultPointCount; ++j)
                {
                    this.region[i].Add(new Point3D(this.random.NextDouble() * this.ClientSize.Width, this.random.NextDouble() * this.ClientSize.Height, 0.0));
                }
            }
        }

        private void PreCalculate()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            this.regionFilter.Update(this.region);
            stopwatch.Stop();
            this.updateRegionTime = stopwatch.ElapsedMilliseconds;
        }

        private void Calculate()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var array = new int[this.ClientSize.Width * this.ClientSize.Height];
            var index = 0;
            for (var y = 0; y < this.ClientSize.Height; ++y)
            {
                for (var x = 0; x < this.ClientSize.Width; ++x)
                {
                    array[index] = (this.regionFilter.Contains(x, y) ? this.pointInRegionColor : this.pointOutRegionColor).ToArgb();
                    ++index;
                }
            }
            stopwatch.Stop();
            this.updatePointsTime = stopwatch.ElapsedMilliseconds;

            var imageData = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), ImageLockMode.ReadWrite, this.bitmap.PixelFormat);
            Marshal.Copy(array, 0, imageData.Scan0, array.Length);
            this.bitmap.UnlockBits(imageData);
        }

        private void DrawContour(Graphics graphics)
        {
            if (!this.isDrawContour)
            {
                return;
            }

            for (var j = 0; j < this.region.Count; ++j)
            {
                var pen = new Pen(Color.FromArgb(j == this.indexPart ? CurrentContourPartAlpha : ContourPartAlpha, this.contourColor));

                var regionPart = this.region[j];
                for (var i = 0; i < regionPart.Count; ++i)
                {
                    pen.Width = j == this.indexPart && i == this.indexPoint ? CurrentContourPartWidth : ContourPartWidth;
                    var regionPoint = regionPart[i];
                    graphics.DrawRectangle(pen, (float)(regionPoint.X) - PointSize / 2, (float)(regionPoint.Y) - PointSize / 2, PointSize, PointSize);

                    pen.Width = j == this.indexPart && i == this.indexLine ? CurrentContourPartWidth : ContourPartWidth;
                    var regionPointSource = regionPart[i];
                    var regionPointTarget = regionPart[i != regionPart.Count - 1 ? i + 1 : 0];
                    graphics.DrawLine(pen, (float)(regionPointSource.X), (float)(regionPointSource.Y), (float)(regionPointTarget.X), (float)(regionPointTarget.Y));
                }
            }
        }

        private void DrawStrings(Graphics graphics)
        {
            float textY = 0;
            graphics.DrawString("F1 - help", this.Font, this.stringsBrush, 0.0f, textY);

            if (!this.isDrawStrings)
            {
                return;
            }

            var textHeight = graphics.MeasureString(" ", this.Font).Height;

            textY += textHeight; graphics.DrawString("F2 - show/hide contour", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("F5 - clean", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("F6 - create random points", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight; graphics.DrawString("To add point (points count in current part less then 3) click mouse left button in any place.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("To add point (points count in current part more then 3) click mouse left button on edge.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("To move point click mouse left button on point, hold and move.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("To delete point click mouse right button on point.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("To delete part delete all points in part.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight; graphics.DrawString($"Count of region points: {this.region.Sum(part => part.Count)}", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString($"PreCalculation time: {this.updateRegionTime} milliseconds", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight; graphics.DrawString($"Count of verificated points: {this.ClientSize.Width * this.ClientSize.Height}", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString($"Calculation time: {this.updatePointsTime} milliseconds", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight; graphics.DrawString("Exc - close", this.Font, this.stringsBrush, 0.0f, textY);
        }

        private bool UpdateCurrentIndexes()
        {
            var currentPointIndex = this.FindCurrentPointIndex();
            if (currentPointIndex.Item2 >= 0)
            {
                if (this.indexPart != currentPointIndex.Item1 || this.indexPoint != currentPointIndex.Item2)
                {
                    this.indexPart = currentPointIndex.Item1;
                    this.indexPoint = currentPointIndex.Item2;
                    this.indexLine = -1;
                    return true;
                }
                return false;
            }

            var currentLineIndex = this.FindCurrentLineIndex();
            if (currentLineIndex.Item2 >= 0)
            {
                if (this.indexPart != currentLineIndex.Item1 || this.indexLine != currentLineIndex.Item2)
                {
                    this.indexPart = currentLineIndex.Item1;
                    this.indexLine = currentLineIndex.Item2;
                    this.indexPoint = -1;
                    return true;
                }
                return false;
            }

            if (this.indexPoint != -1 || this.indexLine != -1)
            {
                this.indexPoint = -1;
                this.indexLine = -1;
            }

            return true;
        }

        private Tuple<int, int> FindCurrentPointIndex()
        {
            var indexPart = -1;
            var indexPoint = -1;

            if (0 < this.indexPart && this.indexPart < this.region.Count)
            {
                var i = this.indexPart;
                indexPoint = this.FindCurrentPointIndex(i);
                if (indexPoint >= 0)
                {
                    indexPart = i;
                }
            }

            for (var i = 0; i < this.region.Count && indexPoint < 0; ++i)
            {
                indexPoint = this.FindCurrentPointIndex(i);
                if (indexPoint >= 0)
                {
                    indexPart = i;
                }
            }

            return Tuple.Create(indexPart, indexPoint);
        }

        private int FindCurrentPointIndex(int indexPart)
        {
            var indexPoint = -1;
            var regionPart = this.region[indexPart];
            for (var i = 0; i < regionPart.Count; ++i)
            {
                var regionPoint = regionPart[i];
                if (Math.Abs(this.mousePoint.X - regionPoint.X) <= PointSize / 2 && Math.Abs(this.mousePoint.Y - regionPoint.Y) <= PointSize / 2)
                {
                    indexPoint = i;
                    break;
                }
            }
            return indexPoint;
        }

        private Tuple<int, int> FindCurrentLineIndex()
        {
            var indexPart = -1;
            var indexLine = -1;

            if (0 < this.indexPart && this.indexPart < this.region.Count)
            {
                var i = this.indexPart;
                indexLine = this.FindCurrentLineIndex(i);
                if (indexLine >= 0)
                {
                    indexPart = i;
                }
            }

            for (var i = 0; i < this.region.Count && indexLine < 0; ++i)
            {
                indexLine = this.FindCurrentLineIndex(i);
                if (indexLine >= 0)
                {
                    indexPart = i;
                }
            }

            return Tuple.Create(indexPart, indexLine);
        }

        private int FindCurrentLineIndex(int indexPart)
        {
            var indexLine = -1;
            var regionPart = this.region[indexPart];
            for (var i = 0; i < regionPart.Count; ++i)
            {
                var regionPointSource = regionPart[i];
                var regionPointTarget = regionPart[i != regionPart.Count - 1 ? i + 1 : 0];
                if (!(Math.Abs(this.mousePoint.X - (regionPointSource.X + regionPointTarget.X) / 2) <
                      Math.Abs(regionPointSource.X - regionPointTarget.X) / 2) ||
                    !(Math.Abs(this.mousePoint.Y - (regionPointSource.Y + regionPointTarget.Y) / 2) <
                      Math.Abs(regionPointSource.Y - regionPointTarget.Y) / 2)) continue;
                var vx = regionPointTarget.X - regionPointSource.X;
                var vy = regionPointTarget.Y - regionPointSource.Y;
                var d = Math.Sqrt(vx * vx + vy * vy);
                if (d != 0)
                {
                    vx /= d;
                    vy /= d;
                    if (Math.Abs((this.mousePoint.X - regionPointSource.X) * vy - (this.mousePoint.Y - regionPointSource.Y) * vx) <= PointSize / 2)
                    {
                        indexLine = i;
                        break;
                    }
                }
            }
            return indexLine;
        }

        private bool AddPoint(double x, double y)
        {
            if (this.indexPart < 0)
            {
                var regionPart = new List<Point3D> {new Point3D(x, y, 0.0)};
                this.region.Add(regionPart);
                this.indexPart = 0;
            }
            else
            {
                var regionPart = this.region[this.indexPart];
                if (regionPart.Count < 3)
                {
                    regionPart.Add(new Point3D(x, y, 0.0));
                }
                else
                {
                    if (this.indexLine >= 0)
                    {
                        regionPart.Insert(this.indexLine + 1, new Point3D(x, y, 0.0));
                    }
                    else
                    {
                        regionPart = this.region[this.region.Count - 1];
                        if (regionPart.Count >= 3)
                        {
                            regionPart = new List<Point3D>();
                        }
                        regionPart.Add(new Point3D(x, y, 0.0));
                        this.region.Add(regionPart);
                        this.indexPart = this.region.Count - 1;
                    }
                }
            }
            return true;
        }

        private bool DelPoint()
        {
            var isDone = false;

            if (this.indexPart < 0)
            {
                return false;
            }

            var regionPart = this.region[this.indexPart];
            if (this.indexPoint >= 0)
            {
                regionPart.RemoveAt(this.indexPoint);
                isDone = true;
            }
            return isDone;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.bitmap.Dispose();
            this.bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.Calculate();
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);

            e.Graphics.DrawImageUnscaled(this.bitmap, 0, 0);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            this.DrawContour(e.Graphics);

            this.DrawStrings(e.Graphics);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyData == Keys.F1)
            {
                this.isDrawStrings = !this.isDrawStrings;

                this.Invalidate();
            }
            if (e.KeyData == Keys.F2)
            {
                this.isDrawContour = !this.isDrawContour;

                this.Invalidate();
            }
            if (e.KeyData == Keys.F5)
            {
                this.region.Clear();
                this.PreCalculate();
                this.Calculate();

                this.indexPart = -1;
                this.UpdateCurrentIndexes();

                this.Invalidate();
            }
            if (e.KeyData == Keys.F6)
            {
                this.region.Clear();
                this.CreateRandomPoints();
                this.PreCalculate();
                this.Calculate();

                this.indexPart = -1;
                this.UpdateCurrentIndexes();

                this.Invalidate();
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            var isInvalidate = false;

            if (e.Button == MouseButtons.Left && this.indexPoint < 0)
            {
                isInvalidate = this.AddPoint(e.X, e.Y);
            }
            if (e.Button == MouseButtons.Right && this.indexPoint >= 0)
            {
                if (this.DelPoint())
                {
                    if (this.region[this.indexPart].Count == 0)
                    {
                        this.region.RemoveAt(this.indexPart);
                    }
                    isInvalidate = true;
                }
            }

            if (isInvalidate)
            {
                this.UpdateCurrentIndexes();
                this.PreCalculate();
                this.Calculate();
                this.Invalidate();
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            this.mousePoint = e.Location;

            if (this.region.Count == 0)
            {
                return;
            }

            var isInvalidate = false;

            if (e.Button == MouseButtons.None)
            {
                isInvalidate = this.UpdateCurrentIndexes();
            }

            if (e.Button == MouseButtons.Left && this.indexPoint >= 0)
            {
                List<Point3D> regionPart = this.region[this.indexPart];
                regionPart[this.indexPoint] = new Point3D(e.X, e.Y, 0.0);
                this.updateOnMouseUp = true;
                isInvalidate = true;
            }

            if (isInvalidate)
            {
                this.Invalidate();
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.updateOnMouseUp)
            {
                this.updateOnMouseUp = false;
                this.PreCalculate();
                this.Calculate();
                this.Invalidate();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.bitmap.Dispose();
        }

        public MainForm()
        {
            this.InitializeComponent();
        }
    }
}

