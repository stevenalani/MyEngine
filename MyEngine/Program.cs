using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Text;
using MyEngine.Assets.Models;
using OpenTK;

namespace MyEngine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var userprofilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Camera cam = new Camera(1400, 900);
            cam.Position = new Vector3(0,0,-25);
            Engine engine = new Engine(1400,900,cam);
            //HeightmapImporter hi = new HeightmapImporter();
            engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");
            engine.enableCrossHair(new Vector4(1f,1f,1f,0.5f));
            engine.LoadModelFromFile(Path.Combine(userprofilePath,"3D Objects\\chr_rain.vox"));
            
            //engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\AxisMat.vox");
            //engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\blocksalongx.vox");
            var testgetmodel = engine.GetModel("chr_rain");
            var model = testgetmodel.First();
            model.rotateX(15f);
            model.MoveForward(10f);
            BoundingBox boundingBox = new BoundingBox(model);
            engine.AddModel(boundingBox);
            engine.Run(60.0);

        }
    }
}