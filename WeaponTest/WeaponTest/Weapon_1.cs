using System;
using System.Collections.Generic;
using Entities;
using System.Drawing;

namespace WeaponTest
{
	class Weapon<DataType>
	{
		protected float secondsElapsed = 0;
		internal readonly List<IEntity> bullets = new List<IEntity> ();
		//
		protected int startCursorX = 0;
		protected int startCursorY = 0;
		protected DateTime startTime;
		protected int cursorX = 0;
		protected int cursorY = 0;
		//
		public bool AutoShots = false;
		//
		//TODO: Create code to use BulletsWidth and BulletsHeight.
		public float BulletsSpeed = 10f; // points in second;
		//TODO: Create code to use default value for fields.
		//
		public float ShotTime = 0.1f; // time (seconds) for one shot;
		protected int shotCount = 0;
		public int ShotCount = 10;
		//
		public float RechargeTime = 0f; // time (seconds) for one recharge;

		public virtual void onManagedDraw (Graphics graphics)
		{
			for (int i = this.bullets.Count-1; i >=0; --i) {
				this.bullets [i].onManagedDraw (graphics);
			}
		}

		public virtual void onManagedUpdate (float secondsElapsed){
			for (int i = this.bullets.Count-1; i >=0; --i) {
				this.bullets [i].onManagedUpdate (secondsElapsed);
			}
	}
}

