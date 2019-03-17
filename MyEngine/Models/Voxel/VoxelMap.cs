using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletSharp;
using OpenTK;

namespace MyEngine.Models.Voxel
{
    class VoxelMap : Volume
    {
        public VoxelMap(Vector3 itemsperaxis, int offset,int[] data) : base(default(Vector3))
        {
            var deltaheight = data.Max( x => x ) - data.Min( x => x );
            VolumeData = new Voxel[(int)itemsperaxis.X * offset, deltaheight,(int)itemsperaxis.Y * offset];

            collisionShape = new HeightfieldTerrainShape((int)itemsperaxis.X, offset, new IntPtr(data[0]), 1f, 0, 90, 1, PhyScalarType.Int32, false);
        }

        public VoxelMap(int witdh, int height, int depth) : base(witdh, height, depth)
        {
        }
    }
}
