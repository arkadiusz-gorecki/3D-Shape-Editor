using _3DShapeEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace _3DShapeEditor
{
    public class ShapeManager
    {
        public static int width; // wymiary rysowania
        public static int height;
        private List<Shape> shapes = new List<Shape>();
        private Camera camera;
        internal List<Shape> Shapes { get => shapes;}
        internal Camera Camera { get => camera;}

        private Pipeline pipeline = new Pipeline();

        public ShapeManager(int w, int h)
        {
            width = w;
            height = h;

            camera = new Camera(new Vertex(6, 0, -4), new Vertex(0, 0, 2), (float)width / height, 90, 1, 10);
            GenerateCubes();
        }

        private void GenerateCubes()
        {
            shapes.Add(new Cube(Color.Red, 1, 2, 0, 4, 0, 0, 0, 1, 1, 2));
            shapes.Add(new Cube(Color.Orange, 1, 0, 4, 0, 0, 0, 0, 1, 1, 1));
            shapes.Add(new Cube(Color.Yellow, 1, 4, 0, 0, 0, 0, 0, 1, 1, 1));
            shapes.Add(new Cube(Color.Green, 1, -4, 0, 0, 0, 0, 0, 1, 1, 1));
            shapes.Add(new Cube(Color.Aquamarine, 1, 0, -4, 0, 0, 0, 0, 1, 1, 1));
            shapes.Add(new Cube(Color.Blue, 1, 0, 0, -4, 0, 0, 0, 1, 1, 1));
        }

        internal void ChangeDrawingScreenResolution(int w, int h)
        {
            width = w;
            height = h;
            camera.ChangeAspect((float)w / h);
        }
        public void DrawAllShapes(Graphics g)
        {
            pipeline.viewMatrix = camera.GetViewMatrix;
            pipeline.projectionMatrix = camera.GetProjectionMatrix;
            pipeline.width = width;
            pipeline.height = height;

            g.Clear(Color.Black);
            foreach (Shape shape in Shapes)
                shape.DrawEdges(g, pipeline);
        }

    }
}