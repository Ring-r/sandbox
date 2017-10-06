using System;
using System.Windows.Forms;

namespace Prototypes.Forms
{
	public abstract class FormWithTimer : Form
	{
		protected readonly Timer timer = new Timer();

		protected override void Dispose(bool disposing)
		{
			this.timer.Dispose();
			base.Dispose(disposing);
		}

		public FormWithTimer()
		{
			this.SuspendLayout();

			this.timer.Interval = 20;

			this.timer.Tick += this.Timer_Tick;

			this.ResumeLayout(false);
		}

		protected override void Form_Load(object sender, EventArgs e)
		{
			this.Init();
			this.timer.Start();
		}

		protected abstract void Timer_Tick(object sender, EventArgs e);
	}
}
