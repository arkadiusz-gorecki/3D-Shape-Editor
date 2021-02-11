using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DShapeEditor.Shapes
{
    class Light
    {
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float Z { get; set; } = 0;
        public float W { get; set; } = 1;
        public Color Ia;
        public Color Id;
        public Color Is;

        public Light(Color c, float x, float y, float z)
        {
            X = x; Y = y; Z = z; W = 1;
            float ambient = 0.2f; // sumowanie do 1
            float diffuse = 0.3f;
            float specular = 0.5f;
            Ia = Color.FromArgb((int)(c.R * ambient), (int)(c.G * ambient), (int)(c.B * ambient));
            Id = Color.FromArgb((int)(c.R * diffuse), (int)(c.G * diffuse), (int)(c.B * diffuse));
            Is = Color.FromArgb((int)(c.R * specular), (int)(c.G * specular), (int)(c.B * specular));
        }
        public Light(float x, float y, float z, Color _ia, Color _id, Color _is)
        {
            X = x; Y = y; Z = z; W = 1;
            Ia = _ia; Id = _id; Is = _is;
        }
    }
}
