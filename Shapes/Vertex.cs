using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    class Vertex // pionowy wektor reprezentujący pozycję w przestrzeni
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vertex(float x, float y, float z)
        {
            X = x; Y = y; Z = z; W = 1;
        }

        public void Normalize()
        {
            if (W == 0)
                return;
            X /= W;
            Y /= W;
            Z /= W;
        }
    }
}
