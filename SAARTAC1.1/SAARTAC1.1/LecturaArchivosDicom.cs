using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Configuration;

namespace SAARTAC1._1
{
    internal class LecturaArchivosDicom{

        public static MatrizDicom[] archivosDicom;
        public static int cargado, numeroHilos;
        private static Mutex mutex;
        private static string python, myPythonApp;

        public MatrizDicom obtenerArchivo(int x) { return archivosDicom[x]; }

        public LecturaArchivosDicom(string ruta, BackgroundWorker reporte_progreso) {
            numeroHilos = Properties.Settings.Default.NumeroProcesos;
            python = Properties.Settings.Default.rutaPython;
            myPythonApp = "\"" + Properties.Settings.Default.rutaLecturaDicom + "\"";

            reporte_progreso.ReportProgress(0);
            cargado = 0;
            mutex = new Mutex();

            string[] fileEntries = Directory.GetFiles(ruta);
            int N = fileEntries.Length;
            Thread [] threadsArray = new Thread[N];
            archivosDicom = new MatrizDicom[N];

            for (int i = 0; i < N; i++){
                string parametro = "\"" + fileEntries[i] + "\"";
                ParametroPython aux = new ParametroPython(0, parametro, i);
                threadsArray[i] = new Thread(() => Pregunta_Python(aux));

            }

            reporte_progreso.ReportProgress(3);
            for (int i = 0; i < N; i++)            
                threadsArray[i].Start();

            while(cargado < N) {
                Thread.Sleep(100);
                Console.WriteLine((cargado * 90) / N);
                reporte_progreso.ReportProgress((cargado * 90) / N);
                if (reporte_progreso.CancellationPending) {
                    return;
                }
            }
            
            reporte_progreso.ReportProgress(90);
        }

        public int num_archivos() { return archivosDicom.Length; }

        public static string PreguntaPythonGeneral(int pregunta, string ruta) {
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            ruta = "\"" + ruta + "\"";
            myProcessStartInfo.Arguments = myPythonApp + " " + pregunta + " " + ruta;
            myProcessStartInfo.CreateNoWindow = true;

            Process myProcess = new Process();
            myProcess.StartInfo = myProcessStartInfo;
            myProcess.Start();

            StreamReader myStreamReader = myProcess.StandardOutput;

            return myStreamReader.ReadLine();
        }

        public static double [] Pregunta_Python_Dimensiones(string ruta) {
            string myString = PreguntaPythonGeneral(DimensionesPixel, ruta);
            string [] tokens = myString.Split();
            double [] M = { Convert.ToDouble(tokens [0]), Convert.ToDouble(tokens [1]) };
            return M;
        }

        public static string PreguntaNombre(string ruta) {
            string myString = PreguntaPythonGeneral(Nombre, ruta);
            return myString.Replace('^', ' ');
        }

        public static int PreguntaEdad(string ruta) {

            string myString = PreguntaPythonGeneral(Edad, ruta);
            myString = myString.Remove(myString.Length - 1);
            return Int32.Parse(myString);
        }

        public static int PreguntaEspesor(string ruta) {

            string myString = PreguntaPythonGeneral(EspesorRebanada, ruta);
            return Int32.Parse(myString);
        }

        public static string PreguntaHospital(string ruta) {

            return PreguntaPythonGeneral(Hospital, ruta);
        }

        public static string PreguntaSexo(string ruta) {

            return PreguntaPythonGeneral(Sexo, ruta);
        }

        public static string PreguntaFecha(string ruta) {

            string myString = PreguntaPythonGeneral(Sexo, ruta);
            string fecha = myString.Substring(6, 2) + "/" + myString.Substring(4, 2) + "/" + myString.Substring(0, 4);
            return fecha;
        }

        public static void EncuentraHiloLibre(){
            while (true){
                mutex.WaitOne();
                if(numeroHilos > 0) {
                    numeroHilos--;
                    mutex.ReleaseMutex();
                    return;
                }        
                mutex.ReleaseMutex();
            }
        }

        public static void Pregunta_Python(ParametroPython o){
            EncuentraHiloLibre();
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

            mutex.WaitOne();
            cargado++;
            numeroHilos++;
            mutex.ReleaseMutex();
        }

        const int DimensionesPixel = 1;
        const int Edad = 3;
        const int EspesorRebanada = 6;
        const int Fecha = 5;
        const int Hospital = 7;
        const int MatrizDICOM = 0;
        const int Nombre = 2;
        const int Sexo = 4;

    }

}
