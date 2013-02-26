using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LockBitsTest
{
    public partial class FastBitmapForm : Form
    {
        private Stopwatch commonStopwatch = new Stopwatch();
        private long commonTime = 0;
        private Stopwatch renderStopwatch = new Stopwatch();
        private long renderTime = 0;
        private Stopwatch updateStopwatch = new Stopwatch();
        private long updateTime = 0;

        private readonly FastBitmap fastBitmap = new FastBitmap();

        public FastBitmapForm()
        {
            InitializeComponent();

            SimpleParticlesWorld.Size = this.ClientSize;
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

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            this.fastBitmap.RefreshImage();
            e.Graphics.DrawImage(this.fastBitmap.Image, 0, 0);
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            this.fastBitmap.Size = (sender as Control).ClientSize;
            SimpleParticlesWorld.Size = this.fastBitmap.Size;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.commonStopwatch.Restart();

            this.updateStopwatch.Restart();
            SimpleParticlesWorld.Update();
            this.updateTime = this.updateStopwatch.ElapsedMilliseconds;

            this.renderStopwatch.Restart();
            this.fastBitmap.Clear();
            foreach (SimpleParticle particle in SimpleParticlesWorld.Particles)
            {
                this.fastBitmap.SetPixel((int)particle.x, (int)particle.y, particle.c);
            }
            this.Invalidate();
            this.Update();
            this.renderTime = this.renderStopwatch.ElapsedMilliseconds;

            this.commonTime = this.commonStopwatch.ElapsedMilliseconds;

            this.Text = string.Format("Fast Bitmap. Points count: {0}. Update time: {1}. Render time: {2}. Common time: {3}.", SimpleParticlesWorld.Count, this.updateTime, this.renderTime, this.commonTime);
        }
    }
}
