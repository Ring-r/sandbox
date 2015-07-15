using System;
using System.Drawing;

namespace Walk {
	public class Map {
		private const int zMax = 255;

		public struct Cell {
			public int type;
			public float x;
			public float y;
			public int z;

			public float gravity;
			public float resistance;
		}

		public Cell[,] cells = new Cell[0, 0];
		public float cellSize = 0.0f;
		public float accelarationCell = 0.0f;

		public float xMin {
			get {
				return 0;
			}
		}

		public float xMax {
			get {
				return this.cells.GetLength(0) * this.cellSize;
			}
		}

		public float yMin {
			get {
				return 0;
			}
		}

		public float yMax {
			get {
				return this.cells.GetLength(1) * this.cellSize;
			}
		}

		private Tuple<int, int> GetIndex(float x, float y) {
			return Tuple.Create((int)Math.Floor(x / this.cellSize), (int)Math.Floor(y / this.cellSize));
		}

		private void FillHeights() {
			for (int i = 0; i < this.cells.GetLength(0); ++i) {
				for (int j = 0; j < this.cells.GetLength(1); ++j) {
					this.cells[i, j].z = Program.rand.Next(zMax);
				}
			}
		}

		private void FillGravity(float gravity) {
			for (int i = 0; i < this.cells.GetLength(0); ++i) {
				for (int j = 0; j < this.cells.GetLength(1); ++j) {
					this.cells[i, j].gravity = gravity;
				}
			}
		}

		private void FillResistance(float friction) {
			for (int i = 0; i < this.cells.GetLength(0); ++i) {
				for (int j = 0; j < this.cells.GetLength(1); ++j) {
					this.cells[i, j].resistance = friction;
				}
			}
		}

		public void CreateLand(int iCount, int jCount, float gravity, float resistance) {
			this.cells = new Cell[iCount, jCount];
			// this.FillHeights (rand);
			this.FillGravity(gravity);
			this.FillResistance(resistance);
		}

		public void FillCheckPoints(int count) {
			if (this.cells.GetLength(0) * this.cells.GetLength(1) < count) {
				return; // throw?
			}

			for (int i = 0; i < this.cells.GetLength(0); ++i) {
				for (int j = 0; j < this.cells.GetLength(1); ++j) {
					if (this.cells[i, j].type == 1) {
						this.cells[i, j].type = 0;
					}
				}
			}

			for (int k = 0; k < count; ++k) {
				int i;
				int j;
				do {
					i = Program.rand.Next(this.cells.GetLength(0));
					j = Program.rand.Next(this.cells.GetLength(1));
				} while (this.cells[i, j].type != 0);
				this.cells[i, j].type = 1;
			}
		}

		public Tuple<float, float> GetVector(float x, float y) {
			var index = this.GetIndex(x, y);
			Cell cell = this.cells[index.Item1, index.Item2];
			return Tuple.Create(cell.x * cell.gravity, cell.y * cell.gravity);
		}

		public float GetResistance(float x, float y) {
			var index = this.GetIndex(x, y);
			return this.cells[index.Item1, index.Item2].resistance;
		}

		public Pen pen = Pens.Yellow;

		private void DrawCells(Graphics g) {
			for (int i = 0; i < this.cells.GetLength(0); ++i) {
				for (int j = 0; j < this.cells.GetLength(1); ++j) {
					int colorKoef = this.cells[i, j].z;
					Brush brush = new SolidBrush(Color.FromArgb(colorKoef, colorKoef, colorKoef));
					float x = i * this.cellSize;
					float y = j * this.cellSize;
					g.FillRectangle(brush, x - 1, y - 1, this.cellSize + 2, this.cellSize + 2);
					Pen pen = Pens.Silver;
					g.DrawRectangle(pen, x, y, this.cellSize, this.cellSize);
				}
			}
		}

		private void DrawTypes(Graphics g) {
			for (int i = 0; i < this.cells.GetLength(0); ++i) {
				for (int j = 0; j < this.cells.GetLength(1); ++j) {
					if (this.cells[i, j].type == 1) {
						float x = i * this.cellSize;
						float y = j * this.cellSize;
						g.DrawRectangle(Pens.White, x, y, this.cellSize, this.cellSize);
					}
				}
			}
		}

		public void Draw(Graphics g) {
			this.DrawCells(g);
			this.DrawTypes(g);
		}

		public void DrawBorder(Graphics g) {
			g.DrawRectangle(pen, this.xMin, this.yMin, this.xMax - this.xMin, this.yMax - this.yMin);
		}

		public void DrawVectors(Graphics g) {
			for (int i = 0; i < this.cells.GetLength(0); ++i) {
				for (int j = 0; j < this.cells.GetLength(1); ++j) {
					float x = i * this.cellSize + this.cellSize / 2;
					float y = j * this.cellSize + this.cellSize / 2;

					const float pointSize = 2.0f;
					g.DrawEllipse(this.pen, x - pointSize, y - pointSize, pointSize, pointSize);
					g.DrawLine(this.pen, x, y, x + this.cells[i, j].x * this.cellSize / 2, y + this.cells[i, j].y * this.cellSize / 2);
				}
			}
		}
	}
}

