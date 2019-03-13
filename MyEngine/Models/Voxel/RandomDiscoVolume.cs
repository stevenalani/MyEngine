using System;
using MyEngine.DataStructures;
using MyEngine.Models.Voxel;
using MyEngine.ShaderImporter;
using OpenTK;

namespace MyEngine.Assets.Models.Voxel
{
    internal class RandomDiscoVolume : Volume
    {
        private int drawings;

        public RandomDiscoVolume(Vector3 dimensions) : base(dimensions)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < dimensions.X; i++)
            for (var j = 0; j < dimensions.Y; j++)
            for (var k = 0; k < dimensions.Z; k++)
                SetVoxel(new Vector3(i, j, k),
                    new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 200)));
            SetVoxel(new Vector3(dimensions.X-1, 0, 0),
                new Vector4(255, 0, 0, 255));
            SetVoxel(new Vector3(0, dimensions.Y - 1, 0),
                new Vector4(0, 255, 0, 255));
            SetVoxel(new Vector3(0, 0, dimensions.Z - 1),
                new Vector4(0, 0, 255, 255));
            Update();
        }

        public RandomDiscoVolume(int witdh, int height, int depth) : base(witdh, height, depth)
        {
            Update();
        }


        public void Update()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            /*for (var i = 0; i < dimensions.X; i++)
            for (var j = 0; j < dimensions.Y; j++)
            for (var k = 0; k < dimensions.Z; k++)*/
            var x = rand.Next(0, (int) (dimensions.X - 1));
            var y = rand.Next(0, (int) (dimensions.Y - 1));
            var z = rand.Next(0, (int) (dimensions.Z - 1));
            ClearVoxel(x, y, z);
            x = rand.Next(0, (int) (dimensions.X - 1));
            y = rand.Next(0, (int) (dimensions.Y - 1));
            z = rand.Next(0, (int) (dimensions.Z - 1));
            ClearVoxel(x, y, z);
            x = rand.Next(0, (int) (dimensions.X - 1));
            y = rand.Next(0, (int) (dimensions.Y - 1));
            z = rand.Next(0, (int) (dimensions.Z - 1));
            SetVoxel(new Vector3(x, y, z),
                new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 200)));
            for (var zz = 0; zz < dimensions.Z; zz++)
            for (var yy = 0; yy < dimensions.Y; yy++)
            for (var xx = 0; xx < dimensions.X; xx++)   
                VolumeData[xx, yy, zz].checkedin = false;

            SetVoxel(new Vector3(dimensions.X - 1, 0, 0),
                new Vector4(255, 0, 0, 255));
            SetVoxel(new Vector3(0, dimensions.Y - 1, 0),
                new Vector4(0, 255, 0, 255));
            SetVoxel(new Vector3(0, 0, dimensions.Z - 1),
                new Vector4(0, 0, 255, 255));

            ComputeVerticesAndIndices();
        }

        public override void Draw(ShaderProgram shader)
        {
            base.Draw(shader);
            drawings++;
            if (drawings == 50)
            {
                Update();
                drawings = 0;
            }
        }
    }
}