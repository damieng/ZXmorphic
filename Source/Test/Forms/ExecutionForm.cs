using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Morphic.Core.Memory;
using Morphic.Core.Primitive;
using Morphic.Core.Processor.Z80;

using Spectrum.Custom;

namespace Test
{
	public partial class ExecutionForm : Form
	{
		#region Constants & enumerations

		private const string addressLineStart = "{0:X4}";
		private const string hexLineByte = " {0:X2}";

		#endregion

		#region Instance variables

		private Z80CPU z80;
		private Disassembler z80Disassembler;
		private LEWord? lastPC;
		private Disassembler.Block currentBlock;

		#endregion

		#region Properties

		#endregion

		#region Construction

		public ExecutionForm(Z80CPU z80)
		{
			this.z80 = z80;
			this.z80Disassembler = new Disassembler(z80);
			InitializeComponent();

			RefreshDisplay();
		}

		#endregion

		#region Methods

		public void RefreshDisplay()
		{
			DisplayDisassembly();
		}

		#endregion

		#region Private functions

		private void DisplayDisassembly()
		{
            ushort currPC = z80.ProgramCounter;

            if (lastPC == currPC)
				return;

			ListViewItem foundItem = null;

			foreach (ListViewItem item in disassemblyListView.Items) {
				var block = (Disassembler.Block)item.Tag;
				if ((currPC >= block.Address) && (currPC <= block.Address + block.Length)) {
					foundItem = item;
					currentBlock = block;
				}
			}

			if (foundItem == null)
				RefreshDisassembly();
			else {
				foundItem.Selected = true;
				foundItem.EnsureVisible();
			}

            lastPC = currPC;
		}

		private void RefreshDisassembly()
		{
			disassemblyListView.BeginUpdate();
			disassemblyListView.Items.Clear();

			foreach (var block in z80Disassembler.Disassemble(z80.ProgramCounter)) {
				var item = new ListViewItem(string.Format(addressLineStart, block.Address));
				String bytes = "";
				for (int i = 0; i < block.Length; i++)
					bytes += String.Format("{0:X2} ", z80.Memory.ReadByte((UInt16) (block.Address + i)));
				item.SubItems.Add(bytes.Trim());
				item.SubItems.Add(block.Disassembly);
				item.Tag = block;
				if (z80.Breakpoints.ContainsKey(block.Address))
					item.ForeColor = Color.Red;
				disassemblyListView.Items.Add(item);
				if ((z80.PC >= block.Address) && (z80.PC <= block.Address + block.Length)) {
					item.Selected = true;
					item.EnsureVisible();
				}
			}

			disassemblyListView.EndUpdate();
		}

		#endregion

		#region Event handling

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

		#endregion

		private void RunButton_Click(object sender, EventArgs e)
		{
			z80.Step = false;
			Program.Resume();
		}

		private void StepButton_Click(object sender, EventArgs e)
		{
			z80.Step = true;
			Program.Resume();
		}

		private void BreakpointButton_Click(object sender, EventArgs e)
		{
            if (disassemblyListView.SelectedItems.Count == 0)
                return;

			var item = disassemblyListView.SelectedItems[0];
			var block = item.Tag as Disassembler.Block;

			if (z80.Breakpoints.ContainsKey(block.Address)) {
				z80.Breakpoints.Remove(block.Address);
				item.ForeColor = SystemColors.WindowText;
			}
			else {
				z80.Breakpoints.Add(block.Address, null);
				item.ForeColor = Color.Red;
			}
		}

        private void RunToSelectedButton_Click(object sender, EventArgs e)
        {
            var selectedBlock = (Disassembler.Block)disassemblyListView.SelectedItems[0].Tag;
            z80.StopBeforeAddress = selectedBlock.Address;
            z80.Step = false;
            Program.Resume();
        }

        private void StepOverButton_Click(object sender, EventArgs e)
        {
			z80.StopBeforeAddress = (ushort)(currentBlock.Address + currentBlock.Length);
            z80.Step = false;
            Program.Resume();
        }
	}
}