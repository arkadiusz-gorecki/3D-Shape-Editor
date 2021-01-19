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
        protected List<Triangle> triangles = new List<Triangle>();
        protected Pen pen;
        public void DrawEdges(Graphics g, Camera cam)
        {
            Matrix4x4 M = GetM(cam);
            foreach(Triangle tri in triangles)
            {
                tri.DrawEdges(g, pen, M);
            }
        }

        public void IncrementAngle()
        {
            xAngle += (float)(Math.PI / (4*36));
            xAngle = xAngle % (float)(2 * Math.PI); // żeby w nieskończonośc się nie zwiększało
            yAngle += (float)(Math.PI / (8*36));
            yAngle = yAngle % (float)(2 * Math.PI);
        }
        private Matrix4x4 GetM(Camera cam)
        {
            Matrix4x4 T = GetTranslationMatrix(x, y, z);
            Matrix4x4 Rx = GetRotationXMatrix(xAngle);
            Matrix4x4 Ry = GetRotationYMatrix(yAngle);
            Matrix4x4 Rz = GetRotationZMatrix(xAngle);
            Matrix4x4 P = cam.GetProjectionMatrix;
            Matrix4x4 S = GetScalingMatrix();
            Matrix4x4 M = P * T * Rx * Ry * Rz * S;
            return M;
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
        private Matrix4x4 GetScalingMatrix()
        {
            Matrix4x4 S = new Matrix4x4(xScale, 0, 0, 0,
                                        0, yScale, 0, 0,
                                        0, 0, zScale, 0,
                                        0, 0, 0, 1);
            return S;
        }
    }
}
