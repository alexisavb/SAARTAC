﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;



namespace SAARTAC1._1
{
    class kMeans
    {
        private int numerosK, ite;
        private List<Double> centros;
        private List<Double> conjunto = new List<Double>();
        private int[,] clases;
        private Random rnd = new Random();
        private BackgroundWorker reporte_progreso;
        private static int operaciones_cargando, operaciones_total;
        private List<int []> datos;
        private List<Point> umbral_centros;
        private bool IgnorarAire;
        private int IGNORAR = -200;

        public kMeans(List<int[]> datos, int k, int iteraciones, BackgroundWorker bw){
            IgnorarAire = Properties.Settings.Default.IgnorarAire;
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
            generaUmbralesCentros();
        }

        private void generaUmbralesCentros() {
            umbral_centros = new List<Point>();
            for (int i = 0; i < numerosK + 1; i++) {
                umbral_centros.Add(new Point(-10000, 10000));
            }
            for (int i = 0; i < datos.Count; i++) {
                for (int j = 0; j < datos [0].Length; j++) {
                    int grupo = clases [i, j];
                    int x = Math.Max(umbral_centros [grupo].X, datos [i] [j]);
                    int y = Math.Min(umbral_centros [grupo].Y, datos [i] [j]);
                    umbral_centros [grupo] = new Point(x, y);
                }
            }
        }

        public List <Point> ObtenerUmbralesGrupos() {
            return umbral_centros;
        }

        public kMeans(List<int []> datos, int k, int iteraciones, BackgroundWorker bw, List<Double> cent){

            IgnorarAire = Properties.Settings.Default.IgnorarAire;
            this.datos = datos;
            numerosK = k;
            reporte_progreso = bw;
            reporte_progreso.ReportProgress(0);
            operaciones_cargando = 0;
            clases = new int [datos.Count, datos [0].Length];
            ite = iteraciones;

            operaciones_total = ite * datos.Count();
            centros = cent;
            mainKmeans();
            generaUmbralesCentros();
        }

        private int ObtenPixelRandom() {

            int filaRandom = rnd.Next(0, datos.Count());
            int columnaRandom = rnd.Next(0, datos [filaRandom].Length);
            return datos [filaRandom] [columnaRandom];
        }

        public void generarCentros(){
            centros = new List<Double>();
            for (int i = 0; i < numerosK; i++) {
                int numero = rnd.Next((IgnorarAire ? IGNORAR : -1000), 1400);
                centros.Add((double)numero);
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
            for (int i = 0; i < datos.Count; i++) {
                for (int j = 0; j < datos [i].Length; j++) {
                    distanciaEuclidiana(i, j);
                }
            }

        }

        public void distanciaEuclidiana(int i, int j){
            if (IgnorarAire && datos[i][j] < IGNORAR) return;
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
                    if (IgnorarAire && datos [i] [j] < IGNORAR)
                        continue;
                    sumas[clases[i, j] - 1] += datos[i][j];
                    contador[clases[i, j] - 1]++;
                }
                if (reporte_progreso.CancellationPending)
                    return;
                operaciones_cargando++;
                reporte_progreso.ReportProgress((90 * operaciones_cargando) / operaciones_total);
            }
            centros.Clear();
            for (int i = 0; i < numerosK; i++) {
                if (contador [i] == 0) {
                    centros.Add(ObtenPixelRandom());
                } else {
                    centros.Add(sumas [i] / contador [i]);
                }
            }
            
        }

        public int[,] getClases(){ return clases;}

    }
}
