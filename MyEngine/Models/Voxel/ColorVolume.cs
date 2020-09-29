using MyEngine.DataStructures;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace MyEngine.Models.Voxel
{
    public class ColorVolume : Volume
    {
        public List<Vector4> Colors = new List<Vector4>() { Vector4.Zero };
        public ColorVolume(int DimensionsX, int DimensionsY, int DimensionsZ) : base()
        {
            Dimensions = new Vector3(DimensionsX, DimensionsY, DimensionsZ);
            VolumeData = new int[DimensionsX, DimensionsY, DimensionsZ];
        }

        private void InitializeVolumeData()
        {
            VolumeData = new int[(int)Dimensions.X, (int)Dimensions.Y, (int)Dimensions.Z];
        }

        public override void SetVoxel(int x, int y, int z, int colorIndex)
        {
            VolumeData[x, y, z] = colorIndex;
            IsReady = false;
        }
        public override void SetVoxel(int x, int y, int z, Vector4 color)
        {
            if (!Colors.Contains(color))
            {
                Colors.Add(color);
            }
            var colorIndex = Colors.IndexOf(color);

            SetVoxel(x, y, z, colorIndex);
        }
        public override void SetVoxel(Vector3 position, Vector4 color)
        {
            SetVoxel((int)position.X, (int)position.Y, (int)position.Z, color);
        }

        public void ClearVolume()
        {
            InitializeVolumeData();
        }

        public override void ClearVoxel(int x, int y, int z)
        {
            if (!(x <= Dimensions.X && y <= Dimensions.Y && z <= Dimensions.Z)) return;
            VolumeData[x, y, z] = 0;
        }

        protected bool IsSameColorFront(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y, z + 1];
        }

        protected bool IsSameColorBack(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y, z - 1];
        }

        protected bool IsSameColorRight(int x, int y, int z)
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
        public Vector4 GetColor(int x, int y, int z)
        {
            return Colors[VolumeData[x, y, z]];
        }
        protected bool IsSameColorLeft(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x - 1, y, z];
        }

        protected bool IsSameColorUp(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y + 1, z];
        }

        protected bool IsSameColorDown(int x, int y, int z)
        {
            return VolumeData[x, y, z] == VolumeData[x, y - 1, z];
        }
        public virtual void ComputeIndices()
        {
            if (Vertices == null) return;

            var vxlcnt = Vertices.Length / 8;
            var indices = new List<uint>();
            for (uint i = 0; i < vxlcnt; i++)
                indices.AddRange(CubeData.Indices.Select(x => x + i * 8).ToList());

            Indices = indices.ToArray();

        }
        public virtual void ComputeVertices()
        {
            int countX, countY, countZ;
            var _checked = 0;
            var poscolresult = new List<PositionColorVertex>();
            CheckedInVoxels = new bool[(int)Dimensions.X, (int)Dimensions.Y, (int)Dimensions.Z];



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
                            for (var i = currentZ; i <= currentZ + countZ; i++)
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
        }

        protected int GetNeighborsX(int x, int y, int z)
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
        protected int GetNeighborsY(int x, int y, int z)
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

        protected int GetNeighborsZ(int x, int y, int z)
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

        protected void checkin(Vector3 start, Vector3 end)
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

        protected void checkout(Vector3 start, Vector3 end)
        {
            for (var i = (int)start.X; i < end.X; i++)
                for (var j = (int)start.Y; j < end.Y; j++)
                    for (var k = (int)start.Z; k < end.Z; k++)
                        CheckedInVoxels[i, j, k] = false;
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
        public override void ComputeVerticesAndIndices()
        {
            ComputeVertices();
            ComputeIndices();
        }
    }
}