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
		private readonly Entity entity;

        private int isV = 0;
        private float stepV = 0.05f;
        private int isA = 0;
        private float stepA = 0.05f;
        private readonly HashSet<Keys> keys = new HashSet<Keys>();
        private bool isNeedToUpdate = false;

        public MainForm()
        {
            InitializeComponent();

            this.world.Create();
			this.entity = this.world.GetMainEntity();

            this.timer.Interval = 15;
        }

        private void MainForm_Resize(object sender, System.EventArgs e)
        {
            World.R = Math.Max(this.ClientSize.Width, this.ClientSize.Height) / 2;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }

            this.keys.Add(e.KeyData);
            this.isNeedToUpdate = true;
		}
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            this.keys.Remove(e.KeyData);
            this.isNeedToUpdate = true;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.TranslateTransform(this.ClientSize.Width >> 1, this.ClientSize.Height >> 1);

            this.world.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
        }

        private void KeysEvent()
        {
            this.isA = 0;
            if (keys.Contains(Keys.Left))
                this.isA -= 1;
            if (keys.Contains(Keys.Right))
                this.isA += 1;
            this.isV = 0;
            if (keys.Contains(Keys.Up))
                this.isV += 1;
            if (keys.Contains(Keys.Down))
                this.isV -= 1;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.isNeedToUpdate)
            {
                this.KeysEvent();
                this.entity.moveAngle = isV * stepV;
                this.entity.rotateAngle = isA * stepA;
            }

			this.world.Update();
            this.Invalidate();
        }
    }
}
