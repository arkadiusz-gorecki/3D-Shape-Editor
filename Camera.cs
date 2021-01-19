using _3DShapeEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor
{
    class Camera
    {
        public float fov { get;} // field of view
        public float near { get;} // przednia płaszczyzna obcinania
        public float far { get; } // tylnia płaszczyzna obcinania
        private Vertex P; // pozycja kamery
        private Vertex T; // punkt na który skierowana jest kamera

        private Matrix4x4 viewMatrix;
        private Matrix4x4 projMatrix;
        public Matrix4x4 GetViewMatrix { get => viewMatrix;}
        public Matrix4x4 GetProjectionMatrix { get => projMatrix;}

        public Camera(Vertex P, Vertex T, float aspect)
        {
            this.P = P;
            this.T = T;
            fov = 90;
            near = 1;
            far = 10;
            ChangePosition(P, T);
            UpdateProjectionMatrix(aspect);
        }
        public Camera(Vertex P, Vertex T, float aspect, float fov, float n, float f) : this(P, T, aspect)
        {
            this.fov = fov;
            near = n;
            far = f;
        }
        public void ChangePosition(Vertex p, Vertex t)
        {
            P = p;
            T = t;
            Vector3 Uworld = new Vector3(0, 1, 0);
            Vector3 D = new Vector3(P.X - T.X, P.Y - T.Y, P.Z - T.Z);
            Vector3 R = Uworld * D;
            Vector3 U = D * R;
            D = Vector3.Normalize(D);
            R = Vector3.Normalize(R);
            U = Vector3.Normalize(U);
            UpdateViewMatrix(D, R, U);
        }
        private void UpdateViewMatrix(Vector3 D, Vector3 R, Vector3 U)
        {
            Matrix4x4 mat = new Matrix4x4(R.X, R.Y, R.Z, 0,
                                           U.X, U.Y, U.Z, 0,
                                           D.X, D.Y, D.Z, 0,
                                           0, 0, 0, 1);
            Matrix4x4 cam = new Matrix4x4(1, 0, 0, -P.X,
                                          0, 1, 0, -P.Y,
                                          0, 0, 1, -P.Z,
                                          0, 0, 0, 1);
            viewMatrix = mat * cam;
        }
        public void UpdateProjectionMatrix(float aspect)
        {
            projMatrix = new Matrix4x4(1 / ((float)Math.Tan(fov / 2) * aspect), 0, 0, 0,
                                        0, 1 / (float)Math.Tan(fov / 2), 0, 0,
                                        0, 0, (far + near) / (far - near), (-2 * far * near) / (far - near),
                                        0, 0, 1, 0);
        }
    }
}
