using System;
using System.Collections.Generic;
using System.Drawing;
using Entities;
using Pools;

namespace WeaponTest
{
	class Evil:IPool
	{
		private float secondsElapsed = 0;
		internal readonly List<IEntity> enemies = new List<IEntity> (); // TODO: Use private.
		//
		public static int AimCursorX = 0;
		public static int AimCursorY = 0;
		public static int ShotCursorX = 0;
		public static int ShotCursorY = 0;
		//
		public float EnemiesWidth = 50f;
		public float EnemiesHeight = 50f;
		public float EnemiesSpeed = 50f; // Points in second.
		public float EnemiesHealth = 1f;
		public float EnemiesLifeTime = 100f; // Seconds.
		//
		public float ShotTime = 1f; // Time (seconds) for prepare bllet to shot.
		private int shotCount = 0;
		public int ShotCount = 10;
		//
		public float RechargeTime = 5f; // Time (seconds) for one recharge.

		public void onManagedDraw (Graphics graphics)
		{
//			float angle;
//			if (this.shotCount < this.ShotCount) {
//				angle = this.secondsElapsed >= this.ShotTime ? 360 : 360 * this.secondsElapsed / this.ShotTime;
//				graphics.FillPie (Brushes.Silver, Options.CameraWidth / 2 - 100, 0, 200, 200, 0, angle);
//			} else {
//				if (this.RechargeTime != 0) {
//					angle = 360 * this.secondsElapsed / this.RechargeTime;
//					graphics.FillPie (Brushes.Black, Options.CameraWidth / 2 - 100, 0, 200, 200, 0, angle);
//				}
//			}
//
//			graphics.DrawEllipse (Pens.Black, Options.CameraWidth / 2 - 100, 0, 200, 200);

			for (int i = 0; i < this.enemies.Count; ++i) {
				this.enemies [i].onManagedDraw (graphics);
			}
		}

		public void onManagedUpdate (float secondsElapsed)
		{
			for (int i = 0; i < this.enemies.Count; ++i) {
				this.enemies [i].onManagedUpdate (secondsElapsed);
			}

			this.secondsElapsed += secondsElapsed;

			if (this.shotCount >= this.ShotCount && this.secondsElapsed >= this.RechargeTime) {
				this.secondsElapsed -= this.RechargeTime;
				this.shotCount = 0;
			}

			while (this.shotCount < this.ShotCount && this.secondsElapsed >= this.ShotTime) {
				this.secondsElapsed -= this.ShotTime;
				AimCursorX = Options.Random.Next (Options.CameraWidth);
				AimCursorY = 0;
				ShotCursorX = Options.Random.Next (Options.CameraWidth);
				ShotCursorY = Options.CameraHeight;
				Enemy enemy = new Enemy ()
                	{
						Parent = this,
                    		CenterX = AimCursorX,
                    		CenterY = AimCursorY,
                    		Width = this.EnemiesWidth,
                    		Height = this.EnemiesHeight,
                    		Angle = (float)Math.Atan2(ShotCursorY - AimCursorY, ShotCursorX - AimCursorX),
                    		Speed = this.EnemiesSpeed,
							LifeTime = this.EnemiesLifeTime,
                        	Health = this.EnemiesHealth,
                		};
				this.enemies.Add (enemy);
				enemy.onManagedUpdate (this.secondsElapsed);
				++this.shotCount;
			}
		}
	}
}

