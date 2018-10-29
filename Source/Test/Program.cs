using System;
using System.Threading;
using System.Windows.Forms;
using Test.Forms;

namespace Test
{
    internal static class Program
    {
        public static Thread EmulationThread;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}