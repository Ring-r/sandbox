using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private Entity[,] cells = null;

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
        }

        private void entities_Update()
        {
            for (int i = 0; i < this.entities.Count; i++)
            {
                Entity entity = this.entities[i];

                this.cells[(int)entity.X >> this.cellSize, (int)entity.Y >> this.cellSize] = null;

                int ci = (int)(entity.X + entity.VX) >> this.cellSize;
                int cj = (int)(entity.Y + entity.VY) >> this.cellSize;

                bool iSCollide = false;
                for (int x = -1; x <= 1 && !iSCollide; x++)
                {
                    for (int y = -1; y <= 1 && !iSCollide; y++)
                    {
                        if (
                            0 <= ci + x && ci + x < this.cells.GetLength(0) &&
                            0 <= cj + y && cj + y < this.cells.GetLength(1))
                        {
                            iSCollide = this.cells[ci + x, cj + y] != null;
                        }
                    }
                }

                if (
                    0 <= ci && ci < this.cells.GetLength(0) &&
                    0 <= cj && cj < this.cells.GetLength(1) &&
                    !iSCollide)
                {
                    entity.X += entity.VX;
                    entity.Y += entity.VY;

                    this.cells[ci, cj] = entity;

                    if (entity.X < entity.R || this.ClientSize.Width - entity.R < entity.X)
                    {
                        entity.VX = -entity.VX;
                    }
                    if (entity.Y < entity.R || this.ClientSize.Height - entity.R < entity.Y)
                    {
                        entity.VY = -entity.VY;
                    }
                }
            }
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
            this.cells = new Entity[(this.ClientSize.Width >> this.cellSize) + 1, (this.ClientSize.Height >> this.cellSize) + 1];
            this.entities_Create();
        }
    }
}
