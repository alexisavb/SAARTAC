using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAARTAC1._1 {
    class SplitMerge {
        private int [,] datos;
        private long eps;
        private int [,] resultado;
        private int N, M;
        public int LMI, LMS;

        public SplitMerge(int [,] datos, int N, int M, long eps = 1000) {
            this.datos = datos;
            this.eps = eps;
            this.N = N;
            this.M = M;
            resultado = new int [N, M];
            LMI = 10000;
            LMS = -10000;
            EvaluacionRegion(0, 0, N - 1, M - 1);
        }

        private void EvaluacionRegion(int y1, int x1, int y2, int x2) {
            if (y1 > y2 || x1 > x2)
                return;
            double promedio = EvaluacionPromedio(y1, x1, y2, x2);
            if (Desviacion(y1, x1, y2, x2, promedio)) {
                SeparaRegiones(y1, x1, y2, x2);
            } else {
                EstablecerValores(y1, x1, y2, x2, promedio);
            }

        }

        private void EstablecerValores(int y1, int x1, int y2, int x2, double M) {
            for (int i = y1; i <= y2; i++) {
                for (int j = x1; j <= x2; j++) {
                    resultado [i, j] = (int)M;
                    LMI = Math.Min((int)M, LMI);
                    LMS = Math.Max((int)M, LMS);

                }
            }
        }

        private bool Desviacion(int y1, int x1, int y2, int x2, double M) {
            long ans = 0;
            for (int i = y1; i <= y2; i++) {
                for (int j = x1; j <= x2; j++) {
                    double valor = M - datos [i, j];
                    ans +=(long) (valor * valor);
                    if (ans > eps)
                        return true;
                }
            }
            return false;
        }

        private double EvaluacionPromedio(int y1, int x1, int y2, int x2) {
            double ans = 0.0;
            for (int i = y1; i <= y2; i++) {
                for (int j = x1; j <= x2; j++) {
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

        public Bitmap ObtenerImagen() {
            Bitmap imagen = new Bitmap(N, M);
            int tam = LMS - LMI + 1;
            double porcion = 255.0 / tam;

            for (int i = 0; i < N; i++) {
                for (int j = 0; j < M; j++) {
                    int valorGris = (int)(porcion * (double)(resultado [i, j] - LMI + 1));
                    if (resultado [i, j] < LMI)
                        valorGris = 0;
                    if (resultado [i, j] > LMS)
                        valorGris = 255;
                    Color color = Color.FromArgb(valorGris, valorGris, valorGris);
                    imagen.SetPixel(i, j, color);
                }
            }
            return imagen;
        }

    }
}
