using Prototypes.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AlgorithmsAndDataStructures;

namespace Buildings
{
	public partial class MainForm : FormWithYieldTimer
	{
		private readonly Random random = new Random();
		private readonly float eps = 5.0f;

		private readonly List<PointF> points = new List<PointF>();
		private readonly List<PointF> polygon = new List<PointF>();
		private readonly PolygonBuilder polygonBuilder = new PolygonBuilder();

		private readonly float stepSize = 10.0f;
		private readonly float pointBigRadius = 3.0f;
		private readonly float pointSmallRadius = 1.0f;

		private bool showPolygon = true;

		public MainForm()
		{
			this.SuspendLayout();

			this.KeyDown += this.MainForm_KeyDown;
			this.MouseClick += this.MainForm_MouseClick;

			this.ResumeLayout(false);
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.F1)
			{
				this.showPolygon = !this.showPolygon;
				this.Invalidate();
			}
		}

		private void MainForm_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (this.points.Count > 0)
				{
					this.Init();
					this.polygon.Clear();
					this.polygonBuilder.Clear();
				}
				this.points.Clear();
				this.polygon.Add(e.Location);
			}
			if (e.Button == MouseButtons.Right)
			{
				this.FillPoints();
				//this.FillPointsRandom();
			}
			this.Invalidate();
		}

		protected override void Form_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.White);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.TranslateTransform(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
			e.Graphics.ScaleTransform(this.scale, this.scale);
			e.Graphics.TranslateTransform(-this.ClientSize.Width / 2, -this.ClientSize.Height / 2);

			if (this.showPolygon)
			{
				if (this.polygon.Count > 2)
				{
					e.Graphics.DrawPolygon(Pens.Silver, this.polygon.ToArray());
				}
				foreach (PointF point in this.polygon)
				{
					e.Graphics.FillEllipse(Brushes.Blue, point.X - this.pointBigRadius, point.Y - this.pointBigRadius, 2 * this.pointBigRadius, 2 * this.pointBigRadius);
				}
			}

			foreach (PointF point in this.points)
			{
				e.Graphics.FillEllipse(Brushes.Black, point.X - this.pointSmallRadius, point.Y - this.pointSmallRadius, 2 * this.pointSmallRadius, 2 * this.pointSmallRadius);
			}

			if (this.polygonBuilder.contour.Count > 0)
			{
				List<PointF> points = this.polygonBuilder.contour.ConvertAll(pointIndex =>
				{
					Vector3d vector3D = this.polygonBuilder.points[pointIndex];
					return new PointF((float)vector3D.X, (float)vector3D.Y);
				});
				//e.Graphics.DrawPolygon(Pens.Pink, points.ToArray());
				e.Graphics.DrawLines(Pens.Pink, points.ToArray());
				e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(50, Color.Yellow)), points.ToArray());
			}
		}

		private void FillPoints()
		{
			this.points.Clear();
			for (float x = 0; x < this.ClientSize.Width; x += this.stepSize)
			{
				for (int i = 0; i < this.polygon.Count; ++i)
				{
					int j = (i + 1) % this.polygon.Count;
					PointF pi = this.polygon[i];
					PointF pj = this.polygon[j];
					float a = (x - pi.X) / (pj.X - pi.X);
					if (0 <= a && a < 1)
					{
						float y = a * pj.Y + (1 - a) * pi.Y;
						this.points.Add(new PointF(x + this.eps * ((float)this.random.NextDouble() - 0.5f), y + this.eps * ((float)this.random.NextDouble() - 0.5f)));
					}
				}
			}
			for (float y = 0; y < this.ClientSize.Height; y += this.stepSize)
			{
				for (int i = 0; i < this.polygon.Count; ++i)
				{
					int j = (i + 1) % this.polygon.Count;
					PointF pi = this.polygon[i];
					PointF pj = this.polygon[j];
					float a = (y - pi.Y) / (pj.Y - pi.Y);
					if (0 <= a && a < 1)
					{
						float x = a * pj.X + (1 - a) * pi.X;
						this.points.Add(new PointF(x + this.eps * ((float)this.random.NextDouble() - 0.5f), y + this.eps * ((float)this.random.NextDouble() - 0.5f)));
					}
				}
			}
		}

		private void FillPointsRandom()
		{
			this.points.Clear();
			for (int i = 0; i < 1000; ++i)
			{
				float x = this.ClientSize.Width / 3 + this.random.Next(this.ClientSize.Width / 3);
				float y = this.ClientSize.Height / 3 + this.random.Next(this.ClientSize.Height / 3);
				this.points.Add(new PointF(x, y));
			}
		}

		protected override void Init()
		{
			this.DisposeEnumerator();

			this.FillPointsRandom();
			List<Vector3d> locators = this.points.ConvertAll(point => new Vector3d(point.X, point.Y, 0.0));
			this.polygonBuilder.Clear();
			this.polygonBuilder.Init(locators);
			this.enumerator = this.polygonBuilder.BuildContour(2 * this.stepSize).GetEnumerator();
		}
	}
}
