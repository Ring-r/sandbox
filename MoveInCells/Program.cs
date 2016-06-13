using System;
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
      this.timer.Interval = 10;
      this.timer.Tick += this.timer_Tick;

      this.DoubleBuffered = true;
      this.Name = this.Text = "MainForm";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.WindowState = FormWindowState.Maximized;

      this.Load += this.MainForm_Load;
      this.Paint += this.MainForm_Paint;
      this.KeyDown += this.MainForm_KeyDown;
      this.KeyUp += this.MainForm_KeyUp;

      this.ResumeLayout(false);
    }
  }

  partial class MainForm : Form
  {
    private readonly World world = new World();
    private readonly Controller controller = new Controller();

    private void MainForm_Load(object sender, EventArgs e)
    {
      this.world.Create();
      this.controller.SetEntity(this.world.GetMainEntity());
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        Close();
      }

      this.controller.AddKey(e.KeyData);
    }
    private void MainForm_KeyUp(object sender, KeyEventArgs e)
    {
      this.controller.RemoveKey(e.KeyData);
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.Clear(Color.White);
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      this.world.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height);
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      if (this.controller.Update())
      {
        this.world.UpdateMainEntity();
      }

      this.world.Update();

      this.Text = this.world.MaxScore.ToString();
      this.Invalidate();
    }
  }
}
