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
        private readonly Map map = new Map();
        private readonly MilkyMan milkyMan = new MilkyMan();

        private readonly Client client = new Client();
        private readonly Server server = new Server();

        private Point mouseMovePoint = new Point();

        public MainForm()
        {
            InitializeComponent();

            this.FillMilkyManData();
            this.map.milkyMan = this.milkyMan;

            this.server.map = this.map;

            this.client.SendConnect(this.server);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.Enter:
                    this.server.StartGame();
                    this.Invalidate();
                    break;
            }
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (Options.State == Options.StateEnum.Choose)
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.milkyMan.AddDotNear(e.Location);

                    if (this.milkyMan.IsFull)
                    {
                        this.client.SendMilkyMan(this.milkyMan);
                        Options.State = Options.StateEnum.Wait;
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (this.milkyMan.dotsStack.Count > 1)
                    {
                        this.milkyMan.dotsStack.RemoveAt(this.milkyMan.dotsStack.Count - 1);
                        this.milkyMan.dotsIndex = this.milkyMan.dotsStack[this.milkyMan.dotsStack.Count - 1];
                    }
                }
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            this.mouseMovePoint = e.Location;
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (Options.State == Options.StateEnum.Choose)
            {
                e.Graphics.DrawLine(Pens.Yellow, this.map.Dots[this.milkyMan.dotsIndex], this.mouseMovePoint);
            }
            this.map.onManagedDraw(e.Graphics);
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.map.Size = this.ClientSize;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.map.onManagedUpdate(this.timer.Interval / 1000f);
            this.Invalidate();
        }

        private void FillMilkyManData()
        {
            this.milkyMan.Speed = 500;
            this.milkyMan.Radius = 3;
            this.milkyMan.map = this.map;
            this.milkyMan.IsDrawWay = true;
        }
    }
}
