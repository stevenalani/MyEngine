using System.Collections.Generic;
using System.Linq;
using MyEngine.DataStructures;
using OpenTK;

namespace MyEngine.Models.Voxel
{
    public abstract class Volume : PositionColorModel
    {
        public Vector3 Dimensions;
        protected int[,,] VolumeData;
        protected bool[,,] CheckedInVoxels;
        protected Volume(string name = "unnamed") : base(null, null, name) { }
        public abstract void SetVoxel(Vector3 position, Vector4 mat);
        public abstract void SetVoxel(int posx, int posy, int posz, Vector4 color);
        public abstract void SetVoxel(int x, int y, int z, int colorIndex);
        public abstract void ClearVoxel(int x, int y, int z);
        public abstract void ComputeVerticesAndIndices();

        
    }
}