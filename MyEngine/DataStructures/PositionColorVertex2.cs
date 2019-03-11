using OpenTK;

namespace MyEngine.DataStructures
{
    public struct PositionColorVertex2 : IVertextype
    {
        public Vector2 position;
        public Vector4 color;

        public static PositionColorVertex2 operator +(PositionColorVertex2 left, Vector2 right)
        {
            return new PositionColorVertex2
            {
                position = left.position + right,
                color = left.color
            };
        }
        public static PositionColorVertex2 operator +(PositionColorVertex2 left, PositionColorVertex2 right)
        {
            return new PositionColorVertex2
            {
                position = left.position + right.position,
                color = left.color + right.color
            };
        }

        public int getStructSize()
        {
            return sizeof(float) * 6;
        }
    }
}