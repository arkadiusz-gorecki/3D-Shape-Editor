using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    class Cube: Shape
    {
        public Cube(Pen pen, float a)
        {
            a /= 2; // długość jednego boku
            this.pen = pen;
            //ściana North
            triangles.Add(new Triangle(-a, a, a,
                                       a, -a, a,
                                       -a, -a, a));
            triangles.Add(new Triangle(-a, a, a,
                                       a, a, a,
                                       a, -a, a));
            //ściana S
            triangles.Add(new Triangle(a, a, -a,
                                       -a, a, -a,
                                       a, -a, -a));
            triangles.Add(new Triangle(-a, a, -a,
                                       -a, -a, -a,
                                       a, -a, -a));
            //ściana E
            triangles.Add(new Triangle(-a, a, -a,
                                       -a, a, a,
                                       -a, -a, a));
            triangles.Add(new Triangle(-a, a, -a,
                                       -a, -a, a,
                                       -a, -a, -a));
            //ściana W
            triangles.Add(new Triangle(a, a, -a,
                                       a, -a, -a,
                                       a, -a, a));
            triangles.Add(new Triangle(a, a, -a,
                                       a, -a, a,
                                       a, a, a));
            //ściana Top
            triangles.Add(new Triangle(a, a, a,
                                       -a, a, a,
                                       -a, a, -a));
            triangles.Add(new Triangle(a, a, a,
                                       -a, a, -a,
                                       a, a, -a));
            //ściana Bottom
            triangles.Add(new Triangle(-a, -a, -a,
                                       -a, -a, a,
                                       a, -a, a));
            triangles.Add(new Triangle(-a, -a, -a,
                                       a, -a, -a,
                                       a, -a, -a));
        }
        public Cube(Pen pen, float size, float x, float y, float z): this(pen, size)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Cube(Pen pen, float size, float x, float y, float z, float xAngle, float yAngle, float zAngle) : this(pen, size, x, y, z)
        {
            this.xAngle = xAngle;
            this.yAngle = yAngle;
            this.zAngle = zAngle;
        }
        public Cube(Pen pen, float size, float x, float y, float z, float xAngle, float yAngle, float zAngle, float xScale, float yScale, float zScale) : this(pen, size, x, y, z, xAngle, yAngle, zAngle)
        {
            this.xScale = xScale;
            this.yScale = yScale;
            this.zScale = zScale;
        }
    }
}
