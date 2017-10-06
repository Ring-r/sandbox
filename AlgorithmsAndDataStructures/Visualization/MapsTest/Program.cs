using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MapsTest
{
  static class Program
  {
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }

  public class MainForm : Form
  {
    private const int iCount = 100;
    private const int jCount = 100;
    private readonly PointF[,] points = new PointF[iCount, jCount];
    private const float step = 30;

    private void CreateSquares()
    {
      float y = 0;
      for (int i = 0; i < iCount; ++i)
      {
        float x = 0;
        for (int j = 0; j < jCount; ++j)
        {
          this.points[i, j] = new PointF(x, y);
          x += step;
        }
        y += step;
      }
    }

    private void CreateTriangles()
    {
      float yStep = 0.5f * (float)Math.Sqrt(2) * step;
      float step_2 = 0.5f * step;
      float y = 0;
      for (int i = 0; i < iCount; ++i)
      {
        float x = (i & 1) == 0 ? 0 : step_2;
        for (int j = 0; j < jCount; ++j)
        {
          this.points[i, j] = new PointF(x, y);
          x += step;
        }
        y += yStep;
      }
    }

    public MainForm()
    {
      this.SuspendLayout();

      this.DoubleBuffered = true;
      this.Name = this.Text = "MainForm";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.WindowState = FormWindowState.Maximized;

      this.Load += this.MainForm_load;
      this.KeyDown += this.MainForm_KeyDown;
      this.MouseClick += this.MainForm_MouseClick;
      this.Paint += this.MainForm_Paint;

      this.ResumeLayout(false);
    }

    private void MainForm_load(object sender, EventArgs e)
    {
      //this.CreateSquares();
      this.CreateTriangles();
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyData == Keys.Escape)
      {
        this.Close();
      }
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.Clear(Color.White);
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

      //this.DrawSquaresLines(e.Graphics);
      this.DrawTrianglesLines(e.Graphics);

      this.DrawNodes(e.Graphics);
    }

    private void DrawSquaresLines(Graphics g)
    {
      for (int i = 0; i < iCount; ++i)
      {
        for (int j = 0; j < jCount - 1; ++j)
        {
          g.DrawLine(Pens.Black, this.points[i, j], this.points[i, j + 1]);
        }
      }


      for (int i = 0; i < iCount - 1; ++i)
      {
        for (int j = 0; j < jCount; ++j)
        {
          g.DrawLine(Pens.Black, this.points[i, j], this.points[i + 1, j]);
        }
      }
    }

    private void DrawTrianglesLines(Graphics g)
    {
      for (int i = 0; i < iCount; ++i)
      {
        for (int j = 0; j < jCount - 1; ++j)
        {
          g.DrawLine(Pens.Black, this.points[i, j], this.points[i, j + 1]);
        }
      }


      for (int i = 0; i < iCount - 1; ++i)
      {
        for (int j = 0; j < jCount; ++j)
        {
          g.DrawLine(Pens.Black, this.points[i, j], this.points[i + 1, j]);
        }
      }

      for (int i = 0; i < iCount - 1; i += 2)
      {
        for (int j = 1; j < jCount; ++j)
        {
          g.DrawLine(Pens.Black, this.points[i, j], this.points[i + 1, j - 1]);
        }
      }

      for (int i = 1; i < iCount - 1; i += 2)
      {
        for (int j = 0; j < jCount - 1; ++j)
        {
          g.DrawLine(Pens.Black, this.points[i, j], this.points[i + 1, j + 1]);
        }
      }
    }

    private void DrawNodes(Graphics g)
    {
      for (int i = 0; i < iCount; ++i)
      {
        for (int j = 0; j < jCount; ++j)
        {
          g.DrawEllipse(Pens.Red, this.points[i, j].X, this.points[i, j].Y, 1, 1);
        }
      }
    }

    private void DrawIcosahedron(Graphics g)
    {
      PointF[] triangle = new PointF[3];
      triangle[0] = new PointF();
    }

    private void MainForm_MouseClick(object sender, MouseEventArgs e)
    {
      // TODO: Define element at X,Y.
      // TODO: Define neighbors for element at X,Y.
    }

    // TODO: Create and draw hexagon.
  }
}
