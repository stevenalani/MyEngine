﻿using System;
using MyEngine.ShaderImporter;
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
            IsInitialized = true;
        }


        public bool Ready()
        {
            return this.IsInitialized;
        }
    }
}