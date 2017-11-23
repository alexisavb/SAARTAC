using System;
using System.Collections;
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
        private int [,] visitados;
        private long sumaTotal, cuadrosTotales;
        private int EPS_merge = 30;
        private static int [,] movimientos = { { -1, 0 },
                                              { 1, 0 },
                                              { 0, -1 },
                                              { 0, 1}};

        public SplitMerge(Bitmap imagen, int N, int M, long eps = 100) {
            this.datos = new int [N, M];
            for(int i = 0; i < N; i++) {
                for(int j = 0; j < M; j++) {
                    Color aux = imagen.GetPixel(j, i);
                    datos [j, i] = aux.R;
                }
            }
            this.eps = eps;
            this.N = N;
            this.M = M;
            resultado = new int [N, M];

            visitados = new int [N, M];
            LMI = 10000;
            LMS = -10000;
            EvaluacionRegion(0, 0, N - 1, M - 1);
            MergeRegion();
        }

        public SplitMerge(int [,] datos, int N, int M, long eps = 400) {
            this.datos = datos;
            this.eps = eps;
            this.N = N;
            this.M = M;
            resultado = new int [N, M];
            visitados = new int [N, M];
            LMI = 10000;
            LMS = -10000;
            EvaluacionRegion(0, 0, N - 1, M - 1);
            MergeRegion();
        }

        private bool DentroMapa(int y, int x) {
            if (y < 0 || x < 0 || y >= N || x >= M)
                return false;
            return true;
        }

        private void JuntaRegiones(int yy, int xx, int valor) {
            Queue busqueda = new Queue();
            busqueda.Enqueue(new Tuple<int, int>(yy, xx));
            visitados [yy, xx] = 1;
            while (busqueda.Count > 0) {
                Tuple<int, int> actual = (Tuple<int, int>)busqueda.Dequeue();
                cuadrosTotales++;
                sumaTotal += resultado [actual.Item1, actual.Item2];
                for (int i = 0; i < 4; i++) {
                    int y = actual.Item1 + movimientos [i, 0];
                    int x = actual.Item2 + movimientos [i, 1];
                    if (DentroMapa(y, x)) {
                        if (visitados [y, x] == 1 || Math.Abs(resultado [y, x] - resultado [actual.Item1, actual.Item2]) > EPS_merge)
                            continue;
                        visitados [y, x] = 1;
                        busqueda.Enqueue(new Tuple<int, int>(y, x));
                    }
                }
            }
        }

        private void ActualizaValor(int yy, int xx, int valor, int set) {
            Queue busqueda = new Queue();
            busqueda.Enqueue(new Tuple<int, int>(yy, xx));
            visitados [yy, xx] = 2;
            while (busqueda.Count > 0) {
                Tuple<int, int> actual = (Tuple<int, int>)busqueda.Dequeue();
                int valor_original = resultado [actual.Item1, actual.Item2];
                resultado [actual.Item1, actual.Item2] = set;
                for (int i = 0; i < 4; i++) {
                    int y = actual.Item1 + movimientos [i, 0];
                    int x = actual.Item2 + movimientos [i, 1];
                    if (DentroMapa(y, x)) {
                        if (visitados [y, x] == 2 || Math.Abs(resultado [y, x] - valor_original) > EPS_merge)
                            continue;
                        visitados [y, x] = 2;
                        busqueda.Enqueue(new Tuple<int, int>(y, x));
                    }
                }
            }
        }



        private void MergeRegion() {
            for(int i = 0; i < N; i++) 
                for (int j = 0; j < M; j++)
                    visitados[i, j] = 0;
            
            for(int i = 0; i < N; i++) {
                for(int j = 0; j < M; j++) {
                    if (visitados [i, j] == 0) {
                        cuadrosTotales = 0;
                        sumaTotal = 0;
                        JuntaRegiones(i, j, resultado[i, j]);
                        long valor = sumaTotal / cuadrosTotales;
                        ActualizaValor(i, j, resultado [i, j], (int)valor);
                        Console.WriteLine(cuadrosTotales);
                    }
                }
            }
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

        private void EstablecerValores(int y1, int x1, int y2, int x2, double MM) {
            for (int i = y1; i <= y2; i++) {
                for (int j = x1; j <= x2; j++) {
                    resultado [i, j] = (int)MM;
                    LMI = Math.Min((int)MM, LMI);
                    LMS = Math.Max((int)MM, LMS);

                }
            }
        }

        private bool Desviacion(int y1, int x1, int y2, int x2, double MM) {
            long ans = 0;
            long totalDatos = -1;
            for (int i = y1; i <= y2; i++) {
                for (int j = x1; j <= x2; j++) {
                    double valor = MM - datos [i, j];
                    ans +=(long) (valor * valor);
                    totalDatos++;
                }
            }
            return ans > eps * totalDatos;
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
