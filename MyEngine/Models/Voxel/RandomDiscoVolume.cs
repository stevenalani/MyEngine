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

        public RandomDiscoVolume(Vector3 size) : base(size)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < size.X; i++)
            for (var j = 0; j < size.Y; j++)
            for (var k = 0; k < size.Z; k++)
                SetVoxel(new Vector3(i, j, k),
                    new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 200)));
            SetVoxel(new Vector3(size.X-1, 0, 0),
                new Vector4(255, 0, 0, 255));
            SetVoxel(new Vector3(0, size.Y - 1, 0),
                new Vector4(0, 255, 0, 255));
            SetVoxel(new Vector3(0, 0, size.Z - 1),
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
            /*for (var i = 0; i < size.X; i++)
            for (var j = 0; j < size.Y; j++)
            for (var k = 0; k < size.Z; k++)*/
            var x = rand.Next(0, (int) (size.X - 1));
            var y = rand.Next(0, (int) (size.Y - 1));
            var z = rand.Next(0, (int) (size.Z - 1));
            ClearVoxel(x, y, z);
            x = rand.Next(0, (int) (size.X - 1));
            y = rand.Next(0, (int) (size.Y - 1));
            z = rand.Next(0, (int) (size.Z - 1));
            ClearVoxel(x, y, z);
            x = rand.Next(0, (int) (size.X - 1));
            y = rand.Next(0, (int) (size.Y - 1));
            z = rand.Next(0, (int) (size.Z - 1));
            SetVoxel(new Vector3(x, y, z),
                new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 200)));
            for (var zz = 0; zz < size.Z; zz++)
            for (var yy = 0; yy < size.Y; yy++)
            for (var xx = 0; xx < size.X; xx++)   
                VolumeData[xx, yy, zz].checkedin = false;

            SetVoxel(new Vector3(size.X - 1, 0, 0),
                new Vector4(255, 0, 0, 255));
            SetVoxel(new Vector3(0, size.Y - 1, 0),
                new Vector4(0, 255, 0, 255));
            SetVoxel(new Vector3(0, 0, size.Z - 1),
                new Vector4(0, 0, 255, 255));

            IsInitialized = false;
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