using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace RegionFilter
{
	public partial class MainForm: Form
	{
		private readonly Random random = new Random();

		private const int defaultPointCount = 10;
		private const int defaultRegionCount = 10;

		private readonly List<List<Point3D>> points = new List<List<Point3D>>();
		private IRegionFilter regionFilter = new RegionFilterOther();

		private bool showText = false;
		private bool showLines = false;

		private Point mousePoint = new Point();

		private long updateTime = 0L;
		private long calculationTime = 0L;

		public MainForm()
		{
			InitializeComponent();
		}

		private void UpdateRegion()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			this.regionFilter.Update(this.points);
			stopwatch.Stop();
			this.updateTime = stopwatch.ElapsedMilliseconds;
		}

		private int[] UpdateRegionFilter()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int[] array = new int[this.ClientSize.Width * this.ClientSize.Height];
			int index = 0;
			for (int y = 0; y < this.ClientSize.Height; ++y)
			{
				for (int x = 0; x < this.ClientSize.Width; ++x)
				{
					array[index] = (this.regionFilter.Contains(x, y) ? Color.Green : Color.Blue).ToArgb();
					++index;
				}
			}
			stopwatch.Stop();
			this.calculationTime = stopwatch.ElapsedMilliseconds;

			return array;
		}

		private void CreateRandomPoints()
		{
			for (int i = 0; i < defaultRegionCount; ++i)
			{
				this.points.Add(new List<Point3D>());
				for (int j = 0; j < defaultPointCount; ++j)
				{
					this.points[i].Add(new Point3D(random.NextDouble() * this.ClientSize.Width, random.NextDouble() * this.ClientSize.Height, 0.0));
				}
			}
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
			{
				this.Close();
			}
			if (e.KeyData == Keys.F1)
			{
				this.showText = !this.showText;

				this.Invalidate();
			}
			if (e.KeyData == Keys.F2)
			{
				this.showLines = !this.showLines;

				this.Invalidate();
			}
			if (e.KeyData == Keys.D1)
			{
				this.regionFilter = new RegionFilter();
				this.UpdateRegion();

				this.Invalidate();
			}
			if (e.KeyData == Keys.D2)
			{
				this.regionFilter = new RegionFilterOther();
				this.UpdateRegion();

				this.Invalidate();
			}
			if (e.KeyData == Keys.D3)
			{
				this.regionFilter = new RegionFilter_Fast();
				this.UpdateRegion();

				this.Invalidate();
			}
			if (e.KeyData == Keys.F5)
			{
				this.points.Clear();
				this.UpdateRegion();

				this.Invalidate();
			}
			if (e.KeyData == Keys.F6)
			{
				this.points.Clear();
				this.CreateRandomPoints();
				this.UpdateRegion();

				this.Invalidate();
			}
		}

		private void MainForm_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.White);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
			{
				BitmapData imageData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
				int[] array = this.UpdateRegionFilter();
				Marshal.Copy(array, 0, imageData.Scan0, array.Length);
				bitmap.UnlockBits(imageData);
				e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
			}

			this.ShowLines(e.Graphics);

			float textY = 0;
			e.Graphics.DrawString("F1 - help", this.Font, Brushes.Yellow, 0.0f, textY);
			if (this.showText)
			{
				float textHeight = e.Graphics.MeasureString(" ", this.Font).Height;

				textY += textHeight;
				e.Graphics.DrawString("F2 - show lines", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString("F5 - clean", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString("F6 - create random points", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString("1 - use Pudlo algorithm", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString("2 - use Greenko algorithm (friendly)", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString("3 - use Greenko algorithm (fast)", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString("Mouse left button - add point", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString("Mouse right button - add region part", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString(string.Format("Region points count: {0}", this.points.Sum(part => part.Count)), this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString(string.Format("Update time: {0} milliseconds", this.updateTime), this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString(string.Format("Verificated points count: {0}", this.ClientSize.Width * this.ClientSize.Height), this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				e.Graphics.DrawString(string.Format("Calculation time: {0} milliseconds", this.calculationTime), this.Font, Brushes.Yellow, 0.0f, textY);
			}
		}

		private void ShowLines(Graphics graphics)
		{
			if (this.showLines)
			{
				Color color = Color.Red;
				Pen pen = new Pen(color);
				Pen penTemp = new Pen(color) { DashStyle = DashStyle.Dash };

				foreach (List<Point3D> points in this.points)
				{
					if (points.Count > 1)
					{
						graphics.DrawPolygon(pen, points.Select(point => new PointF((float)(point.X), (float)(point.Y))).ToArray());
					}
				}

				if (this.points.Count != 0)
				{
					List<Point3D> lastPart = this.points[this.points.Count - 1];
					if (lastPart.Count != 0)
					{
						Point3D point;

						point = lastPart[0];
						graphics.DrawLine(penTemp, this.mousePoint.X, this.mousePoint.Y, (float)(point.X), (float)(point.Y));
						point = lastPart[lastPart.Count - 1];
						graphics.DrawLine(penTemp, this.mousePoint.X, this.mousePoint.Y, (float)(point.X), (float)(point.Y));
					}
				}
			}
		}

		private void MainForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (this.points.Count == 0)
				{
					this.points.Add(new List<Point3D>());
				}

				this.points[this.points.Count - 1].Add(new Point3D(e.X, e.Y, 0.0));
				this.UpdateRegion();
			}

			if (e.Button == MouseButtons.Right)
			{
				if (this.points.Count == 0)
				{
					this.points.Add(new List<Point3D>());
				}

				List<Point3D> lastPart = this.points[this.points.Count - 1];
				if (lastPart.Count < 3)
				{
					lastPart.Clear();
				}
				else
				{
					this.points.Add(new List<Point3D>());
				}
			}

			this.Invalidate();
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e)
		{
			this.mousePoint = e.Location;

			if (this.showLines)
			{
				this.Invalidate();
			}
		}
	}
}

