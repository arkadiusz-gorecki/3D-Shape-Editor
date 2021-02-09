using _3DShapeEditor.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3DShapeEditor
{
    public partial class CreateSphereForm : Form
    {
        public int parallelCount = 5;
        public int meridianCount = 5;
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
        }

        protected void GetValues()
        {
            parallelCount = (int)ParallelNumericUpDown.Value;
            meridianCount = (int)MeridianNumericUpDown.Value;
            size = (float)SizeNumericUpDown.Value;
            x = (float)XNumericUpDown.Value;
            y = (float)YNumericUpDown.Value;
            z = (float)ZNumericUpDown.Value;
            xAngle = (float)XAngleNumericUpDown.Value;
            yAngle = (float)YAngleNumericUpDown.Value;
            zAngle = (float)ZAngleNumericUpDown.Value;
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
