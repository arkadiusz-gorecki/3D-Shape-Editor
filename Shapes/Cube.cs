using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    class Cube: Shape
    {
        private Cube(Color c)
        {
            color = c;
            float ambient = 0.2f; // sumowanie do 1
            float diffuse = 0.3f;
            float specular = 0.5f;
            material = new Material(c, ambient, diffuse, specular);
            float a = 1;
            
            //ściana Z+
            triangles.Add(new Triangle(-a, a, a,
                                       a, -a, a,
                                       -a, -a, a,
                                       new Vector3(0, 0, 1)));
            triangles.Add(new Triangle(-a, a, a,
                                       a, a, a,
                                       a, -a, a,
                                       new Vector3(0, 0, 1)));
            //ściana Z-
            triangles.Add(new Triangle(a, a, -a,
                                       -a, a, -a,
                                       a, -a, -a,
                                       new Vector3(0, 0, -1)));
            triangles.Add(new Triangle(-a, a, -a,
                                       -a, -a, -a,
                                       a, -a, -a,
                                       new Vector3(0, 0, -1)));
            //ściana X-
            triangles.Add(new Triangle(-a, a, -a,
                                       -a, a, a,
                                       -a, -a, a,
                                       new Vector3(-1, 0, 0)));
            triangles.Add(new Triangle(-a, a, -a,
                                       -a, -a, a,
                                       -a, -a, -a,
                                       new Vector3(-1, 0, 0)));
            //ściana X+
            triangles.Add(new Triangle(a, a, -a,
                                       a, -a, -a,
                                       a, -a, a,
                                       new Vector3(1, 0, 0)));
            triangles.Add(new Triangle(a, a, -a,
                                       a, -a, a,
                                       a, a, a,
                                       new Vector3(1, 0, 0)));
            //ściana Y+
            triangles.Add(new Triangle(a, a, a,
                                       -a, a, a,
                                       -a, a, -a,
                                       new Vector3(0, 1, 0)));
            triangles.Add(new Triangle(a, a, a,
                                       -a, a, -a,
                                       a, a, -a,
                                       new Vector3(0, 1, 0)));
            //ściana Y-
            triangles.Add(new Triangle(-a, -a, -a,
                                       -a, -a, a,
                                       a, -a, a,
                                       new Vector3(0, -1, 0)));
            triangles.Add(new Triangle(-a, -a, -a,
                                       a, -a, a,
                                       a, -a, -a,
                                       new Vector3(0, -1, 0)));
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
