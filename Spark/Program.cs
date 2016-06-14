using Prototype.Spark;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Prototype
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

    private static readonly Lazy<Random> random = new Lazy<Random>(() => new Random());
    public static Random Random { get { return random.Value; } }
  }

  public class MainForm : Form
  {
    private readonly Timer timer = new Timer() { Interval = 20 };

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.timer.Dispose();
      }
      base.Dispose(disposing);
    }

    public MainForm()
    {
      this.SuspendLayout();

      this.DoubleBuffered = true;
      this.Name = this.Text = "MainForm";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.WindowState = FormWindowState.Maximized;

      this.Load += this.MainForm_Load;
      this.KeyDown += this.MainForm_KeyDown;
      this.Paint += this.MainForm_Paint;
      this.Resize += this.MainForm_Resize;
      this.timer.Tick += this.Timer_Tick;

      this.ResumeLayout(false);
    }

    private const int LEVEL_MAIN_LINES_COUNT = 10;
    private const int LEVEL_ENTITIES_COUNT = 10;
    private const float LEVEL_ENTITIES_SPEED = 0.05f;

    private readonly Level level = new Level();

    private void MainForm_Load(object sender, EventArgs e)
    {
      this.level.Init(LEVEL_MAIN_LINES_COUNT, LEVEL_ENTITIES_COUNT, LEVEL_ENTITIES_SPEED);
      this.timer.Start();
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        this.Close();
      }
      if (e.KeyCode == Keys.Enter)
      {
        this.MainForm_Load(this, EventArgs.Empty);
      }
    }

    private void MainForm_Resize(object sender, EventArgs e)
    {
      this.Invalidate();
    }

    private const float epsCoef = 0.1f;

    private readonly Color background = Color.Black;
    private readonly Pen linePen = Pens.Green;

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.Clear(background);
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

      float scale = 0.9f * Math.Min(this.ClientSize.Width, this.ClientSize.Height);
      e.Graphics.TranslateTransform(this.ClientSize.Width / 2 - scale / 2, this.ClientSize.Height / 2 - scale / 2);

      foreach (var line in this.level.lines)
      {
        line.Draw(e.Graphics, scale, this.linePen);
      }

      foreach (var entity in this.level.entities)
      {
        entity.Draw(e.Graphics, scale);
      }

      e.Graphics.ResetTransform();

      //			if (this.isEnd) {
      //				string text0 = "You loose aim!";
      //				SizeF text0Size = e.Graphics.MeasureString(text0, this.Font);
      //				e.Graphics.DrawString(text0, this.Font, Brushes.White,
      //					this.ClientSize.Width / 2 - text0Size.Width / 2, this.ClientSize.Height / 2 - text0Size.Height / 2);
      //
      //				string text1 = "Type \"Enter\" to start new game!";
      //				SizeF text1Size = e.Graphics.MeasureString(text1, this.Font);
      //				e.Graphics.DrawString(text1, this.Font, Brushes.White,
      //					this.ClientSize.Width / 2 - text1Size.Width / 2, this.ClientSize.Height / 2 + text0Size.Height / 2);
      //			}
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      float time = this.timer.Interval / 1000.0f;
      foreach (var entity in this.level.entities)
      {
        entity.Update(time);
      }
      this.Invalidate();
    }
  }
}
