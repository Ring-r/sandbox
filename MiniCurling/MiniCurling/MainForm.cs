using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MiniCurling
{
    public partial class MainForm : Form
    {
        private readonly Random random = new Random();

        private readonly Controller controller = new Controller();

        private const float radius = 32; // Get percent from window sizes?
        private readonly Source source = new Source(radius);
        private readonly Target target = new Target(2 * radius);

        private float bx0, by0;
        private float bx1, by1;

        private void InitGame()
        {
            this.bx0 = this.random.Next(this.ClientSize.Width);
            this.bx1 = this.random.Next(this.ClientSize.Width);
            this.by0 = this.random.Next(this.ClientSize.Height);
            this.by1 = this.random.Next(this.ClientSize.Height);

            this.source.Init(this.random, this.ClientSize.Width, this.ClientSize.Height);

            this.InitRound();
        }
        private void InitRound()
        {
            this.target.Init(this.random, this.ClientSize.Width, this.ClientSize.Height);
        }

        public MainForm()
        {
            this.InitializeComponent();

            this.InitGame();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.F5:
                    this.InitGame();
                    break;
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            this.target.Draw(e.Graphics, Brushes.Red);

            e.Graphics.DrawLine(Pens.Black, this.bx0, this.by0, this.bx1, this.by1);

            this.source.Draw(e.Graphics, Brushes.Blue);

            this.controller.Draw(e.Graphics, Pens.Silver);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.ClientSize.Width < radius || this.ClientSize.Height < radius)
            {
                return;
            }

            this.source.Move(this.ClientSize.Width, this.ClientSize.Height);

            bool isMove = this.source.IsMove();
            bool isTarget = Circle.Distance(this.source, this.target) < 0;
            if (!isMove && isTarget)
            {
                this.InitRound();
            }

            this.Invalidate();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.controller.SetSource(e.X, e.Y);
        }
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            this.controller.SetTarget(e.X, e.Y);
        }
        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            int x = this.controller.GetVectorX();
            int y = this.controller.GetVectorY();
            this.source.Push(x, y);
            this.controller.Clear();
        }
    }
}
