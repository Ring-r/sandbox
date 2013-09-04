using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiscreteEventSimulation
{
    public class Entities
    {
        private const int areaShift = 9;
        private const int areaSize = 1 << areaShift; // <- 512

        private const int cellShift = 5;
        private const int cellSize = 1 << cellShift; // <- 32.

        private const int cellsCount = (areaSize >> cellShift) + 1;
        private const int entitiesCount = 10;

        private const float minRadius = 3.0f;
        private const float maxRadius = 10.0f;
        private const float minV = 0.1f;
        private const float maxV = 3.0f;

        private const int cellEntityCount = 5;


        private readonly CellList[,] cells = new CellList[cellsCount, cellsCount];
        private readonly List<Entity> entities = new List<Entity>();
        private readonly Heap heap = new Heap();


        private void RecalculateMinTime(Entity entity)
        {
            entity.T = float.PositiveInfinity;
            this.RecalculateMinTime0(entity);
            // this.RecalculateMinTime1(entity);
        }
        private void RecalculateMinTime0(Entity entity)
        {
            if (entity.VX != 0)
            {
                int ix = entity.VX > 0 ? entity.i + 1 : entity.i;
                ix = ix << cellShift;
                float t = (ix - entity.X) / entity.VX;
                if (entity.T > t)
                {
                    entity.T = t;
                    entity.Event = entity.VX > 0 ? 2 : 1;
                }
            }

            if (entity.VY != 0)
            {
                int jy = entity.VY > 0 ? entity.j + 1 : entity.j;
                jy = jy << cellShift;
                float t = (jy - entity.Y) / entity.VY;
                if (entity.T > t)
                {
                    entity.T = t;
                    entity.Event = entity.VY > 0 ? 4 : 3;
                }
            }
        }
        private void RecalculateMinTime_1(Entity entity)
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
                            this.RecalculateMinTime_10(entity, cells[i, j][k]);
                        }
                    }
                }
            }
        }
        private void RecalculateMinTime_10(Entity entity, Entity entityNext) // t_chast(ic,jc)
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
                entity.Event = -1;
            }
            if (entityNext.T > t2)
            {
                entityNext.T = t2;
                entityNext.Event = -1;
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
                case 1: this.RaiseEventMinX(entity); break;
                case 2: this.RaiseEventMaxX(entity); break;
                case 3: this.RaiseEventMinY(entity); break;
                case 4: this.RaiseEventMaxY(entity); break;
                case -1: this.RaiseEvent_1(entity); break;
            }
        }
        private void RaiseEventMinX(Entity entity)
        {
            this.cells[entity.i, entity.j].Remove(entity);
            --entity.i;
            if (entity.i < 0)
            {
                entity.i = this.cells.GetLength(0) - 1;
                entity.X = areaSize;
            }
            //int i = (int)entity.X >> cellShift; // TODO: For testing.
            this.cells[entity.i, entity.j].Add(entity);
        }
        private void RaiseEventMaxX(Entity entity)
        {
            this.cells[entity.i, entity.j].Remove(entity);
            ++entity.i;
            if (entity.i >= this.cells.GetLength(0))
            {
                entity.i = 0;
                entity.X = 0;
            }
            //int i = (int)entity.X >> cellShift; // TODO: For testing.
            this.cells[entity.i, entity.j].Add(entity);
        }
        private void RaiseEventMinY(Entity entity)
        {
            this.cells[entity.i, entity.j].Remove(entity);
            --entity.j;
            if (entity.j < 0)
            {
                entity.j = this.cells.GetLength(1) - 1;
                entity.Y = areaSize;
            }
            //int j = (int)entity.Y >> cellShift; // TODO: For testing.
            this.cells[entity.i, entity.j].Add(entity);
        }
        private void RaiseEventMaxY(Entity entity)
        {
            this.cells[entity.i, entity.j].Remove(entity);
            ++entity.j;
            if (entity.j >= this.cells.GetLength(1))
            {
                entity.j = 0;
                entity.Y = 0;
            }
            //int j = (int)entity.Y >> cellShift; // TODO: For testing.
            this.cells[entity.i, entity.j].Add(entity);
        }

        private void RaiseEvent_1(Entity entity)
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


        public Entities()
        {
            for (int i = 0; i < this.cells.GetLength(0); ++i)
            {
                for (int j = 0; j < this.cells.GetLength(1); ++j)
                {
                    this.cells[i, j] = new CellList(cellEntityCount);
                }
            }
        }

        public void Create()
        {
            this.entities.Clear();
            for (int i = 0; i < entitiesCount; i++)
            {
                float r = Helper.RandomFloat(minRadius, maxRadius);
                float x = Helper.RandomFloat(r, areaSize - r);
                float y = Helper.RandomFloat(r, areaSize - r);
                Entity entity = new Entity() { Id = i, X = x, Y = y, R = r };

                entity.i = (int)entity.X >> cellShift;
                entity.j = (int)entity.Y >> cellShift;

                float v = Helper.RandomFloat(minV, maxV);
                float a = Helper.RandomFloat(0, 2 * (float)Math.PI);
                entity.SetV(v, a);

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
                Entity entity = this.heap.GetFirst();
                while (entity.T <= 0)
                {
                    this.RaiseEvent(entity);
                    this.AfterEvent(entity);
                    entity = this.heap.GetFirst();
                }

                entity.Move();
                //this.AfterEvent(entity); // TODO: For testing.
            }
        }

        public void Draw(Graphics g, float width, float height, Font font)
        {
            #region For testing.
            for (int i = 0; i < this.cells.GetLength(0); i++)
            {
                for (int j = 0; j < this.cells.GetLength(1); j++)
                {
                    if (this.cells[i, j].Count > 0)
                    {
                        g.FillRectangle(Brushes.Yellow, i << cellShift, j << cellShift, cellSize, cellSize);
                    }
                    g.DrawRectangle(Pens.Silver, i << cellShift, j << cellShift, cellSize, cellSize);
                }
            }
            #endregion For testing.

            foreach (Entity entity in this.entities)
            {
                g.FillEllipse(Brushes.Blue, entity.X - entity.R, entity.Y - entity.R, 2 * entity.R, 2 * entity.R);
                g.DrawEllipse(Pens.Black, entity.X - entity.R, entity.Y - entity.R, 2 * entity.R, 2 * entity.R);
                g.DrawString(entity.T.ToString(), font, Brushes.Black, entity.X + entity.R, entity.Y + entity.R);
                g.DrawLine(Pens.Black, entity.X, entity.Y, entity.X + entity.VX * entity.T, entity.Y + entity.VY * entity.T);
            }
        }
    }
}
