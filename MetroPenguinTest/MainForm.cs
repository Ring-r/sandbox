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
    private readonly Pen penSource = new Pen(Color.Silver) { DashStyle = DashStyle.Dash };
    private readonly Pen penTarget = new Pen(Color.Blue) { DashStyle = DashStyle.Dash };
    private readonly Pen penPath = Pens.Silver;
    private readonly Brush brushText = new SolidBrush(Color.FromArgb(150, 0, 0, 0));

		private PointF source = new Point();
		private PointF target = new Point();

		private bool isPause = false;
		private readonly List<PointF> path = new List<PointF>();
		private int currentIndex = 0;

		private void Start()
		{
			this.penguin.Init(this.ClientSize);
			this.penguins.ForEach(penguin => penguin.Init(this.ClientSize));
			this.penguins.ForEach(penguin => penguin.InitRandomDirection());

      this.target = new PointF(this.penguin.x, this.penguin.y);
			this.StartLevel();
		}

		private void StartLevel()
		{
			this.source = this.target;
			this.target = new PointF(Options.RandomFloat(this.penguin.r, this.ClientSize.Width - this.penguin.r), Options.RandomFloat(this.penguin.r, this.ClientSize.Height - this.penguin.r));

			this.path.Clear();
      this.path.Add(this.source);
			this.currentIndex = 0;

      this.penguin.IsCollisionCounting = false;
		}

    private void CheckEnd()
    {
      var x = this.penguin.x - this.target.X;
      var y = this.penguin.y - this.target.Y;
      var r = this.penguin.r;
      if (x * x + y * y < r * r)
      {
        this.StartLevel();
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

        e.Graphics.DrawEllipse(this.penSource, this.source.X - 2 * this.penguin.r, this.source.Y - 2 * this.penguin.r, 4 * this.penguin.r, 4 * this.penguin.r);
        e.Graphics.DrawEllipse(this.penTarget, this.target.X - 2 * this.penguin.r, this.target.Y - 2 * this.penguin.r, 4 * this.penguin.r, 4 * this.penguin.r);
			}

      if (this.path.Count > 1)
      {
        e.Graphics.DrawLines(this.penPath, this.path.ToArray());
      }

			e.Graphics.DrawString(string.Format("Collision count: {0}", this.penguin.CollisionCount), this.Font, this.brushText, 0, 30);
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			float timeEllapsed = 0.001f * this.stopwatch.ElapsedMilliseconds;
			this.stopwatch.Restart();

			if (!this.isPause)
			{
        this.penguin.Update(timeEllapsed, this.path, ref this.currentIndex);
				this.penguins.ForEach(penguin => penguin.Update(timeEllapsed));

				this.penguin.IsCollided = false;
        this.penguins.ForEach(penguin => penguin.IsCollided = false);

				this.penguins.ForEach(penguin => penguin.CheckCollision(this.penguin));
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
			this.path.Add(this.source);
			this.path.Add(this.target);

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
      this.penguin.IsCollisionCounting = true;
			this.isPause = false;
		}

		#endregion EditState.
	}
}
