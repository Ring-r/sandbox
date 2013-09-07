using System;
using System.Drawing;

namespace MoveOnSphere
{
    public class World
    {
        public static float R;
		public static readonly Vector VectorX = new Vector() { x = 1 };
        public static readonly Vector VectorZ = new Vector() { z = 1 };

        private const int entitiesCount = 100;
        private readonly Entity[] entities = new Entity[entitiesCount];
        private readonly int mainEntityId = 0;

        private const int moveProcent = 50;
        private const float minAngle = 0.001f;
        private const float maxAngle = 0.0005f;

        private const int minS = 10;
        private const int maxS = 30;
        private const int minT = 50;
        private const int maxT = 255;

        private void RandomEntityChange()
        {
            if (Helper.Random.Next(100) < moveProcent)
            {
				int i = Helper.Random.Next(entitiesCount);
                if (i != this.mainEntityId && Helper.Random.Next(100) < 50)
                {
                    this.entities[i].rotateAngle = Helper.RandomFloat(minAngle, maxAngle);
                }
                else
                {
                    this.entities[i].moveAngle = Helper.RandomFloat(minAngle, maxAngle);
                }
            }
        }

		private void CollisionEvent ()
		{
			Vector vji = new Vector(); // Global?
			for (int i = 0; i < entitiesCount - 1; ++i) {
				for (int j = i + 1; j < entitiesCount; ++j) {
					vji.FillAsDistinction(this.entities[i].v, this.entities[j].v);
					float length = vji.GetLength();
					float lengthCollision = 0.5f * (maxS + maxS) / World.R - length;
					if(lengthCollision > 0 && length > 0) {
						vji.Multiply(0.5f * lengthCollision / length);

						this.entities[j].v.Add(vji); this.entities[j].v.Normilize();
						this.entities[j].Recalculate();

						this.entities[i].v.Deduct(vji); this.entities[i].v.Normilize();
						this.entities[i].Recalculate();
					}
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
			Entity mainEntity = this.entities[this.mainEntityId];
			float a = Vector.ScalarProduction(mainEntity.v, mainEntity.v_);
        }

        public void Draw(Graphics g, int width, int height)
        {
            // g.DrawEllipse(Pens.Silver, -World.R, -World.R, 2 * World.R, 2 * World.R);

            int bx = width >> 1;
            int by = height >> 1;

            Entity mainEntity = this.entities[this.mainEntityId];

			Vector v = new Vector(); // TODO: Global?

			TransformationAsQuaternion qMove = new TransformationAsQuaternion(); // TODO: Global?
			qMove.Fill(mainEntity.v, World.VectorZ);

			v.Fill(mainEntity.v_); qMove.Transform(v); v.Normilize();
			TransformationAsQuaternion qRotate = new TransformationAsQuaternion(); // TODO: Global?
			qRotate.Fill(World.VectorZ, v, World.VectorX);

			v.Fill(mainEntity.v_);
            qMove.Transform(v);
			qRotate.Transform(v);
			g.DrawLine(Pens.Black, 0, 0, v.x * World.R, v.y * World.R);

            foreach (Entity entity in this.entities)
            {
                if (-bx <= entity.v.x && entity.v.x <= bx && -by <= entity.v.y && entity.v.y <= by)
                {
					try
					{
						v.Fill(entity.v);
                    	qMove.Transform(v);
						qRotate.Transform(v);

                    	Brush brush = new SolidBrush(Color.FromArgb((int)(0.5f * (maxT - minT) * (v.z + 1) + minT), Color.Black));

                    	float s = 0.5f * (maxS - minS) * (v.z + 1) + minS;

                    	g.FillEllipse(brush, v.x * World.R - 0.5f * s, v.y * World.R - 0.5f * s, s, s);

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

			this.CollisionEvent();
        }
    
		public Entity GetMainEntity()
        {
            return this.entities[this.mainEntityId];
        }
	}
}

