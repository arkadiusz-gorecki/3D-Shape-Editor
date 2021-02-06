using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    abstract class Shape
    {
        protected float x = 0; // pozycja
        protected float y = 0;
        protected float z = 0;
        protected float xAngle = 0; // kąt obrotu wokół osi x w radianach
        protected float yAngle = 0;
        protected float zAngle = 0;
        protected float xScale = 1; // skalowanie (dzięki niemu można z sześcianu otrzymać prostopadłościan)
        protected float yScale = 1;
        protected float zScale = 1;

        protected List<Triangle> triangles = new List<Triangle>(); // trójkąty reprezentujące figurę
        protected List<ScreenTriangle> screenSpaceTriangles = new List<ScreenTriangle>(); // ostateczne trójkąty w 2D do bezpośredniego rysowania

        protected Pen edgePen;

        private Matrix4x4 modelMatrix;
        public Matrix4x4 GetModelMatrix { get => modelMatrix; }

        public void ChangeProperties(float x, float y, float z, float xAngle, float yAngle, float zAngle, float xScale, float yScale, float zScale)
        {
            this.x = x; this.y = y; this.z = z;
            this.xAngle = xAngle; this.yAngle = yAngle; this.zAngle = zAngle;
            this.xScale = xScale; this.yScale = yScale; this.zScale = zScale;
            UpdateModelMatrix();
        }
        protected void UpdateModelMatrix()
        {
            Matrix4x4 T = GetTranslationMatrix(x, y, z);
            Matrix4x4 Rx = GetRotationXMatrix(xAngle);
            Matrix4x4 Ry = GetRotationYMatrix(yAngle);
            Matrix4x4 Rz = GetRotationZMatrix(zAngle);
            Matrix4x4 S = GetScalingMatrix(xScale, yScale, zScale);
            modelMatrix = T * Rx * Ry * Rz * S;
        }
        private Matrix4x4 GetTranslationMatrix(float x, float y, float z)
        {
            Matrix4x4 T = new Matrix4x4(1, 0, 0, x,
                                        0, 1, 0, y,
                                        0, 0, 1, z,
                                        0, 0, 0, 1);
            return T;
        }
        private Matrix4x4 GetRotationXMatrix(float alpha)
        {
            Matrix4x4 Ry = new Matrix4x4(1, 0, 0, 0,
                                        0, (float)Math.Cos(alpha), -(float)Math.Sin(alpha), 0,
                                        0, (float)Math.Sin(alpha), (float)Math.Cos(alpha), 0,
                                        0, 0, 0, 1);
            return Ry;
        }
        private Matrix4x4 GetRotationYMatrix(float alpha)
        {
            Matrix4x4 Ry = new Matrix4x4((float)Math.Cos(alpha), 0, -(float)Math.Sin(alpha), 0,
                                        0, 1, 0, 0,
                                        (float)Math.Sin(alpha), 0, (float)Math.Cos(alpha), 0,
                                        0, 0, 0, 1);
            return Ry;
        }
        private Matrix4x4 GetRotationZMatrix(float alpha)
        {
            Matrix4x4 Ry = new Matrix4x4((float)Math.Cos(alpha), -(float)Math.Sin(alpha), 0, 0,
                                        (float)Math.Sin(alpha), (float)Math.Cos(alpha), 0, 0,
                                        0, 0, 1, 0,
                                        0, 0, 0, 1);
            return Ry;
        }
        private Matrix4x4 GetScalingMatrix(float xScale, float yScale, float zScale)
        {
            Matrix4x4 S = new Matrix4x4(xScale, 0, 0, 0,
                                        0, yScale, 0, 0,
                                        0, 0, zScale, 0,
                                        0, 0, 0, 1);
            return S;
        }

        private void UpdateScreenSpaceTriangles(Pipeline pipeline)
        {
            screenSpaceTriangles.Clear();
            foreach (Triangle t in triangles)
                screenSpaceTriangles.AddRange(pipeline.ModelToScreen(t));
        }
     
        public void IncrementAngle()
        {
            // do debugowania
            xAngle += (float)(Math.PI / (4 * 36));
            xAngle = xAngle % (float)(2 * Math.PI); // żeby w nieskończonośc się nie zwiększało
            yAngle += (float)(Math.PI / (8 * 36));
            yAngle = yAngle % (float)(2 * Math.PI);
            //z += 0.02f;
            //if (z >= -1f)
            //    z = -1f;
            UpdateModelMatrix();
        }
    
        public void DrawEdges(Graphics g, Pipeline pipeline)
        {
            pipeline.modelMatrix = modelMatrix;
            pipeline.Update();
            UpdateScreenSpaceTriangles(pipeline);
            foreach (ScreenTriangle st in screenSpaceTriangles)
                st.DrawEdges(g, edgePen);
        }

    }
}
