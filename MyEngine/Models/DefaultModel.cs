using System;
using System.Linq;
using BulletSharp;
using MyEngine.DataStructures;
using MyEngine.ShaderImporter;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace MyEngine.Assets.Models
{


    public class DefaultModel :Model
    {
        private readonly int[] _vertices;
        private readonly int[] _indices;

        public unsafe DefaultModel(int[] vertices, int[] indices, int[]normals)
        {
            _vertices = vertices;
            _indices = indices;
            this.IndicesCnt = indices.Length;
        }
        public PositionColorVertex[] Vertices;

        public override RigidBody GetRigitBody()
        {
            var minX = Vertices.Min(x => x.Position.X);
            var maxX = Vertices.Max(x => x.Position.X);
            var minY = Vertices.Min(x => x.Position.Y);
            var maxY = Vertices.Max(x => x.Position.Y);
            var minZ = Vertices.Min(x => x.Position.Z);
            var maxZ = Vertices.Max(x => x.Position.Z);
            var shape = new BoxShape(maxX - minX, maxY - minY, maxZ - minZ);
            var localInertia = shape.CalculateLocalInertia(mass);
            var motionstat = new DefaultMotionState(MathHelpers.Matrix4toMatrix(Modelmatrix));
            var rbInfo = new RigidBodyConstructionInfo(mass, motionstat, shape, localInertia);

            var rigidbody = new RigidBody(rbInfo);
            return rigidbody;
        }
        public override RigidBody GetRigitBody(Matrix4 view)
        {
            var minX = Vertices.Min(x => x.Position.X);
            var maxX = Vertices.Max(x => x.Position.X);
            var minY = Vertices.Min(x => x.Position.Y);
            var maxY = Vertices.Max(x => x.Position.Y);
            var minZ = Vertices.Min(x => x.Position.Z);
            var maxZ = Vertices.Max(x => x.Position.Z);
            var shape = new BoxShape(maxX - minX, maxY - minY, maxZ - minZ);
            var localInertia = shape.CalculateLocalInertia(mass);
            var motionstat = new DefaultMotionState(MathHelpers.Matrix4toMatrix(Modelmatrix * view));
            var rbInfo = new RigidBodyConstructionInfo(mass, motionstat, shape, localInertia);

            var rigidbody = new RigidBody(rbInfo);
            return rigidbody;
        }

        public override void Draw(ShaderProgram shader)
        {
            GL.UseProgram(shader.ID);
            GL.DrawElements(BeginMode.Triangles,
                IndicesCnt,
                DrawElementsType.UnsignedInt,
                0
            );
        }

        public float[] VertexData { get; set; }
        public float[] IndexData { get; set; }

        public int IndicesCnt { get; set; }



        public override void InitBuffers()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * PositionColorNormalVertex.Size, _vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(int), _indices, BufferUsageHint.StaticDraw);

            // Vertices positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, IntPtr.Zero);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, IntPtr.Zero);


            GL.BindVertexArray(0);
            IsReady = true;
        }


        public bool Ready()
        {
            return this.IsReady;
        }
    }
}