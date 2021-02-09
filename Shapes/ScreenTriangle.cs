using System;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace _3DShapeEditor
{
    internal class ScreenTriangle
    {
        public Point3D p1;
        public Point3D p2;
        public Point3D p3;

        public ScreenTriangle(Point3D p1, Point3D p2, Point3D p3)
        {
            this.p1 = p1; this.p2 = p2; this.p3 = p3;
        }
        public ScreenTriangle(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
        {
            p1 = new Point3D(x1, y1, z1);
            p2 = new Point3D(x2, y2, z2);
            p3 = new Point3D(x3, y3, z3);
        }
    }
}