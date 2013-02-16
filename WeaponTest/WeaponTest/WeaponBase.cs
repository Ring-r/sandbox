using System;
using System.Collections.Generic;
using System.Drawing;
using Entities;

namespace WeaponTest
{
	abstract class WeaponBase<BulletType>
		where BulletType:IEntity,new()
	{
		protected float secondsElapsed = 0;
		protected readonly List<IEntity> bullets = new List<IEntity> ();
		//
		protected int startCursorX = 0;
		protected int startCursorY = 0;
		protected DateTime startTime;
		protected int cursorX = 0;
		protected int cursorY = 0;
		//
		public float Angle = 0;
		public bool AutoShots = false;
		//
		public float BulletsWidth = 5f;
		public float BulletsHeight = 5f;
		public float BulletsSpeed = 50f; // points in second;
		public float BulletsHelth = 1f;
		//
		public float ShotTime = 0.1f; // time (seconds) for one shot;
		protected int shotCount = 0;
		public int ShotCount = 10;
		//
		public float RechargeTime = 0f; // time (seconds) for one recharge;

		protected virtual BulletType CreateElement ()
		{
			BulletType bullet = new BulletType ()
                	{
                    	CenterX = this.startCursorX,
                    	CenterY = this.startCursorY,
                    	Width = this.BulletsWidth,
                    	Height = this.BulletsHeight,
                    	Angle = (float)Math.Atan2(this.cursorY - this.startCursorY, this.cursorX - this.startCursorX),
                    	Speed = this.BulletsSpeed,
                        Health = this.BulletsHelth,
                	};
			this.bullets.Add (bullet);
			return bullet;
		}

		public virtual void onManagedDraw (Graphics graphics)
		{
			for (int i = 0; i < this.bullets.Count; ++i) {
				this.bullets [i].onManagedDraw (graphics);
			}
		}

		public virtual void onManagedUpdate (float secondsElapsed)
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
					this.CreateElement ().onManagedUpdate (this.secondsElapsed);
					++this.shotCount;
				}
			} else {
				this.secondsElapsed = Math.Min(this.secondsElapsed, Math.Max(this.ShotTime, this.RechargeTime));
			}
            
			for (int i = 0; i < this.bullets.Count; ++i) {
				if (this.bullets [i].Y > Options.CameraHeight || this.bullets [i].Y + this.bullets [i].Height < 0) {
					this.bullets.RemoveAt (i);
					--i;
				}
			}
		}

	}
}

