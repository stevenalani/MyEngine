using OpenTK;

namespace MyEngine.Models.Voxel
{
    public abstract class Volume : PositionColorModel
    {
        public Vector3 Dimensions;
        protected int[,,] VolumeData;
        protected bool[,,] CheckedInVoxels;
        protected Volume(string name = "unnamed") : base(null, null, name) { }
        public int GetVoxel(int x, int y, int z)
        {
            return VolumeData[x, y, z];
        }

        public bool IsVoxel(int x, int y, int z)
        {
            return VolumeData[x, y, z] != 0;
        }
        public abstract void SetVoxel(Vector3 position, Vector4 mat);
        public abstract void SetVoxel(int posx, int posy, int posz, Vector4 color);
        public abstract void SetVoxel(int x, int y, int z, int colorIndex);
        public abstract void ClearVoxel(int x, int y, int z);
        public abstract void ComputeVerticesAndIndices();

        public bool IsValidVoxelPosition(int x, int y, int z)
        {
            if (x >= 0 && x < Dimensions.X && y >= 0 && y < Dimensions.Y && z >= 0 && z < Dimensions.Z) 
                return true;

            return false;
        }


    }
}