using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;

namespace MyEngine.Models.Voxel.VoxelImporter
{
    public static class VoxelImporter
    {
        private static float VoxelSizeFactor = 0.5f;

        public static Volume LoadVoxelModelFromVox(string path)
        {
            List<Vector4> colorsList = new List<Vector4>();
            
            byte[] data = File.ReadAllBytes(path);
            string id = System.Text.Encoding.UTF8.GetString(data.Take(4).ToArray());
            int version = BitConverter.ToInt32(data.Skip(4).Take(4).ToArray(), 0);
            MainChunk chunk = new MainChunk(data.Skip(8).ToArray());
            var sizeInformation = (SizeChunk) chunk.ChildChunks.Select(x => x).Where(x => x is SizeChunk).First();
            var colorInformation = (RgbaChunk) chunk.ChildChunks.Select(x => x).Where(x => x is RgbaChunk).First();
            var voxelInformation = (XyziChunk) chunk.ChildChunks.Select(x => x).Where(x => x is XyziChunk).First();
            var dimensions = new Vector3(sizeInformation.X,sizeInformation.Y,sizeInformation.Z);
            Volume vol = new Volume(dimensions);

            foreach (var color in colorInformation.RGBA)
            {
                colorsList.Add(new Vector4(color.Item1, color.Item2, color.Item3, color.Item4));
            }

            foreach (var voxel in voxelInformation.Voxels)
            {
                vol.SetVoxel(new Vector3(dimensions.X - voxel.Item1 -1, voxel.Item3, voxel.Item2),colorsList[voxel.Item4]);
            }
            vol.ComputeVerticesAndIndices();
            return vol;
        }
    }
}