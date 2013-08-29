using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MoveInCells
{
    public partial class MainForm : Form
    {
        private readonly Entities entities = new Entities();

        public MainForm()
        {
            InitializeComponent();
            this.timer.Interval = 1;
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

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            this.entities.Draw(e.Graphics);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.entities.InitClientSize(this.ClientSize.Width, this.ClientSize.Height);
            this.entities.Create();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.entities.Update();
            this.Invalidate();
        }

    }
}
