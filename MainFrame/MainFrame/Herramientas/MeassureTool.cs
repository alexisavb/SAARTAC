using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace MainFrame.Herramientas
{
    /// <summary>
    /// Clase que implementa el widget <c>vtkDistanceWidget</c> para medir la distancia de
    /// dos puntos.
    /// </summary>
    class MeassureTool
    {
        vtkRenderer renderer = null;
        vtkRenderWindow renderWindow = null;
        vtkRenderWindowInteractor iren = null;
        vtkDistanceWidget distance = null;

        public MeassureTool()
        {
            distance = vtkDistanceWidget.New();            
        }

        /// <summary>
        /// Indica si el widget debe estar prendido o apagado en el entorno 3D
        /// </summary>
        /// <param name="op">Si op es true, el widget se prenderá, en caso contrario se apagará</param>
        public void setMeassureOnOff(bool op)
        {
            if (op)
            {
                distance.On();
                renderer.SetBackground(0.0, 0.0, 0.0);
            }
            else
                distance.Off();
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
        
        /// <summary>
        /// Agrega el widget de medición al entorno 3D, previamente se debio agregar un 
        /// <c>vtkRenderWindowInteractor</c> con el método <see cref="setRenderWindowInteractor"/>,
        /// <c>vtkRenderer</c> usando <see cref="setRenderer"/> y un
        /// <c>vtkRenderWindow</c> con <see cref="setRenderWindow"/>.
        /// </summary>
        public void addMeassureTool()
        {
            vtkInteractorStyleTrackballCamera style = vtkInteractorStyleTrackballCamera.New();            
            iren.SetInteractorStyle(style);

            distance.SetPriority(8);
            distance.SetInteractor(iren);            
            distance.CreateDefaultRepresentation();
            vtkDistanceRepresentation rep = (vtkDistanceRepresentation)distance.GetRepresentation();
            rep.SetLabelFormat("%4.3f mm");
           
            distance.Off();
        }
    }
}
