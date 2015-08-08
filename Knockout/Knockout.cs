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

		public const int blockCount = 8;

		public const float ballRadius = 0.01f;
		public const float blockRadius = 0.01f;
		public const float centerRadius = 0.01f;

		public const float ballOrbitSpeed = -0.5f;
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

		public const float ballOrbit = 0.7f;
		public const float ballOrbitMax = 0.5f;
		public const float blockOrbitMin = 0.1f;
		public const float blockOrbitMax = 0.4f;
		public const int blockOrbitCount = 4;
		public const float blockOrbitStep = (blockOrbitMax - blockOrbitMin) / blockOrbitCount;
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

			this.CreateGame();
		}

		private readonly Entity ball = new Entity() {
			brush = Options.ballBrush,
			radius = Options.ballRadius,
			orbitSpeed = Options.ballOrbitSpeed,
			orbitMax = Options.ballOrbitMax,
		};
		private readonly List<Entity> blockList = new List<Entity>();
		private readonly Entity center = new Entity() {
			brush = Options.centerBrush,
			radius = Options.centerRadius,
		};

		private int waitedOrbitIndex = 0;

		private bool isLeftKey = false;
		private bool isRightKey = false;

		private float score = 0;
		private int state = 0;
		private bool isShowInfo = true;

		private void CreateGame() {
			this.blockList.Clear();
			for (int i = 0; i < Options.blockCount; ++i) {
				this.blockList.Add(new Entity() {
					brush = Options.blockBrush,
					pen = Options.blockPen,
					radius = Options.blockRadius,
					orbitMax = Options.blockOrbitMax,
				});
			}

			this.RelocateBall();
			this.ball.orbitSpeed = Options.ballOrbitSpeed;

			this.RelocateBlocks();

			this.score = 0.0f;
			this.StartLevel();
		}

		private void RelocateBall() {
			this.ball.angle = 2 * (float)Math.PI * (float)Program.rand.NextDouble();
			this.ball.orbit = Options.ballOrbit;

			this.waitedOrbitIndex = 0;
		}

		private void RelocateBlocks() {
			int blockIndex = 0;
			int blockCount = Options.blockCount / Options.blockOrbitCount;
			int orbitIndex = Options.blockOrbitCount - 1;
			for (int i = 0; i < this.blockList.Count; ++i) {
				blockIndex += 1;
				if (blockIndex == blockCount) {
					blockIndex = 0;
					orbitIndex -= 1;
				}

				var block = this.blockList[i];
				block.angle = 2 * (float)Math.PI * (float)Program.rand.NextDouble();
				block.angleSpeed = Options.blockAngleSpeed * (float)(Program.rand.NextDouble() - 0.5f);
				block.orbit = Options.blockOrbitMin + Options.blockOrbitStep * orbitIndex;

				block.next = blockIndex != 0 ? this.blockList[i + 1] : block.next = this.blockList[i + 1 - blockCount];

			}
		}

		private void StartLevel() {
			this.state = 0;
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

			string str;
			SizeF strSize;
			str = this.score.ToString();
			strSize = e.Graphics.MeasureString(str, Options.font);
			e.Graphics.DrawString(str, Options.font, Brushes.Black, this.ClientSize.Width - strSize.Width, 0.0f);

			switch (this.state) {
				case 0:
					break;
				case 1:
					str = "BOOM";
					strSize = e.Graphics.MeasureString(str, Options.font);
					e.Graphics.DrawString(str, Options.font, Brushes.Red, this.ClientSize.Width / 2 - strSize.Width / 2, this.ClientSize.Height / 2 - strSize.Height / 2);
					break;
				default:
					str = "Something wrong!";
					strSize = e.Graphics.MeasureString(str, Options.font);
					e.Graphics.DrawString(str, Options.font, Brushes.Red, this.ClientSize.Width / 2 - strSize.Width / 2, this.ClientSize.Height / 2 - strSize.Height / 2);
					break;
			}
		}

		private void Timer_Tick(object sender, EventArgs e) {
			if (this.state != 0) {
				this.Invalidate();
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

			float prevBallOrbit = this.ball.orbit;
			this.ball.Update(this.timer.Interval * Options.toSeconds);

			bool isRun = true;
			while (this.waitedOrbitIndex < this.blockList.Count && isRun) {
				var waiteredBlock = this.blockList[this.waitedOrbitIndex];
				isRun = this.ball.orbit < waiteredBlock.orbit && waiteredBlock.orbit <= prevBallOrbit;
				if (isRun) {
					var minAngle = Math.Min(this.ball.angle, waiteredBlock.angle);
					var maxAngle = Math.Max(this.ball.angle, waiteredBlock.angle);
					var angleBetween = maxAngle - minAngle;
					while (angleBetween > 2 * (float)Math.PI) {
						angleBetween -= 2 * (float)Math.PI;
					}
					angleBetween = Math.Min(angleBetween, 2 * (float)Math.PI - angleBetween);
					var distance = angleBetween * waiteredBlock.orbit;
					var length = this.ball.radius + waiteredBlock.radius;
					if (distance < length) {
						this.state = 1;
						return;
					} else if (distance < 2 * length) {
							this.score += length / (distance - length);
						}
					this.waitedOrbitIndex += 1;
				}
			}

			foreach (var block in this.blockList) {
				block.Update(this.timer.Interval * Options.toSeconds);
			}
			foreach (var block in this.blockList) {
				if (block.GetDistance(block.next) < block.radius + block.next.radius) {
					var temp = block.angleSpeed;
					block.angleSpeed = block.next.angleSpeed;
					block.next.angleSpeed = temp;
				}
			}

			if (this.ball.orbit == 0.0f) {
				int blockIndex = Program.rand.Next(this.blockList.Count + 1);
				if (blockIndex == this.blockList.Count) {
					this.ball.orbitSpeed += Options.ballOrbitSpeedStep;
				} else {
					var block = this.blockList[blockIndex];
					block.angleSpeed += Math.Sign(block.angleSpeed) * Options.blockAngleSpeedStep;
				}

				this.RelocateBall();
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
					this.CreateGame();
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

		public float orbitMax = 0.0f;

		public Entity next = null;

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
			if (this.orbit > this.orbitMax) {
				var text = (this.orbit - this.orbitMax).ToString("F2");
				var font = new Font("Comic", 12); // TODO: Remove magic.
				var size = g.MeasureString(text, font);
				if (x > 0) {
					x -= r + size.Width;
				} else {
					x += r;
				}
				if (y > 0) {
					y -= r + size.Height;
				} else {
					y += r;
				}
				g.DrawString(text, font, this.brush, x, y);
			}
		}

		public void DrawOrbit(Graphics g, float scale) {
			float orbit = scale * this.orbit;
			g.DrawEllipse(this.penOrbit, -orbit, -orbit, 2 * orbit, 2 * orbit);
		}

		public float GetX() {
			float orbit = Math.Min(this.orbit, this.orbitMax);
			return orbit * (float)Math.Cos(this.angle);
		}

		public float GetY() {
			float orbit = Math.Min(this.orbit, this.orbitMax);
			return orbit * (float)Math.Sin(this.angle);
		}

		public float GetDistance(Entity entity) {
			float x = this.GetX() - entity.GetX();
			float y = this.GetY() - entity.GetY();
			return (float)Math.Sqrt(x * x + y * y);
		}
	}
}
