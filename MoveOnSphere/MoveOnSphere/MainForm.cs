using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace MoveOnSphere
{
    public partial class MainForm : Form
    {
        private readonly Random random = new Random();
        private readonly Entity entity = new Entity();

        public MainForm()
        {
            InitializeComponent();

            this.timer.Interval = 100;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            float r = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / 2;

            float x = this.random.Next((int)r / 2);
            float y = this.random.Next((int)r / 2);
            float z = (float)Math.Sqrt(r * r - x * x - y * y);

            this.entity.r = r;

            this.entity.v = new Vector3D(x, y, z);
            this.entity.v.Normalize();
            this.entity.w = 0;

            this.entity.v_ = Vector3D.CrossProduct(this.entity.v, new Vector3D(0, 0, 1));
            this.entity.v_.Normalize();
            this.entity.w_ = 0;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.TranslateTransform(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

            e.Graphics.DrawEllipse(Pens.Black, -this.entity.r, -this.entity.r, 2 * this.entity.r, 2 * this.entity.r);
            float r = (float)(10 * (this.entity.v.Z + 1) / 2 + 10);
            Brush brush = new SolidBrush(Color.FromArgb((int)(200 * (this.entity.v.Z + 1) / 2) + 55, Color.Blue));
            e.Graphics.FillEllipse(brush, (float)(this.entity.v.X * this.entity.r) - r / 2, (float)(this.entity.v.Y * this.entity.r) - r / 2, r, r);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.random.Next(100) < 5)
            {
                this.entity.w = 10 * (float)this.random.NextDouble() - 5;
            }
            if (this.random.Next(100) >= 95)
            {
                this.entity.w_ = 10 * (float)this.random.NextDouble() - 5;
            }
            this.entity.Move();
            this.Invalidate();
        }
    }
}
