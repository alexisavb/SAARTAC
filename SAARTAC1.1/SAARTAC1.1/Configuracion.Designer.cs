namespace SAARTAC1._1
{
    partial class Configuracion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuracion));
            this.RutaPython = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Procesos = new System.Windows.Forms.NumericUpDown();
            this.textNumIteracion = new System.Windows.Forms.Label();
            this.numIteraciones = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Procesos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIteraciones)).BeginInit();
            this.SuspendLayout();
            // 
            // RutaPython
            // 
            this.RutaPython.AutoSize = true;
            this.RutaPython.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.RutaPython.ForeColor = System.Drawing.Color.White;
            this.RutaPython.Location = new System.Drawing.Point(24, 65);
            this.RutaPython.Name = "RutaPython";
            this.RutaPython.Size = new System.Drawing.Size(93, 18);
            this.RutaPython.TabIndex = 0;
            this.RutaPython.Text = "Ruta Python:";
            this.RutaPython.Click += new System.EventHandler(this.RutaPython_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(24, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ruta lectura DICOM:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(497, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Seleccionar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(497, 118);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Seleccionar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(327, 471);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 41);
            this.button3.TabIndex = 7;
            this.button3.Text = "Cancelar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(489, 471);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(126, 41);
            this.button4.TabIndex = 8;
            this.button4.Text = "Aceptar";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(182, 122);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(271, 20);
            this.textBox2.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(182, 63);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(271, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(24, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "Número de procesos:";
            // 
            // Procesos
            // 
            this.Procesos.Location = new System.Drawing.Point(218, 185);
            this.Procesos.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Procesos.Name = "Procesos";
            this.Procesos.Size = new System.Drawing.Size(60, 20);
            this.Procesos.TabIndex = 11;
            this.Procesos.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textNumIteracion
            // 
            this.textNumIteracion.AutoSize = true;
            this.textNumIteracion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.textNumIteracion.ForeColor = System.Drawing.Color.White;
            this.textNumIteracion.Location = new System.Drawing.Point(24, 233);
            this.textNumIteracion.Name = "textNumIteracion";
            this.textNumIteracion.Size = new System.Drawing.Size(162, 18);
            this.textNumIteracion.TabIndex = 12;
            this.textNumIteracion.Text = "Número de iteraciones:";
            // 
            // numIteraciones
            // 
            this.numIteraciones.Location = new System.Drawing.Point(218, 235);
            this.numIteraciones.Name = "numIteraciones";
            this.numIteraciones.Size = new System.Drawing.Size(60, 20);
            this.numIteraciones.TabIndex = 13;
            this.numIteraciones.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(24, 284);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 18);
            this.label3.TabIndex = 14;
            this.label3.Text = "Omitir píxeles de aire o similares";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(251, 288);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Configuracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(648, 569);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numIteraciones);
            this.Controls.Add(this.textNumIteracion);
            this.Controls.Add(this.Procesos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.RutaPython);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Configuracion";
            this.Text = "Configuracion";
            this.Load += new System.EventHandler(this.Configuracion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Procesos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIteraciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RutaPython;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Procesos;
        private System.Windows.Forms.Label textNumIteracion;
        private System.Windows.Forms.NumericUpDown numIteraciones;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}