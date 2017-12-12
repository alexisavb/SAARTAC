using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAARTAC1._1 {
    class Contraste {
        private static Mutex mutex;
        private static int numeroHilos;
        public static Bitmap[] imagenes;
        public Contraste(int N) {
            mutex = new Mutex();
            numeroHilos = Properties.Settings.Default.NumeroProcesos;
            imagenes = new Bitmap [N];
        }

        public static void EncuentraHiloLibre() {
            while (true) {
                mutex.WaitOne();
                if (numeroHilos > 0) {
                    numeroHilos--;
                    mutex.ReleaseMutex();
                    return;
                }
                mutex.ReleaseMutex();
            }
        }
        //Crea una imagen con la nueva ventana establecida.
        public void obtenerImagenConVentana(ParametroContraste o) {
            EncuentraHiloLibre();
            int [,] matriz = o.matriz;
            int limiteInferior = o.inicio;
            int limiteSuperior = o.fin;
            int N = matriz.GetLength(0);
            int M = matriz.GetLength(1);
            Bitmap imagen = new Bitmap(N, M);
            int tam = limiteSuperior - limiteInferior;
            double porcion = 255.0 / tam;

            for (int i = 0; i < N; i++) {
                for (int j = 0; j < M; j++) {
                    int valorGris = (int)(porcion * (double)(matriz [i, j] - limiteInferior));
                    if (matriz [i, j] < limiteInferior)
                        valorGris = 0;
                    if (matriz [i, j] > limiteSuperior)
                        valorGris = 255;
                    Color color = Color.FromArgb(valorGris, valorGris, valorGris);
                    imagen.SetPixel(i, j, color);
                }
            }
            imagenes[o.indice] = imagen;
            mutex.WaitOne();
            numeroHilos++;
            mutex.ReleaseMutex();
        }
    }
}
