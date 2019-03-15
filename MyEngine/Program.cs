using System;
using System.CodeDom;
using System.Collections.Generic;
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
            cam.Position = new Vector3(0,0,20);
            Engine engine = new Engine(width, height, cam);
            engine.enableCrossHair(new Vector4(1f,1f,1f,0.5f));
            engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");
            
            //var result = HeightmapImporter.getOpenElevation(48.755238f, 9.146392f);
            /*HeightmapImporter.getOpenStreetXMLBBox(new Vector2(48.756846f, 9.156012f),
                new Vector2(48.8f, 9.2f), "Heslach",4);*/

            Vector2 heslachFrom = new Vector2(48.755238f,  9.146392f);
            Vector2 heslachTo = new Vector2(48.767457f, 9.167524f);

            var heslachHeight = HeightmapImporter.GetOpenElevationData(heslachFrom,heslachTo,new Vector2(10, 10));
            Volume vol = GenerateMapFromHeightData(heslachHeight, new Vector2(10, 10));
            engine.AddModel(vol);
            


            engine.LoadModelFromFile(Path.Combine(userprofilePath, "3D Objects\\chr_rain.vox"));
            var model = engine.GetModel("chr_rain").First();
            model.MoveToVector(new Vector3(10,0,0));
            //var model2 = new RandomDiscoVolume(6, 6, 6);
            //model2.name = "Random";
            //model2.Position = Vector3.UnitX; 
            //engine.AddModel(model2);
            //engine.UpdateFrame += (sender, eventArgs) => { model2.rotateX(0.01f); model2.MoveForward(0.08f); };
            engine.Run(60.0);

        }

        private static Volume GenerateMapFromHeightData(List<HeightmapImporter.LocationResult> heslachHeight, Vector2 itemsperaxis, float heightcolorscale = 1f)
        {
            var volsize = Math.Sqrt(heslachHeight.Count);
            var deltaheight = heslachHeight.Max(location => location.elevation) - heslachHeight.Min(location => location.elevation);
            Volume vol = new Volume((int)itemsperaxis.X, deltaheight,(int)itemsperaxis.Y);
            var queue = new Queue<HeightmapImporter.LocationResult>(heslachHeight);

            while (heightcolorscale * deltaheight * 0.1f < 1) heightcolorscale *= 1.1f;
            for(int x = 0; x < itemsperaxis.X; x++)
            for(int z = 0; z < itemsperaxis.Y; z++)
            {
                var location = queue.Dequeue();
                for (int y = location.elevation; y > 0; y--)
                {
                    var color = new Vector4();
                    if(y <= deltaheight*0.1 * heightcolorscale)
                        color = new Vector4(0, 51, 200,127);
                    else if(y > deltaheight * 0.1 && y <= deltaheight * 0.2 * heightcolorscale)
                        color = new Vector4(167, 126, 79,255);
                    else if(y > deltaheight * 0.2 && y <= deltaheight * 0.4 * heightcolorscale)
                        color = new Vector4(0, 136, 0, 255);
                    else if(y > deltaheight * 0.4 && y <= deltaheight * 0.7 * heightcolorscale)
                        color = new Vector4(0, 136, 0, 255);
                    else if (y > deltaheight * 0.7 && y <= deltaheight * 0.9 * heightcolorscale)
                        color = new Vector4(190, 190, 190, 255);
                    else
                        color = new Vector4(200, 200, 200, 255);
                    
                    vol.SetVoxel(x,y,z,color);
                }
            }
            return vol;
        }
    }
}