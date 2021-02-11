using _3DShapeEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor
{
    public class MatrixOperations
    {
        public static Vertex Transform(Matrix4x4 M, Vertex V)
        { 
            // iloczyn macierzy i wektora(wierzchołka)
            Vertex result = new Vertex();
            result.X = M.M11 * V.X + M.M12 * V.Y + M.M13 * V.Z + M.M14 * V.W;
            result.Y = M.M21 * V.X + M.M22 * V.Y + M.M23 * V.Z + M.M24 * V.W;
            result.Z = M.M31 * V.X + M.M32 * V.Y + M.M33 * V.Z + M.M34 * V.W;
            result.W = M.M41 * V.X + M.M42 * V.Y + M.M43 * V.Z + M.M44 * V.W;
            return result;
        }
        public static Vector3 Transform(Matrix4x4 M, Vector3 V)
        {
            // iloczyn macierzy i wektora(wierzchołka)
            Vector3 result = new Vector3();
            result.X = M.M11 * V.X + M.M12 * V.Y + M.M13 * V.Z;
            result.Y = M.M21 * V.X + M.M22 * V.Y + M.M23 * V.Z;
            result.Z = M.M31 * V.X + M.M32 * V.Y + M.M33 * V.Z;
            return result;
        }
        public static Matrix4x4 GetTranslationMatrix(float x, float y, float z)
        {
            return new Matrix4x4(1, 0, 0, x,
                                 0, 1, 0, y,
                                 0, 0, 1, z,
                                 0, 0, 0, 1);
        }
        public static Matrix4x4 GetRotationXMatrix(float alpha)
        {
            return new Matrix4x4(1, 0, 0, 0,
                                 0, (float)Math.Cos(alpha), -(float)Math.Sin(alpha), 0,
                                 0, (float)Math.Sin(alpha), (float)Math.Cos(alpha), 0,
                                 0, 0, 0, 1);
        }
        public static Matrix4x4 GetRotationYMatrix(float alpha)
        {
            return new Matrix4x4((float)Math.Cos(alpha), 0, -(float)Math.Sin(alpha), 0,
                                 0, 1, 0, 0,
                                 (float)Math.Sin(alpha), 0, (float)Math.Cos(alpha), 0,
                                 0, 0, 0, 1);
        }
        public static Matrix4x4 GetRotationZMatrix(float alpha)
        {
            return new Matrix4x4((float)Math.Cos(alpha), -(float)Math.Sin(alpha), 0, 0,
                                 (float)Math.Sin(alpha), (float)Math.Cos(alpha), 0, 0,
                                 0, 0, 1, 0,
                                 0, 0, 0, 1);
        }
        public static Matrix4x4 GetScalingMatrix(float xScale, float yScale, float zScale)
        {
            return new Matrix4x4(xScale, 0, 0, 0,
                                 0, yScale, 0, 0,
                                 0, 0, zScale, 0,
                                 0, 0, 0, 1);
        }

        //public static void Normalize(Vector3 A)
        //{
        //    float len = (float)Math.Sqrt(A.X * A.X + A.Y * A.Y + A.Z * A.Z);
        //    if (len == 1 || len == 0)
        //        return;
        //    A.X /= len;
        //    A.Y /= len;
        //    A.Z /= len;
        //}
    }
}
