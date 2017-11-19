using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace MainFrame.Herramientas
{
    /// <summary>
    /// Clase para realizar cortes a dos objetos3D al mismo tiempo, de manera muy similar a <see cref="ClipTool"/>
    /// </summary>
    class MultiClipTool
    {
        vtkRenderer renderer = null;
        vtkRenderWindow renderWindow = null;
        vtkProp3D prop = null;
        vtkClipPolyData clipone = null;
        vtkClipPolyData cliptwo = null;
        vtkBoxWidget boxWidget = null;
        vtkRenderWindowInteractor iren = null;

        public MultiClipTool()
        {
            clipone = vtkClipPolyData.New();
            cliptwo = vtkClipPolyData.New();
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

        public void setClipOne(vtkClipPolyData cpd)
        {
            clipone = cpd;
        }

        public void setClipTwo(vtkClipPolyData cpd)
        {
            cliptwo = cpd;
        }

        public void setInside(bool op)
        {
            if (op)
                boxWidget.InsideOutOn();
            else
                boxWidget.InsideOutOff();
        }

        public void addClippingBox()
        {
            vtkInteractorStyleTrackballCamera style = vtkInteractorStyleTrackballCamera.New();
            iren.SetInteractorStyle(style);

            boxWidget.SetPriority(1);
            boxWidget.SetInteractor(iren);
            boxWidget.GetHandleProperty().SetOpacity(0.3);
            boxWidget.SetPlaceFactor(1.25);
            boxWidget.SetProp3D(prop);
            boxWidget.PlaceWidget();
            boxWidget.InteractionEvt += new vtkObject.vtkObjectEventHandler(boxWidget_InteractionEvt);
            //boxWidget.EndInteractionEvt +=new vtkObject.vtkObjectEventHandler(boxWidget_EndInteractionEvt);
            boxWidget.Off();
        }

        public void boxWidget_InteractionEvt(object sender, EventArgs e)
        {
            vtkPlanes p = vtkPlanes.New();
            vtkBoxWidget widget = (vtkBoxWidget)(sender);
            widget.GetPlanes(p);
            clipone.SetClipFunction(p);
            cliptwo.SetClipFunction(p);
        }
    }
}
