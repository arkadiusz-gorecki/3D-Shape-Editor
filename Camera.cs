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
        public float Fov { get; } = 90;// field of view
        public float Near { get; } = 1; // przednia płaszczyzna obcinania
        public float Far { get; } = 10;// tylnia płaszczyzna obcinania
        private float aspect = (float)16 / 9;
        public float Aspect { get => aspect; }
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
            this.aspect = aspect;
            ChangeProperties(P, T);
            UpdateProjectionMatrix();
        }
        public Camera(Vertex P, Vertex T, float aspect, float fov, float n, float f)
        {
            this.P = P;
            this.T = T;
            Fov = fov;
            Near = n;
            Far = f;
            this.aspect = aspect;
            ChangeProperties(P, T);
            UpdateProjectionMatrix();
        }

        public void ChangeProperties(Vertex p, Vertex t)
        {
            P = p;
            T = t;
            Vector3 Uworld = new Vector3(0, 1, 0);
            Vector3 D = new Vector3(P.X - T.X, P.Y - T.Y, P.Z - T.Z);
            D = Vector3.Normalize(D);
            Vector3 R = Vector3.Cross(Uworld, D);
            R = Vector3.Normalize(R);
            Vector3 U = Vector3.Cross(D, R);
            U = Vector3.Normalize(U);
            UpdateViewMatrix(D, R, U);
        }

        internal void ChangeAspect(float aspect)
        {
            this.aspect = aspect;
            UpdateProjectionMatrix();
        }

        private void ChangePosition(Vertex p)
        {
            ChangeProperties(p, this.T);
        }
        private void ChangeLookingPosition(Vertex t)
        {
            ChangeProperties(this.P, t);
        }
        internal void IncrementPosition()
        {
            // do debugowania
            //float stop = 10f;
            //T.Z += 0.03f;
            //if (T.Z >= stop)
            //    T.Z = stop;
            //ChangeLookingPosition(T);
            float stop = 10f;
            P.Z += 0.03f;
            if (P.Z >= stop)
                P.Z = stop;
            ChangePosition(P);
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
        public void UpdateProjectionMatrix()
        {
            projMatrix = new Matrix4x4(1 / ((float)Math.Tan(Fov / 2) * Aspect), 0, 0, 0,
                                        0, 1 / (float)Math.Tan(Fov / 2), 0, 0,
                                        0, 0, -(Far + Near) / (Far - Near), -(2 * Far * Near) / (Far - Near),
                                        0, 0, -1, 0);
        }
    }
}
