namespace MyEngine.Models.Voxel
{
    public struct VoxelIndices
    {
        public int X;
        public int Y;
        public int Z;

        public VoxelIndices(int x = -1, int y = -1, int z = -1)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}