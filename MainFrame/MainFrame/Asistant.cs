using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainFrame
{
    public partial class Asistant : Form
    {
        Bitmap[] imagenes;
        int index = 0;
        bool inicioselected = false;
        bool finselected = false;
        bool isDrag = false;
        Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
        Point startPoint;
        Rectangle selection;
        int selectedindex = -1;
        int inicio, final;
        int xi = 0, yi = 0;
        int[] coord;
        String folderD;

        public Asistant(Bitmap[] imgs, String folder)
        {
            InitializeComponent();
            imagenes = imgs;
            folderD = folder;
            loadImgs();
            coord = new int[6];
            pictureBox1.Image = imagenes[index];
            pictureBox1.MouseMove += new MouseEventHandler(coordMove);
            pictureBox1.MouseWheel += new MouseEventHandler(cambiarImagen_MouseWheel);
        }
        
        private void cambiarImagen_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (e.Delta > 0 && index < imagenes.Length - 1)
                    index++;
                else if (e.Delta < 0 && index > 0)
                    index--;
                else
                    index = 0;
                pictureBox1.Image = imagenes[index];
            }
        }

        public void coordMove(object sender,
           MouseEventArgs e)
        {
            label8.Text = e.X + "";
            label9.Text = e.Y + "";
        }

        private void Form1_MouseDown(object sender,
            MouseEventArgs e)
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

        private void Form1_MouseMove(object sender,
            MouseEventArgs e)
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

        private void Form1_MouseUp(object sender,
            MouseEventArgs e)
        {

            // If the MouseUp event occurs, the user is not dragging.
            isDrag = false;

            // Draw the rectangle to be evaluated. Set a dashed frame style 
            // using the FrameStyle enumeration.
            ControlPaint.DrawReversibleFrame(theRectangle,
                this.BackColor, FrameStyle.Thick);

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
        }

        public void loadImgs()
        {
            for (int i = 0; i < imagenes.Length; i++)
            {
                Image aux = imagenes[i];
                this.imageList1.Images.Add(aux);
            }

            this.imageList1.ImageSize = new Size(100, 100);

            this.listView1.LargeImageList = this.imageList1;



            //this.listView1.View = View.SmallIcon;

            //this.listView1.SmallImageList = this.imageList1;

            for (int j = 0; j < this.imageList1.Images.Count; j++)
            {
                ListViewItem item = new ListViewItem();

                item.ImageIndex = j;

                this.listView1.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void seleccionarComoInicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inicio = selectedindex;
            textBox1.Text = "" + (inicio + 1);
            inicioselected = true;
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                selectedindex = e.ItemIndex;
                pictureBox1.Image = imagenes[selectedindex];
            }

        }

        private void seleccionarComoFinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            final = selectedindex;
            textBox2.Text = "" + (final + 1);
            finselected = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            coord[0] = xi;
            coord[1] = xi + selection.Width;
            coord[2] = yi;
            coord[3] = yi + selection.Height;
            coord[4] = inicio;
            coord[5] = final;
            this.Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }
    }
}
