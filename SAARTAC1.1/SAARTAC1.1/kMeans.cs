using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private int[,] clases;
        private Random rnd;
        private BackgroundWorker reporte_progreso;
        private static int operaciones_cargando, operaciones_total;
        private List<int []> datos;

        public kMeans(List<int[]> datos, int k, int iteraciones, BackgroundWorker bw){
            this.datos = datos;    
            numerosK = k;
            reporte_progreso = bw;
            reporte_progreso.ReportProgress(0);
            operaciones_cargando = 0;
            clases = new int[datos.Count, datos[0].Length];
            ite = iteraciones;

            operaciones_total = ite * datos.Count();
            generarCentros();
            mainKmeans();
        }

        public void generarCentros(){
            centros = new List<Double>();
            rnd = new Random();
            for (int i = 0; i < numerosK; i++) {
                int filaRandom = rnd.Next(0, datos.Count());
                int columnaRandom = rnd.Next(0, datos [filaRandom].Length);
                centros.Add((double)datos [filaRandom][columnaRandom]);
            }
        }

        public void mainKmeans(){
            for (int k = 0; k < ite; k++){
                Console.WriteLine(k + 1);
                for(int i = 0; i < datos.Count; i++) {
                    for(int j = 0; j < datos[i].Length; j++) {
                        distanciaEuclidiana(i, j);
                    }
                }
                
                promedio();
                if (reporte_progreso.CancellationPending)
                    return;
            }

        }

        public void distanciaEuclidiana(int i, int j){
            //if (matriz_actual.ObtenerUH(i, j) < -890) return;
            int indc = 0;
            conjunto.Clear();
            foreach (Double indice in centros){
                Double resta = Math.Abs(datos[i][j] - indice);
                conjunto.Add(resta);
            }
            for (int k = 1; k < conjunto.Count; k++)
                if (conjunto[indc] > conjunto[k])
                    indc = k;
            clases[i, j] = indc + 1;
        }

        public void promedio(){
            centros.Clear();
            double[] sumas = new double[numerosK];
            double[] contador = new double[numerosK];
            for (int i = 0; i < numerosK; i++)            
                sumas[i] = contador[i] = 0;            
            for (int i = 0; i < datos.Count; i++){
                for (int j = 0; j < datos[i].Length; j++){
                    //if (matriz_actual.ObtenerUH(i, j) < -890) continue;
                    sumas[clases[i, j] - 1] += datos[i][j];
                    contador[clases[i, j] - 1]++;
                }
                if (reporte_progreso.CancellationPending)
                    return;
                operaciones_cargando++;
                reporte_progreso.ReportProgress((90 * operaciones_cargando) / operaciones_total);
            }
            centros.Clear();
            for (int i = 0; i < numerosK; i++)            
                centros.Add(sumas[i] / contador[i]);
            
        }

        public int[,] getClases(){ return clases;}

    }
}
