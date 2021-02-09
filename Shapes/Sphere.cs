using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    class Sphere: Shape
    {
        private Sphere(Color c, int parallelCount, int meridianCount)
        {
            edgeColor = c;
            float a = 1;

            Vertex v1, v2, v3, v4;
            float dx, dy, dz;
            for (int p = 0; p < parallelCount; p++)
            {
                float alpha1 = (float)Math.PI * ((float)(p) / parallelCount);
                float alpha2 = (float)Math.PI * ((float)(p + 1) / parallelCount);

                for (int m = 0; m < meridianCount; m++)
                {
                    float phi1 = 2 * (float)Math.PI * ((float)(m) / meridianCount);
                    float phi2 = 2 * (float)Math.PI * ((float)(m + 1) / meridianCount);

                    //phi2   phi1
                    // |      |
                    // 2------1 -- alpha1
                    // |\ _   |
                    // |    \ |
                    // 3------4 -- alpha2

                    dx = (float)(Math.Sin(alpha1) * Math.Cos(phi1));
                    dy = (float)(Math.Sin(alpha1) * Math.Sin(phi1));
                    dz = (float)(Math.Cos(alpha1));
                    v1 = new Vertex(dx, dy, dz);

                    dx = (float)(Math.Sin(alpha1) * Math.Cos(phi2));
                    dy = (float)(Math.Sin(alpha1) * Math.Sin(phi2));
                    v2 = new Vertex(dx, dy, dz);

                    dx = (float)(Math.Sin(alpha2) * Math.Cos(phi2));
                    dy = (float)(Math.Sin(alpha2) * Math.Sin(phi2));
                    dz = (float)(Math.Cos(alpha2));
                    v3 = new Vertex(dx, dy, dz);

                    dx = (float)(Math.Sin(alpha2) * Math.Cos(phi1));
                    dy = (float)(Math.Sin(alpha2) * Math.Sin(phi1));
                    v4 = new Vertex(dx, dy, dz);

                    if (p == 0) // górna część
                        triangles.Add(new Triangle(v1, v3, v4));
                    else if (p + 1 == parallelCount) // dolna część
                        triangles.Add(new Triangle(v3, v1, v2));
                    else // środkowe części kuli
                    {
                        triangles.Add(new Triangle(v1, v2, v3));
                        triangles.Add(new Triangle(v1, v3, v4));
                    }
                }
            }
        }

        public Sphere(Color c, float size, int parallelCount, int meridianCount, float x, float y, float z): this(c, parallelCount, meridianCount)
        {
            xScale *= size;
            yScale *= size;
            zScale *= size;
            this.x = x;
            this.y = y;
            this.z = z;
            UpdateModelMatrix();
        }
        public Sphere(Color c, float size, int parallelCount, int meridianCount, float x, float y, float z, float xAngle, float yAngle, float zAngle) : this(c, parallelCount, meridianCount)
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
        public Sphere(Color c, float size, int parallelCount, int meridianCount, float x, float y, float z, float xAngle, float yAngle, float zAngle, float xScale, float yScale, float zScale) : this(c, parallelCount, meridianCount)
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
