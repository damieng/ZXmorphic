using System;
using System.Threading;
using System.Windows.Forms;
using Spectrum.Custom;
using Spectrum.Model;

namespace Test.Forms
{
    public partial class MainForm : Form
    {
        private readonly Spectrum48K machine;

        public MainForm()
        {
            InitializeComponent();
            machine = new Spectrum48K();

            //machine.Cpu.Breakpoints.Add(0x128E, null);

            Program.EmulationThread = new Thread(machine.Run) { Name = "Emulation" };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var cpuForm = new CPUForm(machine.Cpu)
            {
                Top = Bottom,
                Left = Left
            };
            cpuForm.Show();

            var executionForm = new ExecutionForm(machine.Cpu)
            {
                Top = cpuForm.Top,
                Left = cpuForm.Right,
                Height = cpuForm.Height
            };
            executionForm.Show();

            Width = cpuForm.Width + executionForm.Width;

            var displayForm = new DisplayForm(new SpectrumDisplay(machine.Memory.PagedMemory.CurrentPages[1]))
            {
                Top = cpuForm.Bottom,
                Left = Left
            };
            displayForm.Show();

            var memoryForm = new MemoryForm(machine.Memory.PagedMemory, machine.Cpu)
            {
                Top = displayForm.Top,
                Left = displayForm.Right,
                Height = displayForm.Height
            };
            memoryForm.Show();
        }

        private void memoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MemoryForm(machine.Memory.PagedMemory, machine.Cpu).Show();
        }

        private void cPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CPUForm(machine.Cpu).Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            machine.Reset();
        }

        private void displayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DisplayForm(new SpectrumDisplay(machine.Memory.PagedMemory.CurrentPages[1])).Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.EmulationThread == null) return;

            if (Program.EmulationThread.ThreadState == ThreadState.Suspended)
                Program.EmulationThread.Resume();

            if (Program.EmulationThread.ThreadState == ThreadState.Running)
                Program.EmulationThread.Abort();
        }
    }
}