using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MyEngine.Models.Voxel
{
    class VoxelMap : Volume
    {

        public VoxelMap(Vector2 itemsperaxis, int offset,int[] data)
        {
            var minh = data.Min(x => x);
            var maxh = data.Max(x => x);
            var deltaheight = maxh - minh;
            size = new Vector3((int)itemsperaxis.X * offset, deltaheight+1, (int)itemsperaxis.Y * offset);           
            
            name = "Ground";
            InitializeVolumeData();
            
        }

        public VoxelMap(int witdh, int height, int depth) : base(witdh+1, height+1, depth+1)
        {
            name = "Ground";
        }


    }
}
