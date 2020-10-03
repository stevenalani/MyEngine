using System;
using System.Drawing;
using System.Linq;
using MyEngine.DataStructures;
using MyEngine.ShaderImporter;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace MyEngine
{
    public static class VisualRayData
    {
        public static readonly Vector3[] Vertices = {
            new Vector3(-1f,-1f,1f),
            new Vector3(1f,-1f,1f),
            new Vector3(1f,1f,1f),
            new Vector3(-1f,1f,1f),

            new Vector3(0f,0f,-1f),

        };
        public static readonly uint[] Indices =
        {
            0, 1, 2,
            2, 3, 0,
            // right
            4, 2, 1,
            // left
            4, 0, 3,
            // bottom
            1, 0, 4,
            // top
            3, 2, 4,
        };

        public static Vector3[] NotCentered => Vertices.Select(x => (x / 2) + new Vector3(0.5f)).ToArray();
    }
    internal class VisualRay : PositionColorModel, IEngineModel
    {
        public Vector4 Color = Vector4.One;
        public VisualRay(Vector3 position, Vector3 direction, float length = 1000f) : base(null, VisualRayData.Indices)
        {
            Scales = new Vector3(0.1f, 0.1f, 0.1f);
            Position = position;
            var target = direction * length;
            purgesiblings = true;
            Vertices = new PositionColorVertex[]
            {
                new PositionColorVertex(){ Position = new Vector3(-0.01f,-0.01f,0),Color = Color },
                new PositionColorVertex(){ Position = new Vector3(0.01f,-0.01f,0),Color = Color },
                new PositionColorVertex(){ Position = new Vector3(0.01f,0.01f,0),Color = Color },
                new PositionColorVertex(){ Position = new Vector3(-0.01f,0.01f,0),Color = Color },
                new PositionColorVertex(){ Position = target,Color = Color },
            };

        }


        public override void Draw(ShaderProgram shaderProgram)
        {

            base.Draw(shaderProgram);
        }

        public string series { get; set; } = "default";
        public bool purgesiblings { get; set; } = false;

    }

}