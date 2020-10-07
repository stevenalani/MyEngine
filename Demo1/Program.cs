using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using MyEngine;
using MyEngine.HgtImporter;
using MyEngine.Models;
using MyEngine.Models.Voxel;
using OpenTK;
using PlantGeneration;

namespace Demo1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var width = 800;
            var height = 600;
            var userprofilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var cam = new Camera(width, height, 0.1f, 1000f);
            cam.Position = new Vector3(0, 0, -20);
            var engine = new Engine(width, height, cam);
            
            engine.enableCrossHair(new Vector4(1f, 1f, 1f, 0.5f));
            engine.AddShader("Shaders\\DefaultVoxelShader.vs", "Shaders\\DefaultVoxelShader.fs");

            /*ColorVolume volume = new ColorVolume(3,3,3);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        volume.SetVoxel(i,j,k, new Vector4(120,120,90,255));
                    }
                }
            }
            engine.AddModel(volume);
*/
            var light = new LightSource(position: new Vector3(180, 400, 180), color: new Vector3(255, 255, 250));
            engine.AddLight(light);
            
            Mapgenerator mapgen = new Mapgenerator();
            var hgtdata = new HgtReader().GetElevationData("C:\\Users\\Steven\\Documents\\Eigene Dokumente\\Studium\\Softwaretechnik\\Thesis\\Procedural Generation of Content\\N48E009.hgt");
            var vol = mapgen.GenerateMapFromHeightData(hgtdata);
            vol.Scales.X = 3;
            vol.Scales.Z = 3;
            vol.CubeScale = 10f;
            
            engine.AddModel(vol);
            var leaf = new Leaf();
            /*var zero = new ColorVolume(1,1,1);
            zero.SetVoxel(0,0,0,new Vector4(255,0,0,255));
            engine.AddModel(zero);
            var zero1 = new ColorVolume(1, 1, 1);
            zero1.SetVoxel(0, 0, 0, new Vector4(0, 255, 0, 255));
            zero1.Position.X = 10;
            engine.AddModel(zero1);*/
            //engine.UpdateFrame += (sender, eventArgs) => { zero1.rotateX(0.01f); zero1.MoveForward(0.08f); };
            
            /*
            var colorvol = new BigColorVolume(32, 32, 32);
            
            var color1 = new Vector4(10, 90, 255, 100);
            var color2 = new Vector4(10, 255, 90, 100);
            var color3 = new Vector4(255, 90, 10, 100);
            var color4 = new Vector4(90, 255, 10, 100);

            colorvol.SetVoxel(0, 0, 0, color1);
            colorvol.SetVoxel(15, 0, 0, color1);
            colorvol.SetVoxel(0, 0, 15, color1);
            colorvol.SetVoxel(15, 0, 15, color1);
            colorvol.SetVoxel(0, 15, 0, color1);
            colorvol.SetVoxel(15, 15, 0, color1);
            colorvol.SetVoxel(0, 15, 15, color1);
            colorvol.SetVoxel(15, 15, 15, color1);

            colorvol.SetVoxel(16, 0, 0, color2);
            colorvol.SetVoxel(31, 0, 0, color2);
            colorvol.SetVoxel(16, 0, 15, color2);
            colorvol.SetVoxel(31, 0, 15, color2);
            colorvol.SetVoxel(16, 15, 0, color2);
            colorvol.SetVoxel(31, 15, 0, color2);
            colorvol.SetVoxel(16, 15, 15, color2);
            colorvol.SetVoxel(31, 15, 15, color2);

            colorvol.SetVoxel(0, 16, 0, color3);
            colorvol.SetVoxel(15, 16, 0, color3);
            colorvol.SetVoxel(0, 16, 15, color3);
            colorvol.SetVoxel(15, 16, 15, color3);
            colorvol.SetVoxel(0, 31, 0, color3);
            colorvol.SetVoxel(15, 31, 0, color3);
            colorvol.SetVoxel(0, 31, 15, color3);
            colorvol.SetVoxel(15, 31, 15, color3);

            colorvol.SetVoxel(16, 16, 0, color4);
            colorvol.SetVoxel(31, 16, 0, color4);
            colorvol.SetVoxel(16, 16, 15, color4);
            colorvol.SetVoxel(31, 16, 15, color4);
            colorvol.SetVoxel(16, 31, 0, color4);
            colorvol.SetVoxel(31, 31, 0, color4);
            colorvol.SetVoxel(16, 31, 15, color4);
            colorvol.SetVoxel(31, 31, 15, color4);

            colorvol.SetVoxel(0, 0, 16, color4);
            colorvol.SetVoxel(15, 0, 16, color4);
            colorvol.SetVoxel(0, 0, 31, color4);
            colorvol.SetVoxel(15, 0, 31, color4);
            colorvol.SetVoxel(0, 15, 16, color4);
            colorvol.SetVoxel(15, 15, 16, color4);
            colorvol.SetVoxel(0, 15, 31, color4);
            colorvol.SetVoxel(15, 15, 31, color4);

            colorvol.SetVoxel(16, 0, 16, color3);
            colorvol.SetVoxel(31, 0, 16, color3);
            colorvol.SetVoxel(16, 0, 31, color3);
            colorvol.SetVoxel(31, 0, 31, color3);
            colorvol.SetVoxel(16, 15, 16, color3);
            colorvol.SetVoxel(31, 15, 16, color3);
            colorvol.SetVoxel(16, 15, 31, color3);
            colorvol.SetVoxel(31, 15, 31, color3);

            colorvol.SetVoxel(0, 16, 16, color2);
            colorvol.SetVoxel(15, 16, 16, color2);
            colorvol.SetVoxel(0, 16, 31, color2);
            colorvol.SetVoxel(15, 16, 31, color2);
            colorvol.SetVoxel(0, 31, 16, color2);
            colorvol.SetVoxel(15, 31, 16, color2);
            colorvol.SetVoxel(0, 31, 31, color2);
            colorvol.SetVoxel(15, 31, 31, color2);

            colorvol.SetVoxel(16, 16, 16, color1);
            colorvol.SetVoxel(31, 16, 16, color1);
            colorvol.SetVoxel(16, 16, 31, color1);
            colorvol.SetVoxel(31, 16, 31, color1);
            colorvol.SetVoxel(16, 31, 16, color1);
            colorvol.SetVoxel(31, 31, 16, color1);
            colorvol.SetVoxel(16, 31, 31, color1);
            colorvol.SetVoxel(31, 31, 31, color1);

            engine.AddModel(colorvol);
            */
            /*engine.LoadModelFromFile(Path.Combine(userprofilePath, "3D Objects\\chr_rain.vox"));
            var model = engine.GetModel("chr_rain").First();
            */
            engine.Run(60.0);
        }
    }
}