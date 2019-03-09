using System.Linq;
using OpenTK;

namespace MyEngine
{
    public struct BoundingBox
    {
        Vector3 leftlownear;
        Vector3 rightlownear;
        Vector3 rightupnear;
        Vector3 leftupnear;

        Vector3 leftlowfar;
        Vector3 rightlowfar;
        Vector3 rightupfar;
        Vector3 leftupfar;

        public Vector3 dimensions;
        public BoundingBox(PositionColorModel model)
        {
            var vertices = model.Vertices.Select(x => x.position);
            var lowest = vertices.Min(x => x.Y);
            var highest = vertices.Max(x => x.Y);
            var left = vertices.Min(x => x.X);
            var right = vertices.Max(x => x.X);
            var farest = vertices.Min(x => x.Z);
            var nearest = vertices.Max(x => x.Z);

            dimensions = new Vector3(right-left,highest-lowest,nearest-farest);

            leftlownear = new Vector3(left,lowest, nearest);
            rightlownear = new Vector3(right, lowest, nearest);
            rightupnear = new Vector3(right, highest, nearest);
            leftupnear = new Vector3(left, highest, nearest);

            leftlowfar = new Vector3(left,lowest,farest);
            rightlowfar = new Vector3(right, lowest, farest);
            rightupfar = new Vector3(right, highest, farest);
            leftupfar = new Vector3(left, highest, farest);
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
        public static BoundingBox TransformBoundingBox(BoundingBox bb, Matrix4 transforMatrix)
        {
            var edges = bb.ToArray().Select(x => Vector3.TransformVector(x,transforMatrix));
            var lowest = edges.Min(x => x.Y);
            var highest = edges.Max(x => x.Y);
            var left = edges.Min(x => x.X);
            var right = edges.Max(x => x.X);
            var farest = edges.Min(x => x.Z);
            var nearest = edges.Max(x => x.Z);
            return new BoundingBox()
            {
                leftlownear = new Vector3(left, lowest, nearest),
                rightlownear = new Vector3(right, lowest, nearest),
                rightupnear = new Vector3(right, highest, nearest),
                leftupnear = new Vector3(left, highest, nearest),

                leftlowfar = new Vector3(left, lowest, farest),
                rightlowfar = new Vector3(right, lowest, farest),
                rightupfar = new Vector3(right, highest, farest),
                leftupfar = new Vector3(left, highest, farest),
            };
        }
    }
}