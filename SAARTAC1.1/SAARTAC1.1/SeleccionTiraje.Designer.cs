﻿namespace SAARTAC1._1 {
    partial class SeleccionTiraje {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeleccionTiraje));
            this.label1 = new System.Windows.Forms.Label();
            this.InicioTiraje = new System.Windows.Forms.NumericUpDown();
            this.FinTiraje = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.InicioTiraje)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FinTiraje)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Selecciona el rango de imagenes:";
            // 
            // InicioTiraje
            // 
            this.InicioTiraje.Location = new System.Drawing.Point(136, 87);
            this.InicioTiraje.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.InicioTiraje.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.InicioTiraje.Name = "InicioTiraje";
            this.InicioTiraje.Size = new System.Drawing.Size(45, 20);
            this.InicioTiraje.TabIndex = 1;
            this.InicioTiraje.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FinTiraje
            // 
            this.FinTiraje.Location = new System.Drawing.Point(136, 134);
            this.FinTiraje.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.FinTiraje.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FinTiraje.Name = "FinTiraje";
            this.FinTiraje.Size = new System.Drawing.Size(45, 20);
            this.FinTiraje.TabIndex = 2;
            this.FinTiraje.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(106, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Aceptar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(21, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Inicio";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(21, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Hasta";
            // 
            // SeleccionTiraje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.FinTiraje);
            this.Controls.Add(this.InicioTiraje);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SeleccionTiraje";
            this.Text = "Selección";
            ((System.ComponentModel.ISupportInitialize)(this.InicioTiraje)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FinTiraje)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown InicioTiraje;
        private System.Windows.Forms.NumericUpDown FinTiraje;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}