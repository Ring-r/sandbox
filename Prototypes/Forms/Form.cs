using System;
using System.Windows.Forms;

namespace Prototypes.Forms
{
	public abstract class Form : System.Windows.Forms.Form
	{
		protected float scale = 1.0f;
		protected float scaleStep = 0.1f;

		public Form()
		{
			this.SuspendLayout();

			this.DoubleBuffered = true;
			this.Name = this.Text = "MainForm";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.WindowState = FormWindowState.Maximized;

			this.KeyDown += this.Form_KeyDown;
			this.Load += this.Form_Load;
			this.MouseWheel += this.Form_MouseWheel;
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
			if (e.KeyData == Keys.Enter)
			{
				this.Init();
			}
		}

		protected virtual void Form_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		protected virtual void Form_MouseWheel(object sender, MouseEventArgs e)
		{
			this.scale = Math.Max(this.scale + (e.Delta > 0 ? +this.scaleStep : -this.scaleStep), 0.0f);
			this.Invalidate();
		}

		protected abstract void Form_Paint(object sender, PaintEventArgs e);

		protected virtual void Form_Resize(object sender, EventArgs e)
		{
			this.Invalidate();
		}

		protected abstract void Init();
	}
}
