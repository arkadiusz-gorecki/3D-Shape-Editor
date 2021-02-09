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
            this.backfaceCullingCheckBox = new System.Windows.Forms.CheckBox();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.createCubeButton = new System.Windows.Forms.Button();
            this.createSphereButton = new System.Windows.Forms.Button();
            this.mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 2;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.mainTableLayoutPanel.Controls.Add(this.backfaceCullingCheckBox, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.mainPictureBox, 0, 1);
            this.mainTableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(1126, 617);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // backfaceCullingCheckBox
            // 
            this.backfaceCullingCheckBox.AutoSize = true;
            this.backfaceCullingCheckBox.Checked = true;
            this.backfaceCullingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backfaceCullingCheckBox.Location = new System.Drawing.Point(3, 3);
            this.backfaceCullingCheckBox.Name = "backfaceCullingCheckBox";
            this.backfaceCullingCheckBox.Size = new System.Drawing.Size(106, 17);
            this.backfaceCullingCheckBox.TabIndex = 0;
            this.backfaceCullingCheckBox.Text = "Backface Culling";
            this.backfaceCullingCheckBox.UseVisualStyleBackColor = true;
            this.backfaceCullingCheckBox.CheckedChanged += new System.EventHandler(this.backfaceCullingCheckBox_CheckedChanged);
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.BackColor = System.Drawing.Color.White;
            this.mainPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPictureBox.Location = new System.Drawing.Point(3, 32);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(1019, 582);
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
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.createCubeButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.createSphereButton, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1028, 32);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(95, 582);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // createCubeButton
            // 
            this.createCubeButton.Location = new System.Drawing.Point(3, 3);
            this.createCubeButton.Name = "createCubeButton";
            this.createCubeButton.Size = new System.Drawing.Size(89, 23);
            this.createCubeButton.TabIndex = 0;
            this.createCubeButton.Text = "Create Cube";
            this.createCubeButton.UseVisualStyleBackColor = true;
            this.createCubeButton.Click += new System.EventHandler(this.createCubeButton_Click);
            // 
            // createSphereButton
            // 
            this.createSphereButton.Location = new System.Drawing.Point(3, 33);
            this.createSphereButton.Name = "createSphereButton";
            this.createSphereButton.Size = new System.Drawing.Size(89, 23);
            this.createSphereButton.TabIndex = 1;
            this.createSphereButton.Text = "Create Sphere";
            this.createSphereButton.UseVisualStyleBackColor = true;
            this.createSphereButton.Click += new System.EventHandler(this.createSphereButton_Click);
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
    }
}

