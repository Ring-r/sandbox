using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RazorPainterTest
{
    /// <remarks>Some part get from <a href="http://razorgdipainter.codeplex.com/">Mokrov Ivan</a>.</remarks>
    public partial class FormMain : Form
    {
        [DllImport("gdi32")]
        private extern static int SetDIBitsToDevice(HandleRef hDC, int xDest, int yDest, int dwWidth, int dwHeight, int XSrc, int YSrc, int uStartScan, int cScanLines, ref int lpvBits, ref BITMAPINFO lpbmi, uint fuColorUse);

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public int bihSize;
            public int bihWidth;
            public int bihHeight;
            public short bihPlanes;
            public short bihBitCount;
            public int bihCompression;
            public int bihSizeImage;
            public double bihXPelsPerMeter;
            public double bihClrUsed;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFO
        {
            public BITMAPINFOHEADER biHeader;
            public int biColors;
        }

        Stopwatch renderStopwatch = new Stopwatch();
        long renderTime = 0;
        Stopwatch updateStopwatch = new Stopwatch();
        long updateTime = 0;

        private Size size = new Size(1, 1);
        private int[] array = new int[1];

        Task task = null;
        private bool IsTaskTerminate = false;

        private BITMAPINFO _BI = new BITMAPINFO
        {
            biHeader =
            {
                bihBitCount = 32,
                bihPlanes = 1,
                bihSize = 40,
                bihWidth = 1,
                bihHeight = 1,
                bihSizeImage = 2
            }
        };


        public FormMain()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer, false);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            task = Task.Factory.StartNew(() =>
             {
                 Graphics graphics = null;
                 try
                 {
                     graphics = this.CreateGraphics();
                     HandleRef handleRef = new HandleRef(graphics, graphics.GetHdc());

                     SimpleParticlesWorld.Count = 500000;
                     SimpleParticlesWorld.Init();

                     while (!IsTaskTerminate)
                     {
                         Stopwatch stopwatch_render = new Stopwatch();
                         Stopwatch stopwatch = new Stopwatch();
                         stopwatch.Start();

                         if (this.array != null)
                         {
                             SimpleParticlesWorld.Update();

                             Array.Clear(this.array, 0, this.array.Length);
                             foreach (SimpleParticle particle in SimpleParticlesWorld.Particles)
                             {
                                 int pointBase = (int)particle.y * this.size.Width + (int)particle.x;
                                 if (pointBase < this.array.Length)
                                 {
                                     this.array[pointBase] = particle.c;
                                 }
                             }

                             stopwatch_render.Start();
                             SetDIBitsToDevice(handleRef, 0, 0, size.Width, size.Height, 0, 0, 0, size.Height, ref array[0], ref this._BI, 0);
                             stopwatch_render.Stop();
                         }

                         stopwatch.Stop();
                         if (!this.IsTaskTerminate)
                         {
                             this.Invoke((Action)(() =>
                             {
                                 this.Text = string.Format("Points count: {0}. Update time: {1}. Render time: {2}.", SimpleParticlesWorld.Count, stopwatch.ElapsedMilliseconds, stopwatch_render.ElapsedMilliseconds);
                             }));
                         }
                     }
                 }
                 finally
                 {
                     if (graphics != null)
                     {
                         graphics.Dispose();
                     }
                 }
             });
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            this.size = (sender as Control).ClientSize;
            this.array = new int[this.size.Width * this.size.Height];

            SimpleParticlesWorld.Size = this.size;

            this._BI.biHeader.bihWidth = this.size.Width;
            this._BI.biHeader.bihHeight = this.size.Height;
            this._BI.biHeader.bihSizeImage = this.size.Width * this.size.Height;
        }

        private void FormMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SimpleParticlesWorld.Init();
            }
            else
            {
                SimpleParticlesWorld.ActionIn(e.X, this.ClientSize.Height - e.Y);
            }
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.task != null)
            {
                this.IsTaskTerminate = true;
                this.task.Wait();
            }
        }
    }
}
