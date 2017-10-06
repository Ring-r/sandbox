using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prototype
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static readonly Random rand = new Random();
    }

    public class MainForm : Form
    {
        public MainForm()
        {
            this.SuspendLayout();

            this.DoubleBuffered = true;
            this.Name = this.Text = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            this.Load += this.MainForm_Load;
            this.KeyUp += this.MainForm_KeyUp;
            this.Paint += this.MainForm_Paint;
            this.Resize += this.MainForm_Resize;

            this.ResumeLayout(false);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.CreateLevel();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                this.CreateLevel();
                this.Invalidate();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private const int count = 256;
        private const float height = 0.5f;

        private float[] perlinNoise = null;

        private void CreateLevel()
        {
            this.perlinNoise = CreatePerlinNoise(count, height);
        }

        private static float[] CreatePerlinNoise(int count, float height)
        {
            var res = new float[count];

            var step = count / 2;
            do
            {
                var heightBegin = (float)Program.rand.NextDouble() * height;
                for (var i = 0; i < count; i += step)
                {
                    var heightEnd = (float)Program.rand.NextDouble() * height;
                    var heightStep = (heightEnd - heightBegin) / step;
                    for (var j = 0; j < step; j += 1)
                    {
                        res[i + j] += heightBegin + heightStep * j;
                    }
                    heightBegin = heightEnd;
                }
                step /= 2;
                height /= 2;
            } while (step > 1);
            return res;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (this.perlinNoise == null)
            {
                return;
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var xKoef = (float)this.ClientSize.Width / count;
            var yKoef = (float)this.ClientSize.Height / 2 / height;

            var points = new PointF[this.perlinNoise.Length];
            for (var i = 0; i < points.Length; ++i)
            {
                points[i] = new PointF(i * xKoef, this.ClientSize.Height - this.perlinNoise[i] * yKoef);
            }
            e.Graphics.DrawLines(Pens.Green, points);
        }
    }
}
