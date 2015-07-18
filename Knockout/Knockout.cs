using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Knockout {
	static class Program {
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		public static readonly Random rand = new Random();
	}

	static class Options {
		public const int needFPS = 60;

		public const int blockCount = 100;

		public const int ballRadius = 10;
		public const int blockRadius = 10;
		public const int centerRadius = 10;

		public const float ballOrbitSpeed = -0.1f;
		public const float ballOrbitSpeedStep = -0.05f;
		public const float blockAngleSpeed = 0.5f;

		public static readonly Color backgroundColor = Color.White;

		public static readonly Brush ballBrush = Brushes.Black;
		public static readonly Brush blockBrush = Brushes.Red;
		public static readonly Brush centerBrush = Brushes.Silver;

		public static readonly Pen blockPen = Pens.Black;
		public static readonly Pen orbitPen = Pens.Silver;

		public const float angleStepKey = 0.05f;
	}

	public partial class MainForm : Form {
		private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
		private readonly Timer timer = new Timer();

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		public MainForm() {
			this.SuspendLayout();
			this.DoubleBuffered = true;
			this.Name = "MainForm";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.WindowState = FormWindowState.Maximized;
			this.Paint += this.MainForm_Paint;
			this.Resize += this.MainForm_Resize;
			this.KeyDown += this.MainForm_KeyDown;
			this.KeyUp += this.MainForm_KeyUp;
			this.MouseDown += this.MainForm_MouseDown;
			this.MouseMove += this.MainForm_MouseMove;
			this.MouseUp += this.MainForm_MouseUp;
			this.timer.Tick += this.Timer_Tick;
			this.ResumeLayout(false);

			this.Text = "Knockout Game Prototype";

			this.timer.Interval = 1000 / Options.needFPS;
			this.timer.Start();
		}

		private readonly Stopwatch stopwatch = new Stopwatch();

		private readonly Entity ball = new Entity() {
			brush = Options.ballBrush,
			radius = Options.ballRadius,
			orbitSpeed = Options.ballOrbitSpeed,
		};
		private readonly List<Entity> blockList = new List<Entity>();
		private readonly Entity center = new Entity() {
			brush = Options.centerBrush,
			radius = Options.centerRadius,
		};

		private float angle = 0.0f;
		private float scale = 0.0f;

		private bool isLeftKey = false;
		private bool isRightKey = false;
		private bool isMouse = false;
		private float angleMouse = 0.0f;

		private bool isEnd = false;
		private bool isPause = true;
		private bool isShowInfo = true;

		private void StartLevel() {
			this.ball.Fill(0.75f, 0.75f);
			this.ball.orbitSpeed = Options.ballOrbitSpeed;
			this.blockList.Clear();

			this.isEnd = false;
			this.isPause = false;
		}

		private void MainForm_Resize(object sender, EventArgs e) {
			this.scale = Math.Min(this.ClientSize.Width, this.ClientSize.Height);
			this.Invalidate();
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(Options.backgroundColor);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			e.Graphics.TranslateTransform(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

			this.center.Draw(e.Graphics, this.angle, this.scale);

			foreach (var circle in this.blockList) {
				circle.DrawOrbit(e.Graphics, this.angle, this.scale);
			}

			this.ball.Draw(e.Graphics, 0.0f, scale);

			foreach (var circle in this.blockList) {
				circle.Draw(e.Graphics, this.angle, this.scale);
			}

			e.Graphics.ResetTransform();

			if (this.isEnd) {
				string str = "BOOM";
				SizeF strSize = e.Graphics.MeasureString(str, this.Font);
				e.Graphics.DrawString(str, this.Font, Brushes.Red, this.ClientSize.Width / 2 - strSize.Width / 2, this.ClientSize.Height / 2 - strSize.Height / 2);
			}
		}

		private void Timer_Tick(object sender, EventArgs e) {
			if (this.isEnd) {
				return;
			}

			float timeEllapsed = 0.001f * this.stopwatch.ElapsedMilliseconds;
			this.stopwatch.Restart();

			if (this.isLeftKey) {
				this.angle += Options.angleStepKey;
			}
			if (this.isRightKey) {
				this.angle -= Options.angleStepKey;
			}

			if (!this.isPause) {
				this.ball.Update(timeEllapsed);
				if (this.ball.orbit == 0.0f) {
					this.ball.Fill(0.75f, 0.75f);
					var block = new Entity() {
						brush = Options.blockBrush,
						pen = Options.blockPen,
						radius = Options.blockRadius,
						angleSpeed = Options.blockAngleSpeed * (float)(Program.rand.NextDouble() - 0.5f),
					};
					block.Fill(0.5f);
					float x = this.center.GetX(0.0f, this.scale) - block.GetX(this.angle, this.scale);
					float y = this.center.GetY(0.0f, this.scale) - block.GetY(this.angle, this.scale);
					double dd = Math.Sqrt(x * x + y * y);
					if (dd > this.center.radius + block.radius) {
						this.blockList.Add(block);
					} else {
						this.ball.orbitSpeed += Options.ballOrbitSpeedStep;
					} 
				}
				foreach (var block in this.blockList) {
					block.Update(timeEllapsed);
				}

				foreach (var block in this.blockList) {
					float x = this.ball.GetX(0.0f, this.scale) - block.GetX(this.angle, this.scale);
					float y = this.ball.GetY(0.0f, this.scale) - block.GetY(this.angle, this.scale);
					double dd = Math.Sqrt(x * x + y * y);
					if (dd < this.ball.radius + block.radius) {
						this.isEnd = true;
						this.isPause = true;
					}
				}
			}

			this.Invalidate();
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.Left:
					this.isLeftKey = true;
					break;
				case Keys.Right:
					this.isRightKey = true;
					break;
			}
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.Escape:
					this.Close();
					break;
				case Keys.Space:
					this.StartLevel();
					break;
				case Keys.F1:
					this.isShowInfo = !this.isShowInfo;
					break;
			}
			switch (e.KeyCode) {
				case Keys.Left:
					this.isLeftKey = false;
					break;
				case Keys.Right:
					this.isRightKey = false;
					break;
			}
		}

		private void MainForm_MouseDown(object sender, MouseEventArgs e) {
			float x = e.X - this.ClientSize.Width / 2;
			float y = e.Y - this.ClientSize.Height / 2;
			this.angleMouse = (float)Math.Atan2(y, x) - this.angle;
			this.isMouse = true;
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e) {
			if (!this.isMouse) {
				return;
			}

			float x = e.X - this.ClientSize.Width / 2;
			float y = e.Y - this.ClientSize.Height / 2;
			this.angle = (float)Math.Atan2(y, x) - this.angleMouse;
		}

		private void MainForm_MouseUp(object sender, MouseEventArgs e) {
			this.isMouse = false;
		}

	}

	class Entity {
		public Brush brush = Brushes.Black;
		public Pen pen = Pens.Black;
		public Pen penOrbit = Pens.Silver;

		public float angle = 0.0f;
		public float angleSpeed = 0.0f;
		public float orbit = 0.0f;
		public float orbitSpeed = 0.0f;
		public float radius = 0.0f;
		public float radiusSpeed = 0.0f;

		public void Fill(float orbitFrom, float orbitTo = 0.0f) {
			this.angle = 2 * (float)Math.PI * (float)Program.rand.NextDouble();
			this.orbit = (float)Program.rand.NextDouble() * (orbitFrom - orbitTo) + orbitTo;
		}

		public void Update(float timeEllapsed) {
			this.angle += this.angleSpeed * timeEllapsed;
			this.orbit += this.orbitSpeed * timeEllapsed;
			this.orbit = Math.Max(this.orbit, 0.0f);
			this.radius += this.radiusSpeed * timeEllapsed;
			this.radius = Math.Max(this.radius, 0.0f);
		}

		public void Draw(Graphics g, float angle, float scale) {
			float x = this.GetX(angle, scale);
			float y = this.GetY(angle, scale);
			g.FillEllipse(this.brush, x - this.radius, y - this.radius, 2 * this.radius, 2 * this.radius);
			g.DrawEllipse(this.pen, x - this.radius, y - this.radius, 2 * this.radius, 2 * this.radius);
		}

		public void DrawOrbit(Graphics g, float angle, float scale) {
			angle += this.angle;
			scale *= this.orbit;
			g.DrawEllipse(this.penOrbit, -scale, -scale, 2 * scale, 2 * scale);
		}

		public float GetX(float angle, float scale) {
			return scale * this.orbit * (float)Math.Cos(angle + this.angle);
		}

		public float GetY(float angle, float scale) {
			return scale * this.orbit * (float)Math.Sin(angle + this.angle);
		}
	}
}
