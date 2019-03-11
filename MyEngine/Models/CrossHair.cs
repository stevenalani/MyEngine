using System;
using System.Runtime.CompilerServices;
using Assimp.Configs;
using MyEngine.DataStructures;
using MyEngine.Logging;
using MyEngine.ShaderImporter;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace MyEngine.Assets.Models
{
    internal class CrossHair : PositionColorModel
    {
        public ShaderProgram Shader;
        private Engine engine;
        //public new Matrix4 model;

        public CrossHair(Engine engine, Vector4 color) : base(null, null)
        {
            
            this.engine = engine;
            Position = Vector3.Zero;
            Scales = Vector3.One * 0.01f;
            this.Color = color;
            Shader = engine.shaderManager.GetFirst();// new ShaderProgram("Shaders\\DefaultCrosshairShader.vs", "Shaders\\DefaultCrosshairShader.fs");
            Vertices = new[]
            {
                new PositionColorVertex {color = color, position = new Vector3(-engine.Width/2f, engine.Height/2f,-10f)},
            };
            Indices = new uint[] {0};
           
            engine.Load += InitBuffers;
            engine.Load += SetupShader;
            //engine.Camera.CameraMoved += Update;
        }

        private void InitBuffers(object sender, EventArgs e)
        {
            base.InitBuffers();
        }

        private void SetupShader(object sender, EventArgs e)
        {
            if (!Shader.IsCompiled)
                Shader.SetupShader();
        }

        public Vector4 Color { get; set; }

        private void Update(object sender, CameraMovedEventArgs e)
        {
            //model = MathHelpers.getRotation(Rotations.X, Rotations.Y, Rotations.Z) * Matrix4.CreateTranslation(Orgin) * Matrix4.CreateScale(Scales) * e.ViewMatrix;
            //Orgin = e.Orgin + Vector3.Normalize(e.Orientation);
        }

        public void Draw()
        {
        
            Draw(Shader);
        }
        public override void Draw(ShaderProgram shader)
        {
            if (IsInitialized)
            {
                Matrix4 proj = engine.Camera.GetProjection(PROJECTIONTYPE.Orthogonal);
                Matrix4 view = engine.Camera.GetView();
                Matrix4 modelmat = model * view.Inverted();
                shader.use();
                shader.SetUniformMatrix4X4("projection",proj);
                shader.SetUniformMatrix4X4("view",view);
                shader.SetUniformMatrix4X4("model", modelmat);
                GL.PointSize(2f);
                
                GL.BindVertexArray(VAO);
                GL.DrawElements(BeginMode.Points, Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }
    }
}