using _3DShapeEditor.Shapes;
using System;
using System.Windows.Forms;

namespace _3DShapeEditor
{
    public partial class ShapeEditorForm : Form
    {
        private int fpsCap = 20;
        private Timer drawingTimer;
        private ShapeManager shapeManager;
        public ShapeEditorForm()
        {
            InitializeComponent();
            shapeManager = new ShapeManager(mainPictureBox.Width, mainPictureBox.Height);
            shapeManager.SetDrawingScreenResolution(mainPictureBox.Width, mainPictureBox.Height);
            InitializeDrawingTimer();
            drawingTimer.Start();
        }

        private void InitializeDrawingTimer()
        {
            drawingTimer = new Timer();
            drawingTimer.Tick += new EventHandler(Repaint);
            drawingTimer.Interval = 1000 / fpsCap;
        }

        private void Repaint(object myObject, EventArgs e)
        {
            foreach (Shape shape in shapeManager.Shapes)
                shape.IncrementAngle(); // obiekty się obracają
            mainPictureBox.Invalidate();
        }
        private void mainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            shapeManager.DrawAllShapes(e.Graphics);
        }
        private void mainPictureBox_SizeChanged(object sender, EventArgs e)
        {
            shapeManager.SetDrawingScreenResolution(mainPictureBox.Width, mainPictureBox.Height);
            shapeManager.Camera.UpdateProjectionMatrix((float)mainPictureBox.Width / (float)mainPictureBox.Height);
        }

    }
}
