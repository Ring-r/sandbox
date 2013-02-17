using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;

namespace WeaponTest
{
	public partial class MainForm : Form
	{
		private readonly Evil evil = new Evil (){};
		private readonly Weapon weapon = new Weapon ();
		private readonly Stopwatch stopwatch = new Stopwatch ();

		public MainForm ()
		{
			InitializeComponent ();
		}

		private void timer_Tick (object sender, EventArgs e)
		{
			this.evil.onManagedUpdate (this.stopwatch.ElapsedMilliseconds / 1000f);
			this.weapon.onManagedUpdate (this.stopwatch.ElapsedMilliseconds / 1000f);
			this.stopwatch.Restart ();
			this.Invalidate ();
			ColideHelper.Check (this.evil, this.weapon);
		}

		private void MainForm_Paint (object sender, PaintEventArgs e)
		{
			e.Graphics.Clear (Color.White);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			this.evil.onManagedDraw (e.Graphics);
			this.weapon.onManagedDraw (e.Graphics);
		}

		private void MainForm_MouseDown (object sender, MouseEventArgs e)
		{
			this.weapon.Aim ();
			this.weapon.AimCursorX = e.X;
			this.weapon.AimCursorY = e.Y;
			if (e.Button == MouseButtons.Right) {
				this.weapon.AutoShots = true;
			}
		}

		private void MainForm_MouseMove (object sender, MouseEventArgs e)
		{
			this.weapon.ShotCursorX = e.X;
			this.weapon.ShotCursorY = e.Y;
		}

		private void MainForm_MouseUp (object sender, MouseEventArgs e)
		{
			this.weapon.ShotCursorX = e.X;
			this.weapon.ShotCursorY = e.Y;
			this.weapon.Shot ();
			if (e.Button == MouseButtons.Right) {
				this.weapon.AutoShots = false;
			}

		}

		private void MainForm_KeyUp (object sender, KeyEventArgs e)
		{
			switch (e.KeyData) {
			case Keys.Escape:
				this.Close ();
				break;
			}
		}

		private void MainForm_Resize (object sender, EventArgs e)
		{
			Options.CameraWidth = this.ClientSize.Width;
			Options.CameraHeight = this.ClientSize.Height;
		}
	}
}
