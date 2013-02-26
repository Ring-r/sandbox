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
        private readonly Client client = new Client();
        private readonly Server server = new Server();

        //private List<PointF> dots = new List<PointF>();
        //private int currentDotsIndex = 0;
        //private List<int> dotsStack = new List<int>();
        //private int[] dotsChecker = null;

        private MilkyMan milkyMan = new MilkyMan() { Speed = 500, Radius = 3 };
        private MilkyMan milkyManOpponent = new MilkyMan() { Speed = 600, Radius = 3 };

        private Point mouseMovePoint = new Point();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


            if (Options.IsFinish)
            {
                e.Graphics.DrawString(string.Format("Length of path is {0}.", Options.CurrentDistance), this.Font, Brushes.Black, new PointF(0, 0));
                // TODO: Find condition for win.
                //if (Options.CurrentDistance <= Options.opponentDistance + 0.1)
                //{
                //    e.Graphics.DrawString("You win.", this.Font, Brushes.Black, new PointF(0, 90));
                //}
                //else
                //{
                //    e.Graphics.DrawString("You lose.", this.Font, Brushes.Black, new PointF(0, 90));
                //}
            }
            else
            {
                e.Graphics.DrawLine(Pens.Yellow, Options.Dots[this.milkyMan.dotsIndex], this.mouseMovePoint);
            }
            for (int i = 0; i < Options.Dots.Length; ++i)
            {
                PointF dot = Options.Dots[i];
                e.Graphics.FillEllipse(Brushes.Silver, dot.X - Options.DotsRadius, dot.Y - Options.DotsRadius, 2 * Options.DotsRadius, 2 * Options.DotsRadius);
            }
            e.Graphics.FillEllipse(Brushes.Green, Options.Dots[0].X - Options.DotsRadius, Options.Dots[0].Y - Options.DotsRadius, 2 * Options.DotsRadius, 2 * Options.DotsRadius);
            e.Graphics.FillEllipse(Brushes.Red, Options.Dots[Options.DotsCount - 1].X - Options.DotsRadius, Options.Dots[Options.DotsCount - 1].Y - Options.DotsRadius, 2 * Options.DotsRadius, 2 * Options.DotsRadius);
            this.milkyMan.onManagedDraw(e.Graphics);
            this.milkyManOpponent.onManagedDraw(e.Graphics);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.Enter:
                    this.server.DotsInit();
                    this.client.DotsInit();
                    this.Invalidate();
                    break;
                case Keys.F1:
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
                for (int i = 0; i < Options.Dots.Length; ++i)
                {
                    PointF dot = Options.Dots[i];
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
                    }
                    if (Options.IsFinish)
                    {
                        this.milkyMan.Dots.Clear();
                        for (int i = 0; i < Options.DotsCount; ++i)
                        {
                            this.milkyMan.Dots.Add(this.dots[this.dotsStack[i]]);
                        }
                        this.milkyMan.dotsIndex = -1;
                        this.milkyManOpponent.Dots.Clear();
                        for (int i = 0; i < Options.DotsCount; ++i)
                        {
                            //this.milkyManOpponent.Dots.Add(this.dots[Options.DotsStackOpponent[i]]);
                        }
                        this.milkyManOpponent.dotsIndex = -1;
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

        private void timer_Tick(object sender, EventArgs e)
        {
            this.milkyMan.onManagedUpdate(this.timer.Interval / 1000f);
            this.milkyManOpponent.onManagedUpdate(this.timer.Interval / 1000f);
            this.Invalidate();
        }
    }
}
