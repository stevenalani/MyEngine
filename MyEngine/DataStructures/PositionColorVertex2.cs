using OpenTK;

namespace MyEngine.DataStructures
{
    public struct PositionColorVertex2 : IVertextype
    {
        public Vector2 position;
        public Vector4 Color { get; set; }

        public static PositionColorVertex2 operator +(PositionColorVertex2 left, Vector2 right)
        {
            return new PositionColorVertex2
            {
                position = left.position + right,
                Color = left.Color
            };
        }
        public static PositionColorVertex2 operator +(PositionColorVertex2 left, PositionColorVertex2 right)
        {
            return new PositionColorVertex2
            {
                position = left.position + right.position,
                Color = left.Color + right.Color
            };
        }

        public Vector3 Position
        {
            get => new Vector3(position);
            set => position = new Vector2(value.X, value.Y);
        }
        
        public int getStructSize()
        {
            return sizeof(float) * 6;
        }
    }
}