using System;
using Entities;
using System.Drawing;

namespace WeaponTest
{
	class Enemy : EntityCircle
	{
		public override void onManagedUpdate (float pSecondsElapsed)
		{
			this.CenterX += this.VectorX * this.Speed * pSecondsElapsed;
			this.CenterY += this.VectorY * this.Speed * pSecondsElapsed;

            //if (this.Y > Options.CameraHeight || this.Y + this.Height < 0) {
            //    this.Clear ();
            //}
		}

	}
}

