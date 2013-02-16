using System;
using System.Collections.Generic;
using System.Drawing;
using Entities;

namespace WeaponTest
{
	class Weapon
	{
		private float secondsElapsed = 0;
		internal readonly List<IEntity> bullets = new List<IEntity> ();
		//
		private int startCursorX = 0;
		private int startCursorY = 0;
		private DateTime startTime;
		private int cursorX = 0;
		private int cursorY = 0;
		//
		public float Angle = 0;
		public bool AutoShots = false;
		//
		public float BulletsWidth = 5f;
		public float BulletsHeight = 5f;
		public float BulletsSpeed = 500f; // points in second;
		//
		public float ShotTime = 0.1f; // time (seconds) for one shot;
		private int shotCount = 0;
		public int ShotCount = 10;
		//
		public float RechargeTime = 0f; // time (seconds) for one recharge;



		public void onManagedDraw (Graphics graphics)
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

			for (int i = 0; i < this.bullets.Count; ++i) {
				this.bullets [i].onManagedDraw (graphics);
			}
		}

		public void onManagedUpdate (float secondsElapsed)
		{
			for (int i = 0; i < this.bullets.Count; ++i) {
				this.bullets [i].onManagedUpdate (secondsElapsed);
			}

			this.secondsElapsed += secondsElapsed;

			if (this.shotCount >= this.ShotCount) {
				if (this.secondsElapsed > this.RechargeTime) {
					this.secondsElapsed -= this.RechargeTime;
					this.shotCount = 0;
				}
			}

			if (this.AutoShots) {
				while (this.secondsElapsed >= this.ShotTime && this.shotCount < this.ShotCount) {
					this.secondsElapsed -= this.ShotTime;

					Bullet bullet = new Bullet ()
                	{
                    	CenterX = this.startCursorX,
                    	CenterY = this.startCursorY,
                    	Width = this.BulletsWidth,
                    	Height = this.BulletsHeight,
                    	Angle = (float)Math.Atan2(this.cursorY - this.startCursorY, this.cursorX - this.startCursorX) + this.Angle,
                    	Speed = this.BulletsSpeed,
                        Health = (float)Options.Random.NextDouble() * 100,
                	};
					bullet.onManagedUpdate (this.secondsElapsed);
					this.bullets.Add (bullet);
					++this.shotCount;
				}
			}
            
            for (int i = 0; i < this.bullets.Count; ++i)
            {
                if (this.bullets[i].Y > Options.CameraHeight || this.bullets[i].Y + this.bullets[i].Height < 0)
                {
                    this.bullets.RemoveAt(i);
                    --i;
                }

            }
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

		public void Shoot (int cursorX, int cursorY)
		{
			this.cursorX = cursorX;
			this.cursorY = cursorY;
			if (this.secondsElapsed > this.ShotTime && this.shotCount < this.ShotCount) {
				this.secondsElapsed = 0;
				float x = this.cursorX - this.startCursorX;
				float y = this.cursorY - this.startCursorY;
				float dist = (float)Math.Sqrt (x * x + y * y);
				Bullet bullet = new Bullet ()
                {
                    CenterX = this.startCursorX,
                    CenterY = this.startCursorY,
                    Width = this.BulletsWidth,
                    Height = this.BulletsHeight,
                    Angle = (float)Math.Atan2(y, x) + this.Angle,
                    Speed = Math.Min(this.BulletsSpeed, dist / (float)(DateTime.Now - this.startTime).TotalMilliseconds * 1000),
                };
				this.bullets.Add (bullet);
				++this.shotCount;
			}
		}
	}
}
