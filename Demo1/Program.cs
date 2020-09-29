using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEngine;
using MyEngine.HgtImporter;
using MyEngine.Models.Voxel;
using OpenTK;

namespace Demo1
{
        class Program
        {
            private static void Main(string[] args)
            {
                var width = 800;
                var height = 600;
                var userprofilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var cam = new Camera(width, height, 0.1f, 100f, PROJECTIONTYPE.Perspective);
                cam.Position = new Vector3(0, 0, 20);
                var engine = new Engine(width, height, cam);
                engine.enableCrossHair(new Vector4(1f, 1f, 1f, 0.5f));
                engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");

                //var result = HeightmapImporter.getOpenElevation(48.755238f, 9.146392f);
                /*HeightmapImporter.getOpenStreetXMLBBox(new Vector2(48.756846f, 9.156012f),
                    new Vector2(48.8f, 9.2f), "Heslach",4);

                //            var heslachFrom = new Vector2(48.755238f, 9.146392f);
                //            var heslachTo = new Vector2(48.767457f, 9.167524f);


                //            var heslachHeight = HeightmapImporter.GetOpenElevationData(heslachFrom, heslachTo, new Vector2(10, 10));
                Mapgenerator mapgen = new Mapgenerator();
                //            var vol = mapgen.GenerateMapFromHeightData(heslachHeight, new Vector2(10, 10), 20);
                //            engine.SetWorld(vol);
                /*var hgtdata = new HgtReader().GetElevationData("C:\\Users\\Steven\\Documents\\Eigene Dokumente\\Studium\\Softwaretechnik\\Thesis\\Procedural Generation of Content\\N48E009.hgt");
                var vol = mapgen.GenerateMapFromHeightData(hgtdata);
                vol.Scales.X = 3;
                vol.Scales.Z = 3;
                engine.SetWorld(vol);*/
                BigColorVolume colorvol = new BigColorVolume(32, 32, 32, 16);
                var color1 = new Vector4(10, 90, 255, 100);
            /*for (int z = 0; z < 32; z++)
            {
                for (int y = 0; y < 32; y++)
                {
                    for (int x = 0; x < 32; x++)
                    {
                        colorvol.SetVoxel(x, y, z, color1);
                    }
                }
            }*/
            colorvol.SetVoxel(0, 0, 0, color1);
            colorvol.SetVoxel(16, 0, 0, color1);
            colorvol.SetVoxel(0, 16, 0, color1);
            colorvol.SetVoxel(0, 0, 16, color1);
            engine.AddModel(colorvol);
                engine.LoadModelFromFile(Path.Combine(userprofilePath, "3D Objects\\chr_rain.vox"));
                var model = engine.GetModel("chr_rain").First();


                model.Scales = new Vector3(0.05f);
                model.Position.Y += 30;
                model.Position.X += 30;

                //engine.UpdateFrame += (sender, eventArgs) => { model2.rotateX(0.01f); model2.MoveForward(0.08f); };
                engine.Run(60.0);
            }


        }
}
