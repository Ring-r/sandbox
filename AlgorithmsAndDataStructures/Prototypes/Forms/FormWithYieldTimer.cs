using System;
using System.Collections.Generic;

namespace Prototypes.Forms
{
	public abstract class FormWithYieldTimer : FormWithTimer
	{
		protected IEnumerator<Boolean> enumerator = null;

		protected void DisposeEnumerator()
		{
			if (enumerator != null)
			{
				this.enumerator.Dispose();
				this.enumerator = null;
			}
		}

		protected override void Dispose(bool disposing)
		{
			this.DisposeEnumerator();
			base.Dispose(disposing);
		}

		protected override void Timer_Tick(object sender, EventArgs e)
		{
			if (this.enumerator != null && this.enumerator.MoveNext())
			{
				this.Invalidate();
			}
		}
	}
}
