using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Text;
using MyEngine.Assets.Models;
using MyEngine.Assets.Models.Voxel;
using MyEngine.Models;
using MyEngine.Models.Voxel;
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
            Camera cam = new Camera(width, height, 0.1f,100f,PROJECTIONTYPE.Perspective);
            cam.Position = new Vector3(0,0,10);
            Engine engine = new Engine(width, height, cam);
            
            engine.AddShader("Shaders\\DefaultVoxelShader21.vs", "Shaders\\DefaultVoxelShader21.fs");
            engine.enableCrossHair(new Vector4(1f,1f,1f,0.5f));

            /*HeightmapImporter.getOpenStreetXMLPath(new Vector2(48.756846f, 9.156012f),
                new Vector2(48.8f, 9.2f), "Heslach",4);*/
            engine.LoadModelFromFile(Path.Combine(userprofilePath, "3D Objects\\chr_rain.vox"));
            var model = engine.GetModel("chr_rain").First();

            var model2 = new RandomDiscoVolume(6, 6, 6);
            model2.name = "Random";
            engine.AddModel(model2);
            /*engine.MouseUp += (sender, eventArgs) =>
            {
                model2.ClearVolume();
            };*/
            //model.name = "RubicsCube";
            //engine.AddModel(model);
            //engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\AxisMat.vox");
            //engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\blocksalongx.vox");
           
            //var model = testgetmodel.First();
            model.rotateX(15f);
            model.rotateZ(20f);
            model.MoveToVector(new Vector3(15f,-2f,10f));
            //BoundingBox boundingBox = new BoundingBox(model);
            //engine.AddModel(boundingBox);
            engine.Run(60.0);

        }
    }
}