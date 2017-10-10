using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAARTAC1._1 {

    public class MyRenderer : ToolStripProfessionalRenderer {
        public MyRenderer() : base(new MyColors()) { }
        }

    public class MyColors : ProfessionalColorTable {
        public override Color MenuItemBorder {
            get { return Color.Black; }
        }
        public override Color MenuItemSelectedGradientBegin {
            get { return Color.Gray; }
        }
        public override Color MenuItemSelectedGradientEnd {
            get { return Color.Gray; }
        }
        public override Color MenuItemPressedGradientBegin {
            get { return Color.Black; }
        }
        public override Color MenuItemPressedGradientEnd {
            get { return Color.Black; }
        }
        public override Color MenuItemSelected {
            get { return Color.FromArgb(38, 38, 38); }
        }
        public override Color MenuBorder {
            get { return Color.Black; }
        }
        public override Color ToolStripDropDownBackground {
            get { return Color.Black; }
        }

        public override Color ToolStripBorder {
            get { return Color.Transparent; }
        }
        public override Color ToolStripGradientBegin {
            get { return Color.Transparent; }
        }

        public override Color ToolStripGradientMiddle {
            get { return Color.Transparent; }
        }

        public override Color ToolStripGradientEnd {
            get { return Color.Transparent; }
        }
    }
}