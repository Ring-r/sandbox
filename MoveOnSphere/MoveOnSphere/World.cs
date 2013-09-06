using System;
using System.Drawing;

namespace MoveOnSphere
{
    public class World
    {
        public static float R;
        public static readonly Vector Vector = new Vector() { z = 1 };

        private const int entitiesCount = 100;
        private readonly Entity[] entities = new Entity[entitiesCount];
        private readonly int mainEntityId = 0;

        private const int moveProcent = 50;
        private const float minAngle = 0.01f;
        private const float maxAngle = 0.005f;

        private const int minS = 10;
        private const int maxS = 30;
        private const int minT = 50;
        private const int maxT = 255;

        private void RandomEntityChange()
        {
            if (Helper.Random.Next(100) < moveProcent)
            {
				int i = Helper.Random.Next(entitiesCount);
				// i != this.mainEntityId && 
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
        }

        public void Draw(Graphics g, int width, int height)
        {
            // g.DrawEllipse(Pens.Silver, -World.R, -World.R, 2 * World.R, 2 * World.R);

            int bx = width >> 1;
            int by = height >> 1;

            Entity mainEntity = this.entities[this.mainEntityId];
            Quaternion q = new Quaternion(mainEntity.v, World.Vector);
            foreach (Entity entity in this.entities)
            {
                if (-bx <= entity.v.x && entity.v.x <= bx && -by <= entity.v.y && entity.v.y <= by)
                {
					try
					{
                    float x = entity.v.x;
                    float y = entity.v.y;
                    float z = entity.v.z;
                    q.Convert(ref x, ref y, ref z);

                    Brush brush = new SolidBrush(Color.FromArgb((int)((maxT - minT) * (z + 1) / 2) + minT, Color.Black));

                    float s = (maxS - minS) * (z + 1) / 2 + minS;

                    g.FillEllipse(brush, x * World.R - s / 2, y * World.R - s / 2, s, s);
					}
					catch{}
                }
            }
        }

        public void Update ()
		{
			this.RandomEntityChange ();

			foreach (Entity entity in this.entities) {
				entity.Move ();
			}

			for (int i = 0; i < entitiesCount - 1; ++i) {
				for (int j = i + 1; j < entitiesCount; ++j) {
					float x = (this.entities[j].v.x - this.entities[i].v.x) * World.R;
					float y = (this.entities[j].v.y - this.entities[i].v.y) * World.R;
					float z = (this.entities[j].v.z - this.entities[i].v.z) * World.R;
					float d = (float)Math.Sqrt(x * x + y * y + z * z);
					float dd = maxS - d;
					if(dd > 0) {
						d = 1 / d;
						x *= d;
						y *= d;
						z *= d;
						dd *= 0.5f;
						float X, Y, Z, K;
						X = this.entities[i].v.x * World.R - dd * x;
						Y = this.entities[i].v.y * World.R - dd * y;
						Z = this.entities[i].v.z * World.R - dd * z;
						K = 1 / (float)Math.Sqrt(X * X + Y * Y + Z * Z);
						this.entities[i].v.x = X * K;
						this.entities[i].v.y = Y * K;
						this.entities[i].v.z = Z * K;

						X = this.entities[j].v.x * World.R + dd * x;
						Y = this.entities[j].v.y * World.R + dd * y;
						Z = this.entities[j].v.z * World.R + dd * z;
						K = 1 / (float)Math.Sqrt(X * X + Y * Y + Z * Z);
						this.entities[j].v.x = X * K;
						this.entities[j].v.y = Y * K;
						this.entities[j].v.z = Z * K;
					}
				}
			}
        }
    
		public Entity GetMainEntity()
        {
            return this.entities[this.mainEntityId];
        }
	}
}

