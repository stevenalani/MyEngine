using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyEngine.Helpers;
using MyEngine.HgtImporter;
using OpenTK;

namespace MyEngine
{
    internal class Program
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
                new Vector2(48.8f, 9.2f), "Heslach",4);*/

//            var heslachFrom = new Vector2(48.755238f, 9.146392f);
//            var heslachTo = new Vector2(48.767457f, 9.167524f);

           
//            var heslachHeight = HeightmapImporter.GetOpenElevationData(heslachFrom, heslachTo, new Vector2(10, 10));
            Mapgenerator mapgen = new Mapgenerator();
//            var vol = mapgen.GenerateMapFromHeightData(heslachHeight, new Vector2(10, 10), 20);
//            engine.SetWorld(vol);
             /*           var hgtdata = new HgtReader().GetElevationData("C:\\Users\\Steven\\Documents\\Eigene Dokumente\\Studium\\Softwaretechnik\\Thesis\\Procedural Generation of Content\\N48E009.hgt");
            var vol = mapgen.GenerateMapFromHeightData(hgtdata);
            vol.Position = new Vector3();
            engine.AddModel(vol);*/

            var color1 = new Vector4(255, 0, 0, 255);
            var color2 = new Vector4(0, 255, 0, 255);
            var color3 = new Vector4(0, 0, 255, 255);

            var volume = new ColorVolume(10, 10, 10);
            volume.AddColor(color1);
            volume.AddColor(color2);
            volume.AddColor(color3);

            volume.SetVoxel(0, 0, 0, 1);
            volume.SetVoxel(1, 0, 0, 1);
            volume.SetVoxel(2, 0, 0, 1);
            volume.SetVoxel(3, 0, 0, 1);
            volume.SetVoxel(0, 1, 0, 1);
            volume.SetVoxel(1, 1, 0, 1);
            volume.SetVoxel(2, 1, 0, 1);
            volume.SetVoxel(3, 1, 0, 1);
            volume.SetVoxel(0, 1, 1, 1);
            volume.SetVoxel(1, 1, 1, 1);
            volume.SetVoxel(2, 1, 1, 1);
            volume.SetVoxel(3, 1, 1, 1);
            engine.AddModel(volume);

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