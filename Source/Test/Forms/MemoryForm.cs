using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Morphic.Core.Memory;
using Morphic.Core.Processor.Z80;

using Spectrum.Custom;

namespace Test
{
	public partial class MemoryForm : Form
	{
		#region Constants & enumerations

		private const string addressLineStart = "{0:X4}";
		private const string hexLineByte = " {0:X2}";

		#endregion

		#region Instance variables

		private uint hexBytesPerLine = 16;
		private PagedMemory pagedMemory;
		private Z80CPU z80;
		private SpectrumDisplay display;

		#endregion

		#region Properties

		private UInt16 currentAddress;
		public UInt16 CurrentAddress
		{
			get { return currentAddress; }
			set {
				if (currentAddress != value) {
					currentAddress = value;
					if (AddressNumeric.Value != currentAddress)
						AddressNumeric.Value = currentAddress;
					RefreshDisplay();
				}
			}
		}

		#endregion

		#region Construction

		public MemoryForm(PagedMemory pagedMemory, Z80CPU z80)
		{
			this.pagedMemory = pagedMemory;
			this.z80 = z80;
			this.display = new SpectrumDisplay(pagedMemory.AllPages[1]);
			InitializeComponent();

			FollowCombo.SelectedIndex = 2;
			BankCombo.SelectedIndex = 0;
			RefreshDisplay();
		}

		#endregion

		#region Methods

		public void RefreshDisplay()
		{
			DisplayHex();
		}

		#endregion

		#region Private functions

		private void DisplayHex()
		{
			HexTextBox.Clear();
            // Start at the 16 byte boundary below actual address
            ushort address = (ushort) (CurrentAddress - (CurrentAddress % hexBytesPerLine));
            int bytesLeft = 128;
            string addressLine = "";
            bool currentAddressInLine = false;
			ushort lineStartAddress = 0;

            while(bytesLeft-- > 0) {
                bool newLine =  (address % hexBytesPerLine == 0);
				if (newLine) {
					addressLine = string.Format(addressLineStart, address);
					lineStartAddress = address;
				}
				if (address == CurrentAddress)
					currentAddressInLine = true;
				addressLine += string.Format(hexLineByte, pagedMemory.ReadByte(address++));
                if (bytesLeft % hexBytesPerLine == 0) {
                    HexTextBox.AppendText(addressLine + Environment.NewLine);
					if (currentAddressInLine) {
						int offset = ((CurrentAddress - lineStartAddress) * 3) + 5;
						bool trackingZ80 = (FollowCombo.SelectedIndex == 2);
						int size = (!trackingZ80) ? 1 : ((int)z80.lastMemory.Size / 8);
						Color color = (!trackingZ80) ? Color.Blue : (z80.lastMemory.Mode == Z80CPU.MemoryMode.Read) ? Color.Green : Color.Red;
						HexTextBox.Select(offset, 3 * size);
						HexTextBox.SelectionColor = color;
						currentAddressInLine = false;
						HexTextBox.DeselectAll();
					}
                }
			}
		}

		#endregion

		#region Event handling

		private void RefreshTimer_Tick(object sender, EventArgs e)
		{
			switch(FollowCombo.SelectedIndex) {
				case 1: CurrentAddress = z80.SP.ToUInt16(); break;
				case 2: CurrentAddress = z80.lastMemory.Address; break;
			}
		}

		private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshDisplay();
		}

		private void FollowCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshTimer_Tick(sender, e);
		}

		private void MemoryForm_VisibleChanged(object sender, EventArgs e)
		{
			RefreshTimer.Enabled = Visible;
		}

		private void AddressNumeric_ValueChanged(object sender, EventArgs e)
		{
			CurrentAddress = (UInt16) AddressNumeric.Value;
		}

		#endregion
	}
}