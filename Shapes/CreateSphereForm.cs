﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace _3DShapeEditor
{
    public partial class CreateSphereForm : Form
    {
        public Color color = Color.White;
        public int parallelCount = 10;
        public int meridianCount = 10;
        public float size = 1;
        public float x = 0;
        public float y = 0;
        public float z = 0;
        public float xAngle = 0;
        public float yAngle = 0;
        public float zAngle = 0;
        public float xScale = 1;
        public float yScale = 1;
        public float zScale = 1;
        public CreateSphereForm()
        {
            InitializeComponent();
            object[] colors =
            {
                Color.White,
                Color.Red,
                Color.Orange,
                Color.Yellow,
                Color.Lime,
                Color.Green,
                Color.Turquoise,
                Color.Blue,
                Color.Violet,
            };
            colorComboBox.Items.AddRange(colors);
            colorComboBox.SelectedIndex = 0;
        }

        protected void GetValues()
        {
            color = (Color)colorComboBox.SelectedItem;
            parallelCount = (int)ParallelNumericUpDown.Value;
            meridianCount = (int)MeridianNumericUpDown.Value;
            size = (float)SizeNumericUpDown.Value;
            x = (float)XNumericUpDown.Value;
            y = (float)YNumericUpDown.Value;
            z = (float)ZNumericUpDown.Value;
            xAngle = (float)XAngleNumericUpDown.Value * (float)(Math.PI / 180); // konwersja z stopni do radianów
            yAngle = (float)YAngleNumericUpDown.Value * (float)(Math.PI / 180);
            zAngle = (float)ZAngleNumericUpDown.Value * (float)(Math.PI / 180);
            xScale = (float)XScaleNumericUpDown.Value;
            yScale = (float)YScaleNumericUpDown.Value;
            zScale = (float)ZScaleNumericUpDown.Value;
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            GetValues();
            DialogResult = DialogResult.OK;
        }
    }
}
