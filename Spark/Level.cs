using System;
using System.Collections.Generic;
using System.Drawing;

namespace Prototype.Spark
{
  public class Level
  {
    public readonly List<Line> lines = new List<Line>();
    public readonly List<Entity> entities = new List<Entity>();

    public void Init(int linesCount, int entitiesCount, float entitiesSpeed)
    {
      #region Create map.

      this.lines.Clear();

      #region Create base lines.

      var base_lines = new List<Line>();
      base_lines.Add(new Line(0.0f, 0.0f, +1.0f, +0.0f));
      base_lines.Add(new Line(1.0f, 0.0f, +0.0f, +1.0f));
      base_lines.Add(new Line(1.0f, 1.0f, -1.0f, -0.0f));
      base_lines.Add(new Line(0.0f, 1.0f, -0.0f, -1.0f));

      for (int i = 0; i < base_lines.Count - 1; ++i)
      {
        for (int j = i + 1; j < base_lines.Count; ++j)
        {
          CalculateAndAdd(base_lines[i], base_lines[j]);
        }
      }

      #endregion Create base lines.

      #region Create main lines.

      for (int i = 0; i < linesCount; ++i)
      {
        var iLine = Line.CreateRandom();

        #region Cross with base lines.

        foreach (var jLine in base_lines)
        {
          CalculateAndAdd(iLine, jLine);
        }
        iLine.ts.Sort((itemLeft, itemRight) => Math.Sign(itemLeft.offset - itemRight.offset));
        iLine.RemoveBorderParts();

        #endregion Cross with base lines.

        #region Cross with main lines.

        foreach (var jLine in this.lines)
        {
          CalculateAndAdd(iLine, jLine, true);
        }

        #endregion Cross with main lines.

        this.lines.Add(iLine);
      }

      #endregion Create main lines.

      this.lines.AddRange(base_lines);

      foreach (var line in this.lines)
      {
        line.ts.Sort((itemLeft, itemRight) => Math.Sign(itemLeft.offset - itemRight.offset));
      }

      #endregion Create map.

      #region Create entities.

      Random random = new Random(0);
      this.entities.Clear();
      for (int i = 0; i < entitiesCount; i++)
      {
        this.entities.Add(new Entity()
        {
          brush = new SolidBrush(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256))),
          line = this.lines[Program.Random.Next(this.lines.Count)],
          speed = entitiesSpeed,
        });
      }

      #endregion Create entities.
    }

    private static void CalculateAndAdd(Line iLine, Line jLine, bool useCondition = false)
    {
      float t = iLine.vx * jLine.vy - jLine.vx * iLine.vy;
      if (t != 0)
      {
        float jipx = (jLine.px - iLine.px);
        float jipy = (jLine.py - iLine.py);
        float coef = 1 / t;
        float iT = (jipx * jLine.vy - jipy * jLine.vx) * coef;
        float jT = (jipx * iLine.vy - jipy * iLine.vx) * coef;
        if (!useCondition || (iLine.ts[0].offset <= iT && iT <= iLine.ts[1].offset && jLine.ts[0].offset <= jT && jT <= jLine.ts[1].offset))
        {
          iLine.ts.Add(new Line.Cross() { offset = iT, line = jLine });
          jLine.ts.Add(new Line.Cross() { offset = jT, line = iLine });
        }
      }
    }
  }
}
