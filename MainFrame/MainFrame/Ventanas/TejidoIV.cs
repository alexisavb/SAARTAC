using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MainFrame.Clasificar;

namespace MainFrame
{
    public partial class TejidoIV : Form
    {
        Bitmap[] Imgs;
        int PtrLista = 0;
        String tejido;
        Dicom dicom;

        public TejidoIV(Bitmap[] i ,String tej, Dicom dcm)
        {
            InitializeComponent();
            dicom = dcm;
            Imgs = i;
            tejido = tej;
            this.Text = dcm.getPatientName() + ":" + tejido;
            this.pictureBox1.MouseWheel += new MouseEventHandler(cambiarImagen_MouseWheel);
            this.pictureBox1.Image = Imgs[PtrLista];
            this.pictureBox1.Focus();
        }

        private void cambiarImagen_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (e.Delta > 0 && PtrLista < Imgs.Length- 1)
                    PtrLista++;
                else if (e.Delta < 0 && PtrLista > 0)
                    PtrLista--;
                else
                    PtrLista = 0;
                //    toolStripStatusLabel1.Text = "DICOM: " + dicom[PtrLista].DicomFileName;
            }
            pictureBox1.Image = Imgs[PtrLista];
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void marchingCubesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderMain rm = new RenderMain(dicom, tejido,2);
            rm.Show();
        }

        private void rayTracingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderMain rm = new RenderMain(dicom, tejido,1);
            rm.Show();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            //extrarvoi
            ExtraerVOI evoi = new ExtraerVOI(Imgs, dicom,tejido);
            evoi.Show();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
