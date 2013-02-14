using Entities;

namespace WeaponTest
{
	class Ball : EntityRectangle
	{
		public override void onManagedUpdate (float pSecondsElapsed)
		{
			this.CenterX += this.VectorX * this.Speed * pSecondsElapsed;
			this.CenterY += this.VectorY * this.Speed * pSecondsElapsed;
		}
	}
}
