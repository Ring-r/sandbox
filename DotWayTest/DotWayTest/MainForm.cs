using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace DotWayTest
{
    public partial class MainForm : Form
    {
        private Random random = new Random();

        private List<PointF> dots = new List<PointF>();
        private int currentDotsIndex = 0;
        private List<int> dotsStack = new List<int>();
        private int[] dotsChecker = null;

        private bool isHelper = false;

        private Point mouseMovePoint = new Point();

        private void CalculateDistances()
        {
            Options.opponentDistance = float.PositiveInfinity;
            Options.DotsStackOpponent.Clear();
            Options.DotsStackOpponent.Add(0);
            int[] dotsIndexes = new int[Options.DotsCount];
            dotsIndexes[0] = 1;
            for (int i = 0; i < Options.DotsCount - 2; ++i)
            {
                int k = 0;
                double md = float.PositiveInfinity;
                for (int j = 1; j < Options.DotsCount - 1; ++j)
                {
                    if (dotsIndexes[j] == 0)
                    {
                        float x = this.dots[Options.DotsStackOpponent[Options.DotsStackOpponent.Count - 1]].X - this.dots[j].X;
                        float y = this.dots[Options.DotsStackOpponent[Options.DotsStackOpponent.Count - 1]].Y - this.dots[j].Y;
                        double d = Math.Sqrt(x * x + y * y);
                        if (md > d)
                        {
                            md = d;
                            k = j;
                        }
                    }
                }
                Options.DotsStackOpponent.Add(k);
                dotsIndexes[k]++;
            }
            Options.DotsStackOpponent.Add(Options.DotsCount - 1);
        }

        private void DotsInit()
        {
            Options.DotsCount++;
            this.dots.Clear();
            for (int i = 0; i < Options.DotsCount; ++i)
            {
                float x = this.ClientSize.Width * (1 - 2 * Options.BorderSize) * (float)random.NextDouble() + this.ClientSize.Width * Options.BorderSize;
                float y = this.ClientSize.Height * (1 - 2 * Options.BorderSize) * (float)random.NextDouble() + this.ClientSize.Height * Options.BorderSize;
                this.dots.Add(new PointF(x, y));
            }

            this.CalculateDistances();

            Options.CurrentDistance = 0;
            this.currentDotsIndex = 0;
            this.dotsStack.Clear();
            this.dotsStack.Add(this.currentDotsIndex);
            this.dotsChecker = new int[Options.DotsCount];
            this.dotsChecker[this.currentDotsIndex]++;
            Options.IsFinish = false;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (this.dots.Count > 0)
            {
                if (this.isHelper)
                {
                    for (int i = 0; i < Options.DotsStackOpponent.Count - 1; ++i)
                    {
                        PointF p1 = this.dots[Options.DotsStackOpponent[i]];
                        PointF p2 = this.dots[Options.DotsStackOpponent[i + 1]];
                        e.Graphics.DrawLine(Pens.Green, p1, p2);
                    }
                }
                for (int i = 0; i < this.dotsStack.Count - 1; ++i)
                {
                    PointF p1 = this.dots[this.dotsStack[i]];
                    PointF p2 = this.dots[this.dotsStack[i + 1]];
                    e.Graphics.DrawLine(Pens.Yellow, p1, p2);
                }
                if (Options.IsFinish)
                {
                    e.Graphics.DrawString(string.Format("Length of path is {0}.", Options.CurrentDistance), this.Font, Brushes.Black, new PointF(0, 0));
                    e.Graphics.DrawString(string.Format("Length of min path is {0}.", Options.opponentDistance), this.Font, Brushes.Black, new PointF(0, 30));
                    if (Options.CurrentDistance <= Options.opponentDistance + 0.1)
                    {
                        e.Graphics.DrawString("You win.", this.Font, Brushes.Black, new PointF(0, 90));
                    }
                    else
                    {
                        e.Graphics.DrawString("You lose.", this.Font, Brushes.Black, new PointF(0, 90));
                    }
                }
                else
                {
                    e.Graphics.DrawLine(Pens.Yellow, this.dots[this.currentDotsIndex], this.mouseMovePoint);
                }
                for (int i = 0; i < this.dots.Count; ++i)
                {
                    PointF dot = this.dots[i];
                    e.Graphics.FillEllipse(Brushes.Silver, dot.X - Options.DotsRadius, dot.Y - Options.DotsRadius, 2 * Options.DotsRadius, 2 * Options.DotsRadius);
                }
                e.Graphics.FillEllipse(Brushes.Green, this.dots[0].X - Options.DotsRadius, this.dots[0].Y - Options.DotsRadius, 2 * Options.DotsRadius, 2 * Options.DotsRadius);
                e.Graphics.FillEllipse(Brushes.Red, this.dots[Options.DotsCount - 1].X - Options.DotsRadius, this.dots[Options.DotsCount - 1].Y - Options.DotsRadius, 2 * Options.DotsRadius, 2 * Options.DotsRadius);
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
                    this.DotsInit();
                    this.Invalidate();
                    break;
                case Keys.F1:
                    this.isHelper = !this.isHelper;
                    this.Invalidate();
                    break;
            }
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int indexMin = 0;
                double distanceMin = float.PositiveInfinity;
                for (int i = 0; i < this.dots.Count; ++i)
                {
                    PointF dot = this.dots[i];
                    float x = e.X - dot.X;
                    float y = e.Y - dot.Y;
                    double d = x * x + y * y;
                    if (distanceMin > d)
                    {
                        distanceMin = d;
                        indexMin = i;
                    }
                }
                if (distanceMin < Options.DotsRadius * Options.DotsRadius)
                {
                    this.currentDotsIndex = indexMin;
                    float x = this.dots[this.currentDotsIndex].X - this.dots[this.dotsStack[this.dotsStack.Count - 1]].X;
                    float y = this.dots[this.currentDotsIndex].Y - this.dots[this.dotsStack[this.dotsStack.Count - 1]].Y;
                    Options.CurrentDistance += (float)Math.Sqrt(x * x + y * y);
                    this.dotsStack.Add(this.currentDotsIndex);
                    this.dotsChecker[this.currentDotsIndex]++;
                    Options.IsFinish = this.dotsStack[this.dotsStack.Count - 1] == Options.DotsCount - 1;
                    for (int i = 0; i < this.dotsChecker.Length; ++i)
                    {
                        Options.IsFinish = Options.IsFinish && this.dotsChecker[i] > 0;
                        ind = 0;
                        this.x = this.dots[this.dotsStack[0]].X;
                        this.y = this.dots[this.dotsStack[0]].Y;
                        indOp = 0;
                        this.xOp = this.dots[Options.DotsStackOpponent[0]].X;
                        this.yOp = this.dots[Options.DotsStackOpponent[0]].Y;
                        this.timer.Start();
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                Options.CurrentDistance = 0;
                this.currentDotsIndex = 0;
                this.dotsStack.Clear();
                this.dotsStack.Add(this.currentDotsIndex);
                Options.IsFinish = false;
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            this.mouseMovePoint = e.Location;
            this.Invalidate();
        }

        private int ind = 0;
        private float x;
        private float y;
        private int indOp = 0;
        private float xOp;
        private float yOp;
        private void timer_Tick(object sender, EventArgs e)
        {
            //
        }
    }
}
