using _3DShapeEditor.Shapes;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace _3DShapeEditor
{
    public partial class ShapeEditorForm : Form
    {
        Bitmap bitmap;
        private int fpsCap = 20;
        private Timer drawingTimer;
        private ShapeManager shapeManager;
        public ShapeEditorForm()
        {
            InitializeComponent();
            shapeManager = new ShapeManager(mainPictureBox.Width, mainPictureBox.Height, Cursor.Position);
            InitializeDrawingTimer();
            drawingTimer.Start();
        }

        private void InitializeDrawingTimer()
        {
            drawingTimer = new Timer();
            drawingTimer.Tick += new EventHandler(Loop);
            drawingTimer.Interval = 1000 / fpsCap;
        }

        private void Loop(object myObject, EventArgs e)
        {
            shapeManager.Camera.Move();
            shapeManager.Pipeline.SetViewMatrix(shapeManager.Camera.GetViewMatrix);
            shapeManager.Pipeline.UpdateMatrices();
            //foreach (Shape shape in shapeManager.Shapes)
            //    shape.IncrementAngle(); // obiekty się obracają
            mainPictureBox.Invalidate();
        }
        private void mainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            bitmap = new Bitmap(mainPictureBox.Width, mainPictureBox.Height);
            e.Graphics.Clear(Color.Black);
            shapeManager.DrawAllShapes(bitmap);
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
        private void mainPictureBox_SizeChanged(object sender, EventArgs e)
        {
            shapeManager.ChangeDrawingScreenResolution(mainPictureBox.Width, mainPictureBox.Height);
        }
        private void backfaceCullingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            shapeManager.Pipeline.EnableBackfaceCulling(backfaceCullingCheckBox.Checked);
        }
        private void mainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                shapeManager.Camera.SetMouseHolding(true, e.Location);
                Cursor.Current = Cursors.NoMove2D;
            }
        }
        private void mainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            shapeManager.Camera.SetMousePosition(e.Location);
        }
        private void mainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                shapeManager.Camera.SetMouseHolding(false, new Point());
                Cursor.Current = Cursors.Default;
            }
        }

        private void createCubeButton_Click(object sender, EventArgs e)
        {
            CreateCubeForm ccf = new CreateCubeForm();
            if (ccf.ShowDialog() == DialogResult.OK)
            {
                float size = ccf.size;
                float x = ccf.x;
                float y = ccf.y;
                float z = ccf.z;
                float xAngle = ccf.xAngle;
                float yAngle = ccf.yAngle;
                float zAngle = ccf.zAngle;
                float xScale = ccf.xScale;
                float yScale = ccf.yScale;
                float zScale = ccf.zScale;
                shapeManager.AddShape(new Cube(Color.Bisque, size, x, y, z, xAngle, yAngle, zAngle, xScale, yScale, zScale));
            }
            ccf.Close();
        }

        private void createSphereButton_Click(object sender, EventArgs e)
        {
            CreateSphereForm csf = new CreateSphereForm();
            if (csf.ShowDialog() == DialogResult.OK)
            {
                int parallel = csf.parallelCount;
                int meridian = csf.meridianCount;
                float size = csf.size;
                float x = csf.x;
                float y = csf.y;
                float z = csf.z;
                float xAngle = csf.xAngle;
                float yAngle = csf.yAngle;
                float zAngle = csf.zAngle;
                float xScale = csf.xScale;
                float yScale = csf.yScale;
                float zScale = csf.zScale;
                shapeManager.AddShape(new Sphere(Color.Bisque, size, parallel, meridian, x, y, z, xAngle, yAngle, zAngle, xScale, yScale, zScale));
            }
            csf.Close();
        }
    }
}
