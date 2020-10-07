using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEngine;
using MyEngine.DataStructures;
using MyEngine.Models.Voxel;
using OpenTK;

namespace DevOptimizedMeshes
{
    class Program
    {
        static void Main(string[] args)
        {
            var width = 800;
            var height = 600;
            var userprofilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var cam = new Camera(width, height, 0.1f, 100f);
            cam.Position = new Vector3(0, 0, 20);
            var engine = new Engine(width, height, cam);
            engine.enableCrossHair(new Vector4(1f, 1f, 1f, 0.5f));
            engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");
            var optimizer = new MyEngine.Models.Voxel.MeshOptimizer();
            var volume = new ColorVolume(3, 3, 3);
            volume.SetVoxel(0,0,0,new Vector4(255,0,0,120));
            volume.SetVoxel(1,0,0,new Vector4(255,0,0,120));
            volume.SetVoxel(2,0,0,new Vector4(0, 255,0,120));
            /*volume.SetVoxel(0, 0, 0, new Vector4(0, 0, 255, 120));
            volume.SetVoxel(0, 1, 0, new Vector4(0, 0, 255 / 3 * 2, 120));
            volume.SetVoxel(0, 2, 0, new Vector4(0, 0, 255 / 3, 120));

            volume.SetVoxel(1, 0, 0, new Vector4(0, 0, 255 / 3 * 2, 120));
            volume.SetVoxel(1, 1, 0, new Vector4(0, 0, 255 / 3, 120));

            volume.SetVoxel(2, 0, 0, new Vector4(0, 0, 255 / 3, 120));


            volume.SetVoxel(0, 0, 1, new Vector4(0, 255, 0,120));
            volume.SetVoxel(0, 1, 1, new Vector4(0, 255 / 3 * 2, 0,  120));
            volume.SetVoxel(0, 2, 1, new Vector4(0, 255 / 3,0, 120));
            
            volume.SetVoxel(1, 0, 1, new Vector4(0, 255 / 3 * 2, 0, 120));
            volume.SetVoxel(1, 1, 1, new Vector4(0, 255 / 3, 0, 120));
            volume.SetVoxel(1, 2, 1, new Vector4(0, 255, 0, 120));

            volume.SetVoxel(2, 0, 1, new Vector4(0,  255 / 3, 0, 120));
            volume.SetVoxel(2, 1, 1, new Vector4(0, 255, 0, 120));


            volume.SetVoxel(0, 0, 2, new Vector4( 255, 0, 0, 120));
            volume.SetVoxel(0, 1, 2, new Vector4( 255 / 3 * 2, 0, 0, 120));
            volume.SetVoxel(0, 2, 2, new Vector4( 255 / 3, 0, 0, 120));

            volume.SetVoxel(1, 0, 2, new Vector4( 255 / 3 * 2, 0, 0, 120));
            volume.SetVoxel(1, 1, 2, new Vector4( 255 / 3, 0, 0, 120));
            volume.SetVoxel(1, 2, 2, new Vector4( 255, 0, 0, 120));

            volume.SetVoxel(2, 0, 2, new Vector4(255 / 3, 0, 0, 120));
            volume.SetVoxel(2, 1, 2, new Vector4(255, 0, 0, 120));
            volume.SetVoxel(2, 2, 2, new Vector4(255 / 3 * 2, 0, 0, 120));*/





            /*for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        if((x % 5  == 0 && x != 0) || (z % 5 == 0 && z != 0 ) || (y % 5 == 0 && y != 0))
                        {
                            volume.SetVoxel(x, y, z, new Vector4(127, 255, 255, 120));
                        }
                        else
                        {
                            volume.SetVoxel(x, y, z, new Vector4(255, 255, 255, 120));
                        }
                        
                    }
                }
            }*/
            optimizer.optimize(volume);
            engine.AddModel(volume);
            
            engine.Run(60.0);
        }
    }
}

namespace MyEngine.Models.Voxel
{
    internal class MeshOptimizer
    {
        /*public void optimize(ColorVolume volume)
        {
            volume.ComputeVerticesAndIndices();
            // iterate dimensions
            List<PositionColorVertex> newVertices = new List<PositionColorVertex>();
            for (int z = 0; z < volume.Dimensions.Z; z++)
            {
                for (int y = 0; y < volume.Dimensions.Z; y++)
                {
                    for (int x = 0; x < volume.Dimensions.Z; x++)
                    {
                        var currentVoxelColor = volume.VolumeData[x, y, z];
                        if(currentVoxelColor == 0)
                            continue;

                        var currentVoxelPosition = new Vector3(x, y, z);
                        newVertices.Add(new PositionColorVertex()
                        {
                            Position = currentVoxelPosition,
                            Color = volume.Colors[currentVoxelColor],
                        });
                        var nextColorX = getColorChangePosX(currentVoxelPosition, volume);
                        if (nextColorX.X == currentVoxelPosition.X)
                        {

                            newVertices.Add(new PositionColorVertex()
                            {
                                Position = nextColorX + Vector3.UnitX,
                                Color = volume.Colors[currentVoxelColor],
                            });
                            x = (int)nextColorX.X + 1;
                        }
                        else
                        {
                            newVertices.Add(new PositionColorVertex()
                            {
                                Position = nextColorX - Vector3.UnitX,
                                Color = volume.Colors[currentVoxelColor],
                            });
                            x = (int)nextColorX.X - 1;
                        }
                    }
                }
            }
            volume.ComputeIndices();
            /*for (int z = 0; z < volume.Dimensions.Z; z++)
            {
                var disc = volume.Vertices.Where(v => v.Position.Z == z)
                    .OrderBy(v => v.Position.Y)
                    .ThenBy(v => v.Position.X).ToArray();
                var currentVertex = disc.First();
                var neighbourL = disc.FirstOrDefault(x =>
                    x.Position.X == currentVertex.Position.X - 1 && x.Position.Y == currentVertex.Position.Y);
                var neighbourR = disc.FirstOrDefault(x =>
                    x.Position.X == currentVertex.Position.X + 1 && x.Position.Y == currentVertex.Position.Y);
            }
        }*/

        private Vector3 getColorChangePosX(Vector3 currentVoxelPosition, Volume volume)
        {
            int x = (int) currentVoxelPosition.X;
            int y = (int) currentVoxelPosition.Y;
            int z = (int)currentVoxelPosition.Z;
            for (int cx = x+1; cx < volume.Dimensions.X; cx++)
            {
                if (volume.VolumeData[cx, y, z] != volume.VolumeData[x, y, z])
                {
                    currentVoxelPosition.X = cx;
                    break;
                }
            }
            return currentVoxelPosition;
        }
        private Vector3 getLastOfColorInYDir(Vector3 currentVoxelPosition, Volume volume)
        {
            int x = (int)currentVoxelPosition.X;
            int y = (int)currentVoxelPosition.Y;
            int z = (int)currentVoxelPosition.Z;
            for (int cy = y; cy < volume.Dimensions.Y; cy++)
            {
                if (volume.VolumeData[x, cy, z] != volume.VolumeData[x, y, z])
                {
                    currentVoxelPosition.Y = cy;
                    break;
                }
            }
            return currentVoxelPosition;
        }
        private Vector3 getLastOfColorInZDir(Vector3 currentVoxelPosition, Volume volume)
        {
            int x = (int)currentVoxelPosition.X;
            int y = (int)currentVoxelPosition.Y;
            int z = (int)currentVoxelPosition.Z;
            for (int cz = z; cz < volume.Dimensions.Y; cz++)
            {
                if (volume.VolumeData[x, y, cz] != volume.VolumeData[x, y, z])
                {
                    currentVoxelPosition.Z = cz;
                    break;
                }
            }
            return currentVoxelPosition;
        }

        public void optimize(ColorVolume volume)
        {
            
            volume.ComputeVerticesAndIndices();
            var Vertices = new List<PositionColorNormalVertex>(volume.Vertices);
            foreach (var colorVertex in volume.Vertices)
            {
                var doubles = Vertices.Where(v => v.Position == colorVertex.Position && v.Color != colorVertex.Color)
                    .ToArray();
                if (doubles.Length > 0)
                    Vertices.Remove(colorVertex);
            }

            volume.Vertices = Vertices.ToArray();
            volume.ComputeIndices();

        }
    }
}
