using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace MainFrame.Clasificar
{
    class SegmentarArreglo
    {
        vtkImageData data = null;
        const int GRASA = 0;
        const int HUESO_COMPACTO = 1;
        const int CALCIFICACIONES = 2;
        const int LIQUIDO = 3;
        const int SANGRE = 4;
        const int MATERIA_GRIS = 5;
        const int MATERIA_BLANCA = 6;
        const int ALL = 7;
        int option;

        public SegmentarArreglo(vtkImageData imgd,int op)
        {
            data = imgd;
            option = op;
        }

        public vtkImageData segmentData()
        {
            vtkImageData aux = data;
            vtkImageShrink3D shrink = vtkImageShrink3D.New();
            shrink.SetInput(aux);
            shrink.SetShrinkFactors(1, 1, 1);
            shrink.Update();

            vtkImageThreshold thres = vtkImageThreshold.New();
            thres.SetInput(shrink.GetOutput());
            thres.SetOutValue(-1000);
            if (option == GRASA)
            {
                thres.SetInValue(200);
                thres.ThresholdByUpper(-80);
                thres.ThresholdByLower(-800);
                thres.Update();
            }
            else if (option == HUESO_COMPACTO)
            {
                thres.SetInValue(200);
                thres.ThresholdByUpper(300);
                thres.ThresholdByLower(200);
                thres.Update();
            }
            else if (option == LIQUIDO)
            {
                thres.SetInValue(200);
                thres.ThresholdByUpper(22);
                thres.ThresholdByLower(-10);
                thres.Update();
            }
            else if (option == SANGRE)
            {
                thres.SetInValue(200);
                thres.ThresholdByUpper(76);
                thres.ThresholdByLower(56);
                thres.Update();
            }
            else if (option == CALCIFICACIONES)
            {
                thres.SetInValue(200);
                thres.ThresholdByUpper(200);
                thres.ThresholdByLower(76);
                thres.Update();
            }
            else if (option == MATERIA_BLANCA)
            {
                thres.SetInValue(200);
                thres.ThresholdByUpper(36);
                thres.ThresholdByLower(22);
                thres.Update();
            }
            else if (option == MATERIA_GRIS)
            {
                thres.SetInValue(200);
                thres.ThresholdByUpper(46);
                thres.ThresholdByLower(36);
                thres.Update();
            }
            vtkImageGaussianSmooth smooth = vtkImageGaussianSmooth.New();
            smooth.SetInput(thres.GetOutput());
            smooth.SetStandardDeviations(1.4, 1.4, 1.0);
            smooth.Update();
            return smooth.GetOutput();
        }
    }
}
