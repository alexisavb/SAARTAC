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
    public partial class mainVentana : Form {
        public mainVentana() {
            InitializeComponent();
            barraHerramientas.Renderer = new MyRenderer();            
        }       
    }
}
