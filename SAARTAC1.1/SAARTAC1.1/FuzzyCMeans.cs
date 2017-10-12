using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAARTAC1._1{
    class FuzzyCMeans{
        private LecturaArchivosDicom matrices;
        private MatrizDicom matriz_actual;
        private int numerosK, ite, numArchivos;
        private int min = -1000, max = 1600;
        private List<Double> centros;
        private List<Double> conjunto = new List<Double>();
        private int[,,] clases;
        private double[,,,] pertenencia;
        private double[,,,] distancias;
        private Random rnd;
        private double m = 2.0;

        public FuzzyCMeans(LecturaArchivosDicom lect, int k, int numeros_archivos, int iteraciones = 10){
            matrices = lect;
            numerosK = k;
            numArchivos = numeros_archivos;
            clases = new int[512, 512, numeros_archivos];
            pertenencia = new double[512, 512, k, numeros_archivos];
            distancias = new double[512, 512, k, numeros_archivos];
            ite = iteraciones;
            rnd = new Random();
            generarCentros();
            for (int i = 0; i < iteraciones; i++){
                GenerarDistancias();
                ActualizarPertenencia();
                GeneraNuevosCentros();
            }
            for (int i = 0; i < 512; i++){
                for (int j = 0; j < 512; j++){
                    for (int kk = 0; kk < numArchivos; kk++){
                        int tipo = 0;
                        double valor = pertenencia[i, j, 0, kk];
                        for (int p = 1; p < numerosK; p++){
                            if (valor < pertenencia[i, j, p, kk]){
                                tipo = p;
                                valor = pertenencia[i, j, p, kk];
                            }
                        }
                        clases[i, j, kk] = tipo;
                    }
                }
            }
        }

        public void generarCentros(){
            centros = new List<Double>();
            rnd = new Random();
            for (int i = 0; i < numerosK; i++)
                centros.Add(rnd.Next(min, max));
        }

        public void GenerarDistancias(){
            for (int i = 0; i < 512; i++){
                for (int j = 0; j < 512; j++){
                    for (int p = 0; p < numArchivos; p++){
                        matriz_actual = matrices.obtenerArchivo(p);
                        for (int k = 0; k < numerosK; k++){
                            double dist = matriz_actual.ObtenerUH(i, j) - centros[k];
                            distancias[i, j, k, p] = dist * dist;
                        }
                    }
                }
            }
        }


        public void ActualizarPertenencia(){
            for (int i = 0; i < 512; i++){
                for (int j = 0; j < 512; j++){
                    for (int p = 0; p < numArchivos; p++){
                        for (int k = 0; k < numerosK; k++){
                            double sum = 0.0;
                            for (int l = 0; l < numerosK; l++)
                                sum += Math.Pow(distancias[i, j, k, p] / distancias[i, j, l, p], 2.0 / (m - 1.0));                            
                            pertenencia[i, j, k, p] = 1.0 / sum;
                        }
                    }
                }
            }
        }

        public void GeneraNuevosCentros(){
            for (int k = 0; k < numerosK; k++){
                long aa = 0;
                long bb = 0;
                for (int p = 0; p < numArchivos; p++){
                    matriz_actual = matrices.obtenerArchivo(p);
                    for (int i = 0; i < 512; i++){
                        for (int j = 0; j < 512; j++){
                            double valor = Math.Round(Math.Pow(pertenencia[i, j, k, p], m), 5);
                            if (valor <= 0.00001)
                                continue;
                            aa += (long)(Math.Round(valor * matriz_actual.ObtenerUH(i, j), 5) * 100000);
                            bb += (long)(valor * 100000);
                        }
                    }
                }
                centros[k] = (double)aa / (double)bb;
            }
        }

        public int[,,] getClases() { return clases;}

    }
}
