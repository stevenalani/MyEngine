﻿using MyEngine.DataStructures;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEngine.Models.Voxel
{
    public class BigColorVolume : Volume
    {
        public Vector3 ChunkCount;
        int ChunkSize;
        BigColorVolumeChunk[,,] Chunks;
        private bool[,,] ChunkHasChanges;

        public BigColorVolume(int dimensionsX, int dimensionsY, int dimensionsZ, float cubeScale = 1f, int chunksize = 16)
        {
            Dimensions = new Vector3(dimensionsX, dimensionsY, dimensionsZ);
            ChunkSize = chunksize;
            CubeScale = cubeScale;
            var csx = (int)Math.Ceiling(Dimensions.X / chunksize);
            var csy = (int)Math.Ceiling(Dimensions.Y / chunksize);
            var csz = (int)Math.Ceiling(Dimensions.Z / chunksize);
            ChunkCount = new Vector3(csx, csy, csz);
            Chunks = new BigColorVolumeChunk[csx, csy, csz];
            ChunkHasChanges = new bool[csx, csy, csz];
            InitializeChunks();
        }

        private void InitializeChunks()
        {
            for (uint z = 0; z < ChunkCount.Z; z++)
            {
                for (uint y = 0; y < ChunkCount.Y; y++)
                {
                    for (uint x = 0; x < ChunkCount.X; x++)
                    {
                        Chunks[x, y, z] = new BigColorVolumeChunk(ChunkSize, x, y, z);
                        var chunk = Chunks[x, y, z];
                        chunk.CubeScale = CubeScale;
                        chunk.Position.X = ChunkSize * x * CubeScale + Position.X;
                        chunk.Position.Y = ChunkSize * y * CubeScale + Position.Y;
                        chunk.Position.Z = ChunkSize * z * CubeScale + Position.Z;
                    }
                }
            }
        }

        public override void SetVoxel(int x, int y, int z, int materialIndex)
        {
            var chunkIdxX = x / ChunkSize;
            var chunkIdxY = y / ChunkSize;
            var chunkIdxZ = z / ChunkSize;

            var voxelPosX = x % ChunkSize;
            var voxelPosY = y % ChunkSize;
            var voxelPosZ = z % ChunkSize;
            var currentColor =  Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].GetVoxel(voxelPosX, voxelPosY, voxelPosZ);
            if (currentColor != materialIndex)
            {
                Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].SetVoxel(voxelPosX, voxelPosY, voxelPosZ, materialIndex);
                ChunkHasChanges[chunkIdxX, chunkIdxY, chunkIdxZ] = true;
                IsReady = false;
            }
        }

        public override void SetVoxel(Vector3 position, Material material)
        {
            SetVoxel((int)position.X, (int)position.Y, (int)position.Z, material);
        }

        public override void SetVoxel(Vector3 position, int materialIndex)
        {
            SetVoxel((int)position.X, (int)position.Y, (int)position.Z, materialIndex);
        }

        public override void SetVoxel(int x, int y, int z, Material material)
        {
            var chunkIdxX = x / ChunkSize;
            var chunkIdxY = y / ChunkSize;
            var chunkIdxZ = z / ChunkSize;

            var voxelPosX = x % ChunkSize;
            var voxelPosY = y % ChunkSize;
            var voxelPosZ = z % ChunkSize;

            
            var currentColor = Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].GetMaterial(voxelPosX, voxelPosY, voxelPosZ);
            if (currentColor != material || material.Color.W != 0.0f)
            {
                Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].SetVoxel(voxelPosX, voxelPosY, voxelPosZ, material);
                ChunkHasChanges[chunkIdxX, chunkIdxY, chunkIdxZ] = true;
                IsReady = false;
            }
        }

        public override void ClearVoxel(int x, int y, int z)
        {
            SetVoxel(x, y, z, 0);
        }

        public override void ComputeVerticesAndIndices()
        {
            ComputeVertices();
            //ComputeIndices();
            
        }
        public override void InitBuffers()
        {
            ComputeVerticesAndIndices();
            base.InitBuffers();
        }

        public void ComputeVertices()
        {
            List<PositionColorNormalVertex> vertices = new List<PositionColorNormalVertex>();
            List<uint> indices = new List<uint>();
            for (var z = 0; z < ChunkCount.Z; z++)
            {
                for (var y = 0; y < ChunkCount.Y; y++)
                {
                    for (var x = 0; x < ChunkCount.X; x++)
                    {
                        var chunk = Chunks[x, y, z];
                        if (ChunkHasChanges[x, y, z])
                        {
                            chunk.ComputeVertices();
                        }
                        ChunkHasChanges[x, y, z] = false;
                        if (chunk.Vertices == null) continue;
                        uint vertexCount = (uint)vertices.Count;
                        vertices.AddRange(chunk.Vertices);
                        indices.AddRange(chunk.Indices.Select(index => index + vertexCount));
                    }
                }
            }
            Vertices = vertices.ToArray();
            Indices = indices.ToArray();
            vertices.Clear();
            indices.Clear();
            vertices = null;
            indices = null;

        }
        public void ComputeIndices()
        {
            if (Vertices == null) return;

            var vxlcnt = Vertices.Length / 8;
            var indices = new List<uint>();
            for (uint i = 0; i < vxlcnt; i++)
                indices.AddRange(CubeData.Indices.Select(x => x + i * 8).ToList());

            Indices = indices.ToArray();

        }
    }
}