using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace MainFrame.Tecnicas_de_Reconstruccion
{
    class MarchingCubes
    {
        vtkImageData imgdata = null;
        double isovalue;
        vtkMarchingCubes filter = null;
        bool smooth,reverse,windowed;
        vtkDataObject dataobj;
        vtkClipPolyData clippd = null;
        vtkClipPolyData cliptw = null;
        vtkPlanes planes = vtkPlanes.New();
        int option;
        const int GRASA = 0;
        const int HUESO_COMPACTO = 1;
        const int CALCIFICACIONES = 2;
        const int LIQUIDO = 3;
        const int SANGRE = 4;
        const int MATERIA_GRIS = 5;
        const int MATERIA_BLANCA = 6;
        const int ALL = 7;
        
        public MarchingCubes(vtkImageData data, double val,int op)
        {
            imgdata = data;
            isovalue = val;
            filter = vtkMarchingCubes.New();
            clippd = vtkClipPolyData.New();
            cliptw = vtkClipPolyData.New();
            smooth = true;
            reverse = true;
            windowed = true;
            option = op;
        }

        public MarchingCubes(vtkDataObject data, double val, int op)
        {
            imgdata = (vtkImageData)data;
            isovalue = val;
            filter = vtkMarchingCubes.New();
            clippd = vtkClipPolyData.New();
            cliptw = vtkClipPolyData.New();
            smooth = true;
            reverse = true;
            windowed = true;
            option = op;
        }

        public vtkImageData getImageData()
        {
            return imgdata;
        }

        public void setSmoothEnabled(bool set)
        {
            smooth = set;
        }

        public void setReverseEnabled(bool set)
        {
            reverse = set;
        }

        public void setWindowedEnabled(bool set)
        {
            windowed = set;
        }

        public vtkClipPolyData getClipPolyData()
        {
            return clippd;
        }

        public vtkClipPolyData getClipTwo()
        {
            return cliptw;
        }

        public int getOption()
        {
            return option;
        }

        public vtkAssembly getBoneAndSkin()
        {
            vtkMarchingCubes boneFilter = vtkMarchingCubes.New();
            boneFilter.SetInput(imgdata);
            boneFilter.SetValue(0, 200);
            boneFilter.ComputeGradientsOn();
            boneFilter.ComputeNormalsOn();
            boneFilter.ComputeScalarsOn();
            boneFilter.Update();

            /*vtkMarchingCubes skinFilter = vtkMarchingCubes.New();
            skinFilter.SetInput(imgdata);
            skinFilter.SetValue(0, -400);
            skinFilter.ComputeGradientsOn();
            skinFilter.ComputeNormalsOn();
            skinFilter.ComputeScalarsOn();
            skinFilter.Update();*/

            planes.SetBounds(0, 512, 0, 512, 0, 512);

            clippd.SetInput(boneFilter.GetOutput());
            clippd.SetClipFunction(planes);
            clippd.InsideOutOn();
            clippd.Update();

            /*cliptw.SetInput(skinFilter.GetOutput());
            cliptw.SetClipFunction(planes);
            cliptw.InsideOutOn();
            cliptw.Update();*/

            vtkPolyDataMapper boneMapper = vtkPolyDataMapper.New();
            boneMapper.SetInput(clippd.GetOutput());
            boneMapper.ScalarVisibilityOff();
            boneMapper.Update();

            /*vtkPolyDataMapper skinMapper = vtkPolyDataMapper.New();
            skinMapper.SetInput(cliptw.GetOutput());
            skinMapper.ScalarVisibilityOff();
            skinMapper.Update();*/

            vtkActor bone = vtkActor.New();
            bone.SetMapper(boneMapper);
            bone.GetProperty().SetColor(1.0, 1.0, 1.0);
            bone.GetProperty().SetOpacity(1);

            /*vtkActor skin = vtkActor.New();
            skin.SetMapper(skinMapper);
            skin.GetProperty().SetColor(1.0, 0.6, 0.4);
            skin.GetProperty().SetOpacity(.6);*/

            vtkAssembly assembly = vtkAssembly.New();
            assembly.AddPart(bone);
            //assembly.AddPart(skin);

            return assembly;

        }

        public vtkActor getSkin()
        {
            filter.SetInput(imgdata);
            filter.SetValue(0, -400);
            filter.ComputeGradientsOn();
            filter.ComputeNormalsOn();
            filter.ComputeScalarsOn();
            filter.Update();

            planes.SetBounds(0, 512, 0, 512, 0, 512);

            dataobj = filter.GetOutput();

            clippd.SetInput((vtkPolyData)dataobj);
            clippd.SetClipFunction(planes);
            clippd.InsideOutOn();
            clippd.Update();

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInput(clippd.GetOutput());
            mapper.ScalarVisibilityOff();
            mapper.Update();

            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(1.0, 0.6, 0.4);            
            actor.GetProperty().SetOpacity(1);

            return actor;
        }

        public vtkActor getBone()
        {
            filter.SetInput(imgdata);
            filter.SetValue(0, 200);
            filter.ComputeGradientsOn();
            filter.ComputeNormalsOn();
            filter.ComputeScalarsOn();
            filter.Update();

            planes.SetBounds(0, 512, 0, 512, 0, 512);

            dataobj = filter.GetOutput();

            clippd.SetInput((vtkPolyData)dataobj);
            clippd.SetClipFunction(planes);
            clippd.InsideOutOn();
            clippd.Update();

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInput(clippd.GetOutput());
            mapper.ScalarVisibilityOff();
            mapper.Update();

            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(1.0, 1.0, 1.0);
            actor.GetProperty().SetOpacity(1);

            return actor;
        }

        public vtkActor reconstructSegmentedData()
        {
            planes.SetBounds(0, 512, 0, 512, 0, 512);

            vtkDiscreteMarchingCubes dmc = vtkDiscreteMarchingCubes.New();
            dmc.SetInput(imgdata);
            dmc.ComputeScalarsOn();
            dmc.ComputeNormalsOn();
            dmc.ComputeGradientsOn();
            dmc.SetValue(0, 200);
            dmc.Update();

            vtkWindowedSincPolyDataFilter win = vtkWindowedSincPolyDataFilter.New();
            win.SetInput(dmc.GetOutput());          
            win.BoundarySmoothingOff();
            win.FeatureEdgeSmoothingOff();
            win.SetFeatureAngle(90.0);
            win.SetNumberOfIterations(15);
            win.SetPassBand(0.001);
            win.NonManifoldSmoothingOn();
            win.NormalizeCoordinatesOn();
            win.Update();

            clippd.SetInput(win.GetOutput());
            clippd.SetClipFunction(planes);
            clippd.InsideOutOn();
            clippd.Update();

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInput(clippd.GetOutput());
            mapper.ScalarVisibilityOff();
            mapper.Update();

            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            if (option == GRASA)
            {
                actor.GetProperty().SetColor(1.0, 0.6, 0.4);
            }
            else if (option == HUESO_COMPACTO)
            {
                actor.GetProperty().SetColor(1.0, 1.0, 1.0);
            }
            else if (option == CALCIFICACIONES)
            {
                actor.GetProperty().SetColor(0.862745, 0.862745, 0.862745);
            }
            else if (option == LIQUIDO)
            {
                actor.GetProperty().SetColor(0.0, 0.7215, 0.9607);
            }
            else if (option == SANGRE)
            {
                actor.GetProperty().SetColor(0.6, 0.0, 0.0);
            }
            else if (option == MATERIA_BLANCA)
            {
                actor.GetProperty().SetColor(0.7607, 0.7670, 0.7670);
            }
            else if (option == MATERIA_GRIS)
            {
                actor.GetProperty().SetColor(0.7607, 0.7670, 0.7670);
            }
            actor.GetProperty().SetOpacity(1);

            return actor;
        }

        public vtkActor reconstructVolume()
        {            
            filter.SetInput(imgdata);
            filter.SetValue(0, isovalue);
            filter.ComputeGradientsOn();
            filter.ComputeNormalsOn();
            filter.ComputeScalarsOn();
            filter.Update();

            planes.SetBounds(0, 512, 0, 512, 0, 512);
            
            dataobj = filter.GetOutput();

            if (reverse)
            {
                vtkReverseSense rev = vtkReverseSense.New();
                rev.SetInput(dataobj);
                rev.ReverseCellsOn();
                rev.ReverseNormalsOn();
                rev.Update();
                dataobj = rev.GetOutput();
            }
            if (smooth)
            {
                vtkSmoothPolyDataFilter sm = vtkSmoothPolyDataFilter.New();
                sm.SetInput(dataobj);
                sm.BoundarySmoothingOn();
                sm.FeatureEdgeSmoothingOn();
                sm.SetNumberOfIterations(100);
                sm.Update();
                dataobj = sm.GetOutput();
            }
            if (windowed)
            {
                vtkWindowedSincPolyDataFilter wpd = vtkWindowedSincPolyDataFilter.New();
                wpd.SetInput(dataobj);
                wpd.NormalizeCoordinatesOn();
                wpd.BoundarySmoothingOn();
                wpd.GenerateErrorVectorsOn();
                wpd.GenerateErrorScalarsOn();
                wpd.SetNumberOfIterations(10);
                wpd.SetPassBand(0.1);
                wpd.Update();
                dataobj = wpd.GetOutput();
            }

            clippd.SetInput((vtkPolyData)dataobj);
            clippd.SetClipFunction(planes);
            clippd.InsideOutOn();
            clippd.Update();
                       
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInput(clippd.GetOutput());
            mapper.ScalarVisibilityOff();
            mapper.Update();

            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            if (option == GRASA)
            {
                actor.GetProperty().SetColor(1.0, 0.6, 0.4);
            }
            else if (option == HUESO_COMPACTO)
            {
                actor.GetProperty().SetColor(1.0, 1.0, 1.0);
            }
            else if (option == CALCIFICACIONES)
            {
                actor.GetProperty().SetColor(0.862745, 0.862745, 0.862745);
            }
            else if (option == LIQUIDO)
            {
                actor.GetProperty().SetColor(0.0, 0.7215, 0.9607);
            }
            else if (option == SANGRE)
            {
                actor.GetProperty().SetColor(0.6, 0.0, 0.0);
            }
            else if (option == MATERIA_BLANCA)
            {
                actor.GetProperty().SetColor(0.7607, 0.7670, 0.7670);
            }
            else if (option == MATERIA_GRIS)
            {
                actor.GetProperty().SetColor(0.7607, 0.7670, 0.7670);
            }
            actor.GetProperty().SetOpacity(1);

            return actor;
        }            
    }
}
