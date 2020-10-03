using OpenTK;

namespace MyEngine.DataStructures
{
    public struct PositionColorVertex : IVertextype
    {
        public Vector3 Position;
        public Vector4 Color;

        public static PositionColorVertex operator +(PositionColorVertex left, Vector3 right)
        {
            return new PositionColorVertex
            {
                Position = left.Position + right,
                Color = left.Color
            };
        }
        public static PositionColorVertex operator +(PositionColorVertex left, PositionColorVertex right)
        {
            return new PositionColorVertex
            {
                Position = left.Position + right.Position,
                Color = left.Color + right.Color
            };
        }

        public int getStructSize()
        {
            return sizeof(float) * 7;
        }
    }
}