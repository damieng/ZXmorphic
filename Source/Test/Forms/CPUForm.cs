using System;
using System.Windows.Forms;
using Morphic.Core.CPU.Z80;

namespace Test.Forms
{
    public partial class CPUForm : Form
    {
        private readonly Z80CPU z80;

        public CPUForm(Z80CPU z80)
        {
            InitializeComponent();
            this.z80 = z80;
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshCommon();
            RefreshRegisters();
            RefreshFlags();
            RefreshState();
        }

        public void RefreshState()
        {
            SetValue(StatusListView, 0, z80.tStates.ToString());
            SetValue(StatusListView, 1, String.Format("{0} {1} {2}", z80.InterruptMode, z80.IFF1, z80.IFF2));
            SetValue(StatusListView, 2, z80.InstructionSet.ToString());
            SetValue(StatusListView, 3, z80.IndexerRegisterMode.ToString());
        }

        private void RefreshFlags()
        {
            FlagsCheckedListBox.ItemCheck -= FlagsCheckedListBox_ItemCheck;
            FlagsCheckedListBox.SetItemChecked(0, (z80.F & FlagMask.C) != 0);
            FlagsCheckedListBox.SetItemChecked(1, (z80.F & FlagMask.N) != 0);
            FlagsCheckedListBox.SetItemChecked(2, (z80.F & FlagMask.P) != 0);
            FlagsCheckedListBox.SetItemChecked(3, (z80.F & FlagMask.B3) != 0);
            FlagsCheckedListBox.SetItemChecked(4, (z80.F & FlagMask.H) != 0);
            FlagsCheckedListBox.SetItemChecked(5, (z80.F & FlagMask.B5) != 0);
            FlagsCheckedListBox.SetItemChecked(6, (z80.F & FlagMask.Z) != 0);
            FlagsCheckedListBox.SetItemChecked(7, (z80.F & FlagMask.S) != 0);
            FlagsCheckedListBox.ItemCheck += FlagsCheckedListBox_ItemCheck;
        }

        private void RefreshCommon()
        {
            SetValue(CommonListView, 0, z80.IX.ToString());
            SetValue(CommonListView, 1, z80.IY.ToString());
            SetValue(CommonListView, 2, z80.PC.ToString());
            SetValue(CommonListView, 3, z80.SP.ToString());
            SetValue(CommonListView, 4, String.Format("{0:X2} {1:X2}", z80.I, z80.R));
        }

        private void RefreshRegisters()
        {
            SetValue(MainListView, 0, z80.AF.ToString());
            SetValue(MainListView, 1, z80.BC.ToString());
            SetValue(MainListView, 2, z80.DE.ToString());
            SetValue(MainListView, 3, z80.rHL.ToString());

            SetValue(AltListView, 0, z80.altAF.ToString());
            SetValue(AltListView, 1, z80.altBC.ToString());
            SetValue(AltListView, 2, z80.altDE.ToString());
            SetValue(AltListView, 3, z80.altHL.ToString());
        }

        private void SetValue(ListView listView, int index, string value)
        {
            var listViewItem = listView.Items[index];
            var subItem = listViewItem.SubItems[1];
            if (subItem.Text == value) return;

            if (listViewItem.SubItems.Count > 2)
                listViewItem.SubItems[2].Text = subItem.Text;
            subItem.Text = value;
        }

        private void CPUForm_VisibleChanged(object sender, EventArgs e)
        {
            refreshTimer.Enabled = Visible;
        }

        private void FlagsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            switch (e.Index)
            {
                case 0:
                    z80.F ^= FlagMask.C;
                    break;
                case 1:
                    z80.F ^= FlagMask.N;
                    break;
                case 2:
                    z80.F ^= FlagMask.P;
                    break;
                case 3:
                    z80.F ^= FlagMask.B3;
                    break;
                case 4:
                    z80.F ^= FlagMask.H;
                    break;
                case 5:
                    z80.F ^= FlagMask.B5;
                    break;
                case 6:
                    z80.F ^= FlagMask.Z;
                    break;
                case 7:
                    z80.F ^= FlagMask.S;
                    break;
            }
        }
    }
}