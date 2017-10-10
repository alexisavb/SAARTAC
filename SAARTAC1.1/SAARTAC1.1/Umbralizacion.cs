using System;
using System.Collections.Generic;

namespace SAARTAC1._1
{
    internal class Umbralizacion{
        private Dictionary<string, int[]> umbralesDelCuerpo = new Dictionary<string, int[]>();

        public Umbralizacion(){
            umbralesDelCuerpo["Agua"] = new int[] { 0, 10 };
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
