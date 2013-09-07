using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MoveInCells
{
    public partial class MainForm : Form
    {
        private readonly World world = new World();
        private readonly Controller controller = new Controller();

        public MainForm()
        {
            InitializeComponent();
            this.timer.Interval = 1;

            this.world.Create();
			this.controller.SetEntity(this.world.GetMainEntity());
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }

            this.controller.AddKey(e.KeyData);
        }
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            this.controller.RemoveKey(e.KeyData);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.world.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.controller.Update())
            {
				this.world.UpdateMainEntity();
            }

            this.world.Update();

            this.Text = this.world.MaxScore.ToString();
            this.Invalidate();
        }
    }
}
