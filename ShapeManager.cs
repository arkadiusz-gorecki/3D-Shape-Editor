using _3DShapeEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace _3DShapeEditor
{
    public class ShapeManager
    {
        public static float width; // wymiary rysowania
        public static float height;
        private List<Shape> shapes = new List<Shape>();
        private Camera camera;
        internal List<Shape> Shapes { get => shapes;}
        internal Camera Camera { get => camera;}

        public ShapeManager(float w, float h)
        {
            width = w;
            height = h;
            camera = new Camera(new Vertex(0, 0, 0), new Vertex(0, 0, 1), width / height);
            GenerateCubes();
        }

        private void GenerateCubes()
        {
            Pen pen = new Pen(new SolidBrush(Color.Red), 2);
            shapes.Add(new Cube(pen, 2, 0, 0, 4, 0, 1, 1, 1, 1, 3));
            pen = new Pen(new SolidBrush(Color.Green), 2);
            shapes.Add(new Cube(pen, 1, 0, 3, 3, 1, 0, 0));
            pen = new Pen(new SolidBrush(Color.Yellow), 2);
            shapes.Add(new Cube(pen, 2, 3, 0, 3, 1, 0, 1));
        }

        internal void SetDrawingScreenResolution(int w, int h)
        {
            width = w;
            height = h;
        }
        public void DrawAllShapes(Graphics g)
        {
            g.Clear(Color.Black);
            foreach (Shape shape in Shapes)
            {
                shape.DrawEdges(g, camera);
            }
        }

    }
}