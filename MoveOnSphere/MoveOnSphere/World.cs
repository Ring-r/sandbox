using System;
using System.Drawing;

namespace MoveOnSphere
{
    public class World
    {
        public static float R;
        public static readonly Vector Vector = new Vector() { z = 1 };

        private const int entitiesCount = 30;
        private readonly Entity[] entities = new Entity[entitiesCount];
        private readonly int mainEntityId = 0;

        private const int moveProcent = 0;
        private const float minAngle = 0.001f;
        private const float maxAngle = 0.005f;

        private const int minS = 5;
        private const int maxS = 30;
        private const int minT = 50;
        private const int maxT = 255;

        private void RandomEntityChange()
        {
            if (Helper.Random.Next(100) < moveProcent)
            {
                int i = Helper.Random.Next(entitiesCount);
                if (Helper.Random.Next(100) < 50)
                {
                    this.entities[i].rotateAngle = Helper.RandomFloat(minAngle, maxAngle);
                }
                else
                {
                    this.entities[i].moveAngle = Helper.RandomFloat(minAngle, maxAngle);
                }
            }
        }

        public void Create()
        {
            for (int i = 0; i < entitiesCount; ++i)
            {
                this.entities[i] = new Entity();
                this.entities[i].RandomFill();
            }
            this.entities[this.mainEntityId].moveAngle = maxAngle; // TODO: (R) For testing.
        }

        public void Draw(Graphics g, int width, int height)
        {
            g.DrawEllipse(Pens.Silver, -World.R, -World.R, 2 * World.R, 2 * World.R);
            g.DrawLine(Pens.Silver, -World.R, 0, World.R, 0);
            g.DrawLine(Pens.Silver, 0, -World.R, 0, World.R);

            int bx = width >> 1;
            int by = height >> 1;

            Entity mainEntity = this.entities[this.mainEntityId];
            Quaternion q = new Quaternion(mainEntity.v, World.Vector);
            int i = 0; // TODO: For testing.
            foreach (Entity entity in this.entities)
            {
                if (-bx <= entity.v.x && entity.v.x <= bx && -by <= entity.v.y && entity.v.y <= by)
                {
                    float x = entity.v.x;
                    float y = entity.v.y;
                    float z = entity.v.z;
                    q.Convert(ref x, ref y, ref z);

                    Brush brush = new SolidBrush(Color.FromArgb((int)((maxT - minT) * (z + 1) / 2) + minT, Color.Black));
                    if (i == 0)
                    {
                        i = 1;
                        brush = Brushes.Red;
                    }

                    float s = (maxS - minS) * (z + 1) / 2 + minS;

                    g.FillEllipse(brush, x * World.R - s / 2, y * World.R - s / 2, s, s);
                }
            }
        }

        public void Update()
        {
            this.RandomEntityChange();

            foreach (Entity entity in this.entities)
            {
                entity.Move();
            }
        }
    }
}

