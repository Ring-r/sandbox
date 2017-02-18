using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DiscreteEventSimulation
{
    public partial class MainForm : Form
    {
        private readonly Entities entities = new Entities();

        public MainForm()
        {
            InitializeComponent();

            this.timer.Interval = 100;

            this.entities.Create();
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

            this.entities.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height, this.Font);
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            this.entities.Update();

            this.Invalidate();
        }
    }
}
