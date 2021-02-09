using _3DShapeEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace _3DShapeEditor
{
    public class ShapeManager
    {
        public static int width; // wymiary rysowania
        public static int height;
        private List<Shape> shapes;
        private Camera camera;
        private Pipeline pipeline;
        internal List<Shape> Shapes { get => shapes; }
        internal Camera Camera { get => camera;}
        internal Pipeline Pipeline { get => pipeline; }

        public ShapeManager(int w, int h, Point mousePosition)
        {
            width = w;
            height = h;

            camera = new Camera(new Vertex(0, 6, -6), new Vertex(0, 0, 0), (float)width / height, 90, 1, 15, mousePosition);

            pipeline = new Pipeline(camera.GetViewMatrix, camera.GetProjectionMatrix, width, height);

            shapes = new List<Shape>();
            GenerateShapes();
        }

        private void GenerateShapes()
        {
            shapes.Add(new Cube(Color.Red, 1, 0, 2, 6, 0, 0, 0, 1, 1, 3));
            shapes.Add(new Cube(Color.Orange, 1, 0, 8, 0, 1, 1, 1, 1, 1, 1));
            shapes.Add(new Sphere(Color.Lime, 2, 10, 10, 0, -5, 0, 0, 0, 0));
            shapes.Add(new Sphere(Color.MediumPurple, 1, 10, 10, 2, -5, 0));
            shapes.Add(new Sphere(Color.White, 2, 10, 10, 8, 8, 8, 0, 1, 0, 1, 1, 2));
        }

        internal void ChangeDrawingScreenResolution(int w, int h)
        {
            width = w;
            height = h;
            camera.ChangeAspect((float)w / h);
            pipeline.SetScreenDimensions(w, h);
            pipeline.SetViewMatrix(camera.GetViewMatrix);
            pipeline.UpdateMatrices();
            pipeline.InitializeDepthBuffer();
        }
        public void DrawAllShapes(Bitmap bitmap)
        {
            pipeline.ClearDepthBuffer();
            foreach (Shape shape in shapes)
                shape.DrawEdges(bitmap, pipeline);
        }

        internal void AddShape(Shape shape)
        {
            shapes.Add(shape);
        }
    }
}