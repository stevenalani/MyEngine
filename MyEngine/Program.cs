using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            cam.Position = new Vector3(0, 0, -50);
            Engine engine = new Engine(1400,900,cam);
            HeightmapImporter imp = new HeightmapImporter();
            
            engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");
            engine.enableCrossHair(new Vector4(0f,1f,1f,0.5f));
            engine.LoadModelFromFile("C:\\Users\\Chantal\\3D Objects\\chr_rain.vox");
            
            //engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\AxisMat.vox");
            //engine.LoadModelFromFile("C:\\Users\\Steven\\3D Objects\\blocksalongx.vox");
           var testgetmodel = engine.GetModel("chr_rain");
            var model = testgetmodel.First();
            model.rotateX(15f);
            model.MoveForward(10f);
            
            engine.Run(60.0);
        }
    }

    internal class HeightmapImporter
    {
        public int id { get; set; }

        public HeightmapImporter()
        {
            string url = "https://api.openstreetmap.org";
            WebClient wc = new WebClient();
            wc.BaseAddress = url;
            var test = wc.DownloadData("/api/0.6/map?bbox=11.54,48.14,11.543,48.145");
            string s = test.ToString();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.openstreetmap.org/api/0.6/map?bbox=11.54,48.14,11.543,48.145");
          
            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            var stream = response.GetResponseStream();
            string result = "";

            while (result + stream.EndRead())
            {

            }
            WebHeaderCollection WHCol = response.Headers;

        }
    }
}