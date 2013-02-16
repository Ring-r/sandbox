using System;
using System.Collections.Generic;
using System.Drawing;
using Entities;

namespace WeaponTest
{
	class Weapon:WeaponBase<Bullet>
	{
		protected override Bullet CreateElement ()
		{
			Bullet bullet = base.CreateElement ();
			bullet.Angle += this.Angle;
			return bullet;
		}

		public override void onManagedDraw (Graphics graphics)
		{
			float angle;
			if (this.shotCount < this.ShotCount) {
				angle = this.secondsElapsed >= this.ShotTime ? 360 : 360 * this.secondsElapsed / this.ShotTime;
				graphics.FillPie (Brushes.Silver, Options.CameraWidth / 2 - 100, 0, 200, 200, 0, angle);
			} else {
				if (this.RechargeTime != 0) {
					angle = 360 * this.secondsElapsed / this.RechargeTime;
					graphics.FillPie (Brushes.Black, Options.CameraWidth / 2 - 100, 0, 200, 200, 0, angle);
				}
			}

			graphics.DrawEllipse (Pens.Black, Options.CameraWidth / 2 - 100, 0, 200, 200);

			base.onManagedDraw (graphics);
		}

		public void Aim (int cursorX, int cursorY)
		{
			this.startCursorX = cursorX;
			this.startCursorY = cursorY;
			this.startTime = DateTime.Now;
		}

		public void AimUpdate (int cursorX, int cursorY)
		{
			this.cursorX = cursorX;
			this.cursorY = cursorY;
		}

		public void Shot ()
		{
			if (this.secondsElapsed > this.ShotTime && this.shotCount < this.ShotCount) {
				float x = this.cursorX - this.startCursorX;
				float y = this.cursorY - this.startCursorY;
				float dist = (float)Math.Sqrt (x * x + y * y);
				base.CreateElement ().Speed = Math.Min (this.BulletsSpeed, dist / (float)(DateTime.Now - this.startTime).TotalMilliseconds * 1000);
				this.secondsElapsed = 0;
			}
		}
	}
}
