using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Prototypes.Maps.StripsAndRooms
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      this.SuspendLayout();

      this.DoubleBuffered = true;
      this.Name = this.Text = "MainForm";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.WindowState = FormWindowState.Maximized;

      this.KeyDown += this.MainForm_KeyDown;
      this.Load += this.MainForm_Load;
      this.MouseWheel += this.MainForm_MouseWheel;
      this.Paint += this.MainForm_Paint;

      this.ResumeLayout(false);
    }

    private Map map = new Map();

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyData == Keys.Escape)
      {
        this.Close();
      }
      if (e.KeyData == Keys.Enter)
      {
        this.MainForm_Load(this, EventArgs.Empty);
      }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      this.map.Init(Map.Settings.Default);
      this.Invalidate();
    }

    private readonly UInt16 scaleStep = 1;
    private void MainForm_MouseWheel(object sender, MouseEventArgs e)
    {
      Int32 cellSize = Map.Style.Default.CellSize + this.scaleStep * e.Delta / 120;
      Map.Style.Default.CellSize = (UInt16)Math.Max(cellSize, 0);
      this.Invalidate();
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.Clear(Color.Black);
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

      this.map.Draw(e.Graphics, Map.Style.Default);
    }
  }
}
