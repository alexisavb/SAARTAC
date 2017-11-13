using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;
using System.Drawing;
using System.Windows.Forms;

namespace MainFrame
{    

    public class Dicom
    {
        vtkDICOMImageReader reader = vtkDICOMImageReader.New();        
        vtkImageData data = vtkImageData.New();
        String folder;
        vtkDataArray array;
        vtkStringArray fnames;
        int width = 512;
        int height = 512;
        String descriptiveName,studyId,studyUid,patientName;
        double[] pixsp;
        int pixrep;


        public Dicom(String fold)
        {
            vtkFileOutputWindow ow = vtkFileOutputWindow.New();
            ow.SetFileName("error.log");
            vtkOutputWindow.SetInstance(ow);
            this.folder = fold;
            reader.FileLowerLeftOff();            
            reader.SetDataByteOrderToLittleEndian();
            reader.SetDataOrigin(0, 0, 0);              
            reader.SetDirectoryName(fold);
            reader.Update();
            descriptiveName = reader.GetDescriptiveName();
            patientName = reader.GetPatientName();
            pixrep = reader.GetPixelRepresentation();
            pixsp = reader.GetPixelSpacing();
            studyId = reader.GetStudyID();
            studyUid = reader.GetStudyUID();
            data = reader.GetOutput();             
            array = data.GetPointData().GetScalars();
        }

        public Dicom(vtkStringArray names)
        {
            fnames = names;
            reader.FileLowerLeftOff();            
            reader.SetFileNames(names);
            reader.Update();
            data = reader.GetOutput();
            array = data.GetPointData().GetScalars();
        }

        public String getPatientName()
        {
            return patientName;
        }

        public uint getError(){
            return reader.GetErrorCode();
        }

        public double[] getPixsp()
        {
            return pixsp;
        }

        public vtkDataArray getArray()
        {
            return array;
        }

        public vtkImageData getImageData()
        {
            return data;
        }
        
        public double getValueAt(int i, int j, int n){
            double val = 0;
            int ii = height - 1 - i;
            int jj = width - 1 - j;
            val = array.GetTuple1(((jj * width) + ii) + (n * width * width));
            return val;
        }

        public void setValueAt(int i, int j, int n, int val)
        {
            int ii = height - 1 - i;
            int jj = width - 1 - j;
            array.SetTuple1(((ii * width) + jj) + (n * width * width), val);
        }

        public double[,] getArrayofSlice(int n)
        {
            double[,] aos = new double[height,width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    aos[i, j] = getValueAt(i, j, n);
                }
            }
            return aos;
        }

        public Bitmap segmentar(int slice)
        {
            Bitmap ImagenR = new Bitmap(width, height);            
            double[,] pixelsHu = getArrayofSlice(slice);
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    //Ventana para hueso compacto
                    if (pixelsHu[i, j] > 200 && pixelsHu[i, j] < 1100)
                        ImagenR.SetPixel(i, j, Color.White);
                    //Ventana para calcificaciones
                    else if (pixelsHu[i, j] > 76 && pixelsHu[i, j] < 200)
                        ImagenR.SetPixel(i, j, Color.GhostWhite);
                    //Ventana para sangre cuagulada
                    else if (pixelsHu[i, j] > 56 && pixelsHu[i, j] < 76)
                        ImagenR.SetPixel(i, j, Color.Red);
                    //Ventana para sustancia cerebral Gris
                    else if (pixelsHu[i, j] > 36 && pixelsHu[i, j] < 46)
                        ImagenR.SetPixel(i, j, Color.Silver);
                    //Ventana para sustancia cerebral Blanca
                    else if (pixelsHu[i, j] > 22 && pixelsHu[i, j] < 36)
                        ImagenR.SetPixel(i, j, Color.Silver);
                    //Ventana para liquido
                    else if (pixelsHu[i, j] > -10 && pixelsHu[i, j] < 22)
                        ImagenR.SetPixel(i, j, Color.Blue);
                    //Ventana para Grasa
                    else if (pixelsHu[i, j] > -800 && pixelsHu[i, j] < -80)
                        ImagenR.SetPixel(i, j, Color.Yellow);
                    //Resto es aire                    
                    else
                        ImagenR.SetPixel(i, j, Color.Black);
            ImagenR.RotateFlip(RotateFlipType.Rotate270FlipX);
            return ImagenR;
        }

        public Bitmap getTejido(int slice , String tej)
        {
            Bitmap tejido = new Bitmap(height,width);
            //for (int i = 0; i < 200; i++)
            //{
            //    setValueAt(10, i, 4, -2000);
            //    Console.WriteLine(getValueAt(10,i,4)+ "t");
            //}
            double[,] pixelsHu = getArrayofSlice(slice);
            //Console.WriteLine(pixelsHu[10,20] + "=" + getValueAt(10,20,slice));
            if(tej.Equals("Hueso Compacto"))//Ventana para hueso compacto
            {                 
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)   
                        if (pixelsHu[i, j] > 200 && pixelsHu[i, j] < 1100)
                            tejido.SetPixel(i, j, Color.White);
                        else
                            tejido.SetPixel(i, j, Color.Black);
            }
            else if(tej.Equals("Calcificaciones"))//Ventana para calcificaciones
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++) 
                        if (pixelsHu[i, j] > 76 && pixelsHu[i, j] < 200)
                            tejido.SetPixel(i, j, Color.GhostWhite);
                        else
                            tejido.SetPixel(i, j, Color.Black);                
            }
            else if(tej.Equals("Sangre Coagulada"))//Ventana para sangre cuagulada
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++) 
                        if (pixelsHu[i, j] > 56 && pixelsHu[i, j] < 76)
                            tejido.SetPixel(i, j, Color.Red);
                        else
                            tejido.SetPixel(i, j, Color.Black);
            }               
            else if(tej.Equals("Materia Cerebral Gris"))//Ventana para sustancia cerebral Gris
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++) 
                        if (pixelsHu[i, j] > 36 && pixelsHu[i, j] < 46)
                            tejido.SetPixel(i, j, Color.Silver);
                        else
                            tejido.SetPixel(i, j, Color.Black);
            }
            else if(tej.Equals("Materia Cerebral Blanca"))//Ventana para sustancia cerebral Blanca
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++) 
                        if (pixelsHu[i, j] > 22 && pixelsHu[i, j] < 36)
                            tejido.SetPixel(i, j, Color.Silver);
                        else
                            tejido.SetPixel(i, j, Color.Black);
            }
            else if(tej.Equals("Liquido"))//Ventana para liquido
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++) 
                        if (pixelsHu[i, j] > -10 && pixelsHu[i, j] < 22)
                            tejido.SetPixel(i, j, Color.Blue);
                        else if (pixelsHu[i, j] == -2000)
                            tejido.SetPixel(i, j, Color.White);
                        else
                            tejido.SetPixel(i, j, Color.Black);
            }
            else if(tej.Equals("Grasa"))//Ventana para Grasa
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++) 
                        if (pixelsHu[i, j] > -800 && pixelsHu[i, j] < -80)
                            tejido.SetPixel(i, j, Color.Yellow);
                        else
                            tejido.SetPixel(i, j, Color.Black);
            }
            else if (tej.Equals("Todo"))
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        if (pixelsHu[i, j] > 200 && pixelsHu[i, j] < 1100) //Hueso Compacto
                            tejido.SetPixel(i, j, Color.White);
                        else if (pixelsHu[i, j] > 22 && pixelsHu[i, j] < 36) //M C G y B
                            tejido.SetPixel(i, j, Color.Silver);
                        else if (pixelsHu[i, j] > 76 && pixelsHu[i, j] < 200) //Calcificaciones
                            tejido.SetPixel(i, j, Color.GhostWhite);
                        else if (pixelsHu[i, j] > 56 && pixelsHu[i, j] < 76) //Sangre
                            tejido.SetPixel(i, j, Color.Red);
                        else if (pixelsHu[i, j] > -10 && pixelsHu[i, j] < 22) //Liquido
                            tejido.SetPixel(i, j, Color.Blue);
                        else if (pixelsHu[i, j] > -800 && pixelsHu[i, j] < -80) //Grasa
                            tejido.SetPixel(i, j, Color.Yellow);
                        else
                            tejido.SetPixel(i, j, Color.Black);

            }
            //tejido.RotateFlip(RotateFlipType.Rotate270FlipX);
            return tejido;
        }
    }
}
