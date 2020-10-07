using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEngine;
using MyEngine.Models.Voxel;
using OpenTK;

namespace DemoTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var width = 800;
            var height = 600;
            var userprofilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var cam = new Camera(width, height, 0.1f, 1000f);
            cam.Position = new Vector3(0, 0, -20);
            var engine = new Engine(width, height, cam);

            engine.enableCrossHair(new Vector4(1f, 1f, 1f, 0.5f));
            engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");

            var light = new LightSource(position: new Vector3(180, 400, 180), color: new Vector3(255, 255, 250));
            engine.AddLight(light);            
            
            Tree tree = new Tree(100, 100, 100);
            engine.AddModel(tree);
            Leaf leaf = new Leaf();
            engine.AddModel(leaf);
            engine.UpdateFrame += (sender, eventArgs) => { leaf.rotateZ(0.01f);};
            engine.Run(60.0);
        }
    }
}
