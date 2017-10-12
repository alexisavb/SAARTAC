using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAARTAC1._1
{
    class kMeans
    {

        private LecturaArchivosDicom matrices;
        private MatrizDicom matriz_actual;
        private int numerosK, ite;
        private int min = -1000, max = 2000;
        private List<Double> centros;
        private List<Double> conjunto = new List<Double>();
        private int[,,] clases;
        private Random rnd;

        public kMeans(LecturaArchivosDicom lect, int k, int iteraciones, int numeros_archivos){
            matrices = lect;
            numerosK = k;
            clases = new int[512, 512, numeros_archivos];
            ite = iteraciones;
            generarCentros();
            mainKmeans();
        }

        public void generarCentros(){
            centros = new List<Double>();
            rnd = new Random();
            for (int i = 0; i < numerosK; i++)
                centros.Add(rnd.Next(min, max));
        }

        public void mainKmeans(){
            for (int k = 0; k < ite; k++){
                Console.WriteLine(k + 1);
                for (int p = 0; p < matrices.num_archivos(); p++){
                    matriz_actual = matrices.obtenerArchivo(p);
                    for (int i = 0; i < 512; i++)
                        for (int j = 0; j < 512; j++)
                            distanciaEuclidiana(i, j, p);

                }
                promedio();
            }

        }

        public void distanciaEuclidiana(int i, int j, int p){
            //if (matriz_actual.ObtenerUH(i, j) < -890) return;
            int indc = 0;
            conjunto.Clear();
            foreach (Double indice in centros){
                Double resta = Math.Abs(matriz_actual.ObtenerUH(i, j) - indice);
                conjunto.Add(resta);
            }
            for (int k = 1; k < conjunto.Count; k++)
                if (conjunto[indc] > conjunto[k])
                    indc = k;
            clases[i, j, p] = indc + 1;
        }

        public void promedio(){
            centros.Clear();
            double[] sumas = new double[numerosK];
            double[] contador = new double[numerosK];
            for (int i = 0; i < numerosK; i++)            
                sumas[i] = contador[i] = 0;            
            for (int p = 0; p < matrices.num_archivos(); p++){
                matriz_actual = matrices.obtenerArchivo(p);
                for (int i = 0; i < 512; i++){
                    for (int j = 0; j < 512; j++){
                        //if (matriz_actual.ObtenerUH(i, j) < -890) continue;
                        sumas[clases[i, j, p] - 1] += matriz_actual.ObtenerUH(i, j);
                        contador[clases[i, j, p] - 1]++;
                    }
                }
            }
            centros.Clear();
            for (int i = 0; i < numerosK; i++)            
                centros.Add(sumas[i] / contador[i]);
            
        }

        public int[,,] getClases(){ return clases;}

    }
}
