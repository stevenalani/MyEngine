using System.Collections.Generic;
using System.Linq;
using MyEngine.DataStructures;
using OpenTK;

namespace MyEngine.Models.Voxel
{
    public class Volume : PositionColorModel
    {
        public Vector3 size;
        protected Voxel[,,] VolumeData;


        public Volume(Vector3 size) : base(null, null)
        {
            this.size = size;
            VolumeData = new Voxel[(int) size.X, (int) size.Y, (int) size.Z];
            InitializeVolumeData();
        }

        public Volume(int witdh, int height, int depth) : base(CubeData.Vertices.Select(x=> new PositionColorVertex(){Position = x, Color = new Vector4(0)}).ToArray(), CubeData.Indices)
        {
            size = new Vector3(witdh, height, depth);
            InitializeVolumeData();
        }

        protected Volume() : base(null,null)
        {
            
        }


        protected void InitializeVolumeData()
        {
            VolumeData = new Voxel[(int) size.X, (int) size.Y, (int) size.Z];
            for (var z = 0; z < size.Z; z++)
            for (var y = 0; y < size.Y; y++)
            for (var x = 0; x < size.X; x++)
                VolumeData[x, y, z] = new Voxel(new Vector3(x, y, z), Vector4.Zero);
            IsReady = false;
        }

        public void SetVoxel(Vector3 position, Vector4 mat)
        {
            var pos = position;
            if (pos.X >= size.X || pos.Y >= size.Y || pos.Z >= size.Z || pos.X <= -1 || pos.Y <= -1 ||
                pos.Z <= -1)
                return;
            //if (VolumeData[(int) pos.X, (int) pos.Y, (int) pos.Z].Color == Vector4.Zero)_voxelcount++
            VolumeData[(int) pos.X, (int) pos.Y, (int) pos.Z] = new Voxel(pos, mat);
            IsReady = false;
        }

        public void SetVoxel(int posx, int posy, int posz, Vector4 color)
        {
            SetVoxel(new Vector3(posx, posy, posz), color);
        }

        public void ClearVolume()
        {
            InitializeVolumeData();
        }

        protected void ClearVoxel(int x, int y, int z)
        {
            if (!(x <= size.X && y <= size.Y && z <= size.Z)) return;
            VolumeData[x, y, z].Color = Vector4.Zero;
        }

        private bool IsSameColorFront(Voxel voxel)
        {
            var front = voxel.Posindices + Vector3.UnitZ;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorBack(Voxel voxel)
        {
            var front = voxel.Posindices - Vector3.UnitZ;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorRight(Voxel voxel)
        {
            var front = voxel.Posindices + Vector3.UnitX;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorLeft(Voxel voxel)
        {
            var front = voxel.Posindices - Vector3.UnitX;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorUp(Voxel voxel)
        {
            var front = voxel.Posindices + Vector3.UnitY;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        private bool IsSameColorDown(Voxel voxel)
        {
            var front = voxel.Posindices - Vector3.UnitY;
            var result = VolumeData[(int) front.X, (int) front.Y, (int) front.Z];
            return result.Color == voxel.Color;
        }

        public void ComputeVertices()
        {
            Voxel currentvoxel;
            int countX, countY, countZ;
            var _checked = 0;
            var poscolresult = new List<PositionColorVertex>();
            for (var currentZ = 0; currentZ < size.Z; currentZ++)
            for (var currentY = 0; currentY < size.Y; currentY++)
            for (var currentX = 0; currentX < size.X; currentX++)
            {
                currentvoxel = VolumeData[currentX, currentY, currentZ];
                if (currentvoxel.Color == Vector4.Zero || currentvoxel.checkedin)
                    continue;

                _checked++;
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
                    Color = currentvoxel.Color / 255,
                    Position = currentvoxel.Posindices
                };
                posxColorVertex.Position.Z = currentvoxel.Posindices.Z + countZ + 1;
                poscolresult.Add(posxColorVertex);
                posxColorVertex.Position.X = currentvoxel.Posindices.X + countX + 1;
                poscolresult.Add(posxColorVertex);
                posxColorVertex.Position.Y = currentvoxel.Posindices.Y + countY + 1;
                poscolresult.Add(posxColorVertex);
                posxColorVertex.Position.X = currentvoxel.Posindices.X;
                poscolresult.Add(posxColorVertex);

                //Backface Vertex
                posxColorVertex.Position.X = currentvoxel.Posindices.X;
                posxColorVertex.Position.Y = currentvoxel.Posindices.Y;
                posxColorVertex.Position.Z = currentvoxel.Posindices.Z;
                poscolresult.Add(posxColorVertex);
                posxColorVertex.Position.X = currentvoxel.Posindices.X + countX + 1;
                poscolresult.Add(posxColorVertex);
                posxColorVertex.Position.Y = currentvoxel.Posindices.Y + countY + 1;
                poscolresult.Add(posxColorVertex);
                posxColorVertex.Position.X = currentvoxel.Posindices.X;
                poscolresult.Add(posxColorVertex);
                var end = new Vector3(currentvoxel.Posindices.X + countX + 1, currentvoxel.Posindices.Y + countY + 1,
                    currentvoxel.Posindices.Z + countZ + 1);
                checkin(currentvoxel.Posindices, end);
            }

            if (poscolresult.Count != 0)
            {
                if (Vertices == null)
                    Vertices = poscolresult.Select(x =>
                        {
                            return new PositionColorVertex
                            {
                                Color = x.Color,
                                Position = x.Position - size / 2
                            };
                        }
                    ).ToArray();
                else
                {
                    var list1 = new List<PositionColorVertex>(Vertices);
                    foreach (var positionColorVertex in poscolresult)
                    {
                        for (var i = 0;i < Vertices.Length; i++)
                        {
                            if (Vertices[i].Position == positionColorVertex.Position - size / 2)
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
                                Position = x.Position - size / 2
                            };
                        }
                    ));
                    Vertices = list1.ToArray();
                }
            }
        }

        private int GetNeighborsX(Voxel start)
        {
            var next = new Voxel(start.Posindices, start.Color);
            var neighborsX = 0;
            while (next.Posindices.X < size.X - 1 && IsSameColorRight(next) &&
                   !VolumeData[(int) next.Posindices.X, (int) next.Posindices.Y, (int) next.Posindices.Z].checkedin)
            {
                neighborsX++;
                next.Posindices.X++;
            }

            return neighborsX;
        }

        private int GetNeighborsY(Voxel start)
        {
            var next = new Voxel(start.Posindices, start.Color);
            var neighborsY = 0;
            while (next.Posindices.Y < size.Y - 1 && IsSameColorUp(next) &&
                   !VolumeData[(int) next.Posindices.X, (int) next.Posindices.Y, (int) next.Posindices.Z].checkedin)
            {
                next.Posindices.Y++;
                neighborsY++;
            }

            return neighborsY;
        }

        private int GetNeighborsZ(Voxel start)
        {
            var next = new Voxel(start.Posindices, start.Color);
            var neighborsZ = 0;
            while (next.Posindices.Z < size.Z - 1 && IsSameColorFront(next) &&
                   !VolumeData[(int) next.Posindices.X, (int) next.Posindices.Y, (int) next.Posindices.Z].checkedin)
            {
                next.Posindices.Z++;
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
            for (var i = (int) start.X; i < end.X; i++)
            for (var j = (int) start.Y; j < end.Y; j++)
            for (var k = (int) start.Z; k < end.Z; k++)
            {
                var voxel = VolumeData[i, j, k];
                if (voxel.Color != Vector4.Zero)
                    VolumeData[i, j, k].checkedin = true;
            }
        }

        private void checkout(Vector3 start, Vector3 end)
        {
            for (var i = (int) start.X; i < end.X; i++)
            for (var j = (int) start.Y; j < end.Y; j++)
            for (var k = (int) start.Z; k < end.Z; k++)
                VolumeData[i, j, k].checkedin = false;
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
            for (var i = 0; i < size.X; i++)
            for (var j = 0; j < size.Y; j++)
            for (var k = 0; k < size.Z; k++)
                if (VolumeData[i, j, k].Color != Vector4.Zero)
                    _voxelscount++;
            return _voxelscount;
        }

        public int GetCheckedInCount()
        {
            var _voxelscount = 0;
            for (var i = 0; i < size.X; i++)
            for (var j = 0; j < size.Y; j++)
            for (var k = 0; k < size.Z; k++)
                if (VolumeData[i, j, k].checkedin)
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
            if (x >= 0 && x < size.X && y >= 0 && y < size.Y && z >= 0 && z < size.Z)
                return VolumeData[x, y, z].Color != Vector4.Zero;

            return false;
        }

        public bool IsValidVoxelPosition(int x, int y, int z)
        {
            if (x >= 0 && x < size.X && y >= 0 && y < size.Y && z >= 0 && z < size.Z) return true;

            return false;
        }
    }

    public struct Voxel
    {
        public Vector4 Color;
        public Vector3 Posindices;
        public bool checkedin;
        public bool IsVoxel => Color != Vector4.Zero;

        public Voxel(Vector3 position, Vector4 color)
        {
            Color = color;
            Posindices = position;
            checkedin = false;
        }
    }
}