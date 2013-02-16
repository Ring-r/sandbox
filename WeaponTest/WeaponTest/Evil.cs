using System;
using System.Collections.Generic;
using System.Drawing;
using Entities;

namespace WeaponTest
{
	class Evil:WeaponBase<Enemy>
	{
		protected override Enemy CreateElement ()
		{
			this.startCursorX = Options.Random.Next (Options.CameraWidth);
			this.startCursorY = 0;
			this.cursorX = Options.Random.Next (Options.CameraWidth);
			this.cursorY = Options.CameraHeight;

			return base.CreateElement ();
		}
	}
}

