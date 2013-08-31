using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MoveInCells
{
    public partial class MainForm : Form
    {
        private readonly Entities entities = new Entities();

        private int isV = 0;
        private int isA = 0;
        private readonly float stepV = 7.0f;
        private readonly float stepA = 0.1f;
		private readonly Dictionary<Keys, bool> keys = new Dictionary<Keys, bool>();
		private bool isNeedToUpdate = false;

        public MainForm()
        {
            InitializeComponent();
            this.timer.Interval = 1;

			this.keys.Add(Keys.Up, false);
			this.keys.Add(Keys.Left, false);
			this.keys.Add(Keys.Right, false);
			this.keys.Add(Keys.Down, false);
        }

		private void KeysEvent ()
		{
			this.isA = 0;
			if (keys[Keys.Left])
				this.isA -= 1;
			if (keys[Keys.Right])
				this.isA += 1;
			this.isV = 0;
			if (keys[Keys.Up])
				this.isV += 1;
			if (keys[Keys.Down])
				this.isV -= 1;
		}
        private void MainForm_KeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) {
				Close ();
			}

			if (this.keys.ContainsKey (e.KeyData)) {
				this.keys[e.KeyData] = true;
				this.isNeedToUpdate = true;
			}
		}
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
			if (this.keys.ContainsKey (e.KeyData)) {
				this.keys[e.KeyData] = false;
				this.isNeedToUpdate = true;
			}
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //e.Graphics.TranslateTransform(0, this.ClientSize.Height);
            //e.Graphics.ScaleTransform(1, -1);

            this.entities.Draw(e.Graphics);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //this.entities.InitClientSize(this.ClientSize.Width, this.ClientSize.Height);
			float size = 1<<10;
			this.entities.InitClientSize(size, size);
            this.entities.Create();
        }

        private void timer_Tick (object sender, EventArgs e)
		{
			if (this.isNeedToUpdate) {
				this.KeysEvent ();
				Entity entity = this.entities.GetMainEntity ();
				float v = isV * stepV;
				float a = entity.A + isA * stepA;
				if( entity.V != v || entity.A != a)
				{
					entity.SetV (v, a);
					this.entities.UpdateMainEntity ();
				}
			}

            this.entities.Update();

            this.Text = this.entities.Max.ToString();
            this.Invalidate();
        }
    }
}
