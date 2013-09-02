using System;
using System.Drawing;
using System.Windows.Forms;

namespace MoveInCells
{
    public partial class MainForm : Form
    {
        private readonly Entities entities = new Entities();

        public MainForm()
        {
            InitializeComponent();
            this.timer.Interval = 1;
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

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TranslateTransform(0, this.ClientSize.Height);
            e.Graphics.ScaleTransform(1, -1);

            this.entities.Draw(e.Graphics);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.entities.InitClientSize(this.ClientSize.Width, this.ClientSize.Height);
            this.entities.Create();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.entities.Update();
            this.Text = this.entities.Max.ToString();
            this.Invalidate();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            Entity entity = this.entities.Entity;
            float a = (float)Math.Atan2(entity.VY, entity.VX);
            float s = (float)Math.Sqrt(entity.VX * entity.VX + entity.VY * entity.VY);

            const float ak = 0.1f;
            const float sk = 1;
            const float smax = 7;
            if (e.KeyData == Keys.Up) s += sk;
            if (e.KeyData == Keys.Left) a += ak;
            if (e.KeyData == Keys.Right) a -= ak;
            if (e.KeyData == Keys.Down) s -= sk;

            s = Math.Min(s, smax);
            s = Math.Max(s, -smax);

            entity.VX = s * (float)Math.Cos(a);
            entity.VY = s * (float)Math.Sin(a);

            this.entities.UpdateEntity();
        }
    }
}
