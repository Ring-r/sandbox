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
		private int aimEntityId = 0;

        private const int moveProcent = 100;
        private const float minAngle = 0.005f;
        private const float maxAngle = 0.003f;

        private const int minS = 10;
        private const int maxS = 30;
        private const int minT = 50;
        private const int maxT = 255;

		private const int isEndTimerMax = 100;
		private int isEndTimer = 0;
		private string isEndString = "";

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

						if((i == this.mainEntityId && j == this.aimEntityId) || (i == this.aimEntityId && j == this.mainEntityId))
						{
							this.isEndString = "Right!";
							this.isEndTimer = isEndTimerMax;
							this.CreateAim();
						}
						if(this.isEndTimer <= 0 && (i == this.mainEntityId && j != this.aimEntityId) || (i != this.aimEntityId && j == this.mainEntityId))
						{
							this.isEndString = "Loser!";
							this.isEndTimer = isEndTimerMax;
						}

					}
				}
			}
		}

		private void CreateAim ()
		{
			this.aimEntityId = Helper.Random.Next (entitiesCount);
			while (this.aimEntityId == this.mainEntityId) {
				this.aimEntityId = Helper.Random.Next (entitiesCount);
			}
		}

        public void Create ()
		{
			for (int i = 0; i < entitiesCount; ++i) {
				this.entities [i] = new Entity ();
				this.entities [i].RandomFill ();
			}
			this.CreateAim();
        }

        public void Draw (Graphics g, int width, int height)
		{
			// g.DrawEllipse(Pens.Silver, -World.R, -World.R, 2 * World.R, 2 * World.R);

			int bx = width >> 1;
			int by = height >> 1;

			Entity mainEntity = this.entities [this.mainEntityId];

			Vector v = new Vector (); // TODO: Global?

			TransformationAsQuaternion qMove = new TransformationAsQuaternion (); // TODO: Global?
			qMove.Fill (mainEntity.v, World.VectorZ);

			v.Fill (mainEntity.v_);
			qMove.Transform (v);
			v.Normilize ();
			// TODO: Work wrong.
			// TransformationAsQuaternion qRotate = new TransformationAsQuaternion(); // TODO: Global?
			// qRotate.Fill(World.VectorZ, v, World.VectorX);

			Vector direction = new Vector (); // TODO: Global?
			direction.FillAsVectorProduction (mainEntity.v_, mainEntity.v);
			direction.Normilize ();
			v.Fill (direction);
			qMove.Transform (v);
			// TODO: Work wrong. qRotate.Transform(v);
			g.DrawLine (Pens.Black, 0, 0, v.x * World.R, v.y * World.R);

			for (int i = 0; i< entitiesCount; ++i) {
				Entity entity = this.entities [i];
				if (-bx <= entity.v.x && entity.v.x <= bx && -by <= entity.v.y && entity.v.y <= by) {
					try {
						v.Fill (entity.v);
						qMove.Transform (v);
						// TODO: Work wrong. qRotate.Transform(v);

						Color color = i == this.aimEntityId ? Color.Green : Color.Black;
						Brush brush = new SolidBrush (Color.FromArgb ((int)(0.5f * (maxT - minT) * (v.z + 1) + minT), color));

						float s = 0.5f * (maxS - minS) * (v.z + 1) + minS;

						g.FillEllipse (brush, v.x * World.R - 0.5f * s, v.y * World.R - 0.5f * s, s, s);

					} catch {
					}
				}
			}

			if (this.isEndTimer <= 0) {
				this.isEndString = "";
			} else {
				--this.isEndTimer;
				Font font = new Font(FontFamily.Families[0], 36);
				SizeF size = g.MeasureString(this.isEndString, font);
				g.DrawString(this.isEndString, font, Brushes.DeepPink, -size.Width / 2, 0);
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

