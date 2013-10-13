using System;
using System.Windows.Forms;

using Spectrum.Custom;

namespace Test
{
    public partial class DisplayForm : Form
    {
        private SpectrumDisplay display;

        public DisplayForm(SpectrumDisplay display)
        {
            this.display = display;
            InitializeComponent();
            SetFPS(25);
        }

        public void SetFPS(int fps)
        {
            timer.Enabled = (fps != 0);
            if (fps > 0)
                timer.Interval = 1000 / fps;
        }

        public void RefreshDisplay()
        {
            pictureBox.Image = display.GetBitmap();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboBox2.SelectedIndex) {
                case 1: pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; break;
                case 2: pictureBox.SizeMode = PictureBoxSizeMode.Zoom; break;
                default: pictureBox.SizeMode = PictureBoxSizeMode.CenterImage; break;
            }
        }
    }
}
