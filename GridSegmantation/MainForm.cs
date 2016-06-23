using Prototypes.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GridSegmentation
{
	public class MainForm : FormWithYieldTimer
	{
		private const int GRID_I_COUNT = 200;
		private const int GRID_J_COUNT = 200;
		private const int GRID_FILLED_COUNT_PERCENTS = 45;
		private const int GRID_MIN_CELLS_COUNT = 100;
		private const bool GRID_MIN_CLEAR = true;

		private const int GRID_VIEW_CELL_SIZE = 5;
		private const int GRID_VIEW_VECTOR_ALPHA = 100;
		private static readonly Color GRID_VIEW_BORDER_COLOR = Color.Yellow;
		private static readonly Color GRID_VIEW_FILLED_COLOR = Color.Silver;

		private readonly Grid grid = new Grid(GRID_I_COUNT, GRID_J_COUNT);
		private readonly List<List<int>> gridVectors = new List<List<int>>();
		private readonly GridView gridView = new GridView(GRID_VIEW_CELL_SIZE, GRID_VIEW_BORDER_COLOR, GRID_VIEW_FILLED_COLOR, GRID_VIEW_VECTOR_ALPHA);

		protected override void Init()
		{
			this.DisposeEnumerator();
			this.grid.InitCells(GRID_FILLED_COUNT_PERCENTS);
			this.enumerator = Utils.Segmentate(this.grid, this.gridVectors, GRID_MIN_CELLS_COUNT, GRID_MIN_CLEAR).GetEnumerator();
		}

		protected override void Form_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(this.BackColor);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			this.gridView.DrawGrid(e.Graphics, this.grid);
			this.gridView.DrawGridVectors(e.Graphics, this.grid, this.gridVectors);
		}
	}
}
