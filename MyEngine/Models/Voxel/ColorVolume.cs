using MyEngine.DataStructures;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace MyEngine.Models.Voxel
{
    public class ColorVolume : Volume
    {
        
        public ColorVolume(int DimensionsX, int DimensionsY, int DimensionsZ, float cubeScale = 1f) : base()
        {
            Dimensions = new Vector3(DimensionsX, DimensionsY, DimensionsZ);
            VolumeData = new short[DimensionsX, DimensionsY, DimensionsZ];
            CubeScale = cubeScale;
        }

        private void InitializeVolumeData()
        {
            VolumeData = new short[(int)Dimensions.X, (int)Dimensions.Y, (int)Dimensions.Z];
            IsReady = false;
        }

        public override void SetVoxel(int x, int y, int z, int materialIndex)
        {
            VolumeData[x, y, z] = (short)materialIndex;
            IsReady = false;
        }
        public override void SetVoxel(int x, int y, int z, Material material)
        {
            if (!Materials.Contains(material))
            {
                Materials.Add(material);
            }
            var colorIndex = Materials.IndexOf(material);

            SetVoxel(x, y, z, colorIndex);
        }
        public override void SetVoxel(Vector3 position, Material material)
        {
            SetVoxel((int)position.X, (int)position.Y, (int)position.Z, material);
        }
        public override void SetVoxel(Vector3 position, int materialIndex)
        {
            SetVoxel((int)position.X, (int)position.Y, (int)position.Z, materialIndex);
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



 

        public virtual void ComputeVertices()
        {
            var poscolresult = new List<PositionColorNormalVertex>();
            List<uint> indices = new List<uint>();
            CheckedInVoxels = new bool[(int)Dimensions.X, (int)Dimensions.Y, (int)Dimensions.Z];
            uint cubeCount = 0;
            for (var currentZ = 0; currentZ < Dimensions.Z; currentZ++)
            {
                for (var currentY = 0; currentY < Dimensions.Y; currentY++)
                {
                    for (var currentX = 0; currentX < Dimensions.X; currentX++)
                    {
                        var colorIndex = VolumeData[currentX, currentY, currentZ];
                        if(colorIndex == 0) continue;
                        
                        Vector4 color = Materials[colorIndex].Color / 255;

                        var offset = new Vector3(currentX,currentY,currentZ);
                        if (IsFrontfaceVisible(currentX, currentY, currentZ))
                        {
                            poscolresult.AddRange(CubeWithNormals.FrontFace(offset,color,CubeScale));
                        }
                        if (IsBackfaceVisible(currentX, currentY, currentZ))
                        {
                            poscolresult.AddRange(CubeWithNormals.BackFace(offset, color, CubeScale));
                        }
                        if (IsLeftfaceVisible(currentX, currentY, currentZ))
                        {
                            poscolresult.AddRange(CubeWithNormals.LeftFace(offset, color, CubeScale));
                        }
                        if (IsRightfaceVisible(currentX, currentY, currentZ))
                        {
                            poscolresult.AddRange(CubeWithNormals.RightFace(offset, color, CubeScale));
                        }
                        if (IsBottomfaceVisible(currentX, currentY, currentZ))
                        {
                            poscolresult.AddRange(CubeWithNormals.BottomFace(offset, color, CubeScale));
                        }
                        if (IsTopfaceVisible(currentX, currentY, currentZ))
                        {
                            poscolresult.AddRange(CubeWithNormals.TopFace(offset, color, CubeScale));
                        }
                    }
                }
            }

            
            if (poscolresult.Count != 0)
            {
                for (uint i = 0; i < poscolresult.Count; i++)
                {
                    indices.Add(i);
                }
                Vertices = poscolresult.ToArray();
                Indices = indices.ToArray();
            }
        }

        protected int GetSameNeighborsX(int x, int y, int z)
        {
            var neighborsX = 0;
            while (x < Dimensions.X - 1 && IsSameMaterialRight(x, y, z) &&
                   !CheckedInVoxels[x + 1, y, z])
            {
                x++;
                neighborsX++;
            }

            return neighborsX;
        }
        protected int GetSameNeighborsY(int x, int y, int z)
        {
            var neighborsY = 0;
            while (y < Dimensions.Y - 1 && IsSameMaterialUp(x, y, z) &&
                   !CheckedInVoxels[x, y + 1, z])
            {
                y++;
                neighborsY++;
            }

            return neighborsY;
        }
        protected int GetSameNeighborsZ(int x, int y, int z)
        {
            var neighborsZ = 0;
            while (z < Dimensions.Z - 1 && IsSameMaterialUp(x, y, z) &&
                   !CheckedInVoxels[x, y , z + 1])
            {
                z++;
                neighborsZ++;
            }

            return neighborsZ;
        }
        // Check Z Axis if voxel is in front X-1
        protected bool IsFrontfaceVisible(int x, int y, int z)
        {
            if (z == 0 || VolumeData[x, y, z - 1] == 0 || Materials[VolumeData[x,y,z - 1]].Color.W != 255.0f)
            {
                return true;
            }
            return false;
        }
        protected bool IsBackfaceVisible(int x, int y, int z)
        {
            if (z == (int)Dimensions.Z - 1 || VolumeData[x, y, z + 1] == 0 || Materials[VolumeData[x, y, z + 1]].Color.W != 255.0f)
            {
                return true;
            }
            return false;
        }

        protected bool IsLeftfaceVisible(int x, int y, int z)
        {
            if (x == 0 || VolumeData[x - 1, y, z] == 0 || Materials[VolumeData[x - 1, y, z]].Color.W != 255.0f)
            {
                return true;
            }
            return false;
        }
        protected bool IsRightfaceVisible(int x, int y, int z)
        {
            if (x == (int)Dimensions.X - 1 || VolumeData[x + 1, y, z] == 0 || Materials[VolumeData[x + 1, y, z]].Color.W != 255.0f)
            {
                return true;
            }
            return false;
        }

        protected bool IsBottomfaceVisible(int x, int y, int z)
        {
            if (y == 0 || VolumeData[x, y - 1, z] == 0 || Materials[VolumeData[x , y - 1, z]].Color.W != 255.0f)
            {
                return true;
            }
            return false;
        }
        protected bool IsTopfaceVisible(int x, int y, int z)
        {
            if (y == (int)Dimensions.Y - 1 || VolumeData[x , y + 1, z] == 0 || Materials[VolumeData[x, y + 1, z]].Color.W != 255.0f)
            {
                return true;
            }
            return false;
        }



        public override void InitBuffers()
        {
            ComputeVerticesAndIndices();
            base.InitBuffers();
        }

        protected void CheckIn(Vector3 start, Vector3 end)
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

        protected void CheckOut(Vector3 start, Vector3 end)
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
            //ComputeIndices();
        }
    }
}