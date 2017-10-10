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
        
        public mainVentana() {
            InitializeComponent();
            barraHerramientas.Renderer = new MyRenderer();
            barraIconos3D.Renderer = new MyRenderer();
            barraDeIconos.Renderer = new MyRenderer();
            barraIconosUmbralizacion.Renderer = new MyRenderer();
            barraIconoRegla.Renderer = new MyRenderer();
            barraIconoClasificacion.Renderer = new MyRenderer();
        }

//Eventos de los botones
//---------------------------------------------------------------------------------------------------------------------------------------------------
        //Abrir archivos.
        private void abrirBarraHerramientas_Click(object sender, EventArgs e){
            try{
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK){ //verifica si se abrio.                    
                    id_tac = 0;
                    string imagen = folderBrowserDialog1.SelectedPath; //se saca el path del archivo.
                    lect = new LecturaArchivosDicom(imagen);//se le da el path para sacar los archivos.
                    num_tacs = lect.num_archivos();//se saca el número de archivos que hay en el estudio.
                    imagenesCaja2.Clear();
                    imagenesCaja1.Clear();//se limpia la lista del bitmap.
                    MostrarImagenOriginal();
                    MostrarImagenTratada();                    
                }
                else Console.WriteLine("Hay un problema al abrir el archivo");
            }
            catch (Exception ex) { MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido"); }
        }

        //Avanzar hacia delante sobre la tira.
        private void botonSiguient_Click(object sender, EventArgs e){
            if (id_tac >= num_tacs - 1)
                id_tac = 0;
            else
                id_tac++;
            auxUH = lect.obtenerArchivo(id_tac);
            MostrarImagenOriginal();
            if (imagenesCaja2.Count > 0)
                MostrarImagenTratada();
        }

        //ventana para hueso.
        private void huesoBarraDeHerramientas_Click(object sender, EventArgs e){
            int lim_inf_ven = -450;
            int lim_sup_ven = 1050;
            generalEscalaGris(lim_inf_ven, lim_sup_ven);
        }

        //venta para partes blandas.
        private void partesBlandasBarraDeHerramientas_Click(object sender, EventArgs e){
            int lim_inf_ven = -125;
            int lim_sup_ven = 225;
            generalEscalaGris(lim_inf_ven, lim_sup_ven);
        }

        //ventana para pulmón.
        private void pulmónBarraDeHerramientas_Click(object sender, EventArgs e){
            int lim_inf_ven = -1200;
            int lim_sup_ven = 800;
            generalEscalaGris(lim_inf_ven, lim_sup_ven);
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
            if (draw & e.Button == MouseButtons.Left){
                seccion.setFinal(x, y);
                Graphics objGrafico = this.mostrarOriginal.CreateGraphics();
                seccion.setRectangle();
                objGrafico.DrawRectangle(seccion.getPen(), seccion.getRectangle());
                mostrarOriginal.Invalidate();
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

        private void huesoEsponjosoBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Hueso esponjoso", Color.FromArgb(117, 7, 35));}

        private void grasaBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Grasa", Color.FromArgb(225, 183, 24)); }

        private void higadoBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Higado", Color.FromArgb(15, 23, 86)); }

        private void pancreasBarraDeHerramientas_Click(object sender, EventArgs e) { imagenesCaja2.Clear(); dibujarUmbral("Pancreas", Color.FromArgb(220, 48, 13)); }

        private void pulmónUToolStripMenuItem_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Pulmones", Color.FromArgb(9, 134, 66)); }

        private void riñonBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Riñon", Color.FromArgb(104, 0, 146)); }

        private void sangreBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("Sangre", Color.FromArgb(225, 4, 0)); }

        private void sangreCoaguladaBarraDeHerramientas_Click(object sender, EventArgs e){ imagenesCaja2.Clear(); dibujarUmbral("SangreCoagulada", Color.FromArgb(176, 5, 2)); }
        private void contenedorBarraDeIconos_TopToolStripPanel_Click(object sender, EventArgs e) {

        }


        //Ventana para cerebro.
        private void cerebroBarraDeHerramientas_Click(object sender, EventArgs e){
            int lim_inf_ven = -10;
            int lim_sup_ven = 80;
            generalEscalaGris(lim_inf_ven, lim_sup_ven);
        }

        //Avanzar hacia atrás sobre la tira.
        private void botonAtras_Click(object sender, EventArgs e){
            if (id_tac == 0)
                id_tac = num_tacs - 1;
            else
                id_tac--;
            auxUH = lect.obtenerArchivo(id_tac);
            MostrarImagenOriginal();
            if (imagenesCaja2.Count > 0)
                MostrarImagenTratada();
        }

        

        ///---------------------------------------------------------------------------------------------------------------------------------------------------


        //Funciones 
        //***************************************************************************************************************************************************** 
        //mostrar imagen sin tratamiento.


        private void dibujarUmbral(string lectura, Color color){
            Umbralizacion operaciones = new Umbralizacion();
            for (int i = 0; i < lect.num_archivos(); i++){
                var archivo = lect.obtenerArchivo(i);
                var matrizResultado = operaciones.UmbralizacionPara(lectura, archivo.matriz);
                var imagenResultado = obtenerImagenUmbral(matrizResultado, archivo.ObtenerImagen(), color);
                imagenesCaja2.Add(imagenResultado);
            }
            MostrarImagenTratada();
        }

        private void MostrarImagenOriginal(){
            if (imagenesCaja1.Count() > 0){ mostrarOriginal.Image = imagenesCaja1[id_tac]; return;}//Esto es para cambiar de imagen y lo dibuje.
            MatrizDicom aux = lect.obtenerArchivo(id_tac); //se obtiene la matriz.
            auxUH = lect.obtenerArchivo(id_tac);
            mostrarOriginal.Image = aux.ObtenerImagen(); //se dibuja la matriz.
        }

        //mostrar imagen con tratamiento.
        private void MostrarImagenTratada(){
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

        //***************************************************************************************************************************************************** 


    }
}
