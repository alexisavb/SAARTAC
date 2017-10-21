using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace SAARTAC1._1 {

    internal class Umbralizacion{
        private Dictionary<string, int[]> umbralesDelCuerpo = new Dictionary<string, int[]>();
        public static Bitmap [] imagenes;
        private static Mutex mutex;
        private static int numeroHilos;

        public Umbralizacion(int N){
            imagenes = new Bitmap [N];

            mutex = new Mutex();
            numeroHilos = Properties.Settings.Default.NumeroProcesos;
            umbralesDelCuerpo ["Agua"] = new int[] { 0, 10 };
            umbralesDelCuerpo["Aire"] = new int[] { -1000, 150 };
            umbralesDelCuerpo["FluidoEspinal"] = new int[] { 11, 6 };
            umbralesDelCuerpo["CerebralBlanca"] = new int[] { 30, 6 };
            umbralesDelCuerpo["CerebralGris"] = new int[] { 40, 6 };
            umbralesDelCuerpo["Grasa"] = new int[] { -75, 25 };
            umbralesDelCuerpo["Higado"] = new int[] { 65, 5 };
            umbralesDelCuerpo["Pancreas"] = new int[] { 40, 10 };            
            umbralesDelCuerpo["Riñon"] = new int[] { 30, 10 };
            umbralesDelCuerpo["Hueso compacto"] = new int[] { 1000, 750 };
            umbralesDelCuerpo["Hueso esponjoso"] = new int[] { 130, 100 };
            umbralesDelCuerpo["Pulmones"] = new int[] { -700, 200 };
            umbralesDelCuerpo["Sangre"] = new int[] { 55, 5 };
            umbralesDelCuerpo["SangreCoagulada"] = new int[] { 55, 5 };
        }

        public Umbralizacion(int valorUH, int tolerancia, int N){
            imagenes = new Bitmap [N];

            mutex = new Mutex();
            numeroHilos = Properties.Settings.Default.NumeroProcesos;
            umbralesDelCuerpo ["Personalizada"] = new int[] {valorUH, tolerancia };
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

        public void UmbraHilos(ParametroUmbralizacion o) {
            EncuentraHiloLibre();
            var matrizResultado = UmbralizacionPara(o.tipo, o.archivo.matriz);
            imagenes [o.indice] = obtenerImagenUmbral(matrizResultado, o.archivo.ObtenerImagen(), o.color);
            mutex.WaitOne();
            numeroHilos++;
            mutex.ReleaseMutex();

        }

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


        public bool[,] UmbralizacionPara(string tipo, int[,] matriz){
            int x = umbralesDelCuerpo[tipo][0];
            int y = umbralesDelCuerpo[tipo][1];
            return UmbralEnRango(matriz, x - y, x + y);
        }

        public bool[,] UmbralEnRango(int[,] matriz, int limiteInferior, int limiteSuperior){
            int N = matriz.GetLength(0);
            int M = matriz.GetLength(1);
            bool[,] salida = new bool[N, M];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    salida[i, j] = matriz[i, j] >= limiteInferior && matriz[i, j] <= limiteSuperior;                            
            return salida;
        }
    }
}
