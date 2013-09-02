using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MoveInCells
{
    public partial class MainForm : Form
    {
        private readonly Entities entities = new Entities();
		private readonly Entity entity;

        private int isV = 0;
        private float stepV = 20.0f;
        private int isA = 0;
        private float stepA = 0.1f;
		private readonly HashSet<Keys> keys = new HashSet<Keys>();
		private bool isNeedToUpdate = false;

        public MainForm()
        {
            InitializeComponent();
            this.timer.Interval = 1;

            this.entities.Create();
			this.entity = this.entities.GetMainEntity();
        }

        private void MainForm_KeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) {
				Close ();
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
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.entities.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
        }

		private void KeysEvent ()
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
        private void timer_Tick (object sender, EventArgs e)
		{
			if (this.isNeedToUpdate) {
				this.KeysEvent ();
				float v = isV * stepV;
				float a = entity.A + isA * stepA;
				if( this.entity.V != v || this.entity.A != a)
				{
					this.entity.SetV (v, a);
					this.entities.UpdateMainEntity ();
				}
			}

            this.entities.Update();

            this.Text = this.entities.MaxScore.ToString();
            this.Invalidate();
        }
    }
}
