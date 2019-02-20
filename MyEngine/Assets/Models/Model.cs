using System;
using GlmNet;
using MyEngine.ShaderImporter;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyEngine
{
    public abstract class Model: IDisposable
    {
        private static int nextid = 0;
        public readonly int ID;
        public int VAO;

        protected int VBO;
        protected int EBO;

        protected Vector3 Position = Vector3.Zero;
        protected Vector3 Scales = Vector3.One;
        protected Vector3 Rotations = Vector3.Zero;

        private Vector3 Direction = Vector3.UnitZ;

        public Matrix4 model =>  MathHelpers.getRotation(Rotations.X, Rotations.Y,Rotations.Z) * Matrix4.CreateTranslation(Position)*  Matrix4.CreateScale(Scales);
            
        

        protected Model()
        {
            ID  = Model.nextid++;
        }
        public bool IsInitialized { get; set; }


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
                if (IsInitialized)
                {
                    GL.DeleteVertexArray(VAO);
                    GL.DeleteBuffer(VBO);
                    IsInitialized = false;
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
            var rotationMatrix4 = MathHelpers.getRotation(yaw, 0f, 0f);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
        }

        public void rotateY(float pitch)
        {
            var rotationMatrix4 = MathHelpers.getRotation(0f,pitch, 0f);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
        }
        public void rotateZ(float roll)
        {
            var rotationMatrix4 = MathHelpers.getRotation(0f,0f, roll);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
        }

        public void rotate(float yaw, float pitch, float roll)
        {
            var rotationMatrix4 = MathHelpers.getRotation(yaw, pitch, roll);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
        }
        public void rotate(Vector3 yawpitchroll)
        {
            var rotationMatrix4 = MathHelpers.getRotation(yawpitchroll.X,yawpitchroll.Y,yawpitchroll.Z);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
        }

    }
}