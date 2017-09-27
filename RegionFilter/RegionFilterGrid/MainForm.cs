using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AlgorithmsAndDataStructures;

namespace RegionFilter
{
    public partial class MainForm : Form
    {
        #region Private fields.

        private readonly RegionBuilder regionBuilder;

        private long calculationTime;

        private bool isDrawStrings = true;
        private readonly Brush stringsBrush = Brushes.Yellow;


        private readonly GridWithGeometry<bool> grid = new GridWithGeometry<bool>(0, 0);
        private readonly GridWithGeometryViewer gridWithGeometryViewer;

        #endregion Private fields.

        private void Calculate()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            this.grid.ReInitialize((int)(this.ClientSize.Width / this.grid.IStepSize) + 1, (int)(this.ClientSize.Height / this.grid.JStepSize) + 1);
            this.grid.Border(this.regionBuilder.Data);

            var startIndex = 0;
            while (startIndex < this.grid.CellsCount && !Convert.ToBoolean(this.grid[startIndex]))
            {
                ++startIndex;
            }
            if (startIndex != this.grid.CellsCount)
            {
                var gridVector = this.grid.Border(startIndex);
                this.grid.SetValue(true, gridVector);
            }

            stopwatch.Stop();
            this.calculationTime = stopwatch.ElapsedMilliseconds;
        }

        private void DrawStrings(Graphics graphics)
        {
            var textY = 0f;
            graphics.DrawString("F1 - help", this.Font, this.stringsBrush, 0.0f, textY);

            if (!this.isDrawStrings)
            {
                return;
            }

            var textHeight = graphics.MeasureString(" ", this.Font).Height;

            textY += textHeight; graphics.DrawString("F2 - show/hide contour", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("F5 - clean", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight; graphics.DrawString("To add point (points count in current part less then 3) click mouse left button in any place.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("To add point (points count in current part more then 3) click mouse left button on edge.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("To move point click mouse left button on point, hold and move.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("To delete point click mouse right button on point.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight; graphics.DrawString("To delete part delete all points in part.", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight; graphics.DrawString($"Count of region points: {this.regionBuilder.Data.Sum(part => part.Count)}", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight; graphics.DrawString($"Calculation time: {this.calculationTime} milliseconds", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight; graphics.DrawString("Exc - close", this.Font, this.stringsBrush, 0.0f, textY);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.grid.ReInitialize((int)(this.ClientSize.Width / this.grid.IStepSize) + 1, (int)(this.ClientSize.Height / this.grid.JStepSize) + 1);

            this.Calculate();
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            this.gridWithGeometryViewer.Draw(this.grid, e.Graphics, this.ClientSize);

            this.regionBuilder.Draw(e.Graphics);

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
                this.regionBuilder.IsDraw = !this.regionBuilder.IsDraw;

                this.Invalidate();
            }
            if (e.KeyData == Keys.F5)
            {
                this.regionBuilder.Clear();
            }
        }

        private void RegionBuilderChanged(object sender, EventArgs e)
        {
            this.Calculate();
            this.Invalidate();
        }

        private void RegionBuilderChangedLight(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        public MainForm()
        {
            this.regionBuilder = new RegionBuilder(this)
            {
                PointSize = 20.0f,
                IsDraw = true,
                Color = Color.Red,
                PartAlpha = 100,
                PartWidth = 1.0f,
                CurrentPartAlpha = 255,
                CurrentPartWidth = 2.0f,
            };
            this.regionBuilder.Changed += this.RegionBuilderChanged;
            this.regionBuilder.ChangedLight += this.RegionBuilderChangedLight;

            this.grid.IStepSize = 15.2;
            this.grid.JStepSize = 14.8;

            this.gridWithGeometryViewer = new GridWithGeometryViewer
            {
                Pen = Pens.Gray,
                Brush = Brushes.Gray,
            };

            this.InitializeComponent();
        }
    }
}

