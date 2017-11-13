using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace MainFrame.Herramientas
{
    /// <summary>
    /// Clase que implementa el widget <c>vtkBoxWidget</c> para escalar o trasladar el objeto en el entorno
    /// 3D
    /// </summary>
    class ScaleTranslateTool
    {
        vtkRenderer renderer = null;
        vtkRenderWindow renderWindow = null;
        vtkProp3D prop = null;
        vtkBoxWidget boxWidget = null;
        vtkRenderWindowInteractor iren = null;

        public ScaleTranslateTool()
        {
            boxWidget = vtkBoxWidget.New();
        }

        public void setInteractor(vtkRenderWindowInteractor rwi)
        {
            iren = rwi;
        }

        public void setRenderer(vtkRenderer ren)
        {
            renderer = ren;
        }

        public void setRenderWindow(vtkRenderWindow renwin)
        {
            renderWindow = renwin;
        }

        public void setProp3D(vtkProp3D prp)
        {
            prop = prp;
        }

        public void setSTOnOff(bool op)
        {
            if (op)
            {
                boxWidget.On();
                renderer.SetBackground(0.2, 0.4, 1.0);
            }
            else
                boxWidget.Off();
        }

        public void addScaleTransalateBox()
        {
            vtkInteractorStyleTrackballCamera style = vtkInteractorStyleTrackballCamera.New();
            iren.SetInteractorStyle(style);

            boxWidget.SetPriority(9);
            boxWidget.SetInteractor(iren);
            boxWidget.SetPlaceFactor(1.25);
            boxWidget.SetProp3D(prop);
            boxWidget.PlaceWidget();
            boxWidget.InsideOutOn();
            boxWidget.InteractionEvt += new vtkObject.vtkObjectEventHandler(boxWidget_InteractionEvt);
            boxWidget.Off();
        }

        public void boxWidget_InteractionEvt(object sender, EventArgs e)
        {
            vtkTransform t = vtkTransform.New();
            vtkBoxWidget widget = (vtkBoxWidget)(sender);
            widget.GetTransform(t);
            widget.GetProp3D().SetUserTransform(t);
        }
    }
}
