using System;
using System.Windows.Forms;

namespace LockBitsTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FastBitmapForm());
            Application.Run(new RazorBitmapForm());
            Application.Run(new RazorBitmapFormAntiparents());
        }
    }
}
