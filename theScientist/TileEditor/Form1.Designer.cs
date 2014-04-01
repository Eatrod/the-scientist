namespace TileEditor
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
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTileMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtContentPath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.rdbDraw = new System.Windows.Forms.RadioButton();
            this.rdbErase = new System.Windows.Forms.RadioButton();
            this.lstLayers = new System.Windows.Forms.ListBox();
            this.btnAddLayer = new System.Windows.Forms.Button();
            this.btnRemoveLayer = new System.Windows.Forms.Button();
            this.lblLayers = new System.Windows.Forms.Label();
            this.lstTexture = new System.Windows.Forms.ListBox();
            this.btnAddTexture = new System.Windows.Forms.Button();
            this.btnRemoveTexture = new System.Windows.Forms.Button();
            this.lblTexture = new System.Windows.Forms.Label();
            this.picbTexturePreviev = new System.Windows.Forms.PictureBox();
            this.chbFill = new System.Windows.Forms.CheckBox();
            this.trbAlphaSlider = new System.Windows.Forms.TrackBar();
            this.tileDisplay1 = new TileEditor.TileDisplay();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbTexturePreviev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbAlphaSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(-3, 615);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(757, 20);
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.Visible = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(757, 24);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(18, 588);
            this.vScrollBar1.TabIndex = 2;
            this.vScrollBar1.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1311, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTileMapToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newTileMapToolStripMenuItem
            // 
            this.newTileMapToolStripMenuItem.Name = "newTileMapToolStripMenuItem";
            this.newTileMapToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newTileMapToolStripMenuItem.Text = "New Tile Map";
            this.newTileMapToolStripMenuItem.Click += new System.EventHandler(this.newTileMapToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // txtContentPath
            // 
            this.txtContentPath.Location = new System.Drawing.Point(778, 48);
            this.txtContentPath.Name = "txtContentPath";
            this.txtContentPath.ReadOnly = true;
            this.txtContentPath.Size = new System.Drawing.Size(215, 20);
            this.txtContentPath.TabIndex = 4;
            // 
            // rdbDraw
            // 
            this.rdbDraw.AutoSize = true;
            this.rdbDraw.Checked = true;
            this.rdbDraw.Location = new System.Drawing.Point(810, 74);
            this.rdbDraw.Name = "rdbDraw";
            this.rdbDraw.Size = new System.Drawing.Size(70, 17);
            this.rdbDraw.TabIndex = 6;
            this.rdbDraw.TabStop = true;
            this.rdbDraw.Text = "Draw Tile";
            this.rdbDraw.UseVisualStyleBackColor = true;
            // 
            // rdbErase
            // 
            this.rdbErase.AutoSize = true;
            this.rdbErase.Location = new System.Drawing.Point(810, 97);
            this.rdbErase.Name = "rdbErase";
            this.rdbErase.Size = new System.Drawing.Size(72, 17);
            this.rdbErase.TabIndex = 6;
            this.rdbErase.Text = "Erase Tile";
            this.rdbErase.UseVisualStyleBackColor = true;
            // 
            // lstLayers
            // 
            this.lstLayers.FormattingEnabled = true;
            this.lstLayers.Location = new System.Drawing.Point(778, 183);
            this.lstLayers.Name = "lstLayers";
            this.lstLayers.Size = new System.Drawing.Size(215, 69);
            this.lstLayers.TabIndex = 7;
            this.lstLayers.SelectedIndexChanged += new System.EventHandler(this.lstLayers_SelectedIndexChanged);
            // 
            // btnAddLayer
            // 
            this.btnAddLayer.Location = new System.Drawing.Point(808, 259);
            this.btnAddLayer.Name = "btnAddLayer";
            this.btnAddLayer.Size = new System.Drawing.Size(75, 23);
            this.btnAddLayer.TabIndex = 8;
            this.btnAddLayer.Text = "Add";
            this.btnAddLayer.UseVisualStyleBackColor = true;
            this.btnAddLayer.Click += new System.EventHandler(this.btnAddLayer_Click);
            // 
            // btnRemoveLayer
            // 
            this.btnRemoveLayer.Location = new System.Drawing.Point(889, 259);
            this.btnRemoveLayer.Name = "btnRemoveLayer";
            this.btnRemoveLayer.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveLayer.TabIndex = 8;
            this.btnRemoveLayer.Text = "Remove";
            this.btnRemoveLayer.UseVisualStyleBackColor = true;
            this.btnRemoveLayer.Click += new System.EventHandler(this.btnRemoveLayer_Click);
            // 
            // lblLayers
            // 
            this.lblLayers.AutoSize = true;
            this.lblLayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLayers.Location = new System.Drawing.Point(778, 163);
            this.lblLayers.Name = "lblLayers";
            this.lblLayers.Size = new System.Drawing.Size(55, 16);
            this.lblLayers.TabIndex = 9;
            this.lblLayers.Text = "Layers";
            // 
            // lstTexture
            // 
            this.lstTexture.FormattingEnabled = true;
            this.lstTexture.Location = new System.Drawing.Point(778, 300);
            this.lstTexture.Name = "lstTexture";
            this.lstTexture.Size = new System.Drawing.Size(215, 95);
            this.lstTexture.TabIndex = 7;
            this.lstTexture.SelectedIndexChanged += new System.EventHandler(this.lstTexture_SelectedIndexChanged);
            // 
            // btnAddTexture
            // 
            this.btnAddTexture.Location = new System.Drawing.Point(808, 401);
            this.btnAddTexture.Name = "btnAddTexture";
            this.btnAddTexture.Size = new System.Drawing.Size(75, 23);
            this.btnAddTexture.TabIndex = 8;
            this.btnAddTexture.Text = "Add";
            this.btnAddTexture.UseVisualStyleBackColor = true;
            this.btnAddTexture.Click += new System.EventHandler(this.btnAddTexture_Click);
            // 
            // btnRemoveTexture
            // 
            this.btnRemoveTexture.Location = new System.Drawing.Point(889, 401);
            this.btnRemoveTexture.Name = "btnRemoveTexture";
            this.btnRemoveTexture.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveTexture.TabIndex = 8;
            this.btnRemoveTexture.Text = "Remove";
            this.btnRemoveTexture.UseVisualStyleBackColor = true;
            this.btnRemoveTexture.Click += new System.EventHandler(this.btnRemoveTexture_Click);
            // 
            // lblTexture
            // 
            this.lblTexture.AutoSize = true;
            this.lblTexture.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTexture.Location = new System.Drawing.Point(778, 284);
            this.lblTexture.Name = "lblTexture";
            this.lblTexture.Size = new System.Drawing.Size(60, 16);
            this.lblTexture.TabIndex = 9;
            this.lblTexture.Text = "Texture";
            // 
            // picbTexturePreviev
            // 
            this.picbTexturePreviev.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picbTexturePreviev.Location = new System.Drawing.Point(789, 430);
            this.picbTexturePreviev.Name = "picbTexturePreviev";
            this.picbTexturePreviev.Size = new System.Drawing.Size(190, 190);
            this.picbTexturePreviev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbTexturePreviev.TabIndex = 10;
            this.picbTexturePreviev.TabStop = false;
            // 
            // chbFill
            // 
            this.chbFill.AutoSize = true;
            this.chbFill.Location = new System.Drawing.Point(903, 85);
            this.chbFill.Name = "chbFill";
            this.chbFill.Size = new System.Drawing.Size(38, 17);
            this.chbFill.TabIndex = 11;
            this.chbFill.Text = "Fill";
            this.chbFill.UseVisualStyleBackColor = true;
            // 
            // trbAlphaSlider
            // 
            this.trbAlphaSlider.Location = new System.Drawing.Point(776, 119);
            this.trbAlphaSlider.Maximum = 100;
            this.trbAlphaSlider.Name = "trbAlphaSlider";
            this.trbAlphaSlider.Size = new System.Drawing.Size(217, 45);
            this.trbAlphaSlider.TabIndex = 12;
            this.trbAlphaSlider.TickFrequency = 5;
            this.trbAlphaSlider.Value = 100;
            this.trbAlphaSlider.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tileDisplay1.Location = new System.Drawing.Point(0, 24);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(754, 588);
            this.tileDisplay1.TabIndex = 0;
            this.tileDisplay1.Text = "tileDisplay1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1311, 631);
            this.Controls.Add(this.trbAlphaSlider);
            this.Controls.Add(this.chbFill);
            this.Controls.Add(this.picbTexturePreviev);
            this.Controls.Add(this.lblTexture);
            this.Controls.Add(this.lblLayers);
            this.Controls.Add(this.btnRemoveTexture);
            this.Controls.Add(this.btnRemoveLayer);
            this.Controls.Add(this.btnAddTexture);
            this.Controls.Add(this.btnAddLayer);
            this.Controls.Add(this.lstTexture);
            this.Controls.Add(this.lstLayers);
            this.Controls.Add(this.rdbErase);
            this.Controls.Add(this.rdbDraw);
            this.Controls.Add(this.txtContentPath);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.tileDisplay1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Tile Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbTexturePreviev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbAlphaSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TileDisplay tileDisplay1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTileMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtContentPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RadioButton rdbDraw;
        private System.Windows.Forms.RadioButton rdbErase;
        private System.Windows.Forms.ListBox lstLayers;
        private System.Windows.Forms.Button btnAddLayer;
        private System.Windows.Forms.Button btnRemoveLayer;
        private System.Windows.Forms.Label lblLayers;
        private System.Windows.Forms.ListBox lstTexture;
        private System.Windows.Forms.Button btnAddTexture;
        private System.Windows.Forms.Button btnRemoveTexture;
        private System.Windows.Forms.Label lblTexture;
        private System.Windows.Forms.PictureBox picbTexturePreviev;
        private System.Windows.Forms.CheckBox chbFill;
        private System.Windows.Forms.TrackBar trbAlphaSlider;
    }
}

