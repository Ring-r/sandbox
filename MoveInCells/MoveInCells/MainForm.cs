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

        private void entities_Create()
        {
            this.entities.Clear();
            for (int i = 0; i < entitiesCount; i++)
            {
                float r = (1 << this.cellSize) >> 3;
                float x = rand.Next((int)r, this.ClientSize.Width - (int)r);
                float y = rand.Next((int)r, this.ClientSize.Height - (int)r);
                float vx = (float)rand.NextDouble() * this.maxSpeed;
                float vy = (float)rand.NextDouble() * this.maxSpeed;
                this.entities.Add(new Entity() { X = x, Y = y, R = r, Brush = new SolidBrush(Color.FromArgb(150, rand.Next(256), rand.Next(256), rand.Next(256))), VX = vx, VY = vy });
            }
            this.heap.StartFill(this.entities.ToArray());
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < this.cells.GetLength(0); i++)
            {
                for (int j = 0; j < this.cells.GetLength(1); j++)
                {
                    if (this.cells[i, j] != null)
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

        private void entities_Update()
        {
            Entity entity = this.heap.GetFirst();
            if (entity != null)
            {
                while (entity.T <= 0)
                {
                    this.RecalculateMinTime(entity);
                    entity = this.heap.GetFirst();
                }
            }

            #region Old code.

            //for (int i = 0; i < this.entities.Count; i++)
            //{
            //    Entity entity = this.entities[i];

            //    this.cells[(int)entity.X >> this.cellSize, (int)entity.Y >> this.cellSize] = null;

            //    int ci = (int)(entity.X + entity.VX) >> this.cellSize;
            //    int cj = (int)(entity.Y + entity.VY) >> this.cellSize;

            //    bool iSCollide = false;
            //    for (int x = -1; x <= 1 && !iSCollide; x++)
            //    {
            //        for (int y = -1; y <= 1 && !iSCollide; y++)
            //        {
            //            if (
            //                0 <= ci + x && ci + x < this.cells.GetLength(0) &&
            //                0 <= cj + y && cj + y < this.cells.GetLength(1))
            //            {
            //                iSCollide = this.cells[ci + x, cj + y] != null;
            //            }
            //        }
            //    }

            //    if (
            //        0 <= ci && ci < this.cells.GetLength(0) &&
            //        0 <= cj && cj < this.cells.GetLength(1) &&
            //        !iSCollide)
            //    {
            //        entity.X += entity.VX;
            //        entity.Y += entity.VY;

            //        this.cells[ci, cj] = entity;

            //        if (entity.X < entity.R || this.ClientSize.Width - entity.R < entity.X)
            //        {
            //            entity.VX = -entity.VX;
            //        }
            //        if (entity.Y < entity.R || this.ClientSize.Height - entity.R < entity.Y)
            //        {
            //            entity.VY = -entity.VY;
            //        }
            //    }
            //}

            #endregion Old code.
        }

        private void RecalculateMinTime(Entity entity)
        {
            this.RecalculateMinTimeToBlocks(entity);
            this.RecalculateMinTimeToBorders(entity);

            // TODO: 1 is not correct and uniq. What I need to do? What I need to do if radiuses of entities is not equal?
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

        private void RecalculateMinTimeToBlocks(Entity entity)
        {
            throw new NotImplementedException();
        }

        private void RecalculateMinTimeToBorders(Entity entity)
        {
            throw new NotImplementedException();
        }

        private void RecalculateMinTimeToEntity(Entity entity, Entity entity_2)
        {
            throw new NotImplementedException();
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
    }
}
