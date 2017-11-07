using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAARTAC1._1 {
    public partial class SeleccionTiraje : Form {
        public SeleccionTiraje(int x) {
            InitializeComponent();
            this.InicioTiraje.Value = x + 1;
            this.FinTiraje.Value = x + 1;
        }
        public int ObtenerInicio() {
            return (int)this.InicioTiraje.Value;
        }
        
        public int ObtenerFin() {
            return (int)this.FinTiraje.Value;
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
