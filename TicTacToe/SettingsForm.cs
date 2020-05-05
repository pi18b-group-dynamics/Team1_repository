using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            sizeN.Value = Settings.Size;
            winN.Value = Settings.Win;
            xB.BackColor = Settings.XColor;
            oB.BackColor = Settings.OColor;
        }

        private void xB_Click(object sender, EventArgs e)
        {
            if(colorD.ShowDialog() == DialogResult.OK)
            {
                xB.BackColor = colorD.Color;
            }
        }

        private void oB_Click(object sender, EventArgs e)
        {
            if (colorD.ShowDialog() == DialogResult.OK)
            {
                oB.BackColor = colorD.Color;
            }
        }

        private void backB_Click(object sender, EventArgs e)
        {
            Settings.Size = (int)sizeN.Value;
            Settings.Win = (int)winN.Value;
            Settings.XColor = xB.BackColor;
            Settings.OColor = oB.BackColor;
            Dispose();
        }
    }
}
