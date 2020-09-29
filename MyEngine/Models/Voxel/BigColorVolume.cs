using MyEngine.DataStructures;
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

        public BigColorVolume(int DimensionsX, int DimensionsY, int DimensionsZ, int chunksize = 16)
        {
            Dimensions = new Vector3(DimensionsX, DimensionsY, DimensionsZ);
            ChunkSize = chunksize;
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
                        chunk.Position.X = ChunkSize * x + Position.X;
                        chunk.Position.Y = ChunkSize * y + Position.Y;
                        chunk.Position.Z = ChunkSize * z + Position.Z;
                    }
                }
            }
        }

        public override void SetVoxel(int x, int y, int z, int colorIndex)
        {
            var chunkIdxX = x / ChunkSize;
            var chunkIdxY = y / ChunkSize;
            var chunkIdxZ = z / ChunkSize;

            var voxelPosX = x % ChunkSize;
            var voxelPosY = y % ChunkSize;
            var voxelPosZ = z % ChunkSize;
            var currentColor =  Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].GetVoxel(voxelPosX, voxelPosY, voxelPosZ);
            if (currentColor != colorIndex)
            {
                Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].SetVoxel(voxelPosX, voxelPosY, voxelPosZ, colorIndex);
                ChunkHasChanges[chunkIdxX, chunkIdxY, chunkIdxZ] = true;
            }
        }

        public override void SetVoxel(Vector3 position, Vector4 color)
        {
            SetVoxel((int)position.X, (int)position.Y, (int)position.Z, color);
        }

        public override void SetVoxel(int x, int y, int z, Vector4 color)
        {
            var chunkIdxX = x / ChunkSize;
            var chunkIdxY = y / ChunkSize;
            var chunkIdxZ = z / ChunkSize;

            var voxelPosX = x % ChunkSize;
            var voxelPosY = y % ChunkSize;
            var voxelPosZ = z % ChunkSize;

            
            var currentColor = Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].GetColor(voxelPosX, voxelPosY, voxelPosZ);
            if (currentColor != color || color.W != 0.0f)
            {
                Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].SetVoxel(voxelPosX, voxelPosY, voxelPosZ, color);
                ChunkHasChanges[chunkIdxX, chunkIdxY, chunkIdxZ] = true;
            }
        }

        public override void ClearVoxel(int x, int y, int z)
        {
            SetVoxel(x, y, z, 0);
        }

        public override void ComputeVerticesAndIndices()
        {
            List<PositionColorVertex> vertices = new List<PositionColorVertex>();
            for (var z = 0; z < ChunkCount.Z; z++)
            {
                for (var y = 0; y < ChunkCount.Y; y++)
                {
                    for (var x = 0; x < ChunkCount.X; x++)
                    {
                        var chunk = Chunks[x, y, z];
                        chunk.ComputeVertices();
                        if (chunk.Vertices == null) continue;
                        vertices.AddRange(chunk.Vertices);
                    }
                }
            }
            Vertices = vertices.ToArray();
            ComputeIndices();
            vertices.Clear();
        }
        public override void InitBuffers()
        {
            ComputeVerticesAndIndices();
            base.InitBuffers();
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