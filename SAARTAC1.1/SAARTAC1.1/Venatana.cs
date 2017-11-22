using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainFrame;
using Kitware.VTK;

namespace SAARTAC1._1 {
    public partial class mainVentana : Form {

        private static MatrizDicom auxUH;
        private Seccion seccion;
        private Regla regla;
        private static int ventanaZoom = 100;
        private bool draw = false, reglaBool = false, zoomCon = false,
            region_creciente = false, seleccion_region = false;
        private List<Bitmap> imagenesCaja1 = new List<Bitmap>();
        private List<Bitmap> imagenesCaja2 = new List<Bitmap>();
        private int id_tac, num_tacs, uh_per, factor_per, bandera = 0, banderaPersonalizada = 100;
        private LecturaArchivosDicom lect;
        private string ruta;
        private Point region_seleccionada;
        private int numeroCentrosKmeans = 6, numeroCentrosCfuzzy = 6;
        private int banderaCentros = 100, contadorCentro = 0;
        private List<Double> centros;
        private Point[,,] ContrasteEnImagen;
        private List<Point> limites_grupo;
        private Dictionary<Color, Point> color_a_umbral = new Dictionary<Color, Point>();
        private int RCPrecision = 0;
        private Label[] labelsColor,labelsMin, labelsMax;
        private Label[] labelsTextMin, labelsTextMax;
        private int [] ventana_default = new int[2];

        private Dicom dic;
        private int noImgs;
        static int opcion = 0;
        static int count = 0;
        static Object bloqueador = new Object();
        static Bitmap[] tejidos;
        private String folderD;
        static ToolStripProgressBar toolStripProgressBar1;
        private bool opened = false;
        private List<Dicom> dcms = new List<Dicom>();
        private int childs = -1;

        public mainVentana() {
            InitializeComponent();
            barraHerramientas.Renderer = new MyRenderer();
            barraIconos3D.Renderer = new MyRenderer();
            barraDeIconos.Renderer = new MyRenderer();
            barraIconosUmbralizacion.Renderer = new MyRenderer();
            barraIconoRegla.Renderer = new MyRenderer();
            barraIconoClasificacion.Renderer = new MyRenderer();
            barraIconoContrste.Renderer = new MyRenderer();
            this.MouseWheel += new MouseEventHandler(ventanaMouseWheel);
            arregloLabel();
        }



        //Eventos de los botones
        //---------------------------------------------------------------------------------------------------------------------------------------------------

        //Evento MouseWheel cambiar imagen con el scroll
        private void ventanaMouseWheel(object sender, MouseEventArgs e) {
            if (mostrarOriginal.Image != null) {
                if (e.Delta > 0) {
                    if (id_tac >= num_tacs - 1)
                        id_tac = 0;
                    else
                        id_tac++;
                    auxUH = lect.obtenerArchivo(id_tac);
                    MostrarImagenOriginal();
                    if (imagenesCaja2.Count > 0)
                        MostrarImagenTratada();
                }
                if (e.Delta < 0) {
                    if (id_tac == 0)
                        id_tac = num_tacs - 1;
                    else
                        id_tac--;
                    auxUH = lect.obtenerArchivo(id_tac);
                    MostrarImagenOriginal();
                    if (imagenesCaja2.Count > 0)
                        MostrarImagenTratada();
                }
                actualizarNumeroImagen();
            }
        }

        //Abrir archivos.
        private void abrirBarraHerramientas_Click(object sender, EventArgs e) {
            try {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) { //verifica si se abrio.                    
                    id_tac = 0;
                    ruta = folderBrowserDialog1.SelectedPath; //se saca el path del archivo.  
                    panelProgressBar.Visible = true;
                    progressBar1.Value = 1;
                    backgroundWorker1.RunWorkerAsync(1);
                    region_seleccionada = new Point(-1, -1);
                    seccion = null;
                } else Console.WriteLine("Hay un problema al abrir el archivo");
            } catch (Exception ex) { MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido"); }
            dic = new Dicom(ruta);            
            if (!(dic.getError() == 0)){
                MessageBox.Show("El directorio especificado no contiene archivos DICOM o esta dañado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                opened = false;
            }
            else{
                dcms.Clear();
                dcms.Add(dic);
                //tejidos = new Bitmap[noImgs];
                opened = true;
            }

        }

        //Avanzar hacia delante sobre la tira.
        private void botonSiguient_Click(object sender, EventArgs e) {
            try {
                if (id_tac >= num_tacs - 1)
                    id_tac = 0;
                else
                    id_tac++;
                auxUH = lect.obtenerArchivo(id_tac);
                MostrarImagenOriginal();
                if (imagenesCaja2.Count > 0)
                    MostrarImagenTratada();
                actualizarNumeroImagen();
            } catch (Exception ex) {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        //ventana para hueso.
        private void huesoBarraDeHerramientas_Click(object sender, EventArgs e) {
            try {
                int lim_inf_ven = -450;
                int lim_sup_ven = 1050;
                generalEscalaGris(lim_inf_ven, lim_sup_ven);
            } catch (Exception ex) {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        //venta para partes blandas.
        private void partesBlandasBarraDeHerramientas_Click(object sender, EventArgs e) {
            try {
                int lim_inf_ven = -125;
                int lim_sup_ven = 225;
                generalEscalaGris(lim_inf_ven, lim_sup_ven);
            } catch (Exception ex) {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        //ventana para pulmón.
        private void pulmónBarraDeHerramientas_Click(object sender, EventArgs e) {
            try {
                int lim_inf_ven = -1200;
                int lim_sup_ven = 800;
                generalEscalaGris(lim_inf_ven, lim_sup_ven);
            } catch (Exception ex) {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        //Rotar 90 grados a la derecha
        private void rotar90DerechaBarraDeHerramientas_Click(object sender, EventArgs e) {
            if (mostrarOriginal.Image != null /*&& mostrarTratada.Image != null*/) {
                mostrarOriginal.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                auxUH = auxUH.GirarDerecha(auxUH);
                mostrarOriginal.Refresh();
                //mostrarTratada.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                //mostrarTratada.Refresh();
            }
        }

        //Rotar 90 grados a la izquierda
        private void rotar90IquierdaBarraDeHerramientas_Click(object sender, EventArgs e) {
            if (mostrarOriginal.Image != null /*&& mostrarTratada.Image != null*/) {
                mostrarOriginal.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                auxUH = auxUH.GirarIzquierda(auxUH);
                mostrarOriginal.Refresh();
                //mostrarTratada.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                //mostrarTratada.Refresh();
            }
        }

        //El evento mousemove /sacar UH/ /sacar el promedio/
        private void mostrarOriginal_MouseMove(object sender, MouseEventArgs e) {
            int x = mostrarOriginal.PointToClient(Cursor.Position).X;
            int y = mostrarOriginal.PointToClient(Cursor.Position).Y;
            if (auxUH != null) resultadoUHMouse.Text = (auxUH.ObtenerUH(x, y)).ToString();

            mostrarOriginal.Refresh();
            if (draw & e.Button == MouseButtons.Left) {
                seccion.setFinal(x, y);
                Graphics objGrafico = this.mostrarOriginal.CreateGraphics();
                seccion.setRectangle();
                objGrafico.DrawRectangle(seccion.getPen(), seccion.getRectangle());


            }
            //PARTE DEL ZOOM
            if (zoomCon) {
                Bitmap zoomImage = (Bitmap)mostrarOriginal.Image;
                Rectangle zoomRect = new Rectangle(x - (ventanaZoom / 2), y - (ventanaZoom / 2), ventanaZoom, ventanaZoom);
                if (zoomRect.Left >= 0 && zoomRect.Top >= 0 && zoomRect.Right <= 512 && zoomRect.Bottom <= 512) {
                    var newzoomImage = zoomImage.Clone(zoomRect, zoomImage.PixelFormat);
                    zoom.Image = newzoomImage;
                    zoom.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        //Evento cuando es mousedown sacar el punto de inicio.
        private void mostrarOriginal_MouseDown(object sender, MouseEventArgs e) {
            int x = mostrarOriginal.PointToClient(Cursor.Position).X;
            int y = mostrarOriginal.PointToClient(Cursor.Position).Y;
            if (e.Button == MouseButtons.Left && auxUH != null && region_creciente) {
                ProcesoRegionCreciente(x, y, RCPrecision);
                region_creciente = false;
                RCPrecision = 0;
                return;
            }
            if (e.Button == MouseButtons.Left && reglaBool != true && auxUH != null && bandera != 1) {
                draw = true;
                seccion = new Seccion(x, y, auxUH);
            }
        }

        //Dibuja el rectangulo completo y saca el promedio.
        private void mostrarOriginal_MouseUp(object sender, MouseEventArgs e) {
            if (draw) {
                Graphics objGrafico = this.mostrarOriginal.CreateGraphics();
                seccion.setRectangle();
                objGrafico.DrawRectangle(seccion.getPen(), seccion.getRectangle());
                resultadoPromedio.Text = (seccion.createAverage()).ToString();
                draw = false;
                int milliseconds = 1200;
                Thread.Sleep(milliseconds);
                mostrarOriginal.Invalidate();
            }
            if (seleccion_region) {
                seleccion_region = false;
                SeleccionTiraje frm = new SeleccionTiraje(id_tac);
                frm.Owner = this;
                frm.ShowDialog();
            
                int inicio = frm.ObtenerInicio() - 1;
                int fin = frm.ObtenerFin() - 1;
                region_seleccionada = new Point(inicio, fin);

                for (int i = inicio; i <= fin; i++) {
                    var seleccion = seccion.obtenerImagen(imagenesCaja1 [i]);
                    Bitmap ajustarImagen = new Bitmap(seleccion, new Size(512, 512));
                    imagenesCaja2 [i] = ajustarImagen;

                }
                MostrarImagenTratada();
            }
        }

        private void MostrarImagenTratada(Bitmap seleccion) {
            mostrarTratada.Image = seleccion;
        }

        //Se activa la bandera para sacar distancia.
        private void distanciaBarraDeHerramientas_Click(object sender, EventArgs e) { reglaBool = true; }

        //saca la distancia /saca el punto inicial/ /saca el punto final/

        private void mostrarOriginal_Click(object sender, EventArgs e){
            int x = mostrarOriginal.PointToClient(Cursor.Position).X;
            int y = mostrarOriginal.PointToClient(Cursor.Position).Y;
            if (lect == null) return;
            if (reglaBool && bandera == 0 && banderaCentros == 0 && contadorCentro == 0){
                regla = new Regla(mostrarOriginal.PointToClient(Cursor.Position).X, mostrarOriginal.PointToClient(Cursor.Position).Y);
                bandera = 1;
            }
            if (reglaBool && bandera == 1 && banderaCentros == 0 && contadorCentro == 0){
                reglaBool = false;
                bandera = 0;
                regla.setFinal(mostrarOriginal.PointToClient(Cursor.Position).X, mostrarOriginal.PointToClient(Cursor.Position).Y);
                Graphics objGrafico = this.mostrarOriginal.CreateGraphics();
                Pen myPen = new Pen(Color.Red, 1);
                objGrafico.DrawLine(myPen, regla.getPointInicio(), regla.getPoinFinal());
                double [] distancias = LecturaArchivosDicom.Pregunta_Python_Dimensiones(auxUH.obtenerRuta());
                resultadoDistancia.Text = (regla.getDistancia(distancias [0], distancias [1])).ToString("N3");
                int milliseconds = 1200;
                Thread.Sleep(milliseconds);
                mostrarOriginal.Invalidate();
            }
            if (banderaCentros == 1 && reglaBool == false && bandera == 0 && contadorCentro != 0){
                if (contadorCentro != numeroCentrosKmeans + 1){
                    restCent.Text = (numeroCentrosKmeans - contadorCentro).ToString();
                    centros.Add(auxUH.ObtenerUH(x, y));
                    contadorCentro++;
                }
                if (contadorCentro == numeroCentrosKmeans + 1){
                    Console.WriteLine("ya entre carnal");
                    panelProgressBar.Visible = true;
                    progressBar1.Value = 1;
                    this.Cursor = Cursors.Default;
                    backgroundWorker1.RunWorkerAsync(4);
                    restCent.Visible = false;
                    centrosRestantes.Visible = false;
                    restCent.Text = null;
                    contadorCentro = 0;
                    banderaCentros = 0;
                }
            }
            if (banderaCentros == 2 && reglaBool == false && bandera == 0 && contadorCentro != 0){
                if (contadorCentro != numeroCentrosCfuzzy + 1){
                    restCent.Text = (numeroCentrosCfuzzy - contadorCentro).ToString();
                    centros.Add(auxUH.ObtenerUH(x, y));
                    contadorCentro++;
                }
                if (contadorCentro == numeroCentrosCfuzzy + 1){
                    panelProgressBar.Visible = true;
                    progressBar1.Value = 1;
                    this.Cursor = Cursors.Default;
                    backgroundWorker1.RunWorkerAsync(5);
                    restCent.Visible = false;
                    centrosRestantes.Visible = false;
                    restCent.Text = null;
                    contadorCentro = 0;
                    banderaCentros = 0;
                }
            }
        }

        private void mostrarDatosPaciente() {
            auxUH = lect.obtenerArchivo(id_tac);
            string nombreP = LecturaArchivosDicom.PreguntaNombre(auxUH.obtenerRuta());
            int edadP = LecturaArchivosDicom.PreguntaEdad(auxUH.obtenerRuta());
            string sexoP = LecturaArchivosDicom.PreguntaSexo(auxUH.obtenerRuta());
            string hospital = LecturaArchivosDicom.PreguntaHospital(auxUH.obtenerRuta());
            string fecha = LecturaArchivosDicom.PreguntaFecha(auxUH.obtenerRuta());
            informacioNombreP.Text = nombreP;
            informacionEdad.Text = edadP.ToString();
            informacionGenero.Text = sexoP;
            informacionHospital.Text = hospital;
            informacionFecha.Text = fecha;
            panelDatosPaciente.Visible = true;
        }

        private void mostrarNumeroImagenes() {
            infoImagenActual.Text = (id_tac + 1).ToString();
            infoNumeroImagenes.Text = num_tacs.ToString();
            panelNumeroImagen.Visible = true;
        }

        private void actualizarNumeroImagen() {
            infoImagenActual.Text = (id_tac + 1).ToString();
        }

        //umbral de agua
        private void aguaBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Agua", Color.FromArgb(98, 184, 230)); }

        //umbral aire
        private void aireBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Aire", Color.FromArgb(60, 11, 239)); }

        //umbral fluido cerebral espinal
        private void fluidoCerebroEspinalBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("FluidoEspinal", Color.FromArgb(44, 213, 6)); }

        //umbral sustancia cerebral blanca
        private void sustanciaCerebralBlancaBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("CerebralBlanca", Color.FromArgb(76, 205, 72)); }

        //umbral sustancia cerebral gris
        private void sustanciaCerebralGrisToolStripMenuItem_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("CerebralGris", Color.FromArgb(235, 16, 73)); }

        //umbral hueso compacto
        private void huesoCompactoBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Hueso compacto", Color.FromArgb(203, 36, 79)); }

        //umbral hueso esponjonso
        private void huesoEsponjosoBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Hueso esponjoso", Color.FromArgb(117, 7, 35)); }

        //umbral grasa
        private void grasaBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Grasa", Color.FromArgb(225, 183, 24)); }

        //umbral higado
        private void higadoBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Higado", Color.FromArgb(15, 23, 86)); }

        //umbral pancreas
        private void pancreasBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Pancreas", Color.FromArgb(220, 48, 13)); }

        //umbral pulmon
        private void pulmónUToolStripMenuItem_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Pulmones", Color.FromArgb(9, 134, 66)); }

        //umbral riñon
        private void riñonBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Riñon", Color.FromArgb(104, 0, 146)); }

        //umbral sangre
        private void sangreBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Sangre", Color.FromArgb(225, 4, 0)); }

        //umbral sangre coagulada
        private void sangreCoaguladaBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("SangreCoagulada", Color.FromArgb(176, 5, 2)); }

        private void contenedorBarraDeIconos_TopToolStripPanel_Click(object sender, EventArgs e) { }

        //Ventana para cerebro.
        private void cerebroBarraDeHerramientas_Click(object sender, EventArgs e) {
            try {
                int lim_inf_ven = -10;
                int lim_sup_ven = 80;
                generalEscalaGris(lim_inf_ven, lim_sup_ven);
            } catch (Exception ex) {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        //Avanzar hacia atrás sobre la tira.
        private void botonAtras_Click(object sender, EventArgs e) {
            try {
                if (id_tac == 0)
                    id_tac = num_tacs - 1;
                else
                    id_tac--;
                auxUH = lect.obtenerArchivo(id_tac);
                MostrarImagenOriginal();
                if (imagenesCaja2.Count > 0)
                    MostrarImagenTratada();
                actualizarNumeroImagen();
            } catch (Exception ex) {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        private void progressBar1_Click(object sender, EventArgs e) { }

        private void AbrirArchivosDICOM(BackgroundWorker bw) {
            lect = new LecturaArchivosDicom(ruta, bw);//se le da el path para sacar los archivos.
            if (bw.CancellationPending)
                return;

            num_tacs = lect.num_archivos();//se saca el número de archivos que hay en el estudio.
            imagenesCaja2.Clear();
            imagenesCaja1.Clear();//se limpia la lista del bitmap.
            for (int i = 0; i < num_tacs; i++)
                imagenesCaja2.Add(null);

            var primerArchivo = lect.obtenerArchivo(0);
            ventana_default = LecturaArchivosDicom.PreguntaVentanaUH(primerArchivo.obtenerRuta());
            MostrarImagenOriginal();
            MostrarImagenTratada();
            bw.ReportProgress(100);
            Thread.Sleep(1000);
            zoomCon = true;
            ContrasteEnImagen = new Point [num_tacs, primerArchivo.obtenerN(), primerArchivo.obtenerM()];
        }

        private List<int[]> obtenerDatosSelecionado() {

            List<int []> datos = new List<int []>();
            if (region_seleccionada.X == -1) {
                for(int i = 0; i < lect.num_archivos(); i++) {
                    var archivo = lect.obtenerArchivo(i);
                    int [] aux = new int [archivo.obtenerN() * archivo.obtenerM()];
                    int pos = 0;
                    foreach(int x in archivo.obtenerMatriz()) {
                        aux [pos++] = x;
                    }
                    datos.Add(aux);
                }
                return datos;
            }

            for (int i = region_seleccionada.X; i <= region_seleccionada.Y; i++) {
                datos.Add(seccion.ObtenerDatosRegion(lect.obtenerArchivo(i)));
            }
            return datos;
        }
        
        private void GenerarImagenesAClases(int[,] clases, BackgroundWorker bw, int tam_datos) {
            int inicio, tam;
            if (region_seleccionada.X == -1) {
                tam = lect.num_archivos();
                inicio = 0;
            } else {
                tam = region_seleccionada.Y - region_seleccionada.X + 1;
                inicio = region_seleccionada.X;
            }
            for (int i = 0; i < tam; i++) {
                var imagen_original = lect.obtenerArchivo(i + inicio).ObtenerImagen();
                imagenesCaja2 [i + inicio] = obtenerImgK(imagen_original, clases, tam_datos, i);
                bw.ReportProgress(90 + (10 * (i + 1)) / lect.num_archivos());
            }
            bw.ReportProgress(100);
            MostrarImagenTratada();
        }

        private void ProcesoKMeans(BackgroundWorker bw) {
            List<int []> datos = obtenerDatosSelecionado();
            kMeans k = new kMeans(datos, numeroCentrosKmeans, Properties.Settings.Default.numIter, bw);
            if (bw.CancellationPending)
                return;
            int [,] clases = k.getClases();
            limites_grupo = k.ObtenerUmbralesGrupos();
            GenerarImagenesAClases(clases, bw, datos [0].Length);
            seccion = null;
            region_seleccionada.X = -1; // resetea la region seleccionada
            //arregloLabel();
            //CodigoDeColores();                        
        }
        
        private void ProcesoFuzzyCMeans(BackgroundWorker bw) {
            List<int []> datos = obtenerDatosSelecionado();
            FuzzyCMeans algoritmo = new FuzzyCMeans(datos, bw, numeroCentrosCfuzzy, Properties.Settings.Default.numIter);
            if (bw.CancellationPending)
                return;
            int [,] clases = algoritmo.getClases();
            limites_grupo = algoritmo.ObtenerUmbralesGrupos();
            GenerarImagenesAClases(clases, bw, datos [0].Length);
            seccion = null;
            region_seleccionada.X = -1;
        }


        //Todos los metodos del background
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            int opcion = (int)e.Argument;
            BackgroundWorker bw = sender as BackgroundWorker;
            switch (opcion) {
                case 1:
                    AbrirArchivosDICOM(bw);
                    break;

                case 2:
                    ProcesoKMeans(bw);                    
                    break;
                case 3:
                    ProcesoFuzzyCMeans(bw);
                    break;
                case 4:
                    ProcesoKMeansCentros(bw);
                    break;
                case 5:
                    ProcesoCFuzzyCentros(bw);
                    break;

            }
        }

        private void arregloLabel(){
            Label [] l = { C1, C2, C3, C4, C5, C6, C7,C8,C9 };
            Label[] tmi = { TMI1, TMI2, TMI3, TMI4, TMI5, TMI6, TMI7, TMI8, TMI9 };
            Label[] tma = { TMA1, TMA2, TMA3, TMA4, TMA5, TMA6, TMA7, TMA8, TMA9 };
            Label[] min = { MI1, MI2, MI3 };
            Label[] max = { MA1, MA2, MA3 };
            labelsColor = l;
            labelsTextMin = tmi;
            labelsTextMax = tma;
            labelsMax = max;
            labelsMin = min;
        }


        private void ProcesoKMeansCentros(BackgroundWorker bw){
            List<int []> datos = obtenerDatosSelecionado();
            kMeans k = new kMeans(datos, numeroCentrosKmeans, Properties.Settings.Default.numIter, bw, centros);
            if (bw.CancellationPending)
                return;
            int [,] clases = k.getClases();
            
            limites_grupo = k.ObtenerUmbralesGrupos();
            for (int i = region_seleccionada.X; i <= region_seleccionada.Y; i++) {
                imagenesCaja2 [i] = (obtenerImgK(lect.obtenerArchivo(i).ObtenerImagen(), clases, datos [i].Length, i));
                bw.ReportProgress(90 + (10 * (i + 1)) / lect.num_archivos());
            }
            bw.ReportProgress(100);
            MostrarImagenTratada();
            GenerarImagenesAClases(clases, bw, datos [0].Length);
            seccion = null;
            region_seleccionada.X = -1; // resetea la region seleccionada
        }

        private void ProcesoCFuzzyCentros(BackgroundWorker bw){
            List<int []> datos = obtenerDatosSelecionado();
            FuzzyCMeans algoritmo = new FuzzyCMeans(datos, centros, bw, numeroCentrosCfuzzy, Properties.Settings.Default.numIter);
            if (bw.CancellationPending)
                return;
            int [,] clases = algoritmo.getClases();
            limites_grupo = algoritmo.ObtenerUmbralesGrupos();
            for (int i = region_seleccionada.X; i <= region_seleccionada.Y; i++) {
                imagenesCaja2 [i] = (obtenerImgK(lect.obtenerArchivo(i).ObtenerImagen(), clases, datos [i].Length, i));
                bw.ReportProgress(90 + (10 * (i + 1)) / lect.num_archivos());
            }
            bw.ReportProgress(100);
            MostrarImagenTratada();
            GenerarImagenesAClases(clases, bw, datos [0].Length);
            seccion = null;
            region_seleccionada.X = -1; // resetea la region seleccionada
        }


        //reporte del progreso abrir archivos dicom, barra de progreso
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) { progressBar1.Value = e.ProgressPercentage; }

        //abrir archivo icono
        private void abrirBarraIconos_Click(object sender, EventArgs e) { abrirBarraHerramientas_Click(sender, e); }

        //Rotar 90 grados izquirda icono
        private void toolStripButton1_Click(object sender, EventArgs e) { rotar90IquierdaBarraDeHerramientas_Click(sender, e); }

        //Rotar 90 grados derecha icono
        private void toolStripButton2_Click(object sender, EventArgs e) { rotar90DerechaBarraDeHerramientas_Click(sender, e); }

        //Umbral de huevo icono
        private void toolStripButton5_Click(object sender, EventArgs e) { huesoCompactoBarraDeHerramientas_Click(sender, e); }

        //umbral de agua icono
        private void toolStripButton6_Click(object sender, EventArgs e) { aguaBarraDeHerramientas_Click(sender, e); }

        //umbral de sangre icono
        private void toolStripButton7_Click(object sender, EventArgs e) { sangreBarraDeHerramientas_Click(sender, e); }

        //Activar regla icono
        private void toolStripButton10_Click(object sender, EventArgs e) { reglaBool = true; }

        //ventana default
        private void defaultBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja1.Clear(); MostrarImagenOriginal(); }

        //ventana default iconos
        private void predeterminadoIcono_Click(object sender, EventArgs e) { imagenesCaja1.Clear(); MostrarImagenOriginal(); }

        //ventana cerebro iconos
        private void cerebroIcono_Click(object sender, EventArgs e) { cerebroBarraDeHerramientas_Click(sender, e); }

        //venatana hueso iconos
        private void huesoContrasteIcono_Click(object sender, EventArgs e) { huesoBarraDeHerramientas_Click(sender, e); }

        //Sacar UH en la imagen tratada
        private void mostrarTratada_MouseMove(object sender, MouseEventArgs e) {
            int x = mostrarTratada.PointToClient(Cursor.Position).X;
            int y = mostrarTratada.PointToClient(Cursor.Position).Y;
            if (auxUH == null)  return;
            resultadoUHMouse.Text = (auxUH.ObtenerUH(x, y)).ToString();
            //PARTE DEL ZOOM
            if (zoomCon) {
                if (mostrarTratada.Image != null) {
                    Bitmap zoomTratedImage = (Bitmap)mostrarTratada.Image;
                    Rectangle zoomRect2 = new Rectangle(x - (ventanaZoom / 2), y - (ventanaZoom / 2), ventanaZoom, ventanaZoom);
                    if (zoomRect2.Left >= 0 && zoomRect2.Top >= 0 && zoomRect2.Right <= 512 && zoomRect2.Bottom <= 512) {
                        var newzoomImage = zoomTratedImage.Clone(zoomRect2, zoomTratedImage.PixelFormat);
                        zoom.Image = newzoomImage;
                        zoom.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
        }

        //Cluster de k-means 
        private void kmeans_Click(object sender, EventArgs e){
            if (lect == null)
                return;
            panelProgressBar.Visible = true;
            progressBar1.Value = 1;
            backgroundWorker1.RunWorkerAsync(2);
        }

        //Cluster C-fuzzy
        private void fuzzy_Click(object sender, EventArgs e) {

            if (lect == null)
                return;
            panelProgressBar.Visible = true;
            progressBar1.Value = 1;
            backgroundWorker1.RunWorkerAsync(3);
        }

        //Cluster de k-means icono
        private void kmeansIcono_Click(object sender, EventArgs e) { kmeans_Click(sender, e); }

        //Cluster C-fuzzy icono
        private void fuzzyIcono_Click(object sender, EventArgs e) { fuzzy_Click(sender, e); }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled)
                MessageBox.Show("The task has been cancelled");
            else if (e.Error != null)
                MessageBox.Show("Error. Details: " + (e.Error as Exception).ToString());
            else {

                progressBar1.Value = 100;
                Thread.Sleep(500);
                mostrarDatosPaciente();
                mostrarNumeroImagenes();

                panelProgressBar.Visible = false;                
                CodigoDeColores();                
            }

        }

        //zoom para restarle
        private void toolStripButton3_Click(object sender, EventArgs e) {
            if (ventanaZoom == 100) return;
            ventanaZoom += 20;
        }

        //zoom para sumarle
        private void toolStripButton4_Click(object sender, EventArgs e) {
            if (ventanaZoom == 20) return;
            ventanaZoom -= 20;
        }

        //exportar original icono
        private void exportarOriginalIcono_Click(object sender, EventArgs e) {
            if (mostrarOriginal.Image == null) return;
            SaveFileDialog f = new SaveFileDialog();
            f.Filter = "JPG(.JPG)|.jpg|Png Image (.png)|*.png";
            if (f.ShowDialog() == DialogResult.OK)
                mostrarOriginal.Image.Save(f.FileName);
        }

        //exportar tratada icono
        private void exportarTratadaIcono_Click(object sender, EventArgs e) {
            if (mostrarTratada.Image == null) return;
            SaveFileDialog f = new SaveFileDialog();
            f.Filter = "JPG(.JPG)|.jpg|Png Image (.png)|*.png";
            if (f.ShowDialog() == DialogResult.OK)
                mostrarTratada.Image.Save(f.FileName);
        }

        //exportar original 
        private void exportarOriginal_Click(object sender, EventArgs e) { exportarOriginalIcono_Click(sender, e); }

        //exportar tratada
        private void exportarTratada_Click(object sender, EventArgs e) { exportarTratadaIcono_Click(sender, e); }

        //acciones de la personalización
        private void botonAplicarPersonalizada_Click(object sender, EventArgs e) {
            try {
                int centro = int.Parse(valorCentro.Text);
                int ancho = int.Parse(valorAncho.Text);
                int mitad = ancho / 2;
                if (banderaPersonalizada == 0) {
                    generalEscalaGris(centro - mitad, centro + mitad);
                }
                if (banderaPersonalizada == 1) {
                    imagenesCaja2.Clear();
                    dibujarUmbral(centro, ancho, Color.FromArgb(40, 67, 120));
                }                                        
                if(banderaPersonalizada == 2){                    
                    numeroCentrosKmeans = centro;
                    numeroCentrosCfuzzy = ancho;
                    MessageBox.Show("El cambio para el número de centro fue realizado. \nTécnica 1 = " + centro + "\nTécnica 2 = " + ancho, "Correcto");
                }
                             
            }
            catch (Exception ex) { 
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
            banderaPersonalizada = 100;            
            panelPersonalizada.Visible = false;
        }

        //personalizar la venta para unbralización
        private void personalizadaBarraDeHerramientas_Click(object sender, EventArgs e){
            valorCentro.Text = null;
            valorAncho.Text = null;
            textoUHPerso.Visible = true;
            textoUmbralPersonal.Visible = true;
            textoToleranciaUH.Visible = true;
            configNumCentros.Visible = false;
            numPrecAlta.Visible = false;
            nunPrecMedia.Visible = false;
            textoCentroPersonalizada.Visible = false;
            textoAnchoPersonalizada.Visible = false;
            textoPersonalizada.Visible = false;
            panelPersonalizada.Visible = true;
            banderaPersonalizada = 1;
        }

        //personalizar la venta para contraste
        private void pesonalizadaVBarraDeHerramientas_Click(object sender, EventArgs e){
            valorCentro.Text = null;
            valorAncho.Text = null;
            textoUHPerso.Visible = false;
            textoUmbralPersonal.Visible = false;
            textoToleranciaUH.Visible = false;
            configNumCentros.Visible = false;
            numPrecAlta.Visible = false;
            nunPrecMedia.Visible = false;
            textoCentroPersonalizada.Visible = true;
            textoAnchoPersonalizada.Visible = true;
            textoPersonalizada.Visible = true;
            banderaPersonalizada = 0;
            panelPersonalizada.Visible = true;
        }

        private void configCluster_Click(object sender, EventArgs e){
            valorCentro.Text = numeroCentrosKmeans.ToString();
            valorAncho.Text = numeroCentrosCfuzzy.ToString();
            configNumCentros.Visible = true;
            numPrecAlta.Visible = true;
            nunPrecMedia.Visible = true;
            textoUHPerso.Visible = false;
            textoUmbralPersonal.Visible = false;
            textoToleranciaUH.Visible = false;
            textoCentroPersonalizada.Visible = false;
            textoAnchoPersonalizada.Visible = false;
            textoPersonalizada.Visible = false;
            panelPersonalizada.Visible = true;
            banderaPersonalizada = 2;
        }

        //indicar centros k-means
        private void insertCentrosMedia_Click(object sender, EventArgs e){
            if (lect == null) return;
            this.Cursor = Cursors.Cross;
            setIndicarCentros();
            restCent.Text = numeroCentrosKmeans.ToString();
            banderaCentros = 1;
        }

        //configuración básica para poner la información de los centros
        private void setIndicarCentros(){
            if (contadorCentro == 0) contadorCentro++;
            else contadorCentro = 1;
            restCent.Visible = true;
            centrosRestantes.Visible = true;
            centros = new List<Double>();
        }

        private void insertCentroAlta_Click(object sender, EventArgs e){
            if (lect == null) return;
            this.Cursor = Cursors.Cross;
            setIndicarCentros();
            restCent.Text = numeroCentrosCfuzzy.ToString();
            banderaCentros = 2;
        }

        ///---------------------------------------------------------------------------------------------------------------------------------------------------


        //Funciones 
        //***************************************************************************************************************************************************** 
        //mostrar imagen sin tratamiento.


        private void dibujarUmbral(string lectura, Color color) {
            try {
                Cursor.Current = Cursors.WaitCursor;
                Umbralizacion operaciones = new Umbralizacion(lect.num_archivos());
                int N = lect.num_archivos();
                Thread [] threadsArray = new Thread [N];

                for (int i = 0; i < N; i++) {
                    MatrizDicom dicom = lect.obtenerArchivo(i);
                    ParametroUmbralizacion aux = new ParametroUmbralizacion(lectura, dicom, i, color);
                    threadsArray [i] = new Thread(() => operaciones.UmbraHilos(aux));

                }
                for (int i = 0; i < N; i++) {
                    threadsArray [i].Start();
                }
                for (int i = 0; i < N; i++) {
                    threadsArray [i].Join();
                    imagenesCaja2.Add(Umbralizacion.imagenes [i]);
                }

                MostrarImagenTratada();
                Cursor.Current = Cursors.Default;
            } catch (Exception ex) {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }


        private void dibujarUmbral(int valorUH, int tolerancia, Color color) {
            try {
                Umbralizacion operaciones = new Umbralizacion(valorUH, tolerancia, lect.num_archivos());
                int N = lect.num_archivos();
                Thread [] threadsArray = new Thread [N];

                for (int i = 0; i < N; i++) {
                    string tipo = "Personalizada";
                    MatrizDicom dicom = lect.obtenerArchivo(i);
                    ParametroUmbralizacion aux = new ParametroUmbralizacion(tipo, dicom, i, color);
                    threadsArray [i] = new Thread(() => operaciones.UmbraHilos(aux));

                }
                for (int i = 0; i < N; i++) {
                    threadsArray [i].Start();
                }
                for (int i = 0; i < N; i++) {
                    threadsArray [i].Join();
                    imagenesCaja2.Add(Umbralizacion.imagenes [i]);
                }

                MostrarImagenTratada();
            } catch (Exception ex) {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }


        private void exportarBarraIconos_Click(object sender, EventArgs e) { }

        private void butonCancelarProceso_Click(object sender, EventArgs e) {
            backgroundWorker1.CancelAsync();
            panelProgressBar.Visible = false;

        }

        private void panelPersonalizada_Paint(object sender, PaintEventArgs e) {

        }

        

        private void regionCrecienteToolStripMenuItem_Click(object sender, EventArgs e) {
            region_creciente = false;
        }
        

        private void mostrarTratada_MouseDown(object sender, MouseEventArgs e) {
            if (ContrasteEnImagen == null)
                return;

            int x = mostrarTratada.PointToClient(Cursor.Position).X;
            int y = mostrarTratada.PointToClient(Cursor.Position).Y;            
            try {
                Bitmap imagen = (Bitmap) mostrarTratada.Image;
                Color color_seleccionado = imagen.GetPixel(x, y);
                Console.WriteLine(color_seleccionado);
                int LS = color_a_umbral [color_seleccionado].X;
                int LI = color_a_umbral [color_seleccionado].Y;
                generalEscalaGris(LI, LS);
                Console.WriteLine("inferior: " + LI + "   superior: " + LS);
            } catch {
                Console.WriteLine("No hay ningun contraste");
            }
        }

        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e) {
            Configuracion frm = new Configuracion();
            frm.Show();
        }


        private void RCBaja_Click(object sender, EventArgs e){
            region_creciente = true;
            RCPrecision = 1;
        }

        private void RCMedia_Click(object sender, EventArgs e){
            region_creciente = true;
            RCPrecision = 3;
        }

        private void RCAlta_Click(object sender, EventArgs e){
            region_creciente = true;
            RCPrecision = 5;
        }

        private void toolStripButton8_Click(object sender, EventArgs e){
            //reconstruccion();
        }
        private void contenedorBarraDeIconos_ContentPanel_Load(object sender, EventArgs e){

        }

        private void mainVentana_Load(object sender, EventArgs e) {
        }

        private void aguaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderMain rm = new RenderMain(dic, "Liquido", 2);
            rm.Show();
        }

        private void huesoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RenderMain rm = new RenderMain(dcms.ElementAt(0), 6);
            rm.Show();
        }

        private void grasaToolStripMenuItem_Click(object sender, EventArgs e){
            RenderMain rm = new RenderMain(dic, "Grasa", 2);
            rm.Show();
        }

        private void sangreToolStripMenuItem1_Click(object sender, EventArgs e){
            RenderMain rm = new RenderMain(dic, "Sangre Coagulada", 2);
            rm.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            RenderMain rm = new RenderMain(dic, "Liquido", 2);
            rm.Show();
        }

        private void huesoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderMain rm = new RenderMain(dcms.ElementAt(0), 6);
            rm.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            RenderMain rm = new RenderMain(dic, "Grasa", 2);
            rm.Show();
        }

        private void sangreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderMain rm = new RenderMain(dic,"Sangre Coagulada",2);            
            rm.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void ProcesoRegionCreciente(int y, int x, int calidad = 1) {

            RegionCreciente aux = new RegionCreciente(auxUH.obtenerMatriz(), y, x);
            int [,] mancha = aux.ObtenerRegion(calidad);
            imagenesCaja2 [id_tac] = CrearImagenRegion(mancha);
            MostrarImagenTratada();
        }

        private void seleccionarToolStripMenuItem_Click(object sender, EventArgs e) {
            seleccion_region = true;
        }

        private Bitmap CrearImagenRegion(int [,] mancha) {
            int N = auxUH.obtenerN();
            int M = auxUH.obtenerM();
            Bitmap salida = new Bitmap(N, M);
            for (int i = 0; i < N; i++) {
                for (int j = 0; j < M; j++) {
                    salida.SetPixel(i, j, (mancha [i, j] == 1 ? Color.White : Color.Black));
                }
            }
            return salida;
        }

        private void MostrarImagenOriginal() {
            if (lect == null) return;
            if (imagenesCaja1.Count() <= 0) {
                generalEscalaGris(ventana_default[0] - ventana_default[1], ventana_default[0] + ventana_default[1]);
                return;
            }
            mostrarOriginal.Image = imagenesCaja1 [id_tac];
        }

        //mostrar imagen con tratamiento.
        private void MostrarImagenTratada() {
            if (lect == null) return;
            if (imagenesCaja2.Count() > 0) mostrarTratada.Image = imagenesCaja2 [id_tac];
            else mostrarTratada.Image = null;
        }

        //genera escala de gris.
        private void generalEscalaGris(int lim_inf, int lim_sup) {
            Cursor.Current = Cursors.WaitCursor;
            imagenesCaja1.Clear();
            int N = lect.num_archivos();
            Contraste operaciones = new Contraste(N);
            Thread [] threadsArray = new Thread [N];

            for (int i = 0; i < N; i++) {
                MatrizDicom dicom = lect.obtenerArchivo(i);
                ParametroContraste aux = new ParametroContraste(dicom.matriz, lim_inf, lim_sup, i);
                threadsArray [i] = new Thread(() => operaciones.obtenerImagenConVentana(aux));
            }
            for (int i = 0; i < N; i++) {
                threadsArray [i].Start();
            }
            for (int i = 0; i < N; i++) {
                threadsArray [i].Join();
                imagenesCaja1.Add(Contraste.imagenes [i]);
            }

            MostrarImagenTratada();
            MostrarImagenOriginal();
            Cursor.Current = Cursors.Default;
        }

        //genera la imagen con el umbral
        private Bitmap obtenerImagenUmbral(bool [,] umbral, Bitmap matrizOriginal, Color color) {
            Bitmap resultado = new Bitmap(matrizOriginal);
            int N = umbral.GetLength(0);
            int M = umbral.GetLength(1);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    if (umbral [i, j])
                        resultado.SetPixel(i, j, color);
            return resultado;
        }



        //se genera la imagen con base a las clases que se tiene
        private Bitmap obtenerImgK(Bitmap matrizOriginal, int [,] lista, int tam, int p) {
            Bitmap resultado;
            if (seccion == null) {
                resultado = matrizOriginal;
            } else {
                resultado = new Bitmap(seccion.obtenerImagen(matrizOriginal));
            }
            List<Color> colores = new List<Color>() { Color.Black, Color.Red, Color.Blue, Color.Orange, Color.Yellow, Color.Pink, Color.Purple, Color.Aqua, Color.Green, Color.Chocolate};
            int N = resultado.Height;
            int i = 0;
            color_a_umbral.Clear();
            for(int j = 0; j < limites_grupo.Count(); j++) {
                //color_a_umbral [colores [j]] = limites_grupo [j];
                resultado.SetPixel(0, 0, colores [j]);
                color_a_umbral [resultado.GetPixel(0, 0)] = limites_grupo[j];
            }
            for (int j = 0; j < tam; j++) {
                int x = j % N;
                int y = j / N;
                resultado.SetPixel(y, x, colores [lista [p, j]]);
                var color = resultado.GetPixel(y, x);
                ContrasteEnImagen [p, y, x] = limites_grupo [lista [p, j]];
                
            }
            return new Bitmap(resultado, new Size(512, 512));
        }

        private void CodigoDeColores() {
            int con = 0;            
            for(int i = 0; i < labelsColor.Length; i++) {
                labelsColor[i].Visible = false;
                labelsTextMin[i].Visible = false;
                labelsTextMax[i].Visible = false;                
            }
            for(int i = 0; i < labelsMax.Length; i++){
                labelsMax[i].Visible = false;
                labelsMin[i].Visible = false;
            }
            int cantidad = color_a_umbral.Count - 1;
            if(cantidad >= 0){
                //Console.WriteLine( cantidad + " cant");
                //Console.WriteLine(( cantidad / 3) + " cant");
                for (int i = 0; i <= (cantidad / 3); i++){
                    labelsMax[i].Visible = true;
                    labelsMin[i].Visible = true;
                }
            }                

            foreach (var i in color_a_umbral) {                                                       
                Color color_grupo = i.Key;
                int limite_superior = i.Value.X;
                int limite_inferior = i.Value.Y;
                labelsColor[con].BackColor = color_grupo;
                labelsColor[con].ForeColor = color_grupo;
                labelsColor[con].Visible = true;
                labelsTextMin[con].Text = (limite_inferior).ToString();
                labelsTextMax[con].Text = (limite_superior).ToString();
                labelsTextMin[con].Visible = true;
                labelsTextMax[con].Visible = true;
                con++;
                //Console.WriteLine(i.Value);
                //Console.WriteLine(i.Key);                
            }
        }



        //*****************************************************************************************************************************************************                            
    }
}
