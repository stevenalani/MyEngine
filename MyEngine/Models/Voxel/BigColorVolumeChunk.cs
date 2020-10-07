using MyEngine.DataStructures;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace MyEngine.Models.Voxel
{
    public class BigColorVolumeChunk : ColorVolume
    {
        private uint ChunkIdX;
        private uint ChunkIdY;
        private uint ChunkIdZ;

        public BigColorVolumeChunk(int dimensions, uint idX, uint idY, uint idZ) : base(dimensions, dimensions, dimensions)
        {

            ChunkIdX = idX;
            ChunkIdY = idY;
            ChunkIdZ = idZ;
        }

        public override void ComputeVertices()
        {
            base.ComputeVertices();
            var tempVertices = Vertices.Select( x => new PositionColorNormalVertex()
            {
                Color  = x.Color,
                Position = x.Position + this.Position,
                Normal = x.Normal
            });
            Vertices = tempVertices.ToArray();
        }
    }
}