using _3DShapeEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor
{
    class Pipeline
    {
        public Matrix4x4 modelMatrix;
        public Matrix4x4 viewMatrix;
        public Matrix4x4 projectionMatrix;
        public Matrix4x4 PVM;

        public int width; // wymiary rysowania, potrzebne do przejścia z ndc do screen
        public int height;

        public Pipeline() { }

        public void Update()
        {
            PVM = projectionMatrix * viewMatrix * modelMatrix;
        }
        public List<ScreenTriangle> ModelToScreen(Triangle t)
        {
            Triangle clippingTriangle = ModelToClipping(t);
            List<Triangle> clippedTriangle = ClipTriangle(clippingTriangle); // z jednego trójkąta może powstać wiele lub zero trójkątów
            List<ScreenTriangle> result = ClippedToScreen(clippedTriangle);
            return result;
        }

        private Triangle ModelToClipping(Triangle t)
        {
            // VC = PVM * V dla każdego wierzchołka trójkąta
            Vertex v = t.v1;
            Vertex vc1 = new Vertex();
            vc1.X = PVM.M11 * v.X + PVM.M12 * v.Y + PVM.M13 * v.Z + PVM.M14 * v.W;
            vc1.Y = PVM.M21 * v.X + PVM.M22 * v.Y + PVM.M23 * v.Z + PVM.M24 * v.W;
            vc1.Z = PVM.M31 * v.X + PVM.M32 * v.Y + PVM.M33 * v.Z + PVM.M34 * v.W;
            vc1.W = PVM.M41 * v.X + PVM.M42 * v.Y + PVM.M43 * v.Z + PVM.M44 * v.W;

            v = t.v2;
            Vertex vc2 = new Vertex();
            vc2.X = PVM.M11 * v.X + PVM.M12 * v.Y + PVM.M13 * v.Z + PVM.M14 * v.W;
            vc2.Y = PVM.M21 * v.X + PVM.M22 * v.Y + PVM.M23 * v.Z + PVM.M24 * v.W;
            vc2.Z = PVM.M31 * v.X + PVM.M32 * v.Y + PVM.M33 * v.Z + PVM.M34 * v.W;
            vc2.W = PVM.M41 * v.X + PVM.M42 * v.Y + PVM.M43 * v.Z + PVM.M44 * v.W;

            v = t.v3;
            Vertex vc3 = new Vertex();
            vc3.X = PVM.M11 * v.X + PVM.M12 * v.Y + PVM.M13 * v.Z + PVM.M14 * v.W;
            vc3.Y = PVM.M21 * v.X + PVM.M22 * v.Y + PVM.M23 * v.Z + PVM.M24 * v.W;
            vc3.Z = PVM.M31 * v.X + PVM.M32 * v.Y + PVM.M33 * v.Z + PVM.M34 * v.W;
            vc3.W = PVM.M41 * v.X + PVM.M42 * v.Y + PVM.M43 * v.Z + PVM.M44 * v.W;

            vc1.Normalize();
            vc2.Normalize();
            vc3.Normalize();

            return new Triangle(vc1, vc2, vc3);
        }

        private List<Triangle> ClipTriangle(Triangle t)
        {
            List<Triangle> result = new List<Triangle>(); // nowe trójkąty powstałe z tego jednego trójkąta (może być ich zero jeśli obcianny trójkąt poza kostką kamery)
            List<Vertex> vertices = new List<Vertex>();
            List<Vertex> newVertices = new List<Vertex>();
            vertices.Add(t.v1);
            vertices.Add(t.v2);
            vertices.Add(t.v3);

            //foreach płaszczyzna; foreach krawędź
            for (int planeNumber = 1; planeNumber <= 6; planeNumber++)
            {
                for (int i = 0; i < vertices.Count - 1; i++)
                    ClipEdge(vertices[i], vertices[i + 1], planeNumber, newVertices);
                if (vertices.Count != 0) // ostatni z pierwszym
                    ClipEdge(vertices[vertices.Count - 1], vertices[0], planeNumber, newVertices);

                vertices = newVertices;
                newVertices = new List<Vertex>();
            }
            for (int i = 0; i < vertices.Count - 2; i++) // twórz trójkaty dla każdej trójki wierzchołków
                result.Add(new Triangle(vertices[0], vertices[i + 1], vertices[i + 2]));

            return result;
        }
        private void ClipEdge(Vertex A, Vertex B, int planeNumber, List<Vertex> newVertices)
        {
            // dodaje nowe wierzchołki do listy nowych wierzchołków reprezentujących obcinany trójkąt
            float dA = Distance(A, planeNumber);
            float dB = Distance(B, planeNumber);
            float dC = dA / (dA - dB);

            if (dA > 0)
            {
                if (dB > 0) // oba punkty wewnątrz
                    newVertices.Add(B);
                else
                {
                    Vertex C = new Vertex(A.X * (1 - dC) + B.X * dC, A.Y * (1 - dC) + B.Y * dC, A.Z * (1 - dC) + B.Z * dC);
                    newVertices.Add(C);
                }
            }
            else
            {
                if (dB > 0)
                {
                    Vertex C = new Vertex(A.X * (1 - dC) + B.X * dC, A.Y * (1 - dC) + B.Y * dC, A.Z * (1 - dC) + B.Z * dC);
                    newVertices.Add(C);
                    newVertices.Add(B);
                }
                else // oba punkty na zewnątrz
                { }
            }
        }
        private float Distance(Vertex v, int planeNumber)
        {
            switch (planeNumber)
            {
                case 1:
                    return v.W - v.X;
                case 2:
                    return v.W + v.X;
                case 3:
                    return v.W - v.Y;
                case 4:
                    return v.W + v.Y;
                case 5:
                    return v.W - v.Z;
                case 6:
                    return v.W + v.Z;
                default:
                    throw new ArgumentException();
            }
        }

        private List<ScreenTriangle> ClippedToScreen(List<Triangle> clippedTriangle)
        {
            List<ScreenTriangle> result = new List<ScreenTriangle>();
            foreach (Triangle t in clippedTriangle)
                result.Add(new ScreenTriangle(ClippedToScreen(t.v1),
                                              ClippedToScreen(t.v2),
                                              ClippedToScreen(t.v3)));
            return result;
        }
        private PointF ClippedToScreen(Vertex Vc)
        {
            // przekształć jeden punkt clipping space 4D na screen space 2D
            Vc.Normalize(); // zmiana na NDC
            PointF Vs = new PointF((float)(0.5f * (float)width * (1 + Vc.X)),
                                   (float)(0.5f * (float)height * (1 - Vc.Y)));
            return Vs;
        }
    }
}
