﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public PositionColorVertex[] Vertices { get; set; }

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

        public override void Draw(ShaderProgram shader)
        {
            if (IsInitialized){ 
                GL.BindVertexArray(VAO);
                GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            
        }
    }
}
