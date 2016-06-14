using System;
using System.Collections.Generic;
using System.Drawing;

namespace Prototype.Spark
{
  public class Line
  {
    public float px = 0.0f;
    public float py = 0.0f;
    public float vx = 1.0f;
    public float vy = 0.0f;
    public readonly List<Tuple<float, Line>> ts = new List<Tuple<float, Line>>();

    public void RemoveBorderParts()
    {
      // FIXME: Imcorrect work when some Item2 equal to 0.

      int i0 = 0;
      while (i0 < this.ts.Count - 1 && this.ts[i0].Item1 < 0 && this.ts[i0 + 1].Item1 < 0)
      {
        this.ts[i0].Item2.ts.RemoveAll(item => item.Item2 == this);
        i0 += 1;
      }
      this.ts.RemoveRange(0, i0);

      int i1 = 1;
      while (i1 < this.ts.Count - 1 && this.ts[i1].Item1 > 0 && this.ts[i1 + 1].Item1 > 0)
      {
        // TODO: Check equal to 0.
        this.ts[i1 + 1].Item2.ts.RemoveAll(item => item.Item2 == this);
        i1 += 1;
      }
      this.ts.RemoveRange(2, i1 - 1);
    }

    public void Draw(Graphics graphics, float scale, Pen pen)
    {
      float x0 = (this.px + this.vx * this.ts[0].Item1) * scale;
      float y0 = (this.py + this.vy * this.ts[0].Item1) * scale;
      float x1 = (this.px + this.vx * this.ts[this.ts.Count - 1].Item1) * scale;
      float y1 = (this.py + this.vy * this.ts[this.ts.Count - 1].Item1) * scale;
      graphics.DrawLine(pen, x0, y0, x1, y1);
    }
  }
}
