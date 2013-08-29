using System;
using System.Collections.Generic;
using System.Drawing;

namespace MoveInCells
{
    public class Entities
    {
        private readonly Random random = new Random();

        private readonly int entitiesCount = 10;

        private float maxX;
        private float maxY;

        private readonly float minRadius = 5.0f;
        private readonly float maxRadius = 20.0f;
        private readonly float minStep = 0.1f;
        private readonly float maxStep = 1.0f;

        private readonly int cellSize = 6;
        private readonly float timeInterval = 1f;

        private readonly List<Entity> entities = new List<Entity>();
        private List<Entity>[,] cells = null;
        private Heap heap = new Heap();


        private float NextFloat(float min, float max)
        {
            return (float)(min + (max - min) * this.random.NextDouble());
        }

        private void RecalculateMinTime(Entity entity)
        {
            entity.T = float.PositiveInfinity;
            this.RecalculateMinTimeToCellsBorders(entity);
            //this.RecalculateMinTimeToEntities(entity);
        }

        private void RecalculateMinTimeToCellsBorders(Entity entity)
        {
            int ix = entity.VX > 0 ? entity.i + 1 : entity.i;
            ix = ix << this.cellSize;
            if (entity.VX < 0 && ix >= entity.X)
            {
                ix -= 1 << this.cellSize;
            }

            float tx = (ix - entity.X) / entity.VX;
            tx = Math.Max(tx, float.Epsilon);

            int jy = entity.VY > 0 ? entity.j + 1 : entity.j;
            jy = jy << this.cellSize;
            if (entity.VY < 0 && jy >= entity.Y)
            {
                jy -= 1 << this.cellSize;
            }

            float ty = (jy - entity.Y) / entity.VY;
            ty = Math.Max(ty, float.Epsilon);

            float t = tx < ty ? tx : ty;

            if (entity.T > t)
            {
                entity.T = t;
                entity.Event = 1;
            }
        }

        private void RecalculateMinTimeToEntity(Entity entity, Entity entity_2) // t_chast(ic,jc)
        {
            float x = entity_2.X - entity.X;
            float y = entity_2.Y - entity.Y;
            float vx = entity_2.VX - entity.VX;
            float vy = entity_2.VY - entity.VY;
            float L = entity_2.R + entity.R;
            float A = vx * vx + vy * vy;
            float B = -(x * vx + y * vy);
            float C = x * x + y * y;

            float D = B * B - A * (C - L * L);

            float t1 = (float)(B - Math.Sqrt(D)) / A;
            float t2 = (float)(B + Math.Sqrt(D)) / A;

            if (entity.T > t1)
            {
                entity.T = t1;
                entity.Event = 0;
            }
            if (entity_2.T > t2)
            {
                entity_2.T = t2;
                entity_2.Event = 0;
            }
        }
        private void RecalculateMinTimeToEntities(Entity entity)
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
                            this.RecalculateMinTimeToEntity(entity, cells[i, j][k]);
                        }
                    }
                }
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

        private void step(Entity entity)
        {
            switch (entity.Event)
            {
                case 0: this.step_0(entity); break;
                case 1: this.step_1(entity); break;
            }
        }

        private void step_0(Entity entity)
        {
            Entity entityNext = entity.Next;
            if (entity == entityNext.Next && entity.Event == 0)
            {
                //entity.Move();
                //entityNext.Move();
                this.event_0(entity, entityNext);
            }
            this.RecalculateMinTime(entity);
            this.heap.Recalculate(entity.Id);
            this.RecalculateMinTime(entityNext);
            this.heap.Recalculate(entityNext.Id);
        }

        private void event_0(Entity entity, Entity entityNext)
        {
            throw new NotImplementedException();
        }

        private void step_1(Entity entity)
        {
            //entity.Move();
            this.event_1(entity);
            this.RecalculateMinTime(entity);
            this.heap.Recalculate(entity.Id);
        }

        private void event_1(Entity entity)
        {
            this.cells[entity.i, entity.j].Remove(entity);
            entity.i = (int)entity.X >> this.cellSize;
            if (entity.i < 0)
            {
                entity.X = this.maxX;
            }
            if (entity.i >= this.cells.GetLength(0))
            {
                entity.X = 0;
            }
            entity.i = (int)entity.X >> this.cellSize;

            entity.j = (int)entity.Y >> this.cellSize;
            if (entity.j < 0)
            {
                entity.Y = this.maxY;
            }
            if (entity.j >= this.cells.GetLength(1))
            {
                entity.Y = 0;
            }
            entity.j = (int)entity.Y >> this.cellSize;

            this.cells[entity.i, entity.j].Add(entity);
        }


        private void UpdateMana()
        {
            // TODO: Something wrong.
            foreach (Entity entity in this.entities)
            {
                if (!entity.D)
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
                                if (cells[i, j][k].D)
                                {
                                    float x = cells[i, j][k].X - entity.X;
                                    float y = cells[i, j][k].Y - entity.Y;
                                    double d = Math.Sqrt(x * x - y * y) - cells[i, j][k].R - entity.R;
                                    if (d > 0)
                                    {
                                        d = ((1 << this.cellSize) - d) / d;
                                        if (d > 0)
                                        {
                                            entity.M += (float)d;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        public void InitClientSize(float maxX, float maxY)
        {
            this.maxX = maxX;
            this.maxY = maxY;

            this.cells = new List<Entity>[((int)this.maxX >> this.cellSize) + 1, ((int)this.maxY >> this.cellSize) + 1];
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
                float a = this.NextFloat(0, 2 * (float)Math.PI);
                float s = this.NextFloat(this.minStep, this.maxStep);
                float vx = s * (float)Math.Cos(a);
                float vy = s * (float)Math.Sin(a);
                bool d = this.random.Next(100) < 20;
                Entity entity = new Entity()
                {
                    Id = i,
                    X = x,
                    Y = y,
                    R = r,
                    D = d,
                    Brush = d ? Brushes.Black : Brushes.White,//new SolidBrush(Color.FromArgb(150, random.Next(256), random.Next(256), random.Next(256))),
                    VX = vx,
                    VY = vy,
                };
                this.entities.Add(entity);
                entity.i = (int)entity.X >> this.cellSize;
                entity.j = (int)entity.Y >> this.cellSize;
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

                    this.step(entity);
                }

                this.UpdateMana();
            }
        }

        public void Draw(Graphics g)
        {
            for (int i = 0; i < this.cells.GetLength(0); i++)
            {
                for (int j = 0; j < this.cells.GetLength(1); j++)
                {
                    //if (this.cells[i, j].Count > 0)
                    //{
                    //    g.FillRectangle(Brushes.Yellow, i << this.cellSize, j << this.cellSize, 1 << this.cellSize, 1 << this.cellSize);
                    //}
                    g.DrawRectangle(Pens.Silver, i << this.cellSize, j << this.cellSize, 1 << this.cellSize, 1 << this.cellSize);
                }
            }

            Font font = new Font(FontFamily.Families[0], 14);
            for (int i = 0; i < this.entities.Count; i++)
            {
                Entity entity = this.entities[i];

                g.FillEllipse(entity.Brush, entity.X - entity.R, entity.Y - entity.R, 2 * entity.R, 2 * entity.R);
                g.DrawEllipse(Pens.Black, entity.X - entity.R, entity.Y - entity.R, 2 * entity.R, 2 * entity.R);
                g.DrawString(entity.M.ToString(), font, Brushes.Black, entity.X + entity.R, entity.Y + entity.R);
                g.DrawLine(Pens.Black, entity.X, entity.Y, entity.X + entity.VX * entity.T, entity.Y + entity.VY * entity.T);
                if (entity.D)
                {
                    int rr = 1 << this.cellSize;
                    g.DrawEllipse(Pens.Blue, entity.X - entity.R - rr, entity.Y - entity.R - rr, 2 * (entity.R + rr), 2 * (entity.R + rr));
                }
            }
        }
    }
}
