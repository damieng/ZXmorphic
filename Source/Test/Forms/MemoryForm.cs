using System;
using System.Drawing;
using System.Windows.Forms;
using Morphic.Core.CPU.Z80;
using Morphic.Core.Memory;
using Spectrum.Custom;

namespace Test.Forms
{
    public partial class MemoryForm : Form
    {
        private const string AddressLineStart = "{0:X4}";
        private const string HexLineByte = " {0:X2}";
        private const uint HexBytesPerLine = 16;

        private readonly PagedMemory pagedMemory;
        private readonly Z80CPU z80;
        private SpectrumDisplay display;

        private UInt16 currentAddress;

        public UInt16 CurrentAddress
        {
            get { return currentAddress; }
            set
            {
                if (currentAddress != value)
                {
                    currentAddress = value;
                    AddressNumeric.Value = currentAddress;
                    RefreshDisplay();
                }
            }
        }

        public MemoryForm(PagedMemory pagedMemory, Z80CPU z80)
        {
            this.pagedMemory = pagedMemory;
            this.z80 = z80;
            display = new SpectrumDisplay(pagedMemory.AllPages[1]);
            InitializeComponent();

            FollowCombo.SelectedIndex = 2;
            BankCombo.SelectedIndex = 0;
            RefreshDisplay();
        }

        public void RefreshDisplay()
        {
            DisplayHex();
        }

        private void DisplayHex()
        {
            HexTextBox.Clear();
            // Start at the 16 byte boundary below actual address
            var address = (ushort)(CurrentAddress - (CurrentAddress % HexBytesPerLine));
            var bytesLeft = 128;
            var addressLine = "";
            var currentAddressInLine = false;
            ushort lineStartAddress = 0;

            while (bytesLeft-- > 0)
            {
                var newLine = (address % HexBytesPerLine == 0);
                if (newLine)
                {
                    addressLine = string.Format(AddressLineStart, address);
                    lineStartAddress = address;
                }
                if (address == CurrentAddress)
                    currentAddressInLine = true;
                addressLine += string.Format(HexLineByte, pagedMemory.ReadByte(address++));
                if (bytesLeft % HexBytesPerLine != 0) continue;

                HexTextBox.AppendText(addressLine + Environment.NewLine);
                if (currentAddressInLine)
                {
                    var offset = ((CurrentAddress - lineStartAddress) * 3) + 5;
                    var trackingZ80 = (FollowCombo.SelectedIndex == 2);
                    var size = (!trackingZ80) ? 1 : ((int)z80.lastMemory.Size / 8);
                    var color = (!trackingZ80)
                        ? Color.Blue
                        : (z80.lastMemory.Mode == Z80CPU.MemoryMode.Read) ? Color.Green : Color.Red;
                    HexTextBox.Select(offset, 3 * size);
                    HexTextBox.SelectionColor = color;
                    currentAddressInLine = false;
                    HexTextBox.DeselectAll();
                }
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            switch (FollowCombo.SelectedIndex)
            {
                case 1:
                    CurrentAddress = z80.SP.ToUInt16();
                    break;
                case 2:
                    CurrentAddress = z80.lastMemory.Address;
                    break;
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
            CurrentAddress = (UInt16)AddressNumeric.Value;
        }
    }
}