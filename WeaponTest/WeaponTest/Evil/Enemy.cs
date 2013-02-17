using System;
using Entities;
using System.Drawing;
using Pools;

namespace WeaponTest
{
	class Enemy : EntityCircle, IPoolable
	{
		#region IPoolable implementation
		public IPoolable DeepCopy ()
		{
			throw new System.NotImplementedException ();
		}

		public void Initialize ()
		{
			throw new System.NotImplementedException ();
		}

		public IPool Parent { get ; set ; }

		public int Id {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}
		#endregion

		public override void onManagedUpdate (float secondsElapsed)
		{
			this.CenterX += this.VectorX * this.Speed * secondsElapsed;
			this.CenterY += this.VectorY * this.Speed * secondsElapsed;

			this.LifeTime -= secondsElapsed;

			if (this.LifeTime <= 0 || this.Health <= 0 || this.Y > Options.CameraHeight || this.Y + this.Height < 0) {
				(this.Parent as Evil).enemies.Remove(this);
			}
		}

	}
}

