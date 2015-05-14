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
		#region Private fields.

		private readonly Random random = new Random();

		private const int defaultPointCount = 10;
		private const int defaultRegionCount = 10;

		private readonly List<List<Point3D>> region = new List<List<Point3D>>();
		private IRegionFilter regionFilter = new RegionFilter_Fast();

		private Bitmap bitmap = new Bitmap(1, 1);

		private long updateTime = 0L;
		private long calculationTime = 0L;

		private bool isDrawLines = true;
		private bool isDrawPoints = true;
		private bool isDrawStrings = true;

		private int indexPart = -1;
		private int indexLine = -1;
		private int indexPoint = -1;

		private Color drawColor = Color.Red;
		private int alpha = 100;
		private int alphaCurrent = 255;
		private float penWidth = 1.0f;
		private float penWidthCurrent = 2.0f;
		private readonly float pointSize = 10.0f;

		private Point mousePoint = new Point();
		private bool updateAfterMouseMove = false;

		#endregion Private fields.

		private void UpdateRegion()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			this.regionFilter.Update(this.region);
			stopwatch.Stop();
			this.updateTime = stopwatch.ElapsedMilliseconds;
		}

		private void UpdatePoints()
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

			BitmapData imageData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			Marshal.Copy(array, 0, imageData.Scan0, array.Length);
			bitmap.UnlockBits(imageData);
		}

		private void CreateRandomPoints()
		{
			for (int i = 0; i < defaultRegionCount; ++i)
			{
				this.region.Add(new List<Point3D>());
				for (int j = 0; j < defaultPointCount; ++j)
				{
					this.region[i].Add(new Point3D(random.NextDouble() * this.ClientSize.Width, random.NextDouble() * this.ClientSize.Height, 0.0));
				}
			}
		}

		private void DrawLines(Graphics graphics)
		{
			if (!this.isDrawLines)
			{
				return;
			}

			for (int j = 0; j < this.region.Count; ++j)
			{
				Pen pen = new Pen(Color.FromArgb(j == this.indexPart ? this.alphaCurrent : this.alpha, this.drawColor));

				List<Point3D> regionPart = this.region[j];
				for (int i = 0; i < regionPart.Count; ++i)
				{
					pen.Width = j == this.indexPart && i == this.indexLine ? this.penWidthCurrent : this.penWidth;

					Point3D regionPointSource = regionPart[i];
					Point3D regionPointTarget = regionPart[i != regionPart.Count - 1 ? i + 1 : 0];
					graphics.DrawLine(pen, (float)(regionPointSource.X), (float)(regionPointSource.Y), (float)(regionPointTarget.X), (float)(regionPointTarget.Y));
				}
			}
		}

		private void DrawPoints(Graphics graphics)
		{
			if (!this.isDrawPoints)
			{
				return;
			}

			for (int j = 0; j < this.region.Count; ++j)
			{
				Pen pen = new Pen(Color.FromArgb(j == this.indexPart ? this.alphaCurrent : this.alpha, this.drawColor));

				List<Point3D> regionPart = this.region[j];
				for (int i = 0; i < regionPart.Count; ++i)
				{
					pen.Width = j == this.indexPart && i == this.indexPoint ? this.penWidthCurrent : this.penWidth;

					Point3D regionPoint = regionPart[i];
					graphics.DrawRectangle(pen, (float)(regionPoint.X) - this.pointSize / 2, (float)(regionPoint.Y) - this.pointSize / 2, this.pointSize, this.pointSize);
				}
			}
		}

		private void DrawStrings(Graphics graphics)
		{
			float textY = 0;
			graphics.DrawString("F1 - help", this.Font, Brushes.Yellow, 0.0f, textY);
			if (this.isDrawStrings)
			{
				float textHeight = graphics.MeasureString(" ", this.Font).Height;

				textY += textHeight;
				graphics.DrawString("F2 - show lines", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				graphics.DrawString("F5 - clean", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				graphics.DrawString("F6 - create random points", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				graphics.DrawString("Mouse left button - add point", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				graphics.DrawString("Mouse right button - del point", this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				graphics.DrawString(string.Format("Region points count: {0}", this.region.Sum(part => part.Count)), this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				graphics.DrawString(string.Format("Update time: {0} milliseconds", this.updateTime), this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				graphics.DrawString(string.Format("Verificated points count: {0}", this.ClientSize.Width * this.ClientSize.Height), this.Font, Brushes.Yellow, 0.0f, textY);
				textY += textHeight;
				graphics.DrawString(string.Format("Calculation time: {0} milliseconds", this.calculationTime), this.Font, Brushes.Yellow, 0.0f, textY);
			}
		}

		private bool UpdateCurrentLinePointIndex()
		{
			bool isDone = false;

			int indexLine = -1;
			int indexPoint = -1;
			indexLine = this.FindCurrentLineIndex();
			indexPoint = this.FindCurrentPointIndex();
			if (this.indexLine != indexLine || this.indexPoint != indexPoint)
			{
				this.indexLine = indexLine;
				this.indexPoint = indexPoint;
				isDone = true;
			}

			return isDone;
		}

		private int FindCurrentLineIndex()
		{
			if (!this.isDrawLines || this.indexPart < 0)
			{
				return -1;
			}

			int indexLine = -1;
			List<Point3D> regionPart = this.region[this.indexPart];
			for (int i = 0; i < regionPart.Count; ++i)
			{
				Point3D regionPointSource = regionPart[i];
				Point3D regionPointTarget = regionPart[i != regionPart.Count - 1 ? i + 1 : 0];
				double vx = regionPointTarget.X - regionPointSource.X;
				double vy = regionPointTarget.Y - regionPointSource.Y;
				double d = Math.Sqrt(vx * vx + vy * vy);
				if (d != 0)
				{
					vx /= d;
					vy /= d;
					if (Math.Abs((this.mousePoint.X - regionPointSource.X) * vy - (this.mousePoint.Y - regionPointSource.Y) * vx) <= this.pointSize / 2)
					{
						indexLine = i;
						break;
					}
				}
			}
			return indexLine;
		}

		private int FindCurrentPointIndex()
		{
			if (!this.isDrawPoints || this.indexPart < 0)
			{
				return -1;
			}

			int indexPoint = -1;
			List<Point3D> regionPart = this.region[this.indexPart];
			for (int i = 0; i < regionPart.Count; ++i)
			{
				Point3D regionPoint = regionPart[i];
				if (Math.Abs(this.mousePoint.X - regionPoint.X) <= this.pointSize / 2 && Math.Abs(this.mousePoint.Y - regionPoint.Y) <= this.pointSize / 2)
				{
					indexPoint = i;
					break;
				}
			}
			return indexPoint;
		}

		private bool AddPoint(double x, double y)
		{
			bool isDone = false;

			if (this.indexPart < 0)
			{
				this.region.Add(new List<Point3D>());
				this.indexPart = 0;
			}

			List<Point3D> regionPart = this.region[this.indexPart];
			if (regionPart.Count < 3)
			{
				regionPart.Add(new Point3D(x, y, 0.0));
				isDone = true;
			}
			else if (this.indexLine >= 0)
			{
				regionPart.Insert(this.indexLine + 1, new Point3D(x, y, 0.0));
				isDone = true;
			}
			return isDone;
		}

		private bool DelPoint()
		{
			bool isDone = false;

			if (this.indexPart < 0)
			{
				return false;
			}

			List<Point3D> regionPart = this.region[this.indexPart];
			if (this.indexPoint >= 0)
			{
				regionPart.RemoveAt(this.indexPoint);
				isDone = true;
			}
			return isDone;
		}

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Resize(object sender, EventArgs e)
		{
			this.bitmap.Dispose();
			this.bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			this.UpdatePoints();
			this.Invalidate();
		}

		private void MainForm_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(this.BackColor);

			e.Graphics.DrawImageUnscaled(this.bitmap, 0, 0);

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			this.DrawLines(e.Graphics);

			this.DrawPoints(e.Graphics);

			this.DrawStrings(e.Graphics);
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
			{
				this.Close();
			}
			if (e.KeyData == Keys.F1)
			{
				this.isDrawStrings = !this.isDrawStrings;

				this.Invalidate();
			}
			if (e.KeyData == Keys.F2)
			{
				this.isDrawLines = !this.isDrawLines;

				this.Invalidate();
			}
			if (e.KeyData == Keys.F3)
			{
				this.isDrawPoints = !this.isDrawPoints;

				this.Invalidate();
			}
			if (e.KeyData == Keys.F5)
			{
				this.region.Clear();
				this.UpdateRegion();
				this.UpdatePoints();

				this.Invalidate();
			}
			if (e.KeyData == Keys.F6)
			{
				this.region.Clear();
				this.CreateRandomPoints();
				this.UpdateRegion();
				this.UpdatePoints();

				this.Invalidate();
			}
			if (e.KeyData == Keys.PageDown)
			{
				this.indexPart -= 1;
				if (this.indexPart < 0)
				{
					this.indexPart = this.region.Count - 1;
				}

				this.Invalidate();
			}
			if (e.KeyData == Keys.PageUp)
			{
				this.indexPart += 1;
				if (this.indexPart >= this.region.Count)
				{
					this.indexPart = 0;
				}

				this.Invalidate();
			}
			if (e.KeyData == Keys.Enter)
			{
				if (this.region.Count == 0)
				{
					this.region.Add(new List<Point3D>());
					this.indexPart = 0;
				}

				List<Point3D> regionPart = this.region[this.indexPart];
				if (regionPart.Count < 3)
				{
					regionPart.Clear();
				}
				else
				{
					this.indexPart += 1;
					this.region.Insert(this.indexPart, new List<Point3D>());
				}

				this.UpdateRegion();
				this.UpdatePoints();
				this.Invalidate();
			}
		}

		private void MainForm_MouseDown(object sender, MouseEventArgs e)
		{
			bool isInvalidate = false;

			if (e.Button == MouseButtons.Left)
			{
				if (this.indexPoint < 0)
				{
					isInvalidate = this.AddPoint(e.X, e.Y);
				}
			}
			if (e.Button == MouseButtons.Right)
			{
				isInvalidate = this.DelPoint();
			}

			if (isInvalidate)
			{
				this.UpdateCurrentLinePointIndex();
				this.UpdateRegion();
				this.UpdatePoints();
				this.Invalidate();
			}
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e)
		{
			this.mousePoint = e.Location;

			if (this.indexPart < 0)
			{
				return;
			}

			bool isInvalidate = false;

			if (e.Button == MouseButtons.None)
			{
				isInvalidate = this.UpdateCurrentLinePointIndex();
			}

			if (e.Button == MouseButtons.Left && this.indexPoint >= 0)
			{
				List<Point3D> regionPart = this.region[this.indexPart];
				regionPart[this.indexPoint] = new Point3D(e.X, e.Y, 0.0);
				this.updateAfterMouseMove = true;
				isInvalidate = true;
			}

			if (isInvalidate)
			{
				this.Invalidate();
			}
		}

		private void MainForm_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.updateAfterMouseMove)
			{
				this.updateAfterMouseMove = false;
				this.UpdateRegion();
				this.UpdatePoints();
				this.Invalidate();
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.bitmap.Dispose();
		}
	}
}

