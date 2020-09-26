using MyEngine.DataStructures;
using MyEngine.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEngine.HgtImporter
{
    public class ColorVolume : PositionColorModel
    {
        public Vector3 Dimensions;
        protected int[,,] VolumeData;
        private bool[,,] CheckedInVoxels;
        private int Chunksize;
        public Vector3 ChunkCount;
        public List<Vector4> Colors = new List<Vector4>() { Vector4.Zero };
        public ColorVolume(int DimensionsX, int DimensionsY, int DimensionsZ, int chunksize = 16) : base(null, null)
        {
            Dimensions = new Vector3(DimensionsX, DimensionsY, DimensionsZ);
            VolumeData = new int[DimensionsX, DimensionsY + 1, DimensionsZ];
            this.Chunksize = chunksize;
            var csx = Dimensions.X / chunksize;
            var csy = Dimensions.Y / chunksize;
            var csz = Dimensions.Z / chunksize;
            this.ChunkCount = new Vector3((float)Math.Ceiling(csx), (float)Math.Ceiling(csy), (float)Math.Ceiling(csz));
        }

        private void InitializeVolumeData()
        {
            VolumeData = new int[(int)Dimensions.X + 1, (int)Dimensions.Y + 1, (int)Dimensions.Z + 1];
        }

        public void SetVoxel(int x, int y, int z, int colorIndex)
        {
            VolumeData[x, y, z] = colorIndex;
        }
        public void SetVoxel(int x, int y, int z, Vector4 color)
        {
            if (!Colors.Contains(color))
            {
                Colors.Add(color);
            }
            var colorIndex = Colors.IndexOf(color);

            SetVoxel(x, y, z, colorIndex);
        }

        public void ClearVolume()
        {
            InitializeVolumeData();
        }

        protected void ClearVoxel(int x, int y, int z)
        {
            if (!(x <= Dimensions.X && y <= Dimensions.Y && z <= Dimensions.Z)) return;
            VolumeData[x, y, z] = 0;
        }

        private bool IsSameColorFront(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y, z + 1];
        }

        private bool IsSameColorBack(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y, z - 1];
        }

        private bool IsSameColorRight(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x + 1, y, z];
        }

        public void AddColor(Vector4 color)
        {
            if (!Colors.Contains(color))
            {
                Colors.Add(color);
            }
        }

        public int GetColorIndex(Vector4 color)
        {
            return Colors.IndexOf(color);
        }

        private bool IsSameColorLeft(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x - 1, y, z];
        }

        private bool IsSameColorUp(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y + 1, z];
        }

        private bool IsSameColorDown(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y - 1, z];
        }

        public void ComputeVertices()
        {
            int countX, countY, countZ;
            var _checked = 0;
            var poscolresult = new List<PositionColorVertex>();
            CheckedInVoxels = new bool[(int)Dimensions.X, (int)Dimensions.Y, (int)Dimensions.Z];
            
            /*for (var currentCunkZ = 0; currentCunkZ < ChunkCount.Z; currentCunkZ++)
            {
                for (var currentCunkY = 0; currentCunkY < ChunkCount.Y; currentCunkY++)
                {
                    for (var currentCunkX = 0; currentCunkX < ChunkCount.X; currentCunkX++)
                    {*/
                        for (var currentZ = 0; currentZ < Dimensions.Z; currentZ++)
                        {
                            for (var currentY = 0; currentY < Dimensions.Y; currentY++)
                            {
                                for (var currentX = 0; currentX < Dimensions.X; currentX++)
                                {
                                    if (VolumeData[currentX, currentY, currentZ] == 0 || CheckedInVoxels[currentX, currentY, currentZ])
                                        continue;

                                    _checked++;
                                    countX = GetNeighborsX(currentX, currentY, currentZ);
                                    countY = GetNeighborsY(currentX, currentY, currentZ);
                                    countZ = GetNeighborsZ(currentX, currentY, currentZ);
                                    if (countX >= countY && countX >= countZ)
                                        for (var i = currentX; i <= currentX + countX; i++)
                                        {
                                            var voxelsAbove = GetNeighborsY(i, currentY, currentZ);
                                            var voxelsInfront = GetNeighborsZ(i, currentY, currentZ);
                                            if (voxelsAbove < countY || countY == -1)
                                                countY = voxelsAbove;
                                            if (voxelsInfront < countZ || countZ == -1)
                                                countZ = voxelsInfront;
                                        }
                                    else if (countY >= countX && countY >= countZ)
                                        for (var i = currentY; i <= currentY + countY; i++)
                                        {
                                            var voxelsRight = GetNeighborsX(currentX, i, currentZ);
                                            var voxelsInfront = GetNeighborsZ(currentX, i, currentZ);
                                            if (voxelsRight < countX || countX == -1)
                                                countX = voxelsRight;
                                            if (voxelsInfront < countZ || countZ == -1)
                                                countZ = voxelsInfront;
                                        }
                                    else if (countZ >= countX && countZ >= countY)
                                        for (var i = (int)currentZ; i <= currentZ + countZ; i++)
                                        {
                                            var voxelsAbove = GetNeighborsY(currentX, currentY, i);
                                            var voxelsRight = GetNeighborsX(currentX, currentY, i);
                                            if (voxelsAbove < countY || countY == -1)
                                                countY = voxelsAbove;
                                            if (voxelsRight < countX || countX == -1)
                                                countX = voxelsRight;
                                        }

                                    var posxColorVertex = new PositionColorVertex
                                    {
                                        Color = Colors[VolumeData[currentX, currentY, currentZ]] / 255,
                                        Position = new Vector3(currentX, currentY, currentZ)
                                    };
                                    posxColorVertex.Position.Z = currentZ + countZ + 1;
                                    poscolresult.Add(posxColorVertex);
                                    posxColorVertex.Position.X = currentX + countX + 1;
                                    poscolresult.Add(posxColorVertex);
                                    posxColorVertex.Position.Y = currentY + countY + 1;
                                    poscolresult.Add(posxColorVertex);
                                    posxColorVertex.Position.X = currentX;
                                    poscolresult.Add(posxColorVertex);

                                    //Backface Vertex
                                    posxColorVertex.Position.X = currentX;
                                    posxColorVertex.Position.Y = currentY;
                                    posxColorVertex.Position.Z = currentZ;
                                    poscolresult.Add(posxColorVertex);
                                    posxColorVertex.Position.X = currentX + countX + 1;
                                    poscolresult.Add(posxColorVertex);
                                    posxColorVertex.Position.Y = currentY + countY + 1;
                                    poscolresult.Add(posxColorVertex);
                                    posxColorVertex.Position.X = currentX;
                                    poscolresult.Add(posxColorVertex);
                                    var end = new Vector3(currentX + countX + 1, currentY + countY + 1,
                                        currentZ + countZ + 1);
                                    checkin(new Vector3(currentX, currentY, currentZ), end);
                                }
                            }
                        }
                    /*}
                }
            }*/

            if (poscolresult.Count != 0)
            {
                if (Vertices == null)
                    Vertices = poscolresult.Select(x =>
                    {
                        return new PositionColorVertex
                        {
                            Color = x.Color,
                            Position = x.Position - Dimensions / 2
                        };
                    }
                    ).ToArray();
                else
                {
                    var list1 = new List<PositionColorVertex>(Vertices);
                    foreach (var positionColorVertex in poscolresult)
                    {
                        for (var i = 0; i < Vertices.Length; i++)
                        {
                            if (Vertices[i].Position == positionColorVertex.Position - Dimensions / 2)
                            {
                                Vertices[i].Color = positionColorVertex.Color;
                            }
                        }
                    }
                    list1.AddRange(poscolresult.Select(x =>
                    {
                        return new PositionColorVertex
                        {
                            Color = x.Color,
                            Position = x.Position - Dimensions / 2
                        };
                    }
                    ));
                    Vertices = list1.ToArray();
                }
            }

            else
            {

            }
        }
        private int GetNeighborsX(int x, int y, int z)
        {
            var neighborsX = 0;
            while (x < Dimensions.X - 1 && IsSameColorRight(x, y, z) &&
                   !CheckedInVoxels[x + 1, y, z])
            {
                neighborsX++;
                x++;
            }

            return neighborsX;
        }
        private int GetNeighborsY(int x, int y, int z)
        {
            var neighborsY = 0;
            while (y < Dimensions.Y - 1 && IsSameColorUp(x, y, z) &&
                   !CheckedInVoxels[x, y + 1, z])
            {
                y++;
                neighborsY++;
            }

            return neighborsY;
        }

        private int GetNeighborsZ(int x, int y, int z)
        {
            var neighborsZ = 0;
            while (z < Dimensions.Z - 1 && IsSameColorFront(x, y, z) &&
                   !CheckedInVoxels[x, y, z + 1])
            {
                z++;
                neighborsZ++;
            }

            return neighborsZ;
        }

        public override void InitBuffers()
        {
            ComputeVerticesAndIndices();
            base.InitBuffers();
        }

        private void checkin(Vector3 start, Vector3 end)
        {
            for (var i = (int)start.X; i < end.X; i++)
                for (var j = (int)start.Y; j < end.Y; j++)
                    for (var k = (int)start.Z; k < end.Z; k++)
                    {
                        var voxel = VolumeData[i, j, k];
                        if (voxel != 0)
                            CheckedInVoxels[i, j, k] = true;
                    }
        }

        private void checkout(Vector3 start, Vector3 end)
        {
            for (var i = (int)start.X; i < end.X; i++)
                for (var j = (int)start.Y; j < end.Y; j++)
                    for (var k = (int)start.Z; k < end.Z; k++)
                        CheckedInVoxels[i, j, k] = false;
        }

        public void ComputeIndices()
        {
            //var vxlcnt = GetVoxelCount();
            var vxlcnt = Vertices.Length / 8;
            var indices = new List<uint>();
            for (uint i = 0; i < vxlcnt; i++)
                indices.AddRange(CubeData.Indices.Select(x => x + i * 8).ToList());

            Indices = indices.ToArray();
        }

        public int GetVoxelCount()
        {
            var _voxelscount = 0;
            for (var i = 0; i < Dimensions.X; i++)
                for (var j = 0; j < Dimensions.Y; j++)
                    for (var k = 0; k < Dimensions.Z; k++)
                        if (VolumeData[i, j, k] != 0)
                            _voxelscount++;
            return _voxelscount;
        }

        public int GetCheckedInCount()
        {
            var _voxelscount = 0;
            for (var i = 0; i < Dimensions.X; i++)
                for (var j = 0; j < Dimensions.Y; j++)
                    for (var k = 0; k < Dimensions.Z; k++)
                        if (CheckedInVoxels[i, j, k])
                            _voxelscount++;
            return _voxelscount;
        }

        public void ComputeVerticesAndIndices()
        {
            ComputeVertices();
            ComputeIndices();
        }


        public bool IsVoxel(int x, int y, int z)
        {
            if (x >= 0 && x < Dimensions.X && y >= 0 && y < Dimensions.Y && z >= 0 && z < Dimensions.Z)
                return VolumeData[x, y, z] != 0;

            return false;
        }

        public bool IsValidVoxelPosition(int x, int y, int z)
        {
            if (x >= 0 && x < Dimensions.X && y >= 0 && y < Dimensions.Y && z >= 0 && z < Dimensions.Z) return true;

            return false;
        }

    }
}