using System;
using System.Drawing.Drawing2D;
using BulletSharp;
using GlmNet;
using MyEngine.Assets.Models;
using MyEngine.DataStructures;
using MyEngine.ShaderImporter;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyEngine
{
    public abstract class Model:IDisposable
    {
        private static int nextid = 0;
        public readonly int ID;
        public string name;
       
        public int VAO;

        protected int VBO;
        protected int EBO;

        public OpenTK.Vector3 Position = Vector3.Zero;
        public Vector3 Scales = Vector3.One;
        protected internal Vector3 Rotations = Vector3.Zero;

        protected IVertextype[] Vertices;

        public Vector3 Direction = -Vector3.UnitZ;
        public float mass;
        public bool CalculatePhysics = true;
        public Matrix4 Modelmatrix =>
            (PhysicsModelmatrix == Matrix4.Zero)? MathHelpers.getRotation(Rotations.X, Rotations.Y, Rotations.Z) * Matrix4.CreateScale(Scales) *
                                                  Matrix4.CreateTranslation(Position) : PhysicsModelmatrix;

        public Matrix4 PhysicsModelmatrix { get; set; }

        public CollisionObject collisionObject;

        public abstract RigidBody GetRigitBody();
        public abstract RigidBody GetRigitBody(Matrix4 view);

        protected Model()
        {
            ID = Model.nextid++;
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
                    GL.DeleteVertexArray(VAO);
                    GL.DeleteBuffer(VBO);
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
            var rotationMatrix4 = MathHelpers.getRotation(yaw, 0f, 0f);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
            Rotations.X += yaw;
        }

        public void rotateY(float pitch)
        {
            var rotationMatrix4 = MathHelpers.getRotation(0f,pitch, 0f);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
            Rotations.Y += pitch;
        }
        public void rotateZ(float roll)
        {
            var rotationMatrix4 = MathHelpers.getRotation(0f,0f, roll);
            this.Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
            Rotations.Z = roll;
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