using System;
using System.Collections.Generic;
using System.Drawing;

namespace MoveInCells
{
    public class Entities
    {
        private const int cellShift = 6;
        private const int cellSize = 1 << cellShift; // <- 32.
		private const int areaShift = 10;
		private const int areaSize = 1 << areaShift; // <- 1024
        private const int scoreAreaSize = cellSize;

        private readonly int entitiesCount = 10;

        private readonly float minRadius = 10.0f;
        private readonly float maxRadius = 10.0f;
        private readonly float minV = 0.1f;
        private readonly float maxV = 10.0f;
        private readonly int statePercent = 50;
		private readonly int movePercent = 5;

        private readonly List<Entity> entities = new List<Entity>();
        private List<Entity>[,] cells = null;
        private Heap heap = new Heap();

		private readonly int mainEntityId = 0;

        public float MaxScore;
        private readonly float timeInterval = 0.5f;
        private readonly Font font = new Font(FontFamily.Families[0], 14);

        private void RecalculateMinTime(Entity entity)
        {
            entity.T = float.PositiveInfinity;
            this.RecalculateMinTime0(entity);
            // this.RecalculateMinTime1(entity);
        }

        private void RecalculateMinTime0 (Entity entity)
		{
			if (entity.VX != 0) {
				int ix = entity.VX > 0 ? entity.i + 1 : entity.i;
				ix = ix << cellShift;
				if (entity.VX < 0 && ix >= entity.X) {
					ix -= cellSize;
				}

				float t = (ix - entity.X) / entity.VX;
				if(entity.T > t)
				{
					entity.T = t;
					entity.Event = 0;
				}
			}

			if (entity.VY != 0) {
            	int jy = entity.VY > 0 ? entity.j + 1 : entity.j;
            	jy = jy << cellShift;
            	if (entity.VY < 0 && jy >= entity.Y) {
    	            jy -= cellSize;
				}

				float t = (jy - entity.Y) / entity.VY;
				if(entity.T > t)
				{
					entity.T = t;
					entity.Event = 0;
				}
			}
        }

        private void RecalculateMinTime1(Entity entity)
        {
            int imin = Math.Max(0, entity.i - 1);
            int imax = Math.Min(entity.i + 1, this.cells.GetLength(0) - 1);
            int jmin = Math.Max(0, entity.j - 1);
            int jmax = Math.Min(entity.j + 1, this.cells.GetLength(1) - 1);
            for (int i = imin; i <= imax; ++i)
            {
                for (int j = jmin; j <= jmax; ++j)
                {
                    for (int k = 0; k < cells[i, j].Count; ++k)
                    {
                        if (entity != cells[i, j][k])
                        {
                            this.RecalculateMinTime10(entity, cells[i, j][k]);
                        }
                    }
                }
            }
        }

        private void RecalculateMinTime10(Entity entity, Entity entityNext) // t_chast(ic,jc)
        {
            float x = entityNext.X - entity.X;
            float y = entityNext.Y - entity.Y;
            float vx = entityNext.VX - entity.VX;
            float vy = entityNext.VY - entity.VY;
            float L = entityNext.R + entity.R;
            float A = vx * vx + vy * vy;
            float B = -(x * vx + y * vy);
            float C = x * x + y * y;

            float D = B * B - A * (C - L * L);

            float t1 = (float)(B - Math.Sqrt(D)) / A;
            float t2 = (float)(B + Math.Sqrt(D)) / A;

            if (entity.T > t1)
            {
                entity.T = t1;
                entity.Event = 1;
            }
            if (entityNext.T > t2)
            {
                entityNext.T = t2;
                entityNext.Event = 1;
            }
        }

        // procedure t_chast (ic, jc : longint);
        //double test = 1e20;
        //bool ind_sosed = false;
        //bool ind_tip = entity.tip == 2 && entity_2.tip == 2;
        //if (ind_tip)
        //    if (entity.k_sosedej > 0)
        //    {
        //        int k = 1;
        //        while (entity.sosedi[k] != entity_2 && k <= entity.k_sosedej)
        //            ++k;
        //        if (k <= entity.k_sosedej)
        //            ind_sosed = true;
        //    }
        //if (ind_sosed)
        //    t_sosedi(ic, jc, test);
        //else
        //    t_not_sosedi(ic, jc, test);
        //    new_times(ic, jc, test);

        private void RaiseEvent(Entity entity)
        {
            switch (entity.Event)
            {
                case 0: this.RaiseEvent0(entity); break;
                case 1: this.RaiseEvent1(entity); break;
            }
        }

        private void RaiseEvent0(Entity entity)
        {
            this.cells[entity.i, entity.j].Remove(entity);
            entity.i = (int)entity.X >> cellShift;
            if (entity.i < 0)
            {
                entity.X = areaSize;
            }
            if (entity.i >= this.cells.GetLength(0))
            {
                entity.X = 0;
            }
            entity.i = (int)entity.X >> cellShift;

            entity.j = (int)entity.Y >> cellShift;
            if (entity.j < 0)
            {
                entity.Y = areaSize;
            }
            if (entity.j >= this.cells.GetLength(1))
            {
                entity.Y = 0;
            }
            entity.j = (int)entity.Y >> cellShift;

            this.cells[entity.i, entity.j].Add(entity);

            this.AfterEvent(entity);
        }

        private void RaiseEvent1(Entity entity)
        {
            Entity entityNext = entity.Next;
            if (entity == entityNext.Next && entity.Event == 0)
            {
                throw new NotImplementedException();
            }
            this.AfterEvent(entity);
            this.AfterEvent(entityNext);
        }

        private void AfterEvent(Entity entity)
        {
            this.RecalculateMinTime(entity);
            this.heap.Recalculate(entity.Id);
        }


        private void RandomEntityV(Entity entity)
        {
            float v = Helper.RandomFloat(this.minV, this.maxV);
            float a = Helper.RandomFloat(0, 2 * (float)Math.PI);
			entity.SetV(v, a);
        }
        private void RandomMove()
        {
            if (Helper.Random.Next(100) < this.movePercent)
            {
                int i = Helper.Random.Next(this.entitiesCount);
                Entity entity = this.entities[i];
                if (i != this.mainEntityId && entity.State != State.Freeze)
                {
                    this.RandomEntityV(entity);
                    this.AfterEvent(entity);
                }
            }
        }

		private delegate void StateToStateEvent(Entity entity, Entity entityNext, ref bool isRun);
		private void FreezeToRunEvent (Entity entity, Entity entityNext, ref bool isRun)
		{
			if (entityNext.State == State.Run) {
				float d = Helper.ExtDistance (entity, entityNext);
				if (d < scoreAreaSize) {
					isRun = false;
				}
			}
		}
		private void RunToCatchEvent (Entity entity, Entity entityNext, ref bool isRun)
		{
			if (entityNext.State == State.Catch) {
				float d = Helper.ExtDistance(entity, entityNext);
				if (d <= 0) {
					entity.State = State.Freeze;
					// TODO: To freeze or not to freeze?
					//entity.SetV(0, entity.A);
					//this.AfterEvent(entity);

					entityNext.State = State.Run;

					entity.Score *= 0.5f;
					//entityNext.Score += entity.Score;

					isRun = false;
				}
				else if (d <= scoreAreaSize) {
					entity.Score += (scoreAreaSize - d) / d; // TODO: Find formula without dividing.
				}
			}
		}
		private void CatchToRunEvent (Entity entity, Entity entityNext, ref bool isRun)
		{
			if (entityNext.State == State.Run && entity.Id != this.mainEntityId) {
				float d = Helper.ExtDistance(entity, entityNext);
				if(entity.Near == null) {
					entity.Near = entityNext;
				}
				else {
					float dn = Helper.ExtDistance(entity, entity.Near);
					if (d < dn) {
						entity.Near = entityNext;
					}
				}
				if(entity.Near != null) {
					float v = entity.V;
					if( v == 0)
					{
						v = Helper.RandomFloat(this.minV, this.maxV);
					}
					float a = (float)Math.Atan2(entity.Near.Y - entity.Y, entity.Near.X - entity.X) - entity.A;
					a = Math.Min(a, 0.1f);
					a = Math.Max(a, -0.1f);
					entity.SetV(v, entity.A + a);
					this.AfterEvent(entity);
				}
			}
		}
		private bool NearEntitiesEvent (Entity entity, StateToStateEvent stateToStateEvent)
		{
			// TODO: Calculate right min, max by using scoreAreaSize and spherical world.
			int imin = Math.Max (0, entity.i - 1);
			int imax = Math.Min (entity.i + 1, this.cells.GetLength (0) - 1);
			int jmin = Math.Max (0, entity.j - 1);
			int jmax = Math.Min (entity.j + 1, this.cells.GetLength (1) - 1);
			bool isRun = true;
			for (int i = imin; i <= imax && isRun; ++i) {
				for (int j = jmin; j <= jmax && isRun; ++j) {
					for (int k = 0; k < cells [i, j].Count && isRun; ++k) {
						stateToStateEvent (entity, cells [i, j] [k], ref isRun);
					}
				}
			}
			return isRun;
		}
		private void UpdateScore()
        {
            this.MaxScore = 0;
            foreach (Entity entity in this.entities)
            {
				switch(entity.State) {
				case State.Run:
					this.NearEntitiesEvent(entity, this.RunToCatchEvent);
					break;
				case State.Freeze:
					if (this.NearEntitiesEvent (entity, this.FreezeToRunEvent)) {
                        entity.State = State.Catch;
					}
					break;
				case State.Catch:
					this.NearEntitiesEvent(entity, this.CatchToRunEvent);
					break;
				}

                this.MaxScore = Math.Max(this.MaxScore, entity.Score);
            }
        }

		private void UpdateTempCoordinates ()
		{
            // Temp coordinates calculation.
            for (int i = 0; i < this.entities.Count; ++i)
            {
				this.entities[i].XT = this.entities[i].X - this.entities[this.mainEntityId].X;
				this.entities[i].YT = this.entities[i].Y - this.entities[this.mainEntityId].Y;
            }

            // Temp coordinates recalclation.
			int maxX = areaSize >> 1;
			int maxY = areaSize >> 1;
            for (int i = 0; i < this.entities.Count; ++i)
            {
                if (this.entities[i].XT < -maxX)
                    this.entities[i].XT += areaSize;
                else if (this.entities[i].XT > maxX)
                    this.entities[i].XT -= areaSize;

                if (this.entities[i].YT < -maxY)
                    this.entities[i].YT += areaSize;
                else if (this.entities[i].YT > maxY)
                    this.entities[i].YT -= areaSize;
            }

            // Temp coordinates rotation.
            double rg_rad = 1.5f * (float)Math.PI - this.entities[this.mainEntityId].A;
            for (int i = 0; i < this.entities.Count; ++i)
            {
                float x = (float)(this.entities[i].XT * Math.Cos(rg_rad) - this.entities[i].YT * Math.Sin(rg_rad));
                float y = (float)(this.entities[i].XT * Math.Sin(rg_rad) + this.entities[i].YT * Math.Cos(rg_rad));
                this.entities[i].XT = x;
                this.entities[i].YT = y;
            }

            // TODO: Recalculate rotation?
		}

        public Entities()
        {
			int length = (areaSize >> cellShift) + 1;
            this.cells = new List<Entity>[length, length];
            for (int i = 0; i < this.cells.GetLength(0); ++i) {
                for (int j = 0; j < this.cells.GetLength(1); ++j) {
                    this.cells[i, j] = new List<Entity>();
                }
            }
        }

        public void Create()
        {
            this.entities.Clear();
            for (int i = 0; i < entitiesCount; i++)
            {
                float r = Helper.RandomFloat(this.minRadius, this.maxRadius);
                float x = Helper.RandomFloat(r, areaSize - r);
                float y = Helper.RandomFloat(r, areaSize - r);
                Entity entity = new Entity() { Id = i, X = x, Y = y, R = r };

				entity.State = Helper.Random.Next(100) < this.statePercent ? State.Catch : State.Run;

                entity.i = (int)entity.X >> cellShift;
                entity.j = (int)entity.Y >> cellShift;

				this.entities.Add(entity);
                this.cells[entity.i, entity.j].Add(entity);
                this.RecalculateMinTime(entity);
            }

            this.heap.StartFill(this.entities.ToArray());
        }

        public void Update()
        {
            if (this.heap.isBuild) {
                float time = this.timeInterval;
                while (time > 0) {
                    Entity entity = this.heap.GetFirst();
                    while (entity.T <= 0) {
						this.AfterEvent(entity);
                        entity = this.heap.GetFirst();
                    }

                    float t = Math.Min(entity.T, time);
					foreach(Entity entityTemp in this.entities) {
                        entityTemp.Move(t);
                    }
                    time -= t;

                    this.RaiseEvent(entity);
                }

                this.UpdateScore();

                this.RandomMove();
            }
        }

        public void Draw(Graphics g, float width, float height)
        {
			this.UpdateTempCoordinates();
            g.TranslateTransform(width / 2, height / 2);

			#region For testing.
			System.Drawing.Drawing2D.Matrix matrix = g.Transform;
			g.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, 0, 0);
			float x = this.entities[this.mainEntityId].X;
			float y = this.entities[this.mainEntityId].Y;
			float angle = this.entities[this.mainEntityId].A * 180 / (float)Math.PI;
			g.TranslateTransform(width / 2, height / 2);
			g.RotateTransform(-90 - angle);
			g.TranslateTransform(-x, -y);
            for (int i = 0; i < this.cells.GetLength(0); i++) {
				for (int j = 0; j < this.cells.GetLength(1); j++) {
//					if (this.cells[i, j].Count > 0) {
//						g.FillRectangle(Brushes.Yellow, i << cellShift, j << cellShift, cellSize, cellSize);
//					}
					g.DrawRectangle(Pens.Silver, i << cellShift, j << cellShift, cellSize, cellSize);
				}
			}
			g.Transform = matrix;
			#endregion For testing.
            
            foreach (Entity entity in this.entities)
            {
				Brush brush = Brushes.Transparent;
                switch (entity.State)
                {
                    case State.Run: brush = Brushes.White; break;
                    case State.Freeze: brush = Brushes.Silver; break;
                    case State.Catch: brush = Brushes.Black; break;
                }
				#region For testing.
				if(entity.State == State.Run)
				{
					float maxScore = Math.Max(this.MaxScore, 1);
					brush = new SolidBrush(Color.FromArgb(Math.Min((int)(255 * entity.Score / maxScore), 255), Color.Red));
				}
				#endregion For testing.
                g.FillEllipse(brush, entity.XT - entity.R, entity.YT - entity.R, 2 * entity.R, 2 * entity.R);
                g.DrawEllipse(Pens.Black, entity.XT - entity.R, entity.YT - entity.R, 2 * entity.R, 2 * entity.R);
                g.DrawString(entity.Score.ToString(), font, Brushes.Black, entity.XT + entity.R, entity.YT + entity.R);
				#region For testing.
				if(entity.V > 0)
				{
					float a = 1.5f * (float)Math.PI + (entity.A - this.entities[this.mainEntityId].A);
					float vx = entity.V * entity.T * (float)Math.Cos(a);
					float vy = entity.V * entity.T * (float)Math.Sin(a);
                	g.DrawLine(Pens.Black, entity.XT, entity.YT, entity.XT + vx, entity.YT + vy);
				}
				#endregion For testing.
				#region For testing.
//                if (entity.State != State.Run)
//                {
//                    float scoreRadius = entity.R + scoreAreaSize;
//                    g.DrawEllipse(Pens.Blue, entity.XT - scoreRadius, entity.YT - scoreRadius, 2 * scoreRadius, 2 * scoreRadius);
//                }
				#endregion For testing.
            }
		}
    

		public Entity GetMainEntity ()
		{
			return this.entities[this.mainEntityId];
		}
		public void UpdateMainEntity ()
		{
			this.AfterEvent(this.entities[this.mainEntityId]);
		}
	}
}
