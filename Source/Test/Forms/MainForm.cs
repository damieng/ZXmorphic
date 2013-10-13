using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Spectrum.Custom;
using Spectrum.Model;

namespace Test
{
	public partial class MainForm : Form
	{
		Spectrum48K machine;

		public MainForm()
		{
			InitializeComponent();
			machine = new Spectrum48K();

			Program.EmulationThread = new Thread(new ThreadStart(machine.Run));
			Program.EmulationThread.Name = "Emulation";
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			var cpuForm = new CPUForm(machine.cpu) {
				Top = this.Bottom,
				Left = this.Left
			};
			cpuForm.Show();

			var executionForm = new ExecutionForm(machine.cpu) {
				Top = cpuForm.Top,
				Left = cpuForm.Right,
				Height = cpuForm.Height
			};
			executionForm.Show();

			Width = cpuForm.Width + executionForm.Width;

			var displayForm = new DisplayForm(new SpectrumDisplay(machine.memory.pagedMemory.CurrentPages[1])) {
				Top = cpuForm.Bottom,
				Left = Left
			};
			displayForm.Show();

			var memoryForm = new MemoryForm(machine.memory.pagedMemory, machine.cpu) {
				Top = displayForm.Top,
				Left = displayForm.Right,
				Height = displayForm.Height
			};
			memoryForm.Show();
		}

		private void memoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new MemoryForm(machine.memory.pagedMemory, machine.cpu).Show();
		}

		private void cPUToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new CPUForm(machine.cpu).Show();
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			machine.Reset();
		}

		private void displayToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new DisplayForm(new SpectrumDisplay(machine.memory.pagedMemory.CurrentPages[1])).Show();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
            if (Program.EmulationThread != null)
            {
                if (Program.EmulationThread.ThreadState == ThreadState.Suspended)
                    Program.EmulationThread.Resume();

                if (Program.EmulationThread.ThreadState == ThreadState.Running)
                    Program.EmulationThread.Abort();
            }
		}


	}
}