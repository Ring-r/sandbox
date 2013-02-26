using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LockBitsTest
{
    public partial class RazorBitmapFormAntiparents : Form
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

        private BITMAPINFO bitmapInfo = new BITMAPINFO
        {
            biHeader =
            {
                bihBitCount = 32,
                bihPlanes = 1,
                bihSize = 40,
                bihWidth = 1,
                bihHeight = 1,
                bihSizeImage = 1
            }
        };

        private Size size = new Size(1, 1);
        private int[] array = new int[1];

        private Stopwatch commonStopwatch = new Stopwatch();
        private long commonTime = 0;
        private Stopwatch renderStopwatch = new Stopwatch();
        private long renderTime = 0;
        private Stopwatch updateStopwatch = new Stopwatch();
        private long updateTime = 0;

        private Task task = null;
        private bool IsTerminate = false;

        public RazorBitmapFormAntiparents()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, false);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);

            SimpleParticlesWorld.Size = this.ClientSize;

            this.task = Task.Factory.StartNew(() =>
                {
                    using (Graphics graphics = this.CreateGraphics())
                    {
                        IntPtr hdc = IntPtr.Zero;
                        try
                        {
                            hdc = graphics.GetHdc();
                            HandleRef handleRef = new HandleRef(graphics, hdc);

                            while (!this.IsTerminate)
                            {
                                this.commonStopwatch.Restart();

                                this.updateStopwatch.Restart();
                                SimpleParticlesWorld.Update();
                                this.updateTime = this.updateStopwatch.ElapsedMilliseconds;

                                this.renderStopwatch.Restart();
                                Array.Clear(this.array, 0, this.array.Length);
                                SimpleParticle particle;
                                int pointBase;
                                for (int i = SimpleParticlesWorld.Particles.Count - 1; i >= 0; --i)
                                {
                                    particle = SimpleParticlesWorld.Particles[i];
                                    //foreach (SimpleParticle particle in SimpleParticlesWorld.Particles)
                                    //{
                                    pointBase = (this.size.Height - (int)particle.y) * this.size.Width + (int)particle.x;
                                    if (0 <= pointBase && pointBase < this.array.Length)
                                    {
                                        this.array[pointBase] = particle.c;
                                    }
                                }
                                SetDIBitsToDevice(handleRef, 0, 0, this.size.Width, this.size.Height, 0, 0, 0, this.size.Height, ref this.array[0], ref this.bitmapInfo, 0);
                                this.renderTime = this.renderStopwatch.ElapsedMilliseconds;

                                this.commonTime = this.commonStopwatch.ElapsedMilliseconds;
                            }
                        }
                        finally
                        {
                            if (hdc != IntPtr.Zero)
                            {
                                graphics.ReleaseHdc(hdc);
                            }
                        }
                    }
                });
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.IsTerminate = true;
                    this.Close();
                    break;
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            SimpleParticlesWorld.ActionIn(e.X, e.Y);
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            this.size = new Size(Math.Max(1, control.ClientSize.Width), Math.Max(1, control.ClientSize.Height));
            this.array = new int[this.size.Width * this.size.Height];
            this.bitmapInfo.biHeader.bihWidth = this.size.Width;
            this.bitmapInfo.biHeader.bihHeight = this.size.Height;
            this.bitmapInfo.biHeader.bihSizeImage = this.size.Width * this.size.Height;

            SimpleParticlesWorld.Size = this.size;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Text = string.Format("Razor Bitmap Antiparent. Points count: {0}. Update time: {1}. Render time: {2}. Common time: {3}.", SimpleParticlesWorld.Count, this.updateTime, this.renderTime, this.commonTime);
        }
    }
}
