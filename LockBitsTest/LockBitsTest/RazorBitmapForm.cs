using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LockBitsTest
{
    public partial class RazorBitmapForm : Form
    {
        private Stopwatch commonStopwatch = new Stopwatch();
        private long commonTime = 0;
        private Stopwatch renderStopwatch = new Stopwatch();
        private long renderTime = 0;
        private Stopwatch updateStopwatch = new Stopwatch();
        private long updateTime = 0;

        private readonly RazorBitmap razorBitmap = null;

        public RazorBitmapForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, false);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);

            SimpleParticlesWorld.Size = this.ClientSize;

            this.razorBitmap = new RazorBitmap(this.CreateGraphics()) { Size = this.ClientSize };
            this.Disposed += (sender, e) => this.razorBitmap.Dispose();
            this.Form_SizeChanged(this, EventArgs.Empty);
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            SimpleParticlesWorld.ActionIn(e.X, e.Y);
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            if (this.razorBitmap != null)
            {
                this.razorBitmap.Size = (sender as Control).ClientSize;
                SimpleParticlesWorld.Size = this.razorBitmap.Size;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.commonStopwatch.Restart();

            this.updateStopwatch.Restart();
            SimpleParticlesWorld.Update();
            this.updateTime = this.updateStopwatch.ElapsedMilliseconds;

            this.renderStopwatch.Restart();
            this.razorBitmap.Clear();
            foreach (SimpleParticle particle in SimpleParticlesWorld.Particles)
            {
                this.razorBitmap.SetPixel((int)particle.x, this.razorBitmap.Size.Height - (int)particle.y, particle.c);
            }
            this.razorBitmap.Draw();
            this.renderTime = this.renderStopwatch.ElapsedMilliseconds;

            this.commonTime = this.commonStopwatch.ElapsedMilliseconds;

            this.Text = string.Format("Razor Bitmap. Points count: {0}. Update time: {1}. Render time: {2}. Common time: {3}.", SimpleParticlesWorld.Count, this.updateTime, this.renderTime, this.commonTime);
        }
    }
}
