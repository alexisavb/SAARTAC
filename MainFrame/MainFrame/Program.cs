using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MainFrame.Ventanas;
using System.ComponentModel;

namespace MainFrame
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SplashScreen sp = new SplashScreen();
            MainV mv = new MainV();
            if (sp.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Application.Run(new MainV());
                }
                catch (Win32Exception e)
                {
                    MessageBox.Show("Error: " + e.Message);
                }
            }
        }
    }
}
