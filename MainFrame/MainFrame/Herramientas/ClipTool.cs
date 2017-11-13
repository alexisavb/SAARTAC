using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace MainFrame.Herramientas
{
    /// <summary>
    /// Clase que implementa el widget <c>vtkBoxWidget</c> para emplearlo como caja de corte
    /// </summary>
    class ClipTool
    {
        vtkRenderer renderer = null;
        vtkRenderWindow renderWindow = null;
        vtkProp3D prop = null;
        vtkClipPolyData clippd = null;
        vtkFixedPointVolumeRayCastMapper mapper = null;
        vtkBoxWidget boxWidget = null;
        vtkRenderWindowInteractor iren = null;

        public ClipTool()
        {
            clippd = vtkClipPolyData.New();
            mapper = vtkFixedPointVolumeRayCastMapper.New();
            boxWidget = vtkBoxWidget.New();
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
        /// Especifica un objeto de la clase <c>vtkProp3D</c> al que se le realizaran los cortes
        /// </summary>
        /// <param name="prp"></param>
        public void setProp3D(vtkProp3D prp)
        {
            prop = prp;
        }


        /// <summary>
        /// Indica si el widget debe estar prendido o apagado en el entorno 3D
        /// </summary>
        /// <param name="op">Si op es true, el widget se prenderá, en caso contrario se apagará</param>
        public void setClipOnOff(bool op)
        {
            if (op)
            {
                boxWidget.On();
                renderer.SetBackground(0.0, 0.3019, 0.6);
            }
            else
                boxWidget.Off();
        }

        /// <summary>
        /// Especifica un objeto de la clase <c>vtkClipPolyData</c> para realizar las funciones de 
        /// cortado, debe especificarse solo cuando el objeto fue reconstruido mediante <see cref="MarchingCubes"/>
        /// </summary>
        /// <param name="cpd">Función para cortar</param>
        public void setClipPD(vtkClipPolyData cpd)
        {
            clippd = cpd;
        }

        /// <summary>
        /// Especifica un objeto de la clase <c>vtkFixedPointVolumeRayCastMapper</c> para realizar las funciones de
        /// cortado, debe especificarse solo cunado el objeto fue reconstruido mediante <see cref="RayTracing"/>
        /// </summary>
        /// <param name="m">Mapper para cortar</param>
        public void setMapper(vtkFixedPointVolumeRayCastMapper m)
        {
            mapper = m;
        }

        /// <summary>
        /// Define si los cortes se haran desde adentro (contenido dentro de la caja) o desde afuera 
        /// (contenido fuera de la caja), si el objeto fue reconstruido con <see cref="MarchingBubes"/>
        /// </summary>
        /// <param name="op"></param>
        public void setInside(bool op)
        {
            if (op)
                boxWidget.InsideOutOn();
            else
                boxWidget.InsideOutOff();
        }
        
        /// <summary>
        /// Agrega el widget de medición al entorno 3D, previamente se debio agregar un 
        /// <c>vtkRenderWindowInteractor</c> con el método <see cref="setRenderWindowInteractor"/>,
        /// <c>vtkRenderer</c> usando <see cref="setRenderer"/> y un
        /// <c>vtkRenderWindow</c> con <see cref="setRenderWindow"/>.
        /// </summary>
        public void addClippingBox()
        {
            renderer.SetBackground(0.329412, 0.34902, 0.427451);
            
            vtkInteractorStyleTrackballCamera style = vtkInteractorStyleTrackballCamera.New();
            iren.SetInteractorStyle(style);

            boxWidget.SetPriority(1);            
            boxWidget.SetInteractor(iren);
            boxWidget.GetHandleProperty().SetOpacity(0.3);
            boxWidget.SetPlaceFactor(1.25);
            boxWidget.SetProp3D(prop);
            boxWidget.PlaceWidget();
            boxWidget.InteractionEvt += new vtkObject.vtkObjectEventHandler(boxWidget_InteractionEvt);
            boxWidget.Off();
        }

        /// <summary>
        /// Evento para cortar el modelo especificado con <c>setProp3D</c> en tiempo real
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void boxWidget_InteractionEvt(object sender, EventArgs e)
        {
            vtkPlanes p = vtkPlanes.New();
            vtkBoxWidget widget = (vtkBoxWidget)(sender);
            widget.GetPlanes(p);
            clippd.SetClipFunction(p);
            mapper.SetClippingPlanes(p);
        }
    }
}
