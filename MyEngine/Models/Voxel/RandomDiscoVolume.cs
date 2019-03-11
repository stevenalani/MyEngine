using System;
using OpenTK;
using OpenTK.Graphics.ES30;

namespace MyEngine.Assets.Models.Voxel
{
    internal class RandomDiscoVolume : Volume
    {
        public RandomDiscoVolume(Vector3 dimensions) : base(dimensions)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < dimensions.X; i++)
            for (var j = 0; j < dimensions.Y; j++)
            for (var k = 0; k < dimensions.Z; k++)
                SetVoxel(new Vector3(i, j, k),
                    new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));
            Update();
        }

        public RandomDiscoVolume(int witdh, int height, int depth) : base(witdh, height, depth)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            Update();
        }

        

        public void Update()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            /*for (var i = 0; i < dimensions.X; i++)
            for (var j = 0; j < dimensions.Y; j++)
            for (var k = 0; k < dimensions.Z; k++)*/
            var x = rand.Next(0, (int) (dimensions.X-1));
            var y = rand.Next(0, (int) (dimensions.Y-1));
            var z = rand.Next(0, (int) (dimensions.Z-1));
            ClearVoxel(x,y,z);
            x = rand.Next(0, (int) (dimensions.X-1));
            y = rand.Next(0, (int) (dimensions.Y-1));
            z = rand.Next(0, (int) (dimensions.Z-1));
            ClearVoxel(x: x, y: y, z: z);
            x = rand.Next(0, (int)(dimensions.X - 1));
            y = rand.Next(0, (int)(dimensions.Y - 1));
            z = rand.Next(0, (int)(dimensions.Z - 1));
            SetVoxel(new Vector3(x, y, z),
                    new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));
            foreach (var voxelInformation in VolumeData)
            {
                VolumeData[(int) voxelInformation.Posindices.X,(int) voxelInformation.Posindices.Y,(int) voxelInformation.Posindices.Z].checkedin = false;
            }
            Init();
        }


    }
}