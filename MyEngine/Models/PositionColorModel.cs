using System;
using System.Collections.Generic;
using System.Linq;
using BulletSharp;
using BulletSharp.Math;
using MyEngine.Assets.Models;
using MyEngine.Assets.Models.Voxel;
using MyEngine.DataStructures;
using MyEngine.Models.Voxel;
using MyEngine.ShaderImporter;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace MyEngine
{
    public class PositionColorModel : Model
    {
        public event Action<PositionColorModel> OnUpdate;
        public PositionColorModel(PositionColorVertex[] vertices, uint[] indices,string modelname = "untitled")
        {
            Vertices = vertices;
            Indices = indices;
            name = modelname + ID;
        }

        public uint[] Indices { get; set; }

        public PositionColorVertex[] Vertices;

        public override void InitBuffers()

        {
            if(IsInitialized || Vertices == null || Indices == null)
                return;

            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer,new IntPtr(Vertices.Length*sizeof(float)*7), Vertices, BufferUsageHint.StaticDraw );
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices.Length * sizeof(uint)),Indices, BufferUsageHint.StaticDraw);

            
            // Vertices positions
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float)*7, 0);
            GL.EnableVertexAttribArray(0);
            // Color attribute
            
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float)*7, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(0);
            IsInitialized = true;

            OnUpdate?.Invoke(this);
        }

        public override RigidBody GetRigitBody()
        {
            var minX = Vertices.Min(x => x.Position.X);
            var maxX = Vertices.Max(x => x.Position.X);
            var minY = Vertices.Min(x => x.Position.Y);
            var maxY = Vertices.Max(x => x.Position.Y);
            var minZ = Vertices.Min(x => x.Position.Z);
            var maxZ = Vertices.Max(x => x.Position.Z);
            var shape = new BoxShape(maxX - minX,maxY-minY,maxZ - minZ);
            var localInertia = shape.CalculateLocalInertia(mass);
            var motionstat = new DefaultMotionState(MathHelpers.Matrix4toMatrix(Modelmatrix));
            var rbInfo = new RigidBodyConstructionInfo(mass, motionstat, shape, localInertia);
            var rigidbody = new RigidBody(rbInfo);
            rigidbody.UserObject = name;
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
            var shape = new BoxShape(maxX - minX,maxY-minY,maxZ - minZ);
            var localInertia = shape.CalculateLocalInertia(mass);
            var motionstat = new DefaultMotionState(MathHelpers.Matrix4toMatrix(Modelmatrix*view));
            var rbInfo = new RigidBodyConstructionInfo(mass, motionstat, shape, localInertia);
            var rigidbody = new RigidBody(rbInfo);
            rigidbody.UserObject = name;
            return rigidbody;
        }

        public override void Draw(ShaderProgram shader)
        {
            if (IsInitialized){ 
                GL.BindVertexArray(VAO);
                GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            
        }
    }
}
