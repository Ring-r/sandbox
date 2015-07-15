using System;
using System.Drawing;
using System.Windows.Forms;

namespace Walk {
	public class Circle {
		public float a;
		public float m;
		public float r;

		public float px;
		public float py;

		public float vx;
		public float vy;

		private readonly bool[] controllers = new bool[4];

		public void ChangeControllers(Keys keys, bool value) {
			if (keys == Keys.Left) {
				this.controllers[0] = value;
			}
			if (keys == Keys.Right) {
				this.controllers[1] = value;
			}
			if (keys == Keys.Up) {
				this.controllers[2] = value;
			}
			if (keys == Keys.Down) {
				this.controllers[3] = value;
			}
		}

		private Tuple<float, float> GetVector() {
			float ax = 0.0f;
			if (this.controllers[0]) {
				ax -= this.a;
			}
			if (this.controllers[1]) {
				ax += this.a;
			}
			float ay = 0.0f;
			if (this.controllers[2]) {
				ay -= this.a;
			}
			if (this.controllers[3]) {
				ay += this.a;
			}
			return Tuple.Create(ax, ay);
		}

		private void UpdateByResistance(float resistance) {
			float d = (float)Math.Sqrt(this.vx * this.vx + this.vy * this.vy);
			if (d > 0 && d > resistance) {
				float coef = (d - resistance) / d;
				this.vx *= coef;
				this.vy *= coef;
			} else {
				this.vx = 0.0f;
				this.vy = 0.0f;
				this.t = 0.0f;
			}
		}

		public void Update(Map map, float t) {
			if (t == 0.0f) {
				return;
			}

			var vector = this.GetVector();
			var vectorMap = map.GetVector(this.px, this.py);
			this.vx += (vector.Item1 + vectorMap.Item1) * t;
			this.vy += (vector.Item2 + vectorMap.Item2) * t;

			var resistance = map.GetResistance(this.px, this.py);
			this.UpdateByResistance(resistance * t);

			this.px += this.vx * t;
			this.py += this.vy * t;

			this.Correct(map.xMin, map.xMax, map.yMin, map.yMax);
		}

		private void Correct(float xMin, float xMax, float yMin, float yMax) {
			if (this.px < xMin + this.r) {
				this.px = xMin + this.r;
			}
			if (this.px >= xMax - this.r) {
				this.px = xMax - this.r;
			}
			if (this.py < yMin + this.r) {
				this.py = yMin + this.r;
			}
			if (this.py >= yMax - this.r) {
				this.py = yMax - this.r;
			}
		}

		public Brush brush = Brushes.Blue;
		public Pen pen = Pens.Black;
		private float t = 0.0f;

		public void Draw(Graphics g) {
			float r = this.r / 4;

			float d = (float)Math.Sqrt(this.vx * this.vx + this.vy * this.vy);
			if (d != 0.0f) {
				float vx = this.vx / d;
				float vy = this.vy / d;
				float vx_ = vy;
				float vy_ = -vx;

				PointF[] points = new PointF[3];
				points[0] = new PointF(this.px + this.r * vx_, this.py + this.r * vy_);
				points[1] = new PointF(this.px + 3 * this.r / 2 * vx, this.py + 3 * this.r / 2 * vy);
				points[2] = new PointF(this.px - this.r * vx_, this.py - this.r * vy_);
				Brush brush = new SolidBrush(Color.FromArgb(150, Color.Blue));
				g.FillPolygon(brush, points);

				float x;
				float y;

				float rFoot = this.r - r / 2;
				float rHand = 2 * r;
				float koef = (float)Math.Sin(this.t / rFoot * (float)Math.PI);
				float dFoot = rFoot * koef;
				float dHand = rHand * koef;

				x = this.px + vx_ * (this.r - r) + dHand * vx;
				y = this.py + vy_ * (this.r - r) + dHand * vy;
				g.FillEllipse(this.brush, x - r, y - r, 2 * r, 2 * r);
				g.DrawEllipse(this.pen, x - r, y - r, 2 * r, 2 * r);

				x = this.px + vx_ * r - dFoot * vx;
				y = this.py + vy_ * r - dFoot * vy;
				g.FillEllipse(this.brush, x - r, y - r, 2 * r, 2 * r);
				g.DrawEllipse(this.pen, x - r, y - r, 2 * r, 2 * r);

				x = this.px - vx_ * (this.r - r) - dHand * vx;
				y = this.py - vy_ * (this.r - r) - dHand * vy;
				g.FillEllipse(this.brush, x - r, y - r, 2 * r, 2 * r);
				g.DrawEllipse(this.pen, x - r, y - r, 2 * r, 2 * r);

				x = this.px - vx_ * r + dFoot * vx;
				y = this.py - vy_ * r + dFoot * vy;
				g.FillEllipse(this.brush, x - r, y - r, 2 * r, 2 * r);
				g.DrawEllipse(this.pen, x - r, y - r, 2 * r, 2 * r);

				this.t += d;
			} else {
				this.t = 0.0f;
			}

			g.FillEllipse(this.brush, this.px - this.r, this.py - this.r, 2 * this.r, 2 * this.r);
			g.DrawEllipse(this.pen, this.px - this.r, this.py - this.r, 2 * this.r, 2 * this.r);
		}
	}
}

