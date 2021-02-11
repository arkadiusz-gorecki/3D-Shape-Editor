using _3DShapeEditor.Shapes;
using System;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace _3DShapeEditor
{
    internal class ScreenTriangle
    {
        public ScreenPoint p1;
        public ScreenPoint p2;
        public ScreenPoint p3;
        public bool flatShading = true; // czy ten trójkąt ma być cieniowany algorytmem flat czy phong (np. wszystkie trójkąty Cube są flat, połowa trójkątów stożka jest flat a połowa phong)

        public ScreenTriangle(ScreenPoint p1, ScreenPoint p2, ScreenPoint p3, bool flatShading)
        {
            this.p1 = p1; this.p2 = p2; this.p3 = p3; this.flatShading = flatShading;
        }
        public ScreenTriangle(int x1, int y1, float z1, int x2, int y2, float z2, int x3, int y3, float z3)
        {
            p1 = new ScreenPoint(x1, y1, z1);
            p2 = new ScreenPoint(x2, y2, z2);
            p3 = new ScreenPoint(x3, y3, z3);
        }
    }
}