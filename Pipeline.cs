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
        private Matrix4x4 PV;
        private Matrix4x4 MTransInv; // model matrix transposed inverted (dla wektorów normalnych0

        private int width; // wymiary rysowania, potrzebne do przejścia z ndc do screen
        private int height;
        private float[][] depthBuffer;

        private bool backfaceCullingToggled = true;
        private bool zBufferingToggled = true;
        private bool drawingEdges = true;
        private bool shading = false;

        private Material currentShapeMaterial;
        private List<Light> lights;
        private Camera currentCamera;

        public void SetModelMatrix(Matrix4x4 m)
        {
            modelMatrix = new Matrix4x4(m.M11, m.M12, m.M13, m.M14,
                                        m.M21, m.M22, m.M23, m.M24,
                                        m.M31, m.M32, m.M33, m.M34,
                                        m.M41, m.M42, m.M43, m.M44);
        }
        public void AddLight(Light light)
        {
            lights.Add(light);
        }
        public void SetCamera(Camera camera)
        {
            currentCamera = camera;
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
        public void EnableZBuffering(bool b)
        {
            zBufferingToggled = b;
        }
        public void EnableDrawingEdges(bool b)
        {
            drawingEdges = b;
        }
        public void EnableShading(bool b)
        {
            shading = b;
        }

        public Pipeline(Matrix4x4 view, Matrix4x4 proj, int w, int h)
        {
            viewMatrix = view;
            projectionMatrix = proj;
            width = w;
            height = h;
            lights = new List<Light>();
            InitializeDepthBuffer();
        }

        public void InitializeDepthBuffer()
        {
            depthBuffer = new float[width][];
            for (int i = 0; i < width; i++)
                depthBuffer[i] = new float[height];
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
            PV = projectionMatrix * viewMatrix;
            if (!Matrix4x4.Invert(Matrix4x4.Transpose(modelMatrix), out MTransInv))
                throw new ArgumentException();
        }

        public void DrawShape(Bitmap bitmap, Shape shape)
        {
            currentShapeMaterial = shape.GetMaterial();
            SetModelMatrix(shape.ModelMatrix);
            UpdateMatrices();
            foreach (Triangle t in shape.Triangles)
                DrawTriangle(bitmap, t, shape.Color);
        }
        public void DrawTriangle(Bitmap bitmap, Triangle t, Color color)
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
        private Triangle ModelToClipping(Triangle t)
        {
            // VC = PVM * V dla każdego wierzchołka trójkąta
            Vertex vw1 = MatrixOperations.Transform(modelMatrix, t.v1);
            Vertex vw2 = MatrixOperations.Transform(modelMatrix, t.v2);
            Vertex vw3 = MatrixOperations.Transform(modelMatrix, t.v3);
            vw1.normal = MatrixOperations.Transform(MTransInv, t.v1.normal);
            vw2.normal = MatrixOperations.Transform(MTransInv, t.v2.normal);
            vw3.normal = MatrixOperations.Transform(MTransInv, t.v3.normal);
            vw1.normal = Vector3.Normalize(vw1.normal);
            vw2.normal = Vector3.Normalize(vw2.normal);
            vw3.normal = Vector3.Normalize(vw3.normal);

            Vertex vc1 = MatrixOperations.Transform(PV, vw1);
            Vertex vc2 = MatrixOperations.Transform(PV, vw2);
            Vertex vc3 = MatrixOperations.Transform(PV, vw3);
            vc1.worldPosition = new Vector4(vw1.X, vw1.Y, vw1.Z, vw1.W);
            vc2.worldPosition = new Vector4(vw2.X, vw2.Y, vw2.Z, vw2.W);
            vc3.worldPosition = new Vector4(vw3.X, vw3.Y, vw3.Z, vw3.W);
            vc1.normal = new Vector3(vw1.normal.X, vw1.normal.Y, vw1.normal.Z/*, vw1.normal.W*/);
            vc2.normal = new Vector3(vw2.normal.X, vw2.normal.Y, vw2.normal.Z/*, vw2.normal.W*/);
            vc3.normal = new Vector3(vw3.normal.X, vw3.normal.Y, vw3.normal.Z/*, vw3.normal.W*/);

            return new Triangle(vc1, vc2, vc3, t.flatShading);
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
                result.Add(new Triangle(vertices[0], vertices[i + 1], vertices[i + 2], t.flatShading));

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
                    float worldX = A.worldPosition.X + (B.worldPosition.X - A.worldPosition.X) * dC;
                    float worldY = A.worldPosition.Y + (B.worldPosition.Y - A.worldPosition.Y) * dC;
                    float worldZ = A.worldPosition.Z + (B.worldPosition.Z - A.worldPosition.Z) * dC;
                    float worldW = A.worldPosition.W + (B.worldPosition.W - A.worldPosition.W) * dC;
                    float normalX = A.normal.X + (B.normal.X - A.normal.X) * dC;
                    float normalY = A.normal.Y + (B.normal.Y - A.normal.Y) * dC;
                    float normalZ = A.normal.Z + (B.normal.Z - A.normal.Z) * dC;
                    //float normalW = A.normal.W + (B.normal.W - A.normal.W) * dC;
                    Vertex C = new Vertex(A.X * (1 - dC) + B.X * dC, A.Y * (1 - dC) + B.Y * dC, A.Z * (1 - dC) + B.Z * dC, A.W * (1 - dC) + B.W * dC);
                    C.worldPosition = new Vector4(worldX, worldY, worldZ, worldW);
                    C.normal = Vector3.Normalize(new Vector3(normalX, normalY, normalZ));

                    newVertices.Add(C);
                }
            }
            else
            {
                if (dB > 0)
                {
                    float worldX = A.worldPosition.X + (B.worldPosition.X - A.worldPosition.X) * dC;
                    float worldY = A.worldPosition.Y + (B.worldPosition.Y - A.worldPosition.Y) * dC;
                    float worldZ = A.worldPosition.Z + (B.worldPosition.Z - A.worldPosition.Z) * dC;
                    float worldW = A.worldPosition.W + (B.worldPosition.W - A.worldPosition.W) * dC;
                    float normalX = A.normal.X + (B.normal.X - A.normal.X) * dC;
                    float normalY = A.normal.Y + (B.normal.Y - A.normal.Y) * dC;
                    float normalZ = A.normal.Z + (B.normal.Z - A.normal.Z) * dC;
                    //float normalW = A.normal.W + (B.normal.W - A.normal.W) * dC;
                    Vertex C = new Vertex(A.X * (1 - dC) + B.X * dC, A.Y * (1 - dC) + B.Y * dC, A.Z * (1 - dC) + B.Z * dC, A.W * (1 - dC) + B.W * dC);
                    C.worldPosition = new Vector4(worldX, worldY, worldZ, worldW);
                    C.normal = new Vector3(normalX, normalY, normalZ);
                    
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
                                              ClippedToScreen(t.v3),
                                              t.flatShading));
            return result;
        }
        private ScreenPoint ClippedToScreen(Vertex Vc)
        {
            // przekształć jeden punkt clipping space 4D na screen space 2D z głębokością pikseli
            float Wc = Vc.W; // zapamiętać wc do korekcji perspektywy w rasteryzacji
            Vc.Normalize(); // zmiana na NDC
            ScreenPoint Vs = new ScreenPoint((int)Math.Round(width * (1 + Vc.X) / 2),
                                             (int)Math.Round(height * (1 - Vc.Y) / 2),
                                             (float)((Vc.Z + 1) / 2));
            Vs.worldPosition = Vc.worldPosition;
            Vs.normal = Vc.normal;
            Vs.wc = Wc;
            return Vs;
        }

        private bool FacingBackwards(List<ScreenTriangle> screenTriangle)
        {
            // albo cała listę odrzuca albo cała listę rysuje
            // wszystkie trójkąty mają takie samo ustawienie w prezstrzenii więc wystarczy że jeden sprawdzę (np. pierwszy z listy) i wtedy wszystko można odrzucić
            // ale otóż nie
            // istnieje prawdopodobieństwo że ten pierwszy trójkąt który bym sprawdził ma wszystkie wierzchołki w jednej lini i nie może taki trójkąt zdecydować o skrętności całej listy
            foreach (ScreenTriangle st in screenTriangle)
            {
                ScreenPoint A = st.p1;
                ScreenPoint B = st.p2;
                ScreenPoint C = st.p3;
                Vector2 AB = new Vector2(B.X - A.X, B.Y - A.Y);
                Vector2 AC = new Vector2(C.X - A.X, C.Y - A.Y);

                // obliczam sinus kąta pomiędzy wektorem AB a AC, ujemny oznacza że patrząc z A w stronę B punkt C znajduje się po prawej stronie
                float sine = (AB.X * AC.Y - AB.Y * AC.X);// / (AB.Length() * AC.Length()); // dzielenie niepotrzebne bo sprawdzam tylko znak

                if (sine == 0) // wszystkie trzy punkty trójkąta w jednej linii
                    continue; // nie decyduj czy wszystkie trójkąty mają być rysowane czy nie
                else if (sine > 0)
                    return false;
                else
                    return true;
            }
            return true;
        }


        private void Rasterize(Bitmap bitmap, ScreenTriangle st, Color color)
        {
            if (drawingEdges)
            {
                MidpointLine(bitmap, st.p1, st.p2, color);
                MidpointLine(bitmap, st.p2, st.p3, color);
                MidpointLine(bitmap, st.p3, st.p1, color);
            }
            else if (shading)
            {
                if (st.flatShading)
                    FillTriangleShadingFlat(bitmap, st);
                else
                    FillTriangleShadingPhong(bitmap, st);
            }
            else
                FillTriangle(bitmap, st, color);

        }
        private void MidpointLine(Bitmap bitmap, ScreenPoint P1, ScreenPoint P2, Color color)
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

            if (0 <= x && x < width && 0 <= y && y < height)
            {
                if (zBufferingToggled && z > depthBuffer[x][y])
                    return;
                bitmap.SetPixel(x, y, c);
                depthBuffer[x][y] = z;
            }
        }

        private void FillTriangle(Bitmap bitmap, ScreenTriangle st, Color color)
        {
            ScreenPoint v1 = st.p1;
            ScreenPoint v2 = st.p2;
            ScreenPoint v3 = st.p3;
            ScreenPoint tmp;
            if (v1.Y == v2.Y && v2.Y == v3.Y)
                return;

            // sortujemy wierzchołki po Y żeby było v1 <= v2 <= v3
            if (v1.Y <= v2.Y && v1.Y <= v3.Y)
            {
                if (v3.Y <= v2.Y) { tmp = v3; v3 = v2; v2 = tmp; }
            }
            else if (v2.Y <= v1.Y && v2.Y <= v3.Y)
            {
                tmp = v2; v2 = v1; v1 = tmp;
                if (v3.Y <= v2.Y) { tmp = v3; v3 = v2; v2 = tmp; }
            }
            else if (v3.Y <= v1.Y && v3.Y <= v2.Y)
            {
                tmp = v3; v3 = v1; v1 = tmp;
                if (v3.Y <= v2.Y) { tmp = v3; v3 = v2; v2 = tmp; }
            }

            if (v2.Y == v3.Y)
                FillBottomFlatTriangle(bitmap, v1, v2, v3, color);
            else if (v1.Y == v2.Y)
                FillTopFlatTriangle(bitmap, v1, v2, v3, color);
            else
            {
                float ratio = (float)(v2.Y - v1.Y) / (v3.Y - v1.Y);
                ScreenPoint v4 = new ScreenPoint((int)Math.Round(v1.X + ratio * (v3.X - v1.X)), v2.Y, v1.Z + ratio * (v3.Z - v1.Z));
                FillBottomFlatTriangle(bitmap, v1, v2, v4, color);
                FillTopFlatTriangle(bitmap, v2, v4, v3, color);
            }
        }
        private void FillBottomFlatTriangle(Bitmap bitmap, ScreenPoint v1, ScreenPoint v2, ScreenPoint v3, Color color)
        {
            if (v3.X < v2.X) { ScreenPoint tmp = v3; v3 = v2; v2 = tmp; }
            //   v1 
            //   / \
            //  /   \
            // v2---v3
            float xL = v1.X;
            float xR = v1.X;
            float dxL = (float)(v2.X - v1.X) / (v2.Y - v1.Y);
            float dxR = (float)(v3.X - v1.X) / (v3.Y - v1.Y);

            float zL = v1.Z; // początkowa głebokość
            float zR = v1.Z;
            float dy = v2.Y - v1.Y;
            float dzL = (v2.Z - v1.Z) / dy;
            float dzR = (v3.Z - v1.Z) / dy;
            for (int y = v1.Y; y <= v3.Y; y++)
            {
                DrawScanLine(bitmap, y, (int)Math.Round(xL), (int)Math.Round(xR), zL, zR, color);
                xL += dxL;
                xR += dxR;
                zL += dzL;
                zR += dzR;
            }
        }
        private void FillTopFlatTriangle(Bitmap bitmap, ScreenPoint v1, ScreenPoint v2, ScreenPoint v3, Color color)
        {
            if (v2.X < v1.X) { ScreenPoint tmp = v1; v1 = v2; v2 = tmp; }
            // v1---v2
            //  \   /
            //   \ /
            //   v3
            float xL = v1.X;
            float xR = v2.X;
            float dxL = (float)(v3.X - v1.X) / (v3.Y - v1.Y);
            float dxR = (float)(v3.X - v2.X) / (v3.Y - v2.Y);

            float zL = v1.Z; // początkowa głebokość
            float zR = v2.Z;
            float dy = v3.Y - v1.Y;
            float dzL = (v3.Z - v1.Z) / dy;
            float dzR = (v3.Z - v2.Z) / dy;
            for (int y = v1.Y; y <= v3.Y; y++)
            {
                DrawScanLine(bitmap, y, (int)Math.Round(xL), (int)Math.Round(xR), zL, zR, color);
                xL += dxL;
                xR += dxR;
                zL += dzL;
                zR += dzR;
            }
        }
        private void DrawScanLine(Bitmap bitmap, int scanLine, int xL, int xR, float zL, float zR, Color color)
        {
            float z = zL;
            float dz = (zR - zL) / (xR - xL);
            for (int x = xL; x <= xR; x++) // stawianie pixeli
            {
                SetPixel(bitmap, x, scanLine, z, color);
                z += dz;
            }
        }
        


        private void FillTriangleShadingFlat(Bitmap bitmap, ScreenTriangle st)
        {
            ScreenPoint v1 = st.p1;
            ScreenPoint v2 = st.p2;
            ScreenPoint v3 = st.p3;
            ScreenPoint tmp;
            if (v1.Y == v2.Y && v2.Y == v3.Y)
                return;

            // sortujemy wierzchołki po Y żeby było v1 <= v2 <= v3
            if (v1.Y <= v2.Y && v1.Y <= v3.Y)
            {
                if (v3.Y <= v2.Y) { tmp = v3; v3 = v2; v2 = tmp; }
            }
            else if (v2.Y <= v1.Y && v2.Y <= v3.Y)
            {
                tmp = v2; v2 = v1; v1 = tmp;
                if (v3.Y <= v2.Y) { tmp = v3; v3 = v2; v2 = tmp; }
            }
            else if (v3.Y <= v1.Y && v3.Y <= v2.Y)
            {
                tmp = v3; v3 = v1; v1 = tmp;
                if (v3.Y <= v2.Y) { tmp = v3; v3 = v2; v2 = tmp; }
            }

            if (v2.Y == v3.Y)
                FillBottomFlatTriangleShadingFlat(bitmap, v1, v2, v3);
            else if (v1.Y == v2.Y)
                FillTopFlatTriangleShadingFlat(bitmap, v1, v2, v3);
            else
            {
                // interpolacja v4 na krawędzi v1---v3
                float ratio = (float)(v2.Y - v1.Y) / (v3.Y - v1.Y);
                float worldX = v1.worldPosition.X + (v3.worldPosition.X - v1.worldPosition.X) * ratio;
                float worldY = v1.worldPosition.Y + (v3.worldPosition.Y - v1.worldPosition.Y) * ratio;
                float worldZ = v1.worldPosition.Z + (v3.worldPosition.Z - v1.worldPosition.Z) * ratio;
                float worldW = v1.worldPosition.W + (v3.worldPosition.W - v1.worldPosition.W) * ratio;
                float normalX = v1.normal.X + (v3.normal.X - v1.normal.X) * ratio;
                float normalY = v1.normal.Y + (v3.normal.Y - v1.normal.Y) * ratio;
                float normalZ = v1.normal.Z + (v3.normal.Z - v1.normal.Z) * ratio;
                ScreenPoint v4 = new ScreenPoint((int)Math.Round(v1.X + ratio * (v3.X - v1.X)), v2.Y, v1.Z + ratio * (v3.Z - v1.Z));
                v4.worldPosition = new Vector4(worldX, worldY, worldZ, worldW);
                v4.normal = Vector3.Normalize(new Vector3(normalX, normalY, normalZ));
                FillBottomFlatTriangleShadingFlat(bitmap, v1, v2, v4);
                FillTopFlatTriangleShadingFlat(bitmap, v2, v4, v3);
            }
        }
        private void FillBottomFlatTriangleShadingFlat(Bitmap bitmap, ScreenPoint v1, ScreenPoint v2, ScreenPoint v3)
        {
            if (v3.X < v2.X) { ScreenPoint tmp = v3; v3 = v2; v2 = tmp; }
            //   v1 
            //   / \
            //  /   \
            // v2---v3
            float xL = v1.X;
            float xR = v1.X;
            float dxL = (float)(v2.X - v1.X) / (v2.Y - v1.Y);
            float dxR = (float)(v3.X - v1.X) / (v3.Y - v1.Y);

            float zL = v1.Z; // początkowa głebokość
            float zR = v1.Z;
            float dy = v2.Y - v1.Y;
            float dzL = (v2.Z - v1.Z) / dy;
            float dzR = (v3.Z - v1.Z) / dy;

            // średni wektor normalny i środek trójkąta w Flat Shading
            Vector3 normal = new Vector3((v1.normal.X + v2.normal.X + v3.normal.X) / 3,
                                         (v1.normal.Y + v2.normal.Y + v3.normal.Y) / 3,
                                         (v1.normal.Z + v2.normal.Z + v3.normal.Z) / 3);
            Vector4 middle = new Vector4((v1.worldPosition.X + v2.worldPosition.X + v3.worldPosition.X) / 3,
                                         (v1.worldPosition.Y + v2.worldPosition.Y + v3.worldPosition.Y) / 3,
                                         (v1.worldPosition.Z + v2.worldPosition.Z + v3.worldPosition.Z) / 3,
                                         (v1.worldPosition.W + v2.worldPosition.W + v3.worldPosition.W) / 3);
            for (int y = v1.Y; y <= v3.Y; y++)
            {
                DrawScanLineShadingFlat(bitmap, y, (int)Math.Round(xL), (int)Math.Round(xR), zL, zR, normal, middle);
                xL += dxL;
                xR += dxR;
                zL += dzL;
                zR += dzR;
            }
        }
        private void FillTopFlatTriangleShadingFlat(Bitmap bitmap, ScreenPoint v1, ScreenPoint v2, ScreenPoint v3)
        {
            if (v2.X < v1.X) { ScreenPoint tmp = v1; v1 = v2; v2 = tmp; }
            // v1---v2
            //  \   /
            //   \ /
            //   v3
            float xL = v1.X;
            float xR = v2.X;
            float dxL = (float)(v3.X - v1.X) / (v3.Y - v1.Y);
            float dxR = (float)(v3.X - v2.X) / (v3.Y - v2.Y);

            float zL = v1.Z; // początkowa głebokość
            float zR = v2.Z;
            float dy = v3.Y - v1.Y;
            float dzL = (v3.Z - v1.Z) / dy;
            float dzR = (v3.Z - v2.Z) / dy;

            // średni wektor normalny i środek trójkąta w Flat Shading
            Vector3 normal = new Vector3((v1.normal.X + v2.normal.X + v3.normal.X) / 3,
                                         (v1.normal.Y + v2.normal.Y + v3.normal.Y) / 3,
                                         (v1.normal.Z + v2.normal.Z + v3.normal.Z) / 3);
            Vector4 middle = new Vector4((v1.worldPosition.X + v2.worldPosition.X + v3.worldPosition.X) / 3,
                                         (v1.worldPosition.Y + v2.worldPosition.Y + v3.worldPosition.Y) / 3,
                                         (v1.worldPosition.Z + v2.worldPosition.Z + v3.worldPosition.Z) / 3,
                                         (v1.worldPosition.W + v2.worldPosition.W + v3.worldPosition.W) / 3);
            for (int y = v1.Y; y <= v3.Y; y++)
            {
                DrawScanLineShadingFlat(bitmap, y, (int)Math.Round(xL), (int)Math.Round(xR), zL, zR, normal, middle);
                xL += dxL;
                xR += dxR;
                zL += dzL;
                zR += dzR;
            }
        }
        private void DrawScanLineShadingFlat(Bitmap bitmap, int scanLine, int xL, int xR, float zL, float zR, Vector3 normal, Vector4 middle)
        {
            int R = 0;
            int G = 0;
            int B = 0;
            foreach (Light L in lights)
            {
                Color c = CalculatePixelColor(currentShapeMaterial, L, normal, middle);
                R += c.R;
                G += c.G;
                B += c.B;
            }
            R = Math.Min(255, R);
            G = Math.Min(255, G);
            B = Math.Min(255, B);
            Color pixelColor = Color.FromArgb(R, G, B); // wszystkie piksele taki sam kolor w Flat Shading
            float z = zL; // głębokość
            float dz = (zR - zL) / (xR - xL);
            for (int x = xL; x <= xR; x++) // stawianie pixeli
            {
                SetPixel(bitmap, x, scanLine, z, pixelColor);
                z += dz;
            }
        }

        private Color CalculatePixelColor(Material shapeMaterial, Light light, Vector3 pixelNormal, Vector4 pixelPosition)
        {
            float R, G, B;
            Vector3 L = new Vector3(light.X - pixelPosition.X, light.Y - pixelPosition.Y, light.Z - pixelPosition.Z);
            L = Vector3.Normalize(L);
            pixelNormal = Vector3.Normalize(pixelNormal);
            float LdotN = Math.Max(0, Vector3.Dot(L, pixelNormal));

            float RdotV;
            Vector3 reflect = 2 * Vector3.Dot(L, pixelNormal) * pixelNormal - L;
            reflect = Vector3.Normalize(reflect);
            if (Vector3.Dot(reflect, pixelNormal) < 0)
                RdotV = 0;
            else
            {
                Vector3 viewer = new Vector3(currentCamera.GetPosition.X - pixelPosition.X, currentCamera.GetPosition.Y - pixelPosition.Y, currentCamera.GetPosition.Z - pixelPosition.Z);
                viewer = Vector3.Normalize(viewer);
                RdotV = Math.Max(0, Vector3.Dot(reflect, viewer));
            }

            R = PhongModel((float)shapeMaterial.ka.R / 255, (float)shapeMaterial.kd.R / 255, (float)shapeMaterial.ks.R / 255, shapeMaterial.shininess, LdotN, RdotV, (float)light.Ia.R / 255, (float)light.Id.R / 255, (float)light.Is.R / 255);
            G = PhongModel((float)shapeMaterial.ka.G / 255, (float)shapeMaterial.kd.G / 255, (float)shapeMaterial.ks.G / 255, shapeMaterial.shininess, LdotN, RdotV, (float)light.Ia.G / 255, (float)light.Id.G / 255, (float)light.Is.G / 255);
            B = PhongModel((float)shapeMaterial.ka.B / 255, (float)shapeMaterial.kd.B / 255, (float)shapeMaterial.ks.B / 255, shapeMaterial.shininess, LdotN, RdotV, (float)light.Ia.B / 255, (float)light.Id.B / 255, (float)light.Is.B / 255);

            return Color.FromArgb((int)(R * 255), (int)(G * 255), (int)(B * 255));
        }
        private float PhongModel(float ka, float kd, float ks, float shininess, float LdotN, float RdotV, float Ia, float Id, float Is)
        {
            return (float)(ka * Ia + kd * LdotN * Id + ks * Math.Pow(RdotV, shininess) * Is);
        }


        private void FillTriangleShadingPhong(Bitmap bitmap, ScreenTriangle st)
        {
            ScreenPoint v1 = st.p1;
            ScreenPoint v2 = st.p2;
            ScreenPoint v3 = st.p3;
            ScreenPoint tmp;
            if (v1.Y == v2.Y && v2.Y == v3.Y)
                return;

            // sortujemy wierzchołki po Y żeby było v1 <= v2 <= v3
            if (v1.Y <= v2.Y && v1.Y <= v3.Y)
            {
                if (v3.Y <= v2.Y) { tmp = v3; v3 = v2; v2 = tmp; }
            }
            else if (v2.Y <= v1.Y && v2.Y <= v3.Y)
            {
                tmp = v2; v2 = v1; v1 = tmp;
                if (v3.Y <= v2.Y) { tmp = v3; v3 = v2; v2 = tmp; }
            }
            else if (v3.Y <= v1.Y && v3.Y <= v2.Y)
            {
                tmp = v3; v3 = v1; v1 = tmp;
                if (v3.Y <= v2.Y) { tmp = v3; v3 = v2; v2 = tmp; }
            }

            if (v2.Y == v3.Y)
                FillBottomFlatTriangleShadingFlat(bitmap, v1, v2, v3);
            else if (v1.Y == v2.Y)
                FillTopFlatTriangleShadingFlat(bitmap, v1, v2, v3);
            else
            {
                // interpolacja v4 na krawędzi v1---v3
                float ratio = (float)(v2.Y - v1.Y) / (v3.Y - v1.Y);
                float worldX = v1.worldPosition.X + (v3.worldPosition.X - v1.worldPosition.X) * ratio;
                float worldY = v1.worldPosition.Y + (v3.worldPosition.Y - v1.worldPosition.Y) * ratio;
                float worldZ = v1.worldPosition.Z + (v3.worldPosition.Z - v1.worldPosition.Z) * ratio;
                float worldW = v1.worldPosition.W + (v3.worldPosition.W - v1.worldPosition.W) * ratio;
                float normalX = v1.normal.X + (v3.normal.X - v1.normal.X) * ratio;
                float normalY = v1.normal.Y + (v3.normal.Y - v1.normal.Y) * ratio;
                float normalZ = v1.normal.Z + (v3.normal.Z - v1.normal.Z) * ratio;
                ScreenPoint v4 = new ScreenPoint((int)Math.Round(v1.X + ratio * (v3.X - v1.X)), v2.Y, v1.Z + ratio * (v3.Z - v1.Z));
                v4.worldPosition = new Vector4(worldX, worldY, worldZ, worldW);
                v4.normal = Vector3.Normalize(new Vector3(normalX, normalY, normalZ));
                v4.wc = v1.wc + (v3.wc - v1.wc) * ratio;
                FillBottomFlatTriangleShadingPhong(bitmap, v1, v2, v4);
                FillTopFlatTriangleShadingPhong(bitmap, v2, v4, v3);
            }
        }
        private void FillBottomFlatTriangleShadingPhong(Bitmap bitmap, ScreenPoint v1, ScreenPoint v2, ScreenPoint v3)
        {
            if (v3.X < v2.X) { ScreenPoint tmp = v3; v3 = v2; v2 = tmp; }
            //   v1 
            //   / \
            //  /   \
            // v2---v3
            float xL = v1.X;
            float xR = v1.X;
            float dxL = (float)(v2.X - v1.X) / (v2.Y - v1.Y);
            float dxR = (float)(v3.X - v1.X) / (v3.Y - v1.Y);

            float zL = v1.Z; // początkowa głębokość
            float zR = v1.Z;
            float dy = v2.Y - v1.Y;
            float dzL = (v2.Z - v1.Z) / dy;
            float dzR = (v3.Z - v1.Z) / dy;

            Vector3 normalL, normalR;
            Vector4 pointL, pointR;

            float q = 0;
            float dq = 1 / dy;
            float wcL, wcR;

            for (int y = v1.Y; y <= v3.Y; y++)
            {
                normalL = Vector3.Normalize(((v1.normal * (1 - q) / v1.wc) + (v2.normal * q / v2.wc)) / ((1 - q) / v1.wc + q / v2.wc));
                normalR = Vector3.Normalize(((v1.normal * (1 - q) / v1.wc) + (v3.normal * q / v3.wc)) / ((1 - q) / v1.wc + q / v3.wc));
                pointL = Vector4.Normalize(((v1.worldPosition * (1 - q) / v1.wc) + (v2.worldPosition * q / v2.wc)) / ((1 - q) / v1.wc + q / v2.wc));
                pointR = Vector4.Normalize(((v1.worldPosition * (1 - q) / v1.wc) + (v3.worldPosition * q / v3.wc)) / ((1 - q) / v1.wc + q / v3.wc));
                wcL = v1.wc * (1 - q) + v2.wc * q;
                wcR = v1.wc * (1 - q) + v3.wc * q;

                DrawScanLineShadingPhong(bitmap, y, (int)Math.Round(xL), (int)Math.Round(xR), zL, zR, normalL, normalR, pointL, pointR, wcL, wcR);
                xL += dxL; xR += dxR; zL += dzL; zR += dzR; q += dq;
            }
        }
        private void FillTopFlatTriangleShadingPhong(Bitmap bitmap, ScreenPoint v1, ScreenPoint v2, ScreenPoint v3)
        {
            if (v2.X < v1.X) { ScreenPoint tmp = v1; v1 = v2; v2 = tmp; }
            // v1---v2
            //  \   /
            //   \ /
            //   v3
            float xL = v1.X;
            float xR = v2.X;
            float dxL = (float)(v3.X - v1.X) / (v3.Y - v1.Y);
            float dxR = (float)(v3.X - v2.X) / (v3.Y - v2.Y);

            float zL = v1.Z; // początkowa głebokość
            float zR = v2.Z;
            float dy = v3.Y - v1.Y;
            float dzL = (v3.Z - v1.Z) / dy;
            float dzR = (v3.Z - v2.Z) / dy;

            Vector3 normalL, normalR;
            Vector4 pointL, pointR;

            float q = 0;
            float dq = 1 / dy;
            float wcL, wcR;

            for (int y = v1.Y; y <= v3.Y; y++)
            {
                normalL = Vector3.Normalize(((v1.normal * (1 - q) / v1.wc) + (v3.normal * q / v3.wc)) / ((1 - q) / v1.wc + q / v3.wc));
                normalR = Vector3.Normalize(((v2.normal * (1 - q) / v2.wc) + (v3.normal * q / v3.wc)) / ((1 - q) / v2.wc + q / v3.wc));
                pointL = Vector4.Normalize(((v1.worldPosition * (1 - q) / v1.wc) + (v3.worldPosition * q / v3.wc)) / ((1 - q) / v1.wc + q / v3.wc));
                pointR = Vector4.Normalize(((v2.worldPosition * (1 - q) / v2.wc) + (v3.worldPosition * q / v3.wc)) / ((1 - q) / v2.wc + q / v3.wc));
                wcL = v1.wc * (1 - q) + v3.wc * q;
                wcR = v2.wc * (1 - q) + v3.wc * q;

                DrawScanLineShadingPhong(bitmap, y, (int)Math.Round(xL), (int)Math.Round(xR), zL, zR, normalL, normalR, pointL, pointR, wcL, wcR);
                xL += dxL; xR += dxR; zL += dzL; zR += dzR; q += dq;
            }
        }
        private void DrawScanLineShadingPhong(Bitmap bitmap, int scanLine, int xL, int xR, float zL, float zR, Vector3 normalL, Vector3 normalR, Vector4 pointL, Vector4 pointR, float wcL, float wcR)
        {
            int R = 0, G = 0, B = 0;

            Vector3 normal;
            Vector4 point;
            float q = 0;
            float dq = 1 / (float)(xR - xL);
            
            float z = zL; // głębokość
            float dz = (zR - zL) / (xR - xL);
            for (int x = xL; x <= xR; x++) // stawianie pixeli
            {
                normal = Vector3.Normalize(((normalL * (1 - q) / wcL) + (normalR * q / wcR)) / ((1 - q) / wcL + q / wcR));
                point = Vector4.Normalize(((pointL * (1 - q) / wcL) + (pointR * q / wcR)) / ((1 - q) / wcL + q / wcR));
                foreach (Light L in lights)
                {
                    Color c = CalculatePixelColor(currentShapeMaterial, L, normal, point);
                    R += c.R;
                    G += c.G;
                    B += c.B;
                    q += dq;
                }
                R = Math.Min(255, R); G = Math.Min(255, G); B = Math.Min(255, B);
                Color pixelColor = Color.FromArgb(R, G, B);
                SetPixel(bitmap, x, scanLine, z, pixelColor);
                z += dz;
                q += dq;
                R = G = B = 0;
            }
        }
    }
}
