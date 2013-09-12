using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Particles
{
    public partial class MainForm : Form
    {
        private World world = new World();

        public MainForm()
        {
            InitializeComponent();

            this.world.Create();

            this.timer.Interval = 15;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.TranslateTransform(this.ClientSize.Width >> 1, this.ClientSize.Height >> 1);

            this.world.Draw(e.Graphics);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.world.Update(this.timer.Interval);
            this.Invalidate();
        }
    }
}
