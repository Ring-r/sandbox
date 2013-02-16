using System;
using Entities;
using System.Drawing;

namespace WeaponTest
{
	class Enemy : EntityCircle
	{
		public override void onManagedDraw (Graphics graphics)
		{
			graphics.TranslateTransform (this.CenterX, this.CenterY);
			graphics.RotateTransform (90 + this.Angle / (float)Math.PI * 180);
			graphics.TranslateTransform (-this.CenterX, -this.CenterY);
			base.onManagedDraw (graphics);
			graphics.ResetTransform ();
		}

		public override void onManagedUpdate (float pSecondsElapsed)
		{
			this.CenterX += this.VectorX * this.Speed * pSecondsElapsed;
			this.CenterY += this.VectorY * this.Speed * pSecondsElapsed;

			if (this.Y > Options.CameraHeight || this.Y + this.Height < 0) {
				this.Clear ();
			}
		}

	}
}

