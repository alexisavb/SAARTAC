﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SAARTAC1._1{
    class FuzzyCMeans{
        private int numerosK, ite, N, M;
        private List<Double> centros;
        private List<Double> conjunto = new List<Double>();
        private int[,] clases;
        private double[,,] pertenencia;
        private double[,,] distancias;
        private Random rnd;
        private double m = 2.0;
        private BackgroundWorker reporte_progreso;
        private static int operaciones_cargando, operaciones_total;
        private List<int []> datos;

        private List<Point> umbral_centros;
        private bool IgnorarAire;
        private int IGNORAR = -200;

        public FuzzyCMeans(List<int[]> datos, BackgroundWorker reporte_progreso, int k, int iteraciones = 10){
            IgnorarAire = Properties.Settings.Default.IgnorarAire;
            this.datos = datos;
            numerosK = k;
            this.reporte_progreso = reporte_progreso;
            operaciones_cargando = 0;
            N = datos.Count();
            M = datos [0].Length;
            clases = new int[N, M];
            pertenencia = new double[N, M, k];
            distancias = new double[N, M, k];
            ite = iteraciones;
            operaciones_total = iteraciones * N;
            rnd = new Random();
            generarCentros();
            for (int i = 0; i < iteraciones; i++){
                GenerarDistancias();
                ActualizarPertenencia();
                GeneraNuevosCentros();
                if (reporte_progreso.CancellationPending)
                    return;
            }
            GenerarDistancias();
            ActualizarPertenencia();
            for (int i = 0; i < N; i++){
                for (int j = 0; j < M; j++){
                    if (IgnorarAire && datos [i] [j] < IGNORAR) {
                        clases [i, j] = 0;
                        continue;
                    }
                    int tipo = 1;
                    double valor = pertenencia[i, j, 0];
                    for (int p = 1; p < numerosK; p++){
                        if (valor < pertenencia[i, j, p]){
                            tipo = p;
                            valor = pertenencia[i, j, p];
                        }
                    }
                    clases[i, j] = tipo + 1;
                    
                }
            }
            generaUmbralesCentros();
        }
        
        
        public FuzzyCMeans(List<int []> datos, List<Double> cent, BackgroundWorker reporte_progreso, int k, int iteraciones = 10){
            IgnorarAire = Properties.Settings.Default.IgnorarAire;
            this.datos = datos;
            numerosK = k;
            this.reporte_progreso = reporte_progreso;
            operaciones_cargando = 0;
            N = datos.Count();
            M = datos [0].Length;
            clases = new int[N, M];
            pertenencia = new double[N, M, k];
            distancias = new double[N, M, k];
            ite = iteraciones;
            operaciones_total = iteraciones * N;
            rnd = new Random();
            centros = cent;
            for (int i = 0; i < iteraciones; i++){
                GenerarDistancias();
                ActualizarPertenencia();
                GeneraNuevosCentros();
                if (reporte_progreso.CancellationPending)
                    return;
            }

            GenerarDistancias();
            ActualizarPertenencia();
            for (int i = 0; i < N; i++){
                for (int j = 0; j < M; j++){
                    if(IgnorarAire && datos[i][j] < IGNORAR) {
                        clases [i, j] = 0;
                        continue;
                    }
                    int tipo = 0;
                    double valor = pertenencia[i, j, 0];
                    for (int p = 1; p < numerosK; p++){
                        if (valor < pertenencia[i, j, p]){
                            tipo = p;
                            valor = pertenencia[i, j, p];
                        }
                    }
                    clases[i, j] = tipo + 1;
                    
                }
            }
            generaUmbralesCentros();
        }

        private int ObtenPixelRandom() {

            int filaRandom = rnd.Next(0, datos.Count());
            int columnaRandom = rnd.Next(0, datos [filaRandom].Length);
            return datos [filaRandom] [columnaRandom];
        }

        public void generarCentros(){
            centros = new List<Double>();
            rnd = new Random();
            for (int i = 0; i < numerosK; i++) {
                int numero = rnd.Next((IgnorarAire ? IGNORAR : -1000), 1400);
                centros.Add((double)numero);
            }
        }

        public void GenerarDistancias(){
            for (int i = 0; i < N; i++){
                if (reporte_progreso.CancellationPending)
                    return;
                for (int j = 0; j < M; j++){
                    if (IgnorarAire && datos [i] [j] < IGNORAR)
                        continue;
                    for (int k = 0; k < numerosK; k++){
                        double dist = datos[i][j] - centros[k];
                        distancias[i, j, k] = dist * dist;
                    }
                    
                }
            }
        }


        public void ActualizarPertenencia(){
            for (int i = 0; i < N; i++){
                operaciones_cargando++;
                reporte_progreso.ReportProgress((operaciones_cargando * 90) / operaciones_total);
                if (reporte_progreso.CancellationPending)
                    return;
                for (int j = 0; j < M; j++){
                    if (IgnorarAire && datos [i] [j] < IGNORAR)
                        continue;
                    for (int k = 0; k < numerosK; k++){
                        double sum = 0.0;
                        for (int l = 0; l < numerosK; l++)
                            sum += Math.Pow(distancias[i, j, k] / distancias[i, j, l], 2.0 / (m - 1.0));  
                        pertenencia[i, j, k] = 1.0 / sum;
                            
                    }
                    
                }
            }
        }

        public void GeneraNuevosCentros(){
            for (int k = 0; k < numerosK; k++){
                long aa = 0;
                long bb = 0;
                for (int p = 0; p < N; p++){
                    if (reporte_progreso.CancellationPending)
                        return;
                    for (int i = 0; i < M; i++){
                        if (IgnorarAire && datos [p][i] < IGNORAR)
                            continue;
                        double valor = Math.Round(Math.Pow(pertenencia[p, i, k], m), 5);
                        if (valor <= 0.00001)
                            continue;
                        aa += (long)(Math.Round(valor * datos[p][i], 5) * 100000);
                        bb += (long)(valor * 100000);
                        
                    }
                }
                if (bb <= 0.0001) {
                    centros [k] = ObtenPixelRandom();
                } else {
                    centros [k] = (double)aa / (double)bb;
                }
            }
        }

        public int[,] getClases() { return clases;}
        public List<Point> ObtenerUmbralesGrupos() {
            return umbral_centros;
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

    }
}
