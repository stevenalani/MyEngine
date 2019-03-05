using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MyEngine.Assets.Models.Voxel
{
    public class Volume
    {
        // Stores the Voxel Material
        private VoxelInformation[,,] VolumeData;
        private Vector3 dimensions;
        private uint _voxelscount = 0;

        public PositionColorVertex[] Vertices = null;
        public uint[] Indices;
        private bool hasstartpoint;

        public Volume(Vector3 dimensions)
        {
            this.dimensions = dimensions;
            VolumeData = new VoxelInformation[(int) dimensions.X, (int) dimensions.Y, (int) dimensions.Z];
        }

        public Volume(int sizeInformationX, int sizeInformationY, int sizeInformationZ)
        {
            this.dimensions = new Vector3(sizeInformationX,sizeInformationY,sizeInformationZ);
            VolumeData = new VoxelInformation[sizeInformationX,sizeInformationY,sizeInformationZ];
        }

        public void SetVoxel(Vector3 pos, Vector4 mat)
        {
            if (!(pos.X <= dimensions.X && pos.Y <= dimensions.Y && pos.Z <= dimensions.Z)) return;
            VolumeData[(int) pos.X,(int) pos.Y,(int) pos.Z] = new VoxelInformation(pos,mat); 
            _voxelscount++;
        }

        

        private bool IsSameColorFront(VoxelInformation voxel)
        {
            var front = voxel.Posindices + Vector3.UnitZ;
            var result = VolumeData[(int)front.X, (int)front.Y, (int)front.Z];
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorBack(VoxelInformation voxel)
        {
            var front = voxel.Posindices - Vector3.UnitZ;
            var result = VolumeData[(int)front.X, (int)front.Y, (int)front.Z];
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorRight(VoxelInformation voxel)
        {
            var front = voxel.Posindices + Vector3.UnitX;
            var result = VolumeData[(int)front.X, (int)front.Y, (int)front.Z];
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorLeft(VoxelInformation voxel)
        {
            var front = voxel.Posindices - Vector3.UnitX;
            var result = VolumeData[(int)front.X, (int)front.Y, (int)front.Z];
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorUp(VoxelInformation voxel)
        {
            var front = voxel.Posindices + Vector3.UnitY;
            var result = VolumeData[(int) front.X,(int) front.Y,(int) front.Z];           
            return (result.Color == voxel.Color);
        }
        private bool IsSameColorDown(VoxelInformation voxel)
        {
            var front = voxel.Posindices - Vector3.UnitY;
            var result = VolumeData[(int)front.X, (int)front.Y, (int)front.Z];
            return (result.Color == voxel.Color);
        }  
      
        public void ComputeVertices()
        {
            var spacecenter = dimensions / 2;
            VoxelInformation currentvoxel;

            int countX = -1,countY = -1, countZ = -1;
            List<PositionColorVertex> poscolresult = new List<PositionColorVertex>();
            for (int currentZ = 0; currentZ < dimensions.Z; currentZ++)
            {
                for (int currentY = 0; currentY < dimensions.Y; currentY++)
                {
                    for (int currentX = 0; currentX < dimensions.X; currentX++)
                    {
                        currentvoxel = VolumeData[currentX, currentY, currentZ];
                        if (currentvoxel.Color == Vector4.Zero || currentvoxel.checkedin)
                        continue;
                        countX = GetNeighborsX(currentvoxel);
                        
                        for (int i = (int) currentvoxel.Posindices.X; i <= currentvoxel.Posindices.X + countX; i++)
                        {
                            var voxel = VolumeData[i, currentY, currentZ];
                            var voxelsAbove = GetNeighborsY(voxel);

                            var voxelsInfront = GetNeighborsZ(voxel);
                            if (voxelsAbove < countY || countY == -1)
                                countY = voxelsAbove;
                            if (voxelsInfront < countZ || countZ == -1)
                                countZ = voxelsInfront;
                        }
                        PositionColorVertex posxColorVertex = new PositionColorVertex()
                        {
                            color = currentvoxel.Color/255,
                            position = currentvoxel.Posindices
                        };
                        posxColorVertex.position.Z = currentvoxel.Posindices.Z + countZ + 1;
                        poscolresult.Add(posxColorVertex);
                        posxColorVertex.position.X = currentvoxel.Posindices.X + countX + 1;
                        poscolresult.Add(posxColorVertex);
                        posxColorVertex.position.Y = currentvoxel.Posindices.Y + countY + 1;
                        poscolresult.Add(posxColorVertex);
                        posxColorVertex.position.X = currentvoxel.Posindices.X;
                        poscolresult.Add(posxColorVertex);

                        
                        //Backface Vertex
                        posxColorVertex.position.X = currentvoxel.Posindices.X;
                        posxColorVertex.position.Y = currentvoxel.Posindices.Y;
                        posxColorVertex.position.Z = currentvoxel.Posindices.Z;
                        poscolresult.Add(posxColorVertex);
                        posxColorVertex.position.X = currentvoxel.Posindices.X + countX + 1;
                        poscolresult.Add(posxColorVertex);
                        posxColorVertex.position.Y = currentvoxel.Posindices.Y + countY + 1;
                        poscolresult.Add(posxColorVertex);
                        posxColorVertex.position.X = currentvoxel.Posindices.X;
                        poscolresult.Add(posxColorVertex);

                        currentX += countX + 1;
                        
                        checkin(currentvoxel.Posindices,new Vector3(currentvoxel.Posindices.X + countX +1, currentvoxel.Posindices.Y + countY + 1, currentvoxel.Posindices.Z + countZ + 1));
                        
                    }
                }
            }
            this.Vertices = poscolresult.ToArray();
        }

        private int GetNeighborsX(VoxelInformation start)
        {
            VoxelInformation next = new VoxelInformation(start.Posindices, start.Color);
            int neighborsX = 0;
            while ((next.Posindices.X < dimensions.X - 1) && IsSameColorRight(next))
            {
                neighborsX++;
                next.Posindices.X++;
            }

            return neighborsX;
        }
        private int GetNeighborsY(VoxelInformation start)
        {
            VoxelInformation next = new VoxelInformation(start.Posindices, start.Color);
            int neighborsY = 0;
            while ((next.Posindices.Y < dimensions.Y - 1) && IsSameColorUp(next))
            {
                next.Posindices.Y++;
                neighborsY++;
            }

            return neighborsY;

        }
        private int GetNeighborsZ(VoxelInformation start)
        {
            VoxelInformation next = new VoxelInformation(start.Posindices, start.Color);
            int neighborsZ = 0;
            while ((next.Posindices.Z < dimensions.Z-1)&& IsSameColorFront(next))
            {
                next.Posindices.Z++;
                neighborsZ++;
            }
            return neighborsZ;
        }

        private void checkin(Vector3 start, Vector3 end)
        {
            for (int i = (int) start.X; i < end.X; i++)
            {
                for (int j = (int) start.Y; j < end.Y; j++)
                {
                    for (int k = (int) start.Z; k < end.Z; k++)
                    {
                        VolumeData[i, j, k].checkedin = true;
                    }
                }
            }
        }

        public void ComputeIndices()
       {
           List<uint> indices = new List<uint>();
           for(uint i = 0; i < _voxelscount; i++)
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
        public bool checkedin;

        public VoxelInformation(Vector3 position, Vector4 color,bool checkedin = false)
        {
            Color = color;
            Posindices = position;
            this.checkedin = checkedin;
        }
    }
}
