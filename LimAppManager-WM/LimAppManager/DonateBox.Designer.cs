﻿namespace LimAppManager
{
    partial class DonateBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DonateBox));
            this.OKButton = new System.Windows.Forms.Button();
            this.QRBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(204)))), ((int)(((byte)(192)))));
            this.OKButton.Location = new System.Drawing.Point(84, 268);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 20);
            this.OKButton.TabIndex = 12;
            this.OKButton.Text = "OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // QRBox
            // 
            this.QRBox.BackColor = System.Drawing.Color.White;
            this.QRBox.Image = ((System.Drawing.Image)(resources.GetObject("QRBox.Image")));
            this.QRBox.Location = new System.Drawing.Point(13, 40);
            this.QRBox.Name = "QRBox";
            this.QRBox.Size = new System.Drawing.Size(215, 215);
            this.QRBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 26);
            this.label1.Text = "Use your smartphone";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DonateBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(173)))), ((int)(((byte)(160)))));
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.QRBox);
            this.Controls.Add(this.OKButton);
            this.Name = "DonateBox";
            this.Text = "Donate";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.PictureBox QRBox;
        private System.Windows.Forms.Label label1;
    }
}