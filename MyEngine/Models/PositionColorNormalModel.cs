using System;
using System.Collections.Generic;
using System.Linq;
using MyEngine.Models;
using MyEngine.Models.Voxel;
using MyEngine.DataStructures;
using MyEngine.ShaderImporter;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace MyEngine
{
    public class PositionColorNormalModel : Model
    {
        public event Action<PositionColorNormalModel> OnUpdate;
        public PositionColorNormalModel(PositionColorNormalVertex[] vertices, uint[] indices, string modelname = "untitled")
        {
            Vertices = vertices;
            Indices = indices;
            name = modelname + ID;
        }

        public uint[] Indices { get; set; }

        public PositionColorNormalVertex[] Vertices;

        public override void InitBuffers()
        {
            if (IsReady || Vertices == null || Indices == null)
                return;

            Vao = GL.GenVertexArray();
            Vbo = GL.GenBuffer();
            Ebo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vertices.Length * sizeof(float) * 10), Vertices, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices.Length * sizeof(uint)), Indices, BufferUsageHint.DynamicDraw);


            // Vertices positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 10, 0);
           
            // Color attribute
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 10, 3 * sizeof(float));

            // Normal

            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, sizeof(float) * 10, 7 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            GL.BindVertexArray(0);

            IsReady = true;
            OnUpdate?.Invoke(this);
        }

        public override void Draw(ShaderProgram shader)
        {
            if (!IsReady)
            {
                InitBuffers();
            }

            GL.BindVertexArray(Vao);
            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

    }
}