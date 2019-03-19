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
        public Camera Camera { get; set; }
        public new Matrix4 Modelmatrix;
        public BoundingBox(Model model,Camera camera, Vector4 color = default(Vector4))
        {
            this.Camera = camera;
            this.color = color == default(Vector4)?new Vector4(0.1f, 0.7f, 1f, 0.1f) : color;
            if (model is PositionColorModel)
            {
                update((PositionColorModel)model);
                ((PositionColorModel)model).OnUpdate += update;
            }
        }
        private void update(PositionColorModel inmodel)
        {
            var vertices = inmodel.Vertices.Select(x => Vector3.TransformPosition(x.Position, inmodel.Modelmatrix)).ToArray();
            var min_y = vertices.Min(x => x.Y);
            var max_y = vertices.Max(x => x.Y);
            var min_x = vertices.Min(x => x.X);
            var max_x = vertices.Max(x => x.X);
            var min_z = vertices.Min(x => x.Z);
            var max_z = vertices.Max(x => x.Z);

            var size = new Vector3(max_x - min_x, max_y - min_y, max_z - min_z)/2;
            var center = new Vector3((min_x + max_x) / 2, (min_y + max_y) / 2, (min_z + max_z) / 2);
            Modelmatrix = Matrix4.CreateTranslation(center) * Matrix4.CreateScale(size);

            leftlownear = new Vector3(min_x, min_y, max_z);
            rightlownear = new Vector3(max_x, min_y, max_z);
            rightupnear = new Vector3(max_x, max_y, max_z);
            leftupnear = new Vector3(min_x, max_y, max_z);

            leftlowfar = new Vector3(min_x, min_y, min_z);
            rightlowfar = new Vector3(max_x, min_y, min_z);
            rightupfar = new Vector3(max_x, max_y, min_z);
            leftupfar = new Vector3(min_x, max_y, min_z);

            Vertices = ToArray()?.Select(x => new PositionColorVertex()
                {
                    //Position = Vector3.TransformPerspective(x, Camera.GetProjection().Inverted() * Camera.GetView().Inverted()),
                    Position = x,
                    Color = color
                }).ToArray();
            IsInitialized = false;
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
            var edges = Vertices.Select(x => Vector3.TransformPosition(x.Position,transforMatrix));
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
            IsInitialized = false;
        }

        public string series { get; set; } = "default";
        public bool purgesiblings { get; set; } = false;

    }
}