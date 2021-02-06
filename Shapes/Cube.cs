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
        private Cube(Color c)
        {
            edgePen = new Pen(c, 1);

            float a = 1;
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
            ////ściana Bottom
            triangles.Add(new Triangle(-a, -a, -a,
                                       -a, -a, a,
                                       a, -a, a));
            triangles.Add(new Triangle(-a, -a, -a,
                                       a, -a, a,
                                       a, -a, -a));
        }
        public Cube(Color c, float size, float x, float y, float z): this(c)
        {
            xScale *= size;
            yScale *= size;
            zScale *= size;
            this.x = x;
            this.y = y;
            this.z = z;
            UpdateModelMatrix();
        }
        public Cube(Color c, float size, float x, float y, float z, float xAngle, float yAngle, float zAngle) : this(c)
        {
            xScale *= size;
            yScale *= size;
            zScale *= size;
            this.x = x;
            this.y = y;
            this.z = z;
            this.xAngle = xAngle;
            this.yAngle = yAngle;
            this.zAngle = zAngle;
            UpdateModelMatrix();
        }
        public Cube(Color c, float size, float x, float y, float z, float xAngle, float yAngle, float zAngle, float xScale, float yScale, float zScale) : this(c)
        {
            xScale *= size;
            yScale *= size;
            zScale *= size;
            this.x = x;
            this.y = y;
            this.z = z;
            this.xAngle = xAngle;
            this.yAngle = yAngle;
            this.zAngle = zAngle;
            this.xScale = xScale;
            this.yScale = yScale;
            this.zScale = zScale;
            UpdateModelMatrix();
        }
    }
}
