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
            anchoRectanguloSeleccion = PuntoFin.X - PuntoInicio.X;
            alturaRectanguloSeleccion = PuntoFin.Y - PuntoInicio.Y;
        }

        public void setRectangle(){
            RectanguloSeleccion = new Rectangle(PuntoInicio.X, PuntoInicio.Y, anchoRectanguloSeleccion, alturaRectanguloSeleccion);
            pen = new Pen(Color.Red, 1);
            pen.DashPattern = valoresLineaDiscontinua;
        }

        public Pen getPen() { return pen; }

        public Rectangle getRectangle() { return RectanguloSeleccion; }


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
