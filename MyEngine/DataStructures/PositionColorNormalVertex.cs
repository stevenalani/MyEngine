using Assimp;

namespace MyEngine
{
    public struct PositionColorNormalVertex
    {
        public float x, y, z,
            r, g, b, a,
            nx, ny, nz;

        public PositionColorNormalVertex(float x, float y, float z, float r, float g, float b, float a, float nx, float ny, float nz)
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

        public PositionColorNormalVertex(Vector3D position, Color4D color, Vector3D normal)
        {
            this.x = position.X;
            this.y = position.Y;
            this.z = position.Z;
            this.r = color.R;
            this.g = color.G;
            this.b = color.B;
            this.a = color.A;
            this.nx = normal.X;
            this.ny = normal.Y;
            this.nz = normal.Z;
        }

        public const int Size = sizeof(float) * 10;

        public float[] ToArray()
        {
            return new float[10] { x, y, z, r, g, b, a, nx, ny, nz };
        }
    }
}
