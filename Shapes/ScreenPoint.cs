using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    class ScreenPoint
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public float Z { get; set; } = 0; // głębokość [0, 1]

        public Vector3 normal;
        public Vector4 worldPosition;
        public float wc; // do korekcji perspektywy


        public ScreenPoint(int x, int y, float z)
        {
            X = x; Y = y; Z = z;
        }
    }
}
