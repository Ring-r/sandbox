using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MoveInCells
{
    public partial class MainForm : Form
    {
        private readonly int entitiesCount = 100;
        private readonly int cellSize = 6;
        private readonly float maxSpeed = 3;
        private readonly Random rand = new Random();
        private readonly List<Entity> entities = new List<Entity>();
        private List<Entity>[,] cells = null;
        private Heap heap = new Heap();


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            for (int i = 0; i < this.cells.GetLength(0); i++)
            {
                for (int j = 0; j < this.cells.GetLength(1); j++)
                {
                    if (this.cells[i, j].Count > 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Yellow, i << this.cellSize, j << this.cellSize, 1 << this.cellSize, 1 << this.cellSize);
                    }
                    e.Graphics.DrawRectangle(Pens.Silver, i << this.cellSize, j << this.cellSize, 1 << this.cellSize, 1 << this.cellSize);
                }
            }


            for (int i = 0; i < this.entities.Count; i++)
            {
                Entity entity = this.entities[i];

                e.Graphics.FillEllipse(entity.Brush, entity.X - entity.R, entity.Y - entity.R, 2 * entity.R, 2 * entity.R);
                e.Graphics.DrawEllipse(Pens.Black, entity.X - entity.R, entity.Y - entity.R, 2 * entity.R, 2 * entity.R);
                e.Graphics.DrawString(entity.T.ToString(), this.Font, Brushes.Black, entity.X + entity.R, entity.Y + entity.R);
                e.Graphics.DrawLine(Pens.Black, entity.X, entity.Y, entity.X + entity.VX * entity.T, entity.Y + entity.VY * entity.T);
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.entities_Update();
            this.Invalidate();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.cells = new List<Entity>[(this.ClientSize.Width >> this.cellSize) + 1, (this.ClientSize.Height >> this.cellSize) + 1];
            this.entities_Create();
        }

        private void entities_Create()
        {
            for (int i = 0; i < this.cells.GetLength(0); ++i)
            {
                for (int j = 0; j < this.cells.GetLength(1); ++j)
                {
                    this.cells[i, j] = new List<Entity>();
                }
            }

            this.entities.Clear();
            for (int i = 0; i < entitiesCount; i++)
            {
                float r = (1 << this.cellSize) >> 3;
                float x = rand.Next((int)r, this.ClientSize.Width - (int)r);
                float y = rand.Next((int)r, this.ClientSize.Height - (int)r);
                float vx = (float)rand.NextDouble() * this.maxSpeed;
                float vy = (float)rand.NextDouble() * this.maxSpeed;
                Entity entity = new Entity() { X = x, Y = y, R = r, Brush = new SolidBrush(Color.FromArgb(150, rand.Next(256), rand.Next(256), rand.Next(256))), VX = vx, VY = vy };
                this.entities.Add(entity);
                entity.i = (int)entity.X >> this.cellSize;
                entity.j = (int)entity.Y >> this.cellSize;
                this.cells[entity.i, entity.j].Add(entity);
                this.RecalculateMinTime(entity);
            }

            this.heap.StartFill(this.entities.ToArray());
        }

        private void entities_Update()
        {
            if (this.heap.isBuild)
            {
                Entity entity = this.heap.GetFirst();
                while (entity.T <= 0)
                {
                    this.RecalculateMinTime(entity);
                    entity = this.heap.GetFirst();
                }
                this.step(entity);
            }
        }

        private void RecalculateMinTime(Entity entity)
        {
            this.RecalculateMinTimeToCellsBorders(entity);

            int imin = Math.Max(0, entity.i - 1);
            int imax = Math.Min(entity.i + 1, this.cells.GetLength(0) - 1);
            int jmin = Math.Max(0, entity.j - 1);
            int jmax = Math.Min(entity.j + 1, this.cells.GetLength(1) - 1);
            for (int i = imin; i <= imax; ++i)
            {
                for (int j = jmin; j <= jmax; ++j)
                {
                    for (int k = 1; k < cells[i, j].Count; ++k)
                    {
                        if (entity != cells[i, j][k])
                        {
                            RecalculateMinTimeToEntity(entity, cells[i, j][k]);
                        }
                    }
                }
            }
        }

        private void RecalculateMinTimeToCellsBorders(Entity entity)
        {
            int ix = entity.VX < 0 ? entity.i << this.cellSize : (entity.i + 1) << this.cellSize;
            float tx = entity.TLoc + (ix - entity.X) / entity.VX;
            int jy = entity.VY < 0 ? entity.j << this.cellSize : (entity.j + 1) << this.cellSize;
            float ty = entity.TLoc + (jy - entity.Y) / entity.VY;
            entity.T = tx < ty ? tx : ty;
            entity.Event = 1;
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
            }
            if (entity_2.T > t2)
            {
                entity_2.T = t2;
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
                entity.Move();
                entityNext.Move();
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
            entity.Move();
            this.event_1(entity);
            this.RecalculateMinTime(entity);
            this.heap.Recalculate(entity.Id);
        }

        private void event_1(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
