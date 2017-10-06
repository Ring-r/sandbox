using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Prototypes.Maps.StripsAndRooms
{
	public class MainForm : Prototypes.Forms.Form
	{
		private Map map = new Map();

		public MainForm()
		{
			this.scale = 8.0f;
		}

		protected override void Init()
		{
			this.map.Init(Map.Settings.Default);
			this.Invalidate();
		}

		protected override void Form_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Black);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			Map.Style.Default.CellSize = (UInt16)this.scale;
			this.map.Draw(e.Graphics, Map.Style.Default);
		}
	}
}
