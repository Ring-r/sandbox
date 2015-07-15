using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Walk {
	static class Program {
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		public static readonly Random rand = new Random();
	}

	public class MainForm : Form {
		private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
		private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer(){ Interval = 20 };

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		public MainForm() {
			this.SuspendLayout();
			this.DoubleBuffered = true;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Walk Game Prototype";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.KeyDown += this.MainForm_KeyDown;
			this.KeyUp += this.MainForm_KeyUp;
			this.MouseDown += this.MainForm_MouseDown;
			this.MouseMove += this.MainForm_MouseMove;
			this.MouseUp += this.MainForm_MouseUp;
			this.Resize += this.MainForm_Resize;
			this.Paint += this.MainForm_Paint;
			this.timer.Tick += this.Timer_Tick;
			this.ResumeLayout(false);

			this.CreateLevel();

			this.timer.Start();
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e) {
			this.circle.ChangeControllers(e.KeyCode, true);
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				this.Close();
			}
			if (e.KeyCode == Keys.Enter) {
				this.CreateLevel();
				this.Invalidate();
			}

			this.circle.ChangeControllers(e.KeyCode, false);
		}

		private void MainForm_MouseDown(object sender, MouseEventArgs e) {
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e) {
		}

		private void MainForm_MouseUp(object sender, MouseEventArgs e) {
		}

		private void MainForm_Resize(object sender, EventArgs e) {
			this.Invalidate();
		}

		private readonly Stopwatch stopwatch = new Stopwatch();
		private readonly Stopwatch stopwatchLevel = new Stopwatch();

		private const float acceleration = 100.0f;
		private const float mass = 1.0f;
		private const float radius = 30.0f;

		private const int cellCount = 50;
		private const float cellSize = 100;
		private const float gravity = 9.8f * 0;
		private const float resistance = 10.0f;
		private const int checkPointCount = 10;

		private readonly Circle circle = new Circle() {
			a = acceleration,
			m = mass,
			r = radius, 
			brush = Brushes.Blue,
		};

		private readonly Map map = new Map();

		private int res = 0;

		private void CreateLevel() {
			this.map.CreateLand(cellCount, cellCount, gravity, resistance);
			this.map.cellSize = cellSize;

			this.map.FillCheckPoints(checkPointCount);
			this.res = 0;

			this.stopwatchLevel.Restart();
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(Color.Black);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			e.Graphics.TranslateTransform(-this.circle.px, -this.circle.py);
			e.Graphics.TranslateTransform(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

			this.map.Draw(e.Graphics);
			// this.map.DrawBorder (e.Graphics);
			// this.map.DrawVectors (e.Graphics);

			this.circle.Draw(e.Graphics);

			e.Graphics.ResetTransform();
			e.Graphics.DrawString(string.Format("You've check a {0} points and spend a {1} time.", this.res, this.stopwatchLevel.Elapsed.TotalSeconds), this.Font, Brushes.White, 0, 0); 
		}

		private void Timer_Tick(object sender, EventArgs e) {
			float t = (float)this.stopwatch.Elapsed.TotalSeconds;
			this.stopwatch.Restart();
			this.circle.Update(this.map, t);

			int i = (int)Math.Floor(this.circle.px / cellSize);
			int j = (int)Math.Floor(this.circle.py / cellSize);
			if (this.map.cells[i, j].type == 1) {
				this.map.cells[i, j].type = 0;
				this.res += 1;
				if (this.res == checkPointCount) {
					this.stopwatchLevel.Stop();
				}
			}

			this.Invalidate();
		}
	}
}
