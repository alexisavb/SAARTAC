using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace CallPython {

    class MatrizDicom
    {
        private static int N;
        public int[,] matriz;
        public int minValor;
        public int maxValor;

        public MatrizDicom()
        {
            N = 512;
            matriz = new int[N + 10, N + 10];
            minValor = 10000;
            maxValor = -10000;
        }

        public void CopiarMatriz(ref int[,] A)
        {
            for(int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    matriz[i, j] = A[i, j];
                    minValor = Math.Min(minValor, matriz[i, j]);
                    maxValor = Math.Max(maxValor, matriz[i, j]);
                }
            }
        }
        
        public void ImprimeMatriz()
        {
            Console.WriteLine(matriz[0, 1]);
               
             
        }



        public Bitmap ObtenerImagen() {
            Bitmap imagen = new Bitmap(512, 512);
            int tam = maxValor - minValor + 1;
            double porcion = 255.0 / tam;

            for (int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    int valorGris = (int)(porcion * matriz[i, j]);
                    Color color = Color.FromArgb(valorGris, valorGris, valorGris);
                    imagen.SetPixel(i, j, color);
                }
            }
            Console.ReadLine();
            return imagen;

        }
    }

    class ParametroPython
    {
        public int x;
        public string ruta;
        public int pos;
        public ParametroPython(int x, string ruta, int pos)
        {
            this.x = x;
            this.ruta = ruta;
            this.pos = pos;
        }
    }

    class LecturaArchivosDicom {
        
        public static MatrizDicom[] archivosDicom;

        public LecturaArchivosDicom(string ruta) {
            int x = 0;
            string[] fileEntries = Directory.GetFiles(ruta);

            DateTime start = DateTime.Now;
            int N = fileEntries.Length;
            Thread[] threadsArray = new Thread[N];
            archivosDicom = new MatrizDicom[N];
            for (int i = 0; i < N; i++)
            {
                archivosDicom[i] = new MatrizDicom();
            }
            for (int i = 0; i < N; i++)
            {
                Console.WriteLine(fileEntries[i]);
                string parametro = "\"" + fileEntries[i] + "\"";
                ParametroPython aux = new ParametroPython(x, parametro, i);
                threadsArray[i] = new Thread(() => Pregunta_Python(aux));

            }
            for (int i = 0; i < N; i++)
            {
                threadsArray[i].Start();
            }
            for (int i = 0; i < N; i++)
            {
                threadsArray[i].Join();
            }
            
            TimeSpan timeDiff = DateTime.Now - start;
            var res = timeDiff.TotalMilliseconds;

            Console.WriteLine("Tiempo de ejecucion: " + res);
            var pruebaImagen = archivosDicom[1].ObtenerImagen();
            pruebaImagen.Save("prueba.jpg");
            pruebaImagen.Dispose();

            Console.ReadLine();
        }
        public static void Pregunta_Python(ParametroPython o) {
            string ruta = o.ruta;
            int pregunta = o.x;
            int pos = o.pos;
            MatrizDicom dicom = new MatrizDicom();
            string python = @"D:\Python27\python.exe";
            string myPythonApp = @"C:\Users\edgar\Downloads\sum.py";

            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            myProcessStartInfo.Arguments = myPythonApp + " " + pregunta + " " + ruta;

            Process myProcess = new Process();
            myProcess.StartInfo = myProcessStartInfo;

            Console.WriteLine("Calling Python script with arguments {0} and {1} pos == {2}", pregunta, ruta, pos);
            myProcess.Start();

            StreamReader myStreamReader = myProcess.StandardOutput;

            string myString = myStreamReader.ReadLine();
            int M = Convert.ToInt32(myString);  
            int[,] auxMatriz = new int [512, 512];
            for (int j = 0; j < M; j++)
            {
                myString = myStreamReader.ReadLine();
                string[] tokens = myString.Split();
                int[] filaDicom = Array.ConvertAll(tokens, int.Parse);
                for(int k = 0; k < M; k++)
                {
                    auxMatriz[j, k] = filaDicom[k];
                }
            }
            dicom.CopiarMatriz(ref auxMatriz);
            myProcess.WaitForExit();
            myProcess.Close();
            archivosDicom[pos] = dicom;
        }
    }
}
