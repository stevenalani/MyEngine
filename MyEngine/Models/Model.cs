using System;
using System.Drawing.Drawing2D;
using GlmNet;
using MyEngine.Models;
using MyEngine.DataStructures;
using MyEngine.ShaderImporter;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyEngine
{
    public abstract class Model : IDisposable
    {
        private static int _nextId = 0;
        public readonly int ID;
        public string name;

        protected int Vao = -1;
        protected int Vbo = -1;
        protected int Ebo = -1;

        public OpenTK.Vector3 Position = Vector3.Zero;
        public Vector3 Scales = Vector3.One;
        protected internal Vector3 Rotations = Vector3.Zero;


        public Vector3 Direction = Vector3.UnitZ;
        public Matrix4 Modelmatrix => Matrix4.CreateTranslation(-PivotPoint) * 
                                      MathHelpers.GetRotation(Rotations.X, Rotations.Y, Rotations.Z) * 
                                      Matrix4.CreateTranslation(PivotPoint) * 
                                      Matrix4.CreateScale(Scales) *
                                      Matrix4.CreateTranslation(Position);

        public Vector3 PivotPoint = Vector3.Zero;

        protected Model()
        {
            ID = Model._nextId++;
        }

        public bool IsReady { get; set; }


        public abstract void Draw(ShaderProgram shaderProgram);

        public abstract void InitBuffers();
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (IsReady)
                {
                    GL.DeleteVertexArray(Vao);
                    GL.DeleteBuffer(Vbo);
                    IsReady = false;
                }
            }
        }

        public void MoveToVector(Vector3 newposition)
        {
            this.Position = newposition;
        }

        public void MoveForward(float distance)
        {
            this.Position += Direction * distance;
        }

        public void rotateX(float yaw)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(yaw, 0f, 0f);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
            Rotations.X += yaw;
        }

        public void rotateY(float pitch)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(0f, pitch, 0f);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
            Rotations.Y += pitch;
        }
        public void rotateZ(float roll)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(0f, 0f, roll);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
            Rotations.Z += roll;
        }

        public void rotate(float yaw, float pitch, float roll)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(yaw, pitch, roll);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
        }
        public void rotate(Vector3 yawpitchroll)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(yawpitchroll.X, yawpitchroll.Y, yawpitchroll.Z);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
        }


    }
}