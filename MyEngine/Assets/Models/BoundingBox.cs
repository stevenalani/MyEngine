using System.Linq;
using MyEngine.Assets.Models;
using OpenTK;
using OpenTK.Graphics;

namespace MyEngine
{
    public class BoundingBox : Cube
    {
        new Matrix4 model = Matrix4.Identity;
        public Vector3 leftlownear;
        public Vector3 rightlownear;
        public Vector3 rightupnear;
        public Vector3 leftupnear;

        public Vector3 leftlowfar;
        public Vector3 rightlowfar;
        public Vector3 rightupfar;
        public Vector3 leftupfar;

        public Vector3 dimensions;
        public BoundingBox(Model model)
        {
            if (model is PositionColorModel)
            {
                init((PositionColorModel)model);
            }
        }
        private void init(PositionColorModel model)
        {
            var vertices = model.Vertices.Select(x => Vector3.TransformVector(x.position, model.model)).ToArray();
            var lowest = vertices.Min(x => x.Y);
            var highest = vertices.Max(x => x.Y);
            var left = vertices.Min(x => x.X);
            var right = vertices.Max(x => x.X);
            var farest = vertices.Min(x => x.Z);
            var nearest = vertices.Max(x => x.Z);

            dimensions = new Vector3(right - left, highest - lowest, nearest - farest);

            leftlownear = new Vector3(left, lowest, nearest);
            rightlownear = new Vector3(right, lowest, nearest);
            rightupnear = new Vector3(right, highest, nearest);
            leftupnear = new Vector3(left, highest, nearest);

            leftlowfar = new Vector3(left, lowest, farest);
            rightlowfar = new Vector3(right, lowest, farest);
            rightupfar = new Vector3(right, highest, farest);
            leftupfar = new Vector3(left, highest, farest);
            Vertices = ToArray().Select(x => new PositionColorVertex()
                {position = x, color = new Vector4(0.1f, 0.7f, 1f, 0.1f)}).ToArray();
        }


        public Vector3[] ToArray()
        {
            return new Vector3[]
            {
                leftlownear,
                rightlownear,
                rightupnear,
                leftupnear,
                leftlowfar,
                rightlowfar,
                rightupfar,
                leftupfar
            };
        }

        public void TransformBoundingBox(Matrix4 transforMatrix)
        {
            var edges = Vertices.Select(x => Vector3.TransformPosition(x.position,transforMatrix));
            var lowest = edges.Min(x => x.Y);
            var highest = edges.Max(x => x.Y);
            var left = edges.Min(x => x.X);
            var right = edges.Max(x => x.X);
            var farest = edges.Min(x => x.Z);
            var nearest = edges.Max(x => x.Z);

            leftlownear = new Vector3(left, lowest, nearest);
            rightlownear = new Vector3(right, lowest, nearest);
            rightupnear = new Vector3(right, highest, nearest);
            leftupnear = new Vector3(left, highest, nearest);

            leftlowfar = new Vector3(left, lowest, farest);
            rightlowfar = new Vector3(right, lowest, farest);
            rightupfar = new Vector3(right, highest, farest);
            leftupfar = new Vector3(left, highest, farest);

        }
    }
}