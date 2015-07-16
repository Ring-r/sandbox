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
		private readonly int blockCount = 100;
		private readonly int controlCount = 2;

		private readonly int ballRadius = 10;
		private readonly int bonusRadius = 10;
		private readonly int blockRadius = 10;
		private readonly int controlRadius = 10;

		private readonly float ballSpeed = 0.1f;

		private readonly EntityCircle ball = new EntityCircle();
		private readonly List<EntityCircle> controlList = new List<EntityCircle>();
		private readonly List<EntityCircle> objectList = new List<EntityCircle>();

		private readonly Color backgroundColor = Color.White;

		private readonly Brush ballBrush = Brushes.Black;
		private readonly Brush bonusBrush = Brushes.Yellow;
		private readonly Brush blockBrush = Brushes.Red;
		private readonly Brush controlBrush = Brushes.Silver;

		private int x = 0;
		private int y = 0;

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

		private void ChangeControlList() {
			this.controlList[0].px = this.controlList[this.controlList.Count - 1].px;
			this.controlList[0].py = this.controlList[this.controlList.Count - 1].py;
			for (int i = 1; i < this.controlCount; ++i) {
				var control = this.controlList[i];
				control.px = (float)Program.rand.NextDouble();
				control.py = (float)Program.rand.NextDouble();
			}
		}

		private void CreateLevel() {
			this.ball.brush = this.ballBrush;
			this.ball.radius = this.ballRadius;
			this.ball.speed = this.ballSpeed;
			this.ball.alfa = 0.0f;

			this.objectList.Clear();
			for (int i = 0; i < this.bonusCount; ++i) {
				this.objectList.Add(new EntityCircle() {
					brush = this.bonusBrush,
					radius = this.bonusRadius,
					px = (float)Program.rand.NextDouble(),
					py = (float)Program.rand.NextDouble(),
				});
			}

			for (int i = 0; i < this.blockCount; ++i) {
				this.objectList.Add(new EntityCircle() {
					brush = this.blockBrush,
					radius = this.blockRadius,
					px = (float)Program.rand.NextDouble(),
					py = (float)Program.rand.NextDouble(),
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
			this.ball.alfa = 0;
			this.ball.px = this.controlList[0].px;
			this.ball.py = this.controlList[0].py;
		
			this.isPause = false;
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(this.backgroundColor);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			float x = this.x - this.ClientSize.Width / 2;
			float y = this.y - this.ClientSize.Height / 2;
			float angle = (float)Math.Atan2(y, x);

			float scale = Math.Min(this.ClientSize.Width, this.ClientSize.Height);

			e.Graphics.TranslateTransform(-scale / 2, -scale / 2);
			e.Graphics.TranslateTransform(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

			this.ball.Draw(e.Graphics, scale);
			foreach (var control in this.controlList) {
				control.Draw(e.Graphics, scale);
			}

			foreach (var circle in this.objectList) {
				circle.Draw(e.Graphics, scale, angle);
			}
		}

		private void Timer_Tick(object sender, EventArgs e) {
			float timeEllapsed = 0.001f * this.stopwatch.ElapsedMilliseconds;
			this.stopwatch.Restart();

			if (this.isPause) {
				return;
			}

			this.ball.alfa += this.ball.speed * timeEllapsed;

			if (this.ball.alfa > 1.0f) {
				this.ChangeControlList();
				this.ball.alfa -= 1.0f;
			}

			List<PointF> points = this.controlList.Select(item => new PointF(item.px, item.py)).ToList();
			for (int j = 0; j < points.Count - 1; ++j) {
				for (int i = 0; i < points.Count - 1; ++i) {
					points[i] = new PointF(
						points[i].X * (1 - this.ball.alfa) + points[i + 1].X * this.ball.alfa,
						points[i].Y * (1 - this.ball.alfa) + points[i + 1].Y * this.ball.alfa);
				}
			}
			this.ball.px = points[0].X;
			this.ball.py = points[0].Y;

			this.Invalidate();
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e) {
			this.x = e.X;
			this.y = e.Y;

			this.Invalidate();
		}
	}

	class Circle {
		public Brush brush = Brushes.Black;

		public float radius = 0;

		public float px = 0;
		public float py = 0;

		public void Draw(Graphics g, float scale, float angle = 0.0f) {
			float x = this.px - 0.5f;
			float y = this.py - 0.5f;
			float xx = x * (float)Math.Cos(angle) - y * (float)Math.Sin(angle);
			float yy = x * (float)Math.Sin(angle) + y * (float)Math.Cos(angle);
			x = xx + 0.5f;
			y = yy + 0.5f;
			g.FillEllipse(this.brush, scale * x - this.radius, scale * y - this.radius, 2 * this.radius, 2 * this.radius);
		}
	}

	class EntityCircle : Circle {
		public float alfa = 0.0f;
		public float speed = 0.0f;
	}
}
