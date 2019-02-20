using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEngine
{
    public struct PositionColorNormalVertex
    {
        public float x, y, z,
            r, g, b, a,
            nx, ny, nz;

        public PositionColorNormalVertex(float x, float y, float z, float r, float g, float b, float a,float nx,float ny, float nz)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            this.nx = nx;
            this.ny = ny;
            this.nz = nz;
        }

        public const int Size = sizeof(float) * 10;

        public float[] ToArray()
        {
            return new float[10] { x, y, z, r, g, b, a,nx,ny,nz};
        }
    }
}
