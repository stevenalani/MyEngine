using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MyEngine.Models
{
    public class CubeWithNormals : PositionColorNormalModel
    {
        public static readonly Vector3[] CubeVertices = {
            new Vector3(-0.5f,-0.5f,-0.5f),
            new Vector3(0.5f,-0.5f,-0.5f),
            new Vector3(0.5f,0.5f,-0.5f),
            new Vector3(-0.5f,0.5f,-0.5f),

            new Vector3(-0.5f,-0.5f,0.5f),
            new Vector3(0.5f,-0.5f,0.5f),
            new Vector3(0.5f,0.5f,0.5f),
            new Vector3(-0.5f,0.5f,0.5f),
        };

        public static PositionColorNormalVertex[] FrontFace(Vector3 positionOffset, Vector4 color, float scale = 1f) => new[]
        {
            new PositionColorNormalVertex() 
                {Position = (CubeVertices[0] + positionOffset) * scale , Normal = -Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex() 
                {Position = (CubeVertices[1] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex() 
                {Position = (CubeVertices[2] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex() 
                {Position = (CubeVertices[2] + positionOffset) * scale , Normal = -Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex() 
                {Position = (CubeVertices[3] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex() 
                {Position = (CubeVertices[0] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
        };

        public static PositionColorNormalVertex[] BackFace(Vector3 positionOffset, Vector4 color, float scale = 1f) => new[]
        {
            new PositionColorNormalVertex()
                {Position = (CubeVertices[7] + positionOffset ) * scale , Normal = Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[6] + positionOffset ) * scale, Normal = Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[5] + positionOffset ) * scale, Normal = Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[5] + positionOffset ) * scale, Normal = Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[4] + positionOffset ) * scale, Normal = Vector3.UnitZ, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[7] + positionOffset ) * scale, Normal = Vector3.UnitZ, Color = color},
        };

        public static PositionColorNormalVertex[] LeftFace(Vector3 positionOffset, Vector4 color, float scale = 1f) => new[]
        {
            new PositionColorNormalVertex()
                {Position = (CubeVertices[4] + positionOffset ) * scale, Normal = -Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[0] + positionOffset ) * scale, Normal = -Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[3] + positionOffset ) * scale, Normal = -Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[3] + positionOffset ) * scale, Normal = -Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[7] + positionOffset ) * scale, Normal = -Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[4] + positionOffset ) * scale, Normal = -Vector3.UnitX, Color = color},
        };
        public static PositionColorNormalVertex[] RightFace(Vector3 positionOffset, Vector4 color, float scale = 1f) => new[]
        {
            new PositionColorNormalVertex()
                {Position = (CubeVertices[1] + positionOffset ) * scale, Normal = Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[5] + positionOffset ) * scale, Normal = Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[6] + positionOffset ) * scale, Normal = Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[6] + positionOffset ) * scale, Normal = Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[2] + positionOffset ) * scale, Normal = Vector3.UnitX, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[1] + positionOffset ) * scale, Normal = Vector3.UnitX, Color = color},
        };

        public static PositionColorNormalVertex[] BottomFace(Vector3 positionOffset, Vector4 color, float scale = 1f) => new[]
        {
            new PositionColorNormalVertex()
                {Position = (CubeVertices[4] + positionOffset ) * scale, Normal = -Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[5] + positionOffset ) * scale, Normal = -Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[1] + positionOffset ) * scale, Normal = -Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[1] + positionOffset ) * scale, Normal = -Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[0] + positionOffset ) * scale, Normal = -Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[4] + positionOffset ) * scale, Normal = -Vector3.UnitY, Color = color},
        };

        public static PositionColorNormalVertex[] TopFace(Vector3 positionOffset, Vector4 color, float scale = 1f) => new[]
        {
            new PositionColorNormalVertex()
                {Position = (CubeVertices[3] + positionOffset ) * scale, Normal = Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[2] + positionOffset ) * scale, Normal = Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[6] + positionOffset ) * scale, Normal = Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[6] + positionOffset ) * scale, Normal = Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[7] + positionOffset ) * scale, Normal = Vector3.UnitY, Color = color},
            new PositionColorNormalVertex()
                {Position = (CubeVertices[3] + positionOffset ) * scale, Normal = Vector3.UnitY, Color = color},
        };
        public CubeWithNormals(Vector3 offset, Vector4 color) :
            base(null, null, "cube")
        {
            List<PositionColorNormalVertex> vertices = new List<PositionColorNormalVertex>();
            List<uint> indices = new List<uint>();
            vertices.AddRange(FrontFace(offset,color/255));
            vertices.AddRange(BackFace(offset, color / 255));
            vertices.AddRange(LeftFace(offset, color / 255));
            vertices.AddRange(RightFace(offset, color / 255));
            vertices.AddRange(BottomFace(offset, color / 255));
            vertices.AddRange(TopFace(offset, color / 255));
            Vertices = vertices.ToArray();
            for (uint i = 0; i < 6; i++)
            {
                indices.AddRange(new []{
                    0 + 6 * i, 
                    1 + 6 * i,
                    2 + 6 * i,
                    2 + 6 * i,
                    3 + 6 * i,
                    0 + 6 * i});
            }
            Indices = indices.ToArray();
        }
        public CubeWithNormals() :
            base(null, null, "cube")
        {
            var color = new Vector4(10, 150, 255, 255) / 255;
            var offset = Vector3.Zero;
            List<PositionColorNormalVertex> vertices = new List<PositionColorNormalVertex>();
            List<uint> indices = new List<uint>();
            vertices.AddRange(FrontFace(offset, color));
            vertices.AddRange(BackFace(offset, color));
            vertices.AddRange(LeftFace(offset, color));
            vertices.AddRange(RightFace(offset, color));
            vertices.AddRange(BottomFace(offset, color));
            vertices.AddRange(TopFace(offset, color));
            Vertices = vertices.ToArray();
            /*for (uint i = 0; i < 6; i++)
            {
                indices.AddRange(new[]{
                    0 + 6 * i,
                    1 + 6 * i,
                    2 + 6 * i,
                    2 + 6 * i,
                    3 + 6 * i,
                    0 + 6 * i});
            }*/
            for (uint i = 0; i < Vertices.Length; i++)
            {
                indices.Add(i);
            }
            Indices = indices.ToArray();
        }
    }
}
