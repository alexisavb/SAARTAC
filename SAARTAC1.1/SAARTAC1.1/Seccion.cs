using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAARTAC1._1
{
    class Seccion{
        private MatrizDicom auxUH;
        private Point PuntoInicio, PuntoFin;
        private Rectangle RectanguloSeleccion = new Rectangle(new Point(0, 0), new Size(0, 0));
        private int alturaRectanguloSeleccion, anchoRectanguloSeleccion;
        private float[] valoresLineaDiscontinua = { 3, 2, 3, 2 };
        private Pen pen;

        public Seccion(int x, int y, MatrizDicom md){
            PuntoInicio = new Point(x, y);
            auxUH = md;
        }

        public void setFinal(int x, int y){
            PuntoFin = new Point(x, y);
            anchoRectanguloSeleccion = PuntoFin.X - PuntoInicio.X + 1;
            alturaRectanguloSeleccion = PuntoFin.Y - PuntoInicio.Y + 1;
        }

        public void setRectangle(){
            RectanguloSeleccion = new Rectangle(PuntoInicio.X, PuntoInicio.Y, anchoRectanguloSeleccion, alturaRectanguloSeleccion);
            pen = new Pen(Color.Red, 1);
            pen.DashPattern = valoresLineaDiscontinua;
        }

        public Pen getPen() { return pen; }

        public Rectangle getRectangle() { return RectanguloSeleccion; }

        public Bitmap obtenerImagen(Bitmap original) {
            Bitmap salida = original.Clone(RectanguloSeleccion, original.PixelFormat);
            return salida;
        }

        public int [] ObtenerDatosRegion(MatrizDicom archivo) {
            int tamX = PuntoFin.X - PuntoInicio.X + 1;
            int tamY = PuntoFin.Y - PuntoInicio.Y + 1;
            int [] datos = new int [tamX * tamY];
            int pos = 0;
            for (int i = PuntoInicio.X; i <= PuntoFin.X; i++) {
                for (int j = PuntoInicio.Y; j <= PuntoFin.Y; j++) {
                    datos [pos++] = archivo.ObtenerUH(i, j);
                }
            }
            return datos;

        }

        public int createAverage(){
            int limY = RectanguloSeleccion.Y + RectanguloSeleccion.Height;
            int limX = RectanguloSeleccion.X + RectanguloSeleccion.Width;
            int iniX = RectanguloSeleccion.X;
            int iniY = RectanguloSeleccion.Y;
            if (limY < iniY)
            {
                int aux = iniY;
                iniY = limY;
                limY = aux;
            }
            if (limX < iniX)
            {
                int aux = iniX;
                iniX = limX;
                limX = aux;
            }

            int sum = 0, cnt = 0;
            for (int i = iniY; i <= limY; i++){
                for (int j = iniX; j <= limX; j++){
                    sum += auxUH.ObtenerUH(j, i);
                    cnt++;
                }
            }
            return sum / cnt;
        }
    }
}
