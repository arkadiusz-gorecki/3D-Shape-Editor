using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    class Triangle
    {
        private Vertex v1;
        private Vertex v2;
        private Vertex v3;

        public Triangle(Vertex p1, Vertex p2, Vertex p3)
        {
            v1 = p1; v2 = p2; v3 = p3;
        }
        public Triangle(float x1, float y1, float z1,
                        float x2, float y2, float z2,
                        float x3, float y3, float z3)
        {
            v1 = new Vertex(x1, y1, z1);
            v2 = new Vertex(x2, y2, z2);
            v3 = new Vertex(x3, y3, z3);
        }

        private PointF GetVs(Vertex V, Matrix4x4 M)
        {
            // przekształć jeden punkt
            Vector4 Vc; // clipping space matrix
            Vc.X = M.M11 * V.X + M.M12 * V.Y + M.M13 * V.Z + M.M14 * V.W;
            Vc.Y = M.M21 * V.X + M.M22 * V.Y + M.M23 * V.Z + M.M24 * V.W;
            Vc.Z = M.M31 * V.X + M.M32 * V.Y + M.M33 * V.Z + M.M34 * V.W;
            Vc.W = M.M41 * V.X + M.M42 * V.Y + M.M43 * V.Z + M.M44 * V.W;
            Vector4 Vn = new Vector4(Vc.X / Vc.W,
                                     Vc.Y / Vc.W,
                                     Vc.Z / Vc.W,
                                     1);
            PointF Vs = new PointF((float)(0.5 * ShapeManager.width * (1 + Vn.X)),
                                     (float)(0.5 * ShapeManager.height * (1 - Vn.Y)));
            return Vs;
        }

        internal void DrawEdges(Graphics g, Pen pen, Matrix4x4 M)
        {
            PointF vs1 = GetVs(v1, M);
            PointF vs2 = GetVs(v2, M);
            PointF vs3 = GetVs(v3, M);
            g.DrawLine(pen, vs1, vs2);
            g.DrawLine(pen, vs2, vs3);
            g.DrawLine(pen, vs3, vs1);
        }
    }
}
