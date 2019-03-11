using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Text;
using MyEngine.Assets.Models;
using MyEngine.Assets.Models.Voxel;
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
            
            //HeightmapImporter hi = new HeightmapImporter();
            engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");
            engine.enableCrossHair(new Vector4(1f,1f,1f,0.5f));
            //engine.LoadModelFromFile(Path.Combine(userprofilePath,"3D Objects\\blocksalongx.vox"));

            var model = new PositionColorModelCustom(6, 6, 6);
            model.name = "RubicsCube";
            engine.AddModel(model);
            //engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\AxisMat.vox");
            //engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\blocksalongx.vox");
            //var testgetmodel = engine.GetModel("fuckgreedy");
            //var model = testgetmodel.First();
            //model.rotateX(15f);
            //model.MoveForward(10f);
            //BoundingBox boundingBox = new BoundingBox(model);
            //engine.AddModel(boundingBox);
            engine.Run(60.0);

        }
    }
}