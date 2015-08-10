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
		public const int bonusCount = 10;

		public const float ballRadius = 0.01f;
		public const float blockRadius = 0.02f;
		public const float bonusRadius = 0.02f;

		public const float ballSpeed = 0.1f;

		public const int startTouchCount = 3;
		public const int usedTouchCount = 1;

		public static readonly Color backgroundColor = Color.White;
		public static readonly Pen backgroundPen = Pens.Black;

		public static readonly Brush ballBrush = Brushes.Black;
		public static readonly Brush blockBrush = Brushes.Red;
		public static readonly Brush bonusBrush = Brushes.Yellow;

		public static readonly Pen ballPen = Pens.Black;
		public static readonly Pen blockPen = Pens.Black;
		public static readonly Pen bonusPen = Pens.Black;
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

			this.CreateGame();
		}

		private readonly Entity ball = new Entity() {
			type = -1,
			brush = Options.ballBrush,
			pen = Options.ballPen,
			radius = Options.ballRadius,
			Speed = Options.ballSpeed,
		};
		private readonly List<Entity> blockList = new List<Entity>();
		private int avalableTouchCount = 0;

		private int state = 0;
		private bool isShowInfo = true;

		private void CreateGame() {
			this.blockList.Clear();
			for (int i = 0; i < Options.blockCount; ++i) {
				this.blockList.Add(new Entity() {
					type = 0,
					brush = Options.blockBrush,
					pen = Options.blockPen,
					radius = Options.blockRadius,
				});
			}
			for (int i = 0; i < Options.bonusCount; ++i) {
				this.blockList.Add(new Entity() {
					type = 1,
					brush = Options.bonusBrush,
					pen = Options.bonusPen,
					radius = Options.bonusRadius,
				});
			}

			this.RelocateBall();
			this.RelocateBlocks();
			this.StartLevel();
		}

		private void RelocateBall() {
			this.ball.px = (float)Program.rand.NextDouble();
			this.ball.py = (float)Program.rand.NextDouble();
		}

		private void RelocateBlocks() {
			foreach (var item in this.blockList) {
				item.px = (float)Program.rand.NextDouble();
				item.py = (float)Program.rand.NextDouble();
			}
		}

		private void StartLevel() {
			this.ball.center = null;
			this.avalableTouchCount = Options.startTouchCount;
			this.state = 0;
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
			
			e.Graphics.DrawRectangle(Options.backgroundPen, 0.0f, 0.0f, scale, scale);

			e.Graphics.ResetTransform();

			string str;
			SizeF strSize;
			switch (this.state) {
				case 0:
					str = this.avalableTouchCount.ToString();
					strSize = e.Graphics.MeasureString(str, Options.font);
					e.Graphics.DrawString(str, Options.font, Brushes.Black, this.ClientSize.Width - strSize.Width, 0.0f);
					break;
				case 1:
					str = "BOOM";
					strSize = e.Graphics.MeasureString(str, Options.font);
					e.Graphics.DrawString(str, Options.font, Brushes.Red, this.ClientSize.Width / 2 - strSize.Width / 2, this.ClientSize.Height / 2 - strSize.Height / 2);
					break;
				case 2:
					str = "No power points.";
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
				return;
			}

			this.ball.Update(this.timer.Interval * Options.toSeconds);
			foreach (var block in this.blockList) {
				if (this.ball.IsCollided(block)) {
					if (block.type == 0) {
						this.state = 1;
					}
					if (block.type == 1) {
						this.avalableTouchCount += Options.usedTouchCount;
						block.px = (float)Program.rand.NextDouble();
						block.py = (float)Program.rand.NextDouble();
					}
				}
			}
				
			this.Invalidate();

			if (this.avalableTouchCount < 0) {
				this.state = 2;
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
		}

		private void MainForm_MouseClick(object sender, MouseEventArgs e) {
			if (this.ball.center == null) {
				this.ball.center = new Entity();
			}

			float scale = Math.Min(this.ClientSize.Width, this.ClientSize.Height);
			float ex = (e.X - (this.ClientSize.Width - scale) / 2) / scale;
			float ey = (e.Y - (this.ClientSize.Height - scale) / 2) / scale;
			this.ball.SetCenter(ex, ey);
			this.avalableTouchCount -= Options.usedTouchCount;
		}
	}

	class Entity {
		public int type = 0;

		public Brush brush = Brushes.Black;
		public Pen pen = Pens.Black;
		public Pen penOrbit = Pens.Silver;

		public float px = 0.0f;
		public float py = 0.0f;
		public float radius = 0.0f;

		private float speed = 0.0f;
		public float Speed {
			get {
				return this.speed;
			}
			set {
				this.speed = Math.Abs(value);
			}
		}

		private Entity center = null;
		private float angle = 0.0f;
		private float angleSpeed = 0.0f;
		private float orbit = 0.0f;

		public void SetCenter(float px, float py) {
			if (this.center == null) {
				return;
			}

			float vxOld = this.px - this.center.px;
			float vyOld = this.py - this.center.py;

			this.center.px = px;
			this.center.py = py;

			float vx = this.px - this.center.px;
			float vy = this.py - this.center.py;
			this.orbit = (float)Math.Sqrt(vx * vx + vy * vy);
			this.angle = (float)Math.Atan2(vy, vx);
			this.angleSpeed = this.speed / this.orbit; // TODO: Fix divide by zero situation.
			if (vx * vxOld + vy * vyOld < 0.0f) {
				this.angleSpeed = -this.angleSpeed;
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
			this.py = this.center.py + this.orbit * (float)Math.Sin(this.angle);
			if (this.px < 0.0f || this.px > 1.0f || this.py < 0.0f || this.py > 1.0f) {
				this.angleSpeed = -this.angleSpeed;
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

		public bool IsCollided(Entity entity) {
			float x = this.px - entity.px;
			float y = this.py - entity.py;
			return (float)Math.Sqrt(x * x + y * y) < this.radius + entity.radius;
		}
	}
}
