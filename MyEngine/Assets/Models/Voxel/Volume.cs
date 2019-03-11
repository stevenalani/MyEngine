using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace MyEngine.Assets.Models.Voxel
{
    public class Volume
    {
        protected readonly Vector3 dimensions;

        // Stores the Voxel Material
        protected VoxelInformation[,,] VolumeData;
        private uint _voxelscount;
        private bool hasstartpoint;
        public uint[] Indices;

        public PositionColorVertex[] Vertices;
        private bool isInitialized;

        public Volume(Vector3 dimensions)
        {
            this.dimensions = dimensions;
            VolumeData = new VoxelInformation[(int) dimensions.X, (int) dimensions.Y, (int) dimensions.Z];
        }

        public Volume(int witdh, int height, int depth)
        {
            dimensions = new Vector3(witdh, height, depth);
            VolumeData = new VoxelInformation[witdh, height, depth];
        }

        public void SetVoxel(Vector3 pos, Vector4 mat)
        {
            if (!(pos.X <= dimensions.X && pos.Y <= dimensions.Y && pos.Z <= dimensions.Z)) return;
            VolumeData[(int) pos.X, (int) pos.Y, (int) pos.Z] = new VoxelInformation(pos, mat);
            _voxelscount++;
            isInitialized = false;
        }


        private bool IsSameColorFront(VoxelInformation voxel)
        {
            var front = voxel.Posindices + Vector3.UnitZ;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorBack(VoxelInformation voxel)
        {
            var front = voxel.Posindices - Vector3.UnitZ;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorRight(VoxelInformation voxel)
        {
            var front = voxel.Posindices + Vector3.UnitX;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorLeft(VoxelInformation voxel)
        {
            var front = voxel.Posindices - Vector3.UnitX;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorUp(VoxelInformation voxel)
        {
            var front = voxel.Posindices + Vector3.UnitY;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorDown(VoxelInformation voxel)
        {
            var front = voxel.Posindices - Vector3.UnitY;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        public void ComputeVertices()
        {
            VoxelInformation currentvoxel;

            int countX = -1, countY = -1, countZ = -1;
            var poscolresult = new List<PositionColorVertex>();
            for (var currentZ = 0; currentZ < dimensions.Z; currentZ++)
            for (var currentY = 0; currentY < dimensions.Y; currentY++)
            for (var currentX = 0; currentX < dimensions.X; currentX++)
            {
                currentvoxel = VolumeData[currentX, currentY, currentZ];
                if (currentvoxel.Color == Vector4.Zero || currentvoxel.checkedin)
                    continue;
                countX = GetNeighborsX(currentvoxel);
                countY = GetNeighborsY(currentvoxel);
                countZ = GetNeighborsZ(currentvoxel);
                if (countX >= countY && countX >= countZ)
                    for (var i = (int) currentvoxel.Posindices.X; i <= currentvoxel.Posindices.X + countX; i++)
                    {
                        var voxel = VolumeData[i, currentY, currentZ];
                        var voxelsAbove = GetNeighborsY(voxel);
                        var voxelsInfront = GetNeighborsZ(voxel);
                        if (voxelsAbove < countY || countY == -1)
                            countY = voxelsAbove;
                        if (voxelsInfront < countZ || countZ == -1)
                            countZ = voxelsInfront;
                    }
                else if (countY >= countX && countY >= countZ)
                    for (var i = (int) currentvoxel.Posindices.Y; i <= currentvoxel.Posindices.Y + countY; i++)
                    {
                        var voxel = VolumeData[currentX, i, currentZ];
                        var voxelsRight = GetNeighborsX(voxel);
                        var voxelsInfront = GetNeighborsZ(voxel);
                        if (voxelsRight < countX || countX == -1)
                            countX = voxelsRight;
                        if (voxelsInfront < countZ || countZ == -1)
                            countZ = voxelsInfront;
                    }
                else if (countZ >= countX && countZ >= countY)
                    for (var i = (int) currentvoxel.Posindices.Z; i <= currentvoxel.Posindices.Z + countZ; i++)
                    {
                        var voxel = VolumeData[currentX, currentY, i];
                        var voxelsAbove = GetNeighborsY(voxel);
                        var voxelsRight = GetNeighborsX(voxel);
                        if (voxelsAbove < countY || countY == -1)
                            countY = voxelsAbove;
                        if (voxelsRight < countX || countX == -1)
                            countX = voxelsRight;
                    }

                var posxColorVertex = new PositionColorVertex
                {
                    color = currentvoxel.Color / 255,
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

               // currentX += countX;

                checkin(currentvoxel.Posindices,
                    new Vector3(currentvoxel.Posindices.X + countX + 1, currentvoxel.Posindices.Y + countY + 1,
                        currentvoxel.Posindices.Z + countZ + 1));
            }

            Vertices = poscolresult.Select(x =>
                {
                    return new PositionColorVertex
                    {
                        color = x.color,
                        position = x.position - dimensions/2
                    };
                }
            ).ToArray();
        }

        private int GetNeighborsX(VoxelInformation start)
        {
            var next = new VoxelInformation(start.Posindices, start.Color);
            var neighborsX = 0;
            while (next.Posindices.X < dimensions.X - 1 && IsSameColorRight(next))
            {
                if (VolumeData[(int) next.Posindices.X, (int) next.Posindices.Y, (int) next.Posindices.Z].checkedin)
                    break;
                neighborsX++;
                next.Posindices.X++;
            }

            return neighborsX;
        }

        private int GetNeighborsY(VoxelInformation start)
        {
            var next = new VoxelInformation(start.Posindices, start.Color);
            var neighborsY = 0;
            while (next.Posindices.Y < dimensions.Y - 1 && IsSameColorUp(next) && !VolumeData[(int) next.Posindices.X,(int) next.Posindices.Y,(int) next.Posindices.Z].checkedin)
            {
                next.Posindices.Y++;
                neighborsY++;
            }

            return neighborsY;
        }

        private int GetNeighborsZ(VoxelInformation start)
        {
            var next = new VoxelInformation(start.Posindices, start.Color);
            var neighborsZ = 0;
            while (next.Posindices.Z < dimensions.Z - 1 && IsSameColorFront(next))
            {
                next.Posindices.Z++;
                neighborsZ++;
            }

            return neighborsZ;
        }

        private void checkin(Vector3 start, Vector3 end)
        {
            for (var i = (int) start.X; i < end.X; i++)
            for (var j = (int) start.Y; j < end.Y; j++)
            for (var k = (int) start.Z; k < end.Z; k++)
                VolumeData[i, j, k].checkedin = true;
        }

        public void ComputeIndices()
        {
            var indices = new List<uint>();
            for (uint i = 0; i < _voxelscount; i++) indices.AddRange(CubeData.Indices.Select(x => x + i * 8).ToList());

            Indices = indices.ToArray();
        }

        public void Init()
        {
            ComputeVertices();
            ComputeIndices();
            isInitialized = true;
        }

        public PositionColorModel GetModel()
        {
            if(!isInitialized)
                Init();
            return new PositionColorModel(Vertices,Indices);
        }
    }

    public struct VoxelInformation
    {
        public Vector4 Color;
        public Vector3 Posindices;
        public bool checkedin;

        public VoxelInformation(Vector3 position, Vector4 color, bool checkedin = false)
        {
            Color = color;
            Posindices = position;
            this.checkedin = checkedin;
        }
    }
}