using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kitware.VTK;

namespace MainFrame.Clasificar
{
    public partial class ExtraerVOI : Form
    {
        Bitmap[] imgs;
        Dicom dcm;
        int index = 0;
        bool isDrag = false;
        int tecnica = 0;
        Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
        Point startPoint;
        Rectangle selection;
        int inicio=0, final=0;
        int[] VOI;
        int xi = 0, yi = 0;
        String tejido;

        public ExtraerVOI(Bitmap[] i, Dicom d, String tej)
        {
            InitializeComponent();
            imgs = i;
            rotateImgs();
            dcm = d;
            VOI = new int[6];
            tejido = tej;
            pictureBox1.Image = imgs[index];
            pictureBox1.MouseMove +=new MouseEventHandler(coordMove);
        }

        private void rotateImgs()
        {
            for (int i = 0; i < imgs.Length; i++)
            {
                imgs[i].RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
        }

        private void cambiarImagen_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (e.Delta > 0 && index < imgs.Length - 1)
                    index++;
                else if (e.Delta < 0 && index > 0)
                    index--;
                else
                    index = 0;
                pictureBox1.Image = imgs[index];
                label6.Text = index + "";
            }
        }

        public void coordMove(object sender, MouseEventArgs e)
        {
            label3.Text = e.X + "";
            label4.Text = e.Y + "";
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            // Set the isDrag variable to true and get the starting point 
            // by using the PointToScreen method to convert form 
            // coordinates to screen coordinates.
            if (e.Button == MouseButtons.Left)
            {
                isDrag = true;
            }

            Control control = (Control)sender;

            // Calculate the startPoint by using the PointToScreen 
            // method.
            startPoint = control.PointToScreen(new Point(e.X, e.Y));
            xi = e.X;
            yi = e.Y;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

            // If the mouse is being dragged, 
            // undraw and redraw the rectangle as the mouse moves.
            if (isDrag)

            // Hide the previous rectangle by calling the 
            // DrawReversibleFrame method with the same parameters.
            {
                ControlPaint.DrawReversibleFrame(theRectangle,
                    this.BackColor, FrameStyle.Thick);

                // Calculate the endpoint and dimensions for the new 
                // rectangle, again using the PointToScreen method.
                Point endPoint = ((Control)sender).PointToScreen(new Point(e.X, e.Y));

                int width = endPoint.X - startPoint.X;
                int height = endPoint.Y - startPoint.Y;
                theRectangle = new Rectangle(startPoint.X,
                    startPoint.Y, width, height);

                // Draw the new rectangle by calling DrawReversibleFrame
                // again.  
                selection = theRectangle;
                ControlPaint.DrawReversibleFrame(theRectangle,
                    this.BackColor, FrameStyle.Thick);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

            // If the MouseUp event occurs, the user is not dragging.
            isDrag = false;

            // Draw the rectangle to be evaluated. Set a dashed frame style 
            // using the FrameStyle enumeration.
            ControlPaint.DrawReversibleFrame(theRectangle, this.BackColor, FrameStyle.Thick);

            selection = theRectangle;
            // Find out which controls intersect the rectangle and 
            // change their color. The method uses the RectangleToScreen  
            // method to convert the Control's client coordinates 
            // to screen coordinates.
            Rectangle controlRectangle;
            for (int i = 0; i < Controls.Count; i++)
            {
                controlRectangle = Controls[i].RectangleToScreen
                    (Controls[i].ClientRectangle);
                if (controlRectangle.IntersectsWith(theRectangle))
                {
                    Controls[i].BackColor = Color.BurlyWood;
                }
            }

            // Reset the rectangle.
            theRectangle = new Rectangle(0, 0, 0, 0);
        }
        
        // Set up delegates for mouse events.
        protected override void OnLoad(System.EventArgs e)
        {
            pictureBox1.MouseDown += new MouseEventHandler(Form1_MouseDown);
            pictureBox1.MouseUp += new MouseEventHandler(Form1_MouseUp);
            pictureBox1.MouseMove += new MouseEventHandler(Form1_MouseMove);
            pictureBox1.MouseWheel +=new MouseEventHandler(cambiarImagen_MouseWheel);
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (rinicio.Checked)
            {
                pictureBox2.Image = pictureBox1.Image;
                textBox1.Text = index + "";
                inicio = index;
                rfin.Checked = true;
            }
            else if (rfin.Checked)
            {
                pictureBox3.Image = pictureBox1.Image;
                textBox2.Text = index + "";
                final = index;
            }
            else
                MessageBox.Show("Debe especificar un comienzo o fin");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inicio = 0;
            final = 0;
            textBox1.Text = "";
            textBox2.Text = "";
            rinicio.Checked = true;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            VOI[0] = VOI[1] = VOI[2] = VOI[3] = VOI[4] = VOI[5] = 0;
        }

        public void obtenerVOI()
        {
            VOI[0] = xi;
            VOI[1] = xi + selection.Width;
            VOI[2] = yi;
            VOI[3] = yi + selection.Height;
            VOI[4] = inicio;
            VOI[5] = final;
            vtkImageData imgdata = dcm.getImageData();
            vtkExtractVOI voi = vtkExtractVOI.New();
            voi.SetInput(imgdata);
            voi.SetVOI(VOI[0], VOI[1], VOI[2], VOI[3], VOI[4], VOI[5]);
            RenderMain rm = new RenderMain(voi.GetOutput(), tecnica, tejido);
            rm.Show();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            obtenerVOI();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox2.Image;
            index = Convert.ToInt32(textBox1.Text);
            label6.Text = textBox1.Text;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox3.Image;
            index = Convert.ToInt32(textBox2.Text);
            label6.Text = textBox2.Text;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                tecnica = 0;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                tecnica = 1;
            }
        }
    }
}
