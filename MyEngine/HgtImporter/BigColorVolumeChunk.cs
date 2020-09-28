using MyEngine.DataStructures;
using MyEngine.Models;
using MyEngine.Models.Voxel;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace MyEngine.HgtImporter
{
    public class BigColorVolumeChunk : ColorVolume
    {     
        private uint ChunkIdX;
        private uint ChunkIdY;
        private uint ChunkIdZ;

        public BigColorVolumeChunk(int dimensions, uint idX, uint idY, uint idZ) : base(dimensions,dimensions,dimensions)
        {
            
            ChunkIdX = idX;
            ChunkIdY = idY;
            ChunkIdZ = idZ;
        }

        public override void ComputeVertices()
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


            if (poscolresult.Count != 0)
            {
                if (Vertices == null)
                    Vertices = poscolresult.Select(x =>
                    {
                        return new PositionColorVertex
                        {
                            Color = x.Color,
                            Position = x.Position - Dimensions / 2 + Position
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
                            Position = x.Position - Dimensions / 2 + Position
                        };
                    }
                    ));
                    Vertices = list1.ToArray();
                }
            }
        }

        
        public override void InitBuffers()
        {
            ComputeVerticesAndIndices();
            base.InitBuffers();
        }

        
        public override void ComputeVerticesAndIndices()
        {
            ComputeVertices();
            ComputeIndices();
        }
    }
}