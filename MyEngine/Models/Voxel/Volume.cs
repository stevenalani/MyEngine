using System.Collections.Generic;
using OpenTK;

namespace MyEngine.Models.Voxel
{
    public abstract class Volume : PositionColorNormalModel
    {
        public Vector3 Dimensions;
        public short[,,] VolumeData;
        public bool[,,] CheckedInVoxels;
        public float CubeScale = 1f;
        public List<Material> Materials = new List<Material>() { new Material() };

        protected Volume(string name = "unnamed") : base(null, null, name) { }
        public int GetVoxel(int x, int y, int z)
        {
            return VolumeData[x, y, z];
        }

        public bool IsVoxel(int x, int y, int z)
        {
            return VolumeData[x, y, z] != 0;
        }
        public abstract void SetVoxel(Vector3 position, Material material);
        public abstract void SetVoxel(Vector3 position, int materialIndex);
        public abstract void SetVoxel(int posx, int posy, int posz, Material material);
        public abstract void SetVoxel(int x, int y, int z, int materialIndex);

        public abstract void ClearVoxel(int x, int y, int z);
        public abstract void ComputeVerticesAndIndices();
        public void AddMaterial(Material material)
        {
            if (!Materials.Contains(material))
            {
                Materials.Add(material);
            }
        }

        public int GetMaterialIndex(Material material)
        {
            return Materials.IndexOf(material);
        }
        public Material GetMaterial(int x, int y, int z)
        {
            return Materials[VolumeData[x, y, z]];
        }
        protected bool IsSameMaterialLeft(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x - 1, y, z];
        }

        protected bool IsSameMaterialUp(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y + 1, z];
        }

        protected bool IsSameMaterialDown(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y - 1, z];
        }
        protected bool IsSameMaterialFront(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y, z + 1];
        }

        protected bool IsSameMaterialBack(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y, z - 1];
        }

        protected bool IsSameMaterialRight(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x + 1, y, z];
        }
        public bool IsValidVoxelPosition(int x, int y, int z)
        {
            if (x >= 0 && x < Dimensions.X && y >= 0 && y < Dimensions.Y && z >= 0 && z < Dimensions.Z) 
                return true;

            return false;
        }


    }

    public class Material
    {
        public Vector4 Color = Vector4.Zero;
    }
}