using AlgorithmsAndDataStructures;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Prototypes.Forms;
using Form = System.Windows.Forms.Form;

namespace RegionFilter
{
    public partial class MainForm : Form
    {
        private readonly RegionBuilder regionBuilder;
        private readonly IRegionFilter regionFilter = new AlgorithmsAndDataStructures.RegionFilter();

        private long preCalculateTime;
        private long calculateTime;

        private Bitmap bitmap = new Bitmap(1, 1);
        private readonly Color pointInRegionColor = Color.Green;
        private readonly Color pointOutRegionColor = Color.FromArgb(0);

        private bool isDrawStrings = true;
        private readonly Brush stringsBrush = Brushes.Yellow;

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
                InformationFont = this.Font,
                InformationBrush = this.stringsBrush,
            };
            this.regionBuilder.Changed += this.RegionBuilderChanged;
            this.regionBuilder.ChangedLight += this.RegionBuilderChangedLight;

            this.InitializeComponent();
        }

        private void PreCalculate()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            this.regionFilter.Init(this.regionBuilder.Data);
            stopwatch.Stop();
            this.preCalculateTime = stopwatch.ElapsedMilliseconds;
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
            this.calculateTime = stopwatch.ElapsedMilliseconds;

            var imageData = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), ImageLockMode.ReadWrite, this.bitmap.PixelFormat);
            Marshal.Copy(array, 0, imageData.Scan0, array.Length);
            this.bitmap.UnlockBits(imageData);
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

            textY += textHeight;
            graphics.DrawString("F2 - show/hide region", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            graphics.DrawString("F5 - clean", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight;
            this.regionBuilder.DrawInformation(graphics, 0.0f, ref textY);
            textY += textHeight;
            graphics.DrawString($"Count of verificated points: {this.ClientSize.Width * this.ClientSize.Height}", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            graphics.DrawString($"PreCalculation time: {this.preCalculateTime} milliseconds", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            graphics.DrawString($"Calculation time: {this.calculateTime} milliseconds", this.Font, this.stringsBrush, 0.0f, textY);
            textY += textHeight;
            textY += textHeight; graphics.DrawString("Exc - close", this.Font, this.stringsBrush, 0.0f, textY);
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.bitmap.Dispose();
        }

        private void RegionBuilderChanged(object sender, EventArgs e)
        {
            this.PreCalculate();
            this.Calculate();
            this.Invalidate();
        }

        private void RegionBuilderChangedLight(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}

