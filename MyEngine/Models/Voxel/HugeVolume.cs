using System.Collections.Generic;
using MyEngine.Models.Voxel;
using OpenTK;

namespace MyEngine.Assets.Models.Voxel
{
    public class HugeVolume
    {
        private Volume[,,] volumeChunks;
        private Vector3 currentChunkPos = Vector3.Zero;
        public HugeVolume(int width, int height, int depth)
        {
            volumeChunks = new Volume[width,height,depth];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < depth; k++)
                    {
                        volumeChunks[i,j,k] = new Volume(width,height,depth);
                    }
                }
            }
        }

        public void SetVoxel(Vector3 VoxelPos, Vector4 VoxelColor, Vector3 ChunkPos = default(Vector3))
        {
            
            if (ChunkPos != default(Vector3))
            {
                volumeChunks[(int) ChunkPos.X, (int) ChunkPos.Y, (int) ChunkPos.Z].SetVoxel(VoxelPos,VoxelColor);
            }
            else
            {
                volumeChunks[(int) ChunkPos.X, (int) ChunkPos.Y, (int) ChunkPos.Z].SetVoxel(VoxelPos, VoxelColor);
            }
        }
    }
}