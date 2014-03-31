namespace TileEditor
{
    partial class frmNewLayer
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
            this.txtLayerName = new System.Windows.Forms.TextBox();
            this.txtLayerHeight = new System.Windows.Forms.TextBox();
            this.txtLayerWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLayerName
            // 
            this.txtLayerName.Location = new System.Drawing.Point(92, 6);
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(164, 20);
            this.txtLayerName.TabIndex = 0;
            // 
            // txtLayerHeight
            // 
            this.txtLayerHeight.Location = new System.Drawing.Point(92, 35);
            this.txtLayerHeight.Name = "txtLayerHeight";
            this.txtLayerHeight.Size = new System.Drawing.Size(47, 20);
            this.txtLayerHeight.TabIndex = 1;
            // 
            // txtLayerWidth
            // 
            this.txtLayerWidth.Location = new System.Drawing.Point(92, 61);
            this.txtLayerWidth.Name = "txtLayerWidth";
            this.txtLayerWidth.Size = new System.Drawing.Size(47, 20);
            this.txtLayerWidth.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Layer Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Layer Height";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Layer Width";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(123, 87);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(48, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(177, 87);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmNewLayer
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(261, 111);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLayerWidth);
            this.Controls.Add(this.txtLayerHeight);
            this.Controls.Add(this.txtLayerName);
            this.Name = "frmNewLayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add New Layer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox txtLayerName;
        public System.Windows.Forms.TextBox txtLayerHeight;
        public System.Windows.Forms.TextBox txtLayerWidth;
    }
}