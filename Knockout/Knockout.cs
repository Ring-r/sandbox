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
			this.Text = "Knockout Game Prototype";
			this.WindowState = FormWindowState.Maximized;
			this.Paint += this.MainForm_Paint;
			this.KeyUp += this.MainForm_KeyUp;
			this.MouseMove += this.MainForm_MouseMove;
			this.timer.Tick += this.Timer_Tick;
			this.ResumeLayout(false);

			this.CreateLevel();
		}

		private const int needFPS = 60;

		private readonly Stopwatch stopwatch = new Stopwatch();

		private readonly int bonusCount = 10;
		private readonly int blockCount = 10;
		private readonly int controlCount = 3;
		private readonly int controlIndex = 1;

		private readonly int ballRadius = 10;
		private readonly int bonusRadius = 10;
		private readonly int blockRadius = 10;
		private readonly int controlRadius = 10;

		private readonly float ballSpeed = 0.1f;

		private readonly float xMin = 1.0f / 4;
		private readonly float xMax = 3.0f / 4;
		private readonly float yMin = 1.0f / 4;
		private readonly float yMax = 3.0f / 4;

		private readonly EntityCircle ball = new EntityCircle();
		private readonly List<EntityCircle> controlList = new List<EntityCircle>();
		private readonly List<EntityCircle> objectList = new List<EntityCircle>();

		private readonly Color backgroundColor = Color.White;

		private readonly Brush ballBrush = Brushes.Black;
		private readonly Brush bonusBrush = Brushes.Yellow;
		private readonly Brush blockBrush = Brushes.Red;
		private readonly Brush controlBrush = Brushes.Silver;

		private bool isPause = true;
		private bool isShowInfo = true;

		private void MainForm_KeyUp(object sender, KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.Escape:
					this.Close();
					break;
				case Keys.Enter:
					this.CreateLevel();
					break;
				case Keys.Space:
					this.StartLevel();
					break;
				case Keys.F1:
					this.isShowInfo = !this.isShowInfo;
					break;
			}
		}

		private void CreateLevel() {
			this.ball.brush = this.ballBrush;
			this.ball.radius = this.ballRadius;
			this.ball.speed = this.ballSpeed;
			this.ball.time = 0.0f;

			this.objectList.Clear();
			for (int i = 0; i < this.bonusCount; ++i) {
				this.objectList.Add(new EntityCircle() {
					brush = this.bonusBrush,
					radius = this.bonusRadius,
					px = (float)Program.rand.NextDouble() * (this.xMax - this.xMin) + this.xMin,
					py = (float)Program.rand.NextDouble() * (this.yMax - this.yMin) + this.yMin,
				});
			}

			for (int i = 0; i < this.blockCount; ++i) {
				this.objectList.Add(new EntityCircle() {
					brush = this.blockBrush,
					radius = this.blockRadius,
					px = (float)Program.rand.NextDouble() * (this.xMax - this.xMin) + this.xMin,
					py = (float)Program.rand.NextDouble() * (this.yMax - this.yMin) + this.yMin,
				});
			}

			this.controlList.Clear();
			for (int i = 0; i < this.controlCount; ++i) {
				this.controlList.Add(new EntityCircle() {
					brush = this.controlBrush,
					radius = this.controlRadius,
					px = 1.0f * i / (this.controlCount - 1),
					py = 0.5f,
				});
			}

			this.timer.Interval = 1000 / needFPS;
			this.timer.Start();

			this.isPause = true;
		}

		private void StartLevel() {
			this.ball.time = 0;
			this.ball.px = this.controlList[0].px;
			this.ball.py = this.controlList[0].py;
		
			this.isPause = false;
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(this.backgroundColor);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			this.ball.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);

			foreach (var circle in this.objectList) {
				circle.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
			}
			foreach (var circle in this.controlList) {
				circle.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
			}
		}

		private void Timer_Tick(object sender, EventArgs e) {
			float timeEllapsed = 0.001f * this.stopwatch.ElapsedMilliseconds;
			this.stopwatch.Restart();

			if (!this.isPause) {
				if (this.ball.time < 1f) {
					this.ball.time += this.ball.speed * timeEllapsed;

					List<PointF> points = this.controlList.Select(item => new PointF(item.px, item.py)).ToList();
					while (points.Count > 1) {
						for (int i = 0; i < points.Count - 1; ++i) {
							points[i] = new PointF(
								points[i].X * (1 - this.ball.time) + points[i + 1].X * this.ball.time,
								points[i].Y * (1 - this.ball.time) + points[i + 1].Y * this.ball.time);
						}
						points.RemoveAt(points.Count - 1);
					}
					this.ball.px = points[0].X;
					this.ball.py = points[0].Y;
				} else {
					this.ball.px = -100;
					this.ball.py = -100;
				}
			}
			this.Invalidate();
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e) {
			float controlBegin = this.ClientSize.Width / 4;
			float controlLength = this.ClientSize.Width / 2;
			if (controlBegin < e.X && e.X < controlBegin + controlLength) {
				this.controlList[this.controlIndex].py = (e.X - controlBegin) / controlLength;
			}
		}
	}

	class Circle {
		public Brush brush = Brushes.Black;

		public float radius = 0;

		public float px = 0;
		public float py = 0;

		public void Draw(Graphics g, float xScale, float yScale) {
			g.FillEllipse(this.brush, xScale * this.px - this.radius, yScale * this.py - this.radius, 2 * this.radius, 2 * this.radius);
		}
	}

	class EntityCircle : Circle {
		public float speed = 0;
		public float time = 0;
	}
}
