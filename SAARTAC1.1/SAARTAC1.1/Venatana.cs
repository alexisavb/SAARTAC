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

namespace SAARTAC1._1 {
    public partial class mainVentana : Form {

        private static MatrizDicom auxUH;
        private Seccion seccion;
        private Regla regla;
        private static int ventanaZoom = 100;
        private bool draw = false, reglaBool = false, zoomCon = false;
        private List<Bitmap> imagenesCaja1 = new List<Bitmap>();
        private List<Bitmap> imagenesCaja2 = new List<Bitmap>();
        private int id_tac, num_tacs, uh_per, factor_per, bandera = 0;
        private LecturaArchivosDicom lect;
        private string ruta;
        //private int abrirArchivo = 0;
        
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
        }

        //Evento MouseWheel
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
            }
        }

        //Eventos de los botones
        //---------------------------------------------------------------------------------------------------------------------------------------------------
        //Abrir archivos.
        private void abrirBarraHerramientas_Click(object sender, EventArgs e){
            
            try{
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK){ //verifica si se abrio.                    
                    id_tac = 0;
                    ruta = folderBrowserDialog1.SelectedPath; //se saca el path del archivo.  
                    panelProgressBar.Visible = true;
                    progressBar1.Value = 1;
                    backgroundWorker1.RunWorkerAsync(1);
                }
                else Console.WriteLine("Hay un problema al abrir el archivo");
            }
            catch (Exception ex) { MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido"); }

            
        }

        //Avanzar hacia delante sobre la tira.
        private void botonSiguient_Click(object sender, EventArgs e){
            try{
                if (id_tac >= num_tacs - 1)
                    id_tac = 0;
                else
                    id_tac++;
                auxUH = lect.obtenerArchivo(id_tac);
                MostrarImagenOriginal();
                if (imagenesCaja2.Count > 0)
                    MostrarImagenTratada();
            }catch (Exception ex){
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        //ventana para hueso.
        private void huesoBarraDeHerramientas_Click(object sender, EventArgs e){
            try{
                int lim_inf_ven = -450;
                int lim_sup_ven = 1050;
                generalEscalaGris(lim_inf_ven, lim_sup_ven);
            }catch (Exception ex){
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
}

        //venta para partes blandas.
        private void partesBlandasBarraDeHerramientas_Click(object sender, EventArgs e){
            try {
                int lim_inf_ven = -125;
                int lim_sup_ven = 225;
                generalEscalaGris(lim_inf_ven, lim_sup_ven);
            }catch (Exception ex){
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        //ventana para pulmón.
        private void pulmónBarraDeHerramientas_Click(object sender, EventArgs e){
            try{ 
                int lim_inf_ven = -1200;
                int lim_sup_ven = 800;
                generalEscalaGris(lim_inf_ven, lim_sup_ven);
            }catch (Exception ex){
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        //Rotar 90 grados a la derecha
        private void rotar90DerechaBarraDeHerramientas_Click(object sender, EventArgs e){
            if (mostrarOriginal.Image != null /*&& mostrarTratada.Image != null*/){
                mostrarOriginal.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                auxUH = auxUH.GirarDerecha(auxUH);
                mostrarOriginal.Refresh();
                //mostrarTratada.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                //mostrarTratada.Refresh();
            }
        }

        //Rotar 90 grados a la izquierda
        private void rotar90IquierdaBarraDeHerramientas_Click(object sender, EventArgs e){
            if (mostrarOriginal.Image != null /*&& mostrarTratada.Image != null*/){
                mostrarOriginal.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                auxUH = auxUH.GirarIzquierda(auxUH);
                mostrarOriginal.Refresh();
                //mostrarTratada.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                //mostrarTratada.Refresh();
            }
        }

        //El evento mousemove /sacar UH/ /sacar el promedio/
        private void mostrarOriginal_MouseMove(object sender, MouseEventArgs e){
            int x = mostrarOriginal.PointToClient(Cursor.Position).X;
            int y = mostrarOriginal.PointToClient(Cursor.Position).Y;
            if (auxUH != null) resultadoUHMouse.Text = (auxUH.ObtenerUH(x, y)).ToString();

            mostrarOriginal.Refresh();
            if (draw & e.Button == MouseButtons.Left){
                seccion.setFinal(x, y);
                Graphics objGrafico = this.mostrarOriginal.CreateGraphics();
                seccion.setRectangle();
                objGrafico.DrawRectangle(seccion.getPen(), seccion.getRectangle());
                

            }
            //PARTE DEL ZOOM
            if (zoomCon){
               Bitmap zoomImage = imagenesCaja1[id_tac];
                Rectangle zoomRect = new Rectangle(x - (ventanaZoom / 2), y - (ventanaZoom / 2), ventanaZoom, ventanaZoom);
                if (zoomRect.Left >= 0 && zoomRect.Top >= 0 && zoomRect.Right <= 512 && zoomRect.Bottom <= 512){
                    var newzoomImage = zoomImage.Clone(zoomRect, zoomImage.PixelFormat);
                    zoom.Image = newzoomImage;
                    zoom.SizeMode = PictureBoxSizeMode.StretchImage;
                }                
            }
        }

        //Evento cuando es mousedown sacar el punto de inicio.
        private void mostrarOriginal_MouseDown(object sender, MouseEventArgs e){
            if (e.Button == MouseButtons.Left && reglaBool != true && auxUH != null && bandera != 1){                
                draw = true;
                seccion = new Seccion(mostrarOriginal.PointToClient(Cursor.Position).X, mostrarOriginal.PointToClient(Cursor.Position).Y, auxUH);
            }
        }

        //Dibuja el rectangulo completo y saca el promedio.
        private void mostrarOriginal_MouseUp(object sender, MouseEventArgs e){
            if (draw){
                Graphics objGrafico = this.mostrarOriginal.CreateGraphics();
                seccion.setRectangle();
                objGrafico.DrawRectangle(seccion.getPen(), seccion.getRectangle());
                resultadoPromedio.Text = (seccion.createAverage()).ToString();
                draw = false;
                int milliseconds = 1200;
                Thread.Sleep(milliseconds);
                mostrarOriginal.Invalidate();
            }
        }

        //Se activa la bandera para sacar distancia.
        private void distanciaBarraDeHerramientas_Click(object sender, EventArgs e) { reglaBool = true; }

        //saca la distancia /saca el punto inicial/ /saca el punto final/
        private void mostrarOriginal_Click(object sender, EventArgs e){
            if (lect == null) return;
            if (reglaBool && bandera == 0){                
                regla = new Regla(mostrarOriginal.PointToClient(Cursor.Position).X, mostrarOriginal.PointToClient(Cursor.Position).Y);
                bandera = 1;
            }
            else if (reglaBool && bandera == 1){                
                reglaBool = false;
                bandera = 0;
                regla.setFinal(mostrarOriginal.PointToClient(Cursor.Position).X, mostrarOriginal.PointToClient(Cursor.Position).Y);
                Graphics objGrafico = this.mostrarOriginal.CreateGraphics();
                Pen myPen = new Pen(Color.Red, 1);
                objGrafico.DrawLine(myPen, regla.getPointInicio(), regla.getPoinFinal());
                double[] distancias = LecturaArchivosDicom.Pregunta_Python_Dimensiones(1, auxUH.obtenerRuta());
                resultadoDistancia.Text = (regla.getDistancia(distancias[0], distancias[1])).ToString("N3");
                int milliseconds = 1200;
                Thread.Sleep(milliseconds);
                mostrarOriginal.Invalidate();
            }
        }

        //umbral de agua
        private void aguaBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Agua", Color.FromArgb(98, 184, 230)); }

        //umbral aire
        private void aireBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Aire", Color.FromArgb(60,11,239));}

        //umbral fluido cerebral espinal
        private void fluidoCerebroEspinalBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("FluidoEspinal", Color.FromArgb(44, 213, 6)); }

        //umbral sustancia cerebral blanca
        private void sustanciaCerebralBlancaBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("CerebralBlanca", Color.FromArgb(76, 205, 72)); }

        //umbral sustancia cerebral gris
        private void sustanciaCerebralGrisToolStripMenuItem_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("CerebralGris", Color.FromArgb(235, 16, 73)); }

        //umbral hueso compacto
        private void huesoCompactoBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Hueso compacto", Color.FromArgb(203, 36, 79)); }

        //umbral hueso esponjonso
        private void huesoEsponjosoBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Hueso esponjoso", Color.FromArgb(117, 7, 35));}

        //umbral grasa
        private void grasaBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Grasa", Color.FromArgb(225, 183, 24)); }

        //umbral higado
        private void higadoBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Higado", Color.FromArgb(15, 23, 86)); }

        //umbral pancreas
        private void pancreasBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Pancreas", Color.FromArgb(220, 48, 13)); }

        //umbral pulmon
        private void pulmónUToolStripMenuItem_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Pulmones", Color.FromArgb(9, 134, 66)); }

        //umbral riñon
        private void riñonBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Riñon", Color.FromArgb(104, 0, 146)); }

        //umbral sangre
        private void sangreBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Sangre", Color.FromArgb(225, 4, 0)); }

        //umbral sangre coagulada
        private void sangreCoaguladaBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("SangreCoagulada", Color.FromArgb(176, 5, 2)); }

        private void contenedorBarraDeIconos_TopToolStripPanel_Click(object sender, EventArgs e) {}

        //Ventana para cerebro.
        private void cerebroBarraDeHerramientas_Click(object sender, EventArgs e){
            try
            {
                int lim_inf_ven = -10;
                int lim_sup_ven = 80;
                generalEscalaGris(lim_inf_ven, lim_sup_ven);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        //Avanzar hacia atrás sobre la tira.
        private void botonAtras_Click(object sender, EventArgs e){
            try {
                if (id_tac == 0)
                    id_tac = num_tacs - 1;
                else
                    id_tac--;
                auxUH = lect.obtenerArchivo(id_tac);
                MostrarImagenOriginal();
                if (imagenesCaja2.Count > 0)
                    MostrarImagenTratada();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }

        private void progressBar1_Click(object sender, EventArgs e) {}

        private void AbrirArchivosDICOM(BackgroundWorker bw)
        {
            lect = new LecturaArchivosDicom(ruta, bw);//se le da el path para sacar los archivos.
            if (bw.CancellationPending)
                return;

            num_tacs = lect.num_archivos();//se saca el número de archivos que hay en el estudio.
            imagenesCaja2.Clear();
            imagenesCaja1.Clear();//se limpia la lista del bitmap.
            MostrarImagenOriginal();
            MostrarImagenTratada();
            bw.ReportProgress(100);
            Thread.Sleep(1000);
            zoomCon = true;
        }

        private void ProcesoKMeans(BackgroundWorker bw)
        {
            kMeans k = new kMeans(lect, 6, 10, lect.num_archivos(), bw);
            if (bw.CancellationPending)
                return;

            int[,,] clases = k.getClases();
            imagenesCaja2.Clear();

            for (int i = 0; i < lect.num_archivos(); i++)
            {
                imagenesCaja2.Add(obtenerImgK(lect.obtenerArchivo(i).ObtenerImagen(), clases, i));
                bw.ReportProgress(90 + (10 * (i + 1)) / lect.num_archivos());
            }
            bw.ReportProgress(100);
            MostrarImagenTratada();
        }

        private void ProcesoFuzzyCMeans(BackgroundWorker bw)
        {
            FuzzyCMeans algoritmo = new FuzzyCMeans(lect, bw, 6, lect.num_archivos());
            if (bw.CancellationPending)
                return;

            int[,,] clases = algoritmo.getClases();
            imagenesCaja2.Clear();
            for (int i = 0; i < lect.num_archivos(); i++)
            {
                imagenesCaja2.Add(obtenerImgK(lect.obtenerArchivo(i).ObtenerImagen(), clases, i));
                bw.ReportProgress(90 + (10 * (i + 1)) / lect.num_archivos());
            }
            MostrarImagenTratada();
        }


        //Todos los metodos del background
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            int opcion = (int)e.Argument;
            BackgroundWorker bw = sender as BackgroundWorker;
            switch (opcion)
            {
                case 1:
                    AbrirArchivosDICOM(bw);
                    break;
                if (lect == null) return;
                case 2:
                    ProcesoKMeans(bw);
                    break;
                case 3:
                    ProcesoFuzzyCMeans(bw);
                    break;
            }
        }

        //reporte del progreso abrir archivos dicom, barra de progreso
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) { progressBar1.Value = e.ProgressPercentage;}
           
        //abrir archivo icono
        private void abrirBarraIconos_Click(object sender, EventArgs e){ abrirBarraHerramientas_Click(sender,e);}

        //Rotar 90 grados izquirda icono
        private void toolStripButton1_Click(object sender, EventArgs e){ rotar90IquierdaBarraDeHerramientas_Click(sender, e); }

        //Rotar 90 grados derecha icono
        private void toolStripButton2_Click(object sender, EventArgs e) { rotar90DerechaBarraDeHerramientas_Click(sender, e);}

        //Umbral de huevo icono
        private void toolStripButton5_Click(object sender, EventArgs e){ huesoCompactoBarraDeHerramientas_Click(sender,e); }

        //umbral de agua icono
        private void toolStripButton6_Click(object sender, EventArgs e){ aguaBarraDeHerramientas_Click(sender, e); }

        //umbral de sangre icono
        private void toolStripButton7_Click(object sender, EventArgs e){ sangreBarraDeHerramientas_Click(sender,e); }

        //Activar regla icono
        private void toolStripButton10_Click(object sender, EventArgs e){ reglaBool = true; }

        //ventana default
        private void defaultBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja1.Clear(); MostrarImagenOriginal();}

        //ventana default iconos
        private void predeterminadoIcono_Click(object sender, EventArgs e){ imagenesCaja1.Clear(); MostrarImagenOriginal();}

        //ventana cerebro iconos
        private void cerebroIcono_Click(object sender, EventArgs e){ cerebroBarraDeHerramientas_Click(sender, e); }

        //venatana hueso iconos
        private void huesoContrasteIcono_Click(object sender, EventArgs e){ huesoBarraDeHerramientas_Click(sender,e); }

        //Sacar UH en la imagen tratada
        private void mostrarTratada_MouseMove(object sender, MouseEventArgs e){
            int x = mostrarTratada.PointToClient(Cursor.Position).X;
            int y = mostrarTratada.PointToClient(Cursor.Position).Y;
            if (auxUH != null) resultadoUHMouse.Text = (auxUH.ObtenerUH(x, y)).ToString();
            //PARTE DEL ZOOM
            if (zoomCon){
                if (mostrarTratada.Image != null){
                    Bitmap zoomTratedImage = imagenesCaja2[id_tac];
                    Rectangle zoomRect2 = new Rectangle(x - (ventanaZoom / 2), y - (ventanaZoom / 2), ventanaZoom, ventanaZoom);
                    if (zoomRect2.Left >= 0 && zoomRect2.Top >= 0 && zoomRect2.Right <= 512 && zoomRect2.Bottom <= 512){
                        var newzoomImage = zoomTratedImage.Clone(zoomRect2, zoomTratedImage.PixelFormat);
                        zoom.Image = newzoomImage;
                        zoom.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
        }

        //Cluster de k-means 
        private void kmeans_Click(object sender, EventArgs e){
            panelProgressBar.Visible = true;
            progressBar1.Value = 1;
            backgroundWorker1.RunWorkerAsync(2);
        }

        //Cluster C-fuzzy
        private void fuzzy_Click(object sender, EventArgs e){
            panelProgressBar.Visible = true;
            progressBar1.Value = 1;
            backgroundWorker1.RunWorkerAsync(3);
        }

        //Cluster de k-means icono
        private void kmeansIcono_Click(object sender, EventArgs e){ kmeans_Click(sender,e);}

        //Cluster C-fuzzy icono
        private void fuzzyIcono_Click(object sender, EventArgs e){ fuzzy_Click(sender,e); }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e){
            if (e.Cancelled)
                MessageBox.Show("The task has been cancelled");
            else if (e.Error != null)
                MessageBox.Show("Error. Details: " + (e.Error as Exception).ToString());
            else{

                progressBar1.Value = 100;
                Thread.Sleep(500);
                panelProgressBar.Visible = false;
            }
        }

        //zoom para restarle
        private void toolStripButton3_Click(object sender, EventArgs e){
            if (ventanaZoom == 100) return;
            ventanaZoom += 20;
        }

        //zoom para sumarle
        private void toolStripButton4_Click(object sender, EventArgs e){
            if (ventanaZoom == 20) return;
            ventanaZoom -= 20;
        }

        //exportar original icono
        private void exportarOriginalIcono_Click(object sender, EventArgs e){
            if (mostrarOriginal.Image == null) return;
            SaveFileDialog f = new SaveFileDialog();
            f.Filter = "JPG(.JPG)|.jpg|Png Image (.png)|*.png";
            if (f.ShowDialog() == DialogResult.OK)            
                mostrarOriginal.Image.Save(f.FileName);            
        }

        //exportar tratada icono
        private void exportarTratadaIcono_Click(object sender, EventArgs e){
            if (mostrarTratada.Image == null) return;
            SaveFileDialog f = new SaveFileDialog();
            f.Filter = "JPG(.JPG)|.jpg|Png Image (.png)|*.png";
            if (f.ShowDialog() == DialogResult.OK)
                mostrarTratada.Image.Save(f.FileName);
        }

        //exportar original 
        private void exportarOriginal_Click(object sender, EventArgs e){ exportarOriginalIcono_Click(sender,e); }

        //exportar tratada
        private void exportarTratada_Click(object sender, EventArgs e){ exportarTratadaIcono_Click(sender,e); }




        ///---------------------------------------------------------------------------------------------------------------------------------------------------


        //Funciones 
        //***************************************************************************************************************************************************** 
        //mostrar imagen sin tratamiento.


        private void dibujarUmbral(string lectura, Color color){
            try
            {
                Umbralizacion operaciones = new Umbralizacion();
                for (int i = 0; i < lect.num_archivos(); i++)
                {
                    var archivo = lect.obtenerArchivo(i);
                    var matrizResultado = operaciones.UmbralizacionPara(lectura, archivo.matriz);
                    var imagenResultado = obtenerImagenUmbral(matrizResultado, archivo.ObtenerImagen(), color);
                    imagenesCaja2.Add(imagenResultado);
                }
                MostrarImagenTratada();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se ha cargado ningún archivo", "Error");
            }
        }
        private void personalizadaBarraDeHerramientas_Click(object sender, EventArgs e) {
            panelPersonalizada.Visible = true;
        }

        private void pesonalizadaVBarraDeHerramientas_Click(object sender, EventArgs e) {
            panelPersonalizada.Visible = true;
        }

        private void botonAplicarPersonalizada_Click(object sender, EventArgs e) {
            panelPersonalizada.Visible = false;
        }
        private void exportarBarraIconos_Click(object sender, EventArgs e){}

        private void butonCancelarProceso_Click(object sender, EventArgs e) {
            backgroundWorker1.CancelAsync();
            panelProgressBar.Visible = false;
            
        }

        private void mostrarOriginal_MouseHover(object sender, EventArgs e)
        {

        }

        private void MostrarImagenOriginal(){
            if (lect == null) return;
            if (imagenesCaja1.Count() <= 0) {
                for (int i = 0; i < lect.num_archivos(); i++) {
                    var archivo = lect.obtenerArchivo(i);
                    var imagenResultado = archivo.ObtenerImagen();
                    imagenesCaja1.Add(imagenResultado);
                }
            }
            mostrarOriginal.Image = imagenesCaja1[id_tac];
        }

        //mostrar imagen con tratamiento.
        private void MostrarImagenTratada(){
            if (lect == null) return;
            if (imagenesCaja2.Count() > 0) mostrarTratada.Image = imagenesCaja2[id_tac];            
            else mostrarTratada.Image = null;            
        }
       
        //genera escala de gris.
        private void generalEscalaGris(int lim_inf, int lim_sup){
            imagenesCaja1.Clear();
            for (int i = 0; i < lect.num_archivos(); i++){
                var archivo = lect.obtenerArchivo(i);
                var imagen = obtenerImagenConVentana(archivo.matriz, lim_inf, lim_sup);
                imagenesCaja1.Add(imagen);
            }
            MostrarImagenOriginal();
        }

        //genera la imagen con el umbral
        private Bitmap obtenerImagenUmbral(bool[,] umbral, Bitmap matrizOriginal, Color color){
            Bitmap resultado = new Bitmap(matrizOriginal);
            int N = umbral.GetLength(0);
            int M = umbral.GetLength(1);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)                
                    if (umbral[i, j])                    
                        resultado.SetPixel(i, j, color);                                                
            return resultado;
        }

        //Crea una imagen con la nueva ventana establecida.
        private Bitmap obtenerImagenConVentana(int[,] matriz, int limiteInferior, int limiteSuperior){
            int N = matriz.GetLength(0);
            int M = matriz.GetLength(1);
            Bitmap imagen = new Bitmap(N, M);
            int tam = limiteSuperior - limiteInferior + 1;
            double porcion = 255.0 / tam;

            for (int i = 0; i < N; i++){
                for (int j = 0; j < M; j++){
                    int valorGris = (int)(porcion * (double)(matriz[i, j] - limiteInferior + 1));
                    if (matriz[i, j] < limiteInferior)
                        valorGris = 0;
                    if (matriz[i, j] > limiteSuperior)
                        valorGris = 255;
                    Color color = Color.FromArgb(valorGris, valorGris, valorGris);
                    imagen.SetPixel(i, j, color);
                }
            }
            return imagen;
        }

        //se genera la imagen con base a las clases que se tiene
        private Bitmap obtenerImgK(Bitmap matrizOriginal, int[,,] lista, int p){
            Bitmap resultado = new Bitmap(matrizOriginal);
            List<Color> colores = new List<Color>() { Color.Black, Color.Red, Color.Blue, Color.Orange, Color.Yellow, Color.Pink, Color.Purple };
            for (int i = 0; i < 512; i++)            
                for (int j = 0; j < 512; j++)                
                    resultado.SetPixel(i, j, colores[lista[i, j, p]]);                            
            return resultado;
        }


        //***************************************************************************************************************************************************** 


    }
}
