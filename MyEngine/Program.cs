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
            //Cube model = new Cube();
            //engine.AddModel(model);
            //Cube model2 = new Cube();
            //model2.MoveToVector(new Vector3(5f,5f,5f));
            //for (var i = 0; i<model2.Vertices.Length;i++)
            //{
            //    model2.Vertices[i].color = new Vector4(0,255f,0,1f);
            //}
            //engine.AddModel(model2);
            //engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\AxisMat.vox");
            engine.Run(60.0);
        }
    }
}