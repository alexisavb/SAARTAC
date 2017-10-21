
using System.Drawing;

namespace SAARTAC1._1{
    internal class ParametroContraste {
        public int inicio, fin, indice;
        public int [,] matriz;
        public ParametroContraste(int[,] matriz, int inicio, int fin, int indice) {
            this.inicio = inicio;
            this.fin = fin;
            this.indice = indice;
            this.matriz = matriz;
        }
    }
    internal class ParametroUmbralizacion {
        public string tipo;
        public MatrizDicom archivo;
        public int indice;
        public Color color;

        public ParametroUmbralizacion(string tipo, MatrizDicom archivo, int indice, Color color) {
            this.tipo = tipo;
            this.archivo = archivo;
            this.indice = indice;
            this.color = color;
        }
    }

    internal class ParametroPython {
        public int x;
        public string ruta;
        public int pos;
        public ParametroPython(int x, string ruta, int pos){
            this.x = x;
            this.ruta = ruta;
            this.pos = pos;
        }
    }
}
