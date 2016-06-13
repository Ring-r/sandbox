﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridSegmentation
{
  public class MainForm : Form
  {
    private const int GRID_I_COUNT = 100;
    private const int GRID_J_COUNT = 100;
    private const int GRID_FILLED_COUNT_PERCENTS = 50;
    private const int GRID_MIN_CELLS_COUNT = 100;

    private const int GRID_VIEW_CELL_SIZE = 5;
    private const int GRID_VIEW_VECTOR_ALPHA = 100;
    private static readonly Color GRID_VIEW_BORDER_COLOR = Color.Yellow;
    private static readonly Color GRID_VIEW_FILLED_COLOR = Color.Silver;

    private const int AFTER_STEP_INTERVAL = 1;

    private readonly Grid grid = new Grid(GRID_I_COUNT, GRID_J_COUNT);
    private readonly List<List<int>> gridVectors = new List<List<int>>();
    private readonly GridView gridView = new GridView(GRID_VIEW_CELL_SIZE, GRID_VIEW_BORDER_COLOR, GRID_VIEW_FILLED_COLOR, GRID_VIEW_VECTOR_ALPHA);

    private bool isExit = false;

    public MainForm()
    {
      this.SuspendLayout();

      this.DoubleBuffered = true;
      this.Name = this.Text = "MainForm";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.WindowState = FormWindowState.Maximized;

      this.KeyDown += this.MainForm_KeyDown;
      this.Paint += this.MainForm_Paint;
      this.Resize += this.MainForm_Resize;

      this.ResumeLayout(false);
    }

    private bool Afterstep()
    {
      this.Invalidate();
      Application.DoEvents();
      Thread.Sleep(AFTER_STEP_INTERVAL);
      return this.isExit;
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
        this.grid.InitCells(GRID_FILLED_COUNT_PERCENTS);
        Task.Factory.StartNew(() => Utils.RunAlgorithm(this.grid, this.gridVectors, GRID_MIN_CELLS_COUNT, this.Afterstep));
        this.Invalidate();
      }
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.Clear(Color.White);
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

      this.gridView.DrawGrid(e.Graphics, this.grid);
      this.gridView.DrawGridVectors(e.Graphics, this.grid, this.gridVectors);
    }

    private void MainForm_Resize(object sender, EventArgs e)
    {
      this.Invalidate();
    }
  }
}
