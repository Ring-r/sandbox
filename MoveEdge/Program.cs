using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
  }

  partial class MainForm : Form
  {
    private IContainer components = new Container();
    private Timer timer;

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    public MainForm()
    {
      this.timer = new Timer(this.components);

      this.SuspendLayout();
      this.timer.Enabled = true;
      this.timer.Tick += this.timer_Tick;

      this.DoubleBuffered = true;
      this.Name = this.Text = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

      this.Load += this.MainForm_Load;
      this.Paint += this.MainForm_Paint;
      this.KeyDown += this.MainForm_KeyDown;
      this.KeyUp += this.MainForm_KeyUp;

      this.ResumeLayout(false);
    }
  }

  partial class MainForm : Form
  {
    private readonly Random random = new Random();

    private const int ENTITIES_COUNT = 10;

    private readonly Entity entity = new Entity();
    private readonly List<Point> entities = new List<Point>(ENTITIES_COUNT);

    private readonly bool[] keys = new bool[12];

    private void MainForm_Load(object sender, EventArgs e)
    {
      this.entity.Init(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
      for (int i = 0; i < ENTITIES_COUNT; ++i)
      {
        this.entities.Add(new Point(random.Next(this.ClientSize.Width), random.Next(this.ClientSize.Height)));
      }
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.Clear(Color.White);
      e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      e.Graphics.TranslateTransform(0, this.ClientSize.Height);
      e.Graphics.ScaleTransform(1, -1);

      float x = 0.5f * (this.entity.x_ + this.entity._x);
      float y = 0.5f * (this.entity.y_ + this.entity._y);
      float a = (float)Math.Atan2(this.entity._y - this.entity.y_, this.entity._x - this.entity.x_);

      e.Graphics.TranslateTransform(-x, -y);
      e.Graphics.TranslateTransform(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

      e.Graphics.TranslateTransform(x, y);
      e.Graphics.RotateTransform(-(float)(a / Math.PI * 180));
      e.Graphics.TranslateTransform(-x, -y);


      for (int i = 0; i < ENTITIES_COUNT; ++i)
      {
        e.Graphics.FillEllipse(Brushes.Green, this.entities[i].X - 3, this.entities[i].Y - 3, 6, 6);
      }

      this.entity.Draw(e.Graphics, Pens.Black);
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyData)
      {
        case Keys.Escape:
          this.Close();
          break;
        case Keys.W:
          this.keys[0] = true;
          break;
        case Keys.A:
          this.keys[1] = true;
          break;
        case Keys.D:
          this.keys[2] = true;
          break;
        case Keys.S:
          this.keys[3] = true;
          break;
        case Keys.I:
          this.keys[4] = true;
          break;
        case Keys.J:
          this.keys[5] = true;
          break;
        case Keys.L:
          this.keys[6] = true;
          break;
        case Keys.K:
          this.keys[7] = true;
          break;
        case Keys.Up:
          this.keys[8] = true;
          break;
        case Keys.Left:
          this.keys[9] = true;
          break;
        case Keys.Right:
          this.keys[10] = true;
          break;
        case Keys.Down:
          this.keys[11] = true;
          break;
      }
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e)
    {
      switch (e.KeyData)
      {
        case Keys.W:
          this.keys[0] = false;
          break;
        case Keys.A:
          this.keys[1] = false;
          break;
        case Keys.D:
          this.keys[2] = false;
          break;
        case Keys.S:
          this.keys[3] = false;
          break;
        case Keys.I:
          this.keys[4] = false;
          break;
        case Keys.J:
          this.keys[5] = false;
          break;
        case Keys.L:
          this.keys[6] = false;
          break;
        case Keys.K:
          this.keys[7] = false;
          break;
        case Keys.Up:
          this.keys[8] = false;
          break;
        case Keys.Left:
          this.keys[9] = false;
          break;
        case Keys.Right:
          this.keys[10] = false;
          break;
        case Keys.Down:
          this.keys[11] = false;
          break;
      }
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      float s = this.entity.r;
      this.entity.vx_ = 0; this.entity.vy_ = 0; this.entity._vx = 0; this.entity._vy = 0;
      if (this.keys[0]) this.entity.vy_ += s;
      if (this.keys[1]) this.entity.vx_ -= s;
      if (this.keys[2]) this.entity.vx_ += s;
      if (this.keys[3]) this.entity.vy_ -= s;
      if (this.keys[4]) this.entity._vy += s;
      if (this.keys[5]) this.entity._vx -= s;
      if (this.keys[6]) this.entity._vx += s;
      if (this.keys[7]) this.entity._vy -= s;

      if (this.keys[8]) { this.entity.vy_ += s; this.entity._vy += s; }
      if (this.keys[9]) this.entity._vy += s;
      if (this.keys[10]) this.entity.vy_ += s;
      if (this.keys[11]) { this.entity.vy_ -= s; this.entity._vy -= s; }

      this.entity.Move();

      if (this.entity.x_ < 0 && this.entity.vx_ < 0 || this.entity.x_ > this.ClientSize.Width && this.entity.vx_ > 0)
        this.entity.vx_ = -this.entity.vx_;
      if (this.entity.y_ < 0 && this.entity.vy_ < 0 || this.entity.y_ > this.ClientSize.Height && this.entity.vy_ > 0)
        this.entity.vy_ = -this.entity.vy_;
      if (this.entity._x < 0 && this.entity._vx < 0 || this.entity._x > this.ClientSize.Width && this.entity._vx > 0)
        this.entity._vx = -this.entity._vx;
      if (this.entity._y < 0 && this.entity._vy < 0 || this.entity._y > this.ClientSize.Height && this.entity._vy > 0)
        this.entity._vy = -this.entity._vy;

      this.Invalidate();
    }
  }
}
