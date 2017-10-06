using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace Prototypes.Forms
{
    public sealed class RegionBuilder
    {
        private int indexPart = -1;
        private int indexLine = -1;
        private int indexPoint = -1;

        private Point mousePoint;
        private bool updateOnMouseUp;

        public List<List<Point3D>> Data { get; } = new List<List<Point3D>>();

        public float PointSize { get; set; }

        public bool IsDraw { get; set; }
        public Color Color { get; set; }
        public int PartAlpha { get; set; }
        public float PartWidth { get; set; }
        public int CurrentPartAlpha { get; set; }
        public float CurrentPartWidth { get; set; }
        public Font InformationFont { get; set; }
        public Brush InformationBrush { get; set; }

        public event EventHandler Changed;
        public event EventHandler ChangedLight;

        public RegionBuilder(System.Windows.Forms.Form form)
        {
            form.MouseDown += this.Form_MouseDown;
            form.MouseMove += this.Form_MouseMove;
            form.MouseUp += this.Form_MouseUp;
        }

        public void Clear()
        {
            this.Data.Clear();
            this.indexPart = -1;
            this.UpdateCurrentIndexes();

            this.RaiseChanged();
        }

        public void Draw(Graphics graphics)
        {
            if (!this.IsDraw)
            {
                return;
            }

            for (var j = 0; j < this.Data.Count; ++j)
            {
                var pen = new Pen(Color.FromArgb(j == this.indexPart ? this.CurrentPartAlpha : this.PartAlpha, this.Color));

                var regionPart = this.Data[j];
                for (var i = 0; i < regionPart.Count; ++i)
                {
                    pen.Width = j == this.indexPart && i == this.indexPoint ? this.CurrentPartWidth : this.PartWidth;
                    var regionPoint = regionPart[i];
                    graphics.DrawRectangle(pen, (float)(regionPoint.X) - this.PointSize / 2, (float)(regionPoint.Y) - this.PointSize / 2, this.PointSize, this.PointSize);

                    pen.Width = j == this.indexPart && i == this.indexLine ? this.CurrentPartWidth : this.PartWidth;
                    var regionPointSource = regionPart[i];
                    var regionPointTarget = regionPart[i != regionPart.Count - 1 ? i + 1 : 0];
                    graphics.DrawLine(pen, (float)(regionPointSource.X), (float)(regionPointSource.Y), (float)(regionPointTarget.X), (float)(regionPointTarget.Y));
                }
            }
        }

        public void DrawInformation(Graphics graphics, float x, ref float y)
        {
            var font = this.InformationFont;
            var textHeight = graphics.MeasureString(" ", font).Height;
            var brush = this.InformationBrush;

            graphics.DrawString("To add point (points count in current part less then 3) click mouse left button in any place.", font, brush, x, y);
            y += textHeight;
            graphics.DrawString("To add point (points count in current part more then 3) click mouse left button on edge.", font, brush, x, y);
            y += textHeight;
            graphics.DrawString("To move point click mouse left button on point, hold and move.", font, brush, x, y);
            y += textHeight;
            graphics.DrawString("To delete point click mouse right button on point.", font, brush, x, y);
            y += textHeight;
            graphics.DrawString("To delete part delete all points in part.", font, brush, x, y);
            y += textHeight;
            y += textHeight;
            graphics.DrawString($"The region has {this.Data.Sum(part => part.Count)} points in {this.Data.Count} parts.", font, brush, x, y);
            y += textHeight;
        }

        private void RaiseChanged()
        {
            this.Changed?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseChangedLight()
        {
            this.ChangedLight?.Invoke(this, EventArgs.Empty);
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
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
                    if (this.Data[this.indexPart].Count == 0)
                    {
                        this.Data.RemoveAt(this.indexPart);
                    }
                    isInvalidate = true;
                }
            }

            if (isInvalidate)
            {
                this.UpdateCurrentIndexes();
                this.RaiseChanged();
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            this.mousePoint = e.Location;

            if (this.Data.Count == 0)
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
                var regionPart = this.Data[this.indexPart];
                regionPart[this.indexPoint] = new Point3D(e.X, e.Y, 0.0);
                this.updateOnMouseUp = true;
                isInvalidate = true;
            }

            if (isInvalidate)
            {
                this.RaiseChangedLight();
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.updateOnMouseUp)
            {
                this.updateOnMouseUp = false;
                this.RaiseChanged();
            }
        }

        private bool AddPoint(double x, double y)
        {
            if (this.indexPart < 0)
            {
                var regionPart = new List<Point3D> { new Point3D(x, y, 0.0) };
                this.Data.Add(regionPart);
                this.indexPart = 0;
            }
            else
            {
                var regionPart = this.Data[this.indexPart];
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
                        regionPart = this.Data[this.Data.Count - 1];
                        if (regionPart.Count >= 3)
                        {
                            regionPart = new List<Point3D>();
                        }
                        regionPart.Add(new Point3D(x, y, 0.0));
                        this.Data.Add(regionPart);
                        this.indexPart = this.Data.Count - 1;
                    }
                }
            }
            return true;
        }

        private bool DelPoint()
        {
            if (this.indexPart < 0)
            {
                return false;
            }
            if (this.indexPoint < 0)
            {
                return false;
            }

            var regionPart = this.Data[this.indexPart];
            regionPart.RemoveAt(this.indexPoint);
            return true;
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
            var resultIndexPart = -1;
            var resultIndexPoint = -1;

            if (0 < this.indexPart && this.indexPart < this.Data.Count)
            {
                var i = this.indexPart;
                resultIndexPoint = this.FindCurrentPointIndex(i);
                if (resultIndexPoint >= 0)
                {
                    resultIndexPart = i;
                }
            }

            for (var i = 0; i < this.Data.Count && resultIndexPoint < 0; ++i)
            {
                resultIndexPoint = this.FindCurrentPointIndex(i);
                if (resultIndexPoint >= 0)
                {
                    resultIndexPart = i;
                }
            }

            return Tuple.Create(resultIndexPart, resultIndexPoint);
        }

        private int FindCurrentPointIndex(int inputIndexPart)
        {
            var resultIndexPoint = -1;
            var regionPart = this.Data[inputIndexPart];
            for (var i = 0; i < regionPart.Count; ++i)
            {
                var regionPoint = regionPart[i];
                if (Math.Abs(this.mousePoint.X - regionPoint.X) <= this.PointSize / 2 && Math.Abs(this.mousePoint.Y - regionPoint.Y) <= this.PointSize / 2)
                {
                    resultIndexPoint = i;
                    break;
                }
            }
            return resultIndexPoint;
        }

        private Tuple<int, int> FindCurrentLineIndex()
        {
            var resultIndexPart = -1;
            var resultIndexLine = -1;

            if (0 < this.indexPart && this.indexPart < this.Data.Count)
            {
                var i = this.indexPart;
                resultIndexLine = this.FindCurrentLineIndex(i);
                if (resultIndexLine >= 0)
                {
                    resultIndexPart = i;
                }
            }

            for (var i = 0; i < this.Data.Count && resultIndexLine < 0; ++i)
            {
                resultIndexLine = this.FindCurrentLineIndex(i);
                if (resultIndexLine >= 0)
                {
                    resultIndexPart = i;
                }
            }

            return Tuple.Create(resultIndexPart, resultIndexLine);
        }

        private int FindCurrentLineIndex(int inputIndexPart)
        {
            var resultIndexLine = -1;
            var regionPart = this.Data[inputIndexPart];
            for (var i = 0; i < regionPart.Count; ++i)
            {
                var regionPointSource = regionPart[i];
                var regionPointTarget = regionPart[i != regionPart.Count - 1 ? i + 1 : 0];
                if (
                    Math.Abs(this.mousePoint.X - (regionPointSource.X + regionPointTarget.X) / 2) < Math.Abs(regionPointSource.X - regionPointTarget.X) / 2 &&
                    Math.Abs(this.mousePoint.Y - (regionPointSource.Y + regionPointTarget.Y) / 2) < Math.Abs(regionPointSource.Y - regionPointTarget.Y) / 2)
                {
                    var vx = regionPointTarget.X - regionPointSource.X;
                    var vy = regionPointTarget.Y - regionPointSource.Y;
                    var d = Math.Sqrt(vx * vx + vy * vy);
                    if (d != 0)
                    {
                        vx /= d;
                        vy /= d;
                        if (Math.Abs((this.mousePoint.X - regionPointSource.X) * vy - (this.mousePoint.Y - regionPointSource.Y) * vx) <= this.PointSize / 2)
                        {

                            resultIndexLine = i;
                            break;
                        }
                    }
                }
            }
            return resultIndexLine;
        }
    }
}
