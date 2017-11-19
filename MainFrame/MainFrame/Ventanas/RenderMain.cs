using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kitware.VTK;
using MainFrame.Tecnicas_de_Reconstruccion;
using MainFrame.Herramientas;
using MainFrame.Clasificar;

namespace MainFrame
{
    public partial class RenderMain : Form
    {
        vtkDataObject obs = vtkDataObject.New();
        private String tejido = "";
        const int GRASA = 0;
        const int HUESO_COMPACTO = 1;
        const int CALCIFICACIONES = 2;
        const int LIQUIDO = 3;
        const int SANGRE = 4;
        const int MATERIA_GRIS = 5;
        const int MATERIA_BLANCA = 6;
        const int ALL = 7;
        int option;
        int optionC,tecnica;
        Color markColor = Color.FromArgb((int)(0.53 * 255), (int)(0.15 * 255), (int)(0.34 * 255));
        vtkImageData imgdata = vtkImageData.New();
        public static Dicom dicom;
        ClipTool cliptool = new ClipTool();
        MeassureTool meassuretool = new MeassureTool();
        ScaleTranslateTool scaletranslatetool = new ScaleTranslateTool();
        MultiClipTool multicliptool = new MultiClipTool();
        ContourTool contourtool = new ContourTool();
        SeedTool seedtool = new SeedTool();
        vtkRenderWindow rndwin = vtkRenderWindow.New();
        vtkRenderer rndr = vtkRenderer.New();


        public RenderMain(Dicom dcm, String tej,int op)
        {
            InitializeComponent();
            dicom = dcm;
            tejido = tej;
            option = op;
        }

        public RenderMain(Dicom dcm,int op)
        {
            InitializeComponent();
            dicom = dcm;
            option = op;
        }

        public RenderMain(vtkImageData img, int tec)
        {
            InitializeComponent();
            imgdata = img;
            tecnica = tec;
            option = 3;
        }

        public RenderMain(vtkImageData img)
        {
            InitializeComponent();
            imgdata = img;            
            option = 7;
        }       

        public RenderMain()
        {
            InitializeComponent();
            option = 3;
        }

        public RenderMain(vtkImageData img, int tec, String tej)
        {
            InitializeComponent();
            imgdata = img;
            tecnica = tec;
            tejido = tej;
            option = 0;
        }

        private void renderWindowControl1_Load(object sender, EventArgs e)
        {
            try
            {
                vtkFileOutputWindow ow = vtkFileOutputWindow.New();
                ow.SetFileName("error.log");
                vtkOutputWindow.SetInstance(ow);
                generarVolumen(option);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK);
            }
        }        

        public Color getColor()
        {
            Color col = Color.Black;
            if (tejido.Equals("Hueso Compacto"))
                col = Color.White;
            else if (tejido.Equals("Calcificaciones"))
                col = Color.GhostWhite;
            else if (tejido.Equals("Sangre"))
                col = Color.Red;
            else if (tejido.Equals("Materia Cerebral Gris") || tejido.Equals("Materia Cebral Blanca"))
                col = Color.Silver;
            else if (tejido.Equals("Liquido"))
                col = Color.Blue;
            else if (tejido.Equals("Grasa"))
                col = Color.Yellow;
            else
                col = Color.Black;
            return col;
        }

        public void setTejido(String tej)
        {
            tejido = tej;
        }

        public int getOpTejido()
        {
            int tej = ALL;
            if (tejido.Equals("Hueso Compacto"))
                tej = HUESO_COMPACTO;
            else if (tejido.Equals("Calcificaciones"))
                tej = CALCIFICACIONES;
            else if (tejido.Equals("Sangre Coagulada"))
                tej = SANGRE;
            else if (tejido.Equals("Materia Cerebral Gris"))
                tej = MATERIA_GRIS;
            else if (tejido.Equals("Materia Cerebral Blanca"))
                tej = MATERIA_BLANCA;
            else if (tejido.Equals("Liquido"))
                tej = LIQUIDO;
            else if (tejido.Equals("Grasa"))
                tej = GRASA;
            else if (tejido.Equals("Todo"))
                tej = ALL;
            return tej;
        }       

        public void VolumeVOI()
        {
            if (tecnica == 0)
            {
                SegmentarArreglo sa = new SegmentarArreglo(imgdata, getOpTejido());
                MarchingCubes mc = new MarchingCubes(sa.segmentData(), 200, getOpTejido());

                vtkActor ac = mc.reconstructSegmentedData();

                vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
                vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
                renderer.SetBackground(0.329412, 0.34902, 0.427451);

                obs = mc.getImageData();

                renderer.AddActor(ac);

                vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
                iren.SetRenderWindow(renderWindow);

                rndwin = renderWindow;

                // Add the actor to the scene
                renderer.AddActor(ac);

                cliptool.setRenderer(renderer);
                cliptool.setRenderWindow(renderWindow);
                cliptool.setInteractor(iren);
                cliptool.setProp3D(ac);
                cliptool.setInside(false);
                cliptool.setClipPD(mc.getClipPolyData());
                cliptool.setClipOnOff(false);
                cliptool.addClippingBox();

                meassuretool.setRenderer(renderer);
                meassuretool.setRenderWindow(renderWindow);
                meassuretool.setInteractor(iren);
                meassuretool.setMeassureOnOff(false);
                meassuretool.addMeassureTool();

                scaletranslatetool.setRenderer(renderer);
                scaletranslatetool.setRenderWindow(renderWindow);
                scaletranslatetool.setInteractor(iren);
                scaletranslatetool.setProp3D(ac);
                scaletranslatetool.setSTOnOff(false);
                scaletranslatetool.addScaleTransalateBox();

                contourtool.setRenderer(renderer);
                contourtool.setRenderWindow(renderWindow);
                contourtool.setInteractor(iren);
                contourtool.setActor(ac);
                contourtool.setSource(imgdata);
                contourtool.setContourOnOff(false);
                contourtool.addContourTool();

                seedtool.setRenderer(renderer);
                seedtool.setRenderWindow(renderWindow);
                seedtool.setInteractor(iren);
                seedtool.setSeedOnOff(false);
                seedtool.addSeedTool();
            }
            else if (tecnica == 1)
            {
                RayTracing rt = new RayTracing(imgdata, getOpTejido());

                vtkVolume vo = rt.reconstructVolume();

                vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
                vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
                renderer.SetBackground(0.329412, 0.34902, 0.427451);

                obs = rt.getMapper().GetInput();

                renderer.AddActor(vo);

                vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
                iren.SetRenderWindow(renderWindow);

                rndwin = renderWindow;

                cliptool.setRenderer(renderer);
                cliptool.setRenderWindow(renderWindow);
                cliptool.setInteractor(iren);
                cliptool.setProp3D(vo);
                cliptool.setInside(true);
                cliptool.setMapper(rt.getMapper());
                cliptool.setClipOnOff(false);
                cliptool.addClippingBox();

                meassuretool.setRenderer(renderer);
                meassuretool.setRenderWindow(renderWindow);
                meassuretool.setInteractor(iren);
                meassuretool.setMeassureOnOff(false);
                meassuretool.addMeassureTool();

                scaletranslatetool.setRenderer(renderer);
                scaletranslatetool.setRenderWindow(renderWindow);
                scaletranslatetool.setInteractor(iren);
                scaletranslatetool.setProp3D(vo);
                scaletranslatetool.setSTOnOff(false);
                scaletranslatetool.addScaleTransalateBox();

                seedtool.setRenderer(renderer);
                seedtool.setRenderWindow(renderWindow);
                seedtool.setInteractor(iren);
                seedtool.setSeedOnOff(false);
                seedtool.addSeedTool();
            }
        }

        public void customRender()
        {
            vtkImageData imgdata = dicom.getImageData();

            RayTracing rt = new RayTracing(imgdata, getOpTejido());
            vtkVolume vol = rt.reconstructVolume();
            optionC = rt.getOption();
            tecnica = 1;

            obs = rt.getMapper().GetInput();
                        
            // Create a renderer, render window, and interactor
            vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
            vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
            renderer.SetBackground(0.329412, 0.34902, 0.427451);

            vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
            iren.SetRenderWindow(renderWindow);

            rndwin = renderWindow;

            // Add the actor to the scene
            renderer.AddActor(vol);

            cliptool.setRenderer(renderer);
            cliptool.setRenderWindow(renderWindow);
            cliptool.setInteractor(iren);
            cliptool.setProp3D(vol);
            cliptool.setInside(true);
            cliptool.setMapper(rt.getMapper());
            cliptool.setClipOnOff(false);
            cliptool.addClippingBox();

            meassuretool.setRenderer(renderer);
            meassuretool.setRenderWindow(renderWindow);
            meassuretool.setInteractor(iren);
            meassuretool.setMeassureOnOff(false);
            meassuretool.addMeassureTool();

            scaletranslatetool.setRenderer(renderer);
            scaletranslatetool.setRenderWindow(renderWindow);
            scaletranslatetool.setInteractor(iren);
            scaletranslatetool.setProp3D(vol);
            scaletranslatetool.setSTOnOff(false);
            scaletranslatetool.addScaleTransalateBox();

            seedtool.setRenderer(renderer);
            seedtool.setRenderWindow(renderWindow);
            seedtool.setInteractor(iren);
            seedtool.setSeedOnOff(false);
            seedtool.addSeedTool();

            contorno.Enabled = false;
        }

        public void customMRender()
        {
            vtkImageData imgdata = dicom.getImageData();

            SegmentarArreglo seg = new SegmentarArreglo(imgdata, getOpTejido());

            MarchingCubes mc = new MarchingCubes(seg.segmentData(), 200,getOpTejido());
            vtkActor act = mc.reconstructSegmentedData();
            optionC = mc.getOption();
            tecnica = 0;

            obs = mc.getImageData();
            // Create a renderer, render window, and interactor
            vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
            vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
            renderer.SetBackground(0.329412, 0.34902, 0.427451);

            vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
            iren.SetRenderWindow(renderWindow);

            rndwin = renderWindow;
                 
            // Add the actor to the scene
            renderer.AddActor(act);

            cliptool.setRenderer(renderer);
            cliptool.setRenderWindow(renderWindow);
            cliptool.setInteractor(iren);
            cliptool.setProp3D(act);
            cliptool.setInside(false);
            cliptool.setClipPD(mc.getClipPolyData());
            cliptool.setClipOnOff(false);
            cliptool.addClippingBox();

            meassuretool.setRenderer(renderer);
            meassuretool.setRenderWindow(renderWindow);
            meassuretool.setInteractor(iren);
            meassuretool.setMeassureOnOff(false);
            meassuretool.addMeassureTool();

            scaletranslatetool.setRenderer(renderer);
            scaletranslatetool.setRenderWindow(renderWindow);
            scaletranslatetool.setInteractor(iren);
            scaletranslatetool.setProp3D(act);
            scaletranslatetool.setSTOnOff(false);
            scaletranslatetool.addScaleTransalateBox();
            
            contourtool.setRenderer(renderer);
            contourtool.setRenderWindow(renderWindow);
            contourtool.setInteractor(iren);
            contourtool.setActor(act);
            contourtool.setSource(imgdata);
            contourtool.setContourOnOff(false);
            contourtool.addContourTool();

            seedtool.setRenderer(renderer);
            seedtool.setRenderWindow(renderWindow);
            seedtool.setInteractor(iren);
            seedtool.setSeedOnOff(false);
            seedtool.addSeedTool();
        }

        public void skinAndBone()
        {
            vtkImageData imgdata = dicom.getImageData();

            MarchingCubes mc = new MarchingCubes(imgdata, 200, 0);
            vtkAssembly asmb = mc.getBoneAndSkin();

            // Create a renderer, render window, and interactor
            vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
            vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
            renderer.SetBackground(0.329412, 0.34902, 0.427451);

            vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
            iren.SetRenderWindow(renderWindow);

            rndwin = renderWindow;

            // Add the actor to the scene
            renderer.AddActor(asmb);

            multicliptool.setRenderer(renderer);
            multicliptool.setRenderWindow(renderWindow);
            multicliptool.setInteractor(iren);
            multicliptool.setProp3D(asmb);
            multicliptool.setInside(false);
            multicliptool.setClipOne(mc.getClipPolyData());
            multicliptool.setClipTwo(mc.getClipTwo());
            multicliptool.setClipOnOff(false);
            multicliptool.addClippingBox();

            meassuretool.setRenderer(renderer);
            meassuretool.setRenderWindow(renderWindow);
            meassuretool.setInteractor(iren);
            meassuretool.setMeassureOnOff(false);
            meassuretool.addMeassureTool();

            scaletranslatetool.setRenderer(renderer);
            scaletranslatetool.setRenderWindow(renderWindow);
            scaletranslatetool.setInteractor(iren);
            scaletranslatetool.setProp3D(asmb);
            scaletranslatetool.setSTOnOff(false);
            scaletranslatetool.addScaleTransalateBox();
            
            seedtool.setRenderer(renderer);
            seedtool.setRenderWindow(renderWindow);
            seedtool.setInteractor(iren);
            seedtool.setSeedOnOff(false);
            seedtool.addSeedTool();

            contorno.Enabled = false;
        }

        public void skin()
        {
            vtkImageData imgdata = dicom.getImageData();

            MarchingCubes mc = new MarchingCubes(imgdata, 200, 0);
            vtkActor asmb = mc.getSkin();

            // Create a renderer, render window, and interactor
            vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
            vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
            renderer.SetBackground(0.329412, 0.34902, 0.427451);

            vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
            iren.SetRenderWindow(renderWindow);

            rndwin = renderWindow;

            // Add the actor to the scene
            renderer.AddActor(asmb);

            cliptool.setRenderer(renderer);
            cliptool.setRenderWindow(renderWindow);
            cliptool.setInteractor(iren);
            cliptool.setProp3D(asmb);
            cliptool.setInside(false);
            cliptool.setClipPD(mc.getClipPolyData());
            cliptool.setClipOnOff(false);
            cliptool.addClippingBox();

            meassuretool.setRenderer(renderer);
            meassuretool.setRenderWindow(renderWindow);
            meassuretool.setInteractor(iren);
            meassuretool.setMeassureOnOff(false);
            meassuretool.addMeassureTool();

            scaletranslatetool.setRenderer(renderer);
            scaletranslatetool.setRenderWindow(renderWindow);
            scaletranslatetool.setInteractor(iren);
            scaletranslatetool.setProp3D(asmb);
            scaletranslatetool.setSTOnOff(false);
            scaletranslatetool.addScaleTransalateBox();

            contourtool.setRenderer(renderer);
            contourtool.setRenderWindow(renderWindow);
            contourtool.setInteractor(iren);
            contourtool.setActor(asmb);
            contourtool.setSource(imgdata);
            contourtool.setContourOnOff(false);
            contourtool.addContourTool();

            seedtool.setRenderer(renderer);
            seedtool.setRenderWindow(renderWindow);
            seedtool.setInteractor(iren);
            seedtool.setSeedOnOff(false);
            seedtool.addSeedTool();
        }

        public void bone()
        {
            vtkImageData imgdata = dicom.getImageData();

            MarchingCubes mc = new MarchingCubes(imgdata, 200, 0);
            vtkActor asmb = mc.getBone();

            // Create a renderer, render window, and interactor
            vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
            vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
            renderer.SetBackground(0.329412, 0.34902, 0.427451);

            vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
            iren.SetRenderWindow(renderWindow);

            rndwin = renderWindow;

            // Add the actor to the scene
            renderer.AddActor(asmb);

            cliptool.setRenderer(renderer);
            cliptool.setRenderWindow(renderWindow);
            cliptool.setInteractor(iren);
            cliptool.setProp3D(asmb);
            cliptool.setInside(false);
            cliptool.setClipPD(mc.getClipPolyData());
            cliptool.setClipOnOff(false);
            cliptool.addClippingBox();

            meassuretool.setRenderer(renderer);
            meassuretool.setRenderWindow(renderWindow);
            meassuretool.setInteractor(iren);
            meassuretool.setMeassureOnOff(false);
            meassuretool.addMeassureTool();

            scaletranslatetool.setRenderer(renderer);
            scaletranslatetool.setRenderWindow(renderWindow);
            scaletranslatetool.setInteractor(iren);
            scaletranslatetool.setProp3D(asmb);
            scaletranslatetool.setSTOnOff(false);
            scaletranslatetool.addScaleTransalateBox();

            contourtool.setRenderer(renderer);
            contourtool.setRenderWindow(renderWindow);
            contourtool.setInteractor(iren);
            contourtool.setActor(asmb);
            contourtool.setSource(imgdata);
            contourtool.setContourOnOff(false);
            contourtool.addContourTool();

            seedtool.setRenderer(renderer);
            seedtool.setRenderWindow(renderWindow);
            seedtool.setInteractor(iren);
            seedtool.setSeedOnOff(false);
            seedtool.addSeedTool();
        }

        public void loadedRender()
        {
            try
            {
                Load l = new Load();
                vtkImageData imgdata = l.loadObject();

                // Create a renderer, render window, and interactor
                vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
                vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
                renderer.SetBackground(0.329412, 0.34902, 0.427451);

                vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
                iren.SetRenderWindow(renderWindow);

                rndwin = renderWindow;

                if (l.getTecnica() == 1)
                {
                    RayTracing rt = new RayTracing(imgdata, l.getOpcion());
                    vtkVolume vol = rt.reconstructVolume();

                    renderer.AddActor(vol);
                    cliptool.setProp3D(vol);
                    cliptool.setMapper(rt.getMapper());
                    cliptool.setInside(true);
                    scaletranslatetool.setProp3D(vol);

                    cliptool.setRenderer(renderer);
                    cliptool.setRenderWindow(renderWindow);
                    cliptool.setInteractor(iren);
                    cliptool.setClipOnOff(false);
                    cliptool.addClippingBox();

                    meassuretool.setRenderer(renderer);
                    meassuretool.setRenderWindow(renderWindow);
                    meassuretool.setInteractor(iren);
                    meassuretool.setMeassureOnOff(false);
                    meassuretool.addMeassureTool();
                    
                    scaletranslatetool.setRenderer(renderer);
                    scaletranslatetool.setRenderWindow(renderWindow);
                    scaletranslatetool.setInteractor(iren);
                    scaletranslatetool.setSTOnOff(false);
                    scaletranslatetool.addScaleTransalateBox();

                    seedtool.setRenderer(renderer);
                    seedtool.setRenderWindow(renderWindow);
                    seedtool.setInteractor(iren);
                    seedtool.setSeedOnOff(false);
                    seedtool.addSeedTool();

                    contorno.Enabled = false;

                }
                else if (l.getTecnica() == 0)
                {
                    MarchingCubes mc = new MarchingCubes(imgdata, 200, l.getOpcion());
                    vtkActor ac = mc.reconstructSegmentedData();

                    renderer.AddActor(ac);
                    cliptool.setProp3D(ac);
                    cliptool.setClipPD(mc.getClipPolyData());
                    cliptool.setInside(false);
                    scaletranslatetool.setProp3D(ac);

                    cliptool.setRenderer(renderer);
                    cliptool.setRenderWindow(renderWindow);
                    cliptool.setInteractor(iren);
                    cliptool.setClipOnOff(false);
                    cliptool.addClippingBox();

                    meassuretool.setRenderer(renderer);
                    meassuretool.setRenderWindow(renderWindow);
                    meassuretool.setInteractor(iren);
                    meassuretool.setMeassureOnOff(false);
                    meassuretool.addMeassureTool();

                    scaletranslatetool.setRenderer(renderer);
                    scaletranslatetool.setRenderWindow(renderWindow);
                    scaletranslatetool.setInteractor(iren);
                    scaletranslatetool.setSTOnOff(false);
                    scaletranslatetool.addScaleTransalateBox();

                    contourtool.setRenderer(renderer);
                    contourtool.setRenderWindow(renderWindow);
                    contourtool.setInteractor(iren);
                    contourtool.setActor(ac);
                    contourtool.setSource(imgdata);
                    contourtool.setContourOnOff(false);
                    contourtool.addContourTool();

                    seedtool.setRenderer(renderer);
                    seedtool.setRenderWindow(renderWindow);
                    seedtool.setInteractor(iren);
                    seedtool.setSeedOnOff(false);
                    seedtool.addSeedTool();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }

        public void marchingandray()
        {
            SegmentarArreglo sr = new SegmentarArreglo(imgdata, GRASA);
            MarchingCubes mc = new MarchingCubes(sr.segmentData(), 200, GRASA);
            vtkActor piel = mc.reconstructSegmentedData();
            piel.GetProperty().SetOpacity(0.9);

            RayTracing rt = new RayTracing(imgdata, getOpTejido());
            vtkVolume sa = rt.reconstructVolume();

            vtkAssembly asmb = vtkAssembly.New();
            asmb.AddPart(piel);
            asmb.AddPart(sa);

            // Create a renderer, render window, and interactor
            vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
            vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
            renderer.SetBackground(0.329412, 0.34902, 0.427451);

            vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
            iren.SetRenderWindow(renderWindow);

            rndwin = renderWindow;

            renderer.AddActor(asmb);

            meassuretool.setRenderer(renderer);
            meassuretool.setRenderWindow(renderWindow);
            meassuretool.setInteractor(iren);
            meassuretool.setMeassureOnOff(false);
            meassuretool.addMeassureTool();

            seedtool.setRenderer(renderer);
            seedtool.setRenderWindow(renderWindow);
            seedtool.setInteractor(iren);
            seedtool.setSeedOnOff(false);
            seedtool.addSeedTool();

            contorno.Enabled = false;
        }

        public void generarVolumen(int op)
        {
            if (op == 0)
            {
                VolumeVOI();
            }
            else if (op == 1)
            {
                customRender();
            }
            else if (op == 2)
            {
                customMRender();
            }
            else if(op == 3)
            {
                loadedRender();
            }
            else if (op == 4)
            {
                skinAndBone();
            }

            else if (op == 5)
            {
                skin();
            }

            else if (op == 6)
            {
                bone();
            }
            else if (op == 7)
            {
                marchingandray();
            }
            
        }
      
        private void guarda_Click(object sender, EventArgs e)
        {
            Save s = new Save();
            s.setDataObject(obs);
            s.setOption(optionC);
            s.setTecnica(tecnica);
            s.saveObject();
        }

        private void medir_CheckedChanged(object sender, EventArgs e)
        {
            if (medir.Checked)
            {
                cortar.Checked = false;
                escalar.Checked = false;
                puntero.Checked = false;
                contorno.Checked = false;
                seed.Checked = false;
                meassuretool.setMeassureOnOff(true);
            }
            else
                meassuretool.setMeassureOnOff(false);
        }

        private void cortar_CheckedChanged(object sender, EventArgs e)
        {
            if (cortar.Checked)
            {
                medir.Checked = false;
                escalar.Checked = false;
                puntero.Checked = false;
                contorno.Checked = false;
                seed.Checked = false;
                if (option == 4)
                    multicliptool.setClipOnOff(true);
                else
                    cliptool.setClipOnOff(true);
            }
            else
            {
                if (option == 4)
                    multicliptool.setClipOnOff(false);
                else
                    cliptool.setClipOnOff(false);
            }
        }

        private void escalar_CheckedChanged(object sender, EventArgs e)
        {
            if (escalar.Checked)
            {
                medir.Checked = false;
                cortar.Checked = false;
                puntero.Checked = false;
                contorno.Checked = false;
                seed.Checked = false;
                scaletranslatetool.setSTOnOff(true);
            }
            else
                scaletranslatetool.setSTOnOff(false);
        }

        private void puntero_CheckedChanged(object sender, EventArgs e)
        {
            if (puntero.Checked)
            {
                medir.Checked = false;
                cortar.Checked = false;
                escalar.Checked = false;
                contorno.Checked = false;
                seed.Checked = false;
                meassuretool.setMeassureOnOff(false);
                if (option == 4)
                    multicliptool.setClipOnOff(false);
                else
                    cliptool.setClipOnOff(false);
                scaletranslatetool.setSTOnOff(false);
                contourtool.setContourOnOff(false);
                seedtool.setSeedOnOff(false);
            }
        }

        private void contorno_Click(object sender, EventArgs e)
        {
            if (contorno.Checked)
            {
                medir.Checked = false;
                cortar.Checked = false;
                puntero.Checked = false;
                escalar.Checked = false;
                seed.Checked = false;
                contourtool.setContourOnOff(true);
            }
            else
                contourtool.setContourOnOff(false);
        }

        private void seed_Click(object sender, EventArgs e)
        {
            if (seed.Checked)
            {
                medir.Checked = false;
                cortar.Checked = false;
                puntero.Checked = false;
                escalar.Checked = false;
                contorno.Checked = false;
                seedtool.setSeedOnOff(true);
            }
            else
                seedtool.setSeedOnOff(false);
        }

        private void imp_Click(object sender, EventArgs e)
        {
            SaveFileDialog svd = new SaveFileDialog();
            svd.DefaultExt = "png";
            svd.AddExtension = true;
            svd.Filter = "Archivos PNG (*.png)|.*png";          
            DialogResult dr = svd.ShowDialog();
            if (dr.Equals(DialogResult.OK))
            {
                vtkWindowToImageFilter windowToImageFilter = vtkWindowToImageFilter.New();
                windowToImageFilter.SetInput(rndwin);
                windowToImageFilter.ShouldRerenderOn();
                windowToImageFilter.SetMagnification(3); //set the resolution of the output image (3 times the current resolution of vtk render window)
                windowToImageFilter.SetInputBufferTypeToRGBA(); //also record the alpha (transparency) channel
                windowToImageFilter.Update();

                vtkPNGWriter writer = vtkPNGWriter.New();
                writer.SetFileName(svd.FileName);
                writer.SetInputConnection(windowToImageFilter.GetOutputPort());
                writer.Write();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            rndwin.GetRenderers().GetFirstRenderer().ResetCamera();
            rndwin.GetInteractor().Render();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            vtkPolyData aPlane = contourtool.getContour();
            vtkLookupTable lut = vtkLookupTable.New();
            lut.Build();
 
            // Fill in a few known colors, the rest will be generated if needed
            lut.SetTableValue(0, 0.5300, 0.1500, 0.3400, 1); // Raspberry
 
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInput(aPlane);
            mapper.SetLookupTable(lut);
 
            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor((markColor.R / 255), (markColor.G / 255), (markColor.B / 255));
            actor.GetProperty().SetLineWidth((float)4.0);
            actor.GetProperty().SetInterpolationToFlat();
            
            rndwin.GetRenderers().GetFirstRenderer().AddActor(actor);
 
            //vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;
            //vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
            //renderer.SetBackground(0.329412, 0.34902, 0.427451);
            
            //vtkRenderWindowInteractor iren = vtkRenderWindowInteractor.New();
            //iren.SetRenderWindow(renderWindow);

            //rndwin = renderWindow;
            //renderer.AddActor(actor);

        }
        
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
            markColor = cd.Color;
            colorb.BackColor = markColor;
        }

    }
}
