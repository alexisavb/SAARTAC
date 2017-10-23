using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAARTAC1._1 {
    public partial class Configuracion : Form {
        public Configuracion() {
            InitializeComponent();
            textBox1.Text = Properties.Settings.Default.rutaPython;
            textBox2.Text = Properties.Settings.Default.rutaLecturaDicom;
            Procesos.Value = Properties.Settings.Default.NumeroProcesos;
            iteNum.Value = Properties.Settings.Default.iteNume;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void RutaPython_Click(object sender, EventArgs e) {

        }

        private string AbrirSelecionador() {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Seleccionar archivo python.exe";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = "exe";
            openFileDialog1.Filter = "Ejecutables (*.exe)|*.exe|Archivos python (*.py)|*.py";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                return openFileDialog1.FileName;
            } else {
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            string ruta = AbrirSelecionador();
            if(ruta != null) textBox1.Text = ruta;
        }

        private void button2_Click(object sender, EventArgs e) {
            string ruta = AbrirSelecionador();
            if (ruta != null)  textBox2.Text = ruta;
        }

        private void button4_Click(object sender, EventArgs e) {
            Properties.Settings.Default.rutaPython = textBox1.Text;
            Properties.Settings.Default.rutaLecturaDicom = textBox2.Text;
            Properties.Settings.Default.NumeroProcesos = (int)Procesos.Value;
            Properties.Settings.Default.iteNume = (int)iteNum.Value;
            Properties.Settings.Default.Save();            
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
