namespace AGVSOFTWARE
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnGetGPS = new System.Windows.Forms.Button();
            this.btnScanLidar = new System.Windows.Forms.Button();
            this.pnlPaint = new System.Windows.Forms.Panel();
            this.btnCloseRpLidar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCloseRpLidar);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.btnGetGPS);
            this.panel1.Controls.Add(this.btnScanLidar);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 60);
            this.panel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(162, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // btnGetGPS
            // 
            this.btnGetGPS.Location = new System.Drawing.Point(289, 19);
            this.btnGetGPS.Name = "btnGetGPS";
            this.btnGetGPS.Size = new System.Drawing.Size(118, 23);
            this.btnGetGPS.TabIndex = 1;
            this.btnGetGPS.Text = "GET GPS";
            this.btnGetGPS.UseVisualStyleBackColor = true;
            this.btnGetGPS.Click += new System.EventHandler(this.btnGetGPS_Click);
            // 
            // btnScanLidar
            // 
            this.btnScanLidar.Location = new System.Drawing.Point(19, 19);
            this.btnScanLidar.Name = "btnScanLidar";
            this.btnScanLidar.Size = new System.Drawing.Size(118, 23);
            this.btnScanLidar.TabIndex = 0;
            this.btnScanLidar.Text = "SCAN DATA";
            this.btnScanLidar.UseVisualStyleBackColor = true;
            this.btnScanLidar.Click += new System.EventHandler(this.btnScanLidar_Click);
            // 
            // pnlPaint
            // 
            this.pnlPaint.Location = new System.Drawing.Point(1, 64);
            this.pnlPaint.Name = "pnlPaint";
            this.pnlPaint.Size = new System.Drawing.Size(1083, 473);
            this.pnlPaint.TabIndex = 1;
            // 
            // btnCloseRpLidar
            // 
            this.btnCloseRpLidar.Location = new System.Drawing.Point(438, 19);
            this.btnCloseRpLidar.Name = "btnCloseRpLidar";
            this.btnCloseRpLidar.Size = new System.Drawing.Size(161, 23);
            this.btnCloseRpLidar.TabIndex = 0;
            this.btnCloseRpLidar.Text = "CloseScan";
            this.btnCloseRpLidar.UseVisualStyleBackColor = true;
            this.btnCloseRpLidar.Click += new System.EventHandler(this.btnCloseRpLidar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 540);
            this.Controls.Add(this.pnlPaint);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnGetGPS;
        private System.Windows.Forms.Button btnScanLidar;
        private System.Windows.Forms.Panel pnlPaint;
        private System.Windows.Forms.Button btnCloseRpLidar;
    }
}

