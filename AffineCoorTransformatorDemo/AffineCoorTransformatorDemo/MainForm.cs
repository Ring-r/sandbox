using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace AffineCoorTransformatorDemo {
	public partial class MainForm : Form {
		private Random rand = new Random();

		private const double eps = 4;
		private const int pointsCount = 30;
		private const int pointsCountForFill = 10;
		private Point3D[] sourcePoints = new Point3D[pointsCount];
		private Point3D[] targetPoints = new Point3D[pointsCount];
		private Point3D[] targetPointsRes = new Point3D[pointsCount];

		public MainForm() {
			InitializeComponent();
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(Color.White);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			double[] min = new double[] { double.PositiveInfinity, double.PositiveInfinity };
			double[] size = new double[] { double.NegativeInfinity, double.NegativeInfinity };
			for (int i = 0; i < pointsCount; ++i) {
				min[0] = Math.Min(min[0], this.sourcePoints[i].X);
				min[0] = Math.Min(min[0], this.targetPoints[i].X);
				min[0] = Math.Min(min[0], this.targetPointsRes[i].X);

				min[1] = Math.Min(min[1], this.sourcePoints[i].Y);
				min[1] = Math.Min(min[1], this.targetPoints[i].Y);
				min[1] = Math.Min(min[1], this.targetPointsRes[i].Y);

				size[0] = Math.Max(size[0], this.sourcePoints[i].X);
				size[0] = Math.Max(size[0], this.targetPoints[i].X);
				size[0] = Math.Max(size[0], this.targetPointsRes[i].X);

				size[1] = Math.Max(size[1], this.sourcePoints[i].Y);
				size[1] = Math.Max(size[1], this.targetPoints[i].Y);
				size[1] = Math.Max(size[1], this.targetPointsRes[i].Y);
			}
			for (int j = 0; j < min.Length; ++j) {
				size[j] -= min[j];
			}
			if (size[0] > 0 && size[1] > 0) {
				e.Graphics.ScaleTransform(this.ClientSize.Width / (1.1f * (float)(size[0])), this.ClientSize.Height / (1.1f * (float)(size[1])));
				e.Graphics.TranslateTransform(-(float)(min[0] - 0.05 * size[0]), -(float)(min[1] - 0.05 * size[1]));
			}

			for (int i = 0; i < pointsCount; ++i) {
				e.Graphics.DrawLine(Pens.Yellow, (float)this.sourcePoints[i].X, (float)this.sourcePoints[i].Y, (float)this.targetPoints[i].X, (float)this.targetPoints[i].Y);
				e.Graphics.DrawLine(Pens.Silver, (float)this.sourcePoints[i].X, (float)this.sourcePoints[i].Y, (float)this.targetPointsRes[i].X, (float)this.targetPointsRes[i].Y);
				e.Graphics.FillRectangle(Brushes.Blue, new RectangleF((float)this.sourcePoints[i].X, (float)this.sourcePoints[i].Y, 5.0f, 5.0f));
				e.Graphics.FillRectangle(Brushes.Red, new RectangleF((float)this.targetPoints[i].X, (float)this.targetPoints[i].Y, 5.0f, 5.0f));
				e.Graphics.FillRectangle(Brushes.Green, new RectangleF((float)this.targetPointsRes[i].X, (float)this.targetPointsRes[i].Y, 5.0f, 5.0f));
			}
		}

		private void MainForm_Click(object sender, EventArgs e) {
			Console.Clear();

			#region Create source data.

			for (int i = 0; i < pointsCountForFill; ++i) {
				this.sourcePoints[i] = new Point3D(
					this.rand.Next(Math.Max(this.ClientSize.Width, this.ClientSize.Height)),
					this.rand.Next(Math.Max(this.ClientSize.Width, this.ClientSize.Height)),
					this.rand.Next(Math.Max(this.ClientSize.Width, this.ClientSize.Height)));
			}
			for (int i = pointsCountForFill; i < pointsCount; ++i) {
				this.sourcePoints[i] = new Point3D(
					this.rand.Next(Math.Max(this.ClientSize.Width, this.ClientSize.Height)),
					this.rand.Next(Math.Max(this.ClientSize.Width, this.ClientSize.Height)),
					this.rand.Next(Math.Max(this.ClientSize.Width, this.ClientSize.Height)));
			}

			#endregion Create source data.

			#region Create target data.

			Matrix3D matrix3D = new Matrix3D();
			matrix3D.Translate(new Vector3D(10, 50, 20));
			matrix3D.RotateAt(new Quaternion(new Vector3D(2, 9, 1), 20), new Point3D(30, 30, 70));
			matrix3D.Translate(new Vector3D(10, 50, 20));
			matrix3D.RotateAt(new Quaternion(new Vector3D(1, 3, 5), -5), new Point3D(10, 0, 10));

			Console.WriteLine("{0,30}{1,30}{2,30}{3,30}", matrix3D.M11, matrix3D.M21, matrix3D.M31, matrix3D.OffsetX);
			Console.WriteLine("{0,30}{1,30}{2,30}{3,30}", matrix3D.M12, matrix3D.M22, matrix3D.M32, matrix3D.OffsetY);
			Console.WriteLine("{0,30}{1,30}{2,30}{3,30}", matrix3D.M13, matrix3D.M23, matrix3D.M33, matrix3D.OffsetZ);

			for (int i = 0; i < pointsCount; ++i) {
				this.targetPoints[i] = matrix3D.Transform(this.sourcePoints[i]);
			}

			Point3D[] sourcePointsToFill = new Point3D[pointsCountForFill];
			Point3D[] targetPointsToFill = new Point3D[pointsCountForFill];
			for (int i = 0; i < pointsCountForFill; ++i) {
				sourcePointsToFill[i] = this.sourcePoints[i];
				targetPointsToFill[i] = this.targetPoints[i];
			}
			AffineCoorTransformator ct = new AffineCoorTransformator();
			ct.Fill(sourcePointsToFill, targetPointsToFill);

			double[] matrix = ct.MatrixCopy;
			int index = 0;
			for (int i = 0; i < AffineCoorTransformator.dim; ++i) {
				for (int j = 0; j < AffineCoorTransformator.dimExt; ++j) {
					Console.Write("{0,30}", matrix[index]);
					++index;
				}
				Console.WriteLine();
			}
			Console.WriteLine();

			#endregion Create target data.

			double[] error = new double[pointsCount];
			double totalError = 0;
			for (int i = 0; i < pointsCount; ++i) {
				this.targetPointsRes[i] = ct.Transform(this.sourcePoints[i]);

				error[i] = (this.targetPointsRes[i] - this.targetPoints[i]).Length;
				Console.WriteLine("Error {0}: {1}.", i, error[i]);
				totalError += error[i];
			}
			double totalErrorMax = pointsCount * eps * Math.Sqrt(AffineCoorTransformator.dim);
			Console.WriteLine();
			Console.WriteLine("Max: {0}. Current: {1}. Middle: {2}", totalErrorMax, totalError, totalError / pointsCount);
			Console.WriteLine();

			this.Invalidate();
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e) {
			switch (e.KeyData) {
				case Keys.Escape:
					this.Close();
					break;
			}
		}
	}
}
