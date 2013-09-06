using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MoveOnSphere
{
    public partial class MainForm : Form
    {
		private const int entitiesCount = 30;
        private readonly Entity[] entities = new Entity[entitiesCount];

		private const int moveProcent = 10;
		private const float minAngle = 0.001f;
		private const float maxAngle = 0.005f;

		private const int minS = 5;
		private const int maxS = 30;
		private const int minT = 50;
		private const int maxT = 255;

        public MainForm ()
		{
			InitializeComponent ();

			for (int i = 0; i < entitiesCount; ++i) {
				this.entities[i] = new Entity();
			}

            this.timer.Interval = 15;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
			foreach(Entity entity in this.entities)
			{
				entity.a = 0;
				entity.x = Helper.RandomFloat(-1, 1);
        	    entity.y = Helper.RandomSign() * Helper.RandomFloat(0, (float)Math.Sqrt(1 - entity.x * entity.x));
            	entity.z = Helper.RandomSign() * Helper.RandomFloat(0, (float)Math.Sqrt(1 - entity.x * entity.x - entity.y * entity.y));

				entity.a_ = 0;
				entity.x_ = entity.y;
				entity.y_ = -entity.x;
				entity.z_ = 0;
				float d = (float)Math.Sqrt(entity.x_ * entity.x_ + entity.y_ * entity.y_);
				entity.x_ /= d;
				entity.y_ /= d;
			}
        }

		private void MainForm_Resize(object sender, System.EventArgs e)
        {
            World.R = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / 2;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void MainForm_Paint (object sender, PaintEventArgs e)
		{
			e.Graphics.Clear (Color.White);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			int bx = this.ClientSize.Width >> 1;
			int by = this.ClientSize.Height >> 1;
			e.Graphics.TranslateTransform (bx, by);

			e.Graphics.DrawEllipse (Pens.Silver, -World.R, -World.R, 2 * World.R, 2 * World.R);
			foreach (Entity entity in this.entities) {
				if(-bx <= entity.x && entity.x <= bx && -by <= entity.y && entity.y <= by)
				{
					float s = (maxS - minS) * (entity.z + 1) / 2 + minS;
					Brush brush = new SolidBrush (Color.FromArgb ((int)((maxT - minT) * (entity.z + 1) / 2) + minT, Color.Black));
					e.Graphics.FillEllipse (brush, (float)(entity.x * World.R) - s / 2, (float)(entity.y * World.R) - s / 2, s, s);
				}
			}
        }

        private void timer_Tick (object sender, EventArgs e)
		{
			if (Helper.Random.Next (100) < moveProcent) {
				int i = Helper.Random.Next(entitiesCount);
				if (Helper.Random.Next (100) < 50) {
					this.entities[i].a = Helper.RandomFloat(minAngle, maxAngle);
				}
				else {
					this.entities[i].a_ = Helper.RandomFloat(minAngle, maxAngle);
				}
			}

			foreach (Entity entity in this.entities) {
				entity.Move ();
			}
			this.Invalidate ();
        }
    }
}
