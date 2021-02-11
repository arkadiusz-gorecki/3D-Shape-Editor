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
        public bool flatShading = false; // czy ten trójkąt ma być rysowany algorytmem flat shading czy phong shading

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
        public Triangle(Vertex p1, Vertex p2, Vertex p3, bool flatShading)
        {
            v1 = p1; v2 = p2; v3 = p3;
            this.flatShading = flatShading;
        }
        public Triangle(float x1, float y1, float z1,
                        float x2, float y2, float z2,
                        float x3, float y3, float z3)
        {
            v1 = new Vertex(x1, y1, z1);
            v2 = new Vertex(x2, y2, z2);
            v3 = new Vertex(x3, y3, z3);
        }
        public Triangle(float x1, float y1, float z1,
                        float x2, float y2, float z2,
                        float x3, float y3, float z3,
                        Vector3 normal)
        {
            v1 = new Vertex(x1, y1, z1, normal);
            v2 = new Vertex(x2, y2, z2, normal);
            v3 = new Vertex(x3, y3, z3, normal);
        }
        public Triangle(float x1, float y1, float z1,
                        float x2, float y2, float z2,
                        float x3, float y3, float z3,
                        Vector3 normal, bool flatShading)
        {
            v1 = new Vertex(x1, y1, z1, normal);
            v2 = new Vertex(x2, y2, z2, normal);
            v3 = new Vertex(x3, y3, z3, normal);
            this.flatShading = flatShading;
        }
    }
}
