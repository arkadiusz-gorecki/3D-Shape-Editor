using System.Drawing;

namespace _3DShapeEditor
{
    internal class ScreenTriangle
    {
        public PointF p1;
        public PointF p2;
        public PointF p3;

        public ScreenTriangle(PointF p1, PointF p2, PointF p3)
        {
            this.p1 = p1; this.p2 = p2; this.p3 = p3;
        }
        public ScreenTriangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            p1 = new PointF(x1, y1);
            p2 = new PointF(x2, y2);
            p3 = new PointF(x3, y3);
        }

        internal void DrawEdges(Graphics g, Pen pen)
        {
            g.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
            g.DrawLine(pen, p2.X, p2.Y, p3.X, p3.Y);
            g.DrawLine(pen, p3.X, p3.Y, p1.X, p1.Y);
        }

    }
}