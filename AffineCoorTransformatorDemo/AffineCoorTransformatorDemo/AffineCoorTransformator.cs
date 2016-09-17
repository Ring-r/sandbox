using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Media.Media3D;

namespace AffineCoorTransformatorDemo {
	/// <summary>
	/// Affine coordinate transformator.
	/// </summary>
	[DataContract]
	public class AffineCoorTransformator {
		#region Private fields.

		public const int dim = 3;
		public const int dimExt = dim + 1;
		private double[,] matrix = new double[dim, dimExt];
		private const double sqrEps = 0.04; // 20 cm

		#endregion Private fields.

		#region Private methods.

		private double[,] SolveLinearEquationsSystem(double[,] les) {
			int rowCount = les.GetLength(0); // dimExt
			int columnCount = les.GetLength(1); // dimExt + dim

			int[] checkedRows = new int[rowCount];
			int[] checkedColumns = new int[rowCount];
			for (int i = 0; i < rowCount; ++i) {
				checkedRows[i] = -1;
				checkedColumns[i] = -1;
			}

			int maxRowIndex = 0;
			int maxColumnIndex = 0;
			double maxAbs = 0;
			do {
				maxRowIndex = 0;
				maxColumnIndex = 0;
				maxAbs = 0;
				for (int i = 0; i < rowCount; ++i) {
					if (checkedRows[i] < 0) {
						for (int j = 0; j < rowCount; ++j) {
							if (checkedColumns[j] < 0) {
								if (maxAbs < Math.Abs(les[i, j])) {
									maxRowIndex = i;
									maxColumnIndex = j;
									maxAbs = Math.Abs(les[maxRowIndex, maxColumnIndex]);
								}
							}
						}
					}
				}

				if (maxAbs > 0) {
					double main = les[maxRowIndex, maxColumnIndex];
					for (int j = 0; j < columnCount; ++j) {
						les[maxRowIndex, j] /= main;
					}

					for (int i = 0; i < rowCount; ++i) {
						if (i != maxRowIndex) {
							double underMain = les[i, maxColumnIndex];
							for (int j = 0; j < columnCount; ++j) {
								les[i, j] -= les[maxRowIndex, j] * underMain;
							}
						}
					}

					checkedRows[maxRowIndex] = maxColumnIndex;
					checkedColumns[maxColumnIndex] = maxRowIndex;
				}
			} while (maxAbs > 0);

			double[,] res = new double[rowCount, rowCount - 1];
			for (int i = 0; i < rowCount; ++i) {
				if (checkedRows[i] >= 0) {
					for (int j = 0; j < rowCount - 1; ++j) {
						res[checkedRows[i], j] = les[i, rowCount + j];
					}
				}
			}
			return res;
		}

		private void Fill(double[,] sourcePoints, double[,] targetPoints) // [i,j], where i - number of point, j - number of coordinate (0 - x, 1 - y, 2 - z, 3 - const = 1).
		{
			double[,] aExt = new double[dimExt, dimExt + dim]; // [j, k], where j - number of rows (number of coefficients), k - number of column (number of coefficients + constant terms)
			int pointsCount = sourcePoints.GetLength(0);
			for (int rowIndex = 0; rowIndex < dimExt; ++rowIndex) {
				for (int columnIndex = 0; columnIndex < dim; ++columnIndex) {
					for (int i = 0; i < pointsCount; ++i) {
						aExt[rowIndex, columnIndex] += sourcePoints[i, rowIndex] * sourcePoints[i, columnIndex];
						aExt[rowIndex, dimExt + columnIndex] += sourcePoints[i, rowIndex] * targetPoints[i, columnIndex];
					}
				}
			}
			for (int rowIndex = 0; rowIndex < dimExt; ++rowIndex) {
				aExt[rowIndex, dim] = aExt[dim, rowIndex];
			}
			aExt[dim, dim] = pointsCount;

			double[,] coef = this.SolveLinearEquationsSystem(aExt);
			for (int rowIndex = 0; rowIndex < dim; ++rowIndex) {
				for (int columnIndex = 0; columnIndex < dimExt; ++columnIndex) {
					this.matrix[rowIndex, columnIndex] = coef[columnIndex, rowIndex];
				}
			}
		}

		#endregion Private methods.

		#region Public properties.

		/// <summary>
		/// Get affine transformation matrix.
		/// </summary>
		[DataMember]
		public double[] MatrixCopy {
			get {
				double[] res = new double[dim * dimExt];
				int index = 0;
				for (int i = 0; i < dim; ++i) {
					for (int j = 0; j < dimExt; ++j) {
						res[index] = this.matrix[i, j];
						++index;
					}
				}
				return res;
			}
			set {
				if (value.Length == dim * dimExt) {
					int index = 0;
					for (int i = 0; i < dim; ++i) {
						for (int j = 0; j < dimExt; ++j) {
							this.matrix[i, j] = value[index];
							++index;
						}
					}
				}
				// TODO: (R) Exceptions?
			}
		}

		#endregion Public properties.

		#region Public methods.

		/// <summary>
		/// Constructor.
		/// </summary>
		public AffineCoorTransformator() {
			this.SetIdentity();
		}

		/// <summary>
		/// Calculate data of transformator by using source and target points.
		/// </summary>
		/// <param name="sourcePoints">Source points.</param>
		/// <param name="targetPoints">Target points.</param>
		public void Fill(Point3D[] sourcePoints, Point3D[] targetPoints) {
			Debug.Assert(sourcePoints.Length == targetPoints.Length);
			int length = Math.Min(sourcePoints.Length, targetPoints.Length);

			if (length == 0) {
				this.SetIdentity();
				return;
			}

			#region Find base.

			int baseIndex = 1;
			Vector3D[] sourceBbase = new Vector3D[dimExt];
			Vector3D[] targetBbase = new Vector3D[dimExt];
			for (int i = 0; i < length; ++i) {
				sourceBbase[0] += (Vector3D)sourcePoints[i];
				targetBbase[0] += (Vector3D)targetPoints[i];
			}
			sourceBbase[0] /= length; targetBbase[0] /= length;

			Vector3D sourceVector = new Vector3D();
			Vector3D targetVector = new Vector3D();
			for (int i = 0; i < length && baseIndex < dimExt; ++i) {
				sourceVector = (Vector3D)sourcePoints[i] - sourceBbase[0];
				targetVector = (Vector3D)targetPoints[i] - targetBbase[0];
				for (int j = 1; j < baseIndex; ++j) {
					sourceVector -= Vector3D.DotProduct(sourceVector, sourceBbase[j]) * sourceBbase[j];
					targetVector -= Vector3D.DotProduct(targetVector, targetBbase[j]) * targetBbase[j];
				}
				if (sourceVector.LengthSquared > sqrEps && targetVector.LengthSquared > sqrEps) {
					sourceVector.Normalize();
					targetVector.Normalize();
					sourceBbase[baseIndex] = sourceVector;
					targetBbase[baseIndex] = targetVector;
					++baseIndex;
				}
			}

			#endregion Find base.

			if (baseIndex == 1) {
				this.SetIdentity();
				this.matrix[0, dim] = targetBbase[0].X - sourceBbase[0].X;
				this.matrix[1, dim] = targetBbase[0].Y - sourceBbase[0].Y;
				this.matrix[2, dim] = targetBbase[0].Z - sourceBbase[0].Z;
				return;
			}

			if (baseIndex == 2) {
				// TODO: (R) Bad code. :-(
				Matrix3D matrix3D = new Matrix3D();
				matrix3D.Translate(targetBbase[0] - sourceBbase[0]);
				matrix3D.Rotate(new Quaternion(Vector3D.CrossProduct(sourceBbase[1], targetBbase[1]), Vector3D.AngleBetween(sourceBbase[1], targetBbase[1])));
				this.MatrixCopy = new double[dim * dimExt]
				{
					matrix3D.M11, matrix3D.M21, matrix3D.M31, matrix3D.OffsetX,
					matrix3D.M12, matrix3D.M22, matrix3D.M32, matrix3D.OffsetY,
					matrix3D.M13, matrix3D.M23, matrix3D.M33, matrix3D.OffsetZ
				};
				return;
			}

			if (baseIndex == 3) {
				sourceBbase[3] = Vector3D.CrossProduct(sourceBbase[1], sourceBbase[2]);
				targetBbase[3] = Vector3D.CrossProduct(targetBbase[1], targetBbase[2]);
			}

			int fictiveLength = baseIndex == 3 ? 1 : 0;
			double[,] sourcePointsCopy = new double[length + fictiveLength, dimExt];
			double[,] targetPointsCopy = new double[length + fictiveLength, dimExt];
			for (int i = 0; i < length; ++i) {
				sourcePointsCopy[i, 0] = sourcePoints[i].X;
				sourcePointsCopy[i, 1] = sourcePoints[i].Y;
				sourcePointsCopy[i, 2] = sourcePoints[i].Z;
				sourcePointsCopy[i, 3] = 1;
				targetPointsCopy[i, 0] = targetPoints[i].X;
				targetPointsCopy[i, 1] = targetPoints[i].Y;
				targetPointsCopy[i, 2] = targetPoints[i].Z;
				targetPointsCopy[i, 3] = 1;
			}
			if (fictiveLength > 0) {
				sourcePointsCopy[length, 0] = sourceBbase[0].X + sourceBbase[3].X;
				sourcePointsCopy[length, 1] = sourceBbase[0].Y + sourceBbase[3].Y;
				sourcePointsCopy[length, 2] = sourceBbase[0].Z + sourceBbase[3].Z;
				sourcePointsCopy[length, 3] = 1;
				targetPointsCopy[length, 0] = targetBbase[0].X + targetBbase[3].X;
				targetPointsCopy[length, 1] = targetBbase[0].Y + targetBbase[3].Y;
				targetPointsCopy[length, 2] = targetBbase[0].Z + targetBbase[3].Z;
				targetPointsCopy[length, 3] = 1;
			}

			this.Fill(sourcePointsCopy, targetPointsCopy);
		}

		/// <summary>
		/// Transform point from source coordinate system to target coordinate system.
		/// </summary>
		/// <param name="point">Point in source coordinate system.</param>
		/// <returns>Point in target coordinate system.</returns>
		public Point3D Transform(Point3D point) {
			double[] inpPoint = new double[dimExt] { point.X, point.Y, point.Z, 1 };
			double[] outputPoint = new double[dimExt];
			for (int i = 0; i < dim; ++i) {
				for (int j = 0; j < dimExt; ++j) {
					outputPoint[i] += inpPoint[j] * this.matrix[i, j];
				}
			}
			return new Point3D(outputPoint[0], outputPoint[1], outputPoint[2]);
		}

		/// <summary>
		/// Transform point from source coordinate system to target coordinate system.
		/// </summary>
		/// <param name="point">Point in source coordinate system.</param>
		/// <returns>Point in target coordinate system.</returns>
		public double[] Transform(double[] point) {
			double[] outputPoint = new double[dimExt];
			for (int i = 0; i < dim; ++i) {
				for (int j = 0; j < dimExt; ++j) {
					outputPoint[i] += point[j] * this.matrix[i, j];
				}
			}
			outputPoint[dim] = 1;
			return outputPoint;
		}

		/// <summary>
		/// Fill transformator data by default value.
		/// </summary>
		public void SetIdentity() {
			for (int i = 0; i < dim; ++i) {
				for (int j = 0; j < dimExt; ++j) {
					this.matrix[i, j] = 0;
				}
				this.matrix[i, i] = 1;
			}
		}

		/// <summary>
		/// Create common affine transformation.
		/// </summary>
		/// <param name="first">First affine transformation.</param>
		/// <param name="second">Second affine transformation.</param>
		/// <returns>Common affine transformation.</returns>
		public static AffineCoorTransformator Create(AffineCoorTransformator first, AffineCoorTransformator second) {
			var res = new AffineCoorTransformator();
			for (int i = 0; i < dim; ++i) {
				for (int j = 0; j < dim; ++j) {
					for (int k = 0; k < dim; ++k) {
						res.matrix[i, j] += first.matrix[i, k] * second.matrix[k, j];
					}
				}
				for (int k = 0; k < dim; ++k) {
					res.matrix[i, dim] += first.matrix[i, k] * second.matrix[k, dim];
				}
				res.matrix[i, dim] += first.matrix[i, dim];
			}
			return res;
		}

		#endregion Public methods.
	}
}
