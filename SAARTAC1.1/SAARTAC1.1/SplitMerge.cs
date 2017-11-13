using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAARTAC1._1 {
    class SplitMerge {
        private int [,] datos;
        private int eps;
        private int [,] resultado;
        private int N, M;

        public SplitMerge(int[,] datos, int N, int M, int eps = 25) {
            this.datos = datos;
            this.eps = eps;
            this.N = N;
            this.M = M;
            EvaluacionRegion(0, 0, N - 1, M - 1);
        }

        private void EvaluacionRegion(int y1, int x1, int y2, int x2) {
            if (y1 > y2 || x1 > x2)
                return;
            double promedio = EvaluacionPromedio(y1, x1, y2, x2);
            if(Desviacion(y1, x1, y2, x2, promedio)) {
                SeparaRegiones(y1, x1, y2, x2);
            } else {
                EstablecerValores(y1, x1, y2, x2, promedio);
            }

        }

        private void EstablecerValores(int y1, int x1, int y2, int x2, double M) {
            for(int i = y1; i <= y2; i++) {
                for(int j = x1; j <= x2; j++) {
                    resultado [i, j] = (int) M;
                }
            }
        }

        private bool Desviacion(int y1, int x1, int y2, int x2, double M) {
            double ans = 0.0;
            for(int i = y1; i <= y2; i++) {
                for(int j = x1; j <= x2; j++) {
                    double valor = M - datos [i, j];
                    ans += valor * valor;
                    if (ans > eps)
                        return true;
                }
            }
            return false;
        }

        private double EvaluacionPromedio(int y1, int x1, int y2, int x2) {
            double ans = 0.0;
            for(int i = y1; i <= y2; i++) {
                for(int j = x1; j <= x2; j++) {
                    ans += datos [i, j];
                }
            }
            double total = (y2 - y1 + 1) * (x2 - x1 + 1);
            return ans / total;
        }

        private void SeparaRegiones(int y1, int x1, int y2, int x2) {
            int PMY = (y1 + y2) / 2;
            int PMX = (x1 + x2) / 2;
            EvaluacionRegion(y1, x1, PMY, PMX);
            EvaluacionRegion(y1, PMX + 1, PMY, x2);
            EvaluacionRegion(PMY + 1, x1, y2, PMX);
            EvaluacionRegion(PMY + 1, PMX + 1, y2, x2);
        }
        
    }
}
