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
		public const float toSeconds = 0.001f;

		public const int blockCount = 12;

		public const float ballRadius = 0.01f;
		public const float blockRadius = 0.01f;
		public const float centerRadius = 0.01f;

		public const float ballOrbitSpeed = -0.1f;
		public const float ballOrbitSpeedStep = -0.05f;
		public const float blockAngleSpeed = 0.5f;
		public const float blockAngleSpeedStep = 0.1f;

		public static readonly Color backgroundColor = Color.White;

		public static readonly Brush ballBrush = Brushes.Black;
		public static readonly Brush blockBrush = Brushes.Red;
		public static readonly Brush centerBrush = Brushes.Silver;

		public static readonly Pen blockPen = Pens.Black;
		public static readonly Pen orbitPen = Pens.Silver;

		public const float angleStepKey = 0.05f;

		public static readonly Font font = new Font("Comic", 30);
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
			this.Resize += (sender, e) => this.Invalidate();
			this.KeyDown += this.MainForm_KeyDown;
			this.KeyUp += this.MainForm_KeyUp;
			this.timer.Tick += this.Timer_Tick;
			this.ResumeLayout(false);

			this.Text = "Knockout Game Prototype";

			this.timer.Interval = 1000 / Options.needFPS;
			this.timer.Start();

			this.StartLevel();
		}

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

		private bool isLeftKey = false;
		private bool isRightKey = false;

		private bool isEnd = false;
		private bool isShowInfo = true;

		private void StartLevel() {
			this.ball.angle = 2 * (float)Math.PI * (float)Program.rand.NextDouble();
			this.ball.orbit = 0.5f;
			this.ball.orbitSpeed = Options.ballOrbitSpeed;

			this.blockList.Clear();
			for (int i = 0; i < Options.blockCount / 4; ++i) {
				for (int orbitLevel = 0; orbitLevel < 4; ++orbitLevel) {
					var block = new Entity() {
						brush = Options.blockBrush,
						pen = Options.blockPen,
						radius = Options.blockRadius,
						angle = 2 * (float)Math.PI * (float)Program.rand.NextDouble(),
						angleSpeed = Options.blockAngleSpeed * (float)(Program.rand.NextDouble() - 0.5f),
						orbit = 0.1f + 0.1f * orbitLevel,
					};
					this.blockList.Add(block);
				}
			}

			this.isEnd = false;
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(Options.backgroundColor);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			e.Graphics.TranslateTransform(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

			float scale = Math.Min(this.ClientSize.Width, this.ClientSize.Height);

			foreach (var block in this.blockList) {
				block.DrawOrbit(e.Graphics, scale);
			}

			this.ball.Draw(e.Graphics, scale);

			foreach (var block in this.blockList) {
				block.Draw(e.Graphics, scale);
			}

			this.center.Draw(e.Graphics, scale);

			e.Graphics.ResetTransform();

			if (this.isEnd) {
				string str = "BOOM";
				SizeF strSize = e.Graphics.MeasureString(str, Options.font);
				e.Graphics.DrawString(str, Options.font, Brushes.Red, this.ClientSize.Width / 2 - strSize.Width / 2, this.ClientSize.Height / 2 - strSize.Height / 2);
			}
		}

		private void Timer_Tick(object sender, EventArgs e) {
			if (this.isEnd) {
				return;
			}

			float angle = 0.0f;
			if (this.isLeftKey) {
				angle += Options.angleStepKey;
			}
			if (this.isRightKey) {
				angle -= Options.angleStepKey;
			}
			foreach (var block in this.blockList) {
				block.angle += angle;
			}

			this.ball.Update(this.timer.Interval * Options.toSeconds);
			if (this.ball.orbit == 0.0f) {
				this.ball.angle = 2 * (float)Math.PI * (float)Program.rand.NextDouble();
				this.ball.orbit = 0.5f;

				if (Program.rand.NextDouble() < 0.5) {
					this.ball.orbitSpeed += Options.ballOrbitSpeedStep;
				} else {
					foreach (var block in this.blockList) {
						block.angleSpeed += Math.Sign(block.angleSpeed) * Options.blockAngleSpeedStep;
					}
				}
			}
			foreach (var block in this.blockList) {
				block.Update(this.timer.Interval * Options.toSeconds);
			}

			foreach (var block in this.blockList) {
				if (block.GetDistance(this.ball) < this.ball.radius + block.radius) {
					this.isEnd = true;
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

		public void Update(float timeEllapsed) {
			this.angle += this.angleSpeed * timeEllapsed;
			this.orbit += this.orbitSpeed * timeEllapsed;
			this.orbit = Math.Max(this.orbit, 0.0f);
			this.radius += this.radiusSpeed * timeEllapsed;
			this.radius = Math.Max(this.radius, 0.0f);
		}

		public void Draw(Graphics g, float scale) {
			float x = scale * this.GetX();
			float y = scale * this.GetY();
			float r = scale * this.radius;
			g.FillEllipse(this.brush, x - r, y - r, 2 * r, 2 * r);
			g.DrawEllipse(this.pen, x - r, y - r, 2 * r, 2 * r);
		}

		public void DrawOrbit(Graphics g, float scale) {
			float orbit = scale * this.orbit;
			g.DrawEllipse(this.penOrbit, -orbit, -orbit, 2 * orbit, 2 * orbit);
		}

		public float GetX() {
			return this.orbit * (float)Math.Cos(this.angle);
		}

		public float GetY() {
			return this.orbit * (float)Math.Sin(this.angle);
		}

		public float GetDistance(Entity entity) {
			float x = this.GetX() - entity.GetX();
			float y = this.GetY() - entity.GetY();
			return (float)Math.Sqrt(x * x + y * y);
		}
	}
}
