using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;
using System.IO;
using System.Windows.Forms;

namespace MainFrame.Herramientas
{
    class Save
    {
        vtkXMLImageDataWriter writer = null;        
        vtkDataObject dataobj = null;
        int tecnica, option;

        public Save()
        {
            writer = vtkXMLImageDataWriter.New();            
        }

        public void setDataObject(vtkDataObject obj)
        {
            dataobj = obj;
        }

        public void setOption(int op)
        {
            option = op;
        }

        public void setTecnica(int tec)
        {
            tecnica = tec;
        }

        public bool saveObject()
        {
            vtkXMLWriter vt = vtkXMLImageDataWriter.New();
            bool saved = false;
            vt.SetInput(dataobj);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "vti";
            sfd.AddExtension = true;
            sfd.Filter = "Archivos VTI (*.vti)|.*vti";
            sfd.InitialDirectory = "C:\\Rec\\Saves";
            DialogResult dr = sfd.ShowDialog();
            if (dr.Equals(DialogResult.OK))
            {               
                String nombre = sfd.FileName;
                String path = Path.GetDirectoryName(nombre);
                StreamWriter sw = new StreamWriter(path + "\\" + Path.GetFileNameWithoutExtension(nombre) + ".prop");
                sw.WriteLine(tecnica);
                sw.WriteLine(option);
                sw.Close();
                vt.SetFileName(nombre);
                vt.Write();
                saved = true;
            }          
            return saved;
        }
    }
}
