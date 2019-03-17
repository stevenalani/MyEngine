using System;
using System.Collections.Generic;
using System.IO;
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

            var heslachFrom = new Vector2(48.755238f, 9.146392f);
            var heslachTo = new Vector2(48.767457f, 9.167524f);

            /*var heslachHeight = new List<HeightmapImporter.LocationResult>();
            heslachHeight.Add(new HeightmapImporter.LocationResult {elevation = 0});
            heslachHeight.Add(new HeightmapImporter.LocationResult {elevation = 3});
            heslachHeight.Add(new HeightmapImporter.LocationResult {elevation = 2});
            heslachHeight.Add(new HeightmapImporter.LocationResult {elevation = 2});*/
            Mapgenerator mapgen = new Mapgenerator();
            var heslachHeight = HeightmapImporter.GetOpenElevationData(heslachFrom, heslachTo, new Vector2(20, 20));
            var vol = mapgen.GenerateMapFromHeightData(heslachHeight, new Vector2(20, 20), 20);
            //var vol = Mapgenerator.generateLineFillLower(5, 10, 10, false,Vector3.UnitY);
            vol.Scales = new Vector3(0.1f);
            engine.AddModel(vol);


            //engine.LoadModelFromFile(Path.Combine(userprofilePath, "3D Objects\\chr_rain.vox"));
            //engine.LoadModelFromFile(Path.Combine(userprofilePath, "3D Objects\\untitled.vox"));
            //var model = engine.GetModel("chr_rain").First();
            //model.MoveToVector(new Vector3(10,0,0));
            //var model2 = new RandomDiscoVolume(6, 6, 6);
            //model2.name = "Random";
            //model2.Position = Vector3.UnitX; 
            //engine.AddModel(model2);
            //engine.UpdateFrame += (sender, eventArgs) => { model2.rotateX(0.01f); model2.MoveForward(0.08f); };
            engine.Run(60.0);
        }

        
    }
}