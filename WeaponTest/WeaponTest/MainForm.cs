using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WeaponTest
{
	public partial class MainForm : Form
	{
		private readonly Weapon weapon = new Weapon ();

		public MainForm ()
		{
			InitializeComponent ();
		}

		private void timer_Tick (object sender, EventArgs e)
		{
			this.weapon.onManagedUpdate (this.timer.Interval / 1000f);
			this.Invalidate ();
		}

		private void MainForm_Paint (object sender, PaintEventArgs e)
		{
			this.weapon.onManagedDraw (e.Graphics);
		}

		private void MainForm_MouseMove (object sender, MouseEventArgs e)
		{
			Options.CursorX = e.X;
			Options.CursorY = e.Y;
		}

		private void MainForm_KeyUp (object sender, KeyEventArgs e)
		{
			switch (e.KeyData) {
			case Keys.Escape:
				this.Close ();
				break;
			}
		}
	}
}
