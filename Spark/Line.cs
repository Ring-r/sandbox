using System;
using System.Collections.Generic;
using System.Drawing;

namespace Prototype.Spark
{
  public class Line
  {
    public float px { get; private set; }
    public float py { get; private set; }
    public float vx { get; private set; }
    public float vy { get; private set; }

    public struct Cross
    {
      public float offset;
      public Line line;
    }
    public readonly List<Cross> ts = new List<Cross>();

    public Line(float px, float py, float angle)
    {
      this.px = px;
      this.py = py;
      this.vx = (float)Math.Cos(angle);
      this.vy = (float)Math.Sin(angle);
    }
    public Line(float px, float py, float vx, float vy)
    {
      this.px = px;
      this.py = py;
      this.vx = vx;
      this.vy = vy;
    }

    public static Line CreateRandom()
    {
      float x = (float)Program.Random.NextDouble();
      float y = (float)Program.Random.NextDouble();
      float angle = (float)(Math.PI * Program.Random.NextDouble());
      return new Line(x, y, angle);
    }

    public void RemoveBorderParts()
    {
      // FIXME: Imcorrect work when some line equal to 0.

      int i0 = 0;
      while (i0 < this.ts.Count - 1 && this.ts[i0].offset < 0 && this.ts[i0 + 1].offset < 0)
      {
        this.ts[i0].line.ts.RemoveAll(item => item.line == this);
        i0 += 1;
      }
      this.ts.RemoveRange(0, i0);

      int i1 = 1;
      while (i1 < this.ts.Count - 1 && this.ts[i1].offset > 0 && this.ts[i1 + 1].offset > 0)
      {
        // TODO: Check equal to 0.
        this.ts[i1 + 1].line.ts.RemoveAll(item => item.line == this);
        i1 += 1;
      }
      this.ts.RemoveRange(2, i1 - 1);
    }

    public void Draw(Graphics graphics, float scale, Pen pen)
    {
      float x0 = (this.px + this.vx * this.ts[0].offset) * scale;
      float y0 = (this.py + this.vy * this.ts[0].offset) * scale;
      float x1 = (this.px + this.vx * this.ts[this.ts.Count - 1].offset) * scale;
      float y1 = (this.py + this.vy * this.ts[this.ts.Count - 1].offset) * scale;
      graphics.DrawLine(pen, x0, y0, x1, y1);
    }
  }
}
