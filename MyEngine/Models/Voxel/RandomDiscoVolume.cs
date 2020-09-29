using MyEngine.ShaderImporter;
using OpenTK;
using System;

namespace MyEngine.Models.Voxel
{
    internal class RandomDiscoVolume : ColorVolume
    {
        private int drawings;

        public RandomDiscoVolume(Vector3 Dimensions) : base((int)Dimensions.X, (int)Dimensions.Y, (int)Dimensions.Z)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < Dimensions.X; i++)
                for (var j = 0; j < Dimensions.Y; j++)
                    for (var k = 0; k < Dimensions.Z; k++)
                        SetVoxel(new Vector3(i, j, k),
                            new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 200)));
            SetVoxel(new Vector3(Dimensions.X - 1, 0, 0),
                new Vector4(255, 0, 0, 255));
            SetVoxel(new Vector3(0, Dimensions.Y - 1, 0),
                new Vector4(0, 255, 0, 255));
            SetVoxel(new Vector3(0, 0, Dimensions.Z - 1),
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
            /*for (var i = 0; i < Dimensions.Value; i++)
            for (var j = 0; j < Dimensions.Y; j++)
            for (var k = 0; k < Dimensions.Z; k++)*/
            var x = rand.Next(0, (int)(Dimensions.X - 1));
            var y = rand.Next(0, (int)(Dimensions.Y - 1));
            var z = rand.Next(0, (int)(Dimensions.Z - 1));
            ClearVoxel(x, y, z);
            x = rand.Next(0, (int)(Dimensions.X - 1));
            y = rand.Next(0, (int)(Dimensions.Y - 1));
            z = rand.Next(0, (int)(Dimensions.Z - 1));
            ClearVoxel(x, y, z);
            x = rand.Next(0, (int)(Dimensions.X - 1));
            y = rand.Next(0, (int)(Dimensions.Y - 1));
            z = rand.Next(0, (int)(Dimensions.Z - 1));
            SetVoxel(new Vector3(x, y, z),
                new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 200)));
            for (var zz = 0; zz < Dimensions.Z; zz++)
            {
                for (var yy = 0; yy < Dimensions.Y; yy++)
                {
                    for (var xx = 0; xx < Dimensions.X; xx++)
                    {
                        CheckedInVoxels[xx, yy, zz] = false;
                    }
                }
            }

            SetVoxel(new Vector3(Dimensions.X - 1, 0, 0),
                new Vector4(255, 0, 0, 255));
            SetVoxel(new Vector3(0, Dimensions.Y - 1, 0),
                new Vector4(0, 255, 0, 255));
            SetVoxel(new Vector3(0, 0, Dimensions.Z - 1),
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