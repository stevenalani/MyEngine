using System.Linq;
using MyEngine.DataStructures;
using MyEngine.ShaderImporter;
using OpenTK;

namespace MyEngine.Models
{
    public static class CubeData
    {
        public static readonly Vector3[] Vertices = {
            new Vector3(-0.5f,-0.5f,0.5f),
            new Vector3(0.5f,-0.5f,0.5f),
            new Vector3(0.5f,0.5f,0.5f),
            new Vector3(-0.5f,0.5f,0.5f), 

            new Vector3(-0.5f,-0.5f,-0.5f),
            new Vector3(0.5f,-0.5f,-0.5f),
            new Vector3(0.5f,0.5f,-0.5f),
            new Vector3(-0.5f,0.5f,-0.5f),
        };
        public static readonly uint[] Indices =
        {
            0, 1, 2,
            2, 3, 0,
            // right
            1, 5, 6,
            6, 2, 1,
            // back
            7, 6, 5,
            5, 4, 7,
            // left
            4, 0, 3,
            3, 7, 4,
            // bottom
            4, 5, 1,
            1, 0, 4,
            // top
            3, 2, 6,
            6, 7, 3
        };

        public static Vector3[] NotCentered => Vertices.Select(x => (x / 2) + new Vector3(0.5f)).ToArray();
    }
    internal class Cube : PositionColorModel
    {
        public Cube() : base(null, CubeData.Indices)
        {
            Vertices = CubeData.Vertices.Select(x => new PositionColorVertex
                {position = x, color = new Vector4(0.1f, 0.5f, 0.2f, 0.4f)}).ToArray();
        }
        public Cube(BoundingBox boundingBox) : base(null, CubeData.Indices)
        {
            Vertices = boundingBox.ToArray().Select(x => new PositionColorVertex
                {position = x, color = new Vector4(0.1f, 0.5f, 0.2f, 0.2f)}).ToArray();
        }

        public override void Draw(ShaderProgram shaderProgram)
        {
            base.Draw(shaderProgram);
        }

        public override void InitBuffers()
        {
            base.InitBuffers();
        }
    }
}