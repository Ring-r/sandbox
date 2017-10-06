using System;
using System.Windows.Forms;

namespace Prototypes.Maps.StripsAndRooms
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

    private static Lazy<Random> random = new Lazy<Random>(() => new Random());
    public static Random Random { get { return random.Value; } }
  }
}
