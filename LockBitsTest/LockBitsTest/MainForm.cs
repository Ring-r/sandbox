using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LockBitsTest
{
    public partial class MainForm : Form
    {
        Stopwatch renderStopwatch = new Stopwatch();
        long renderTime = 0;
        Stopwatch updateStopwatch = new Stopwatch();
        long updateTime = 0;

        private Size size = new Size(1, 1);
        private int[] array = new int[1];

        Task task = null;
        private bool IsTaskTerminate = false;

        public MainForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer, false);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            task = Task.Factory.StartNew(() =>
            {
                Graphics graphics = null;
                try
                {
                    graphics = this.CreateGraphics();

                    SimpleParticlesWorld.Count = 500000;
                    SimpleParticlesWorld.Init();

                    while (!IsTaskTerminate)
                    {
                        Stopwatch stopwatch_render = new Stopwatch();
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();

                        if (this.array != null)
                        {
                            SimpleParticlesWorld.Update();

                            Array.Clear(this.array, 0, this.array.Length);
                            foreach (SimpleParticle particle in SimpleParticlesWorld.Particles)
                            {
                                int pointBase = (int)particle.y * this.size.Width + (int)particle.x;
                                if (pointBase < this.array.Length)
                                {
                                    this.array[pointBase] = particle.c;
                                }
                            }

                            stopwatch.Stop();
                            if (!this.IsTaskTerminate)
                            {
                                this.Invoke((Action)(() =>
                                {
                                    this.renderStopwatch.Restart();
                                    Array.Clear(this.array, 0, this.array.Length);
                                    int pointBase;
                                    foreach (SimpleParticle particle in SimpleParticlesWorld.Particles)
                                    {
                                        pointBase = (int)particle.y * this.size.Width + (int)particle.x;
                                        if (pointBase < this.array.Length)
                                        {
                                            this.array[pointBase] = particle.c;
                                        }
                                    }

                                    using (Bitmap image = new Bitmap(this.size.Width, this.size.Height))// TODO:
                                    {
                                        BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
                                        Marshal.Copy(this.array, 0, imageData.Scan0, this.array.Length);
                                        image.UnlockBits(imageData);

                                        graphics.Clear(Color.White);
                                        graphics.DrawImage(image, 0, 0);
                                    }
                                    this.renderStopwatch.Stop();
                                    this.renderTime = this.renderStopwatch.ElapsedMilliseconds;
                                    this.Text = string.Format("Points count: {0}. Update time: {1}. Render time: {2}.", SimpleParticlesWorld.Count, stopwatch.ElapsedMilliseconds, stopwatch_render.ElapsedMilliseconds);
                                }));
                            }
                        }
                    }
                }
                finally
                {
                    if (graphics != null)
                    {
                        graphics.Dispose();
                    }
                }
            });
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.size = (sender as Control).ClientSize;
            this.array = new int[this.size.Width * this.size.Height];

            SimpleParticlesWorld.Size = this.ClientSize;
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
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.task != null)
            {
                this.IsTaskTerminate = true;
                this.task.Wait();
            }
        }
    }
}
