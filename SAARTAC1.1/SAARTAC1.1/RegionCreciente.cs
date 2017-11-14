using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAARTAC1._1 {
    class RegionCreciente {
        private int origenX, origenY;
        private int [,] mapa;
        private int [] precision = {2, 5, 10, 20, 30, 50};
        private int N, M;
        private static int[,] movimientos = { { -1, 0 }, 
                                              { 1, 0 }, 
                                              { 0, -1 }, 
                                              { 0, 1}, 
                                              { -1, 1 }, 
                                              { -1, -1 }, 
                                              { 1, 1 }, 
                                              { 1, -1 }};

        public RegionCreciente(int [,] escenario, int Y = 255, int X = 255) {
            mapa = escenario;
            origenX = X;
            origenY = Y;
            N = mapa.GetLength(0);
            M = mapa.GetLength(1);
        }

        private bool DentroMapa(int y, int x) {
            if (y < 0 || x < 0 || y >= N || x >= M)
                return false;
            return true;
        }

        public int[,] ObtenerRegion(int calidad = 0) {
            int [,] salida = new int [N, M];
            Queue busqueda = new Queue();
            busqueda.Enqueue(new Tuple<int, int>(origenY, origenX));
            salida [origenY, origenX] = 1;
            while(busqueda.Count > 0) {
                Tuple<int, int> actual = (Tuple <int, int>) busqueda.Dequeue();
                for(int i = 0; i < 8; i++){
                    int y = actual.Item1 + movimientos [i, 0];
                    int x = actual.Item2 + movimientos [i, 1];
                    if(DentroMapa(y, x)) {
                        if (salida [y, x] == 1 || Math.Abs(mapa[origenY, origenX] - mapa[y, x]) > precision[calidad])
                            continue;
                        salida [y, x] = 1;
                        busqueda.Enqueue(new Tuple<int, int>(y, x));
                    }
                }
            }
            return salida;
        }
    }
}
