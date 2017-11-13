namespace MainFrame
{
    partial class RenderMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenderMain));
            this.renderWindowControl1 = new Kitware.VTK.RenderWindowControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.guarda = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.puntero = new System.Windows.Forms.ToolStripButton();
            this.medir = new System.Windows.Forms.ToolStripButton();
            this.cortar = new System.Windows.Forms.ToolStripButton();
            this.escalar = new System.Windows.Forms.ToolStripButton();
            this.contorno = new System.Windows.Forms.ToolStripButton();
            this.seed = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.imp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.colorb = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // renderWindowControl1
            // 
            this.renderWindowControl1.AddTestActors = false;
            this.renderWindowControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderWindowControl1.Location = new System.Drawing.Point(12, 27);
            this.renderWindowControl1.Name = "renderWindowControl1";
            this.renderWindowControl1.Size = new System.Drawing.Size(858, 494);
            this.renderWindowControl1.TabIndex = 2;
            this.renderWindowControl1.TestText = null;
            this.renderWindowControl1.Load += new System.EventHandler(this.renderWindowControl1_Load);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.guarda,
            this.toolStripSeparator1,
            this.puntero,
            this.medir,
            this.cortar,
            this.escalar,
            this.contorno,
            this.seed,
            this.toolStripSeparator2,
            this.imp,
            this.toolStripButton1,
            this.toolStripButton2,
            this.colorb});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(882, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // guarda
            // 
            this.guarda.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.guarda.Image = ((System.Drawing.Image)(resources.GetObject("guarda.Image")));
            this.guarda.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.guarda.Name = "guarda";
            this.guarda.Size = new System.Drawing.Size(23, 22);
            this.guarda.Text = "toolStripButton1";
            this.guarda.Visible = false;
            this.guarda.Click += new System.EventHandler(this.guarda_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // puntero
            // 
            this.puntero.Checked = true;
            this.puntero.CheckOnClick = true;
            this.puntero.CheckState = System.Windows.Forms.CheckState.Checked;
            this.puntero.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.puntero.Image = ((System.Drawing.Image)(resources.GetObject("puntero.Image")));
            this.puntero.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.puntero.Name = "puntero";
            this.puntero.Size = new System.Drawing.Size(23, 22);
            this.puntero.Text = "Puntero";
            this.puntero.Visible = false;
            this.puntero.CheckedChanged += new System.EventHandler(this.puntero_CheckedChanged);
            // 
            // medir
            // 
            this.medir.CheckOnClick = true;
            this.medir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.medir.Image = ((System.Drawing.Image)(resources.GetObject("medir.Image")));
            this.medir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.medir.Name = "medir";
            this.medir.Size = new System.Drawing.Size(23, 22);
            this.medir.Text = "Medir";
            this.medir.Visible = false;
            this.medir.CheckedChanged += new System.EventHandler(this.medir_CheckedChanged);
            // 
            // cortar
            // 
            this.cortar.CheckOnClick = true;
            this.cortar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cortar.Image = ((System.Drawing.Image)(resources.GetObject("cortar.Image")));
            this.cortar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cortar.Name = "cortar";
            this.cortar.Size = new System.Drawing.Size(23, 22);
            this.cortar.Text = "Cortar";
            this.cortar.Visible = false;
            this.cortar.CheckedChanged += new System.EventHandler(this.cortar_CheckedChanged);
            // 
            // escalar
            // 
            this.escalar.CheckOnClick = true;
            this.escalar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.escalar.Image = ((System.Drawing.Image)(resources.GetObject("escalar.Image")));
            this.escalar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.escalar.Name = "escalar";
            this.escalar.Size = new System.Drawing.Size(23, 22);
            this.escalar.Text = "Escalar/Mover";
            this.escalar.Visible = false;
            this.escalar.CheckedChanged += new System.EventHandler(this.escalar_CheckedChanged);
            // 
            // contorno
            // 
            this.contorno.CheckOnClick = true;
            this.contorno.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.contorno.Image = ((System.Drawing.Image)(resources.GetObject("contorno.Image")));
            this.contorno.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.contorno.Name = "contorno";
            this.contorno.Size = new System.Drawing.Size(23, 22);
            this.contorno.Text = "Delimitar Contorno";
            this.contorno.Visible = false;
            this.contorno.Click += new System.EventHandler(this.contorno_Click);
            // 
            // seed
            // 
            this.seed.CheckOnClick = true;
            this.seed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.seed.Image = ((System.Drawing.Image)(resources.GetObject("seed.Image")));
            this.seed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.seed.Name = "seed";
            this.seed.Size = new System.Drawing.Size(23, 22);
            this.seed.Text = "Marcar";
            this.seed.Visible = false;
            this.seed.Click += new System.EventHandler(this.seed_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // imp
            // 
            this.imp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.imp.Image = ((System.Drawing.Image)(resources.GetObject("imp.Image")));
            this.imp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imp.Name = "imp";
            this.imp.Size = new System.Drawing.Size(23, 22);
            this.imp.Text = "Capturar Pantalla";
            this.imp.ToolTipText = "Capturar Pantalla";
            this.imp.Visible = false;
            this.imp.Click += new System.EventHandler(this.imp_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Reset Camera";
            this.toolStripButton1.Visible = false;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Pintar";
            this.toolStripButton2.Visible = false;
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // colorb
            // 
            this.colorb.BackColor = System.Drawing.Color.Purple;
            this.colorb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.colorb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.colorb.Name = "colorb";
            this.colorb.Size = new System.Drawing.Size(23, 22);
            this.colorb.Text = "Cambiar Color";
            this.colorb.Visible = false;
            this.colorb.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // RenderMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(882, 533);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.renderWindowControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RenderMain";
            this.Text = "SAARTAC 3D";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Kitware.VTK.RenderWindowControl renderWindowControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton guarda;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton medir;
        private System.Windows.Forms.ToolStripButton cortar;
        private System.Windows.Forms.ToolStripButton puntero;
        private System.Windows.Forms.ToolStripButton escalar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton imp;
        private System.Windows.Forms.ToolStripButton contorno;
        private System.Windows.Forms.ToolStripButton seed;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton colorb;
    }
}