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
        public Vertex v1;
        public Vertex v2;
        public Vertex v3;

        public Triangle()
        {
            v1 = new Vertex();
            v2 = new Vertex();
            v3 = new Vertex();
        }

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
    }
}
