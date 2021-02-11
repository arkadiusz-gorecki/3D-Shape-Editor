namespace _3DShapeEditor
{
    partial class ShapeEditorForm
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
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.createSphereButton = new System.Windows.Forms.Button();
            this.createCubeButton = new System.Windows.Forms.Button();
            this.backfaceCullingCheckBox = new System.Windows.Forms.CheckBox();
            this.zBufferingCheckBox = new System.Windows.Forms.CheckBox();
            this.drawingEdgesCheckBox = new System.Windows.Forms.CheckBox();
            this.shadingCheckBox = new System.Windows.Forms.CheckBox();
            this.animateCheckBox = new System.Windows.Forms.CheckBox();
            this.mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTableLayoutPanel.ColumnCount = 7;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 476F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.mainTableLayoutPanel.Controls.Add(this.mainPictureBox, 0, 1);
            this.mainTableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 6, 1);
            this.mainTableLayoutPanel.Controls.Add(this.backfaceCullingCheckBox, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.zBufferingCheckBox, 1, 0);
            this.mainTableLayoutPanel.Controls.Add(this.drawingEdgesCheckBox, 2, 0);
            this.mainTableLayoutPanel.Controls.Add(this.shadingCheckBox, 3, 0);
            this.mainTableLayoutPanel.Controls.Add(this.animateCheckBox, 5, 0);
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(1126, 617);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.BackColor = System.Drawing.Color.White;
            this.mainPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainTableLayoutPanel.SetColumnSpan(this.mainPictureBox, 6);
            this.mainPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPictureBox.Location = new System.Drawing.Point(3, 32);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(1050, 582);
            this.mainPictureBox.TabIndex = 0;
            this.mainPictureBox.TabStop = false;
            this.mainPictureBox.SizeChanged += new System.EventHandler(this.mainPictureBox_SizeChanged);
            this.mainPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPictureBox_Paint);
            this.mainPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainPictureBox_MouseDown);
            this.mainPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainPictureBox_MouseMove);
            this.mainPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mainPictureBox_MouseUp);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.createSphereButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.createCubeButton, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1059, 32);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(64, 582);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // createSphereButton
            // 
            this.createSphereButton.Location = new System.Drawing.Point(3, 48);
            this.createSphereButton.Name = "createSphereButton";
            this.createSphereButton.Size = new System.Drawing.Size(58, 41);
            this.createSphereButton.TabIndex = 1;
            this.createSphereButton.Text = "Create Sphere";
            this.createSphereButton.UseVisualStyleBackColor = true;
            this.createSphereButton.Click += new System.EventHandler(this.createSphereButton_Click);
            // 
            // createCubeButton
            // 
            this.createCubeButton.Location = new System.Drawing.Point(3, 3);
            this.createCubeButton.Name = "createCubeButton";
            this.createCubeButton.Size = new System.Drawing.Size(58, 39);
            this.createCubeButton.TabIndex = 0;
            this.createCubeButton.Text = "Create Cube";
            this.createCubeButton.UseVisualStyleBackColor = true;
            this.createCubeButton.Click += new System.EventHandler(this.createCubeButton_Click);
            // 
            // backfaceCullingCheckBox
            // 
            this.backfaceCullingCheckBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.backfaceCullingCheckBox.AutoSize = true;
            this.backfaceCullingCheckBox.Checked = true;
            this.backfaceCullingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backfaceCullingCheckBox.Location = new System.Drawing.Point(10, 6);
            this.backfaceCullingCheckBox.Name = "backfaceCullingCheckBox";
            this.backfaceCullingCheckBox.Size = new System.Drawing.Size(106, 17);
            this.backfaceCullingCheckBox.TabIndex = 0;
            this.backfaceCullingCheckBox.Text = "Backface Culling";
            this.backfaceCullingCheckBox.UseVisualStyleBackColor = true;
            this.backfaceCullingCheckBox.CheckedChanged += new System.EventHandler(this.backfaceCullingCheckBox_CheckedChanged);
            // 
            // zBufferingCheckBox
            // 
            this.zBufferingCheckBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zBufferingCheckBox.AutoSize = true;
            this.zBufferingCheckBox.Checked = true;
            this.zBufferingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zBufferingCheckBox.Location = new System.Drawing.Point(129, 6);
            this.zBufferingCheckBox.Name = "zBufferingCheckBox";
            this.zBufferingCheckBox.Size = new System.Drawing.Size(78, 17);
            this.zBufferingCheckBox.TabIndex = 2;
            this.zBufferingCheckBox.Text = "Z-Buffering";
            this.zBufferingCheckBox.UseVisualStyleBackColor = true;
            this.zBufferingCheckBox.CheckedChanged += new System.EventHandler(this.zBufferingCheckBox_CheckedChanged);
            // 
            // drawingEdgesCheckBox
            // 
            this.drawingEdgesCheckBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.drawingEdgesCheckBox.AutoSize = true;
            this.drawingEdgesCheckBox.Checked = true;
            this.drawingEdgesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.drawingEdgesCheckBox.Location = new System.Drawing.Point(221, 6);
            this.drawingEdgesCheckBox.Name = "drawingEdgesCheckBox";
            this.drawingEdgesCheckBox.Size = new System.Drawing.Size(98, 17);
            this.drawingEdgesCheckBox.TabIndex = 3;
            this.drawingEdgesCheckBox.Text = "Drawing Edges";
            this.drawingEdgesCheckBox.UseVisualStyleBackColor = true;
            this.drawingEdgesCheckBox.CheckedChanged += new System.EventHandler(this.drawingEdgesCheckBox_CheckedChanged);
            // 
            // shadingCheckBox
            // 
            this.shadingCheckBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.shadingCheckBox.AutoSize = true;
            this.shadingCheckBox.Enabled = false;
            this.shadingCheckBox.Location = new System.Drawing.Point(349, 6);
            this.shadingCheckBox.Name = "shadingCheckBox";
            this.shadingCheckBox.Size = new System.Drawing.Size(65, 17);
            this.shadingCheckBox.TabIndex = 4;
            this.shadingCheckBox.Text = "Shading";
            this.shadingCheckBox.UseVisualStyleBackColor = true;
            this.shadingCheckBox.CheckedChanged += new System.EventHandler(this.shadingCheckBox_CheckedChanged);
            // 
            // animateCheckBox
            // 
            this.animateCheckBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.animateCheckBox.AutoSize = true;
            this.animateCheckBox.Location = new System.Drawing.Point(950, 6);
            this.animateCheckBox.Name = "animateCheckBox";
            this.animateCheckBox.Size = new System.Drawing.Size(64, 17);
            this.animateCheckBox.TabIndex = 5;
            this.animateCheckBox.Text = "Animate";
            this.animateCheckBox.UseVisualStyleBackColor = true;
            this.animateCheckBox.CheckedChanged += new System.EventHandler(this.animateCheckBox_CheckedChanged);
            // 
            // ShapeEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 617);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "ShapeEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "3D Shape Editor";
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.mainTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox backfaceCullingCheckBox;
        private System.Windows.Forms.Button createCubeButton;
        private System.Windows.Forms.Button createSphereButton;
        private System.Windows.Forms.CheckBox zBufferingCheckBox;
        private System.Windows.Forms.CheckBox drawingEdgesCheckBox;
        private System.Windows.Forms.CheckBox shadingCheckBox;
        private System.Windows.Forms.CheckBox animateCheckBox;
    }
}

