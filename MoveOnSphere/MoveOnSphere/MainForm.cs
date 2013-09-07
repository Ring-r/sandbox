using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MoveOnSphere
{
    public partial class MainForm : Form
    {
        private readonly World world = new World();
		private readonly Controller controller = new Controller();

        public MainForm()
        {
            InitializeComponent();

            this.world.Create();
			this.controller.SetEntity(this.world.GetMainEntity());

            this.timer.Interval = 15;
        }

        private void MainForm_Resize(object sender, System.EventArgs e)
        {
            World.R = Math.Max(this.ClientSize.Width, this.ClientSize.Height) / 2;
			this.controller.RecalculateSteps();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }

			this.controller.AddKey(e.KeyData);
		}
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
			this.controller.Remove(e.KeyData);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.TranslateTransform(this.ClientSize.Width >> 1, this.ClientSize.Height >> 1);

            this.world.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
			this.controller.Update();
			this.world.Update();
            this.Invalidate();
        }
    }
}
