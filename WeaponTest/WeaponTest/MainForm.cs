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
		private readonly List<Weapon> weapons = new List<Weapon> ();
		private readonly Stopwatch stopwatch = new Stopwatch ();

		public MainForm ()
		{
			InitializeComponent ();

			this.weapons.Add (new Weapon (){BulletsSpeed=100f});
		}

		private void timer_Tick (object sender, EventArgs e)
		{
			this.weapons.ForEach ((weapon) => weapon.onManagedUpdate (this.stopwatch.ElapsedMilliseconds / 1000f));
			this.stopwatch.Restart ();
			this.Invalidate ();
		}

		private void MainForm_Paint (object sender, PaintEventArgs e)
		{
			e.Graphics.Clear (Color.White);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			this.weapons.ForEach ((weapon) => weapon.onManagedDraw (e.Graphics));
		}

		private void MainForm_MouseDown (object sender, MouseEventArgs e)
		{
			this.weapons.ForEach ((weapon) => weapon.Aim (e.X, e.Y));
			if (e.Button == MouseButtons.Right) {
				this.weapons.ForEach ((weapon) => weapon.AutoShots = true);
			}
		}

		private void MainForm_MouseMove (object sender, MouseEventArgs e)
		{
			this.weapons.ForEach ((weapon) => {
				if (e.Button == MouseButtons.Right) {
					weapon.AimUpdate (e.X, e.Y);
				} }
			);
		}

		private void MainForm_MouseUp (object sender, MouseEventArgs e)
		{
			this.weapons.ForEach ((weapon) => weapon.Shoot (e.X, e.Y));
			if (e.Button == MouseButtons.Right) {
				this.weapons.ForEach ((weapon) => weapon.AutoShots = false);
			}

		}

		private void MainForm_KeyUp (object sender, KeyEventArgs e)
		{
			switch (e.KeyData) {
			case Keys.Escape:
				this.Close ();
				break;
			case Keys.Enter:
				this.weapons.ForEach ((weapon) => weapon.AutoShots = !weapon.AutoShots);
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
