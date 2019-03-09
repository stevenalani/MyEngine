using System;
using System.Linq;
using MyEngine.ShaderImporter;
using OpenTK;

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
    internal class VisualRay : PositionColorModel
    {
        public VisualRay(Camera camera) : base(null, VisualRayData.Indices)
        {
            Vector4 color = Vector4.One;
            Scales = new Vector3(0.1f,0.1f,0.1f);
            Position = camera.Position;
            var target = camera.ViewDirection;

            Vertices = new PositionColorVertex[]
            {
                new PositionColorVertex(){ position = new Vector3(-0.01f,-0.01f,0),color = color }, 
                new PositionColorVertex(){ position = new Vector3(0.01f,-0.01f,0),color = color }, 
                new PositionColorVertex(){ position = new Vector3(0.01f,0.01f,0),color = color }, 
                new PositionColorVertex(){ position = new Vector3(-0.01f,0.01f,0),color = color }, 
                new PositionColorVertex(){ position = target*1000f,color = color }, 
            };
            /*
            
            float yaw = direction.X >= 0 ? (float)Math.Acos(target.X/target.Length) : (float)Math.Acos((1f + target.X) / target.Length);
            float pitch = direction.Y >= 0 ? (float)Math.Acos(target.Y / target.Length) : (float)Math.Acos((1f + target.Y) / target.Length);
            float roll = direction.Z >= 0 ? (float)Math.Acos(target.Z / target.Length): (float)Math.Acos((1f + target.Z) / target.Length);
            Rotations = new Vector3((float) yaw,(float) pitch,(float) roll) * (float)(180/Math.PI);*/
        }


        public override void Draw(ShaderProgram shaderProgram)
        {
            base.Draw(shaderProgram);
        }
    }
}