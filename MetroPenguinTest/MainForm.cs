using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MetroPenguinTest
{
	public class MainForm : Form
	{
		private readonly Penguin penguin = new Penguin();
		private readonly List<Penguin> penguins = new List<Penguin>();

		private readonly Pen penPenguin = new Pen(Color.FromArgb(255, 0, 0, 255));
		private readonly Brush brushPenguin = new SolidBrush(Color.FromArgb(150, 0, 255, 0));
		private readonly Pen penPenguins = new Pen(Color.FromArgb(255, 255, 0, 0));
		private readonly Brush brushPenguins = new SolidBrush(Color.FromArgb(150, 255, 0, 0));
		private readonly Brush brushText = new SolidBrush(Color.FromArgb(150, 0, 0, 0));

		private PointF start = new Point();
		private PointF finish = new Point();
		private readonly Pen penFinish = new Pen(Color.Blue) { DashStyle = DashStyle.Dash };

		private bool isPause = false;
		private readonly List<PointF> path = new List<PointF>();
		private int currentIndex = 0;

		private int collisionsCount = 0;

		private void Start()
		{
			this.penguin.Init(this.ClientSize);
			this.penguins.ForEach(penguin => penguin.Init(this.ClientSize));
			this.penguins.ForEach(penguin => penguin.InitRandomDirection());

			this.finish = new PointF(Options.RandomFloat(this.penguin.r, this.ClientSize.Width - this.penguin.r), Options.RandomFloat(this.penguin.r, this.ClientSize.Height - this.penguin.r));
			this.StartLevel();
		}

		private void StartLevel()
		{
			this.start = this.finish;
			this.finish = new PointF(Options.RandomFloat(this.penguin.r, this.ClientSize.Width - this.penguin.r), Options.RandomFloat(this.penguin.r, this.ClientSize.Height - this.penguin.r));

			this.path.Clear();
			this.path.Add(new PointF(this.penguin.x, this.penguin.y));
			this.currentIndex = 0;
		}

		private void CheckEnd()
		{
			if (this.currentIndex == this.path.Count - 1)
			{
				var x = this.penguin.x - this.finish.X;
				var y = this.penguin.y - this.finish.Y;
				if (x * x + y * y < this.penguin.r * this.penguin.r)
				{
					this.penguin.x = this.finish.X;
					this.penguin.y = this.finish.Y;
					this.StartLevel();
				}
			}
		}

		private readonly Stopwatch stopwatch = new Stopwatch();
		private readonly Timer timer = new Timer();

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.timer.Dispose();
			}
			base.Dispose(disposing);
		}

		public MainForm()
		{
			this.SuspendLayout();

			this.DoubleBuffered = true;
			this.Name = this.Text = "MainForm";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.WindowState = FormWindowState.Maximized;

			this.KeyUp += this.MainForm_KeyUp;
			this.Load += this.MainForm_Load;
			this.Paint += this.MainForm_Paint;

			this.timer.Interval = 1000 / Options.needFPS;
			this.timer.Enabled = true;

			this.timer.Tick += this.Timer_Tick;

			this.ResumeLayout(false);
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
			{
				this.Close();
			}
			if (e.KeyData == Keys.Enter)
			{
				this.SwitchEditState();
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			for (int i = 0; i < Options.startCount; ++i)
			{
				this.penguins.Add(new Penguin());
			}

			this.Start();
		}

		private void MainForm_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.White);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			if (this.penguins.Count > 0)
			{
				this.penguins.ForEach(penguin => penguin.Draw(e.Graphics, this.brushPenguins, this.penPenguins));
				this.penguin.Draw(e.Graphics, this.brushPenguin, this.penPenguin);

				e.Graphics.DrawEllipse(this.penFinish, this.finish.X - 2 * this.penguin.r, this.finish.Y - 2 * this.penguin.r, 4 * this.penguin.r, 4 * this.penguin.r);
			}

			for (int i = this.currentIndex; i < this.path.Count; ++i)
			{
				e.Graphics.FillEllipse(this.brushPenguin, this.path[i].X - Options.pointR, this.path[i].Y - Options.pointR, 2 * Options.pointR, 2 * Options.pointR);
			}

			e.Graphics.DrawString(string.Format("Collision count: {0}", this.collisionsCount), this.Font, this.brushText, 0, 30);
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			float timeEllapsed = 0.001f * this.stopwatch.ElapsedMilliseconds;
			this.stopwatch.Restart();

			if (!this.isPause)
			{
				this.penguins.ForEach(penguin => penguin.Move(timeEllapsed));

				#region Moving penguin (check points).
				var step = this.penguin.s * timeEllapsed;
				if (this.currentIndex < this.path.Count)
				{
					float x = this.path[this.currentIndex].X - this.penguin.x;
					float y = this.path[this.currentIndex].Y - this.penguin.y;
					float d = (float)Math.Sqrt(x * x + y * y);
					if (d <= step)
					{
						this.penguin.x = this.path[this.currentIndex].X;
						this.penguin.y = this.path[this.currentIndex].Y;
						if (this.currentIndex < this.path.Count - 1)
						{
							this.currentIndex++;
						}
						else
						{
							this.penguin.vx = 0;
							this.penguin.vy = 0;
						}
					}
					else
					{
						this.penguin.vx = x / d;
						this.penguin.vy = y / d;

						this.penguin.x += this.penguin.vx * step;
						this.penguin.y += this.penguin.vy * step;
					}
				}
				#endregion Moving penguin (check points).

				this.penguin.IsCollided = false;
				this.penguins.ForEach(penguin => penguin.CheckCollision(this.penguin));
				if (this.penguin.IsCollided && this.penguin.s != 0 && !(this.penguin.vx == 0 && this.penguin.vy == 0))
				{
					this.collisionsCount++;
				}

				#region Collision between penguins.
				// TODO: (R) Need to decrease complexity.
				for (var i = 0; i < this.penguins.Count - 1; ++i)
				{
					for (var j = i + 1; j < this.penguins.Count; ++j)
					{
						this.penguins[i].CheckCollision(this.penguins[j]);
					}
				}
				#endregion Collision between penguins.

				this.penguin.ReturnInBorders(this.ClientSize);
				this.penguins.ForEach(penguin => penguin.ReturnInBorders(this.ClientSize));

				this.CheckEnd();
			}
			this.Invalidate();
		}

		#region EditState.

		private bool isEditState = false;
		private void SwitchEditState()
		{
			this.isEditState = !this.isEditState;
			if (this.isEditState)
			{
				RunInEditState();
			}
			else
			{
				RunOutEditState();
			}
		}

		private void RunInEditState()
		{
			this.isPause = true;

			this.path.Clear();
			this.path.Add(this.start);
			this.path.Add(this.finish);

			this.MouseDown += this.MainForm_MouseDown;
			this.MouseMove += this.MainForm_MouseMove;
		}

		private void MainForm_MouseDown(object sender, MouseEventArgs e)
		{
			this.path.Insert(this.path.Count - 1, e.Location);
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e)
		{
			// TODO: Add example for touchscreen.
		}

		private void RunOutEditState()
		{
			this.MouseDown -= this.MainForm_MouseDown;
			this.MouseMove -= this.MainForm_MouseMove;

			// Go to MoveState or go to BeginState.
			this.currentIndex = 0;
			this.collisionsCount = 0;
			this.isPause = false;
		}

		#endregion EditState.
	}
}
