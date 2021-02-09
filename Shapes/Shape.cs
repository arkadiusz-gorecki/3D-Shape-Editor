using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    abstract class Shape: MatrixOperations
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

        protected Color edgeColor;

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
    
        public void DrawEdges(Bitmap bitmap, Pipeline pipeline)
        {
            pipeline.SetModelMatrix(modelMatrix);
            pipeline.UpdateMatrices();

            foreach (Triangle t in triangles)
                pipeline.DrawTriangleEdges(bitmap, t, edgeColor);
        }

    }
}
