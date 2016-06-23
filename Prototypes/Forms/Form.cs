using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Prototypes.Forms
{
	public abstract class Form : System.Windows.Forms.Form
	{
		public Form()
		{
			this.SuspendLayout();

			this.DoubleBuffered = true;
			this.Name = this.Text = "MainForm";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.WindowState = FormWindowState.Maximized;

			this.KeyDown += this.Form_KeyDown;
			this.Paint += this.Form_Paint;
			this.Resize += this.Form_Resize;

			this.ResumeLayout(false);
		}

		protected virtual void Form_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
			{
				this.Close();
			}
		}

		protected abstract void Form_Paint(object sender, PaintEventArgs e);

		protected virtual void Form_Resize(object sender, EventArgs e)
		{
			this.Invalidate();
		}
	}
}
