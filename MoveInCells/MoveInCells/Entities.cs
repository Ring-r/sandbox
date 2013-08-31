using System;
using System.Collections.Generic;
using System.Drawing;

namespace MoveInCells
{
    public class Entities
    {
        private readonly Random random = new Random();

        private const int shift = 6; // cellSize <- 32.
        private const int cellSize = 1 << shift;
        private const int mAreaSize = cellSize;

        private readonly int entitiesCount = 10;

        private float maxX;
        private float maxY;

        private readonly float minRadius = 5.0f;
        private readonly float maxRadius = 20.0f;
        private readonly float minV = 0.1f;
        private readonly float maxV = 10.0f;
        private readonly int dPercent = 50;

        private readonly List<Entity> entities = new List<Entity>();
        private List<Entity>[,] cells = null;
        private Heap heap = new Heap();

		private readonly int mainEntityId = 0;

        public float Max;
        private readonly float timeInterval = 0.5f;
        private readonly Font font = new Font(FontFamily.Families[0], 14);

        private float NextFloat(float min, float max)
        {
            return (float)(min + (max - min) * this.random.NextDouble());
        }

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
				ix = ix << shift;
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
            	jy = jy << shift;
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
            entity.i = (int)entity.X >> shift;
            if (entity.i < 0)
            {
                entity.X = this.maxX;
            }
            if (entity.i >= this.cells.GetLength(0))
            {
                entity.X = 0;
            }
            entity.i = (int)entity.X >> shift;

            entity.j = (int)entity.Y >> shift;
            if (entity.j < 0)
            {
                entity.Y = this.maxY;
            }
            if (entity.j >= this.cells.GetLength(1))
            {
                entity.Y = 0;
            }
            entity.j = (int)entity.Y >> shift;

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
            float v = this.NextFloat(this.minV, this.maxV);
            float a = this.NextFloat(0, 2 * (float)Math.PI);
			entity.SetV(v, a);
        }
        private void RandomMove()
        {
            if (this.random.Next(100) < 10)
            {
                int i = this.random.Next(this.entitiesCount);
                Entity entity = this.entities[i];
                if (i != this.mainEntityId && entity.State != State.Freeze)
                {
                    this.RandomEntityV(entity);
                    this.AfterEvent(entity);
                }
            }
        }
        private void UpdateMana()
        {
            this.Max = 0;
            foreach (Entity entity in this.entities)
            {
                if (entity.State == State.Run)
                {
                    int imin = Math.Max(0, entity.i - 2);
                    int imax = Math.Min(entity.i + 2, this.cells.GetLength(0) - 1);
                    int jmin = Math.Max(0, entity.j - 2);
                    int jmax = Math.Min(entity.j + 2, this.cells.GetLength(1) - 1);
                    for (int i = imin; i <= imax && entity.State == State.Run; ++i)
                    {
                        for (int j = jmin; j <= jmax && entity.State == State.Run; ++j)
                        {
                            for (int k = 0; k < cells[i, j].Count && entity.State == State.Run; ++k)
                            {
                                Entity entityTemp = cells[i, j][k];

                                if (entityTemp.State == State.Catch)
                                {
                                    float x = entityTemp.X - entity.X;
                                    float y = entityTemp.Y - entity.Y;
                                    float d = (float)Math.Sqrt(x * x + y * y) - entityTemp.R - entity.R;
                                    if (0 < d && d < mAreaSize)
                                    {
                                        entity.M += (mAreaSize - d) / d;
                                    }
                                    if (d <= 0)
                                    {
                                        entity.M *= 0.5f;
                                        entity.State = State.Freeze;
                                        entity.VX = 0;
                                        entity.VY = 0;
                                        this.AfterEvent(entity);

                                        entityTemp.M += entity.M;
                                        entityTemp.State = State.Run;
                                    }
                                }
                            }
                        }
                    }
                }

                if (entity.State == State.Freeze)
                {
                    int imin = Math.Max(0, entity.i - 2);
                    int imax = Math.Min(entity.i + 2, this.cells.GetLength(0) - 1);
                    int jmin = Math.Max(0, entity.j - 2);
                    int jmax = Math.Min(entity.j + 2, this.cells.GetLength(1) - 1);
                    bool unfreeze = true;
                    for (int i = imin; i <= imax && unfreeze; ++i)
                    {
                        for (int j = jmin; j <= jmax && unfreeze; ++j)
                        {
                            for (int k = 0; k < cells[i, j].Count && unfreeze; ++k)
                            {
                                Entity entityTemp = cells[i, j][k];

                                if (entityTemp.State == State.Run)
                                {
                                    float x = entityTemp.X - entity.X;
                                    float y = entityTemp.Y - entity.Y;
                                    float d = (float)Math.Sqrt(x * x + y * y) - entityTemp.R - entity.R;
                                    if (d < mAreaSize)
                                    {
                                        unfreeze = false;
                                    }
                                }
                            }
                        }
                    }
                    if (unfreeze)
                    {
                        entity.State = State.Catch;
                        this.RandomEntityV(entity);
                        this.AfterEvent(entity);
                    }
                }


                this.Max = Math.Max(this.Max, entity.M);
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
			float maxX = 0.5f * this.maxX;
			float maxY = 0.5f * this.maxY;
            for (int i = 0; i < this.entities.Count; ++i)
            {
                if (this.entities[i].XT < -maxX)
                    this.entities[i].XT += this.maxX;
                else if (this.entities[i].XT > maxX)
                    this.entities[i].XT -= this.maxX;

                if (this.entities[i].YT < -maxY)
                    this.entities[i].YT += this.maxY;
                else if (this.entities[i].YT > maxY)
                    this.entities[i].YT -= this.maxY;
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

        public void InitClientSize(float maxX, float maxY)
        {
            this.maxX = maxX;
            this.maxY = maxY;

            this.cells = new List<Entity>[((int)this.maxX >> shift) + 1, ((int)this.maxY >> shift) + 1];
            for (int i = 0; i < this.cells.GetLength(0); ++i)
            {
                for (int j = 0; j < this.cells.GetLength(1); ++j)
                {
                    this.cells[i, j] = new List<Entity>();
                }
            }
        }

        public void Create()
        {
            this.entities.Clear();
            for (int i = 0; i < entitiesCount; i++)
            {
                float r = this.NextFloat(this.minRadius, this.maxRadius);
                float x = this.NextFloat(r, this.maxX - r);
                float y = this.NextFloat(r, this.maxY - r);
                Entity entity = new Entity() { Id = i, X = x, Y = y, R = r };

				entity.State = this.random.Next(100) < this.dPercent ? State.Catch : State.Run;

                entity.i = (int)entity.X >> shift;
                entity.j = (int)entity.Y >> shift;

				this.entities.Add(entity);
                this.cells[entity.i, entity.j].Add(entity);
                this.RecalculateMinTime(entity);
            }

            this.heap.StartFill(this.entities.ToArray());
        }

        public void Update()
        {
            if (this.heap.isBuild)
            {
                float time = this.timeInterval;
                while (time > 0)
                {
                    Entity entity = this.heap.GetFirst();
                    while (entity.T <= 0)
                    {
                        this.RecalculateMinTime(entity);
                        this.heap.Recalculate(entity.Id);
                        entity = this.heap.GetFirst();
                    }

                    float t = Math.Min(entity.T, time);
                    for (int i = 0; i < this.entities.Count; ++i)
                    {
                        this.entities[i].Move(t);
                    }
                    time -= t;

                    this.RaiseEvent(entity);
                }

                this.UpdateMana();

                this.RandomMove();
            }
        }

        public void Draw(Graphics g)
        {
			this.UpdateTempCoordinates();
            g.TranslateTransform(this.maxX / 2, this.maxY / 2);

            //for (int i = 0; i < this.cells.GetLength(0); i++)
            //{
              //  for (int j = 0; j < this.cells.GetLength(1); j++)
                //{
                    //if (this.cells[i, j].Count > 0)
                    //{
                    //    g.FillRectangle(Brushes.Yellow, i << cellSize, j << cellSize, this.cellSize_, this.cellSize_);
                    //}
                  //  g.DrawRectangle(Pens.Silver, i << shift, j << shift, cellSize, cellSize);
                //}
            //}

            float d;
            Brush brush;
            foreach (Entity entity in this.entities)
            {
                d = 2 * entity.R;
                switch (entity.State)
                {
                    case State.Run: brush = Brushes.White; break;
                    case State.Freeze: brush = Brushes.Silver; break;
                    case State.Catch: brush = Brushes.Black; break;
                    default: brush = Brushes.Transparent; break;
                }
                g.FillEllipse(brush, entity.XT - entity.R, entity.YT - entity.R, d, d);
                g.DrawEllipse(Pens.Black, entity.XT - entity.R, entity.YT - entity.R, d, d);
                g.DrawString(entity.M.ToString(), font, Brushes.Black, entity.XT + entity.R, entity.YT + entity.R);
				if(entity.V > 0)
				{
					//float a = 1.5f * (float)Math.PI + (entity.A - this.entities[this.mainEntityId].A);
					//float vx = entity.V * entity.T * (float)Math.Cos(a);
					//float vy = entity.V * entity.T * (float)Math.Sin(a);
                	//g.DrawLine(Pens.Black, entity.XT, entity.YT, entity.XT + vx, entity.YT + vy);
				}
                if (entity.State != State.Run)
                {
                    d = entity.R + mAreaSize;
                    //g.DrawEllipse(Pens.Blue, entity.XT - d, entity.YT - d, 2 * d, 2 * d);
                }
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
