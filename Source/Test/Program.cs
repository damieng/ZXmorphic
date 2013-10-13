using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Test
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
			Application.Run(new MainForm());
		}

		public static Thread EmulationThread;

		public static void Resume()
		{
			switch (Program.EmulationThread.ThreadState)
			{
				case System.Threading.ThreadState.Unstarted:
					Program.EmulationThread.Start();
					break;
				case System.Threading.ThreadState.Suspended:
					Program.EmulationThread.Resume();
					break;
				case System.Threading.ThreadState.Running:
					Program.EmulationThread.Suspend();
					break;
			}
		}

	}
}