using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MetroPenguinTest
{
    public partial class MainForm : Form
    {
        private readonly Random rand = new Random();
        private readonly Stopwatch stopwatch = new Stopwatch();

        private readonly Penguin penguin = new Penguin();
        private readonly List<Penguin> penguins = new List<Penguin>();

        private readonly Pen penPenguin = new Pen(Color.FromArgb(255, 0, 0, 255));
        private readonly Brush brushPenguin = new SolidBrush(Color.FromArgb(150, 0, 255, 0));
        private readonly Pen penPenguins = new Pen(Color.FromArgb(255, 255, 0, 0));
        private readonly Brush brushPenguins = new SolidBrush(Color.FromArgb(150, 255, 0, 0));
        private readonly Brush brushText = new SolidBrush(Color.FromArgb(150, 0, 0, 0));

        private PointF start = new Point();
        private PointF finish = new Point();
        private readonly Pen penFinish = new Pen(Color.Blue) { DashStyle = DashStyle.Dash };

        private bool isPause = false;
        private readonly List<PointF> path = new List<PointF>();
        private int currentIndex = 0;

        private int collisionsCount = 0;

        private bool isShowInfo = true;

        public MainForm()
        {
            InitializeComponent();
        }

        private void create()
        {
            int r = this.rand.Next(Options.minR, Options.maxR);
            float x = r + (this.ClientSize.Width - 2 * r) * (float)this.rand.NextDouble();
            float y = r + (this.ClientSize.Height - 2 * r) * (float)this.rand.NextDouble();
            this.penguin.x = x;
            this.penguin.y = y;
            this.penguin.vx = 0;
            this.penguin.vy = 0;
            this.penguin.r = r;
            this.penguin.s = Options.maxS;

            float a;
            float s;

            this.penguins.Clear();
            for (int i = 0; i < Options.startCount; ++i)
            {
                r = this.rand.Next(Options.minR, Options.maxR);
                x = r + (this.ClientSize.Width - 2 * r) * (float)this.rand.NextDouble();
                y = r + (this.ClientSize.Height - 2 * r) * (float)this.rand.NextDouble();

                a = (float)(2 * Math.PI * this.rand.NextDouble());
                //if (this.rand.Next(2) == 0)
                //{
                //    a = 0;
                //}
                //else
                //{
                //    a = (float)Math.PI;
                //}

                s = Options.minS + (Options.maxS - Options.minS) * (float)this.rand.NextDouble();

                this.penguins.Add(new Penguin() { x = x, y = y, vx = (float)Math.Cos(a), vy = (float)Math.Sin(a), s = s, r = r });
            }

            this.start = new PointF(this.penguin.x, this.penguin.y);
            this.finish = new PointF(this.penguin.r + (this.ClientSize.Width - 2 * this.penguin.r) * (float)this.rand.NextDouble(), this.penguin.r + (this.ClientSize.Height - 2 * this.penguin.r) * (float)this.rand.NextDouble());

            this.path.Clear();
            this.path.Add(new PointF(this.penguin.x, this.penguin.y));
            this.currentIndex = 0;

            this.collisionsCount = 0;

            this.timer.Interval = 1000 / Options.needFPS;
            this.timer.Start();
        }

        private void collision(Penguin penguin, Penguin penguinOther)
        {
            float vx = penguinOther.x - penguin.x;
            float vy = penguinOther.y - penguin.y;
            float d = (float)Math.Sqrt(vx * vx + vy * vy);
            if (d == 0)
            {
                penguinOther.x -= Options.eps;
                penguinOther.x += Options.eps;
                vx = 2 * Options.eps;
                d = 2 * Options.eps;
            }
            float r = penguinOther.r + penguin.r;

            if (d < r - Options.eps)
            {
                vx /= d;
                vy /= d;

                vx *= (r - d) * 0.5f;
                vy *= (r - d) * 0.5f;

                penguinOther.x += vx;
                penguinOther.y += vy;
                penguin.x -= vx;
                penguin.y -= vy;

                penguinOther.isCollide = true;
                penguin.isCollide = true;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            float timeEllapsed = 0.001f * this.stopwatch.ElapsedMilliseconds;
            this.stopwatch.Restart();

            if (!this.isPause)
            {
                Penguin penguin;
                float step;

                #region Moving penguins.
                for (var i = 0; i < this.penguins.Count; ++i)
                {
                    penguin = this.penguins[i];
                    step = penguin.s * timeEllapsed;
                    penguin.x += penguin.vx * step;
                    penguin.y += penguin.vy * step;
                }
                #endregion Moving penguins.

                #region Moving penguin (check points).
                step = this.penguin.s * timeEllapsed;
                if (this.currentIndex < this.path.Count)
                {
                    float x = this.path[this.currentIndex].X - this.penguin.x;
                    float y = this.path[this.currentIndex].Y - this.penguin.y;
                    float d = (float)Math.Sqrt(x * x + y * y);
                    if (d <= step)
                    {
                        this.penguin.x = this.path[this.currentIndex].X;
                        this.penguin.y = this.path[this.currentIndex].Y;
                        if (this.currentIndex < this.path.Count - 1)
                        {
                            this.currentIndex++;
                        }
                        else
                        {
                            this.penguin.vx = 0;
                            this.penguin.vy = 0;
                        }
                    }
                    else
                    {
                        this.penguin.vx = x / d;
                        this.penguin.vy = y / d;

                        this.penguin.x += this.penguin.vx * step;
                        this.penguin.y += this.penguin.vy * step;
                    }
                }
                #endregion Moving penguin (check points).

                this.penguin.isCollide = false;
                for (int j = 0; j < this.penguins.Count; ++j)
                {
                    this.collision(this.penguin, this.penguins[j]);
                }
                if (this.penguin.isCollide && this.penguin.s != 0 && !(this.penguin.vx == 0 && this.penguin.vy == 0))
                {
                    this.collisionsCount++;
                }

                #region Collision between penguins.
                // TODO: (R) Need to decrees complexity.
                for (var i = 0; i < this.penguins.Count - 1; ++i)
                {
                    for (var j = i + 1; j < this.penguins.Count; ++j)
                    {
                        this.collision(this.penguins[i], this.penguins[j]);
                    }
                }
                #endregion Collision between penguins.

                #region Collision with borders.
                if (this.penguin.x < this.penguin.r)
                {
                    this.penguin.x = this.penguin.r;
                    this.penguin.vx = Math.Abs(this.penguin.vx);
                }
                if (this.penguin.x > this.ClientSize.Width - this.penguin.r)
                {
                    this.penguin.x = this.ClientSize.Width - this.penguin.r;
                    this.penguin.vx = -Math.Abs(this.penguin.vx);
                }

                if (this.penguin.y < this.penguin.r)
                {
                    this.penguin.y = this.penguin.r;
                    this.penguin.vy = Math.Abs(this.penguin.vy);
                }
                if (this.penguin.y > this.ClientSize.Height - this.penguin.r)
                {
                    this.penguin.y = this.ClientSize.Height - this.penguin.r;
                    this.penguin.vy = -Math.Abs(this.penguin.vy);
                }

                for (var i = 0; i < this.penguins.Count; ++i)
                {
                    penguin = this.penguins[i];

                    if (penguin.x < penguin.r)
                    {
                        penguin.x = penguin.r;
                        penguin.vx = Math.Abs(penguin.vx);
                    }
                    if (penguin.x > this.ClientSize.Width - penguin.r)
                    {
                        penguin.x = this.ClientSize.Width - penguin.r;
                        penguin.vx = -Math.Abs(penguin.vx);
                    }

                    if (penguin.y < penguin.r)
                    {
                        penguin.y = penguin.r;
                        penguin.vy = Math.Abs(penguin.vy);
                    }
                    if (penguin.y > this.ClientSize.Height - penguin.r)
                    {
                        penguin.y = this.ClientSize.Height - penguin.r;
                        penguin.vy = -Math.Abs(penguin.vy);
                    }
                }
                #endregion Collision with borders.
            }
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (this.penguins.Count > 0)
            {
                Penguin penguin;
                for (int i = 0; i < this.penguins.Count; ++i)
                {
                    penguin = this.penguins[i];
                    e.Graphics.FillEllipse(this.brushPenguins, penguin.x - penguin.r, penguin.y - penguin.r, 2 * penguin.r, 2 * penguin.r);
                    e.Graphics.DrawLine(this.penPenguins, penguin.x, penguin.y, penguin.x + penguin.r * penguin.vx, penguin.y + penguin.r * penguin.vy);
                }

                e.Graphics.FillEllipse(this.brushPenguin, this.penguin.x - this.penguin.r, this.penguin.y - this.penguin.r, 2 * this.penguin.r, 2 * this.penguin.r);
                e.Graphics.DrawLine(this.penPenguin, this.penguin.x, this.penguin.y, this.penguin.x + this.penguin.r * this.penguin.vx, this.penguin.y + this.penguin.r * this.penguin.vy);

                e.Graphics.DrawEllipse(this.penFinish, this.finish.X - 2 * this.penguin.r, this.finish.Y - 2 * this.penguin.r, 4 * this.penguin.r, 4 * this.penguin.r);
            }

            for (int i = this.currentIndex; i < this.path.Count; ++i)
            {
                e.Graphics.FillEllipse(this.brushPenguin, this.path[i].X - Options.pointR, this.path[i].Y - Options.pointR, 2 * Options.pointR, 2 * Options.pointR);
            }

            if (this.isShowInfo)
            {
                e.Graphics.DrawString(string.Format("Collision count: {0}", this.collisionsCount), this.Font, this.brushText, 0, 30);
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.Enter:
                    this.create();
                    break;
                case Keys.F1:
                    this.isShowInfo = !this.isShowInfo;
                    break;
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    this.isPause = true;
                    this.path.Clear();
                    this.currentIndex = 0;
                    break;
                case MouseButtons.Right:
                    this.create();
                    break;
            }
            // TODO:
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isPause)
            {
                this.path.Add(e.Location);
            }
            // TODO:
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    this.isPause = false;
                    this.stopwatch.Restart();
                    this.collisionsCount = 0;
                    break;
            }
        }
    }
}
