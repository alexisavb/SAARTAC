using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAARTAC1._1 {
    class RegionCreciente {
        private int origenX, origenY, origenZ;
        private List<int [,]> mapa;
        private int [] precision = {2, 5, 10, 20, 30, 50};
        private int N, M, K;
        private static int[,] movimientos = { { -1, 0, 0 }, 
                                              { 1, 0, 0 }, 
                                              { 0, -1, 0 }, 
                                              { 0, 1, 0}, 
                                              { -1, 1, 0 }, 
                                              { -1, -1, 0 },
                                              { 1, 1, 0 },
                                              { 1, -1, 0 },
                                              { 0, 0, 1 },
                                              { 0, 0, -1 }};

        public RegionCreciente(List<int [,]> escenario, int Z, int Y = 255, int X = 255) {
            mapa = escenario;
            origenX = X;
            origenY = Y;
            origenZ = Z;
            N = mapa[0].GetLength(0);
            M = mapa[0].GetLength(1);
            K = escenario.Count();
        }

        private bool DentroMapa(int y, int x, int z) {
            if (y < 0 || x < 0 || y >= N || x >= M || z < 0 || z >= K)
                return false;
            return true;
        }

        public List<int[,]> ObtenerRegion(int calidad = 0) {
            List<int [,]> salida = new List<int [,]>();
            for(int i = 0; i < K; i++) {
                salida.Add(new int [N, M]);
            }
            Queue busqueda = new Queue();
            busqueda.Enqueue(new Tuple<int, int, int>(origenY, origenX, origenZ));
            salida [origenZ][origenY, origenX] = 1;
            while(busqueda.Count > 0) {
                Tuple<int, int, int> actual = (Tuple <int, int, int>) busqueda.Dequeue();
                for(int i = 0; i < 10; i++){
                    int y = actual.Item1 + movimientos [i, 0];
                    int x = actual.Item2 + movimientos [i, 1];
                    int z = actual.Item3 + movimientos [i, 2];
                    if(DentroMapa(y, x, z)) {
                        if (salida[z][y, x] == 1 || Math.Abs(mapa[origenZ][origenY, origenX] - mapa[actual.Item3][actual.Item1, actual.Item2]) > precision[calidad])
                            continue;
                        salida [z][y, x] = 1;
                        busqueda.Enqueue(new Tuple<int, int, int>(y, x, z));
                    }
                }
            }
            return salida;
        }
    }
}
