using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace LockBitsTest
{
    public partial class MainForm : Form
    {
        private readonly Random rand = new Random();
        private int count = 1000000;
        private readonly List<Particle> particles = new List<Particle>();
        private FastBitmap bitmap = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (this.bitmap != null)
            {
                using (Bitmap image = bitmap.Image)
                {
                    e.Graphics.DrawImage(image, 0, 0);
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Stopwatch stopwatch_render = new Stopwatch();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (this.bitmap != null)
            {
                try
                {
                    foreach (Particle particle in particles)
                    {
                        particle.x += particle.vx;
                        particle.y += particle.vy;

                        if (particle.x < 0)
                        {
                            particle.x = 0;
                            particle.vx = Math.Abs(particle.vx);
                        }
                        else if (this.ClientSize.Width <= particle.x)
                        {
                            particle.x = this.ClientSize.Width - 1;
                            particle.vx = -Math.Abs(particle.vx);
                        }
                        if (particle.y < 0)
                        {
                            particle.y = 0;
                            particle.vy = Math.Abs(particle.vy);
                        }
                        else if (this.ClientSize.Height <= particle.y)
                        {
                            particle.y = this.ClientSize.Height - 1;
                            particle.vy = -Math.Abs(particle.vy);
                        }
                    }

                    this.bitmap.Clear();
                    foreach (Particle particle in this.particles)
                    {
                        this.bitmap.SetPixel((int)particle.x, (int)particle.y, particle.c);
                    }

                    stopwatch_render.Start();
                    this.Invalidate();
                    //stopwatch_render.Stop();
                }
                catch { }
            }

            stopwatch.Stop();
            this.Text = string.Format("Points count: {0}. Update time: {1}. Render time: {2}.", this.count, stopwatch.ElapsedMilliseconds, stopwatch_render.ElapsedMilliseconds);
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.bitmap = new FastBitmap(this.ClientSize.Width, this.ClientSize.Height);
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.particles.Clear();
                for (int i = 0; i < this.count; i++)
                {
                    this.particles.Add(new Particle()
                    {
                        x = rand.Next(this.ClientSize.Width),
                        y = rand.Next(this.ClientSize.Height),
                        vx = 2 * (float)rand.NextDouble() - 1,
                        vy = 2 * (float)rand.NextDouble() - 1,
                        c = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255))
                    });
                }
            }
            else
            {
                foreach (Particle particle in particles)
                {
                    float x = particle.x - e.X;
                    float y = particle.y - e.Y;
                    float distance = (float)Math.Sqrt(x * x + y * y);
                    if (distance == 0)
                    {
                        distance = 1;
                    }
                    float speed = -(10 * (float)rand.NextDouble() + 1) / distance;
                    particle.vx = speed * x;
                    particle.vy = speed * y;
                }
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
    }
}
