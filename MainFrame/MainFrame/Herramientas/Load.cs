using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kitware.VTK;
using System.IO;
using System.ComponentModel;

namespace MainFrame.Herramientas
{
    class Load
    {
        vtkXMLImageDataReader reader = null;
        vtkDataObject dataobj = null;
        int tecnica = 0;
        int opcion = 0;

        public Load()
        {
            reader = vtkXMLImageDataReader.New();
        }

        public void setDataObject(vtkDataObject obj)
        {
            dataobj = obj;
        }

        public int getTecnica()
        {
            return tecnica;
        }

        public int getOpcion()
        {
            return opcion;
        }

        public void setTecnica(int t)
        {
            tecnica = t;
        }

        public void setOpcion(int o)
        {
            opcion = o;
        }

        public vtkImageData loadObject()
        {
            bool opened = false;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ArchivosVTI (*.vti)|*.vti";
            ofd.InitialDirectory = "C:\\Rec\\Saves";            
            DialogResult dr = ofd.ShowDialog();            
            if (dr.Equals(DialogResult.OK))
            {
                String nombre = ofd.FileName;
                String ext = Path.GetExtension(nombre);
                String path = Path.GetDirectoryName(nombre);
                try
                {
                    StreamReader sr = new StreamReader(path + "\\" + Path.GetFileNameWithoutExtension(nombre) + ".prop");
                    int tec = Convert.ToInt32(sr.ReadLine());
                    int op = Convert.ToInt32(sr.ReadLine());
                    setTecnica(tec);
                    setOpcion(op);
                    reader.SetFileName(nombre);
                    opened = true;
                }
                catch (Win32Exception e)
                {
                    MessageBox.Show("Error: " + e.Message);
                }
            }
            return reader.GetOutput();
        }
    }
}
