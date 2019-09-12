using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyEngine.DataStructures;
using MyEngine;
using MyEngine.DataStructures.Vectors;
using MyEngine.Models;
using Vector3 = MyEngine.DataStructures.Vectors.Vector3;
using Vector4 = MyEngine.DataStructures.Vectors.Vector4;


namespace DemoOne1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            
            var width = 1400;
            var height = 900;
            var userprofilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var cam = new Camera(width, height, 0.1f, 200f, PROJECTIONTYPE.Perspective);
            cam.SetPosition(new Vector3(0, 20, 110));
            var engine = new Engine(width, height, cam);
            engine.enableCrossHair(new Vector4(1f, 1f, 1f, 0.5f));
            engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");
            /*
                        //var result = HeightmapImporter.getOpenElevation(48.755238f, 9.146392f);
                       HeightmapImporter.getOpenStreetXMLBBox(new Vector2(48.756846f, 9.156012f),
                            new Vector2(48.8f, 9.2f), "Heslach",4);

                        var heslachFrom = new Vector2(48.755238f, 9.146392f);
                        var heslachTo = new Vector2(48.767457f, 9.167524f);


                        var heslachHeight = HeightmapImporter.GetOpenElevationData(heslachFrom, heslachTo, new Vector2(10, 10));
                        Mapgenerator mapgen = new Mapgenerator();
                        var vol = mapgen.GenerateMapFromHeightData(heslachHeight, new Vector2(10, 10), 20);
                        engine.SetWorld(vol);

                         engine.LoadModelFromFile(Path.Combine(userprofilePath, "3D Objects\\chr_rain.vox"));

                        engine.LoadModelFromFile(Path.Combine(userprofilePath, "3D Objects\\chr_gumi.vox"));

                        var model = engine.GetModel("chr_rain").First();
                        model.Scales = new Vector3(0.3f);
                        model.Position.Y = 30;
                        model.mass = 0.1f;
                        */
            engine.LoadModelFromFile(Path.Combine(userprofilePath, "3D Objects\\chr_fox.vox"));
            var model1 = engine.GetModel("chr_fox").First();   
            model1.Scales = new OpenTK.Vector3(0.3f);
            model1.Position.Y = 30;
            model1.mass = 0.1f;
            model1.Position.X = 20;
            model1.CalculatePhysics = false;
            /*var model2 = engine.GetModel("chr_gumi").First();
            model2.Scales = new Vector3(0.3f);
            model2.Position.Y = 30;
            model2.mass = 0.1f;
            model2.Position.X = 40;
            Random random = RandomProvider.GetThreadRandom();
            for (int i = 0; i < 10; i++)
            {
                var posx = random.Next(-100,100);
                var posy = random.Next(30, 100);
                var posz = random.Next(-100, 100);
                Cube cube = new Cube(new Vector4(255,0,0,255));
                cube.name = "cube" + i;
                cube.Position = new Vector3(posx,posy,posz);
                cube.mass = 0.1f;
                engine.AddModel(cube);
            }*/
            
            
            Line line = new Line(new Vector3(1f,2f,5f), new Vector3(1f,3f,1f), 0.01f);
            engine.AddModel(line);
            
            BoundingBox bb = new BoundingBox(line);
            engine.AddModel(bb);
            
            engine.Run(90);
        }

        
    }

    internal class Scene
    {
        private List<Model> models;
        
    }
}
 