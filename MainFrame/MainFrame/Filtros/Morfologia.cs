using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace MainFrame
{
    class Morfologia
    {
        private int height;
        private int width;

        int[,] ee = { { 1, 1, 0, 1, 1 }, 
                      { 1, 0, 0, 0, 1 }, 
                      { 0, 0, 0, 0, 0 }, 
                      { 1, 0, 0, 0, 1 }, 
                      { 1, 1, 0, 1, 1 } };

        public Morfologia(int w, int h)
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

        public Bitmap dilatacion(Bitmap imo)
        {
            Bitmap b = new Bitmap(height,width);
            int c,r,cc, rr;
            int pix;
            bool ok;
            for(c= 0; c < b.Height - 4; c++){
                for(r = 0; r< b.Width - 4; r++){
                    cc= 0; ok = true;
                    while(cc < 5 && ok){
                        rr= 0;
                        while(rr < 5 && ok){
                            pix=(imo.GetPixel(c+cc,r+rr)).R;
                            if(ee[cc,rr]== 0 && pix != 0)
                                ok= false;
                            rr++;
                        }
                        cc++;
                    }
                    if(ok)
                        b.SetPixel((c+1),(r+1),Color.FromArgb(0,0,0));
                }
            }
            return b;
        }

        public Bitmap erosion(Bitmap imo)
        {
            Bitmap b = new Bitmap(height,width);
            int y, x, xx, yy;
            for(x = 1; x < b.Height - 4; x++){
                for(y = 1; y < b.Width - 4; y++){
                    if((imo.GetPixel(x,y)).R == 0){
                        int col= x-1, ren= y-1;
                        for(xx=0; xx < 5; xx++){
                            for(yy=0; yy< 5; yy++){
                                if(ee[xx,yy]== 0)
                                    b.SetPixel(col+xx,ren+yy,Color.FromArgb(0,0,0));
                            }
                        }
                    }
                }
            }
            return b;
        }
    }
}
