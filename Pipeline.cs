using _3DShapeEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;

namespace _3DShapeEditor
{
    class Pipeline
    {
        private Matrix4x4 modelMatrix = new Matrix4x4();
        private Matrix4x4 viewMatrix = new Matrix4x4();
        private Matrix4x4 projectionMatrix = new Matrix4x4();
        private Matrix4x4 PVM;

        private int width; // wymiary rysowania, potrzebne do przejścia z ndc do screen
        private int height;
        private float[][] depthBuffer;

        private bool backfaceCullingToggled = true;

        public void SetModelMatrix(Matrix4x4 m)
        {
            modelMatrix = new Matrix4x4(m.M11, m.M12, m.M13, m.M14,
                                        m.M21, m.M22, m.M23, m.M24,
                                        m.M31, m.M32, m.M33, m.M34,
                                        m.M41, m.M42, m.M43, m.M44);
        }
        public void SetViewMatrix(Matrix4x4 m)
        {
            viewMatrix = new Matrix4x4(m.M11, m.M12, m.M13, m.M14,
                                        m.M21, m.M22, m.M23, m.M24,
                                        m.M31, m.M32, m.M33, m.M34,
                                        m.M41, m.M42, m.M43, m.M44);
        }
        public void SetProjectionMatrix(Matrix4x4 m)
        {
            projectionMatrix = new Matrix4x4(m.M11, m.M12, m.M13, m.M14,
                                        m.M21, m.M22, m.M23, m.M24,
                                        m.M31, m.M32, m.M33, m.M34,
                                        m.M41, m.M42, m.M43, m.M44);
        }
        public void SetScreenDimensions(int w, int h)
        {
            width = w;
            height = h;
        }
        public void EnableBackfaceCulling(bool b)
        {
            backfaceCullingToggled = b;
        }

        public Pipeline(Matrix4x4 view, Matrix4x4 proj, int w, int h) 
        {
            viewMatrix = view;
            projectionMatrix = proj;
            width = w;
            height = h;

            InitializeDepthBuffer();
        }

        public void ClearDepthBuffer()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    depthBuffer[i][j] = 2;
        }

        public void UpdateMatrices()
        {
            PVM = projectionMatrix * viewMatrix * modelMatrix;
        }

        public void InitializeDepthBuffer()
        {
            depthBuffer = new float[width][];
            for (int i = 0; i < width; i++)
                depthBuffer[i] = new float[height];
        }

        public void DrawTriangleEdges(Bitmap bitmap, Triangle t, Color color)
        {
            List<ScreenTriangle> screenTriangle = ModelToScreen(t);
            if (backfaceCullingToggled && screenTriangle.Count != 0 && FacingBackwards(screenTriangle))
                return;
            foreach (ScreenTriangle st in screenTriangle)
                Rasterize(bitmap, st, color);
        }
        private List<ScreenTriangle> ModelToScreen(Triangle t)
        {
            Triangle clippingTriangle = ModelToClipping(t);
            List<Triangle> clippedTriangle = ClipTriangle(clippingTriangle); // z jednego trójkąta może powstać wiele lub zero trójkątów
            List<ScreenTriangle> result = ClippedToScreen(clippedTriangle);

            return result;
        }

        private bool FacingBackwards(List<ScreenTriangle> screenTriangle)
        {
            // albo cała listę odrzuca albo cała listę rysuje
            // wszystkie trójkąty mają takie samo ustawienie w prezstrzenii więc wystarczy że jeden obliczę i wtedy wszystko można odrzucić
            ScreenTriangle st = screenTriangle[0];
            Point3D A = st.p1;
            Point3D B = st.p2;
            Point3D C = st.p3;
            Vector2 AB = new Vector2(B.X - A.X, B.Y - A.Y);
            Vector2 AC = new Vector2(C.X - A.X, C.Y - A.Y);

            // obliczam sinus kąta pomiędzy wektorem AB a AC, ujemny oznacza że patrząc z A w stronę B punkt C znajduje się po prawej stronie
            float sine = (AB.X * AC.Y - AB.Y * AC.X);// / (AB.Length() * AC.Length()); // dzielenie niepotrzebne bo sprawdzam tylko znak

            if(sine == 0) // wszystkie trzy punkty trójkąta w jednej linii
                return true; // nie rysuj
            else if(sine > 0)
                return false;
            else
                return true;
        }

        private Triangle ModelToClipping(Triangle t)
        {
            // VC = PVM * V dla każdego wierzchołka trójkąta
            Vertex vc1 = MatrixOperations.Transform(PVM, t.v1);
            Vertex vc2 = MatrixOperations.Transform(PVM, t.v2);
            Vertex vc3 = MatrixOperations.Transform(PVM, t.v3);

            //vc1.Normalize();
            //vc2.Normalize();
            //vc3.Normalize();

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
        private Point3D ClippedToScreen(Vertex Vc)
        {
            // przekształć jeden punkt clipping space 4D na screen space 2D z głębokością pikseli
            Vc.Normalize(); // zmiana na NDC
            Point3D Vs = new Point3D((float)(0.5f * width * (1 + Vc.X)),
                                     (float)(0.5f * height * (1 - Vc.Y)),
                                     (float)(0.5f * (Vc.Z + 1)));
            return Vs;
        }


        private void Rasterize(Bitmap bitmap, ScreenTriangle st, Color color)
        {
            MidpointLine(bitmap, st.p1, st.p2, color);
            MidpointLine(bitmap, st.p2, st.p3, color);
            MidpointLine(bitmap, st.p3, st.p1, color);
        }
        private void MidpointLine(Bitmap bitmap, Point3D P1, Point3D P2, Color color)
        {
            Point3D p1 = new Point3D(P1.X, P1.Y, P1.Z);
            Point3D p2 = new Point3D(P2.X, P2.Y, P2.Z);
            if (p2.X < p1.X) // czyli jeśli dx < 0 to symetryczny przypadek
            {
                // zamiana punktów miejscami
                Point3D tmp = p1;
                p1 = p2;
                p2 = tmp;
            }

            int dx = (int)Math.Round(p2.X - p1.X);
            int dy = (int)Math.Round(p2.Y - p1.Y);
            int x = (int)Math.Round(p1.X);
            int y = (int)Math.Round(p1.Y);
            float q;
            float z; // depth
            SetPixel(bitmap, x, y, p1.Z, color);
            int fm; // F(m)

            int ychange = dy < 0 ? -1 : 1; // zależnie w którym kierunku od p1 do p2
            int sign = ychange; // znak w while
            dy = Math.Abs(dy);


            if (dx >= Math.Abs(dy)) // 3 2
            {
                fm = 2 * dy - dx; // F(m);
                while (x < p2.X)
                {
                    if (fm >= 0)
                    {
                        fm -= 2 * dx;
                        y += ychange;
                    }
                    fm += 2 * dy;
                    x++;

                    q = Math.Abs(x - p1.X) / dx;
                    z = p1.Z * (1 - q) + p2.Z * q;

                    SetPixel(bitmap, x, y, z, color);
                }
            }
            else // 1 4
            {
                fm = 2 * dx - dy; // F(m)
                while (sign * y < sign * p2.Y)
                {
                    if (fm >= 0)
                    {
                        fm -= 2 * dy;
                        x++;
                    }
                    fm += 2 * dx;
                    y += ychange;

                    q = Math.Abs(y - p1.Y) / dy;
                    z = p1.Z * (1 - q) + p2.Z * q;

                    SetPixel(bitmap, x, y, z, color);
                }
            }
        }
        private void SetPixel(Bitmap bitmap, int x, int y, float z, Color c)
        {
           
            if (0 <= x && x < width && 0 <= y && y < height && z < depthBuffer[x][y])
            {
                bitmap.SetPixel(x, y, c);
                depthBuffer[x][y] = z;
            }
        }
        

    }
}
