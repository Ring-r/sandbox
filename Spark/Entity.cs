using System;
using System.Collections.Generic;
using System.Drawing;

namespace Prototype.Spark
{
  public class Entity
  {
    private readonly float radius = 3.0f;
    public Brush brush = Brushes.Black;

    public Line line = null;
    public float offset = 0.0f;

    public float speed = 0.0f;

    protected int direction = 0;
    public int directionNext = 0;

    private bool auto = true;
    private void RecalculateDirection()
    {
      this.direction = auto ? Program.Random.Next(3) - 1 : Math.Sign(this.directionNext);
    }

    public void Clear()
    {
      this.line = null;
      this.offset = 0.0f;

      this.speed = 0.0f;

      this.direction = 0;
      this.directionNext = 0;
    }

    public void Draw(Graphics graphics, float scale)
    {
      float x = (this.line.px + this.line.vx * this.offset) * scale;
      float y = (this.line.py + this.line.vy * this.offset) * scale;
      graphics.FillEllipse(this.brush, x - this.radius, y - this.radius, 2 * this.radius, 2 * this.radius);
    }

    public void Update(float time)
    {
      float step = this.speed * time;
      float stepEps = Math.Abs(0.9f * step);

      this.offset += step;

      var prevLine = this.line;

      bool changeLine = false;
      int prevOffsetIndex = -1;
      if (this.offset < prevLine.ts[0].Item1)
      {
        this.offset = prevLine.ts[0].Item1;
        prevOffsetIndex = 0;
        changeLine = true;
      }
      if (this.offset > prevLine.ts[prevLine.ts.Count - 1].Item1)
      {
        this.offset = prevLine.ts[prevLine.ts.Count - 1].Item1;
        prevOffsetIndex = prevLine.ts.Count - 1;
        changeLine = true;
      }
      if (!changeLine)
      {
        prevOffsetIndex = prevLine.ts.FindIndex(item => Math.Abs(item.Item1 - this.offset) < stepEps);
        if (0 <= prevOffsetIndex && prevOffsetIndex < prevLine.ts.Count)
        {
          this.RecalculateDirection();
          changeLine = this.direction != 0;
        }
      }

      if (changeLine)
      {
        this.line = prevLine.ts[prevOffsetIndex].Item2;

        int offsetIndex = this.line.ts.FindIndex(item => item.Item2 == prevLine);
        this.offset = this.line.ts[offsetIndex].Item1;

        bool isUseEntityDirection = true;
        if (offsetIndex == 0)
        {
          this.speed = Math.Abs(this.speed);
          isUseEntityDirection = false;
        }
        if (offsetIndex == this.line.ts.Count - 1)
        {
          this.speed = -Math.Abs(this.speed);
          isUseEntityDirection = false;
        }
        if (isUseEntityDirection)
        {
          // TODO: Correct by using dot vector production.
          if (this.direction != 0)
          {
            int sign = Math.Sign(this.speed);
            float prevVx = sign * prevLine.vx;
            float prevVy = sign * prevLine.vy;
            float prevVx_ = -prevVy;
            float prevVy_ = +prevVx;
            float prod = prevVx_ * this.line.vx + prevVy_ * this.line.vy;
            this.speed = Math.Sign(this.direction * prod) * Math.Abs(this.speed);
          }
        }
      }
    }
  }
}
