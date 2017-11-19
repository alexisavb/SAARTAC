using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace MainFrame.Tecnicas_de_Reconstruccion
{
    class RayTracing
    {
        vtkImageData imgdata = null;
        vtkPlanes planes = null;
        vtkFixedPointVolumeRayCastMapper mapper = null;
        const int GRASA = 0;
        const int HUESO_COMPACTO = 1;
        const int CALCIFICACIONES = 2;
        const int LIQUIDO = 3;
        const int SANGRE = 4;
        const int MATERIA_GRIS = 5;
        const int MATERIA_BLANCA = 6;
        const int ALL = 7;
        int option;

        public RayTracing(vtkImageData data,int op)
        {
            imgdata = data;
            option = op;
            planes = vtkPlanes.New();
            mapper = vtkFixedPointVolumeRayCastMapper.New(); 
        }

        public int getOption()
        {
            return option;
        }

        public void setPlanes(vtkPlanes pl)
        {
            planes = pl;
        }

        public vtkFixedPointVolumeRayCastMapper getMapper()
        {
            return mapper;
        }

        public vtkPiecewiseFunction getOpacity()
        {
            vtkPiecewiseFunction opacityTransferFunction = vtkPiecewiseFunction.New();
            if(option == ALL)
            {
                opacityTransferFunction.AddPoint(-2000, 0.0);
                opacityTransferFunction.AddPoint(-1000, 0.0);
                opacityTransferFunction.AddPoint(-800, 0.0);
                opacityTransferFunction.AddPoint(-80, 1.0);
                opacityTransferFunction.AddPoint(-10, 1.0);
                opacityTransferFunction.AddPoint(22, 1.0);
                opacityTransferFunction.AddPoint(46, 1.0);
                opacityTransferFunction.AddPoint(56, 1.0);
                opacityTransferFunction.AddPoint(76, 1.0);
                opacityTransferFunction.AddPoint(200, 1.0);
                opacityTransferFunction.AddPoint(1000, 0.0);
                opacityTransferFunction.AddPoint(2000, 0.0);
                opacityTransferFunction.Update();
            }
            else if (option == HUESO_COMPACTO)
            {
                opacityTransferFunction.AddPoint(-2000, 0.0);
                opacityTransferFunction.AddPoint(-1000, 0.0);
                opacityTransferFunction.AddPoint(-800, 0.0);
                opacityTransferFunction.AddPoint(-80, 0.0);
                opacityTransferFunction.AddPoint(-10, 0.0);
                opacityTransferFunction.AddPoint(22, 0.0);
                opacityTransferFunction.AddPoint(46, 0.0);
                opacityTransferFunction.AddPoint(56, 0.0);
                opacityTransferFunction.AddPoint(76, 0.0);
                opacityTransferFunction.AddPoint(200, 1.0);
                opacityTransferFunction.AddPoint(1500, 0.0);
                opacityTransferFunction.AddPoint(2000, 0.0);
                opacityTransferFunction.Update();
            }
            else if (option == GRASA)
            {

                opacityTransferFunction.AddPoint(-2000, 0.0);
                opacityTransferFunction.AddPoint(-1000, 0.0);
                opacityTransferFunction.AddPoint(-800, 0.0);
                opacityTransferFunction.AddPoint(-80, 1.0);
                opacityTransferFunction.AddPoint(-10, 0.0);
                opacityTransferFunction.AddPoint(22, 0.0);
                opacityTransferFunction.AddPoint(46, 0.0);
                opacityTransferFunction.AddPoint(56, 0.0);
                opacityTransferFunction.AddPoint(76, 0.0);
                opacityTransferFunction.AddPoint(200, 0.0);
                opacityTransferFunction.AddPoint(1000, 0.0);
                opacityTransferFunction.AddPoint(2000, 0.0);
                opacityTransferFunction.Update();
            }
            else if (option == LIQUIDO)
            {
                opacityTransferFunction.AddPoint(-2000, 0.0);
                opacityTransferFunction.AddPoint(-1000, 0.0);
                opacityTransferFunction.AddPoint(-800, 0.0);
                opacityTransferFunction.AddPoint(-80, 0.0);
                opacityTransferFunction.AddPoint(-10, 0.0);
                opacityTransferFunction.AddPoint(0, 1.0);
                opacityTransferFunction.AddPoint(22, 0.0);
                opacityTransferFunction.AddPoint(46, 0.0);
                opacityTransferFunction.AddPoint(56, 0.0);
                opacityTransferFunction.AddPoint(76, 0.0);
                opacityTransferFunction.AddPoint(200, 0.0);
                opacityTransferFunction.AddPoint(1000, 0.0);
                opacityTransferFunction.AddPoint(2000, 0.0);
                opacityTransferFunction.Update();
            }
            else if (option == SANGRE)
            {
                opacityTransferFunction.AddPoint(-2000, 0.0);
                opacityTransferFunction.AddPoint(-1000, 0.0);
                opacityTransferFunction.AddPoint(-800, 0.0);
                opacityTransferFunction.AddPoint(-80, 0.0);
                opacityTransferFunction.AddPoint(-10, 0.0);
                opacityTransferFunction.AddPoint(22, 0.0);
                opacityTransferFunction.AddPoint(46, 0.0);
                opacityTransferFunction.AddPoint(56, 1.0);
                opacityTransferFunction.AddPoint(76, 0.0);
                opacityTransferFunction.AddPoint(200, 0.0);
                opacityTransferFunction.AddPoint(1000, 0.0);
                opacityTransferFunction.AddPoint(2000, 0.0);
                opacityTransferFunction.Update();
            }
            else if (option == CALCIFICACIONES)
            {
                opacityTransferFunction.AddPoint(-2000, 0.0);
                opacityTransferFunction.AddPoint(-1000, 0.0);
                opacityTransferFunction.AddPoint(-800, 0.0);
                opacityTransferFunction.AddPoint(-80, 0.0);
                opacityTransferFunction.AddPoint(-10, 0.0);
                opacityTransferFunction.AddPoint(22, 0.0);
                opacityTransferFunction.AddPoint(46, 0.0);
                opacityTransferFunction.AddPoint(56, 0.0);
                opacityTransferFunction.AddPoint(76, 1.0);
                opacityTransferFunction.AddPoint(200, 0.0);
                opacityTransferFunction.AddPoint(1000, 0.0);
                opacityTransferFunction.AddPoint(2000, 0.0);
                opacityTransferFunction.Update();
            }
            else if (option == MATERIA_BLANCA)
            {
                opacityTransferFunction.AddPoint(-2000, 0.0);
                opacityTransferFunction.AddPoint(-1000, 0.0);
                opacityTransferFunction.AddPoint(-800, 0.0);
                opacityTransferFunction.AddPoint(-80, 0.0);
                opacityTransferFunction.AddPoint(-10, 0.0);
                opacityTransferFunction.AddPoint(22, 1.0);
                opacityTransferFunction.AddPoint(36, 0.0);
                opacityTransferFunction.AddPoint(46, 0.0);
                opacityTransferFunction.AddPoint(56, 0.0);
                opacityTransferFunction.AddPoint(76, 0.0);
                opacityTransferFunction.AddPoint(200, 0.0);
                opacityTransferFunction.AddPoint(1000, 0.0);
                opacityTransferFunction.AddPoint(2000, 0.0);
                opacityTransferFunction.Update();
            }
            else if (option == MATERIA_GRIS)
            {
                opacityTransferFunction.AddPoint(-2000, 0.0);
                opacityTransferFunction.AddPoint(-1000, 0.0);
                opacityTransferFunction.AddPoint(-800, 0.0);
                opacityTransferFunction.AddPoint(-80, 0.0);
                opacityTransferFunction.AddPoint(-10, 0.0);
                opacityTransferFunction.AddPoint(22, 0.0);
                opacityTransferFunction.AddPoint(36, 1.0);
                opacityTransferFunction.AddPoint(46, 0.0);
                opacityTransferFunction.AddPoint(56, 0.0);
                opacityTransferFunction.AddPoint(76, 0.0);
                opacityTransferFunction.AddPoint(200, 0.0);
                opacityTransferFunction.AddPoint(1000, 0.0);
                opacityTransferFunction.AddPoint(2000, 0.0);
                opacityTransferFunction.Update();
            }
            
            return opacityTransferFunction;
        }

        public vtkColorTransferFunction getColor()
        {            
            vtkColorTransferFunction colorTransferFunction = vtkColorTransferFunction.New();
            colorTransferFunction.AddRGBPoint(-2000, 0.0, 0.0, 0.0, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(-1000, 0.0, 0.0, 0.0, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(-800, 1.0, 0.6, 0.4, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(-80, 1.0, 0.6, 0.4, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(-10, 0.0, 0.7215, 0.9607, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(0, 0.0, 0.7215, 0.9607, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(22, 0.7607, 0.7607, 0.7607, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(36, 0.7607, 0.7607, 0.7607, .5, 0.0);
            colorTransferFunction.AddRGBPoint(46, 0.7607, 0.7607, 0.7607, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(56, 0.6, 0.0, 0.0, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(76, 0.862745, 0.862745, 0.862745, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(200, 1.0, 1.0, 1.0, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(1000, 0.0, 0.0, 0.0, 0.5, 0.0);
            colorTransferFunction.AddRGBPoint(2000, 0.0, 0.0, 0.0, 0.5, 0.0);
            colorTransferFunction.Build();

            return colorTransferFunction;
        }

        public vtkVolume reconstructVolume()
        {
            vtkVolumeProperty volprop=vtkVolumeProperty.New();        
            volprop.SetColor(getColor());            
            volprop.SetScalarOpacity(getOpacity());
            volprop.ShadeOn();
            volprop.SetAmbient(0.4);
            volprop.SetDiffuse(0.6);
            volprop.SetSpecular(0.2);  
            volprop.SetInterpolationTypeToLinear();

            planes.SetBounds(0, 0, 0, 0, 0, 0);
           
            mapper.SetSampleDistance((float)0.25);
            mapper.CroppingOn();
            mapper.SetCroppingRegionFlagsToInvertedCross();
            mapper.SetInput(imgdata);
            mapper.SetClippingPlanes(planes);     
            mapper.Update();

            vtkVolume vol = vtkVolume.New();
            vol.SetMapper(mapper);
            vol.SetProperty(volprop);
            vol.Update();            

            return vol;
        }
    }
}
