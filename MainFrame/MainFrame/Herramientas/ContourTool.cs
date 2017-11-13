using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace MainFrame.Herramientas
{
    class ContourTool
    {
        vtkRenderer renderer = null;
        vtkRenderWindow renderWindow = null;
        vtkRenderWindowInteractor iren = null;
        vtkContourWidget contour = null;
        vtkProp3D actor = null;
        vtkDataObject source = null;

        public ContourTool()
        {
            contour = vtkContourWidget.New();
        }

        public void setContourOnOff(bool op)
        {
            if (op)
            {
                contour.On();
            }
            else
                contour.Off();
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

        public void setActor(vtkProp3D ac)
        {
            actor = ac;
        }

        public void setSource(vtkDataObject obj)
        {
            source = obj;
        }

        public vtkPolyData getContour()
        {
            return contour.GetContourRepresentation().GetContourRepresentationAsPolyData();
        }

        public void addContourTool()
        {
            vtkInteractorStyleTrackballCamera style = vtkInteractorStyleTrackballCamera.New();
            iren.SetInteractorStyle(style);

            contour.SetInteractor(iren);

            vtkOrientedGlyphContourRepresentation rep = vtkOrientedGlyphContourRepresentation.New();
 
            vtkPolygonalSurfacePointPlacer pointPlacer = vtkPolygonalSurfacePointPlacer.New();
            pointPlacer.AddProp(actor);            
            pointPlacer.GetPolys().AddItem(source);
 
            rep.GetLinesProperty().SetColor(1, 0, 0);
            rep.GetLinesProperty().SetLineWidth((float)8.0);
            rep.SetPointPlacer(pointPlacer);
            
            contour.SetRepresentation(rep);
            contour.Off();
        }
    }
}
