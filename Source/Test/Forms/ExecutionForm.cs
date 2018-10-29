using System;
using System.Drawing;
using System.Windows.Forms;
using Morphic.Core.CPU.Z80;
using Morphic.Core.Primitive;

namespace Test.Forms
{
    public partial class ExecutionForm : Form
    {
        private const string AddressLineStart = "{0:X4}";

        private readonly Z80CPU z80;
        private readonly Disassembler z80Disassembler;
        private Disassembler.Block currentBlock;
        private LEWord? lastPC;

        public ExecutionForm(Z80CPU z80)
        {
            this.z80 = z80;
            z80Disassembler = new Disassembler(z80);
            InitializeComponent();

            RefreshDisplay();
        }

        public void RefreshDisplay()
        {
            DisplayDisassembly();
        }

        private void DisplayDisassembly()
        {
            var currPC = z80.ProgramCounter;

            if (lastPC == currPC)
                return;

            ListViewItem foundItem = null;

            foreach (ListViewItem item in disassemblyListView.Items)
            {
                var block = (Disassembler.Block)item.Tag;
                if ((currPC >= block.Address) && (currPC <= block.Address + block.Length))
                {
                    foundItem = item;
                    currentBlock = block;
                }
            }

            if (foundItem == null)
                RefreshDisassembly();
            else
            {
                foundItem.Selected = true;
                foundItem.EnsureVisible();
            }

            lastPC = currPC;
        }

        private void RefreshDisassembly()
        {
            disassemblyListView.BeginUpdate();
            disassemblyListView.Items.Clear();

            foreach (var block in z80Disassembler.Disassemble(z80.ProgramCounter))
            {
                var item = new ListViewItem(string.Format(AddressLineStart, block.Address));
                var bytes = "";
                for (var i = 0; i < block.Length; i++)
                    bytes += $"{z80.Memory.ReadByte((UInt16)(block.Address + i)):X2} ";
                item.SubItems.Add(bytes.Trim());
                item.SubItems.Add(block.Disassembly);
                item.Tag = block;
                if (z80.Breakpoints.ContainsKey(block.Address))
                    item.ForeColor = Color.Red;
                disassemblyListView.Items.Add(item);
                if ((z80.PC >= block.Address) && (z80.PC <= block.Address + block.Length))
                {
                    item.Selected = true;
                    item.EnsureVisible();
                }
            }

            disassemblyListView.EndUpdate();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void MemoryForm_VisibleChanged(object sender, EventArgs e)
        {
            RefreshTimer.Enabled = Visible;
        }

        private void disassemblyListView_DoubleClick(object sender, EventArgs e)
        {
            BreakpointButton_Click(sender, e);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            z80.Step = false;
            Program.EmulationThread.Resume();
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            z80.Step = true;
            Program.EmulationThread.Resume();
        }

        private void BreakpointButton_Click(object sender, EventArgs e)
        {
            if (disassemblyListView.SelectedItems.Count == 0)
                return;

            var item = disassemblyListView.SelectedItems[0];
            var block = (Disassembler.Block)item.Tag;

            if (z80.Breakpoints.ContainsKey(block.Address))
            {
                z80.Breakpoints.Remove(block.Address);
                item.ForeColor = SystemColors.WindowText;
            }
            else
            {
                z80.Breakpoints.Add(block.Address, null);
                item.ForeColor = Color.Red;
            }
        }

        private void RunToSelectedButton_Click(object sender, EventArgs e)
        {
            var selectedBlock = (Disassembler.Block)disassemblyListView.SelectedItems[0].Tag;
            z80.StopBeforeAddress = selectedBlock.Address;
            z80.Step = false;
            Program.EmulationThread.Resume();
        }

        private void StepOverButton_Click(object sender, EventArgs e)
        {
            z80.StopBeforeAddress = (ushort)(currentBlock.Address + currentBlock.Length);
            z80.Step = false;
            Program.EmulationThread.Resume();
        }
    }
}