using System.Security.Cryptography.X509Certificates;
using MyEngine.Assets.Models;
using OpenTK;

namespace MyEngine
{
    public struct PositionColorVertex: IVertextype
    {
        public Vector3 position;
        public Vector4 color;

        public static PositionColorVertex operator +(PositionColorVertex left, Vector3 right)
        {
            return new PositionColorVertex
            {
                position = left.position + right,
                color =  left.color
            };
        }
        public static PositionColorVertex operator +(PositionColorVertex left, PositionColorVertex right)
        {
            return new PositionColorVertex
            {
                position = left.position + right.position,
                color =  left.color + right.color
            };
        }


        public int getStructSize()
        {
            return sizeof(float) * 7;
        }
    }
}