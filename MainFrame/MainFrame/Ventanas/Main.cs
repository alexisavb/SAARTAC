using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using Kitware.VTK;
using System.IO;
using MainFrame.Herramientas;

namespace MainFrame
{
    public partial class MainV : Form
    {
        public static Dicom dic;
        int noImgs;
        static int count = 0;
        static Object bloqueador = new Object();
        static Bitmap[] tejidos;
        String folderD;
        static ToolStripProgressBar toolStripProgressBar1;
        bool opened = false;
        List<Dicom> dcms = new List<Dicom>();
        int childs = -1;

        //Constantes de identificación de tejido
        const int GRASA = 0;
        const int HUESO_COMPACTO = 1;
        const int CALCIFICACIONES = 2;
        const int LIQUIDO = 3;
        const int SANGRE = 4;
        const int MATERIA_GRIS = 5;
        const int MATERIA_BLANCA = 6;
        const int TODOS = 7;
        const int original = 8;
        const int seleccion = 9;


        public MainV()
        {
            InitializeComponent();
            toolStripProgressBar1 = new ToolStripProgressBar();
            toolStripProgressBar1.Size = new Size(500, 16);            
            statusStrip1.Items.Add(toolStripProgressBar1);            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            toolStripProgressBar1.Maximum = noImgs;
            obtenerTejidos("" + e.Argument);
            e.Result = e.Argument;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            toolStripStatusLabel1.Text = "Carga de Imágenes Completa";
            Cursor = Cursors.Default;
            DicomIV aux = (DicomIV)this.ActiveMdiChild;
            TejidoIV tiv = new TejidoIV(tejidos, e.Result + "", dcms.ElementAt(aux.Uid));
            tiv.MdiParent = this;            
            tiv.Show();
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Visible = false;
            toolStripButton7.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.selectDirectory(sender, e);
        }

        public void selectTejido()
        {
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Text = "";
            count = 0;
            toolStripProgressBar1.Maximum = noImgs;
            String t = toolStripComboBox1.Text;
            backgroundWorker1.RunWorkerAsync(t);
            Cursor = Cursors.WaitCursor;
            toolStripButton7.Enabled = false;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (opened)
            {
                if (!toolStripComboBox1.Text.Equals(String.Empty))
                    selectTejido();
                else
                    MessageBox.Show("Debe seleccionar un tejido primero","Falta tejido",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            else
                MessageBox.Show("No se ha especificado un directorio", "Falta Directorio", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void obtenerTejidos(String t)
        {
            int a = (int)Math.Floor((double)(noImgs / 10));
            Thread t1 = new Thread(new ThreadStart(delegate { hilo1(0, a, t); }));
            Thread t2 = new Thread(new ThreadStart(delegate { hilo1(a, a + a, t); }));
            Thread t3 = new Thread(new ThreadStart(delegate { hilo1(a + a, 3 * a, t); }));
            Thread t4 = new Thread(new ThreadStart(delegate { hilo1(3 * a, 4 * a, t); }));
            Thread t5 = new Thread(new ThreadStart(delegate { hilo1(4 * a, 5 * a, t); }));
            Thread t6 = new Thread(new ThreadStart(delegate { hilo1(5 * a, 6 * a, t); }));
            Thread t7 = new Thread(new ThreadStart(delegate { hilo1(6 * a, 7 * a, t); }));
            Thread t8 = new Thread(new ThreadStart(delegate { hilo1(7 * a, 8 * a, t); }));
            Thread t9 = new Thread(new ThreadStart(delegate { hilo1(8 * a, 9 * a, t); }));
            Thread t10 = new Thread(new ThreadStart(delegate { hilo1(9 * a, noImgs, t); }));
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();
            t10.Join();
        }

        public void hilo1(int inicio ,int final, String t)
        {
            for (int i = inicio; i < final; i++)
            {
                lock (bloqueador)
                {   
                    DicomIV div = (DicomIV)this.ActiveMdiChild;
                    Bitmap aux = dcms.ElementAt(div.Uid).getTejido(i, t);
                    tejidos[i] = aux;
                    try
                    {
                        backgroundWorker1.ReportProgress(count++);
                    }
                    catch (InvalidOperationException ioe)
                    {
                        Console.WriteLine(ioe.Source);
                    }
                }
            }
        }

        public void fastRender()
        {
            DicomIV aux = (DicomIV)this.ActiveMdiChild;
            if (toolStripComboBox2.Text.Equals("Piel y Hueso"))
            {
                RenderMain rm = new RenderMain(dcms.ElementAt(aux.Uid), 4);
                rm.Show();
            }
            else if (toolStripComboBox2.Text.Equals("Piel"))
            {
                RenderMain rm = new RenderMain(dcms.ElementAt(aux.Uid), 5);
                rm.Show();
            }
            else if (toolStripComboBox2.Text.Equals("Hueso"))
            {
                RenderMain rm = new RenderMain(dcms.ElementAt(aux.Uid), 6);
                rm.Show();
            }
            else if (toolStripComboBox2.Text.Equals("Piel y Sangre"))
            {
                RenderMain rm = new RenderMain(dcms.ElementAt(aux.Uid).getImageData());
                rm.setTejido("Sangre Coagulada");
                rm.Show();
            }
            else if (toolStripComboBox2.Text.Equals("Piel y Liquido"))
            {
                RenderMain rm = new RenderMain(dcms.ElementAt(aux.Uid).getImageData());
                rm.setTejido("Liquido");
                rm.Show();
            }
            else if (toolStripComboBox2.Text.Equals("Piel y Materia Gris"))
            {
                RenderMain rm = new RenderMain(dcms.ElementAt(aux.Uid).getImageData());
                rm.setTejido("Materia Cerebral Gris");
                rm.Show();
            }
            else if (toolStripComboBox2.Text.Equals("Piel y Materia Blanca"))
            {
                RenderMain rm = new RenderMain(dcms.ElementAt(aux.Uid).getImageData());
                rm.setTejido("Materia Cerebral Blanca");
                rm.Show();
            }
        }
        
        private void seleccionarDirectorioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.selectDirectory(sender, e);
        }

        public void selectDirectory(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            DialogResult r = fbd.ShowDialog();
            String f = "";
            if (r == DialogResult.OK)
            {
                f = fbd.SelectedPath;
                folderD = f;
                dic = new Dicom(f);
                if (!(dic.getError() == 0))
                {
                    MessageBox.Show("El directorio especificado no contiene archivos DICOM o esta dañado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    opened = false;
                }
                else
                {
                    dcms.Add(dic);
                    DicomIV div = new DicomIV(f, ++childs);
                    div.MdiParent = this;
                    div.Show();
                    noImgs = div.getSlices();
                    tejidos = new Bitmap[noImgs];
                    opened = true;
                }
            }       
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (opened)
            {
                if (!toolStripComboBox2.Text.Equals(String.Empty))
                    fastRender();
                else
                    MessageBox.Show("No se ha especificado una opción de reconstrucción","Campo faltante",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            else
                MessageBox.Show("No se ha especificado un directorio", "Falta Directorio", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            carga_modelo();
        }

        private void cargarModeloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            carga_modelo();
        }

        public void carga_modelo()
        {
            try
            {
                RenderMain rm = new RenderMain();
                rm.Show();
            }
            catch (StackOverflowException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aRchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
