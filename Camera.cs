using _3DShapeEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _3DShapeEditor
{
    public class Camera: MatrixOperations
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

        private float cameraMovementSpeed = 0.2f;
        private float cameraRotationSpeed = 0.01f;
        private bool mouseHolding;
        private Point lastMousePosition;
        private Point currentMousePosition;

        public Matrix4x4 GetViewMatrix { get => viewMatrix;}
        public Matrix4x4 GetProjectionMatrix { get => projMatrix; }

        public void SetMouseHolding(bool holding, Point mousePosition)
        {
            if (mouseHolding = holding)
                lastMousePosition = mousePosition;
        }
        public void SetMousePosition(Point mousePosition)
        {
            currentMousePosition = mousePosition;
        }

        public Camera(Vertex P, Vertex T, float aspect, Point mousePosition)
        {
            this.P = P;
            this.T = T;
            this.aspect = aspect;
            currentMousePosition = mousePosition;
            ChangeProperties(P, T);
            UpdateProjectionMatrix();
        }
        public Camera(Vertex P, Vertex T, float aspect, float fov, float n, float f, Point mousePosition)
        {
            this.P = P;
            this.T = T;
            Fov = fov;
            Near = n;
            Far = f;
            this.aspect = aspect;
            currentMousePosition = mousePosition;
            ChangeProperties(P, T);
            UpdateProjectionMatrix();
        }

        public void Move()
        {
            MovePosition();
            Rotate();
            UpdateViewMatrix();
        }
        private void MovePosition()
        {
            // przemieszczenie punktów P i T
            float dx = P.X - T.X;
            float dz = P.Z - T.Z;
            float dy = P.Y - T.Y;
            float len = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
            // podział prędkości kamery pomiędzy każdą współrzędną
            float xCam = cameraMovementSpeed * dx / len;
            float yCam = cameraMovementSpeed * dy / len;
            float zCam = cameraMovementSpeed * dz / len;

            if (Keyboard.IsKeyDown(Constants.KEY_FORWARD_CAMERA))
            {
                P.X -= xCam; P.Y -= yCam; P.Z -= zCam;
                T.X -= xCam; T.Y -= yCam; T.Z -= zCam;
                return; // żeby nie było sytuacji że kilka przycisków na raz można przycisnąć (oznacza to że nie da się iść na ukos)
            }
            if (Keyboard.IsKeyDown(Constants.KEY_BACKWARD_CAMERA))
            {
                P.X += xCam; P.Y += yCam; P.Z += zCam;
                T.X += xCam; T.Y += yCam; T.Z += zCam;
                return;
            }

            // podział prędkości tylko pomiędzy x i z bo strafe porusza tylko wzdłuż płaszczyzny XZ (bo nie ma w programie obracania kamery (roll))
            len = (float)Math.Sqrt(dx * dx + dz * dz);
            xCam = cameraMovementSpeed * dx / len;
            zCam = cameraMovementSpeed * dz / len;
            if (Keyboard.IsKeyDown(Constants.KEY_STRAFELEFT_CAMERA))
            {
                P.X -= zCam; P.Z += xCam;
                T.X -= zCam; T.Z += xCam;
            }
            else if (Keyboard.IsKeyDown(Constants.KEY_STRAFERIGHT_CAMERA))
            {
                P.X += zCam; P.Z -= xCam;
                T.X += zCam; T.Z -= xCam;
            }

        }
        private void Rotate()
        {
            if (!mouseHolding)
                return;

            float dx = currentMousePosition.X - lastMousePosition.X;
            float dy = currentMousePosition.Y - lastMousePosition.Y;
            lastMousePosition.X = currentMousePosition.X;
            lastMousePosition.Y = currentMousePosition.Y;

            float len = (float)Math.Sqrt((dx * dx) + (dy * dy));
            if (len == 0)
                return;

            float yawAngle = cameraRotationSpeed * (dx); // obracanie kamery na boki
            float pitchAngle = -cameraRotationSpeed * (dy); // obracanie kamery do góry do dołu

            RotateYawn(yawAngle);
            RotatePitch(pitchAngle);
        }
        private void RotatePitch(float pitchAngle)
        {
            Matrix4x4 T1 = GetTranslationMatrix(-P.X, -P.Y, -P.Z);
            Matrix4x4 T2 = GetTranslationMatrix(P.X, P.Y, P.Z);

            T = Transform(T1, T);
            float alignToXYPlaneAngle = -(float)Math.Atan2(T.Z, T.X);

            Matrix4x4 Ry1 = GetRotationYMatrix(alignToXYPlaneAngle);
            T = Transform(Ry1, T);

            float angleRemaining = (float)Math.Abs(Math.Atan(T.X / T.Y)); // jak dużo możemy jeszcze obrócić kamerę do góry (lub do dołu) żeby nie przekręcić jej "za siebie"

            if (pitchAngle > 0 && T.Y > 0 && pitchAngle > angleRemaining) // do góry obracasz kamerę
            {
                if (angleRemaining < (Math.PI / 180) * 1) // jeden stopień w radianach (nie można bardziej pionowo niż 89 stopni)
                    pitchAngle = 0;
                else
                    pitchAngle = angleRemaining - (float)(Math.PI / 180) * 1;
            }
            else if (pitchAngle < 0 && T.Y < 0 && -pitchAngle > angleRemaining) // do dołu obracasz kamerę
            {
                if (angleRemaining < (Math.PI / 180) * 1)
                    pitchAngle = 0;
                else
                    pitchAngle = -angleRemaining + (float)(Math.PI / 180) * 1;
            }
            Matrix4x4 Rz = GetRotationZMatrix(pitchAngle);
            Matrix4x4 Ry2 = GetRotationYMatrix(-alignToXYPlaneAngle);

            T = Transform(T2 * Ry2 * Rz, T);
        }
        private void RotateYawn(float yawAngle)
        {
            Matrix4x4 T1 = GetTranslationMatrix(-P.X, -P.Y, -P.Z);
            Matrix4x4 Ry = GetRotationYMatrix(yawAngle);
            Matrix4x4 T2 = GetTranslationMatrix(P.X, P.Y, P.Z);
            Matrix4x4 TRT = T2 * Ry * T1;
            T = Transform(TRT, T);
        }

        private void Rotate_FastAlgorithm()
        {
            if (!mouseHolding)
                return;

            float dx = currentMousePosition.X - lastMousePosition.X;
            float dy = currentMousePosition.Y - lastMousePosition.Y;
            lastMousePosition.X = currentMousePosition.X;
            lastMousePosition.Y = currentMousePosition.Y;

            float len = (float)Math.Sqrt((dx * dx) + (dy * dy));
            if (len == 0)
                return;

            float yawAngle = cameraRotationSpeed * (dx); // obracanie kamery na boki
            float pitchAngle = -cameraRotationSpeed * (dy); // obracanie kamery do góry do dołu

            Matrix4x4 T1 = GetTranslationMatrix(-P.X, -P.Y, -P.Z);
            T = Transform(T1, T);

            float alignToXYPlaneAngle = -(float)Math.Atan2(T.Z, T.X);

            Matrix4x4 Ry1 = GetRotationYMatrix(alignToXYPlaneAngle);
            T = Transform(Ry1, T);

            float angleRemaining = (float)Math.Abs(Math.Atan(T.X / T.Y)); // jak dużo możemy jeszcze obrócić kamerę do góry żeby nie przekręcić jej "za siebie" (lub do dołu)

            if (pitchAngle > 0 && T.Y > 0 && pitchAngle > angleRemaining) // do góry przekręcasz kamerę
            {
                if (angleRemaining < (Math.PI / 180) * 1) // jeden stopień w radianach (nie można bardziej pionowo niż 89 stopni)
                    pitchAngle = 0;
                else
                    pitchAngle = angleRemaining - (float)(Math.PI / 180) * 1;
            }
            else if (pitchAngle < 0 && T.Y < 0 && -pitchAngle > angleRemaining) // do dołu
            {
                if (angleRemaining < (Math.PI / 180) * 1) // jeden stopień w radianach
                    pitchAngle = 0;
                else
                    pitchAngle = -angleRemaining + (float)(Math.PI / 180) * 1;
            }

            Matrix4x4 Rz = GetRotationZMatrix(pitchAngle);
            Matrix4x4 Ry2 = GetRotationYMatrix(-alignToXYPlaneAngle);
            Matrix4x4 Ry = GetRotationYMatrix(yawAngle);
            Matrix4x4 T2 = GetTranslationMatrix(P.X, P.Y, P.Z);

            T = Transform(T2 * Ry * Ry2 * Rz, T);
        }
        public void ChangeProperties(Vertex P, Vertex T)
        {
            this.P = P;
            this.T = T;
            UpdateViewMatrix();
        }
        public void ChangeAspect(float aspect)
        {
            this.aspect = aspect;
            UpdateProjectionMatrix();
        }
        private void UpdateViewMatrix()
        {
            Vector3 Uworld = new Vector3(0, 1, 0);
            Vector3 D = new Vector3(P.X - T.X, P.Y - T.Y, P.Z - T.Z);
            D = Vector3.Normalize(D);
            Vector3 R = Vector3.Cross(Uworld, D);
            R = Vector3.Normalize(R);
            Vector3 U = Vector3.Cross(D, R);
            U = Vector3.Normalize(U);
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
