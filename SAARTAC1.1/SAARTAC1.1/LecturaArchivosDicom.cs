using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace SAARTAC1._1
{
    internal class LecturaArchivosDicom{

        public static MatrizDicom[] archivosDicom;
        public static int cargado = 0;
        public Thread[] threadsArray;
        private static Mutex[] mutex;
        private int numeroHilos = 4;
        private const string python = @"D:\Python27\python.exe";
        //C:\Users\raull\Documents\VersionFinalGit\SAARTAC\TT2.0C#
        private const string myPythonApp = "\"D:\\Trabajo Terminal\\SAARTAC\\TT2.0C#\\sum.py\"";
        public MatrizDicom obtenerArchivo(int x) { return archivosDicom[x]; }

        public LecturaArchivosDicom(string ruta, BackgroundWorker reporte_progreso) {
            reporte_progreso.ReportProgress(0);
            cargado = 0;
            mutex = new Mutex[numeroHilos];
            for (int i = 0; i < mutex.Length; i++)            
                mutex[i] = new Mutex();            
            int x = 0;
            string[] fileEntries = Directory.GetFiles(ruta);

            DateTime start = DateTime.Now;
            int N = fileEntries.Length;
            threadsArray = new Thread[N];
            archivosDicom = new MatrizDicom[N];
            for (int i = 0; i < N; i++){
                //Console.WriteLine(fileEntries[i]);
                string parametro = "\"" + fileEntries[i] + "\"";
                ParametroPython aux = new ParametroPython(x, parametro, i);
                threadsArray[i] = new Thread(() => Pregunta_Python(aux));

            }
            reporte_progreso.ReportProgress(3);
            for (int i = 0; i < N; i++)            
                threadsArray[i].Start();
            for (int i = 0; i < N; i++) {
                threadsArray [i].Join();
                Thread.Sleep(100);
                Console.WriteLine((cargado * 90) / N);
                reporte_progreso.ReportProgress((cargado * 90) / N);
                if (reporte_progreso.CancellationPending) {
                    return;
                }
            }
            TimeSpan timeDiff = DateTime.Now - start;
            var res = timeDiff.TotalMilliseconds;
            Console.WriteLine("Tiempo de ejecucion: " + res);
            //var pruebaImagen = archivosDicom[1].ObtenerImagen();
            //pruebaImagen.Save("prueba.jpg");
            //pruebaImagen.Dispose();

            //Console.WriteLine("llegue aqui");
            reporte_progreso.ReportProgress(90);
        }

        public int num_archivos() { return archivosDicom.Length; }

        public static double[] Pregunta_Python_Dimensiones(int pregunta, string ruta){
            
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            ruta = "\"" + ruta + "\"";
            myProcessStartInfo.Arguments = myPythonApp + " " + pregunta + " " + ruta;
            myProcessStartInfo.CreateNoWindow = true;
            Process myProcess = new Process();
            myProcess.StartInfo = myProcessStartInfo;

            //Console.WriteLine("Calling Python script with arguments {0} and {1} pos == {2}", pregunta, ruta, pos);
            myProcess.Start();

            StreamReader myStreamReader = myProcess.StandardOutput;

            string myString = myStreamReader.ReadLine();
            string[] tokens = myString.Split();
            double[] M = { Convert.ToDouble(tokens[0]), Convert.ToDouble(tokens[1]) };
            return M;
        }

        public static int EncuentraHiloLibre(){
            int pos_hilo = 0;
            while (true){
                if (!mutex[pos_hilo].WaitOne(100)){
                    pos_hilo++;
                    pos_hilo %= mutex.Length;
                }
                else  break;                
            }
            return pos_hilo;
        }

        public static void Pregunta_Python(ParametroPython o){
            int pos_hilo = EncuentraHiloLibre();
            string ruta = o.ruta;
            int pregunta = o.x;
            int pos = o.pos;

            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            myProcessStartInfo.Arguments = myPythonApp + " " + pregunta + " " + ruta;
            myProcessStartInfo.CreateNoWindow = true;
            Process myProcess = new Process();
            myProcess.StartInfo = myProcessStartInfo;

            //Console.WriteLine("Calling Python script with arguments {0} and {1} pos == {2}", pregunta, ruta, pos);
            myProcess.Start();

            StreamReader myStreamReader = myProcess.StandardOutput;

            string myString = myStreamReader.ReadLine();
            string[] tokens = myString.Split();
            int N = Convert.ToInt32(tokens[0]);
            int M = Convert.ToInt32(tokens[1]);
            MatrizDicom dicom = new MatrizDicom(ruta, N, M);
            int[,] auxMatriz = new int[N, M];
            for (int j = 0; j < N; j++){
                myString = myStreamReader.ReadLine();
                string[] tokens2 = myString.Split();
                int[] filaDicom = Array.ConvertAll(tokens2, int.Parse);
                for (int k = 0; k < M; k++)                
                    auxMatriz[k, j] = filaDicom[k] - 1000;                
            }
            dicom.CopiarMatriz(ref auxMatriz);
            myProcess.WaitForExit();
            myProcess.Close();
            archivosDicom[pos] = dicom;
            mutex[pos_hilo].ReleaseMutex();
            cargado++;
        }
    }

}
