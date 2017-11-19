using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kitware.VTK;

namespace MainFrame
{
    public partial class DicomIV : Form
    {

        vtkImageViewer2 ImageViewer;
        vtkTextMapper SliceStatusMapper;
        vtkTextMapper HuMapper;
        int Slice;
        int MinSlice;
        int MaxSlice;
        String dicomFolder;
        int cl, wl;
        int op = -1;
        Dicom dm;
        int uid;
        public int Uid
        {            
            get { return uid; }
            set { uid = value; }
        }

        public DicomIV(String folder,int id)
        {
            InitializeComponent();
            dicomFolder = folder;
            op = 0;
            Uid = id;
            dm = new Dicom(folder);
            this.Text = dm.getPatientName();     
        }

        private void renderWindowControl1_Load(object sender, EventArgs e)
        {
            try
            {
                ReadDICOMSeries();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK);
            }
        }

        public int getSlices()
        {
            return MaxSlice;
        }
        
        private void ReadDICOMSeries()
        {
            vtkDICOMImageReader reader = vtkDICOMImageReader.New();
            if (op == 0)
            {
                reader.SetDirectoryName(dicomFolder);
            }
            else if (op == 1)
            {                
                reader.SetFileName("C:\\Users\\Hugol\\Documents\\2013\\TT2\\ANGELES_CUERPO_COMPLETO_3\\00010288");
                //reader.SetFileName(filenames.GetValue(0));
                //reader.SetFileNames(filenames);
                Console.WriteLine("op 1");
            }
            reader.Update();
            
            // Visualize
            ImageViewer = vtkImageViewer2.New();
            ImageViewer.SetInputConnection(reader.GetOutputPort());
            
            // get range of slices (min is the first index, max is the last index)
            ImageViewer.GetSliceRange(ref MinSlice, ref MaxSlice);
            Console.WriteLine("slices range from : " + MinSlice.ToString() + " to " + MaxSlice.ToString());

            //Hounsfield Text
            vtkTextProperty patient = vtkTextProperty.New();
            patient.SetFontFamilyToCourier();
            patient.SetFontSize(14);
            patient.SetVerticalJustificationToTop();
            patient.SetJustificationToLeft();

            HuMapper = vtkTextMapper.New();
            HuMapper.SetInput(dm.getPatientName());
            HuMapper.SetTextProperty(patient);

            vtkActor2D huActor = vtkActor2D.New();
            huActor.SetMapper(HuMapper);
            huActor.SetPosition(15, 40);

            // slice status message
            vtkTextProperty sliceTextProp = vtkTextProperty.New();
            sliceTextProp.SetFontFamilyToCourier();
            sliceTextProp.SetFontSize(20);
            sliceTextProp.SetVerticalJustificationToBottom();
            sliceTextProp.SetJustificationToLeft();

            SliceStatusMapper = vtkTextMapper.New();
            SliceStatusMapper.SetInput("Imagen No " + (Slice + 1).ToString() + "/" + (MaxSlice + 1).ToString());
            SliceStatusMapper.SetTextProperty(sliceTextProp);

            vtkActor2D sliceStatusActor = vtkActor2D.New();
            sliceStatusActor.SetMapper(SliceStatusMapper);
            sliceStatusActor.SetPosition(15, 10);

            // usage hint message
            vtkTextProperty usageTextProp = vtkTextProperty.New();
            usageTextProp.SetFontFamilyToCourier();
            usageTextProp.SetFontSize(14);
            usageTextProp.SetVerticalJustificationToTop();
            usageTextProp.SetJustificationToLeft();

            vtkTextMapper usageTextMapper = vtkTextMapper.New();
            usageTextMapper.SetInput("Cambiar de Imagen\nTeclas-Up/Down\nZoom\nScroll del mouse");
            usageTextMapper.SetTextProperty(usageTextProp);

            vtkActor2D usageTextActor = vtkActor2D.New();
            usageTextActor.SetMapper(usageTextMapper);
            usageTextActor.GetPositionCoordinate().SetCoordinateSystemToNormalizedDisplay();
            usageTextActor.GetPositionCoordinate().SetValue(0.05, 0.95);

            vtkRenderWindow renderWindow = renderWindowControl1.RenderWindow;

            vtkInteractorStyleImage interactorStyle = vtkInteractorStyleImage.New();            

            renderWindow.GetInteractor().SetInteractorStyle(interactorStyle);                        
            renderWindow.GetRenderers().InitTraversal();              
            vtkRenderer ren;
            while ((ren = renderWindow.GetRenderers().GetNextItem()) != null)
                ren.SetBackground(0.0, 0.0, 0.0);            

            ImageViewer.SetRenderWindow(renderWindow);
            ImageViewer.GetRenderer().AddActor2D(sliceStatusActor);
            ImageViewer.GetRenderer().AddActor2D(usageTextActor);
            ImageViewer.GetRenderer().AddActor2D(huActor);
            ImageViewer.SetSlice(MinSlice);                       
            cl = trackBar1.Value=(int)ImageViewer.GetColorLevel();
            wl = trackBar2.Value = (int)ImageViewer.GetColorWindow();
            label1.Text = "" + cl;
            label2.Text = "" + wl;
            ImageViewer.Render();
        }

        /// <summary>
        /// move forward to next slice
        /// </summary>
        private void MoveForwardSlice()
        {
            Console.WriteLine(Slice.ToString());
            if (Slice < MaxSlice)
            {
                Slice += 1;
                ImageViewer.SetSlice(Slice);                 
                SliceStatusMapper.SetInput("Imagen No " + (Slice + 1).ToString() + "/" + (MaxSlice + 1).ToString());
                ImageViewer.Render();
            }
        }


        /// <summary>
        /// move backward to next slice
        /// </summary>
        private void MoveBackwardSlice()
        {
            Console.WriteLine(Slice.ToString());
            if (Slice > MinSlice)
            {
                Slice -= 1;
                ImageViewer.SetSlice(Slice);
                SliceStatusMapper.SetInput("Imagen No " + (Slice + 1).ToString() + "/" + (MaxSlice + 1).ToString());
                ImageViewer.Render();
            }
        }


        /// <summary>
        /// eventhanndler to process keyboard input
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            //Debug.WriteLine(DateTime.Now + ":" + msg.Msg + ", " + keyData);
            if (keyData == System.Windows.Forms.Keys.Up)
            {
                MoveForwardSlice();
                return true;
            }
            else if (keyData == System.Windows.Forms.Keys.Down)
            {
                MoveBackwardSlice();
                return true;
            }
            // don't forward the following keys
            // add all keys which are not supposed to get forwarded
            else if (
                  keyData == System.Windows.Forms.Keys.F
               || keyData == System.Windows.Forms.Keys.L
            )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// event handler for mousewheel forward event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void interactor_MouseWheelForwardEvt(vtkObject sender, vtkObjectEventArgs e)
        {
            MoveForwardSlice();
        }


        /// <summary>
        /// event handler for mousewheel backward event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void interactor_MouseWheelBackwardEvt(vtkObject sender, vtkObjectEventArgs e)
        {
            MoveBackwardSlice();
        }
        
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ImageViewer.SetColorLevel(trackBar1.Value);
            label1.Text = "" + ImageViewer.GetColorLevel();
            ImageViewer.Render();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ImageViewer.SetColorWindow(trackBar2.Value);
            label2.Text = "" + ImageViewer.GetColorWindow();
            ImageViewer.Render();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImageViewer.SetColorLevel(cl);
            ImageViewer.SetColorWindow(wl);
            trackBar1.Value = cl;
            trackBar2.Value = wl; 
            label1.Text = "" + cl;
            label2.Text = "" + wl;
            ImageViewer.Render();
        }

    }
}
