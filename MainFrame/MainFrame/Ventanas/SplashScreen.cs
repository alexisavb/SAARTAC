using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainFrame.Ventanas
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            Timer.Enabled = true;
            Timer.Interval = 5000;
        }

        private void Tiempo_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
