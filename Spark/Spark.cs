using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Spark {
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
			this.Text = "Spark Prototype";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Resize += this.MainForm_Resize;
			this.KeyUp += this.MainForm_KeyUp;
			this.Paint += this.MainForm_Paint;
			this.timer.Tick += this.Timer_Tick;
			this.MouseMove += this.MainForm_MouseMove;
			this.ResumeLayout(false);

			this.CreateLevel();
			this.timer.Start();
		}

		private void MainForm_Resize(object sender, EventArgs e) {
			this.Invalidate();
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				this.Close();
			}
			if (e.KeyCode == Keys.Enter) {
				this.CreateLevel();
			}
		}

		private const float epsCoef = 0.1f;

		private readonly int linesCount = 10;

		private readonly Color background = Color.Black;
		private readonly Pen linePen = Pens.Green;
		private readonly int centerWidth = 100;

		private readonly List<Line> lines = new List<Line>();
		private readonly Entity entity = new Entity() {
			brush = Brushes.Blue,
		};
		private readonly Entity entityBot = new EntityBot() {
			brush = Brushes.Yellow,
		};
		private float initialEntitySpeed = 0.05f;
		private float initialEntityOffsetTime = 2.0f;

		private int score = 0;

		private bool isEnd = false;

		private int x = 0;

		private Line CreateLine() {
			float px = (float)Program.rand.NextDouble();
			float py = (float)Program.rand.NextDouble();
			float angle = (float)(Math.PI * Program.rand.NextDouble());
			float vx = (float)Math.Cos(angle);
			float vy = (float)Math.Sin(angle);
			return new Line(){ px = px, py = py, vx = vx, vy = vy };
		}

		private static void CalculateAndAdd(Line iLine, Line jLine, bool useCondition = false) {
			float t = iLine.vx * jLine.vy - jLine.vx * iLine.vy;
			if (t != 0) {
				float jipx = (jLine.px - iLine.px);
				float jipy = (jLine.py - iLine.py);
				float coef = 1 / t;
				float iT = (jipx * jLine.vy - jipy * jLine.vx) * coef;
				float jT = (jipx * iLine.vy - jipy * iLine.vx) * coef;
				if (!useCondition || (iLine.ts[0].Item1 <= iT && iT <= iLine.ts[1].Item1 && jLine.ts[0].Item1 <= jT && jT <= jLine.ts[1].Item1)) {
					iLine.ts.Add(Tuple.Create(iT, jLine));
					jLine.ts.Add(Tuple.Create(jT, iLine));
				}
			}
		}

		private void CreateLevel() {
			this.lines.Clear();

			var baseLines = new List<Line>();
			baseLines.Add(new Line(){ px = 0.0f, py = 0.0f, vx = +1.0f, vy = +0.0f });
			baseLines.Add(new Line(){ px = 1.0f, py = 0.0f, vx = +0.0f, vy = +1.0f });
			baseLines.Add(new Line(){ px = 1.0f, py = 1.0f, vx = -1.0f, vy = -0.0f });
			baseLines.Add(new Line(){ px = 0.0f, py = 1.0f, vx = -0.0f, vy = -1.0f });
			for (int i = 0; i < baseLines.Count - 1; ++i) {
				for (int j = i + 1; j < baseLines.Count; ++j) {
					MainForm.CalculateAndAdd(baseLines[i], baseLines[j]);
				}
			}

			for (int i = 0; i < this.linesCount; ++i) {
				this.lines.Add(this.CreateLine());

				var iLine = this.lines[i];

				for (int j = 0; j < baseLines.Count; ++j) {
					MainForm.CalculateAndAdd(iLine, baseLines[j]);
				}
				iLine.ts.Sort((itemLeft, itemRight) => Math.Sign(itemLeft.Item1 - itemRight.Item1));
				iLine.RemoveBorderParts();

				for (int j = 0; j < i - 1; ++j) {
					MainForm.CalculateAndAdd(iLine, this.lines[j], true);
				}
			}

			this.lines.AddRange(baseLines);

			foreach (var line in this.lines) {
				line.ts.Sort((itemLeft, itemRight) => Math.Sign(itemLeft.Item1 - itemRight.Item1));
			}

			Line randomLine = this.lines[Program.rand.Next(this.lines.Count)];

			this.entity.Clear();
			this.entity.line = randomLine;
			this.entity.speed = this.initialEntitySpeed;
			this.entity.offsetTime = this.initialEntityOffsetTime;

			this.entityBot.Clear();
			this.entityBot.line = randomLine;
			this.entityBot.speed = this.initialEntitySpeed;

			this.score = 0;
			this.isEnd = false;
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(background);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			float scale = 0.9f * Math.Min(this.ClientSize.Width, this.ClientSize.Height);
			e.Graphics.TranslateTransform(this.ClientSize.Width / 2 - scale / 2, this.ClientSize.Height / 2 - scale / 2);

			foreach (var line in this.lines) {
				float x0 = (line.px + line.vx * line.ts[0].Item1) * scale;
				float y0 = (line.py + line.vy * line.ts[0].Item1) * scale;
				float x1 = (line.px + line.vx * line.ts[line.ts.Count - 1].Item1) * scale;
				float y1 = (line.py + line.vy * line.ts[line.ts.Count - 1].Item1) * scale;
				e.Graphics.DrawLine(this.linePen, x0, y0, x1, y1);
			}

			this.entity.Draw(e.Graphics, scale);
			this.entityBot.Draw(e.Graphics, scale);

			e.Graphics.ResetTransform();

			e.Graphics.DrawLine(Pens.Gray, this.ClientSize.Width / 2 - this.centerWidth / 2, 0, this.ClientSize.Width / 2 - this.centerWidth / 2, this.ClientSize.Height);
			e.Graphics.DrawLine(Pens.Gray, this.ClientSize.Width / 2 + this.centerWidth / 2, 0, this.ClientSize.Width / 2 + this.centerWidth / 2, this.ClientSize.Height);

//			if (this.isEnd) {
//				string text0 = "You loose aim!";
//				SizeF text0Size = e.Graphics.MeasureString(text0, this.Font);
//				e.Graphics.DrawString(text0, this.Font, Brushes.White,
//					this.ClientSize.Width / 2 - text0Size.Width / 2, this.ClientSize.Height / 2 - text0Size.Height / 2);
//
//				string text1 = "Type \"Enter\" to start new game!";
//				SizeF text1Size = e.Graphics.MeasureString(text1, this.Font);
//				e.Graphics.DrawString(text1, this.Font, Brushes.White,
//					this.ClientSize.Width / 2 - text1Size.Width / 2, this.ClientSize.Height / 2 + text0Size.Height / 2);
//			}
		}

		private void Timer_Tick(object sender, EventArgs e) {
			int centerX = this.ClientSize.Width / 2;
			if (Math.Abs(this.x - centerX) < this.centerWidth / 2) {
				this.entity.directionValue = 0;
			} else {
				this.entity.directionValue = this.x - centerX;
			}

			float time = this.timer.Interval / 1000.0f;
			this.entity.Update(time);
			this.entityBot.Update(time);

			if (this.entity.queue.Count > 0 && this.entityBot.queue.Count > 0) {
				if (this.entity.queue.Peek() == this.entityBot.queue.Peek()) {
					this.entity.queue.Dequeue();
					this.entityBot.queue.Dequeue();
					this.score += 1;
				} else {
					this.isEnd = true;
				}
			}

			this.Invalidate();
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e) {
			if (this.isEnd) {
				//return;
			}
			this.x = e.X;
		}
	}

	class Line {
		public float px = 0.0f;
		public float py = 0.0f;
		public float vx = 1.0f;
		public float vy = 0.0f;
		public readonly List<Tuple<float, Line>> ts = new List<Tuple<float, Line>>();

		public void RemoveBorderParts() {
			// FIXME: Imcorrect work when some Item2 equal to 0.

			int i0 = 0;
			while (i0 < this.ts.Count - 1 && this.ts[i0].Item1 < 0 && this.ts[i0 + 1].Item1 < 0) {
				this.ts[i0].Item2.ts.RemoveAll(item => item.Item2 == this);
				i0 += 1;
			}
			this.ts.RemoveRange(0, i0);

			int i1 = 1;
			while (i1 < this.ts.Count - 1 && this.ts[i1].Item1 > 0 && this.ts[i1 + 1].Item1 > 0) {
				// TODO: Check equal to 0.
				this.ts[i1 + 1].Item2.ts.RemoveAll(item => item.Item2 == this);
				i1 += 1;
			}
			this.ts.RemoveRange(2, i1 - 1);
		}
	}

	class Entity {
		private const int directionEps = 5;
		private readonly float radius = 3.0f;

		protected int direction = 0;

		protected virtual void RecalculateDirection() {
			this.direction = Math.Sign(this.directionValue);
		}

		public Line line = null;
		public float offset = 0.0f;
		public float speed = 0.0f;

		public float offsetTime = 0.0f;

		public int directionValue = 0;

		public Brush brush = Brushes.Black;

		public readonly Queue<Line> queue = new Queue<Line>();

		public void Clear() {
			this.line = null;
			this.offset = 0.0f;
			this.direction = 0;
			this.speed = 0.0f;

			this.offsetTime = 0.0f;

			this.directionValue = 0;

			this.queue.Clear();
		}

		public void Draw(Graphics g, float scale) {
			float x = (this.line.px + this.line.vx * this.offset) * scale;
			float y = (this.line.py + this.line.vy * this.offset) * scale;
			g.FillEllipse(this.brush, x - this.radius, y - this.radius, 2 * this.radius, 2 * this.radius);
		}

		public void Update(float time) {
			if (this.offsetTime > 0) {
				this.offsetTime -= time;
				return;
			}

			float step = this.speed * time;
			float stepEps = Math.Abs(0.9f * step);

			this.offset += step;

			var prevLine = this.line;

			bool changeLine = false;
			int prevOffsetIndex = -1;
			if (this.offset < prevLine.ts[0].Item1) {
				this.offset = prevLine.ts[0].Item1;
				prevOffsetIndex = 0;
				changeLine = true;
			}
			if (this.offset > prevLine.ts[prevLine.ts.Count - 1].Item1) {
				this.offset = prevLine.ts[prevLine.ts.Count - 1].Item1;
				prevOffsetIndex = prevLine.ts.Count - 1;
				changeLine = true;
			}
			if (!changeLine) {
				prevOffsetIndex = prevLine.ts.FindIndex(item => Math.Abs(item.Item1 - this.offset) < stepEps);
				if (0 <= prevOffsetIndex && prevOffsetIndex < prevLine.ts.Count) {
					this.RecalculateDirection();
					changeLine = this.direction != 0;
					this.queue.Enqueue(prevLine);
				}
			}

			if (changeLine) {
				this.line = prevLine.ts[prevOffsetIndex].Item2;

				int offsetIndex = this.line.ts.FindIndex(item => item.Item2 == prevLine);
				this.offset = this.line.ts[offsetIndex].Item1;

				bool isUseEntityDirection = true;
				if (offsetIndex == 0) {
					this.speed = Math.Abs(this.speed);
					isUseEntityDirection = false;
				}
				if (offsetIndex == this.line.ts.Count - 1) {
					this.speed = -Math.Abs(this.speed);
					isUseEntityDirection = false;
				}
				if (isUseEntityDirection) {
					// TODO: Correct by using dot vector production.
					if (this.direction != 0) {
						int sign = Math.Sign(this.speed);
						float prevVx = sign * prevLine.vx;
						float prevVy = sign * prevLine.vy;
						float prevVx_ = -prevVy;
						float prevVy_ = +prevVx;
						float prod = prevVx_ * this.line.vx + prevVy_ * this.line.vy;
						this.speed = Math.Sign(this.direction * prod) * Math.Abs(this.speed);
					}
				}			

				this.queue.Enqueue(this.line);
			}
		}
	}

	class EntityBot : Entity {
		public readonly float changeConditionCoef = 0.3f;

		protected override void RecalculateDirection() {
			if (Program.rand.NextDouble() < this.changeConditionCoef) {
				this.direction = 0;
			} else {
				this.direction = Program.rand.NextDouble() < 0.5 ? -1 : +1;
			}
		}
	}
}
