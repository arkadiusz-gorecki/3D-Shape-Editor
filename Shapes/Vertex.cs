using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    public class Vertex // pionowy wektor reprezentujący pozycję w przestrzeni
    {
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float Z { get; set; } = 0;
        public float W { get; set; } = 1;

        public Vector3 normal;
        public Vector4 worldPosition;

        public Vertex() { }

        public Vertex(float x, float y, float z)
        {
            X = x; Y = y; Z = z; W = 1;
        }
        public Vertex(float x, float y, float z, float w)
        {
            X = x; Y = y; Z = z; W = w;
        }
        public Vertex(float x, float y, float z, float w, Vector4 worldPosition)
        {
            X = x; Y = y; Z = z; W = w;
            this.worldPosition = worldPosition;
        }
        public Vertex(float x, float y, float z, Vector3 n)
        {
            X = x; Y = y; Z = z; W = 1; normal = n;
        }

        public void Normalize()
        {
            if (W == 0)
                return;
            X /= W;
            Y /= W;
            Z /= W;
            W = 1;
        }

    }
}
