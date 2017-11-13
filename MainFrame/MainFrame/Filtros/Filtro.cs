using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.IO;

namespace MainFrame.Filtros
{
    class Filtro
    {
        private int height;
        private int width;

        public Filtro(int w, int h)
        {
            this.height = h;
            this.width = w;
        }

        public void setHeight(int h)
        {
            this.height = h;
        }

        public int getHeight()
        {
            return height;
        }

        public void setWidth(int w)
        {
            this.width = w;
        }

        public int getWidth()
        {
            return width;
        }

        private int convolucion(int x,int y,int[,]img,int[,]mask)
        {
            int r = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    r += (img[(x + i),(y + j)]) * (mask[i,j]);
                }
            }
            return r;
        }

        public Bitmap toBW(Bitmap b)
        {
            Bitmap bw = new Bitmap(b.Height, b.Width);
            Color r;
            int neg=0;
            for (int i = 0; i < b.Height; i++)
            {
                for (int j = 0; j < b.Width; j++)
                {
                    r = b.GetPixel(i, j);
                    neg = (r.R + r.G + r.B)/3;
                    bw.SetPixel(i, j,Color.FromArgb(neg,neg,neg));
                }
            }
            return bw;
        }

        private int[,] toArray(Bitmap b)
        {
            int[,] a = new int[b.Height,b.Width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    a[i,j] = b.GetPixel(i, j).R;
                }
            }
            return a;
        }

        private Bitmap toBitmap(int[,] a)
        {
            Bitmap b = new Bitmap(height, width);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (a[i, j] < 0)
                        a[i, j] = 0;
                    else if (a[i, j] > 255)
                        a[i, j] = 255;
                    b.SetPixel(i, j, Color.FromArgb(a[i, j], a[i, j], a[i, j]));
                }
            }
            return b;
        }

        public Bitmap filtroPrewitt(Bitmap imo)
        {
            Bitmap b = new Bitmap(height, width);
            int[,] array = toArray(toBW(imo));
            int[,] res = array;
            int[,] prewit = {{-1,-1,-1},{0,0,0},{1,1,1}};
            int p;
            for (int j = 0; j < b.Height-2 ; j++)
            {
                for (int k = 0; k < b.Width-2 ; k++)
                {
                    p = this.convolucion(j, k, array, prewit) / 3;
                    res[j,k] = p;
                }
            }
            b = this.toBitmap(res);
            return b;
        }

        public Bitmap filtroSobel(Bitmap imo)
        {
            Bitmap b = new Bitmap(height, width);
            int[,] array = toArray(toBW(imo));
            int[,] res = array;
            int[,] sobel = { { -1, 0, -1 }, { -2, 0, 2 }, { -1, 0, -1 } };
            int p;
            for (int j = 0; j < b.Height - 2; j++)
            {
                for (int k = 0; k < b.Width - 2; k++)
                {
                    p = this.convolucion(j, k, array, sobel) / 4;
                    res[j, k] = p;
                }
            }
            b = this.toBitmap(res);
            return b;
        }

        public Bitmap filtroRoberts(Bitmap imo)
        {
            Bitmap b = new Bitmap(height, width);
            int[,] array = toArray(toBW(imo));
            int[,] res = array;
            int[,] roberts = { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
            int p;
            for (int j = 0; j < b.Height - 2; j++)
            {
                for (int k = 0; k < b.Width - 2; k++)
                {
                    p = this.convolucion(j, k, array, roberts);
                    res[j, k] = p;
                }
            }
            b = this.toBitmap(res);
            return b;
        }

        public Bitmap filtroPromedio(Bitmap imo)
        {
            Bitmap b = new Bitmap(height, width);
            int[,] array = toArray(toBW(imo));
            int[,] res = array;
            int[,] prom = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            int p;
            for (int j = 0; j < b.Height - 2; j++)
            {
                for (int k = 0; k < b.Width - 2; k++)
                {
                    p = (this.convolucion(j, k, array, prom))/ 9;
                    res[j, k] = p;
                }
            }
            b = this.toBitmap(res);
            return b;
        }
        
        public Bitmap filtroLaplaciano(Bitmap imo)
        {
            Bitmap b = new Bitmap(height, width);
            int[,] array = toArray(toBW(imo));
            int[,] res = array;
            int[,] prom = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            int p;
            for (int j = 0; j < b.Height - 2; j++)
            {
                for (int k = 0; k < b.Width - 2; k++)
                {
                    p = (this.convolucion(j, k, array, prom));
                    res[j, k] = p;
                }
            }
            b = this.toBitmap(res);
            return b;
        }

        public Bitmap filtroMenosMedia(Bitmap imo)
        {
            Bitmap b = new Bitmap(height, width);
            int[,] array = toArray(toBW(imo));
            int[,] res = array;
            int[,] prom = { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };
            int p;
            for (int j = 0; j < b.Height - 2; j++)
            {
                for (int k = 0; k < b.Width - 2; k++)
                {
                    p = (this.convolucion(j, k, array, prom)) / 9;
                    res[j, k] = p;
                }
            }
            b = this.toBitmap(res);
            return b;
        }

        public Bitmap filtroMenosLaplaciano(Bitmap imo)
        {
            Bitmap b = new Bitmap(height, width);
            int[,] array = toArray(toBW(imo));
            int[,] res = array;
            int[,] prom = { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            int p;
            for (int j = 0; j < b.Height - 2; j++)
            {
                for (int k = 0; k < b.Width - 2; k++)
                {
                    p = (this.convolucion(j, k, array, prom)) / 9;
                    res[j, k] = p;
                }
            }
            b = this.toBitmap(res);
            return b;
        }

        public Bitmap filtroGaussiano(Bitmap imo)
        {
            Bitmap b = new Bitmap(height, width);
            int[,] array = toArray(toBW(imo));
            int[,] res = array;
            int[,] prom = { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
            //int[,] prom = { { 2, 4, 5, 4, 2 }, { 4, 9, 12, 9, 4 }, { 5, 12, 15, 12, 5 }, { 4, 9, 12, 9, 4 }, { 2, 4, 5, 4, 2 } };
            int p;
            for (int j = 0; j < b.Height - 2; j++)
            {
                for (int k = 0; k < b.Width - 2; k++)
                {
                    p = (this.convolucion(j, k, array, prom)) / 16;
                    res[j, k] = p;
                }
            }
            b = this.toBitmap(res);
            return b;
        }

        private Bitmap binarizar(Bitmap imo)
        {
            Bitmap bin = new Bitmap(height, width);
            bin = toBW(imo);
            int pix = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pix = (bin.GetPixel(i, j)).R;
                    if (pix >=50)
                        bin.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else
                    {
                        bin.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return bin;
        }
        
        public Bitmap contorno(Bitmap imo)
        {
            int[,] array = toArray(binarizar(imo));
            int[,] res = new int[height, width];
            int cx = 0, cy = 0;
            int puntos = 0;
            bool ok = false;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (array[i,j] == 255)
                    {
                        cx = i; cy = j;
                        ok = true;
                        res[i, j] = array[i, j];
                        array[i, j] = 0;
                        while (ok)
                        {
                            if (i == 0 || j == 0 || i == 511 || j == 511)
                                ok = false;
                            else
                            {
                                if (array[i, j - 1] == 255)
                                {
                                    res[i, j - 1] = array[i, j - 1];
                                    array[i, j - 1] = 0;
                                    j = j - 1;
                                    puntos++;
                                }
                                else if (array[i + 1, j] == 255)
                                {
                                    res[i + 1, j] = array[i + 1, j];
                                    array[i + 1, j] = 0;
                                    i = i + 1;
                                    puntos++;
                                }
                                else if (array[i, j + 1] == 255)
                                {
                                    res[i, j + 1] = array[i, j + 1];
                                    array[i, j + 1] = 0;
                                    j = j + 1;
                                    puntos++;
                                }
                                else if (array[i - 1, j] == 255)
                                {
                                    res[i - 1, j] = array[i - 1, j];
                                    array[i - 1, j] = 0;
                                    i = i - 1;
                                    puntos++;
                                }
                                else if (array[i - 1, j - 1] == 255)
                                {
                                    res[i - 1, j - 1] = array[i - 1, j - 1];
                                    array[i - 1, j - 1] = 0;
                                    i = i - 1;
                                    j = j - 1;
                                    puntos++;
                                }
                                else if (array[i + 1, j - 1] == 255)
                                {
                                    res[i + 1, j - 1] = array[i + 1, j - 1];
                                    array[i + 1, j - 1] = 0;
                                    i = i + 1;
                                    j = j - 1;
                                    puntos++;
                                }
                                else if (array[i - 1, j + 1] == 255)
                                {
                                    res[i - 1, j + 1] = array[i - 1, j + 1];
                                    array[i - 1, j + 1] = 0;
                                    i = i - 1;
                                    j = j + 1;
                                    puntos++;
                                }
                                else if (array[i + 1, j + 1] == 255)
                                {
                                    res[i + 1, j + 1] = array[i + 1, j + 1];
                                    array[i + 1, j + 1] = 0;
                                    i = i + 1;
                                    j = j + 1;
                                    puntos++;
                                }
                                else ok = false;
                            }
                        }
                        if (puntos < 50)
                            res = new int[width, height];
                        i = cx; j = cy;
                    }
                }                    
            }
            return toBitmap(res) ;
        }
    }
}
