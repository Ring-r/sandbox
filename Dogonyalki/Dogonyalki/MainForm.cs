using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Dogonyalki
{
    public partial class MainForm : Form
    {
        private readonly Hero hero = new Hero();
        private readonly Random random = new Random();
        private const int pointsCount = 10;
        private readonly List<Point> points = new List<Point>(pointsCount);
        private readonly bool[] keys = new bool[12];

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.hero.Init(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
            for (int i = 0; i < pointsCount; ++i)
            {
                this.points.Add(new Point(random.Next(this.ClientSize.Width), random.Next(this.ClientSize.Height)));
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TranslateTransform(0, this.ClientSize.Height);
            e.Graphics.ScaleTransform(1, -1);

            float x = 0.5f * (this.hero.x_ + this.hero._x);
            float y = 0.5f * (this.hero.y_ + this.hero._y);
            float a = (float)Math.Atan2(this.hero._y - this.hero.y_, this.hero._x - this.hero.x_);

            e.Graphics.TranslateTransform(-x, -y);
            e.Graphics.TranslateTransform(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform(-(float)(a / Math.PI * 180));
            e.Graphics.TranslateTransform(-x, -y);


            for (int i = 0; i < pointsCount; ++i)
            {
                e.Graphics.FillEllipse(Brushes.Green, this.points[i].X - 3, this.points[i].Y - 3, 6, 6);
            }

            this.hero.Draw(e.Graphics, Pens.Black);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.W:
                    this.keys[0] = true;
                    break;
                case Keys.A:
                    this.keys[1] = true;
                    break;
                case Keys.D:
                    this.keys[2] = true;
                    break;
                case Keys.S:
                    this.keys[3] = true;
                    break;
                case Keys.I:
                    this.keys[4] = true;
                    break;
                case Keys.J:
                    this.keys[5] = true;
                    break;
                case Keys.L:
                    this.keys[6] = true;
                    break;
                case Keys.K:
                    this.keys[7] = true;
                    break;
                case Keys.Up:
                    this.keys[8] = true;
                    break;
                case Keys.Left:
                    this.keys[9] = true;
                    break;
                case Keys.Right:
                    this.keys[10] = true;
                    break;
                case Keys.Down:
                    this.keys[11] = true;
                    break;
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.W:
                    this.keys[0] = false;
                    break;
                case Keys.A:
                    this.keys[1] = false;
                    break;
                case Keys.D:
                    this.keys[2] = false;
                    break;
                case Keys.S:
                    this.keys[3] = false;
                    break;
                case Keys.I:
                    this.keys[4] = false;
                    break;
                case Keys.J:
                    this.keys[5] = false;
                    break;
                case Keys.L:
                    this.keys[6] = false;
                    break;
                case Keys.K:
                    this.keys[7] = false;
                    break;
                case Keys.Up:
                    this.keys[8] = false;
                    break;
                case Keys.Left:
                    this.keys[9] = false;
                    break;
                case Keys.Right:
                    this.keys[10] = false;
                    break;
                case Keys.Down:
                    this.keys[11] = false;
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            float s = this.hero.r;
            this.hero.vx_ = 0; this.hero.vy_ = 0; this.hero._vx = 0; this.hero._vy = 0;
            if (this.keys[0]) this.hero.vy_ += s;
            if (this.keys[1]) this.hero.vx_ -= s;
            if (this.keys[2]) this.hero.vx_ += s;
            if (this.keys[3]) this.hero.vy_ -= s;
            if (this.keys[4]) this.hero._vy += s;
            if (this.keys[5]) this.hero._vx -= s;
            if (this.keys[6]) this.hero._vx += s;
            if (this.keys[7]) this.hero._vy -= s;

            if (this.keys[8]) { this.hero.vy_ += s; this.hero._vy += s; }
            if (this.keys[9]) this.hero._vy += s;
            if (this.keys[10]) this.hero.vy_ += s;
            if (this.keys[11]) { this.hero.vy_ -= s; this.hero._vy -= s; }

            this.hero.Move();

            if (this.hero.x_ < 0 && this.hero.vx_ < 0 || this.hero.x_ > this.ClientSize.Width && this.hero.vx_ > 0)
                this.hero.vx_ = -this.hero.vx_;
            if (this.hero.y_ < 0 && this.hero.vy_ < 0 || this.hero.y_ > this.ClientSize.Height && this.hero.vy_ > 0)
                this.hero.vy_ = -this.hero.vy_;
            if (this.hero._x < 0 && this.hero._vx < 0 || this.hero._x > this.ClientSize.Width && this.hero._vx > 0)
                this.hero._vx = -this.hero._vx;
            if (this.hero._y < 0 && this.hero._vy < 0 || this.hero._y > this.ClientSize.Height && this.hero._vy > 0)
                this.hero._vy = -this.hero._vy;

            this.Invalidate();
        }
    }
}
