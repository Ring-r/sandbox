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

		private const int defaultPointCount = 100;
		private const int defaultRegionCount = 10;

		private readonly List<List<Point3D>> region = new List<List<Point3D>>();
		private IRegionFilter regionFilter = new RegionFilter();

		private long updateRegionTime = 0L;
		private long updatePointsTime = 0L;

		private Bitmap bitmap = new Bitmap(1, 1);
		private readonly Color pointInRegionColor = Color.Green;
		private readonly Color pointOutRegionColor = Color.FromArgb(0);

		private bool isDrawContour = true;
		private readonly Color contourColor = Color.Red;
		private readonly int contourPartAlpha = 100;
		private readonly float contourPartWidth = 1.0f;
		private readonly int currentContourPartAlpha = 255;
		private readonly float currentContourPartWidth = 2.0f;
		private readonly float pointSize = 20.0f;

		private bool isDrawStrings = true;
		private readonly Brush stringsBrush = Brushes.Yellow;

		private int indexPart = -1;
		private int indexLine = -1;
		private int indexPoint = -1;

		private Point mousePoint = new Point();
		private bool updateOnMouseUp = false;

		#endregion Private fields.

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

		private void PreCalculate()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			this.regionFilter.Update(this.region);
			stopwatch.Stop();
			this.updateRegionTime = stopwatch.ElapsedMilliseconds;
		}

		private void Calculate()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int[] array = new int[this.ClientSize.Width * this.ClientSize.Height];
			int index = 0;
			for (int y = 0; y < this.ClientSize.Height; ++y)
			{
				for (int x = 0; x < this.ClientSize.Width; ++x)
				{
					array[index] = (this.regionFilter.Contains(x, y) ? pointInRegionColor : pointOutRegionColor).ToArgb();
					++index;
				}
			}
			stopwatch.Stop();
			this.updatePointsTime = stopwatch.ElapsedMilliseconds;

			BitmapData imageData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			Marshal.Copy(array, 0, imageData.Scan0, array.Length);
			bitmap.UnlockBits(imageData);
		}

		private void DrawContour(Graphics graphics)
		{
			for (int j = 0; j < this.region.Count; ++j)
			{
				Pen pen = new Pen(Color.FromArgb(j == this.indexPart ? this.currentContourPartAlpha : this.contourPartAlpha, this.contourColor));

				List<Point3D> regionPart = this.region[j];
				for (int i = 0; i < regionPart.Count; ++i)
				{
					pen.Width = j == this.indexPart && i == this.indexPoint ? this.currentContourPartWidth : this.contourPartWidth;
					Point3D regionPoint = regionPart[i];
					graphics.DrawRectangle(pen, (float)(regionPoint.X) - this.pointSize / 2, (float)(regionPoint.Y) - this.pointSize / 2, this.pointSize, this.pointSize);

					pen.Width = j == this.indexPart && i == this.indexLine ? this.currentContourPartWidth : this.contourPartWidth;
					Point3D regionPointSource = regionPart[i];
					Point3D regionPointTarget = regionPart[i != regionPart.Count - 1 ? i + 1 : 0];
					graphics.DrawLine(pen, (float)(regionPointSource.X), (float)(regionPointSource.Y), (float)(regionPointTarget.X), (float)(regionPointTarget.Y));
				}
			}
		}

		private void DrawStrings(Graphics graphics)
		{
			float textY = 0;
			graphics.DrawString("F1 - help", this.Font, stringsBrush, 0.0f, textY);
			if (this.isDrawStrings)
			{
				float textHeight = graphics.MeasureString(" ", this.Font).Height;

				textY += textHeight; graphics.DrawString("F2 - show contour", this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight; graphics.DrawString("F5 - clean", this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight; graphics.DrawString("F6 - create random points", this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight;
				textY += textHeight; graphics.DrawString("To add point (points count in current part less then 3) click mouse left button in any place.", this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight; graphics.DrawString("To add point (points count in current part more then 3) click mouse left button on edge.", this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight; graphics.DrawString("To move point click mouse left button on point, hold and move.", this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight; graphics.DrawString("To delete point click mouse right button on point.", this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight; graphics.DrawString("To delete part delete all points in part.", this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight;
				textY += textHeight; graphics.DrawString(string.Format("Count of region points: {0}", this.region.Sum(part => part.Count)), this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight; graphics.DrawString(string.Format("PreCalculate time: {0} milliseconds", this.updateRegionTime), this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight;
				textY += textHeight; graphics.DrawString(string.Format("Count of verificated points: {0}", this.ClientSize.Width * this.ClientSize.Height), this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight; graphics.DrawString(string.Format("Calculation time: {0} milliseconds", this.updatePointsTime), this.Font, stringsBrush, 0.0f, textY);
				textY += textHeight;
				textY += textHeight; graphics.DrawString("Exc - close", this.Font, stringsBrush, 0.0f, textY);
			}
		}

		#region Find current point and line indexes.

		private bool UpdateCurrentIndexes()
		{
			Tuple<int, int> currentPointIndex = this.FindCurrentPointIndex();
			if (currentPointIndex.Item2 >= 0)
			{
				if (this.indexPart != currentPointIndex.Item1 || this.indexPoint != currentPointIndex.Item2)
				{
					this.indexPart = currentPointIndex.Item1;
					this.indexPoint = currentPointIndex.Item2;
					this.indexLine = -1;
					return true;
				}
				return false;
			}

			Tuple<int, int> currentLineIndex = this.FindCurrentLineIndex();
			if (currentLineIndex.Item2 >= 0)
			{
				if (this.indexPart != currentLineIndex.Item1 || this.indexLine != currentLineIndex.Item2)
				{
					this.indexPart = currentLineIndex.Item1;
					this.indexLine = currentLineIndex.Item2;
					this.indexPoint = -1;
					return true;
				}
				return false;
			}

			if (this.indexPoint != -1 || this.indexLine != -1)
			{
				this.indexPoint = -1;
				this.indexLine = -1;
			}

			return true;
		}

		private Tuple<int, int> FindCurrentPointIndex()
		{
			int indexPart = -1;
			int indexPoint = -1;

			if (0 < this.indexPart && this.indexPart < this.region.Count)
			{
				int i = this.indexPart;
				indexPoint = this.FindCurrentPointIndex(i);
				if (indexPoint >= 0)
				{
					indexPart = i;
				}
			}

			for (int i = 0; i < this.region.Count && indexPoint < 0; ++i)
			{
				indexPoint = this.FindCurrentPointIndex(i);
				if (indexPoint >= 0)
				{
					indexPart = i;
				}
			}

			return Tuple.Create(indexPart, indexPoint);
		}

		private int FindCurrentPointIndex(int indexPart)
		{
			int indexPoint = -1;
			List<Point3D> regionPart = this.region[indexPart];
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

		private Tuple<int, int> FindCurrentLineIndex()
		{
			int indexPart = -1;
			int indexLine = -1;

			if (0 < this.indexPart && this.indexPart < this.region.Count)
			{
				int i = this.indexPart;
				indexLine = this.FindCurrentLineIndex(i);
				if (indexLine >= 0)
				{
					indexPart = i;
				}
			}

			for (int i = 0; i < this.region.Count && indexLine < 0; ++i)
			{
				indexLine = this.FindCurrentLineIndex(i);
				if (indexLine >= 0)
				{
					indexPart = i;
				}
			}

			return Tuple.Create(indexPart, indexLine);
		}

		private int FindCurrentLineIndex(int indexPart)
		{
			int indexLine = -1;
			List<Point3D> regionPart = this.region[indexPart];
			for (int i = 0; i < regionPart.Count; ++i)
			{
				Point3D regionPointSource = regionPart[i];
				Point3D regionPointTarget = regionPart[i != regionPart.Count - 1 ? i + 1 : 0];
				if (
					Math.Abs(this.mousePoint.X - (regionPointSource.X + regionPointTarget.X) / 2) < Math.Abs(regionPointSource.X - regionPointTarget.X) / 2 &&
					Math.Abs(this.mousePoint.Y - (regionPointSource.Y + regionPointTarget.Y) / 2) < Math.Abs(regionPointSource.Y - regionPointTarget.Y) / 2)
				{
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
			}
			return indexLine;
		}

		#endregion Find current point and line indexes.

		private bool AddPoint(double x, double y)
		{
			if (this.indexPart < 0)
			{
				List<Point3D> regionPart = new List<Point3D>();
				regionPart.Add(new Point3D(x, y, 0.0));
				this.region.Add(regionPart);
				this.indexPart = 0;
			}
			else
			{
				List<Point3D> regionPart = this.region[this.indexPart];
				if (regionPart.Count < 3)
				{
					regionPart.Add(new Point3D(x, y, 0.0));
				}
				else
				{
					if (this.indexLine >= 0)
					{
						regionPart.Insert(this.indexLine + 1, new Point3D(x, y, 0.0));
					}
					else
					{
						regionPart = this.region[this.region.Count - 1];
						if (regionPart.Count >= 3)
						{
							regionPart = new List<Point3D>();
						}
						regionPart.Add(new Point3D(x, y, 0.0));
						this.region.Add(regionPart);
						this.indexPart = this.region.Count - 1;
					}
				}
			}
			return true;
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
			this.Calculate();
			this.Invalidate();
		}

		private void MainForm_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(this.BackColor);

			e.Graphics.DrawImageUnscaled(this.bitmap, 0, 0);

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			if (this.isDrawContour)
			{
				this.DrawContour(e.Graphics);
			}

			if (this.isDrawStrings)
			{
				this.DrawStrings(e.Graphics);
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
				this.isDrawStrings = !this.isDrawStrings;

				this.Invalidate();
			}
			if (e.KeyData == Keys.F2)
			{
				this.isDrawContour = !this.isDrawContour;

				this.Invalidate();
			}
			if (e.KeyData == Keys.F5)
			{
				this.region.Clear();
				this.PreCalculate();
				this.Calculate();

				this.indexPart = -1;
				this.UpdateCurrentIndexes();

				this.Invalidate();
			}
			if (e.KeyData == Keys.F6)
			{
				this.region.Clear();
				this.CreateRandomPoints();
				this.PreCalculate();
				this.Calculate();

				this.Invalidate();
			}
		}

		private void MainForm_MouseDown(object sender, MouseEventArgs e)
		{
			bool isInvalidate = false;

			if (e.Button == MouseButtons.Left && this.indexPoint < 0)
			{
				isInvalidate = this.AddPoint(e.X, e.Y);
			}
			if (e.Button == MouseButtons.Right && this.indexPoint >= 0)
			{
				if (this.DelPoint())
				{
					if (this.region[this.indexPart].Count == 0)
					{
						this.region.RemoveAt(this.indexPart);
					}
					isInvalidate = true;
				}
			}

			if (isInvalidate)
			{
				this.UpdateCurrentIndexes();
				this.PreCalculate();
				this.Calculate();
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
				isInvalidate = this.UpdateCurrentIndexes();
			}

			if (e.Button == MouseButtons.Left && this.indexPoint >= 0)
			{
				List<Point3D> regionPart = this.region[this.indexPart];
				regionPart[this.indexPoint] = new Point3D(e.X, e.Y, 0.0);
				this.updateOnMouseUp = true;
				isInvalidate = true;
			}

			if (isInvalidate)
			{
				this.Invalidate();
			}
		}

		private void MainForm_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.updateOnMouseUp)
			{
				this.updateOnMouseUp = false;
				this.PreCalculate();
				this.Calculate();
				this.Invalidate();
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.bitmap.Dispose();
		}
	}
}

