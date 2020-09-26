using System.Linq;
using System.Runtime.CompilerServices;
using MyEngine.Assets.Models;
using MyEngine.DataStructures;
using MyEngine.Logging;
using MyEngine.Models;
using OpenTK;
using OpenTK.Graphics;

namespace MyEngine
{
    public class BoundingBox : Cube, IEngineModel
    {

        public Vector3 leftlownear;
        public Vector3 rightlownear;
        public Vector3 rightupnear;
        public Vector3 leftupnear;

        public Vector3 leftlowfar;
        public Vector3 rightlowfar;
        public Vector3 rightupfar;
        public Vector3 leftupfar;

        public Vector4 color;
        public new Matrix4 Modelmatrix { get; set; }
        public BoundingBox(Model model, Vector4 color = default(Vector4))
        {
            purgesiblings = false;
            this.color = color == default(Vector4) ? new Vector4(0.1f, 0.7f, 1f, 0.1f) : color;
            if (model is PositionColorModel)
            {
                update((PositionColorModel)model);
                ((PositionColorModel)model).OnUpdate += update;
            }
        }
        private void update(PositionColorModel inmodel)
        {
            var vertices = inmodel.Vertices.Select(x => Vector3.TransformPosition(x.Position, inmodel.Modelmatrix)).ToArray();
            var minY = vertices.Min(x => x.Y);
            var maxY = vertices.Max(x => x.Y);
            var minX = vertices.Min(x => x.X);
            var maxX = vertices.Max(x => x.X);
            var minZ = vertices.Min(x => x.Z);
            var maxZ = vertices.Max(x => x.Z);

            var size = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);
            var center = new Vector3(size.X / 2, size.Y / 2, size.Z / 2);
            Modelmatrix = Matrix4.CreateTranslation(center);

            leftlownear = new Vector3(minX, minY, maxZ);
            rightlownear = new Vector3(maxX, minY, maxZ);
            rightupnear = new Vector3(maxX, maxY, maxZ);
            leftupnear = new Vector3(minX, maxY, maxZ);

            leftlowfar = new Vector3(minX, minY, minZ);
            rightlowfar = new Vector3(maxX, minY, minZ);
            rightupfar = new Vector3(maxX, maxY, minZ);
            leftupfar = new Vector3(minX, maxY, minZ);

            Vertices = ToArray()?.Select(x => new PositionColorVertex()
            {
                Position = x - center,
                Color = color
            }).ToArray();
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
            var edges = Vertices.Select(x => Vector3.TransformPosition(x.Position, transforMatrix));
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
            Vertices = ToArray().Select(x => new PositionColorVertex()
            { Position = x, Color = color }).ToArray();
        }

        public string series { get; set; } = "default";
        public bool purgesiblings { get; set; } = false;

    }
}