using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MyEngine.Assets.Models.Voxel
{
    public class Volume
    {
        // Stores the Voxel Material
        private List<VoxelInformation> VolumeData = new List<VoxelInformation>();
        private Vector3 dimensions;
        private uint voxelscount = 0;

        public PositionColorVertex[] Vertices = null;
        public uint[] Indices;

        public Volume(Vector3 dimensions)
        {
            this.dimensions = dimensions;
        }

        public Volume(int sizeInformationX, int sizeInformationY, int sizeInformationZ)
        {
            this.dimensions = new Vector3(sizeInformationX,sizeInformationY,sizeInformationZ);
        }

        public void SetVoxel(Vector3 pos, Vector4 mat)
        {
            if (!(pos.X <= dimensions.X && pos.Y <= dimensions.Y && pos.Z <= dimensions.Z)) return;
            VolumeData.Add(new VoxelInformation()
            {
                Color = mat,
                Posindices = pos,

            });
            voxelscount++;
        }

        private void SortData()
        {
            VolumeData = VolumeData.OrderBy(x => x.Posindices.X).ThenBy(x => x.Posindices.Y).ToList();
        }

        private bool IsSameColorFront(VoxelInformation voxel)
        {
            var front = voxel.Posindices + Vector3.UnitZ;
            var result = VolumeData.FirstOrDefault(x => x.Posindices == front);
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorBack(VoxelInformation voxel)
        {
            var front = voxel.Posindices - Vector3.UnitZ;
            var result = VolumeData.FirstOrDefault(x => x.Posindices == front);
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorRight(VoxelInformation voxel)
        {
            var front = voxel.Posindices + Vector3.UnitX;
            var result = VolumeData.FirstOrDefault(x => x.Posindices == front);
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorLeft(VoxelInformation voxel)
        {
            var front = voxel.Posindices - Vector3.UnitX;
            var result = VolumeData.FirstOrDefault(x => x.Posindices == front);
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorUp(VoxelInformation voxel)
        {
            var front = voxel.Posindices + Vector3.UnitY;
            var result = VolumeData.FirstOrDefault(x => x.Posindices == front);
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorDown(VoxelInformation voxel)
        {
            var front = voxel.Posindices - Vector3.UnitY;
            var result = VolumeData.FirstOrDefault(x => x.Posindices == front);
            return (result.Color == voxel.Color);
        }  
      
        public void ComputeVertices()
        {
            SortData();
            List<PositionColorVertex> poscolresult = new List<PositionColorVertex>();
            Queue<VoxelInformation> processQueue = new Queue<VoxelInformation>(); 
            foreach (var voxelInformation in VolumeData)
            {
                processQueue.Enqueue(voxelInformation);
            }

            VoxelInformation currentvoxel = new VoxelInformation();
            while (processQueue.Count != 0)
            {
                currentvoxel = processQueue.Dequeue();
                var front =  IsSameColorFront(currentvoxel);
                var back = IsSameColorBack(currentvoxel);
                var right= IsSameColorRight(currentvoxel);
                var left = IsSameColorLeft(currentvoxel);
                var up = IsSameColorUp(currentvoxel);
                var down = IsSameColorDown(currentvoxel);
            }
            //var currentvoxel = VolumeData.First();
            List<VoxelInformation> samecolored;
           // if (currentvoxel.Color != Vector4.Zero)
           
            for (int currentX = (int)currentvoxel.Posindices.X; currentX < dimensions.X; currentX++)
            {
                
                for (int currentY = (int)currentvoxel.Posindices.Y; currentY < dimensions.Y; currentY++)
                {
                    for (int currentZ = (int)currentvoxel.Posindices.Z; currentZ < dimensions.Z; currentZ++)
                    {


                        var translation = dimensions / 2;
                        //checkRestOfPlane(currventX,currentY,currentZ);
                        var result2 = CubeData.NotCentered.Select(x => new PositionColorVertex
                            {position = x + new Vector3(currentX,  currentY, currentZ)-translation,color = currentvoxel.Color/255});
                        poscolresult.AddRange(result2);
                    }
                }
            }

            this.Vertices = poscolresult.ToArray();
        }

        private void checkRestOfPlane(int currentX, int currentY, int currentZ)
        {

            
        }

        public void ComputeIndices()
       {
           List<uint> indices = new List<uint>();
           for(uint i = 0; i < voxelscount; i++)
           {
               indices.AddRange(CubeData.Indices.Select(x => x + (i*8)).ToList());
           }

           this.Indices = indices.ToArray();
       }
    }

    internal struct VoxelInformation
    {
        public Vector4 Color;
        public Vector3 Posindices;
    }
}
