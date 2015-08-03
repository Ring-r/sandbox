using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Orbits {
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

		public const int blockCount = 10;

		public const float ballRadius = 0.01f;
		public const float blockRadius = 0.02f;

		public const float ballSpeed = 0.1f;

		public static readonly Color backgroundColor = Color.White;

		public static readonly Brush ballBrush = Brushes.Black;
		public static readonly Brush blockBrush = Brushes.Red;

		public static readonly Pen ballPen = Pens.Black;
		public static readonly Pen blockPen = Pens.Black;
		public static readonly Pen orbitPen = Pens.Silver;

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
			this.KeyUp += this.MainForm_KeyUp;
			this.MouseClick += this.MainForm_MouseClick;
			this.timer.Tick += this.Timer_Tick;
			this.ResumeLayout(false);

			this.Text = "Orbits Game Prototype";

			this.timer.Interval = 1000 / Options.needFPS;
			this.timer.Start();

			this.StartLevel();
		}

		private readonly Entity ball = new Entity() {
			brush = Options.ballBrush,
			pen = Options.ballPen,
			radius = Options.ballRadius,
			speed = Options.ballSpeed,
		};
		private readonly List<Entity> blockList = new List<Entity>();

		private bool isEnd = false;
		private bool isShowInfo = true;

		private void StartLevel() {
			this.blockList.Clear();
			for (int i = 0; i < Options.blockCount; ++i) {
				this.blockList.Add(new Entity() {
					brush = Options.blockBrush,
					pen = Options.blockPen,
					radius = Options.blockRadius,
					px = (float)Program.rand.NextDouble(),
					py = (float)Program.rand.NextDouble(),
				});
			}

			this.ball.px = (float)Program.rand.NextDouble();
			this.ball.py = (float)Program.rand.NextDouble();
			this.ball.SetCenter(this.blockList[Program.rand.Next(this.blockList.Count)]);

			this.isEnd = false;
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(Options.backgroundColor);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			float scale = Math.Min(this.ClientSize.Width, this.ClientSize.Height);
			
			e.Graphics.TranslateTransform((this.ClientSize.Width - scale) / 2, (this.ClientSize.Height - scale) / 2);

			foreach (var block in this.blockList) {
				block.Draw(e.Graphics, scale);
			}

			this.ball.Draw(e.Graphics, scale, true);

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

			this.ball.Update(this.timer.Interval * Options.toSeconds);
			foreach (var block in this.blockList) {
				if (this.ball.GetDistance(block) < this.ball.radius + block.radius) {
					this.isEnd = true;
				}
			}
				
			this.Invalidate();
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
		}

		private void MainForm_MouseClick(object sender, MouseEventArgs e) {
			float scale = Math.Min(this.ClientSize.Width, this.ClientSize.Height);
			float ex = (e.X - (this.ClientSize.Width - scale) / 2) / scale;
			float ey = (e.Y - (this.ClientSize.Height - scale) / 2) / scale;
			foreach (var block in this.blockList) {
				float x = block.px - ex;
				float y = block.py - ey;
				float d = (float)Math.Sqrt(x * x + y * y);
				if (d < block.radius) {
					this.ball.SetCenter(block);
				}
			}
		}
	}

	class Entity {
		public Brush brush = Brushes.Black;
		public Pen pen = Pens.Black;
		public Pen penOrbit = Pens.Silver;

		public float px = 0.0f;
		public float py = 0.0f;
		public float radius = 0.0f;

		public float speed = 0.0f;

		private Entity center = null;
		private float angle = 0.0f;
		private float angleSpeed = 0.0f;
		private float orbit = 0.0f;

		public void SetCenter(Entity center) {
			if (this.center == center) {
				this.speed = -this.speed;
				this.angleSpeed = -this.angleSpeed;
			} else {
				this.center = center;
				float x = this.px - this.center.px;
				float y = this.py - this.center.py;
				this.orbit = (float)Math.Sqrt(x * x + y * y);
				this.angle = (float)Math.Atan2(y, x);
				this.angleSpeed = Math.Sign(this.speed) * (float)Math.Asin(Math.Abs(this.speed) / this.orbit);
			}
		}

		public void Update(float timeEllapsed) {
			this.angle += this.angleSpeed * timeEllapsed;
			if (this.angle < 0) {
				this.angle += 2 * (float)Math.PI;
			}
			if (this.angle > 2 * Math.PI) {
				this.angle -= 2 * (float)Math.PI;
			}
			
			this.px = this.center.px + this.orbit * (float)Math.Cos(this.angle);
			if (this.px < 0.0f) {
				this.speed = Math.Abs(this.speed);
				this.angleSpeed = Math.Abs(this.angleSpeed);
			}
			if (this.px > 1.0f) {
				this.speed = -Math.Abs(this.speed);
				this.angleSpeed = -Math.Abs(this.angleSpeed);
			}
			this.py = this.center.py + this.orbit * (float)Math.Sin(this.angle);
			if (this.py < 0.0f) {
				this.speed = Math.Abs(this.speed);
				this.angleSpeed = Math.Abs(this.angleSpeed);
			}
			if (this.py > 1.0f) {
				this.speed = -Math.Abs(this.speed);
				this.angleSpeed = -Math.Abs(this.angleSpeed);
			}
		}

		public void Draw(Graphics g, float scale, bool withOrbit = false) {
			if (withOrbit && this.center != null) {
				float orbitX = scale * this.center.px;
				float orbitY = scale * this.center.py;
				float orbitR = scale * this.orbit;
				g.DrawEllipse(this.penOrbit, orbitX - orbitR, orbitY - orbitR, 2 * orbitR, 2 * orbitR);
			}

			float x = scale * this.px;
			float y = scale * this.py;
			float r = scale * this.radius;
			g.FillEllipse(this.brush, x - r, y - r, 2 * r, 2 * r);
			g.DrawEllipse(this.pen, x - r, y - r, 2 * r, 2 * r);
		}

		public float GetDistance(Entity entity) {
			float x = this.px - entity.px;
			float y = this.py - entity.py;
			return (float)Math.Sqrt(x * x + y * y);
		}
	}
}
