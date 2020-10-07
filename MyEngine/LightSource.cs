using OpenTK;

namespace MyEngine
{
    public class LightSource
    {
        public Vector3 LightColor;
        public Vector3 LightPosition;

        //public Matrix4 LightView => Matrix4.CreateOrthographic()
        public LightSource(Vector3 position, Vector3 color)
        {
            LightColor = color;
            LightPosition = position;
        }
    }
}