using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace LockBitsTest
{
    public partial class MainForm : Form
    {
        private Stopwatch commonStopwatch = new Stopwatch();
        private long commonTime = 0;
        private Stopwatch renderStopwatch = new Stopwatch();
        private long renderTime = 0;
        private Stopwatch updateStopwatch = new Stopwatch();
        private long updateTime = 0;

        private readonly FastBitmap fastBitmap = new FastBitmap();
        private readonly RazorBitmap razorBitmap = null;
        private bool isRazor = false;

        public MainForm()
        {
            InitializeComponent();

            SimpleParticlesWorld.Count = 1000000;

            this.razorBitmap = new RazorBitmap(this.CreateGraphics()) { Size = this.ClientSize };
            this.Disposed += (sender, e) => this.razorBitmap.Dispose();
        }

        private void SetRazorStyle(bool isRazor)
        {
            this.SetStyle(ControlStyles.DoubleBuffer, !isRazor);
            this.SetStyle(ControlStyles.UserPaint, isRazor);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, isRazor);
            this.SetStyle(ControlStyles.Opaque, isRazor);
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.razorBitmap != null)
            {
                this.razorBitmap.Size = (sender as Control).ClientSize;
            }
            this.fastBitmap.Size = (sender as Control).ClientSize;
            SimpleParticlesWorld.Size = this.ClientSize;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            this.fastBitmap.RefreshImage();
            e.Graphics.DrawImage(this.fastBitmap.Image, 0, 0);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.commonStopwatch.Restart();

            this.updateStopwatch.Restart();
            SimpleParticlesWorld.Update();
            this.updateTime = this.updateStopwatch.ElapsedMilliseconds;

            this.renderStopwatch.Restart();

            if (this.isRazor)
            {
                this.fastBitmap.Clear();
                foreach (SimpleParticle particle in SimpleParticlesWorld.Particles)
                {
                    this.fastBitmap.SetPixel((int)particle.x, (int)particle.y, particle.c);
                }
                this.Invalidate();
                this.Update();
            }
            else
            {
                this.razorBitmap.Clear();
                foreach (SimpleParticle particle in SimpleParticlesWorld.Particles)
                {
                    this.razorBitmap.SetPixel((int)particle.x, this.razorBitmap.Size.Height - (int)particle.y, particle.c);
                }
                this.razorBitmap.Draw();
            }

            this.renderTime = this.renderStopwatch.ElapsedMilliseconds;

            this.commonTime = this.commonStopwatch.ElapsedMilliseconds;

            this.Text = string.Format("Points count: {0}. Update time: {1}. Render time: {2}. Common time: {3}. Is razor: {4}.", SimpleParticlesWorld.Count, this.updateTime, this.renderTime, this.commonTime, this.isRazor);
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SimpleParticlesWorld.Init();
            }
            else
            {
                SimpleParticlesWorld.ActionIn(e.X, e.Y);
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.Enter:
                    this.isRazor = !this.isRazor;
                    this.SetRazorStyle(this.isRazor);
                    break;
            }
        }
    }
}
