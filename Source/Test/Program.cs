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

        public static void Resume()
        {
            switch (EmulationThread.ThreadState)
            {
                case ThreadState.Unstarted:
                    EmulationThread.Start();
                    break;
                case ThreadState.Suspended:
                    EmulationThread.Resume();
                    break;
                case ThreadState.Running:
                    EmulationThread.Suspend();
                    break;
            }
        }
    }
}