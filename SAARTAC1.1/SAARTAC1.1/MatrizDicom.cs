using System;
using System.IO;
using System.Drawing;

namespace SAARTAC1._1{
    internal class MatrizDicom{
        private int N;
        private int M;
        public int[,] matriz;
        public int minValor;
        public int maxValor;
        public string ruta;

        public MatrizDicom(){
            N = 512;
            M = 512;
            matriz = new int[N, N];
            minValor = 10000;
            maxValor = -10000;
        }

        public MatrizDicom(string ruta, int NN, int MM){
            N = NN;
            M = MM;
            matriz = new int[N, M];
            minValor = 10000;
            maxValor = -10000;
            this.ruta = ruta;
        }

        public int ObtenerUH(int x, int y){
            if (x >= N || y >= M || x < 0 || y < 0)
                return -100000;
            return matriz[x, y];
        }

        public string obtenerRuta() { return ruta; }
        public int[,] obtenerMatriz() { return matriz; }
        public int obtenerN() { return N; }
        public int obtenerM() { return M; }

        public void CopiarMatriz(ref int[,] A){
            for (int i = 0; i < N; i++){
                for (int j = 0; j < N; j++){
                    matriz[i, j] = A[i, j];
                    minValor = Math.Min(minValor, matriz[i, j]);
                    maxValor = Math.Max(maxValor, matriz[i, j]);
                }
            }
        }

        public Bitmap ObtenerImagen(){
            Bitmap imagen = new Bitmap(N, M);
            int tam = maxValor - minValor + 1;
            double porcion = 256.0 / tam;

            for (int i = 0; i < N; i++){
                for (int j = 0; j < M; j++){
                    int valorGris = (int)(porcion * (matriz[i, j] - minValor));
                    Color color = Color.FromArgb(valorGris, valorGris, valorGris);
                    imagen.SetPixel(i, j, color);
                }
            }
            return imagen;
        }
        public MatrizDicom GirarDerecha(MatrizDicom matrizD){
            MatrizDicom matrizGirada = new MatrizDicom();
            for (int i = 0; i < N; i++){
                int h = N - 1;
                for (int j = 0; j < N; j++){
                    matrizGirada.matriz[h, i] = matrizD.matriz[i, j];
                    h--;
                }
            }
            return matrizGirada;
        }
        public MatrizDicom GirarIzquierda(MatrizDicom matrizI){
            MatrizDicom matrizGirada = new MatrizDicom();
            for (int i = 0; i < N; i++){
                int h = N - 1;
                for (int j = 0; j < N; j++){
                    matrizGirada.matriz[i, j] = matrizI.matriz[N - j - 1, i];
                    h--;
                }
            }
            return matrizGirada;
        }
    }

}
