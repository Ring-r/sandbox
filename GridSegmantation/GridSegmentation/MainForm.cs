using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridSegmentation
{
    public partial class MainForm : Form
    {
        private readonly Random random = new Random();

		private readonly Grid grid = new Grid();
		private readonly List<List<int>> gridVectors = new List<List<int>>();

        private int cellSize;
        private int minGridCount;

        private int filedCount;
        private int interval;

        private bool isExit = false;

        private void Init()
        {
			this.grid.Init(100, 100);

            this.cellSize = 5;
            this.minGridCount = 100;

            this.filedCount = this.grid.cellsCount / 2;

            this.interval = 1;
        }

        private void InitCells()
        {
            for (int i = 0; i < this.grid.cellsCount; ++i)
            {
                this.grid.cells[i] = 0;
            }
            for (int i = 0; i < this.filedCount; ++i)
            {
				int index = this.grid.Index(this.random.Next(this.grid.iCount), this.random.Next(this.grid.jCount));
                this.grid.cells[index] = 1;
            }
        }

        private bool Afterstep()
        {
            this.Invalidate();
            Application.DoEvents();
            System.Threading.Thread.Sleep(this.interval);
            return this.isExit;
        }

        public MainForm()
        {
            InitializeComponent();

            this.Init();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.isExit = true;
                this.Close();
            }

            if (e.KeyData == Keys.F5)
            {
				this.InitCells();
				Task.Factory.StartNew(() =>
					{
						Utils.RunAlgorithm(this.grid, gridVectors, this.cellSize, this.minGridCount, this.Afterstep);
					});
                this.Invalidate();
            }
        }

        private static void DrawCells(Graphics g, Grid grid, float cellSize)
        {
            for (int i = 0; i < grid.iCount; ++i)
            {
                for (int j = 0; j < grid.jCount; ++j)
                {
                    int index = grid.Index(i, j);
                    if (grid.cells[index] > 0)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(255 * grid.cells[index] / 10, Color.Black)), i * cellSize, j * cellSize, cellSize, cellSize);
                    }
                }
            }
        }

        private static void DrawGridVector(Graphics g, Grid grid, List<int> gridVector, float cellSize, Color color)
        {
            Brush brush = new SolidBrush(color);
            int count = gridVector.Count;
            for (int i = 0; i < count; i += 2)
            {
                for (int index = gridVector[i]; index <= gridVector[i + 1]; ++index)
                {
                    int index_i, index_j; grid.Index(index, out index_i, out index_j);
                    g.FillRectangle(brush, index_i * cellSize, index_j * cellSize, cellSize, cellSize);
                }
            }
        }

        private static void DrawGridVector_(Graphics g, Grid grid, List<int> gridVector, float cellSize, Color color)
        {
            Brush brush = new SolidBrush(color);
            int count = gridVector.Count;
            for (int i = 0; i < count; ++i)
            {
                int index_i, index_j; grid.Index(gridVector[i], out index_i, out index_j);
                g.FillRectangle(brush, index_i * cellSize, index_j * cellSize, cellSize, cellSize);
            }
        }

        private static void DrawGridVectors(Graphics g, Grid grid, List<List<int>> gridVectors, float cellSize)
        {
            Random random = new Random(0);
            int count = gridVectors.Count;
            for (int i = 0; i < count - 1; ++i)
            {
                List<int> gridVector = gridVectors[i];
                Color color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                DrawGridVector(g, grid, gridVector, cellSize, color);
            }
            if (count > 0)
            {
                DrawGridVector_(g, grid, gridVectors[count - 1], cellSize, Color.Yellow);
            }
        }

        private void MainForm_Paint (object sender, PaintEventArgs e)
		{
			e.Graphics.Clear (Color.White);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			try {
				DrawGridVectors (e.Graphics, this.grid, this.gridVectors, this.cellSize);
				DrawCells (e.Graphics, this.grid, this.cellSize);
			} catch {
			}
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
