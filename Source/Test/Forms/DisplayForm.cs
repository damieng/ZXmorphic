using System;
using System.Windows.Forms;
using Spectrum.Custom;

namespace Test.Forms
{
    public partial class DisplayForm : Form
    {
        private readonly SpectrumDisplay display;

        public DisplayForm(SpectrumDisplay display)
        {
            this.display = display;
            InitializeComponent();
            SetFps(25);
        }

        public void SetFps(int fps)
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
            switch (fitComboBox.SelectedIndex)
            {
                case 1: pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; break;
                case 2: pictureBox.SizeMode = PictureBoxSizeMode.Zoom; break;
                default: pictureBox.SizeMode = PictureBoxSizeMode.CenterImage; break;
            }
        }
    }
}