﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiscreteEventSimulation
{
    public class Entities
    {
        private const int areaShift = 9;
        private const int areaSize = 1 << areaShift; // <- 512

        private const int cellShift = 6;
        private const int cellSize = 1 << cellShift; // <- 64.

        private const int cellsCount = (areaSize >> cellShift) + 1;
        private const int entitiesCount = 10;

        private const float minRadius = 10.0f;
        private const float maxRadius = 20.0f;
        private const float minV = 0.1f;
        private const float maxV = 3.0f;


        private readonly CellList[,] cells = new CellList[cellsCount, cellsCount];
        private readonly List<Entity> entities = new List<Entity>();
        private readonly Heap heap = new Heap();


        private void RecalculateMinTime(Entity entity)
        {
            entity.ClearEvent();
            this.CheckEventByX(entity);
            this.CheckEventByY(entity);
            this.CheckCollisionEvent(entity);
        }

        private void CheckEventByX(Entity entity)
        {
            if (entity.VX != 0)
            {
                var @event = entity.VX > 0 ? 2 : 1;

                var ix = entity.VX > 0 ? entity.i + 1 : entity.i;
                ix = ix << cellShift;
                var time = (ix - entity.X) / entity.VX;

                entity.SetEvent(time, @event);
            }
        }

        private void CheckEventByY(Entity entity)
        {
            if (entity.VY != 0)
            {
                var @event = entity.VY > 0 ? 4 : 3;

                var jy = entity.VY > 0 ? entity.j + 1 : entity.j;
                jy = jy << cellShift;
                var time = (jy - entity.Y) / entity.VY;

                entity.SetEvent(time, @event);
            }
        }

        private void CheckCollisionEvent(Entity entity)
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
                            this.CheckCollisionEvent(entity, cells[i, j][k]);
                        }
                    }
                }
            }
        }
        private void CheckCollisionEvent(Entity entity, Entity entityNext)
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

            var entityTime = (float)(B - Math.Sqrt(D)) / A;
            entity.SetEvent(entityTime, -1, entityNext);

            var entityNextTime = (float)(B + Math.Sqrt(D)) / A;
            entityNext.SetEvent(entityNextTime, -1, entity);
        }

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
            entity.i--;
            if (entity.i < 0)
            {
                entity.i = this.cells.GetLength(0) - 1;
                entity.X = areaSize;
            }
            this.cells[entity.i, entity.j].Add(entity);
        }
        private void RaiseEventMaxX(Entity entity)
        {
            this.cells[entity.i, entity.j].Remove(entity);
            entity.i++;
            if (entity.i >= this.cells.GetLength(0))
            {
                entity.i = 0;
                entity.X = 0;
            }
            this.cells[entity.i, entity.j].Add(entity);
        }
        private void RaiseEventMinY(Entity entity)
        {
            this.cells[entity.i, entity.j].Remove(entity);
            entity.j--;
            if (entity.j < 0)
            {
                entity.j = this.cells.GetLength(1) - 1;
                entity.Y = areaSize;
            }
            this.cells[entity.i, entity.j].Add(entity);
        }
        private void RaiseEventMaxY(Entity entity)
        {
            this.cells[entity.i, entity.j].Remove(entity);
            entity.j++;
            if (entity.j >= this.cells.GetLength(1))
            {
                entity.j = 0;
                entity.Y = 0;
            }
            this.cells[entity.i, entity.j].Add(entity);
        }

        private void RaiseEvent_1(Entity entity)
        {
            Entity entityNext = entity.Next;

            var x = entityNext.X - entity.X;
            var y = entityNext.Y - entity.Y;
            var vx = entityNext.VX - entity.VX;
            var vy = entityNext.VY - entity.VY;

            var coef = (vx * x + vy * y) / (x * x + y * y);

            entity.VX += coef * x;
            entity.VY += coef * y;

            entityNext.VX -= coef * x;
            entityNext.VY -= coef * y;

            //var vx1 = entity.VX; entity.VX = entityNext.VX; entityNext.VX = vx1;
            //var vy1 = entity.VY; entity.VY = entityNext.VY; entityNext.VY = vy1;

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
                    this.cells[i, j] = new CellList();
                }
            }

            this.Create();
        }

        public void Create()
        {
            for (int i = 0; i < this.cells.GetLength(0); ++i)
            {
                for (int j = 0; j < this.cells.GetLength(1); ++j)
                {
                    this.cells[i, j].Clear();
                }
            }

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
            var enity = this.heap.GetFirst();

            var time = enity.Time;
            foreach (var entity in entities)
                entity.Update(time);

            this.RaiseEvent(enity);
            this.AfterEvent(enity);
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
                        //g.FillRectangle(Brushes.Yellow, i << cellShift, j << cellShift, cellSize, cellSize);
                    }
                    g.DrawRectangle(Pens.Silver, i << cellShift, j << cellShift, cellSize, cellSize);
                }
            }
            #endregion For testing.

            var topEntityTime = this.heap.GetFirst().Time;
            foreach (Entity entity in this.entities)
            {
                g.FillEllipse(Brushes.Blue, entity.X - entity.R, entity.Y - entity.R, 2 * entity.R, 2 * entity.R);
                g.DrawEllipse(Pens.Black, entity.X - entity.R, entity.Y - entity.R, 2 * entity.R, 2 * entity.R);
                //g.DrawString($"Id: {entity.Id}, time: {entity.Time}", font, Brushes.Black, entity.X + entity.R, entity.Y + entity.R);
                //g.DrawLine(Pens.Black, entity.X, entity.Y, entity.X + entity.VX * entity.Time, entity.Y + entity.VY * entity.Time);
                g.DrawLine(Pens.Red, entity.X, entity.Y, entity.X + entity.VX * topEntityTime, entity.Y + entity.VY * topEntityTime);
                
            }
        }
    }
}
