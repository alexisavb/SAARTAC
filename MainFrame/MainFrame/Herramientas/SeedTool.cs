using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace MainFrame.Herramientas
{
    class SeedTool
    {
        vtkRenderer renderer = null;
        vtkRenderWindow renderWindow = null;
        vtkRenderWindowInteractor iren = null;
        vtkSeedWidget seed = null;

        public SeedTool()
        {
            seed = vtkSeedWidget.New();
        }

        /// <summary>
        /// Especifica un interactor para el widget
        /// </summary>
        /// <param name="rwi">Interactor</param>
        public void setInteractor(vtkRenderWindowInteractor rwi)
        {
            iren = rwi;
        }

        /// <summary>
        /// Especifica un renderer para el widget
        /// </summary>
        /// <param name="ren">Renderer</param>
        public void setRenderer(vtkRenderer ren)
        {
            renderer = ren;
        }

        /// <summary>
        /// Especifica un RenderWindow
        /// </summary>
        /// <param name="renwin">Render Window</param>
        public void setRenderWindow(vtkRenderWindow renwin)
        {
            renderWindow = renwin;
        }

        public void setSeedOnOff(bool op)
        {
            if (op)
            {
                seed.On();
            }
            else
                seed.Off();
        }

        public void addSeedTool()
        {
            vtkPointHandleRepresentation2D handle = vtkPointHandleRepresentation2D.New();
            handle.GetProperty().SetColor(1,0,0);
            handle.GetProperty().SetPointSize((float)10.0);
            handle.GetProperty().SetOpacity(0.8);            

            vtkSeedRepresentation rep = vtkSeedRepresentation.New();
            rep.SetHandleRepresentation(handle);
            
            seed.SetInteractor(iren);
            seed.SetRepresentation(rep);

            seed.Off();
        }
    }
}
