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
			speed = Options.ballSpeed,
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
			this.bonusCount = 0
			this.state = State.IsRun;
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

			str = this.avalableTouchCount.ToString();
			strSize = e.Graphics.MeasureString(str, Options.font);
			e.Graphics.DrawString(str, Options.font, Brushes.Black, this.ClientSize.Width - strSize.Width, 0.0f);

			switch (this.state) {
				case State.CollidedWithBlock:
					str = "BOOM";
					strSize = e.Graphics.MeasureString(str, Options.font);
					e.Graphics.DrawString(str, Options.font, Brushes.Red, this.ClientSize.Width / 2 - strSize.Width / 2, this.ClientSize.Height / 2 - strSize.Height / 2);
					break;
				case State.NotEnoughPoints:
					str = "No power points.";
					strSize = e.Graphics.MeasureString(str, Options.font);
					e.Graphics.DrawString(str, Options.font, Brushes.Red, this.ClientSize.Width / 2 - strSize.Width / 2, this.ClientSize.Height / 2 - strSize.Height / 2);
					break;
				case State.NotEnoughScores:
					str = "Not enough scores.";
					strSize = e.Graphics.MeasureString(str, Options.font);
					e.Graphics.DrawString(str, Options.font, Brushes.Red, this.ClientSize.Width / 2 - strSize.Width / 2, this.ClientSize.Height / 2 - strSize.Height / 2);
					break;
				case State.Win:
					str = "You win!";
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
			if (this.state != State.IsRun) {
				return;
			}

			this.ball.Update(this.timer.Interval * Options.toSeconds);
			for (int i = 0; i < this.blockList.Count && this.state == State.IsRun; ++i) {
				var block = this.blockList[i];
				if (this.ball.IsCollided(block)) {
					switch (block.type) {
						case Entity.Block:
							this.state = State.CollidedWithBlock;
							break;
						case Entity.Bonus:
							this.blockList.RemoveAt(i);
							this.score += 1;
							i -= 1;
							break;
						case Entity.Finish:
							var bonusPercent = 1.0 * this.bonusCount / Options.bonusCount;
							this.state = bonusPercent < Options.bonusPercent ? State.NotEnoughScores : State.Win;
							break;
					}
				}
			}

			if (this.avalableTouchCount < 0) {
				this.state = State.NotEnoughPoints;
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
				if (block.type == 0) {
					float x = block.px - ex;
					float y = block.py - ey;
					float d = (float)Math.Sqrt(x * x + y * y);
					if (d < block.radius) {
						if (this.ball.SetCenter(block)) {
							this.avalableTouchCount -= Options.usedTouchCount;
						}
					}
				}
			}
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

		public float speed = 0.0f;

		private Entity center = null;
		private float angle = 0.0f;
		private float angleSpeed = 0.0f;
		private float orbit = 0.0f;

		public bool SetCenter(Entity center) {
			bool res = true;
			if (this.center == center) {
				this.speed = -this.speed;
				this.angleSpeed = -this.angleSpeed;
				res = false;
			} else {
				this.center = center;
				float x = this.px - this.center.px;
				float y = this.py - this.center.py;
				this.orbit = (float)Math.Sqrt(x * x + y * y);
				this.angle = (float)Math.Atan2(y, x);
				this.angleSpeed = Math.Sign(this.speed) * (float)Math.Asin(Math.Abs(this.speed) / this.orbit);
			}
			return res;
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
				this.speed = -this.speed;
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
