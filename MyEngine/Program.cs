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
            Camera cam = new Camera(1400, 900);
            Engine engine = new Engine(1400,900,cam);
            
            engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");
            engine.enableCrossHair(new Vector4(1f,1f,1f,0.5f));
            engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\chr_rain.vox");
            
            engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\AxisMat.vox");
            engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\blocksalongx.vox");
            var testgetmodel = engine.GetModel("chr_rain");
            var model = testgetmodel.First();
            //model.rotateX(15f);
            model.MoveForward(10f);
            
            engine.Run(60.0);
        }
    }
}