using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WalkDiscrete {
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
		private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

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
			this.Text = "WalkDiscrete Game Prototype";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.KeyUp += this.MainForm_KeyUp;
			this.MouseDown += this.MainForm_MouseDown;
			this.MouseMove += this.MainForm_MouseMove;
			this.MouseUp += this.MainForm_MouseUp;
			this.Resize += this.MainForm_Resize;
			this.Paint += this.MainForm_Paint;
			this.timer.Tick += this.Timer_Tick;
			this.ResumeLayout(false);

			this.CreateLevel();
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				this.Close();
			}
			if (e.KeyCode == Keys.Enter) {
				this.CreateLevel();
				this.Invalidate();
			}
		}

		private void MainForm_MouseDown(object sender, MouseEventArgs e) {
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e) {
		}

		private void MainForm_MouseUp(object sender, MouseEventArgs e) {
			float px = e.X - (this.ClientSize.Width - this.cells.GetLength(0) * cellSize) / 2;
			float py = e.Y - (this.ClientSize.Height - this.cells.GetLength(1) * cellSize) / 2;
			int i = (int)(Math.Floor(px / cellSize));
			int j = (int)(Math.Floor(py / cellSize));
			if (this.path.Count == 0) {
				if (i == 0 || i == this.cells.GetLength(0) - 1 || j == 0 || j == this.cells.GetLength(1) - 1) {
					this.path.Add(Tuple.Create(i, j));
					this.res += this.cells[i, j, 1];
					this.Invalidate();
				}
			} else {
				if (0 <= i && i < this.cells.GetLength(0) && 0 <= j && j < this.cells.GetLength(1)) {
					int iLast = this.path[this.path.Count - 1].Item1;
					int jLast = this.path[this.path.Count - 1].Item2;
					if (Math.Abs(i - iLast) <= 1 && Math.Abs(j - jLast) <= 1) {
						this.path.Add(Tuple.Create(i, j));
						this.res += this.cells[i, j, 1];
						this.Invalidate();
					}
				}
			}
		}

		private void MainForm_Resize(object sender, EventArgs e) {
		}

		private const int maxNumber = 10;
		private const int cellCount = 21;
		private const float cellSize = 30;
		private const int indexSize = 3;

		private int[,,] cells = new int[cellCount, cellCount, 2];
		private readonly List<Tuple<int, int>> path = new List<Tuple<int, int>>();
		private int res = 0;

		private void CreateLevel() {
			for (int i = 0; i < this.cells.GetLength(0); ++i) {
				for (int j = 0; j < this.cells.GetLength(1); ++j) {
					this.cells[i, j, 1] = this.cells[i, j, 0] = Program.rand.Next(1, maxNumber);
				}
			}
			for (int k = 0; k < 10; ++k) {
				//int ii = this.cells.GetLength (0) / 2 + 1;
				//int jj = this.cells.GetLength (1) / 2 + 1;
				int ii = Program.rand.Next(this.cells.GetLength(0) - 1);
				int jj = Program.rand.Next(this.cells.GetLength(1) - 1);
				this.cells[ii, jj, 1] = this.cells[ii, jj, 0] = 0;
			}
			this.path.Clear();
			this.res = 0;
		}

		private void DrawCells(PaintEventArgs e, int i0, int i1, int j0, int j1) {
			for (int i = i0; i <= i1; ++i) {
				for (int j = j0; j <= j1; ++j) {
					if (0 <= i && i < this.cells.GetLength(0) && 0 <= j && j < this.cells.GetLength(1)) {
						float x = i * cellSize;
						float y = j * cellSize;
						Brush brush = Brushes.Yellow;
						if (this.cells[i, j, 0] > 0) {
							int colorKoef = 255 * this.cells[i, j, 0] / maxNumber;
							brush = new SolidBrush(Color.FromArgb(colorKoef, 255, colorKoef));
						}
						e.Graphics.FillRectangle(brush, x, y, cellSize, cellSize);
						//string str = this.cells [i, j, 1].ToString ();
						//SizeF size = e.Graphics.MeasureString (str, this.Font);
						//e.Graphics.DrawString (str, this.Font, Brushes.Black, x + (cellSize - size.Width) / 2, y + (cellSize - size.Height) / 2);
					}
				}
			}
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(Color.Black);

			float px = (this.ClientSize.Width - this.cells.GetLength(0) * cellSize) / 2;
			float py = (this.ClientSize.Height - this.cells.GetLength(1) * cellSize) / 2;
			e.Graphics.TranslateTransform(px, py);

			this.DrawCells(e, 0, this.cells.GetLength(0) - 1, 0, this.cells.GetLength(1) - 1);

//			foreach (var index in this.path) {
//				this.DrawCells (e, index.Item1 - indexSize, index.Item1 + indexSize, index.Item2 - indexSize, index.Item2 + indexSize);
//			}
			foreach (var index in this.path) {
				float x = index.Item1 * cellSize;
				float y = index.Item2 * cellSize;
				e.Graphics.DrawRectangle(Pens.Black, x, y, cellSize, cellSize);
			}
			if (this.path.Count > 0) {
				var index = this.path[this.path.Count - 1];
				float x = index.Item1 * cellSize;
				float y = index.Item2 * cellSize;
				e.Graphics.DrawRectangle(Pens.Red, x, y, cellSize, cellSize);
			}

			e.Graphics.DrawRectangle(Pens.White, 0, 0, this.cells.GetLength(0) * cellSize, this.cells.GetLength(1) * cellSize);

			e.Graphics.ResetTransform();
			e.Graphics.DrawString(string.Format("You've spend a {0} scores.", this.res), this.Font, Brushes.White, 0, 0); 
		}

		private void Timer_Tick(object sender, EventArgs e) {
		}
	}
}
